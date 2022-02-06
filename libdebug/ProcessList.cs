using System;

namespace libdebug
{
	// Token: 0x02000003 RID: 3
	public class ProcessList
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002084 File Offset: 0x00000284
		public ProcessList(int number, string[] names, int[] pids)
		{
			this.processes = new Process[number];
			for (int i = 0; i < number; i++)
			{
				this.processes[i] = new Process(names[i], pids[i]);
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020C4 File Offset: 0x000002C4
		public Process FindProcess(string name, bool contains = false)
		{
			foreach (Process process in this.processes)
			{
				if (contains)
				{
					if (process.name.Contains(name))
					{
						return process;
					}
				}
				else if (process.name == name)
				{
					return process;
				}
			}
			return null;
		}

		// Token: 0x04000003 RID: 3
		public Process[] processes;
	}
}
