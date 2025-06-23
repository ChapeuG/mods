using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class Floor_Controller : MonoBehaviour
{
	// Token: 0x060000B3 RID: 179 RVA: 0x00008518 File Offset: 0x00006718
	private void Start()
	{
		Inv_Manager.instance.AddFloorControllers(this);
		this.itemIndex = Inv_Manager.instance.GetItemIndex(base.gameObject);
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x0000853B File Offset: 0x0000673B
	public void DestroyThis()
	{
		Inv_Manager.instance.DeleteFloorController(this);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040000F0 RID: 240
	[SerializeField]
	public bool buyable;

	// Token: 0x040000F1 RID: 241
	[SerializeField]
	public bool randonlyUnlockable;

	// Token: 0x040000F2 RID: 242
	[SerializeField]
	public Color[] itemColor;

	// Token: 0x040000F3 RID: 243
	[SerializeField]
	public Sprite itemSprite;

	// Token: 0x040000F4 RID: 244
	[SerializeField]
	public int itemPrice;

	// Token: 0x040000F5 RID: 245
	[SerializeField]
	public MeshRenderer meshRenderer;

	// Token: 0x040000F6 RID: 246
	public int itemIndex;

	// Token: 0x040000F7 RID: 247
	public EventReference event_Material;
}
