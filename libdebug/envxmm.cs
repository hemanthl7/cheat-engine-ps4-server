using System;

namespace libdebug
{
	// Token: 0x0200000A RID: 10
	public struct envxmm
	{
		// Token: 0x04000062 RID: 98
		public ushort en_cw;

		// Token: 0x04000063 RID: 99
		public ushort en_sw;

		// Token: 0x04000064 RID: 100
		public byte en_tw;

		// Token: 0x04000065 RID: 101
		public byte en_zero;

		// Token: 0x04000066 RID: 102
		public ushort en_opcode;

		// Token: 0x04000067 RID: 103
		public ulong en_rip;

		// Token: 0x04000068 RID: 104
		public ulong en_rdp;

		// Token: 0x04000069 RID: 105
		public uint en_mxcsr;

		// Token: 0x0400006A RID: 106
		public uint en_mxcsr_mask;
	}
}
