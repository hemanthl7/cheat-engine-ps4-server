using CEServerPS4.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.C2S
{
    public class Process32NextCommand : Process32FirstCommand
    {
        public override CommandType CommandType => CommandType.CMD_PROCESS32NEXT;

        public override Process32Response Process()
        {
            PS4API.ToolHelp.PROCESSENTRY32 processEntry = new PS4API.ToolHelp.PROCESSENTRY32();
            processEntry.dwSize = (uint)Marshal.SizeOf(processEntry);

            var result = PS4API.ToolHelp.Process32Next(this.Handle, ref processEntry);

            return new Process32Response(result, processEntry);
        }

    }
}
