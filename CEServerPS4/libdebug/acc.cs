using System;
using System.Runtime.InteropServices;

namespace libdebug
{
	// Token: 0x0200000B RID: 11
	public struct acc
	{
		// Token: 0x0400006B RID: 107
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public byte[] fp_bytes;

		// Token: 0x0400006C RID: 108
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		private byte[] fp_pad;
	}
}
