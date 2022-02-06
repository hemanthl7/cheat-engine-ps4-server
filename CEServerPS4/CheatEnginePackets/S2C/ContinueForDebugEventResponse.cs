using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace CEServerPS4.CheatEnginePackets.S2C
{
    public class ContinueForDebugEventResponse : ICheatEngineResponse
    {
        private IntPtr handle;
        private PS4API.DebugAPI.DebugEvent et;


        public ContinueForDebugEventResponse(IntPtr handle)
        {
            this.handle = handle;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
           
            br.Write((int)handle);          
            br.Close();
            return ms.ToArray();
        }
   
    }
}
