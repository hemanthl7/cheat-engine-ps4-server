using System;
using System.Runtime.InteropServices;

namespace libdebug
{
	// Token: 0x02000011 RID: 17
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct dbregs
	{
		// Token: 0x04000079 RID: 121
		public ulong dr0;

		// Token: 0x0400007A RID: 122
		public ulong dr1;

		// Token: 0x0400007B RID: 123
		public ulong dr2;

		// Token: 0x0400007C RID: 124
		public ulong dr3;

		// Token: 0x0400007D RID: 125
		public ulong dr4;

		// Token: 0x0400007E RID: 126
		public ulong dr5;

		// Token: 0x0400007F RID: 127
		public ulong dr6;

		// Token: 0x04000080 RID: 128
		public ulong dr7;

		// Token: 0x04000081 RID: 129
		public ulong dr8;

		// Token: 0x04000082 RID: 130
		public ulong dr9;

		// Token: 0x04000083 RID: 131
		public ulong dr10;

		// Token: 0x04000084 RID: 132
		public ulong dr11;

		// Token: 0x04000085 RID: 133
		public ulong dr12;

		// Token: 0x04000086 RID: 134
		public ulong dr13;

		// Token: 0x04000087 RID: 135
		public ulong dr14;

		// Token: 0x04000088 RID: 136
		public ulong dr15;
	}
}
