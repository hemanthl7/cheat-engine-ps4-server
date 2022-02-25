using System;
using System.Threading;
using libdebug;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace CEServerPS4.PS4API
{

    public class PS4DedugAPIWrapper
    {
        private static PS4DBG ps4Debugger;

        public static PS4DBG getps4()
        {
            return ps4Debugger;
        }

        public static uint[] GetThreadsList()
        {
            
            try
            {
                return ps4Debugger.GetThreadList(); 
            }
            catch (Exception e)
            {
                Trace.WriteLine("Error while GetThreadsList :" + e.Message);
            }
            
            return null;
        }

        public static int attachDebugger(PS4DBG.DebuggerInterruptCallback callback)
        {
            try
            {
                ps4Debugger = new PS4DBG(PS4Static.IP);
                ps4Debugger.Connect();
                if (!ps4Debugger.IsDebugging)
                {
                    ps4Debugger.AttachDebugger(PS4APIWrapper.ProcessID, callback);
                    ps4Debugger.ProcessResume();
                }
               
                return 1;
            }
            catch (Exception e)
            {
                Trace.WriteLine("unable to attach:" + e.Message);
                Trace.WriteLine(e.Message);
            }

            return 0;
        }

        public static int dettachDebugger()
        {
            try
            {
                ps4Debugger.DetachDebugger();
                return 1;
            }
            catch(Exception e)
            {
                Trace.WriteLine("unable to detach:" + e.Message);
                Trace.WriteLine(e.Message);
                return 0;
            }
            
        }


        public static void ProcessResume()
        {
            try
            {
                ps4Debugger.ProcessResume();
            }
            catch (Exception e)
            {
                Trace.WriteLine("unable to resume process:" + e.Message);
                Trace.WriteLine(e.Message);
            }

        }

        public static void resumeDebuggerThread(uint tid)
        {
            try
            {
                ps4Debugger.ResumeThread(tid);
            }
            catch (Exception e)
            {
                Trace.WriteLine("unable to resume:" + e.Message);
                Trace.WriteLine(e.Message);
                throw e;
            }

        }


        public static void ContinueBreakpointThread(uint tid,bool IsSingleStep)
        {
            try
            {
                if (IsSingleStep)
                {
                    ps4Debugger.SingleStep();
                }
                else
                {
                    ps4Debugger.ProcessResume();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("unable to continue:"+e.Message);
                Trace.WriteLine(e.Message);
                throw e;
            }

        }


        public static void StopDebuggerThread(uint tid)
        {
            try
            {
                ps4Debugger.StopThread(tid);
            }
            catch (Exception e)
            {
                Trace.WriteLine("unable to stop thread:"+e.Message);
                Trace.WriteLine(e.Message);
                throw e;
            }

        }

      
        public static void ClearBreakpoints()
        {
            for (int index = 0; (long)index < (long)PS4DBG.MAX_BREAKPOINTS; ++index)
                ps4Debugger.ChangeBreakpoint(index, false, 0UL);

            ps4Debugger.ProcessResume();
        }

        public static void ClearWatchPoints()
        {
            for (int index = 0; (long)index < (long)PS4DBG.MAX_WATCHPOINTS; ++index)
                ps4Debugger.ChangeWatchpoint(index, false, PS4DBG.WATCHPT_LENGTH.DBREG_DR7_LEN_1,
                    PS4DBG.WATCHPT_BREAKTYPE.DBREG_DR7_EXEC, 0UL);
            ps4Debugger.ProcessResume();
        }

        public static void SetWatchpoint(int watchpoint,ulong address,int watchtLength,int type)
        {
            PS4DBG.WATCHPT_LENGTH watchptLength = PS4DBG.WATCHPT_LENGTH.DBREG_DR7_LEN_1;
            
            if (!(watchtLength == 1))
            {
                if (!(watchtLength == 2))
                {
                    if (!(watchtLength == 4))
                    {
                        if (watchtLength == 8)
                            watchptLength = PS4DBG.WATCHPT_LENGTH.DBREG_DR7_LEN_8;
                    }
                    else
                        watchptLength = PS4DBG.WATCHPT_LENGTH.DBREG_DR7_LEN_4;
                }
                else
                    watchptLength = PS4DBG.WATCHPT_LENGTH.DBREG_DR7_LEN_2;
            }
            else
                watchptLength = PS4DBG.WATCHPT_LENGTH.DBREG_DR7_LEN_1;

            PS4DBG.WATCHPT_BREAKTYPE watchptBreaktype = PS4DBG.WATCHPT_BREAKTYPE.DBREG_DR7_EXEC;

            if (!(type == (int)PS4DBG.WATCHPT_BREAKTYPE.DBREG_DR7_EXEC))
            {
                if (!(type == (int)PS4DBG.WATCHPT_BREAKTYPE.DBREG_DR7_WRONLY))
                {
                    if (type == (int)PS4DBG.WATCHPT_BREAKTYPE.DBREG_DR7_RDWR)
                        watchptBreaktype = PS4DBG.WATCHPT_BREAKTYPE.DBREG_DR7_RDWR;
                }
                else
                    watchptBreaktype = PS4DBG.WATCHPT_BREAKTYPE.DBREG_DR7_WRONLY;
            }
            else
                watchptBreaktype = PS4DBG.WATCHPT_BREAKTYPE.DBREG_DR7_EXEC;
            try
            {
                ps4Debugger.ChangeWatchpoint(watchpoint, true, watchptLength, watchptBreaktype,
                address);
            }catch(Exception e)
            {
                Trace.WriteLine("breakpoint error:"+e.Message);
                throw e;
            }
            
        }

        public static  void ClearWatchpoint(int watchpoint)
        {
            ps4Debugger.ChangeWatchpoint(watchpoint, false,
                PS4DBG.WATCHPT_LENGTH.DBREG_DR7_LEN_1, PS4DBG.WATCHPT_BREAKTYPE.DBREG_DR7_EXEC, 0UL);
        }

        public static  void SetBreakpoint(int breakpoint, ulong address)
        {
            ps4Debugger.ChangeBreakpoint(breakpoint, true, address);
        }

        public static void ClearBreakpoint(int breakpoint)
        {
            ps4Debugger.ChangeBreakpoint(breakpoint, false, 0UL);
        }

        public static regs getRegisters(uint tid)
        {
            return ps4Debugger.GetRegisters(tid);
        }
    }
}
