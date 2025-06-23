using System;
using System.Collections.Generic;

// Token: 0x02000039 RID: 57
[Serializable]
public class LocalCustomer_Data
{
	// Token: 0x040002E3 RID: 739
	public List<bool> prodPreferenceUnlocked = new List<bool>();

	// Token: 0x040002E4 RID: 740
	public int[] prodQntForFriendship = new int[0];

	// Token: 0x040002E5 RID: 741
	public int[] decorsQntForFriendship = new int[0];

	// Token: 0x040002E6 RID: 742
	public int[] freeProdQntForFriendship = new int[0];

	// Token: 0x040002E7 RID: 743
	public int actionsForFriendship;

	// Token: 0x040002E8 RID: 744
	public int hairColorIndex = -1;

	// Token: 0x040002E9 RID: 745
	public int hairCutIndex = -1;

	// Token: 0x040002EA RID: 746
	public int hairChangeVisitIndex;

	// Token: 0x040002EB RID: 747
	public int visitIndex;
}
