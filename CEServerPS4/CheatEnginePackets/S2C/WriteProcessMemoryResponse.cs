using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.S2C
{
    public class WriteProcessMemoryResponse : ICheatEngineResponse
    {
        public Int32 Data;
        public WriteProcessMemoryResponse(Int32 data)
        {
            this.Data = data;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            br.Write(Data);
            br.Close();
            return ms.ToArray();
        }
    }
}
