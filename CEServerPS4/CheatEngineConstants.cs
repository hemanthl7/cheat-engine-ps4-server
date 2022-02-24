using CEServerPS4.CheatEnginePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CEServerPS4.CheatEnginePackets.C2S;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace CEServerPS4
{
    public static class CheatEngineConstants 
    {
        public static bool isCustomCheatEngine
        {
            get; set;
        }

    }
}
