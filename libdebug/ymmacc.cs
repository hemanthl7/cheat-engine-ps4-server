using System;
using System.Runtime.InteropServices;

namespace libdebug
{
	// Token: 0x0200000D RID: 13
	public struct ymmacc
	{
		// Token: 0x0400006E RID: 110
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] ymm_bytes;
	}
}
