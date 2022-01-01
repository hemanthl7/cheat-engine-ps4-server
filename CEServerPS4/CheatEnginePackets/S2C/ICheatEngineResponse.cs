using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.S2C
{
    public interface ICheatEngineResponse
    {
        byte[] Serialize();

    }
}
