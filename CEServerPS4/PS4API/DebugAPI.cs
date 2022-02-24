using CEServerPS4.EventHandler;
using CEServerPS4.EventHandler.Event;
using CEServerPS4.EventHandler.Request;
using libdebug;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;


namespace CEServerPS4.PS4API
{
    public static class DebugAPI
    {

        private static uint[] threadIds;

        private static Object currentDebugEvent;

        private static BlockingCollection<Object> debugEvents = new BlockingCollection<Object>();

        private static uint debugThread;

        public static uint getThd()
        {
            return debugThread;
        }

        private static HashSet<uint> threads = new HashSet<uint>();

        private static HashSet<ulong> adrresses = new HashSet<ulong>();
        private static Dictionary<ulong, int> breakpointsAddress = new Dictionary<ulong, int>();

        private static Dictionary<ulong, int> watchpointAddress = new Dictionary<ulong, int>();

        private static Dictionary<uint, ulong> watchThreaads = new Dictionary<uint, ulong>();
        private static Dictionary<ulong, uint> watchAddress = new Dictionary<ulong, uint>();

        private static Dictionary<uint, ulong> breakThreaads = new Dictionary<uint, ulong>();
        private static Dictionary<ulong, uint> breakAddress = new Dictionary<ulong, uint>();

        private static Stack<int> watchpoints = new Stack<int>();
        private static Stack<int> breakpoints = new Stack<int>();

        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct CONTEXT_REGS
        {
            public ulong r15;
            public ulong r14;
            public ulong r13;
            public ulong r12;
            public ulong rbp;
            public ulong rbx;
            public ulong r11;
            public ulong r10;
            public ulong r9;
            public ulong r8;
            public ulong rax;
            public ulong rcx;
            public ulong rdx;
            public ulong rsi;
            public ulong rdi;
            public ulong orig_rax;
            public ulong rip;
            public ulong cs;
            public ulong eflags;
            public ulong rsp;
            public ulong ss;
            public ulong fs_base;
            public ulong gs_base;
            public ulong ds;
            public ulong es;
            public ulong fs;
            public ulong gs;
        };


        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct CONTEXT
        {
            public CONTEXT_REGS regs;
        }

        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct CreateDebugEvent
        {

            public int debugevent;

            public ulong threadid;

        }

        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct DebugEvent
        {

            public int debugevent;

            public ulong threadid;

            public ulong address;

        }

        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct ProcessEvent
        {

            public int debugevent;

            public ulong threadid;

            public sbyte maxBreakpointCount;

            public sbyte maxWatchpointCount;

            public sbyte maxSharedBreakpoints;
        }


        public static void DebuggerInterruptCallback(
            uint lwpid,
            uint status,
            string tdname,
            regs regs,
            fpregs fpregs,
            dbregs dbregs)
        {

            //debugThreadId = lwpid;
            Console.WriteLine("status=:" + status);
            DebugEvent evet = new DebugEvent();
            evet.debugevent = 5;
            evet.threadid = lwpid;
           
            debugThread = lwpid;
            ulong daddress;
            bool isBp;
            if((dbregs.dr6 & 1) == 1)
            {
                daddress = dbregs.dr0;
                isBp = false;
                if (regs.r_rip == dbregs.dr0 && dbregs.dr7 == 0)
                {
                    isBp = true;
                }
            }else if(((dbregs.dr6 >>1)& 1) == 1)
            {
                daddress = dbregs.dr1;
                isBp = false;
            }
            else if (((dbregs.dr6 >> 2) & 1) == 1)
            {
                daddress = dbregs.dr2;
                isBp = false;
            }
            else if (((dbregs.dr6 >> 3) & 1) == 1)
            {
                daddress = dbregs.dr3;
                isBp = false;
            }
            else
            {
                if(!breakThreaads.TryGetValue(lwpid,out daddress)){
                    daddress = regs.r_rip;
                }
                isBp = true;
            }

            if (isBp)
            {
                breakThreaads[lwpid] = daddress;
                breakAddress[daddress] = lwpid;
            }
            else
            {
                watchThreaads[lwpid] = daddress;
                watchAddress[daddress] = lwpid;
            }
           // dbregs.dr6 = 0;
           // PS4DedugAPIWrapper.getps4().SetDebugRegisters(lwpid, dbregs);
            PS4DedugAPIWrapper.getps4().SingleStep();
            evet.address = daddress;

            Thread nThread = new Thread(addDebugEvent) { IsBackground = true };
            nThread.Start(evet);
            Console.WriteLine("address=:" + regs.r_rip);
            Console.WriteLine("thread=:" + lwpid);
            Console.WriteLine("breakaddress=:" + daddress);
        }

        private static void addDebugEvent(Object evet)
        {
            debugEvents.Add(evet);
        }

        private static void assignThreadRegisters(regs r, out CONTEXT_REGS regs)
        {
            regs = new CONTEXT_REGS();
            regs.r15 = r.r_r15;
            regs.r14 = r.r_r14;
            regs.r13 = r.r_r13;
            regs.r12 = r.r_r12;
            regs.r11 = r.r_r11;
            regs.r10 = r.r_r10;
            regs.r9 = r.r_r9;
            regs.r8 = r.r_r8;
            regs.rax = r.r_rax;
            regs.rbx = r.r_rbx;
            regs.rcx = r.r_rcx;
            regs.rbp = r.r_rbp;
            regs.rdi = r.r_rdi;
            regs.rdx = r.r_rdx;
            regs.rip = r.r_rip;
            regs.rsi = r.r_rsi;
            regs.rsp = r.r_rsp;
            regs.cs = r.r_cs;
            regs.ds = r.r_ds;
            regs.eflags = r.r_rflags;
            regs.es = r.r_es;
            regs.fs = r.r_fs;
            regs.gs = r.r_gs;
            regs.ss = r.r_ss;

        }



        public static IntPtr StartDebug(IntPtr hProcess)
        {

            IntPtr value = (IntPtr)PS4DedugAPIWrapper.attachDebugger(new PS4DBG.DebuggerInterruptCallback(DebuggerInterruptCallback));
            threadIds = PS4DedugAPIWrapper.GetThreadsList();
            ProcessEvent processEvent = new ProcessEvent();
            processEvent.debugevent = -2;
            processEvent.threadid = threadIds[0];
            processEvent.maxBreakpointCount = Convert.ToSByte(PS4DBG.MAX_BREAKPOINTS);
            processEvent.maxWatchpointCount = Convert.ToSByte(PS4DBG.MAX_WATCHPOINTS);


            processEvent.maxSharedBreakpoints = 4;
            debugThread = threadIds[0];

            addDebugEvent(processEvent);

            //CreateDebugEvent deEvent = new CreateDebugEvent();
            //deEvent.debugevent = -1;
            //deEvent.threadid = threadIds[0];

            //addDebugEvent(deEvent);

            //DebugEvent evet = new DebugEvent();
            //evet.debugevent = 5;
            //evet.threadid = threadIds[0];
            //evet.address = 12356;
            //addDebugEvent(evet);

            threads = new HashSet<uint>(threadIds);

            return value;
        }
        public static IntPtr StopDebug(IntPtr handle)
        {
            IntPtr value = (IntPtr)PS4DedugAPIWrapper.dettachDebugger();

            return value;
        }


        public static IntPtr SetBreakpoint(IntPtr handle, int tid, IntPtr debugreg, UInt64 address, int bptype, int bpsize)
        {
           // return SetWatchpointToPS4(handle, tid, debugreg, address, bptype, bpsize);
           // return SetBreakpointToPS4(handle, tid, debugreg, address, bptype, bpsize);
            if (isWatchPoint(bptype, bpsize))
            {
                return SetWatchpointToPS4(handle, tid, debugreg, address, bptype, bpsize);
            }
            else
            {
                return SetBreakpointToPS4(handle, tid, debugreg, address, bptype, bpsize);
            }
        }
            public static IntPtr SetBreakpointToPS4(IntPtr handle, int tid, IntPtr debugreg, UInt64 address, int bptype, int bpsize)
        { 

            int num = findBreakPointNumber();

            if(num == -2)
            {
                return (IntPtr)0;
            }

            if (breakpointsAddress.ContainsKey(address))
            {
                uint ti;
                if(!breakAddress.TryGetValue(address,out ti)){
                    Console.WriteLine("no tid present for the address " + address.ToString("X"));
                }
                return (IntPtr)1;
            }

            if (SetBreakpoint(num, debugreg, address, bptype, bpsize) != 0)
            {
                breakpointsAddress[address] = num;
                breakpoints.Push(num);
                return (IntPtr)1;
            }
            return (IntPtr)0;
        }

        public static IntPtr SetWatchpointToPS4(IntPtr handle, int tid, IntPtr debugreg, UInt64 address, int bptype, int bpsize)
        {

            int num = findWatchPointNumber();

            if (num == -2)
            {
                return (IntPtr)0;
            }

            if (watchpointAddress.ContainsKey(address))
            {
                uint ti;
                if (!watchAddress.TryGetValue(address, out ti))
                {
                    Console.WriteLine("no tid present for the address " + address.ToString("X"));
                }
                return (IntPtr)1;
            }

            if (SetWatchpoint(num, debugreg, address, bptype, bpsize) != 0)
            {
                watchpointAddress[address] = num;
                watchpoints.Push(num);
                return (IntPtr)1;
            }
            return (IntPtr)0;
        }

        private static int SetBreakpoint(int tid, IntPtr debugreg, UInt64 address, int bptype, int bpsize)
        {
            try
            {
                SetBreakPointEvent se = new SetBreakPointEvent();
                SetBreakPointRequest request = new SetBreakPointRequest
                {
                    Address = address,
                    Tid = tid
                };
                se.Data = request;
                DebugEventHandler.AddEvent(se);

                return (int)DebugEventHandler.ConsumeAsync(se.BufferBlock).Result;
            }
            catch (Exception)
            {
                Console.WriteLine("cant create break point for" + tid);
                return 0;
            }
        }

        private static int SetWatchpoint(int tid, IntPtr debugreg, UInt64 address, int bptype, int bpsize)
        {
            try
            {
                SetWatchPointEvent se = new SetWatchPointEvent();
                SetWatchPointRequest request = new SetWatchPointRequest
                {
                    Address = address,
                    Bpsize = bpsize,
                    Bptype = bptype,
                    Tid = tid
                };
                se.Data = request;
                DebugEventHandler.AddEvent(se);

                return (int)DebugEventHandler.ConsumeAsync(se.BufferBlock).Result;
            }
            catch (Exception)
            {
                Console.WriteLine("cant create watch point for" + tid);
                return 0;
            }
        }


        public static IntPtr RemoveBreakpoint(IntPtr handle, int tid, IntPtr debugreg,ulong address, int wasWatchpoint)
        {
           // return RemoveWatchpointFromPS4(handle, tid, debugreg, address, wasWatchpoint);
           // return RemoveBreakpointFromPS4(handle, tid, debugreg, wasWatchpoint);
            if (wasWatchpoint == 1)
            {
                return RemoveWatchpointFromPS4(handle, tid, debugreg, address, wasWatchpoint);
            }
            else
            {
                return RemoveBreakpointFromPS4(handle, tid, debugreg, address, wasWatchpoint);
            }
        }

            public static IntPtr RemoveWatchpointFromPS4(IntPtr handle, int tid, IntPtr debugreg, ulong address, int wasWatchpoint)
        {

            if (address > 0)
            {
                if (watchpoints.Count > 0)
                {
                    watchpoints.Pop();
                }
                if (watchpointAddress.Count > 0)
                {

                    if (watchpointAddress.TryGetValue(address, out int wp))
                    {

                        if (watchAddress.TryGetValue(address, out uint ti))
                        {
                            breakAddress.Remove(address);
                            watchThreaads.Remove(ti);
                        }
                        watchpointAddress.Remove(address);
                        return RemoveWatchPointEventFromPS4(wp);
                    }

                    return (IntPtr)1;
                }
            }
            if (tid == -1)
            {
                watchpointAddress.Clear();
                watchpoints.Clear();
                watchThreaads.Clear();
                watchAddress.Clear();
                return RemoveWatchPointEventFromPS4(tid);
            }
            else
            {
                if (watchpoints.Count > 0)
                {
                    watchpoints.Pop();
                }
               
                if (watchpointAddress.Count > 0)
                {
                    if (watchThreaads.TryGetValue(Convert.ToUInt32(tid), out ulong ad))
                    {
                        if (watchpointAddress.TryGetValue(Convert.ToUInt32(tid), out int wp))
                        {
                            watchThreaads.Remove(Convert.ToUInt32(tid));
                            watchAddress.Remove(ad);
                            watchpointAddress.Remove(ad);
                            return RemoveWatchPointEventFromPS4(wp);
                        }

                    }
                    return (IntPtr)1;
                }
                else
                {
                    return (IntPtr)1;
                }
            }
            
        }

        public static IntPtr RemoveWatchPointEventFromPS4(int tid)
        {
            RemoveWatchPointEvent se = new RemoveWatchPointEvent();
            RemoveWatchPointRequest request = new RemoveWatchPointRequest
            {

                Tid = tid
            };
            se.Data = request;
            DebugEventHandler.AddEvent(se);

            int obj = (int)DebugEventHandler.ConsumeAsync(se.BufferBlock).Result;
            return (IntPtr)obj;
        }


        public static IntPtr RemoveBreakpointFromPS4(IntPtr handle, int tid, IntPtr debugreg, ulong address, int wasWatchpoint)
        {
            if (address > 0)
            {
                if (breakpoints.Count > 0)
                {
                    breakpoints.Pop();
                }
                if (breakpointsAddress.Count > 0)
                {

                    if (breakpointsAddress.TryGetValue(address, out int bp))
                    {
                        
                        if (breakAddress.TryGetValue(address, out uint ti))
                        {
                            breakAddress.Remove(address);
                            breakThreaads.Remove(ti);
                        }
                        breakpointsAddress.Remove(address);
                        return RemoveBreakPointEventFromPS4(bp);
                    }
                    
                    return (IntPtr)1;
                }
            }
            
            if (tid == -1)
            {
                breakpointsAddress.Clear();
                breakpoints.Clear();
                breakAddress.Clear();
                breakThreaads.Clear();
                return RemoveBreakPointEventFromPS4(tid);
            }
            else
            {
                if (breakpoints.Count > 0)
                {
                    breakpoints.Pop();
                }
                if (breakpointsAddress.Count > 0)
                {
                    if (breakThreaads.TryGetValue(Convert.ToUInt32(tid), out ulong ad))
                    {
                        if (breakpointsAddress.TryGetValue(Convert.ToUInt32(tid), out int bp))
                        {
                            breakThreaads.Remove(Convert.ToUInt32(tid));
                            breakAddress.Remove(ad);
                            breakpointsAddress.Remove(ad);
                            return RemoveBreakPointEventFromPS4(bp);
                        }
                    }
                    return (IntPtr)1;
                }
                else
                {
                    return (IntPtr)1;
                }
            }

        }


        public static IntPtr RemoveBreakPointEventFromPS4(int tid)
        {
            RemoveBreakPointEvent se = new RemoveBreakPointEvent();
            RemoveBreakPointRequest request = new RemoveBreakPointRequest
            {

                Tid = tid
            };
            se.Data = request;
            DebugEventHandler.AddEvent(se);

            int obj = (int)DebugEventHandler.ConsumeAsync(se.BufferBlock).Result;
            return (IntPtr)obj;
        }


        public static IntPtr SuspendThread(IntPtr handle, uint tid)
        {
            SuspendThreadEvent se = new SuspendThreadEvent();
            SuspendThreadRequest request = new SuspendThreadRequest
            {

                Tid = tid
            };
            se.Data = request;
            DebugEventHandler.AddEvent(se);

            int obj = (int)DebugEventHandler.ConsumeAsync(se.BufferBlock).Result;
            return (IntPtr)obj;
        }
        public static IntPtr ResumeThread(IntPtr handle, uint tid)
        {
            ResumeThreadEvent se = new ResumeThreadEvent();
            ResumeThreadRequest request = new ResumeThreadRequest
            {

                Tid = tid
            };
            se.Data = request;
            DebugEventHandler.AddEvent(se);

            int obj = (int)DebugEventHandler.ConsumeAsync(se.BufferBlock).Result;
            return (IntPtr)obj;
        }

        public static IntPtr GetThreadContext(IntPtr handle, uint tid, out CONTEXT Context, int type)
        {
            try
            {
                if (!threads.Contains(tid))
                {
                    tid = debugThread;
                }
                ThreadContextEvent se = new ThreadContextEvent();
                ThreadContextRequest request = new ThreadContextRequest
                {

                    Tid = tid ,
                    Type = type
                };
                se.Data = request;
                DebugEventHandler.AddEvent(se);
                regs r = (regs)DebugEventHandler.ConsumeAsync(se.BufferBlock).Result;


                assignThreadRegisters(r, out CONTEXT_REGS regs);
                Context = new CONTEXT
                {
                    regs = regs
                };

                if (regs.rip == 0)
                {
                    return (IntPtr)0;
                }

                return (IntPtr)1;
            }
            catch (Exception)
            {
                Context = new CONTEXT();
                return (IntPtr)0;
            }

        }

        public static IntPtr setThreadContext(IntPtr handle, uint tid, CONTEXT Context, int type)
        {
            try
            {
                if (!threads.Contains(tid))
                {
                    tid = debugThread;
                }
                ThreadContextEvent se = new ThreadContextEvent();
                ThreadContextRequest request = new ThreadContextRequest
                {

                    Tid = tid,
                    Type = type
                };
                se.Data = request;
                DebugEventHandler.AddEvent(se);
                regs r = (regs)DebugEventHandler.ConsumeAsync(se.BufferBlock).Result;


                assignThreadRegisters(r, out CONTEXT_REGS regs);
                Context = new CONTEXT
                {
                    regs = regs
                };

                if (regs.rip == 0)
                {
                    return (IntPtr)0;
                }

                return (IntPtr)1;
            }
            catch (Exception)
            {
                Context = new CONTEXT();
                return (IntPtr)0;
            }

        }

        public static IntPtr WaitForDebugEvent(IntPtr handle, int timeout, out Object evet)
        {
            try
            {
                Object et;
                if(debugEvents.TryTake(out et, timeout))
                {
                    evet = et;
                    currentDebugEvent = evet;
                    return (IntPtr)1;
                }
                evet = new DebugEvent();
                return (IntPtr)0;
            }
            catch (Exception)
            {
                evet = new DebugEvent();
                return (IntPtr)0;
            }
        }

        public static IntPtr ContinueForDebugEvent(IntPtr handle, uint tid, uint continuemethod)
        {
            try
            {
               
                ContinueDebugEvent se = new ContinueDebugEvent();
                ContinueDebugEventRequest rq = new ContinueDebugEventRequest();
                rq.Tid = tid;
                if(continuemethod == 2)
                {
                    rq.singleStep = true;
                }
                else
                {
                    rq.singleStep = false;
                }
                se.Data = rq;      
                DebugEventHandler.AddEvent(se);

                int obj = (int)DebugEventHandler.ConsumeAsync(se.BufferBlock).Result;
                return (IntPtr)obj;
                
            }
            catch (Exception)
            {
                return (IntPtr)0;
            }
        }

        private static int findWatchPointNumber()
        {
            if(watchpoints.Count<=0)
            {
                return 0;
            }else if(watchpoints.Count > 0 && watchpoints.Count < PS4DBG.MAX_WATCHPOINTS)
            {
                return watchpoints.Peek()+1;
            }
            else
            {
                return -2;
            }
        }

        private static int findBreakPointNumber()
        {
            if (breakpoints.Count <= 0)
            {
                return 0;
            }
            else if (breakpoints.Count > 0 && breakpoints.Count < PS4DBG.MAX_BREAKPOINTS)
            {
                return breakpoints.Peek() + 1;
            }
            else
            {
                return -2;
            }
        }

        private static bool isWatchPoint(int btype,int bsize)
        {
            if(btype==0 && bsize == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
