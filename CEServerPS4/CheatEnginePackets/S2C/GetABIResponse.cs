using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.S2C
{
    public class GetABIResponse : ICheatEngineResponse
    {
 
        public GetABIResponse( )
        {
           
        }

        public byte[] Serialize()
        {
            return new byte[] { (byte)1 };
        }
    }
}
