using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CEServerPS4.EventHandler.Event
{
    public abstract class DebugThreadEvent
    {
        public Object Data { get; set; }
        public abstract CommandType CommandType { get; }
        public BufferBlock<Object> BufferBlock { get; } = new BufferBlock<Object>();
    }
}
