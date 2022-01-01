using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.C2S
{
    public interface ICheatEngineCommand
    {
       bool initialized { get;  }

       void Initialize(System.IO.BinaryReader reader);

       void Unintialize();
       CommandType CommandType { get; }
       byte[] ProcessAndGetBytes();

    }
}
