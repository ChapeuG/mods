using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004D RID: 77
[Serializable]
public class TaskSaveData
{
	// Token: 0x0400048A RID: 1162
	[SerializeField]
	public int state;

	// Token: 0x0400048B RID: 1163
	[SerializeField]
	public int daysCurrent;

	// Token: 0x0400048C RID: 1164
	[SerializeField]
	public int needsCurrent;

	// Token: 0x0400048D RID: 1165
	[SerializeField]
	public int needsQnt;

	// Token: 0x0400048E RID: 1166
	[SerializeField]
	public List<int> prod_Indexes;

	// Token: 0x0400048F RID: 1167
	[SerializeField]
	public string[] tags;

	// Token: 0x04000490 RID: 1168
	[SerializeField]
	public string[] values;
}
