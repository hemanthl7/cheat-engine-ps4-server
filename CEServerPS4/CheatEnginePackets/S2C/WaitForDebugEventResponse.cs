using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace CEServerPS4.CheatEnginePackets.S2C
{
    public class WaitForDebugEventResponse : ICheatEngineResponse
    {
        private IntPtr handle;
        private object et;


        public WaitForDebugEventResponse(IntPtr handle,object et)
        {
            this.et = et;
            this.handle = handle;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
           
            
            br.Write((int)handle);
            set(ref br);
            br.Close();
            return ms.ToArray();
        }

        private void set(ref BinaryWriter br)
        {
            
            if(et is PS4API.DebugAPI.CreateDebugEvent)
            {
                PS4API.DebugAPI.CreateDebugEvent bf = (PS4API.DebugAPI.CreateDebugEvent)et;
                br.Write(bf.debugevent);
                br.Write(bf.threadid);
                br.Write((ulong)0);
            }
            else if(et is PS4API.DebugAPI.ProcessEvent)
            {
                PS4API.DebugAPI.ProcessEvent pe = (PS4API.DebugAPI.ProcessEvent)et;
                br.Write(pe.debugevent);
                br.Write(pe.threadid);
                br.Write(pe.maxBreakpointCount);
                br.Write(pe.maxWatchpointCount);
                br.Write(pe.maxSharedBreakpoints);
                br.Write((int)0);
                br.Write((sbyte)0);
            }
            else
            {
                PS4API.DebugAPI.DebugEvent de = (PS4API.DebugAPI.DebugEvent)et; 
                br.Write(de.debugevent);
                br.Write(de.threadid);
                br.Write(de.address);
            }
             
            
        }

    }
}
