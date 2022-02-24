using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEServerPS4.EventHandler.Request
{
    public class ContinueDebugEventRequest
    {
        public uint Tid { get; set; }
        public bool singleStep { get; set; }
    }
}
