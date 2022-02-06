using CEServerPS4.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.C2S
{
    public class SuspendThreadCommand : CheatEngineCommand<HandleResponse>
    {
        public override CommandType CommandType => CommandType.CMD_SUSPENDTHREAD;// throw new NotImplementedException();
        public IntPtr handle;
        public uint ThreadId;
        public SuspendThreadCommand()
        {

        }

        public SuspendThreadCommand(IntPtr handle,uint ThreadId)
        {
            this.handle = handle;
            this.ThreadId = ThreadId;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            this.handle = (IntPtr)reader.ReadInt32();
            this.ThreadId = reader.ReadUInt32();
            this.initialized = true;
        }

        public override HandleResponse Process()
        {
            IntPtr hande = PS4API.DebugAPI.SuspendThread(handle, ThreadId);

            return new HandleResponse(hande);
        }
    }
}
