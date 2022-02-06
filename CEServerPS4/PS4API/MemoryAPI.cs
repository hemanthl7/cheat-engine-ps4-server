using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using libdebug;

namespace CEServerPS4.PS4API
{
    public static class MemoryAPI
    {

        private static  uint regionsize = 512;

        private static uint pagesize = 512 * 8;

        private static SortedDictionary<UInt64, MEMORY_BASIC_INFORMATION> memoryMap = new SortedDictionary<UInt64, MEMORY_BASIC_INFORMATION>();

        private static List<UInt64> keys;

        public enum AllocationProtectEnum : uint
        {
            PAGE_EXECUTE = 0x00000010,
            PAGE_EXECUTE_READ = 0x00000020,
            PAGE_EXECUTE_READWRITE = 0x00000040,
            PAGE_EXECUTE_WRITECOPY = 0x00000080,
            PAGE_NOACCESS = 0x00000001,
            PAGE_READONLY = 0x00000002,
            PAGE_READWRITE = 0x00000004,
            PAGE_WRITECOPY = 0x00000008,
            PAGE_GUARD = 0x00000100,
            PAGE_NOCACHE = 0x00000200,
            PAGE_WRITECOMBINE = 0x00000400
        }

        public enum StateEnum : uint
        {
            MEM_COMMIT = 0x1000,
            MEM_FREE = 0x10000,
            MEM_RESERVE = 0x2000
        }

        public enum TypeEnum : uint
        {
            MEM_IMAGE = 0x1000000,
            MEM_MAPPED = 0x40000,
            MEM_PRIVATE = 0x20000
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public AllocationProtectEnum AllocationProtect;
            public IntPtr RegionSize;
            public StateEnum State;
            public AllocationProtectEnum Protect;
            public TypeEnum Type;
        }

        [StructLayout(LayoutKind.Sequential)]
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
        public struct CONTEXT
        { 
           public  CONTEXT_REGS regs;
        }


    public static bool ReadProcessMemory(
           IntPtr hProcess,
           ulong lpBaseAddress,
           out byte[] lpBuffer,
           Int32 nSize,
           out IntPtr lpNumberOfBytesRead)
        {
            byte[] readbuffer = new byte[nSize];
            readbuffer = PS4APIWrapper.ReadMemory(lpBaseAddress, nSize);
            lpBuffer = readbuffer;
            string value = BitConverter.ToString(readbuffer);
            lpNumberOfBytesRead = new IntPtr(readbuffer.Length);
            return true;
        }

        
        public static int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength)
        {
            lpBuffer = new MEMORY_BASIC_INFORMATION();
            ulong address = (ulong)lpAddress;
            if (keys.Contains(address))
            {
                memoryMap.TryGetValue(address, out lpBuffer);
                if (lpBuffer.BaseAddress == null)
                {
                    return 0;
                }
                return 1;
            }

            if (lpAddress.ToInt64() == 0)
            {
                lpBuffer = memoryMap.First().Value;
                return 1;
            }
            else
            {

                foreach(ulong adrs in keys)
                {
                    if (address < adrs)
                    {
                        memoryMap.TryGetValue(adrs, out lpBuffer);
                        if (lpBuffer.BaseAddress == null)
                        {
                            return 0;
                        }

                        return 1;
                    }
                }
               
                return 0;
            }

        }


        public static int VirtualQueryExFull(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength)
        {
            if (lpAddress.ToInt64() == 0)
            {
                lpBuffer = memoryMap.First().Value;
                return 1;
            }
            else
            {
                if (lpAddress.ToInt32() < keys.Count)
                {
                    ulong address = keys[lpAddress.ToInt32()];
                    memoryMap.TryGetValue(address, out lpBuffer);
                    if (lpBuffer.BaseAddress == null)
                    {
                        return 0;
                    }
                }
                else
                {
                    lpBuffer = new PS4API.MemoryAPI.MEMORY_BASIC_INFORMATION();
                    return 0;
                }
                
                
                return 1;
            }

        }


        public static void intit( int pid)
        {
            memoryMap.Clear();
            ProcessMap pm = PS4APIWrapper.GetProcessMaps(pid);

            for (int i = 0; i < pm.entries.Length; i++)
            {
                MemoryEntry entry = pm.entries[i];
                if ((entry.prot & 0x1) == 0x1)
                {

                    ulong length = entry.end - entry.start;
                    ulong start = entry.start;
                    string name = entry.name;
                    int idx = 0;
                    uint buffer_length = regionsize * pagesize;

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
                        MEMORY_BASIC_INFORMATION m32Entry = new MEMORY_BASIC_INFORMATION();
                        m32Entry.BaseAddress = (IntPtr)start;
                        m32Entry.AllocationBase = (IntPtr)start;
                        m32Entry.AllocationProtect = allocationProtect(entry.prot);
                        m32Entry.RegionSize = (IntPtr)cur_length;
                        m32Entry.Type = typeEnum(entry.prot);
                        part++;

                        memoryMap[start]= m32Entry;

                        start += cur_length;
                        ++idx;
                    }
                }
            }
            keys = new List<UInt64>(memoryMap.Keys.AsEnumerable());

        }

        private static AllocationProtectEnum allocationProtect(uint prot)
        {
            if ((uint)PS4DBG.VM_PROTECTIONS.VM_PROT_ALL ==prot)
            {
                return AllocationProtectEnum.PAGE_EXECUTE_READWRITE;
            }
            else if((uint)PS4DBG.VM_PROTECTIONS.VM_PROT_EXECUTE == prot)
            {
                return AllocationProtectEnum.PAGE_EXECUTE;
            }
            else if ((uint)PS4DBG.VM_PROTECTIONS.VM_PROT_READ == prot)
            {
                return AllocationProtectEnum.PAGE_READONLY;
            }
            else if ((uint)PS4DBG.VM_PROTECTIONS.VM_PROT_DEFAULT == prot)
            {
                return AllocationProtectEnum.PAGE_READWRITE;
            }      
            return AllocationProtectEnum.PAGE_WRITECOPY;
        }


        private static TypeEnum typeEnum(uint prot)
        {
            return TypeEnum.MEM_PRIVATE;
        }

    }
}
