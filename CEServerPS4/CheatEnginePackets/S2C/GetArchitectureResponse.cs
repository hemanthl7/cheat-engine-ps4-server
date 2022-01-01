using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.S2C
{
    public class GetArchitectureResponse : ICheatEngineResponse
    {
        public Architecture Architecture;

        public GetArchitectureResponse(Architecture arch)
        {
            this.Architecture = arch;
        }

        public byte[] Serialize()
        {
            return new byte[] { (byte)Architecture };
        }
    }
}
