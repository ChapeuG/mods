using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02000034 RID: 52
public class WallPaint_Controller : MonoBehaviour
{
	// Token: 0x060001BB RID: 443 RVA: 0x00010BB0 File Offset: 0x0000EDB0
	private void Start()
	{
		Inv_Manager.instance.AddWallPaintControllers(this);
		this.itemIndex = Inv_Manager.instance.GetItemIndex(base.gameObject);
	}

	// Token: 0x060001BC RID: 444 RVA: 0x00010BD3 File Offset: 0x0000EDD3
	private void Update()
	{
	}

	// Token: 0x060001BD RID: 445 RVA: 0x00010BD5 File Offset: 0x0000EDD5
	public void DestroyThis()
	{
		Inv_Manager.instance.DeleteWallPaintController(this);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04000249 RID: 585
	public int itemIndex;

	// Token: 0x0400024A RID: 586
	[SerializeField]
	public bool buyable;

	// Token: 0x0400024B RID: 587
	[SerializeField]
	public bool randonlyUnlockable;

	// Token: 0x0400024C RID: 588
	[SerializeField]
	public Color[] itemColor;

	// Token: 0x0400024D RID: 589
	[SerializeField]
	public Sprite itemSprite;

	// Token: 0x0400024E RID: 590
	[SerializeField]
	public int itemPrice;

	// Token: 0x0400024F RID: 591
	[SerializeField]
	public MeshRenderer meshRenderer;

	// Token: 0x04000250 RID: 592
	public EventReference event_Material;
}
