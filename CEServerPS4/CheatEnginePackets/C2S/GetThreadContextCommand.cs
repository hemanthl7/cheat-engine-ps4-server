using CEServerPS4.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.C2S
{
    public class GetThreadContextCommand : CheatEngineCommand<ThreadContextResponse>
    {
        public IntPtr Handle;
        public uint tid;
        public int type;
       
        public override CommandType CommandType => CommandType.CMD_GETTHREADCONTEXT;

        public GetThreadContextCommand() { }

        public GetThreadContextCommand(IntPtr handle, uint tid, int type)
        {
            this.Handle = handle;
            this.tid = tid;
            this.type = type;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            Handle = (IntPtr)reader.ReadInt32();
            tid = reader.ReadUInt32();
            type = reader.ReadInt32();    
            this.initialized = true;
        }

        public override ThreadContextResponse Process()
        {
            
            IntPtr ptr = PS4API.DebugAPI.GetThreadContext(Handle, tid, out PS4API.DebugAPI.CONTEXT Context, type);
            return new ThreadContextResponse(Context.regs, ptr);
        }
    }
}
