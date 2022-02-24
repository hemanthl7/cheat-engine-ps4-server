using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEServerPS4.EventHandler.Request
{
    public class SetBreakPointRequest
    {
        public int Tid { get; set; }
        public IntPtr Debugreg { get; set; }
        public ulong Address { get; set; }
    }
}
