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

        private static HashSet<uint> threads = new HashSet<uint>();

        private static HashSet<ulong> adrresses = new HashSet<ulong>();


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
            evet.address = regs.r_rip;
            debugThread = lwpid;


            Thread nThread = new Thread(addDebugEvent) { IsBackground = true };
            nThread.Start(evet);

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

            IntPtr value = (IntPtr)PS4APIWrapper.attachDebugger();
            threadIds = PS4APIWrapper.GetThreadsList();
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
            IntPtr value = (IntPtr)PS4APIWrapper.dettachDebugger();

            return value;
        }


        public static IntPtr SetBreakpoint(IntPtr handle, int tid, IntPtr debugreg, UInt64 address, int bptype, int bpsize)
        {
            if (adrresses.Contains(address))
            {
                return (IntPtr)1;
            }

            if (tid == -1)
            {

                for (int i = 0; i < PS4DBG.MAX_WATCHPOINTS; i++)
                {
                    if (SetBreakpoint(i, debugreg, address, bptype, bpsize) != 0)
                    {
                        adrresses.Add(address);
                        return (IntPtr)1;
                    }

                }
                return (IntPtr)0;
            }
            else
            {
                if (SetBreakpoint(tid, debugreg, address, bptype, bpsize) != 0)
                {
                    adrresses.Add(address);
                    return (IntPtr)tid;
                }
                return (IntPtr)0;
            }

        }

        private static int SetBreakpoint(int tid, IntPtr debugreg, UInt64 address, int bptype, int bpsize)
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

        public static IntPtr RemoveBreakpoint(IntPtr handle, int tid, IntPtr debugreg, int wasWatchpoint)
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

        public static IntPtr WaitForDebugEvent(IntPtr handle, uint timeout, out Object evet)
        {
            try
            {
                evet = debugEvents.Take();
                currentDebugEvent = evet;
                return (IntPtr)1;
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
               
                ProcessReumeEvent se = new ProcessReumeEvent();
                DebugEventHandler.AddEvent(se);

                int obj = (int)DebugEventHandler.ConsumeAsync(se.BufferBlock).Result;
                return (IntPtr)obj;
                
            }
            catch (Exception)
            {
                return (IntPtr)0;
            }
        }

    }
}
