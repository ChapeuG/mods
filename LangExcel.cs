using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000003 RID: 3
[ExcelAsset(LogOnImport = true)]
public class LangExcel : ScriptableObject
{
	// Token: 0x0400000B RID: 11
	public List<LangClass> Languages;

	// Token: 0x0400000C RID: 12
	public List<LangClass> UI;

	// Token: 0x0400000D RID: 13
	public List<LangClass> ItemNames;

	// Token: 0x0400000E RID: 14
	public List<LangClass> Messages;
}
