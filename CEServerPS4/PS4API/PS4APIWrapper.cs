using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using libdebug;

namespace CEServerPS4.PS4API
{

    public class PS4APIWrapper
    {
        private static PS4DBG ps4Debugger;

        public static int num_threads = 3;
        public static PS4DBG[] ps4 = new PS4DBG[num_threads];
        private static Mutex[] mutex = new Mutex[num_threads];

        private int mutexId = 0;

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

        private int MutrexID
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


        public int ProcessID
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
                    mutex[i].ReleaseMutex();
                }
                ps4Debugger = new PS4DBG(ip);
                return true;
            }
            catch
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
                    mutex[i].ReleaseMutex();
                }

                return true;
            }
            catch
            {
                for (int i = 0; i < num_threads; i++)
                    mutex[i].ReleaseMutex();
            }
            return false;
        }

        public byte[] ReadMemory(ulong address, int length)
        {
            int currentThreadId = MutrexID;
            mutex[currentThreadId].WaitOne();
            try
            {
                byte[] buf = ps4[currentThreadId].ReadMemory(ProcessID, address, length);
                mutex[currentThreadId].ReleaseMutex();
                return buf;
            }
            catch
            {
                mutex[currentThreadId].ReleaseMutex();
            }
            return new byte[length];
        }

        public void WriteMemory(ulong address, byte[] data)
        {
            mutex[0].WaitOne();
            try
            {
                ps4[0].WriteMemory(ProcessID, address, data);
                mutex[0].ReleaseMutex();
            }
            catch
            {
                mutex[0].ReleaseMutex();
            }
        }

        public static ProcessList GetProcessList()
        {
            mutex[0].WaitOne();
            try
            {
                ProcessList processList = ps4[0].GetProcessList();
                mutex[0].ReleaseMutex();
                return processList;
            }
            catch
            {
                mutex[0].ReleaseMutex();
            }
            return null;
        }

        public static ProcessInfo? GetProcessInfo(int processID)
        {
            mutex[0].WaitOne();
            try
            {
                ProcessInfo processInfo = ps4[0].GetProcessInfo(processID);
                mutex[0].ReleaseMutex();
                return processInfo;
            }
            catch
            {
                mutex[0].ReleaseMutex();
                return null;
            }
        }

        public static ProcessMap GetProcessMaps(int processID)
        {
            mutex[0].WaitOne();
            try
            {
                ProcessMap processMap = ps4[0].GetProcessMaps(processID);
                mutex[0].ReleaseMutex();
                return processMap;
            }
            catch
            {
                mutex[0].ReleaseMutex();
                return null;
            }
        }

    }
}
