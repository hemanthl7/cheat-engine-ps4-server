using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace CEServerPS4.CheatEnginePackets.S2C
{
    public class ThreadContextResponse : ICheatEngineResponse
    {
        public PS4API.DebugAPI.CONTEXT_REGS rgs;
        public IntPtr handle;

        public ThreadContextResponse(PS4API.DebugAPI.CONTEXT_REGS regs, IntPtr handle)
        {
            this.rgs = regs;
            this.handle = handle;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            if ((int)handle == 0)
            {
                br.Write(0);
                br.Close();
                return ms.ToArray();
            }
          
            br.Write(1);
            br.Write(Marshal.SizeOf(typeof(PS4API.DebugAPI.CONTEXT_REGS)));
            writeregs(br);
            br.Close();
            return ms.ToArray();
        }

        public void writeregs(BinaryWriter br)
        {
            br.Write(rgs.r15);

            br.Write(rgs.r14);

            br.Write(rgs.r13);

            br.Write(rgs.r12);

            br.Write(rgs.rbp);

            br.Write(rgs.rbx);

            br.Write(rgs.r11);

            br.Write(rgs.r10);

            br.Write(rgs.r9);

            br.Write(rgs.r8);

            br.Write(rgs.rax);

            br.Write(rgs.rcx);

            br.Write(rgs.rdx);

            br.Write(rgs.rsi);

            br.Write(rgs.rdi);

            br.Write(rgs.orig_rax);

            br.Write(rgs.rip);

            Trace.WriteLine("adress:"+rgs.rip);

            br.Write(rgs.cs);

            br.Write(rgs.eflags);

            br.Write(rgs.rsp);

            br.Write(rgs.ss);

            br.Write(rgs.fs_base);

            br.Write(rgs.gs_base);

            br.Write(rgs.ds);

            br.Write(rgs.es);

            br.Write(rgs.fs);

            br.Write(rgs.gs);
        }
    }
}
