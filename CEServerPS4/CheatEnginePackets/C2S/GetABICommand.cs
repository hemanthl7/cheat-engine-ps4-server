using CEServerPS4.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.C2S
{
    public class GetABICommand : CheatEngineCommand<GetABIResponse>
    {
        public override CommandType CommandType => CommandType.CMD_GETABI;

        public GetABICommand()
        {
            this.initialized = true;
        }
        public override void Initialize(BinaryReader reader)
        {
            this.initialized = true;
        }

        public override GetABIResponse Process()
        {
            //https://github.com/cheat-engine/cheat-engine/blob/master/Cheat%20Engine/ceserver/ceserver.c#L137-L153
            return new GetABIResponse();
        }
    }
}
