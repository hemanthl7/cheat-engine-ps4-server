using System;
using System.Runtime.InteropServices;

namespace libdebug
{
	// Token: 0x0200000E RID: 14
	public struct xstate_hdr
	{
		// Token: 0x0400006F RID: 111
		public ulong xstate_bv;

		// Token: 0x04000070 RID: 112
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		private byte[] xstate_rsrv0;

		// Token: 0x04000071 RID: 113
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
		private byte[] xstate_rsrv;
	}
}
