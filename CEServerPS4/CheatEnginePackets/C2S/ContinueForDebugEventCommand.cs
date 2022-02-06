using CEServerPS4.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.C2S
{
    public class ContinueForDebugEventCommand : CheatEngineCommand<ContinueForDebugEventResponse>
    {
        public override CommandType CommandType => CommandType.CMD_CONTINUEFROMDEBUGEVENT;// throw new NotImplementedException();
        public IntPtr handle;
        public uint tid;
        public uint ignore;
        public ContinueForDebugEventCommand()
        {

        }

        public ContinueForDebugEventCommand(IntPtr handle,uint tid,uint ignore)
        {
            this.handle = handle;
            this.tid = tid;
            this.ignore = ignore;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            this.handle = (IntPtr)reader.ReadInt32();
            this.tid = reader.ReadUInt32();
            this.ignore = reader.ReadUInt32();
            this.initialized = true;
        }

        public override ContinueForDebugEventResponse Process()
        {
            IntPtr hande = PS4API.DebugAPI.ContinueForDebugEvent(handle, tid, ignore);

            return new ContinueForDebugEventResponse(hande);
        }
    }
}
