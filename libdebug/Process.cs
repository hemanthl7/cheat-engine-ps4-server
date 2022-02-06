using System;

namespace libdebug
{
	// Token: 0x02000002 RID: 2
	public class Process
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public Process(string name, int pid)
		{
			this.name = name;
			this.pid = pid;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002066 File Offset: 0x00000266
		public override string ToString()
		{
			return string.Format("[{0}] {1}", this.pid, this.name);
		}

		// Token: 0x04000001 RID: 1
		public string name;

		// Token: 0x04000002 RID: 2
		public int pid;
	}
}
