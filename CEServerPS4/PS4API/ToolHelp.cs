using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using libdebug;

namespace CEServerPS4.PS4API
{
    public static class ToolHelp
    {

        private static PS4APIWrapper ps4 = PS4APIWrapper.Instance();

        private static ProcessList processList = null;

        private static ProcessMap processMap = null;

        private static List<MODULEENTRY32> moduleEntries = new List<MODULEENTRY32>();

        private static int processIndex ;

        private static int moduleIndex;

        [Flags]
        public enum SnapshotFlags : uint
        {
            HeapList = 0x00000001,
            Process = 0x00000002,
            Thread = 0x00000004,
            Module = 0x00000008,
            Module32 = 0x00000010,
            All = (HeapList | Process | Thread | Module),
            Inherit = 0x80000000,
            NoHeaps = 0x40000000
        }

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESSENTRY32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szExeFile;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct MODULEENTRY32
        {
            internal uint dwSize;
            internal uint th32ModuleID;
            internal uint th32ProcessID;
            internal uint GlblcntUsage;
            internal uint ProccntUsage;
            internal IntPtr modBaseAddr;
            internal uint modBaseSize;
            internal IntPtr hModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            internal string szModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            internal string szExePath;
        }



        public static IntPtr CreateToolhelp32Snapshot(SnapshotFlags dwFlags, uint th32ProcessID)
        {
            bool processFlag = ((uint)SnapshotFlags.Process & (uint)dwFlags) != 0;
            bool moduleFlag = ((uint)SnapshotFlags.Module & (uint)dwFlags) != 0;
            if (processFlag)
            {
                processIndex = 0;
                processList = PS4APIWrapper.GetProcessList();
                return (IntPtr)1;

            }else if (moduleFlag)
            {
                moduleIndex = 0;
                processMap = PS4APIWrapper.GetProcessMaps(Convert.ToInt32(th32ProcessID));
                moduleEntries.Clear();
                InitModuleList(processMap);
                return (IntPtr)2;
            }
            else
            {
                return (IntPtr)0;
            }
        }


       
        public static bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe)
        {
            if(processList == null)
            {
                return false;
            }
            if(processIndex< processList.processes.Length)
            {
                Process process = processList.processes[processIndex++];
                processEntry(process, ref lppe);
                return true;
            }
            else
            {
                processIndex = 0;
            }

            return false;
            
        }

        public static bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe)
        {
            return Process32First(hSnapshot, ref lppe);
        }

        public static bool Module32First(IntPtr hSnapshot, ref MODULEENTRY32 lpme)
        {
            if (processMap == null)
            {
                return false;
            }
            if(moduleIndex < moduleEntries.Count())
            {
                MODULEENTRY32 MODULEENTRY32 = moduleEntries[moduleIndex++];
                moduleEntry(MODULEENTRY32, ref lpme);
                return true;
            }
            else
            {
                moduleIndex = 0;
            }

            
            return false;
        }

        private static void moduleEntry(MODULEENTRY32 m1, ref MODULEENTRY32 result)
        {
            result.modBaseAddr = m1.modBaseAddr;
            result.GlblcntUsage = m1.GlblcntUsage;
            result.modBaseSize = m1.modBaseSize;
            result.szModule = m1.szModule;
            result.th32ProcessID = m1.th32ProcessID;
        }



        public static bool Module32Next(IntPtr hSnapshot, ref MODULEENTRY32 lpme)
        {
            return Module32First(hSnapshot, ref lpme);
        }

        public static bool CloseHandle(IntPtr handle)
        {
           return true;
        }

      
        public static IntPtr OpenProcess(
         ProcessAccessFlags processAccess,
         bool bInheritHandle,
         int processId)
        {
            if(processList == null)
            {
                processList = PS4APIWrapper.GetProcessList();
            }
            int i = 0;
            foreach(Process process in processList.processes)
            {
                if (process.pid.Equals(processId))
                {
                    i++;
                    ps4.ProcessID = processId;
                    MemoryAPI.intit(processId);
                    return (IntPtr)i;
                }
            }
           
            return (IntPtr)0;
        }

        private static void processEntry(Process process,ref PROCESSENTRY32 p32Entry)
        {
            p32Entry.th32ProcessID = Convert.ToUInt32(process.pid);
            p32Entry.szExeFile = process.name; 

        }



        public static void InitModuleList(ProcessMap pm)
        {
            moduleEntries.Clear();
            moduleIndex = 0;

            for (int i = 0; i < pm.entries.Length; i++)
            {
                MemoryEntry entry = pm.entries[i];
                if ((entry.prot & 0x1) == 0x1)
                {

                    ulong length = entry.end - entry.start;
                    ulong start = entry.start;
                    string name = entry.name;
                    int idx = 0;
                    uint buffer_length = 1024 * 1024 * 128;

                    //Executable section
                    if ((entry.prot & 0x5) == 0x5)
                    {
                        buffer_length = Convert.ToUInt32(length);
                    }
                    int part = 0;
                    while (length != 0)
                    {
                        uint cur_length = buffer_length;

                        if (cur_length > length)
                        {
                            cur_length = Convert.ToUInt32(length);
                            length = 0;
                        }
                        else
                        {
                            length -= cur_length;
                        }
                        MODULEENTRY32 m32Entry = new PS4API.ToolHelp.MODULEENTRY32();
                        m32Entry.modBaseAddr = (IntPtr)start;
                        m32Entry.GlblcntUsage = Convert.ToUInt32(part);
                        m32Entry.modBaseSize = cur_length;
                        m32Entry.szModule = names(entry.name,part, (IntPtr)start);
                        m32Entry.th32ProcessID = Convert.ToUInt32(pm.pid);
                        part++;

                        moduleEntries.Add(m32Entry);

                        start += cur_length;
                        ++idx;
                    }
                }
            }

        }

        private static string names(string name, int part,IntPtr bs)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Join("-",part ,Convert.ToString(bs.ToInt64(), 16)); 
            }
            return name;
        }
    }
}
