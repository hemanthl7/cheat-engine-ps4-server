using System;
using System.Runtime.InteropServices;

namespace libdebug
{
	// Token: 0x02000006 RID: 6
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ProcessInfo
	{
		// Token: 0x0400000B RID: 11
		public int pid;

		// Token: 0x0400000C RID: 12
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
		public string name;

		// Token: 0x0400000D RID: 13
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string path;

		// Token: 0x0400000E RID: 14
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string titleid;

		// Token: 0x0400000F RID: 15
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string contentid;
	}
}
