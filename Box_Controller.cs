using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class Box_Controller : MonoBehaviour
{
	// Token: 0x0600000D RID: 13 RVA: 0x000021F3 File Offset: 0x000003F3
	private void Awake()
	{
		this.rigidbody = base.GetComponent<Rigidbody>();
		this.boxCollider = base.GetComponent<BoxCollider>();
	}

	// Token: 0x0600000E RID: 14 RVA: 0x0000220D File Offset: 0x0000040D
	private void Update()
	{
		if (base.transform.position.y <= -20f)
		{
			VoidPortal_Controller.instance.TransportObject(base.gameObject, this.rigidbody);
			this.Set_EE_BoxHight_State(-1);
		}
		this.Update_EasterEgg();
	}

	// Token: 0x0600000F RID: 15 RVA: 0x0000224C File Offset: 0x0000044C
	public void CreateBox(int _itemIndex, int _prodQnt, int _lifeSpan, bool _frozen = false)
	{
		if (this.itemIndex < 0)
		{
			return;
		}
		this.itemIndex = _itemIndex;
		if (this.isProd)
		{
			Prod_Controller prodPrefab = Inv_Manager.instance.GetProdPrefab(this.itemIndex);
			this.prodImage_Image.sprite = Inv_Manager.instance.GetProdSprite(this.itemIndex);
			this.event_Material = prodPrefab.event_Material;
			this.isBall = prodPrefab.isBall;
			this.Set_Freeze(_frozen);
		}
		if (this.isShelf)
		{
			Shelf_Controller itemShelf = Inv_Manager.instance.GetItemShelf(this.itemIndex);
			this.prodImage_Image.sprite = itemShelf.itemSprite;
			this.event_Material = itemShelf.event_Material;
		}
		if (this.isDecor)
		{
			Decor_Controller itemDecor = Inv_Manager.instance.GetItemDecor(this.itemIndex);
			this.prodImage_Image.sprite = itemDecor.itemSprite;
			this.event_Material = itemDecor.event_Material;
		}
		if (this.isWall)
		{
			WallPaint_Controller itemWallPaint = Inv_Manager.instance.GetItemWallPaint(this.itemIndex);
			this.prodImage_Image.color = itemWallPaint.itemColor[0];
			this.event_Material = itemWallPaint.event_Material;
		}
		if (this.isFloor)
		{
			Floor_Controller itemFloor = Inv_Manager.instance.GetItemFloor(this.itemIndex);
			this.prodImage_Image.sprite = itemFloor.itemSprite;
			this.meshRenderer.material = itemFloor.meshRenderer.sharedMaterial;
			this.event_Material = itemFloor.event_Material;
		}
		if (this.isUtil)
		{
			Util_Controller itemUtil = Inv_Manager.instance.GetItemUtil(this.itemIndex);
			this.prodImage_Image.sprite = itemUtil.itemSprite;
			this.event_Material = itemUtil.event_Material;
		}
		this.SetProdQnt(_prodQnt);
		this.SetLifeSpan(_lifeSpan);
	}

	// Token: 0x06000010 RID: 16 RVA: 0x0000240B File Offset: 0x0000060B
	public void SetProdQnt(int _qnt)
	{
		this.prodQnt = _qnt;
		this.prodQnt_Image.sprite = Inv_Manager.instance.boxSize_Sprites[this.prodQnt];
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002434 File Offset: 0x00000634
	public void DeleteBox(int _player_index)
	{
		Inv_Manager.instance.DeleteBox(this, _player_index);
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002444 File Offset: 0x00000644
	public void HoldBox(GameObject _boxHolder, bool _grabbed, Shelf_Controller _shelf_Controller, Cart_Controller _cart = null)
	{
		if (_cart != null)
		{
			GameObject freeSpace = _cart.Get_FreeSpace();
			if (freeSpace != null)
			{
				_boxHolder = freeSpace;
				_cart.IncludeBox(this);
			}
		}
		this.shelf_Controller_Holding = _shelf_Controller;
		this.isHeld = true;
		this.rigidbody.isKinematic = true;
		this.boxCollider.enabled = false;
		base.transform.SetParent(_boxHolder.transform);
		base.transform.localRotation = Quaternion.Euler(Vector3.zero);
		base.transform.localPosition = Vector3.zero;
		base.transform.localScale = Vector3.one;
		base.gameObject.GetComponent<Animator>().PlayInFixedTime("Box_On", -1, 0f);
		EventReference @event;
		if (_grabbed)
		{
			if (this.prodQnt > 6)
			{
				@event = Audio_Manager.instance.event_GrabHeavy;
			}
			else if (this.prodQnt > 3)
			{
				@event = Audio_Manager.instance.event_GrabMedium;
			}
			else
			{
				@event = Audio_Manager.instance.event_GrabLight;
			}
		}
		else if (this.prodQnt > 6)
		{
			@event = Audio_Manager.instance.event_PlaceHeavy;
		}
		else if (this.prodQnt > 3)
		{
			@event = Audio_Manager.instance.event_PlaceMedium;
		}
		else
		{
			@event = Audio_Manager.instance.event_PlaceLight;
		}
		Audio_Manager.instance.PlaySound(@event, base.transform, this.event_Material);
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002588 File Offset: 0x00000788
	public void ThrowBox()
	{
		if (!this.isHeld)
		{
			return;
		}
		if (this.cart_controller != null)
		{
			this.cart_controller.RemoveBox(this);
			this.cart_controller = null;
		}
		this.isHeld = false;
		this.rigidbody.isKinematic = false;
		this.boxCollider.enabled = true;
		base.transform.SetParent(null);
		base.transform.rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
		base.transform.localScale = Vector3.one;
		this.rigidbody.AddForce(this.throwForce * (base.transform.forward + base.transform.up), ForceMode.Impulse);
		this.Set_EE_BoxHight_State(0);
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002668 File Offset: 0x00000868
	public void ChangeQnt(int _qnt, int _lifeSpanToMerge, int _player_index)
	{
		if (Cheat_Manager.instance.GetBoxesNeverRunOut() == 1)
		{
			_qnt = 0;
		}
		this.prodQnt += _qnt;
		this.prodQnt = Mathf.Clamp(this.prodQnt, 0, 8);
		this.SetProdQnt(this.prodQnt);
		if (this.prodQnt == 0)
		{
			if (this.cart_controller != null)
			{
				this.cart_controller.RemoveBox(this);
				this.cart_controller = null;
			}
			this.DeleteBox(_player_index);
		}
		if (_qnt > 0)
		{
			base.gameObject.GetComponent<Animator>().SetTrigger("Shake");
		}
		else if (_qnt < 0)
		{
			base.gameObject.GetComponent<Animator>().SetTrigger("ShakeNegative");
		}
		this.MergeLifeSpan(_lifeSpanToMerge);
		if (_qnt > 0)
		{
			EventReference @event = Audio_Manager.instance.event_GrabLight;
			if (this.prodQnt >= 6)
			{
				@event = Audio_Manager.instance.event_GrabHeavy;
			}
			else if (this.prodQnt >= 3)
			{
				@event = Audio_Manager.instance.event_GrabMedium;
			}
			Audio_Manager.instance.PlaySound(@event, base.transform, this.event_Material);
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002770 File Offset: 0x00000970
	public void PlaceItem(GameObject _place, int _player_index)
	{
		EventReference @event = Audio_Manager.instance.event_PlaceLight;
		if (this.isShelf)
		{
			Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.place_shelf, -1);
			Inv_Manager.instance.PlaceShelf(this.itemIndex, _place, _player_index);
			@event = Audio_Manager.instance.event_PlaceHeavy;
		}
		else if (this.isDecor)
		{
			Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.place_decor, -1);
			Inv_Manager.instance.PlaceDecor(this.itemIndex, _place, this.lifeSpanIndex, _player_index);
			@event = Audio_Manager.instance.event_PlaceMedium;
		}
		else if (this.isUtil)
		{
			Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.place_util, -1);
			Inv_Manager.instance.PlaceUtil(this.itemIndex, _place, _player_index);
			@event = Audio_Manager.instance.event_PlaceHeavy;
		}
		this.ChangeQnt(-1, this.lifeSpanIndex, _player_index);
		Audio_Manager.instance.PlaySound(@event, _place.transform, this.event_Material);
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002850 File Offset: 0x00000A50
	public void ChangeItemWallPaint(WallPaint_Controller _controller, int _player_index)
	{
		Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.place_wall, -1);
		Inv_Manager.instance.PlaceWallPaint(this.itemIndex, _controller);
		this.ChangeQnt(-1, this.lifeSpanIndex, _player_index);
		EventReference event_UsePaint = Audio_Manager.instance.event_UsePaint;
		Audio_Manager.instance.PlaySound(event_UsePaint, base.transform, this.event_Material);
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000028AC File Offset: 0x00000AAC
	public void ChangeItemFloor(Floor_Controller _controller, int _player_index)
	{
		Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.place_floor, -1);
		Inv_Manager.instance.PlaceFloor(this.itemIndex, _controller);
		this.ChangeQnt(-1, this.lifeSpanIndex, _player_index);
		EventReference event_UseFloor = Audio_Manager.instance.event_UseFloor;
		Audio_Manager.instance.PlaySound(event_UseFloor, base.transform, this.event_Material);
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002908 File Offset: 0x00000B08
	private void OnCollisionEnter(Collision other)
	{
		if (Mathf.Abs(this.rigidbody.velocity.magnitude) > 0f && other.gameObject.tag != "Player")
		{
			EventReference @event = Audio_Manager.instance.event_DropLight;
			if (this.prodQnt >= 6)
			{
				@event = Audio_Manager.instance.event_DropHeavy;
			}
			else if (this.prodQnt >= 3)
			{
				@event = Audio_Manager.instance.event_DropMedium;
			}
			Audio_Manager.instance.PlaySound(@event, base.transform, this.event_Material);
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002998 File Offset: 0x00000B98
	public int GetBoxType()
	{
		int result = 0;
		if (this.isProd)
		{
			result = 0;
		}
		else if (this.isShelf)
		{
			result = 1;
		}
		else if (this.isDecor)
		{
			result = 2;
		}
		else if (this.isWall)
		{
			result = 3;
		}
		else if (this.isFloor)
		{
			result = 4;
		}
		else if (this.isUtil)
		{
			result = 5;
		}
		return result;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000029EE File Offset: 0x00000BEE
	public void SetLifeSpan(int _index)
	{
		this.lifeSpanIndex = _index;
		this.RefreshLifeSpanIcon();
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002A00 File Offset: 0x00000C00
	public void DecreaseLifeSpan()
	{
		int num = Inv_Manager.instance.lifeSpan_DailyLoss_RightShelf;
		if (this.shelf_Controller_Holding)
		{
			if (!this.shelf_Controller_Holding.isRefrigerated && this.needsRefrigerator)
			{
				num = Inv_Manager.instance.lifeSpan_DailyLoss_WrongShelf;
			}
		}
		else if (this.needsRefrigerator)
		{
			num = Inv_Manager.instance.lifeSpan_DailyLoss_WrongShelf;
		}
		else
		{
			num = Inv_Manager.instance.lifeSpan_DailyLoss_NoShelf;
		}
		if (this.lifeSpanIndex > 0)
		{
			this.lifeSpanIndex -= num;
		}
		if (this.lifeSpanIndex < 0)
		{
			this.lifeSpanIndex = 0;
		}
		this.RefreshLifeSpanIcon();
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002A94 File Offset: 0x00000C94
	public void MergeLifeSpan(int _LifeSpanToMerge)
	{
		if (_LifeSpanToMerge < this.lifeSpanIndex)
		{
			this.lifeSpanIndex = _LifeSpanToMerge;
		}
		this.RefreshLifeSpanIcon();
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002AAC File Offset: 0x00000CAC
	public void RefreshLifeSpanIcon()
	{
		if (!this.isProd)
		{
			return;
		}
		if (!this.prodLifeSpan_Image)
		{
			return;
		}
		if (!Inv_Manager.instance.GetProdPrefab(this.itemIndex).lifeSpan)
		{
			this.prodLifeSpan_Image.gameObject.SetActive(false);
			return;
		}
		this.prodLifeSpan_Image.gameObject.SetActive(true);
		if (this.lifeSpanIndex >= Inv_Manager.instance.GetLifeSpanLevels(0))
		{
			this.prodLifeSpan_Image.color = Inv_Manager.instance.prod_LifeSpan_Colors[0];
			return;
		}
		if (this.lifeSpanIndex >= Inv_Manager.instance.GetLifeSpanLevels(1))
		{
			this.prodLifeSpan_Image.color = Inv_Manager.instance.prod_LifeSpan_Colors[1];
			return;
		}
		if (this.lifeSpanIndex >= Inv_Manager.instance.GetLifeSpanLevels(2))
		{
			this.prodLifeSpan_Image.color = Inv_Manager.instance.prod_LifeSpan_Colors[2];
			return;
		}
		this.prodLifeSpan_Image.color = Inv_Manager.instance.prod_LifeSpan_Colors[3];
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002BB8 File Offset: 0x00000DB8
	public void Check_Freezer(bool _only_freeze)
	{
		if (this.ice_go == null)
		{
			return;
		}
		if (this.shelf_Controller_Holding != null && this.shelf_Controller_Holding.isRefrigerated && !this.needsRefrigerator)
		{
			this.Set_Freeze(true);
			return;
		}
		if (!_only_freeze)
		{
			this.Set_Freeze(false);
		}
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002C09 File Offset: 0x00000E09
	public void Set_Freeze(bool _bool)
	{
		if (this.ice_go == null)
		{
			return;
		}
		this.frozen = _bool;
		this.Refresh_Ice();
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002C27 File Offset: 0x00000E27
	public void Refresh_Ice()
	{
		if (this.ice_go == null)
		{
			return;
		}
		this.ice_go.SetActive(this.frozen);
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002C4C File Offset: 0x00000E4C
	private void Update_EasterEgg()
	{
		float y = base.transform.position.y;
		if (this.ee_max_hight >= 0f && y >= this.ee_hight_to_start)
		{
			if (y > this.ee_max_hight)
			{
				this.ee_max_hight = y;
				return;
			}
			if (y < this.ee_max_hight)
			{
				EasterEgg_Manager.instance.Set_BoxHight_Hight(this.ee_max_hight);
				this.ee_max_hight = -1f;
				return;
			}
		}
		else if (this.ee_max_hight == -1f && y < this.ee_hight_to_start && this.ee_max_hight >= this.ee_hight_to_start)
		{
			this.ee_max_hight = 0f;
		}
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002CE5 File Offset: 0x00000EE5
	public void Set_EE_BoxHight_State(int _state)
	{
		this.ee_max_hight = (float)_state;
	}

	// Token: 0x04000012 RID: 18
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x04000013 RID: 19
	[SerializeField]
	public SpriteRenderer prodImage_Image;

	// Token: 0x04000014 RID: 20
	[SerializeField]
	private SpriteRenderer prodQnt_Image;

	// Token: 0x04000015 RID: 21
	[SerializeField]
	private SpriteRenderer prodLifeSpan_Image;

	// Token: 0x04000016 RID: 22
	[SerializeField]
	private Animator anim;

	// Token: 0x04000017 RID: 23
	[SerializeField]
	public Rigidbody rigidbody;

	// Token: 0x04000018 RID: 24
	[SerializeField]
	private BoxCollider boxCollider;

	// Token: 0x04000019 RID: 25
	[SerializeField]
	public Shelf_Controller shelf_Controller_Holding;

	// Token: 0x0400001A RID: 26
	[SerializeField]
	private float throwForce = 2f;

	// Token: 0x0400001B RID: 27
	[SerializeField]
	private GameObject ice_go;

	// Token: 0x0400001C RID: 28
	public Cart_Controller cart_controller;

	// Token: 0x0400001D RID: 29
	[Header("Type")]
	public bool isProd;

	// Token: 0x0400001E RID: 30
	public bool isShelf;

	// Token: 0x0400001F RID: 31
	public bool isDecor;

	// Token: 0x04000020 RID: 32
	public bool isWall;

	// Token: 0x04000021 RID: 33
	public bool isFloor;

	// Token: 0x04000022 RID: 34
	public bool isUtil;

	// Token: 0x04000023 RID: 35
	[Header("Index")]
	public int itemIndex;

	// Token: 0x04000024 RID: 36
	public int prodQnt;

	// Token: 0x04000025 RID: 37
	public int lifeSpanIndex;

	// Token: 0x04000026 RID: 38
	public bool isHeld;

	// Token: 0x04000027 RID: 39
	public bool isGarbage;

	// Token: 0x04000028 RID: 40
	public bool needsRefrigerator;

	// Token: 0x04000029 RID: 41
	public bool frozen;

	// Token: 0x0400002A RID: 42
	public bool isBall;

	// Token: 0x0400002B RID: 43
	private EventReference event_Grab;

	// Token: 0x0400002C RID: 44
	[HideInInspector]
	public EventReference event_Material;

	// Token: 0x0400002D RID: 45
	private float ee_hight_to_start = 10f;

	// Token: 0x0400002E RID: 46
	[SerializeField]
	private float ee_max_hight = -1f;
}
