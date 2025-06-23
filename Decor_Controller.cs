using System;
using FMODUnity;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class Decor_Controller : MonoBehaviour
{
	// Token: 0x06000098 RID: 152 RVA: 0x00007B2A File Offset: 0x00005D2A
	private void Start()
	{
		if (Inv_Manager.instance)
		{
			Inv_Manager.instance.AddDecorControllers(this);
			this.decorIndex = Inv_Manager.instance.GetItemIndex(base.gameObject);
		}
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00007B5C File Offset: 0x00005D5C
	public void GetThisDecorBox(Player_Controller _player, bool _garbage, Plant_Controller _plant)
	{
		int lifeSpanIndex = 0;
		if (_plant)
		{
			lifeSpanIndex = _plant.GetLifeSpanIndex();
		}
		Box_Controller box_Controller = Inv_Manager.instance.CreateBox_Decor(this.decorIndex, 1, lifeSpanIndex);
		box_Controller.isGarbage = _garbage;
		if (_player.HoldOrChangeBox(box_Controller))
		{
			Inv_Manager.instance.DeleteDecorController(this);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		box_Controller.DeleteBox(_player.playerIndex);
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00007BC0 File Offset: 0x00005DC0
	public void GetThisDecorBox(Staff_Controller _staff, bool _garbage, Plant_Controller _plant)
	{
		int lifeSpanIndex = 0;
		if (_plant)
		{
			lifeSpanIndex = _plant.GetLifeSpanIndex();
		}
		Box_Controller box_Controller = Inv_Manager.instance.CreateBox_Decor(this.decorIndex, 1, lifeSpanIndex);
		box_Controller.isGarbage = _garbage;
		if (_staff.HoldOrChangeBox(box_Controller))
		{
			Inv_Manager.instance.DeleteDecorController(this);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		box_Controller.DeleteBox(0);
	}

	// Token: 0x040000C5 RID: 197
	[SerializeField]
	public bool buyable;

	// Token: 0x040000C6 RID: 198
	[SerializeField]
	public bool randonlyUnlockable;

	// Token: 0x040000C7 RID: 199
	[SerializeField]
	public Color[] itemColor;

	// Token: 0x040000C8 RID: 200
	[SerializeField]
	public Sprite itemSprite;

	// Token: 0x040000C9 RID: 201
	[SerializeField]
	public int itemPrice;

	// Token: 0x040000CA RID: 202
	public int decorIndex;

	// Token: 0x040000CB RID: 203
	public EventReference event_Material;
}
