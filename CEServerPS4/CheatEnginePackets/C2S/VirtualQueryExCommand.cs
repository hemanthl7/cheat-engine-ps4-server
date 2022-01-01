using CEServerPS4.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.C2S
{
    public class VirtualQueryExCommand : CheatEngineCommand<VirtualQueryExResponse>
    {

        public IntPtr Handle;
        public UInt64 Address;

        public override CommandType CommandType => CommandType.CMD_VIRTUALQUERYEX;// throw new NotImplementedException();

        public VirtualQueryExCommand() { }

        public VirtualQueryExCommand(IntPtr handle, UInt64 address) 
        {
            this.Handle = handle;
            this.Address = address;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            this.Handle = (IntPtr)reader.ReadInt32();
            this.Address = reader.ReadUInt64();
            this.initialized = true;
        }

        public override VirtualQueryExResponse Process()
        {
            PS4API.MemoryAPI.MEMORY_BASIC_INFORMATION mbi = new PS4API.MemoryAPI.MEMORY_BASIC_INFORMATION();
            int ret = PS4API.MemoryAPI.VirtualQueryEx(Handle, (IntPtr)Address, out mbi, (uint)Marshal.SizeOf(mbi));

            return new VirtualQueryExResponse(ret, mbi);
        }
    }
}
