using CEServerPS4.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.C2S
{
    public class StopDebugCommand : CheatEngineCommand<HandleResponse>
    {
        public override CommandType CommandType => CommandType.CMD_STOPDEBUG;// throw new NotImplementedException();
        public IntPtr handle;
        public StopDebugCommand()
        {

        }

        public StopDebugCommand(IntPtr handle)
        {
            this.handle = handle;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            this.handle = (IntPtr)reader.ReadInt32();
            this.initialized = true;
        }

        public override HandleResponse Process()
        {
            IntPtr hande = PS4API.DebugAPI.StopDebug(handle);

            return new HandleResponse(hande);
        }
    }
}
