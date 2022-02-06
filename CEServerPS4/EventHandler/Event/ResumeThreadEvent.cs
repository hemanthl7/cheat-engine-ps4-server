using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using CEServerPS4.EventHandler.Request;

namespace CEServerPS4.EventHandler.Event
{
    public class ResumeThreadEvent : DebugThreadEvent
    {
        public override CommandType CommandType => CommandType.CMD_RESUMETHREAD;
    }
}
