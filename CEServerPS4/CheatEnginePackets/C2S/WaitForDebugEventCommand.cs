using CEServerPS4.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.C2S
{
    public class WaitForDebugEventCommand : CheatEngineCommand<WaitForDebugEventResponse>
    {
        public override CommandType CommandType => CommandType.CMD_WAITFORDEBUGEVENT;// throw new NotImplementedException();
        public IntPtr handle;
        public uint timeout;
        public WaitForDebugEventCommand()
        {

        }

        public WaitForDebugEventCommand(IntPtr handle,uint timeout)
        {
            this.handle = handle;
            this.timeout = timeout;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            this.handle = (IntPtr)reader.ReadInt32();
            this.timeout = reader.ReadUInt32();
            this.initialized = true;
        }

        public override WaitForDebugEventResponse Process()
        {
            IntPtr hande = PS4API.DebugAPI.WaitForDebugEvent(handle, timeout,out object evt);
            Console.WriteLine("wait");
            return new WaitForDebugEventResponse(hande,evt);
        }
    }
}
