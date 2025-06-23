using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

// Token: 0x02000032 RID: 50
public class Util_Controller : MonoBehaviour
{
	// Token: 0x060001AB RID: 427 RVA: 0x000105C8 File Offset: 0x0000E7C8
	private void Awake()
	{
		if (this.needsSource)
		{
			this.shelfController = base.gameObject.AddComponent<Shelf_Controller>();
			this.shelfController.utilController = this;
			this.shelfController.isShelfInv = true;
			this.shelfController.isShelfProd = false;
			this.shelfController.boxPlace = new GameObject[]
			{
				this.boxPlace
			};
			this.shelfController.prodPlace = new GameObject[1, 1];
			this.shelfController.prodPlace[0, 0] = this.boxPlace;
			this.shelfController.prodControllers = new Prod_Controller[1, 1];
			this.shelfController.boxControllers = new Box_Controller[1];
			this.shelfController.discountPapers = new DiscountPaper_Controller[1];
			this.shelfController.height = 1;
			this.shelfController.width = 1;
		}
	}

	// Token: 0x060001AC RID: 428 RVA: 0x000106A5 File Offset: 0x0000E8A5
	private void Start()
	{
		if (Inv_Manager.instance)
		{
			Inv_Manager.instance.AddUtilControllers(this);
			this.utilIndex = Inv_Manager.instance.GetItemIndex(base.gameObject);
		}
		bool flag = this.isJukebox;
	}

	// Token: 0x060001AD RID: 429 RVA: 0x000106DB File Offset: 0x0000E8DB
	private void Update()
	{
	}

	// Token: 0x060001AE RID: 430 RVA: 0x000106E0 File Offset: 0x0000E8E0
	public void GetThisUtilBox(Player_Controller _player, bool _garbage)
	{
		Box_Controller box_Controller = Inv_Manager.instance.CreateBox_Util(this.utilIndex, 1);
		box_Controller.isGarbage = _garbage;
		if (_player.HoldOrChangeBox(box_Controller))
		{
			Inv_Manager.instance.DeleteUtilController(this);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		box_Controller.DeleteBox(_player.playerIndex);
	}

	// Token: 0x060001AF RID: 431 RVA: 0x00010734 File Offset: 0x0000E934
	public void GetThisUtilBox(Staff_Controller _staff, bool _garbage)
	{
		Box_Controller box_Controller = Inv_Manager.instance.CreateBox_Util(this.utilIndex, 1);
		box_Controller.isGarbage = _garbage;
		if (_staff.HoldOrChangeBox(box_Controller))
		{
			Inv_Manager.instance.DeleteUtilController(this);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		box_Controller.DeleteBox(0);
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00010784 File Offset: 0x0000E984
	public bool GetIsEmpty()
	{
		bool result = true;
		if (this.shelfController)
		{
			result = this.shelfController.GetIsEmpty();
		}
		return result;
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x000107B0 File Offset: 0x0000E9B0
	public bool CheckItemAcceptance(Box_Controller _box)
	{
		bool result = false;
		if (_box.isProd)
		{
			Prod_Controller prodPrefab = Inv_Manager.instance.GetProdPrefab(_box.itemIndex);
			int itemIndex = Inv_Manager.instance.GetItemIndex(prodPrefab.gameObject);
			for (int i = 0; i < this.prodAcccepted.Count; i++)
			{
				if (this.prodAcccepted[i] && itemIndex == Inv_Manager.instance.GetItemIndex(this.prodAcccepted[i].gameObject))
				{
					return true;
				}
			}
			for (int j = 0; j < this.prodTypes.Length; j++)
			{
				if (this.prodTypes[j] != Inv_Manager.ProdType.Null && prodPrefab.prodType == this.prodTypes[j])
				{
					return true;
				}
			}
		}
		return result;
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x0001086C File Offset: 0x0000EA6C
	public bool CheckPersonMayUse(Util_Controller.MayUse _personType)
	{
		bool result = false;
		for (int i = 0; i < this.mayUse.Length; i++)
		{
			if (_personType != Util_Controller.MayUse.Null && _personType == this.mayUse[i])
			{
				return true;
			}
		}
		return result;
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x0001089F File Offset: 0x0000EA9F
	public void Interact(bool _holdingButton, int _buttonIndex, Player_Controller _player)
	{
		if (this.needsSource)
		{
			this.shelfController.Interact(_holdingButton, _buttonIndex, _player);
		}
		if (this.isJukebox)
		{
			Audio_Manager.instance.ChangeMusic_RandomList_Next();
		}
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x000108CC File Offset: 0x0000EACC
	public bool Interact(Customer_Controller _cust)
	{
		bool isBroken = base.GetComponent<Interaction_Controller>().isBroken;
		bool result;
		if (this.needsSource)
		{
			if (!this.shelfController.boxControllers[0])
			{
				return false;
			}
			this.shelfController.ChangeBox(0, -1, 0);
			base.gameObject.GetComponent<Animator>().SetTrigger("Shake");
			result = true;
		}
		else
		{
			base.gameObject.GetComponent<Animator>().SetTrigger("Shake");
			base.gameObject.GetComponent<Interaction_Controller>().Play_Particles();
			result = true;
		}
		if (this.isJukebox && this.dayIndex != World_Manager.instance.GetDayIndex())
		{
			this.dayIndex = World_Manager.instance.GetDayIndex();
		}
		return result;
	}

	// Token: 0x04000235 RID: 565
	public int utilIndex;

	// Token: 0x04000236 RID: 566
	[SerializeField]
	public bool buyable;

	// Token: 0x04000237 RID: 567
	[SerializeField]
	public bool randonlyUnlockable;

	// Token: 0x04000238 RID: 568
	[SerializeField]
	public Color[] itemColor;

	// Token: 0x04000239 RID: 569
	[SerializeField]
	public Sprite itemSprite;

	// Token: 0x0400023A RID: 570
	[SerializeField]
	public int itemPrice;

	// Token: 0x0400023B RID: 571
	public bool needsSource;

	// Token: 0x0400023C RID: 572
	public bool isJukebox;

	// Token: 0x0400023D RID: 573
	private int dayIndex = -1;

	// Token: 0x0400023E RID: 574
	[SerializeField]
	public GameObject boxPlace;

	// Token: 0x0400023F RID: 575
	[HideInInspector]
	public Shelf_Controller shelfController;

	// Token: 0x04000240 RID: 576
	public EventReference event_Material;

	// Token: 0x04000241 RID: 577
	[SerializeField]
	public Util_Controller.MayUse[] mayUse;

	// Token: 0x04000242 RID: 578
	[Header("Acceptance")]
	[SerializeField]
	public List<Prod_Controller> prodAcccepted = new List<Prod_Controller>();

	// Token: 0x04000243 RID: 579
	[SerializeField]
	public Inv_Manager.ProdType[] prodTypes = new Inv_Manager.ProdType[0];

	// Token: 0x02000075 RID: 117
	public enum MayUse
	{
		// Token: 0x0400068F RID: 1679
		Null,
		// Token: 0x04000690 RID: 1680
		Everyone,
		// Token: 0x04000691 RID: 1681
		Player,
		// Token: 0x04000692 RID: 1682
		AnyCustomer,
		// Token: 0x04000693 RID: 1683
		GenericCustomers,
		// Token: 0x04000694 RID: 1684
		LocalCustomers
	}
}
