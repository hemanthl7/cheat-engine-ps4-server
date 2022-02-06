using System;
using System.Runtime.InteropServices;

namespace libdebug
{
	// Token: 0x02000010 RID: 16
	[StructLayout(LayoutKind.Sequential, Pack = 64)]
	public struct fpregs
	{
		// Token: 0x04000074 RID: 116
		public envxmm svn_env;

		// Token: 0x04000075 RID: 117
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public acc[] sv_fp;

		// Token: 0x04000076 RID: 118
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public xmmacc[] sv_xmm;

		// Token: 0x04000077 RID: 119
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
		private byte[] sv_pad;

		// Token: 0x04000078 RID: 120
		public savefpu_xstate sv_xstate;
	}
}
