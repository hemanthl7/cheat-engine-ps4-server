using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.S2C
{
    public class Process32Response : ICheatEngineResponse
    {
        public bool Result;
        public PS4API.ToolHelp.PROCESSENTRY32 ProcessEntry;

        public Process32Response(bool result, PS4API.ToolHelp.PROCESSENTRY32 processEntry)
        {
            this.Result = result;
            this.ProcessEntry = processEntry;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);

            br.Write((int)(this.Result ? 1 : 0));
            if (this.Result)
            {
                br.Write((int)ProcessEntry.th32ProcessID);
                br.Write((int)ProcessEntry.szExeFile.Length);
                br.Write(Encoding.UTF8.GetBytes(ProcessEntry.szExeFile));
            }
            else
            {
                br.Write((int)0);
                br.Write((int)0);
            }

            br.Close();
            return ms.ToArray();
        }
    }
}
