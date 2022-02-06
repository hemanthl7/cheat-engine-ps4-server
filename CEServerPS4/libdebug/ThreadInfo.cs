using System;
using System.Runtime.InteropServices;

namespace libdebug
{
	// Token: 0x02000007 RID: 7
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ThreadInfo
	{
		// Token: 0x04000010 RID: 16
		public int pid;

		// Token: 0x04000011 RID: 17
		public int priority;

		// Token: 0x04000012 RID: 18
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string name;
	}
}
