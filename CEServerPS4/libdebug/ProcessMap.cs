using System;

namespace libdebug
{
	// Token: 0x02000005 RID: 5
	public class ProcessMap
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002116 File Offset: 0x00000316
		public ProcessMap(int pid, MemoryEntry[] entries)
		{
			this.pid = pid;
			this.entries = entries;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000212C File Offset: 0x0000032C
		public MemoryEntry FindEntry(string name, bool contains = false)
		{
			foreach (MemoryEntry memoryEntry in this.entries)
			{
				if (contains)
				{
					if (memoryEntry.name.Contains(name))
					{
						return memoryEntry;
					}
				}
				else if (memoryEntry.name == name)
				{
					return memoryEntry;
				}
			}
			return null;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002178 File Offset: 0x00000378
		public MemoryEntry FindEntry(ulong size)
		{
			foreach (MemoryEntry memoryEntry in this.entries)
			{
				if (memoryEntry.start - memoryEntry.end == size)
				{
					return memoryEntry;
				}
			}
			return null;
		}

		// Token: 0x04000009 RID: 9
		public int pid;

		// Token: 0x0400000A RID: 10
		public MemoryEntry[] entries;
	}
}
