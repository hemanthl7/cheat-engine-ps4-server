using System;
using System.Runtime.InteropServices;

namespace libdebug
{
	// Token: 0x0200000F RID: 15
	public struct savefpu_xstate
	{
		// Token: 0x04000072 RID: 114
		public xstate_hdr sx_hd;

		// Token: 0x04000073 RID: 115
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public ymmacc[] sx_ymm;
	}
}
