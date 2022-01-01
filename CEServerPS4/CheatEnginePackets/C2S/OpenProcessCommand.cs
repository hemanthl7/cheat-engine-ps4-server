using CEServerPS4.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.C2S
{
    public class OpenProcessCommand : CheatEngineCommand<HandleResponse>
    {
        public override CommandType CommandType => CommandType.CMD_OPENPROCESS;// throw new NotImplementedException();
        public int ProcessID;
        public OpenProcessCommand()
        {

        }

        public OpenProcessCommand(int pid)
        {
            this.ProcessID = pid;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            this.ProcessID = reader.ReadInt32();
            this.initialized = true;
        }

        public override HandleResponse Process()
        {
            IntPtr handle = PS4API.ToolHelp.OpenProcess(
                PS4API.ToolHelp.ProcessAccessFlags.VirtualMemoryRead |
                PS4API.ToolHelp.ProcessAccessFlags.VirtualMemoryWrite |
                PS4API.ToolHelp.ProcessAccessFlags.QueryInformation, 
                false, this.ProcessID);

            return new HandleResponse(handle);
        }
    }
}
