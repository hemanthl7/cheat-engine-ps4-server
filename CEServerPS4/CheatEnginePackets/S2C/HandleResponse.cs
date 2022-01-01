using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.S2C
{
    public class HandleResponse : ICheatEngineResponse
    {
        public IntPtr Handle;

        public HandleResponse(IntPtr handle)
        {
            this.Handle = handle;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);

            br.Write((int)this.Handle);
            br.Close();
            return ms.ToArray();
        }
    }
}
