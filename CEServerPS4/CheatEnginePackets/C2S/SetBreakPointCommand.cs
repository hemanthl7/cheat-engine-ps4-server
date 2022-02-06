using CEServerPS4.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.C2S
{
    public class SetBreakPointCommand : CheatEngineCommand<HandleResponse>
    {
        public IntPtr Handle;
        public int bid;
        public IntPtr debugregister;
        public ulong Address;
        public int bptype;
        public int bpsize;

        public override CommandType CommandType => CommandType.CMD_SETBREAKPOINT;

        public SetBreakPointCommand() { }

        public SetBreakPointCommand(IntPtr handle, int bid, IntPtr debugregister, UInt64 address, int bptype, int bpsize)
        {
            this.Handle = handle;
            this.bid = bid;
            this.debugregister = debugregister;
            this.Address = address;
            this.bptype = bptype;
            this.bpsize = bpsize;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            Handle = (IntPtr)reader.ReadInt32();
            bid = reader.ReadInt32();
            debugregister = (IntPtr)reader.ReadInt32();
            Address = reader.ReadUInt64();
            bptype = reader.ReadInt32();
            bpsize = reader.ReadInt32();
            this.initialized = true;
        }
                         
        public override HandleResponse Process()
        {

            IntPtr o = PS4API.DebugAPI.SetBreakpoint(this.Handle, this.bid, this.debugregister, this.Address, this.bptype,this.bpsize);

            return new HandleResponse(o);
        }
    }
}
