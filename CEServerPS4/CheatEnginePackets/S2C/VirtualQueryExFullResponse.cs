using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.S2C
{
    public class VirtualQueryExFullResponse : ICheatEngineResponse
    {

        public IList<PS4API.MemoryAPI.MEMORY_BASIC_INFORMATION> Regions;
        public VirtualQueryExFullResponse(IList<PS4API.MemoryAPI.MEMORY_BASIC_INFORMATION> regions)
        {
            this.Regions = regions;
        }

        public byte[] Serialize()
        {

            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            br.Write(Regions.Count);
            //The number of bytes return by VirtualQueryEx is the number of bytes written to mbi, if it's 0 it failed
            //But in Cheat engise server 1 is success and 0 is failed
            foreach (var region in Regions)
            {//Yes this is reversed from VirtualQueryEx....
                br.Write(region.BaseAddress);
                br.Write(region.RegionSize);
                br.Write((int)region.AllocationProtect); // Or Protect
                br.Write((int)region.Type);
            }

            br.Close();
            return ms.ToArray();
        }
    }
}
