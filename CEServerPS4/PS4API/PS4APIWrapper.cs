using System;
using System.Threading;
using libdebug;
using System.Runtime.InteropServices;

namespace CEServerPS4.PS4API
{

    public class PS4APIWrapper
    {
        private static PS4DBG ps4Debugger;

        public static int num_threads = 3;
        public static PS4DBG[] ps4 = new PS4DBG[num_threads];
        private static Mutex[] mutex = new Mutex[num_threads];


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

        private static  int mutexId = 0;

        private static PS4APIWrapper ps4Wrapper;

        public static PS4APIWrapper Instance()
        {
            if(ps4Wrapper == null)
            {
                ps4Wrapper = new PS4APIWrapper();
            }
            return ps4Wrapper;
        }


        static PS4APIWrapper()
        {
            for (int i = 0; i < num_threads; i++)
                mutex[i] = new Mutex();
        }

        private static int MutrexID
        {
            get
            {
                mutexId++;
                if (mutexId > 2)
                {
                    mutexId = 0;
                }
                return mutexId;
            }
        }


        public static int ProcessID
        {
          get; set; 
        }

        public static bool Connect(string ip)
        {
            try
            {
                for (int i = 0; i < num_threads; i++)
                {
                    mutex[i].WaitOne();
                    ps4[i] = new PS4DBG(ip);
                    ps4[i].Connect();                   
                }
                ps4Debugger = new PS4DBG(ip);
                ps4Debugger.Connect();
                return true; 
            }
            catch
            {
                Console.WriteLine("unable to connect");
            }
            finally
            {
                for (int i = 0; i < num_threads; i++)
                    mutex[i].ReleaseMutex();
            }
            return false;
        }

        public static bool Disconnect()
        {

            try
            {
                for (int i = 0; i < num_threads; i++)
                {
                    mutex[i].WaitOne();
                    if (ps4[i] != null)
                    {
                        ps4[i].Disconnect();
                    }                 
                }

                return true;
            }
            catch
            {
                Console.WriteLine("unable to disconnect");
            }
            finally
            {
                for (int i = 0; i < num_threads; i++)
                    mutex[i].ReleaseMutex();
            }
            return false;
        }

        public static byte[] ReadMemory(ulong address, int length)
        {
            int currentThreadId = MutrexID;
            mutex[currentThreadId].WaitOne();
            try
            {
                return ps4[currentThreadId].ReadMemory(ProcessID, address, length);                
            }
            catch
            {
                Console.WriteLine("Error while reading Memory ");
            }
            finally
            {
                mutex[currentThreadId].ReleaseMutex();
            }
            return new byte[length];
        }

        public static void WriteMemory(ulong address, byte[] data)
        {
            int currentThreadId = MutrexID;
            mutex[currentThreadId].WaitOne();
            try
            {
                ps4[currentThreadId].WriteMemory(ProcessID, address, data);
            }
            catch
            {
                Console.WriteLine("Error while write Memory ");
            }
            finally
            {
                mutex[currentThreadId].ReleaseMutex();
            }
        }

        public static ProcessList GetProcessList()
        {
            int currentThreadId = MutrexID;
            mutex[currentThreadId].WaitOne();
            try
            {
                return ps4[currentThreadId].GetProcessList();

            }
            catch
            {
                Console.WriteLine("Error while GetProcessList ");
            }
            finally
            {
                mutex[currentThreadId].ReleaseMutex();
            }
            return null;
        }

        public static ProcessInfo? GetProcessInfo(int processID)
        {
            int currentThreadId = MutrexID;
            mutex[currentThreadId].WaitOne();
           
            try
            {
                ProcessInfo processInfo = ps4[currentThreadId].GetProcessInfo(processID);
                return processInfo;
            }
            catch
            {
                Console.WriteLine("Error while GetProcessList ");
            }
            finally
            {
                mutex[currentThreadId].ReleaseMutex();
            }
            return null;
        }

        public static ProcessMap GetProcessMaps(int processID)
        {
            int currentThreadId = MutrexID;
            mutex[currentThreadId].WaitOne();
            try
            {
                ProcessMap processMap = ps4[currentThreadId].GetProcessMaps(processID);
                return processMap;
            }
            catch
            {
                Console.WriteLine("Error while GetProcessMaps ");
            }
            finally
            {
                mutex[currentThreadId].ReleaseMutex();
            }
            return null;
        }

        public static uint[] GetThreadsList()
        {
            
            try
            {
                return ps4Debugger.GetThreadList(); 
            }
            catch
            {
                Console.WriteLine("Error while GetThreadsList ");
            }
            
            return null;
        }

        public static int attachDebugger()
        {
            try
            {
                if (!ps4Debugger.IsDebugging)
                {
                    ps4Debugger.AttachDebugger(ProcessID, new PS4DBG.DebuggerInterruptCallback(DebugAPI.DebuggerInterruptCallback));
                    ps4Debugger.ProcessResume();
                }
               
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine("unable to connect");
                Console.WriteLine(e.Message);
            }

            return 0;
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
            Console.WriteLine("status=:" + regs.r_rip);


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
                Console.WriteLine("unable to detach");
                Console.WriteLine(e.Message);
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
                Console.WriteLine("unable to resume");
                Console.WriteLine(e.Message);
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
                Console.WriteLine("unable to detach");
                Console.WriteLine(e.Message);
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
                Console.WriteLine("unable to detach");
                Console.WriteLine(e.Message);
            }

        }

      
        public static void ClearBreakpoints()
        {
            for (int index = 0; (long)index < (long)PS4DBG.MAX_BREAKPOINTS; ++index)
                ps4Debugger.ChangeBreakpoint(index, false, 0UL);
        }

        public static void ClearWatchPoints()
        {
            for (int index = 0; (long)index < (long)PS4DBG.MAX_WATCHPOINTS; ++index)
                ps4Debugger.ChangeWatchpoint(index, false, PS4DBG.WATCHPT_LENGTH.DBREG_DR7_LEN_1,
                    PS4DBG.WATCHPT_BREAKTYPE.DBREG_DR7_EXEC, 0UL);
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
                Console.WriteLine("breakpoint error");
            }
            
        }

        public static  void ClearWatchpoint(int watchpoint)
        {
            ps4Debugger.ChangeWatchpoint(watchpoint, false,
                PS4DBG.WATCHPT_LENGTH.DBREG_DR7_LEN_1, PS4DBG.WATCHPT_BREAKTYPE.DBREG_DR7_EXEC, 0UL);
        }

        public static  void SetBreakpoint(int watchpoint,ulong address)
        {
            ps4Debugger.ChangeBreakpoint(watchpoint, true, address);
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
