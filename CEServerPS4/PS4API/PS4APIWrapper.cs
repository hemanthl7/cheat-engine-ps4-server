﻿using System;
using System.Threading;
using libdebug;
using System.Runtime.InteropServices;

namespace CEServerPS4.PS4API
{

    public class PS4APIWrapper
    {

        public static int num_threads = 6;
        public static PS4DBG[] ps4 = new PS4DBG[num_threads];
        private static Mutex[] mutex = new Mutex[num_threads];



        private static int mutexId = 0;


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
                if (mutexId > num_threads - 1)
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

        public static bool Connect()
        {
            try
            {
                for (int i = 0; i < num_threads; i++)
                {
                    mutex[i].WaitOne();
                    ps4[i] = new PS4DBG(PS4Static.IP);
                    ps4[i].Connect();
                }

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

    }
}
