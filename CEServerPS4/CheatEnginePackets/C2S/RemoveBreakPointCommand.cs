using CEServerPS4.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.C2S
{
    public class RemoveBreakPointCommand : CheatEngineCommand<HandleResponse>
    {
        public IntPtr Handle;
        public int bid;
        public IntPtr debugregister;
        public int wasbp;

        public override CommandType CommandType => CommandType.CMD_REMOVEBREAKPOINT;

        public RemoveBreakPointCommand() { }

        public RemoveBreakPointCommand(IntPtr handle, int bid, IntPtr debugregister, int wasbp)
        {
            this.Handle = handle;
            this.bid = bid;
            this.debugregister = debugregister;
            this.wasbp = wasbp;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            Handle = (IntPtr)reader.ReadInt32();
            bid = reader.ReadInt32();
            debugregister = (IntPtr)reader.ReadInt32();
            wasbp = reader.ReadInt32();
            this.initialized = true;
        }
                         
        public override HandleResponse Process()
        {

            IntPtr o = PS4API.DebugAPI.RemoveBreakpoint(this.Handle, this.bid, this.debugregister,this.wasbp);
      

            return new HandleResponse(o);
        }
    }
}
