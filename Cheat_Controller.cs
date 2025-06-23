using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class Cheat_Controller : MonoBehaviour
{
	// Token: 0x06000069 RID: 105 RVA: 0x000056E5 File Offset: 0x000038E5
	private void Start()
	{
		this.StartCheat();
	}

	// Token: 0x0600006A RID: 106 RVA: 0x000056ED File Offset: 0x000038ED
	public void StartCheat()
	{
	}

	// Token: 0x0400008A RID: 138
	[Header("StartCheat")]
	[SerializeField]
	private Shelf_Controller[] start_InvNormal = new Shelf_Controller[4];

	// Token: 0x0400008B RID: 139
	[SerializeField]
	private Shelf_Controller[] start_InvFreezer = new Shelf_Controller[2];

	// Token: 0x0400008C RID: 140
	[SerializeField]
	private Shelf_Controller[] start_ShelfNormal = new Shelf_Controller[6];

	// Token: 0x0400008D RID: 141
	[SerializeField]
	private Shelf_Controller[] start_ShelfFreezer = new Shelf_Controller[2];
}
