using System;
using System.Runtime.InteropServices;

namespace libdebug
{
	// Token: 0x02000009 RID: 9
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct regs
	{
		// Token: 0x04000048 RID: 72
		public ulong r_r15;

		// Token: 0x04000049 RID: 73
		public ulong r_r14;

		// Token: 0x0400004A RID: 74
		public ulong r_r13;

		// Token: 0x0400004B RID: 75
		public ulong r_r12;

		// Token: 0x0400004C RID: 76
		public ulong r_r11;

		// Token: 0x0400004D RID: 77
		public ulong r_r10;

		// Token: 0x0400004E RID: 78
		public ulong r_r9;

		// Token: 0x0400004F RID: 79
		public ulong r_r8;

		// Token: 0x04000050 RID: 80
		public ulong r_rdi;

		// Token: 0x04000051 RID: 81
		public ulong r_rsi;

		// Token: 0x04000052 RID: 82
		public ulong r_rbp;

		// Token: 0x04000053 RID: 83
		public ulong r_rbx;

		// Token: 0x04000054 RID: 84
		public ulong r_rdx;

		// Token: 0x04000055 RID: 85
		public ulong r_rcx;

		// Token: 0x04000056 RID: 86
		public ulong r_rax;

		// Token: 0x04000057 RID: 87
		public uint r_trapno;

		// Token: 0x04000058 RID: 88
		public ushort r_fs;

		// Token: 0x04000059 RID: 89
		public ushort r_gs;

		// Token: 0x0400005A RID: 90
		public uint r_err;

		// Token: 0x0400005B RID: 91
		public ushort r_es;

		// Token: 0x0400005C RID: 92
		public ushort r_ds;

		// Token: 0x0400005D RID: 93
		public ulong r_rip;

		// Token: 0x0400005E RID: 94
		public ulong r_cs;

		// Token: 0x0400005F RID: 95
		public ulong r_rflags;

		// Token: 0x04000060 RID: 96
		public ulong r_rsp;

		// Token: 0x04000061 RID: 97
		public ulong r_ss;
	}
}
