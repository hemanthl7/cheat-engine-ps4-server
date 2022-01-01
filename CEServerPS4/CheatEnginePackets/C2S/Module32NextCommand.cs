using CEServerPS4.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.C2S
{
    public class Module32NextCommand : Module32FirstCommand
    {
        public override CommandType CommandType => CommandType.CMD_MODULE32NEXT;

        public override Module32Response Process()
        {
            PS4API.ToolHelp.MODULEENTRY32 moduleEntry = new PS4API.ToolHelp.MODULEENTRY32();
            moduleEntry.dwSize = (uint)Marshal.SizeOf(moduleEntry);

            var result = PS4API.ToolHelp.Module32Next(this.Handle, ref moduleEntry);

            return new Module32Response(result, moduleEntry);
        }

    }
}
