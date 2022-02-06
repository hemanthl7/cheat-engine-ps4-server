using System;
using System.Runtime.InteropServices;

namespace libdebug
{
	// Token: 0x0200000C RID: 12
	public struct xmmacc
	{
		// Token: 0x0400006D RID: 109
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] xmm_bytes;
	}
}
