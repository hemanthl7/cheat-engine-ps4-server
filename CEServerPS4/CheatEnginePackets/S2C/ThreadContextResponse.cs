using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace CEServerPS4.CheatEnginePackets.S2C
{
    public class ThreadContextResponse : ICheatEngineResponse
    {
        public PS4API.DebugAPI.CONTEXT_REGS rgs;
        public bool isSucess;
      
        public ThreadContextResponse(PS4API.DebugAPI.CONTEXT_REGS regs,bool isSucess)
        {
            this.rgs = regs;
            this.isSucess = isSucess;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            if (!isSucess)
            {
                br.Write(0);
                br.Close();
                return ms.ToArray();
            }
            byte[] regs = value();
            br.Write(regs.Length);
            br.Write(regs);
            br.Close();
            return ms.ToArray();
        }

        private byte[] value()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, rgs);
                return ms.ToArray();
            }
        }
    }
}
