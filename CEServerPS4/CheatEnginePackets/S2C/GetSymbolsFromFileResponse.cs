using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEServerPS4.CheatEnginePackets.S2C
{
    public class GetSymbolsFromFileResponse : ICheatEngineResponse
    {
        public byte[] Data;

        public GetSymbolsFromFileResponse(byte[] data)
        {
            this.Data = data;
        }
        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            br.Write(0L);

            br.Close();
            return ms.ToArray();
        }
    }
}
