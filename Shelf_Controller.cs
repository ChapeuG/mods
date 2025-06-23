using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class Shelf_Controller : MonoBehaviour
{
	// Token: 0x06000151 RID: 337 RVA: 0x0000D978 File Offset: 0x0000BB78
	private void Awake()
	{
		if (this.utilController)
		{
			return;
		}
		this.CreateReferences();
	}

	// Token: 0x06000152 RID: 338 RVA: 0x0000D990 File Offset: 0x0000BB90
	private void Start()
	{
		if (this.isShelfProd)
		{
			if (Inv_Manager.instance)
			{
				Inv_Manager.instance.AddShelfProdControllers(this);
				this.shelfIndex = Inv_Manager.instance.GetItemIndex(base.gameObject);
				return;
			}
		}
		else if (this.isShelfInv && Inv_Manager.instance && !this.utilController)
		{
			Inv_Manager.instance.AddShelfInvControllers(this);
		}
	}

	// Token: 0x06000153 RID: 339 RVA: 0x0000DA00 File Offset: 0x0000BC00
	public void CreateReferences()
	{
		this.boxPlace = new GameObject[this.height];
		this.prodPlace = new GameObject[this.height, this.width];
		for (int i = 0; i < this.height; i++)
		{
			this.boxPlace[i] = this.shelfParent.transform.Find("Shelf (" + i.ToString() + ")").gameObject;
			for (int j = 0; j < this.width; j++)
			{
				this.prodPlace[i, j] = this.boxPlace[i].transform.Find("ItemPlace (" + j.ToString() + ")").gameObject;
			}
		}
		for (int k = 0; k < this.height; k++)
		{
		}
		this.prodControllers = new Prod_Controller[this.height, this.width];
		this.boxControllers = new Box_Controller[this.height];
		this.discountPapers = new DiscountPaper_Controller[this.height];
	}

	// Token: 0x06000154 RID: 340 RVA: 0x0000DB14 File Offset: 0x0000BD14
	public void GetThisShelfBox(Player_Controller _player)
	{
		_player.HoldOrChangeBox(Inv_Manager.instance.CreateBox_Shelf(this.shelfIndex, 1));
		Inv_Manager.instance.DeleteShelfProdController(this);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06000155 RID: 341 RVA: 0x0000DB44 File Offset: 0x0000BD44
	public void Interact(bool _holdingButton, int _buttonIndex, Player_Controller _player)
	{
		if (_buttonIndex >= this.height)
		{
			return;
		}
		if (this.isShelfInv)
		{
			if (_player.discountPaper_Controller)
			{
				return;
			}
			if (_player.boxController)
			{
				if (!this.boxControllers[_buttonIndex])
				{
					this.StoreBox(_buttonIndex, _player);
					Input_Manager.instance.SetVibration();
					Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.store_Boxes_Inventory, -1);
				}
				else if (this.boxControllers[_buttonIndex].itemIndex == _player.boxController.itemIndex && this.boxControllers[_buttonIndex].prodQnt < 8)
				{
					if (!_holdingButton)
					{
						this.ChangeBox(_buttonIndex, _player);
						Input_Manager.instance.SetVibration();
					}
					else
					{
						this.ChangeBoxSettingQnt(_buttonIndex, 1, _player, _player.boxController.lifeSpanIndex, _player.boxController.frozen);
						Input_Manager.instance.SetVibration();
					}
				}
			}
			else if (this.boxControllers[_buttonIndex])
			{
				if (!_holdingButton)
				{
					this.GiveBox(_buttonIndex, _player);
					Input_Manager.instance.SetVibration();
				}
				else
				{
					this.ChangeBoxSettingQnt(_buttonIndex, 1, _player, this.boxControllers[_buttonIndex].lifeSpanIndex, _player.boxController.frozen);
					Input_Manager.instance.SetVibration();
				}
			}
		}
		else if (this.isShelfProd)
		{
			if (_player.boxController)
			{
				if (_player.boxController.isShelf || _player.boxController.isDecor || _player.boxController.isUtil)
				{
					return;
				}
				if (!this.prodControllers[_buttonIndex, 0])
				{
					this.StoreProd(_buttonIndex, _player.boxController, _player.playerIndex);
					Input_Manager.instance.SetVibration();
				}
				else if (this.prodControllers[_buttonIndex, 0].prodIndex == _player.boxController.itemIndex)
				{
					if (this.GetAvailableProdSpaces(_buttonIndex) > 0)
					{
						this.StoreProd(_buttonIndex, _player.boxController, _player.playerIndex);
						Input_Manager.instance.SetVibration();
					}
					else
					{
						this.GiveProd(_buttonIndex, _player);
						Input_Manager.instance.SetVibration();
					}
				}
			}
			else if (_player.discountPaper_Controller)
			{
				DiscountPaper_Controller discountPaper_Controller = _player.discountPaper_Controller;
				if (this.prodControllers[_buttonIndex, 0])
				{
					if (discountPaper_Controller.GetDiscountLevel() == -1)
					{
						this.RemoveProdDiscount(_buttonIndex);
					}
					else if (Inv_Manager.instance.GetProdDiscountLevel(this.prodControllers[_buttonIndex, 0].prodIndex) == discountPaper_Controller.GetDiscountLevel())
					{
						this.RemoveProdDiscount(_buttonIndex);
					}
					else
					{
						if (this.prodControllers[_buttonIndex, 0] == null)
						{
							return;
						}
						Inv_Manager.instance.Select_Discount_Level(this.prodControllers[_buttonIndex, 0].prodIndex, discountPaper_Controller.GetDiscountLevel());
					}
				}
			}
			else if (this.prodControllers[_buttonIndex, 0])
			{
				this.GiveProd(_buttonIndex, _player);
				Input_Manager.instance.SetVibration();
			}
		}
		Interactor_Manager.instance.RefreshInteractorType(_player.playerIndex);
	}

	// Token: 0x06000156 RID: 342 RVA: 0x0000DE49 File Offset: 0x0000C049
	public void StoreBox(int _buttonIndex, Player_Controller _player)
	{
		this.boxControllers[_buttonIndex] = _player.boxController;
		_player.StoreBox(this.prodPlace[_buttonIndex, 0], this);
	}

	// Token: 0x06000157 RID: 343 RVA: 0x0000DE6D File Offset: 0x0000C06D
	public void StoreBox(int _buttonIndex, Staff_Controller _staff)
	{
		this.boxControllers[_buttonIndex] = _staff.boxController;
		_staff.StoreBox(this.prodPlace[_buttonIndex, 0], this);
	}

	// Token: 0x06000158 RID: 344 RVA: 0x0000DE91 File Offset: 0x0000C091
	public void StoreBox(int _buttonIndex, Box_Controller _box)
	{
		this.boxControllers[_buttonIndex] = _box;
		_box.HoldBox(this.boxPlace[_buttonIndex], false, this, null);
	}

	// Token: 0x06000159 RID: 345 RVA: 0x0000DEB0 File Offset: 0x0000C0B0
	public void ChangeBox(int _buttonIndex, Player_Controller _player)
	{
		int lifeSpanIndex = _player.boxController.lifeSpanIndex;
		int num = Mathf.Clamp(8 - this.boxControllers[_buttonIndex].prodQnt, 0, _player.boxController.prodQnt);
		_player.boxController.ChangeQnt(-num, _player.boxController.lifeSpanIndex, _player.playerIndex);
		this.boxControllers[_buttonIndex].ChangeQnt(num, lifeSpanIndex, _player.playerIndex);
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0000DF1D File Offset: 0x0000C11D
	public void ChangeBox(int _buttonIndex, int _qnt, int _player_index)
	{
		this.boxControllers[_buttonIndex].ChangeQnt(_qnt, this.boxControllers[_buttonIndex].lifeSpanIndex, _player_index);
		if (this.boxControllers[_buttonIndex] == null)
		{
			this.boxControllers[_buttonIndex] = null;
		}
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0000DF54 File Offset: 0x0000C154
	public void ChangeBoxSettingQnt(int _buttonIndex, int _qnt, Player_Controller _player, int _lifeSpan, bool _frozen)
	{
		if (!_player.boxController)
		{
			Box_Controller box = null;
			if (this.boxControllers[_buttonIndex].isShelf)
			{
				box = Inv_Manager.instance.CreateBox_Shelf(this.boxControllers[_buttonIndex].itemIndex, 1);
			}
			if (this.boxControllers[_buttonIndex].isDecor)
			{
				box = Inv_Manager.instance.CreateBox_Decor(this.boxControllers[_buttonIndex].itemIndex, 1, _lifeSpan);
			}
			if (this.boxControllers[_buttonIndex].isWall)
			{
				box = Inv_Manager.instance.CreateBox_Wall(this.boxControllers[_buttonIndex].itemIndex, 1);
			}
			if (this.boxControllers[_buttonIndex].isFloor)
			{
				box = Inv_Manager.instance.CreateBox_Floor(this.boxControllers[_buttonIndex].itemIndex, 1);
			}
			if (this.boxControllers[_buttonIndex].isProd)
			{
				box = Inv_Manager.instance.CreateBox_Prod(this.boxControllers[_buttonIndex].itemIndex, 1, _lifeSpan, _frozen);
			}
			if (this.boxControllers[_buttonIndex].isUtil)
			{
				box = Inv_Manager.instance.CreateBox_Util(this.boxControllers[_buttonIndex].itemIndex, 1);
			}
			_player.HoldOrChangeBox(box);
		}
		else
		{
			_player.boxController.ChangeQnt(_qnt, _lifeSpan, _player.playerIndex);
		}
		this.boxControllers[_buttonIndex].ChangeQnt(-_qnt, this.boxControllers[_buttonIndex].lifeSpanIndex, _player.playerIndex);
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0000E0A9 File Offset: 0x0000C2A9
	public void GiveBox(int _buttonIndex, Player_Controller _player)
	{
		_player.HoldOrChangeBox(this.boxControllers[_buttonIndex]);
		this.boxControllers[_buttonIndex] = null;
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0000E0C3 File Offset: 0x0000C2C3
	public void GiveBox(int _place, Staff_Controller _staff)
	{
		if (!_staff.HoldOrChangeBox(this.boxControllers[_place]))
		{
			return;
		}
		this.boxControllers[_place] = null;
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0000E0DF File Offset: 0x0000C2DF
	public void DeleteBox(int _buttonIndex)
	{
		if (this.boxControllers[_buttonIndex])
		{
			UnityEngine.Object.Destroy(this.boxControllers[_buttonIndex].gameObject);
		}
		this.boxControllers[_buttonIndex] = null;
	}

	// Token: 0x0600015F RID: 351 RVA: 0x0000E10C File Offset: 0x0000C30C
	public void StoreProd(int _buttonIndex, Box_Controller _box, int _player_index)
	{
		Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.place_Prods, -1);
		int itemIndex = _box.itemIndex;
		int availableProdSpaces = this.GetAvailableProdSpaces(_buttonIndex);
		int num = Mathf.Clamp(_box.prodQnt, 0, availableProdSpaces);
		if (num > 0)
		{
			for (int i = this.width - availableProdSpaces; i < this.width; i++)
			{
				if (num > 0)
				{
					Prod_Controller prod_Controller = Inv_Manager.instance.CreateProd(itemIndex, true, _box.lifeSpanIndex);
					this.prodControllers[_buttonIndex, i] = prod_Controller;
					int lifeSpanIndex = _box.lifeSpanIndex;
					if (i > 0)
					{
						int lifeSpanIndex2 = this.prodControllers[_buttonIndex, i].lifeSpanIndex;
					}
					prod_Controller.HoldProd(this.prodPlace[_buttonIndex, i], this, _box.lifeSpanIndex);
					_box.ChangeQnt(-1, _box.lifeSpanIndex, _player_index);
					num--;
				}
			}
			this.RefreshDiscountPaper();
			EventReference event_PlaceLight = Audio_Manager.instance.event_PlaceLight;
			Audio_Manager.instance.PlaySound(event_PlaceLight, base.transform, _box.event_Material);
		}
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000E208 File Offset: 0x0000C408
	public void StoreProd(int _buttonIndex, Prod_Controller[] _prod)
	{
		for (int i = 0; i < Mathf.Clamp(_prod.Length, this.GetAvailableProdSpaces(_buttonIndex), this.width); i++)
		{
			if (i < _prod.Length)
			{
				this.prodControllers[_buttonIndex, i] = _prod[i];
				_prod[i].HoldProd(this.prodPlace[_buttonIndex, i], this, _prod[i].lifeSpanIndex);
			}
		}
		this.RefreshDiscountPaper();
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0000E270 File Offset: 0x0000C470
	public void StoreProdForTestingOnly(int _buttonIndex, Prod_Controller[] _prod)
	{
		for (int i = 0; i < Mathf.Clamp(_prod.Length, this.GetAvailableProdSpaces(_buttonIndex), this.width); i++)
		{
			this.prodControllers[_buttonIndex, i] = _prod[i];
			_prod[i].HoldProd(this.prodPlace[_buttonIndex, i], this, _prod[i].lifeSpanIndex);
		}
	}

	// Token: 0x06000162 RID: 354 RVA: 0x0000E2CC File Offset: 0x0000C4CC
	public int GetAvailableProdSpaces(int _buttonIndex)
	{
		int num = 0;
		for (int i = 0; i < this.width; i++)
		{
			if (!this.prodControllers[_buttonIndex, i])
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000163 RID: 355 RVA: 0x0000E308 File Offset: 0x0000C508
	public int FindProdPlace(int _prodIndex)
	{
		for (int i = 0; i < this.height; i++)
		{
			if (this.prodControllers[i, 0] && this.prodControllers[i, 0].prodIndex == _prodIndex && this.GetAvailableProdSpaces(i) > 0)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000164 RID: 356 RVA: 0x0000E35C File Offset: 0x0000C55C
	public int FindEmptyPlace()
	{
		for (int i = 0; i < this.height; i++)
		{
			if (!this.prodControllers[i, 0])
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000165 RID: 357 RVA: 0x0000E394 File Offset: 0x0000C594
	public int FindProdBox(int _prodIndex)
	{
		if (!this.isShelfInv)
		{
			return -1;
		}
		for (int i = 0; i < this.height; i++)
		{
			Box_Controller box_Controller = this.boxControllers[i];
			if (!(box_Controller == null) && box_Controller.isProd && box_Controller.itemIndex == _prodIndex)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000166 RID: 358 RVA: 0x0000E3E4 File Offset: 0x0000C5E4
	public bool HasProdBox()
	{
		if (!this.isShelfInv)
		{
			return false;
		}
		for (int i = 0; i < this.height; i++)
		{
			Box_Controller box_Controller = this.boxControllers[i];
			if (!(box_Controller == null) && box_Controller.isProd)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0000E42C File Offset: 0x0000C62C
	public void GiveProd(int _buttonIndex, Player_Controller _player)
	{
		if (!this.prodControllers[_buttonIndex, 0])
		{
			return;
		}
		int num = this.width - this.GetAvailableProdSpaces(_buttonIndex);
		if (_player.boxController)
		{
			int max = Inv_Manager.instance.boxSize - _player.boxController.prodQnt;
			int num2 = Mathf.Clamp(num, 0, max);
			for (int i = this.width - 1; i >= this.width - num2; i--)
			{
				if (this.prodControllers[_buttonIndex, i])
				{
					_player.boxController.ChangeQnt(1, this.prodControllers[_buttonIndex, i].lifeSpanIndex, _player.playerIndex);
					Inv_Manager.instance.Check_DiscountedProdQntForPurchase(this.prodControllers[_buttonIndex, i].prodIndex);
					this.prodControllers[_buttonIndex, i].DeleteProd();
					this.prodControllers[_buttonIndex, i] = null;
				}
			}
		}
		else
		{
			_player.HoldOrChangeBox(Inv_Manager.instance.CreateBox_Prod(this.prodControllers[_buttonIndex, 0].prodIndex, num, this.prodControllers[_buttonIndex, 0].lifeSpanIndex, false));
			for (int j = 0; j < num; j++)
			{
				if (this.prodControllers[_buttonIndex, j])
				{
					Inv_Manager.instance.Check_DiscountedProdQntForPurchase(this.prodControllers[_buttonIndex, j].prodIndex);
					this.prodControllers[_buttonIndex, j].DeleteProd();
					this.prodControllers[_buttonIndex, j] = null;
				}
			}
		}
		this.RefreshDiscountPaper();
	}

	// Token: 0x06000168 RID: 360 RVA: 0x0000E5C8 File Offset: 0x0000C7C8
	public void GiveProd(int _placeIndex, Staff_Controller _staff)
	{
		if (!this.prodControllers[_placeIndex, 0])
		{
			return;
		}
		int num = this.width - this.GetAvailableProdSpaces(_placeIndex);
		if (_staff.boxController)
		{
			int max = Inv_Manager.instance.boxSize - _staff.boxController.prodQnt;
			int num2 = Mathf.Clamp(num, 0, max);
			for (int i = this.width - 1; i >= this.width - num2; i--)
			{
				if (this.prodControllers[_placeIndex, i])
				{
					_staff.boxController.ChangeQnt(1, this.prodControllers[_placeIndex, i].lifeSpanIndex, 0);
					Inv_Manager.instance.Check_DiscountedProdQntForPurchase(this.prodControllers[_placeIndex, i].prodIndex);
					this.prodControllers[_placeIndex, i].DeleteProd();
					this.prodControllers[_placeIndex, i] = null;
				}
			}
		}
		else
		{
			_staff.HoldOrChangeBox(Inv_Manager.instance.CreateBox_Prod(this.prodControllers[_placeIndex, 0].prodIndex, num, this.prodControllers[_placeIndex, 0].lifeSpanIndex, false));
			for (int j = 0; j < num; j++)
			{
				if (this.prodControllers[_placeIndex, j])
				{
					Inv_Manager.instance.Check_DiscountedProdQntForPurchase(this.prodControllers[_placeIndex, j].prodIndex);
					this.prodControllers[_placeIndex, j].DeleteProd();
					this.prodControllers[_placeIndex, j] = null;
				}
			}
		}
		this.RefreshDiscountPaper();
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0000E75C File Offset: 0x0000C95C
	public bool BuyProd(Prod_Controller _prod)
	{
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				if (this.prodControllers[i, j] && this.prodControllers[i, j] == _prod)
				{
					Inv_Manager.instance.Check_DiscountedProdQntForPurchase(_prod.prodIndex);
					this.DeleteProd(i);
					this.RefreshDiscountPaper();
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600016A RID: 362 RVA: 0x0000E7D8 File Offset: 0x0000C9D8
	public bool DeleteProd(int _buttonIndex)
	{
		for (int i = this.width - 1; i >= 0; i--)
		{
			if (this.prodControllers[_buttonIndex, i])
			{
				this.prodControllers[_buttonIndex, i].DeleteProd();
				this.prodControllers[_buttonIndex, i] = null;
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600016B RID: 363 RVA: 0x0000E82F File Offset: 0x0000CA2F
	public void SetProdDiscount(int _buttonIndex, int _discountLevel, bool _isBlock)
	{
		if (this.prodControllers[_buttonIndex, 0] == null)
		{
			return;
		}
		Inv_Manager.instance.Start_DiscountMenu(this.prodControllers[_buttonIndex, 0].prodIndex);
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0000E863 File Offset: 0x0000CA63
	public void RemoveProdDiscount(int _buttonIndex)
	{
		if (this.prodControllers[_buttonIndex, 0])
		{
			Inv_Manager.instance.SetProdDiscount(this.prodControllers[_buttonIndex, 0].prodIndex, -1, true);
		}
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0000E898 File Offset: 0x0000CA98
	public bool RefreshDiscountPaper()
	{
		for (int i = 0; i < this.height; i++)
		{
			if (this.prodControllers[i, 0])
			{
				int prodDiscountLevel = Inv_Manager.instance.GetProdDiscountLevel(this.prodControllers[i, 0].prodIndex);
				if (prodDiscountLevel >= 0)
				{
					if (prodDiscountLevel != this.GetDiscountPaperLevel(i))
					{
						this.CreateDiscountPaper(i, prodDiscountLevel, false);
					}
				}
				else
				{
					this.DestroyDiscountPaper(i);
				}
			}
			else
			{
				this.DestroyDiscountPaper(i);
			}
		}
		return false;
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0000E914 File Offset: 0x0000CB14
	public bool CreateDiscountPaper(int _buttonIndex, int _discountLevel, bool _isBlock)
	{
		this.DestroyDiscountPaper(_buttonIndex);
		this.discountPapers[_buttonIndex] = Inv_Manager.instance.CreateDiscountPaper(_isBlock);
		this.discountPapers[_buttonIndex].CreatePaper(_discountLevel);
		this.discountPapers[_buttonIndex].HoldPaper(this.discountPaperPlace[_buttonIndex]);
		return true;
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0000E960 File Offset: 0x0000CB60
	public bool DestroyDiscountPaper(int _buttonIndex)
	{
		if (this.discountPapers[_buttonIndex])
		{
			this.discountPapers[_buttonIndex].DestroyPaper();
			this.discountPapers[_buttonIndex] = null;
			return true;
		}
		return false;
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0000E98A File Offset: 0x0000CB8A
	public int GetDiscountPaperLevel(int _index)
	{
		if (!this.discountPapers[_index])
		{
			return -1;
		}
		return this.discountPapers[_index].GetDiscountLevel();
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0000E9AC File Offset: 0x0000CBAC
	public bool GetIsEmpty()
	{
		bool result = true;
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				if (this.isShelfProd && this.prodControllers[i, 0] != null)
				{
					result = false;
				}
				if (this.isShelfInv && this.boxControllers[i] != null)
				{
					result = false;
				}
			}
		}
		return result;
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0000EA18 File Offset: 0x0000CC18
	public bool GetHasAnyEmptyHeight()
	{
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				if (this.prodControllers[i, 0] == null)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0000EA60 File Offset: 0x0000CC60
	public bool Get_HasAnyUndiscountedProd()
	{
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				if (this.prodControllers[i, 0] != null && Inv_Manager.instance.GetProdDiscountLevel(this.prodControllers[i, 0].prodIndex) == -1)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0000EAC8 File Offset: 0x0000CCC8
	public bool Get_HasAnyProdDifferentFromCategory(Inv_Manager.ProdType _prodType)
	{
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				if (this.prodControllers[i, 0] != null && this.prodControllers[i, 0].prodType != _prodType)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x040001E8 RID: 488
	[SerializeField]
	public bool buyable;

	// Token: 0x040001E9 RID: 489
	[SerializeField]
	public bool randonlyUnlockable;

	// Token: 0x040001EA RID: 490
	[SerializeField]
	public int shelfIndex;

	// Token: 0x040001EB RID: 491
	[SerializeField]
	private GameObject shelfParent;

	// Token: 0x040001EC RID: 492
	[SerializeField]
	public int height;

	// Token: 0x040001ED RID: 493
	[SerializeField]
	public int width;

	// Token: 0x040001EE RID: 494
	[SerializeField]
	public bool isShelfProd;

	// Token: 0x040001EF RID: 495
	[SerializeField]
	public bool isShelfInv;

	// Token: 0x040001F0 RID: 496
	[HideInInspector]
	public Util_Controller utilController;

	// Token: 0x040001F1 RID: 497
	[SerializeField]
	public bool isRefrigerated;

	// Token: 0x040001F2 RID: 498
	[SerializeField]
	public Color[] itemColor;

	// Token: 0x040001F3 RID: 499
	[SerializeField]
	public Sprite itemSprite;

	// Token: 0x040001F4 RID: 500
	[SerializeField]
	public int itemPrice;

	// Token: 0x040001F5 RID: 501
	public GameObject[] boxPlace;

	// Token: 0x040001F6 RID: 502
	public GameObject[,] prodPlace;

	// Token: 0x040001F7 RID: 503
	public GameObject[] discountPaperPlace;

	// Token: 0x040001F8 RID: 504
	public Prod_Controller[,] prodControllers;

	// Token: 0x040001F9 RID: 505
	public Box_Controller[] boxControllers;

	// Token: 0x040001FA RID: 506
	public DiscountPaper_Controller[] discountPapers;

	// Token: 0x040001FB RID: 507
	public EventReference event_Material;
}
