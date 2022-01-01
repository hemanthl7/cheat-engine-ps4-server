using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.S2C
{
    public class CloseHandleResponse : ICheatEngineResponse
    {

        public bool Result;
        public CloseHandleResponse(bool result)
        {
            this.Result = result;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            br.Write(Result ? 1 : 0);

            br.Close();
            return ms.ToArray();
        }
    }
}
