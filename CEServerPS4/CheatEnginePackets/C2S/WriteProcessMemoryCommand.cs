using CEServerPS4.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerPS4.CheatEnginePackets.C2S
{
    public class WriteProcessMemoryCommand : CheatEngineCommand<WriteProcessMemoryResponse>
    {
        public IntPtr Handle;
        public ulong Address;
        public int Size;
        public byte[] data;

        public override CommandType CommandType => CommandType.CMD_WRITEPROCESSMEMORY;

        public WriteProcessMemoryCommand() { }

        public WriteProcessMemoryCommand(IntPtr handle, UInt64 address, int size, byte[] data)
        {
            this.Handle = handle;
            this.Address = address;
            this.Size = size;
            this.data = data;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            Handle = (IntPtr)reader.ReadInt32();
            Address = reader.ReadUInt64();
            Size = reader.ReadInt32();
            data = reader.ReadBytes(Size);
            this.initialized = true;
        }

        public override WriteProcessMemoryResponse Process()
        {

            IntPtr dataWriten;
            PS4API.MemoryAPI.WriteMemory(this.Handle, this.Address, data, this.Size, out dataWriten);

            return new WriteProcessMemoryResponse(this.Size);
        }
    }
}
