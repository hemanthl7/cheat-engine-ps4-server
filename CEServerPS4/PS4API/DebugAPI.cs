using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using libdebug;

namespace CEServerPS4.PS4API
{
    public static class DebugAPI
    {
        
        private static uint[] threadIds;

        private static Object currentDebugEvent;

        private static BlockingCollection<Object> debugEvents = new BlockingCollection<Object>();


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

            public ulong address;

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
           
            public uint maxSharedBreakpoints;

            public ulong address;
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
            evet.threadid = lwpid;
            evet.address = regs.r_rip;
            Thread debugThread = new Thread(addDebugEvent) { IsBackground = true };
            debugThread.Start(evet);
            

            //ps4.Notify(222, "interrupt hit\n(thread: " + tdname + " id: " + (object)lwpid + ")");

            // this.data = this.ps4.ReadMemory(this.attachpid, this.address, this.length);

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
            processEvent.address = 0;
            //processEvent.maxBreakpointCount = 0;
            //processEvent.maxWatchpointCount = 0;

            processEvent.maxSharedBreakpoints = 4;

            addDebugEvent(processEvent);
           

            DebugEvent deEvent = new DebugEvent();
            deEvent.debugevent = -1;
            deEvent.threadid = threadIds[0];
            deEvent.address = PS4APIWrapper.getRegisters(threadIds[0]).r_rip;
            addDebugEvent(deEvent);


            return value;
        }
        public static IntPtr StopDebug(IntPtr handle)
        {
            IntPtr value = (IntPtr)PS4APIWrapper.dettachDebugger();

            return value;
        }


        public static IntPtr SetBreakpoint(IntPtr handle, int tid, IntPtr debugreg, UInt64 address, int bptype, int bpsize)
        {
            try
            {
                if (tid == -1)
                {
                    for (int i = 0; i < PS4DBG.MAX_WATCHPOINTS; i++)
                    {
                        try
                        {
                            PS4APIWrapper.SetWatchpoint(i, address, bpsize, bptype);
                            return (IntPtr)i;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("cant create watch point");
                        }

                    }
                    return (IntPtr)0;
                }
                else
                {
                    PS4APIWrapper.SetWatchpoint(tid, address, bpsize, bptype);
                    return (IntPtr)1;
                }
            }
            catch (Exception e)
            {
                return (IntPtr)0;
            }
        }

        public static IntPtr RemoveBreakpoint(IntPtr handle, int tid, IntPtr debugreg, int wasWatchpoint)
        {
            try
            {
                if (tid == -1)
                {
                    PS4APIWrapper.ClearWatchPoints();
                }
                else
                {
                    PS4APIWrapper.ClearWatchpoint(tid);
                }

                return (IntPtr)1;
            }
            catch (Exception e)
            {

                return (IntPtr)0;
            }
        }

        public static IntPtr SuspendThread(IntPtr handle, uint tid)
        {
            try
            {
                PS4APIWrapper.SuspendDebuggerThread(tid);
                return (IntPtr)1;
            }
            catch (Exception e)
            {

                return (IntPtr)0;
            }
        }
        public static IntPtr ResumeThread(IntPtr handle, uint tid)
        {
            try
            {
                PS4APIWrapper.resumeDebuggerThread(tid);
                return (IntPtr)1;
            }
            catch (Exception e)
            {

                return (IntPtr)0;
            }
        }

        public static IntPtr GetThreadContext(IntPtr handle, uint tid, out CONTEXT Context, int type)
        {
            try
            {
                regs r = PS4APIWrapper.getRegisters(tid);
                assignThreadRegisters(r, out CONTEXT_REGS regs);
                Context = new CONTEXT();
                Context.regs = regs;
                return (IntPtr)1;
            }
            catch (Exception e)
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
            catch (Exception e)
            {
                evet = new DebugEvent();
                return (IntPtr)0;
            }
        }

        public static IntPtr ContinueForDebugEvent(IntPtr handle, uint tid, uint continuemethod)
        {
            try
            {
                return (IntPtr)1;

            }
            catch (Exception e)
            {
                return (IntPtr)0;
            }
        }

    }
}
