using CEServerPS4.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.C2S
{
    public class GetArchitectureCommand : CheatEngineCommand<GetArchitectureResponse>
    {
        public override CommandType CommandType => CommandType.CMD_GETARCHITECTURE;

        public GetArchitectureCommand()
        {
            this.initialized = true;
        }
        public override void Initialize(BinaryReader reader)
        {
            this.initialized = true;
        }

        public override GetArchitectureResponse Process()
        {
            //https://github.com/cheat-engine/cheat-engine/blob/master/Cheat%20Engine/ceserver/ceserver.c#L137-L153
            return new GetArchitectureResponse(Architecture.x86_64);
        }
    }
}
