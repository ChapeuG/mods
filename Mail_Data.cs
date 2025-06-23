using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000046 RID: 70
[Serializable]
public class Mail_Data
{
	// Token: 0x040003C9 RID: 969
	[SerializeField]
	public int owner_index;

	// Token: 0x040003CA RID: 970
	[SerializeField]
	public int owner_cat;

	// Token: 0x040003CB RID: 971
	[SerializeField]
	public int category;

	// Token: 0x040003CC RID: 972
	[SerializeField]
	public int type;

	// Token: 0x040003CD RID: 973
	[SerializeField]
	public string title = "";

	// Token: 0x040003CE RID: 974
	[SerializeField]
	public string message = "";

	// Token: 0x040003CF RID: 975
	[SerializeField]
	public int date;

	// Token: 0x040003D0 RID: 976
	[SerializeField]
	public int time_to_expire = -1;

	// Token: 0x040003D1 RID: 977
	[SerializeField]
	public bool opened;

	// Token: 0x040003D2 RID: 978
	[SerializeField]
	public int task_state;

	// Token: 0x040003D3 RID: 979
	[SerializeField]
	public int task_index;

	// Token: 0x040003D4 RID: 980
	[SerializeField]
	public bool task_procedural;

	// Token: 0x040003D5 RID: 981
	[SerializeField]
	public string[] task_tag;

	// Token: 0x040003D6 RID: 982
	[SerializeField]
	public string[] task_tag_value;

	// Token: 0x040003D7 RID: 983
	[SerializeField]
	public List<int> reward_cats = new List<int>();

	// Token: 0x040003D8 RID: 984
	[SerializeField]
	public List<int> reward_indexes = new List<int>();

	// Token: 0x040003D9 RID: 985
	[SerializeField]
	public bool has_staff_data;

	// Token: 0x040003DA RID: 986
	[SerializeField]
	public Staff_Data staff_data;
}
