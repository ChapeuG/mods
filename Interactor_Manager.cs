using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class Interactor_Manager : MonoBehaviour
{
	// Token: 0x060002D0 RID: 720 RVA: 0x00016BEF File Offset: 0x00014DEF
	private void Awake()
	{
		if (!Interactor_Manager.instance)
		{
			Interactor_Manager.instance = this;
		}
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x00016C04 File Offset: 0x00014E04
	private void Start()
	{
		this.ui_ctrls.Add(UnityEngine.Object.Instantiate<RectTransform>(this.interactor_Canvas).GetComponent<WorldUI_Controller>());
		this.ui_ctrls.Add(UnityEngine.Object.Instantiate<RectTransform>(this.interactor_Canvas).GetComponent<WorldUI_Controller>());
		this.ui_ctrls.Add(UnityEngine.Object.Instantiate<RectTransform>(this.interactor_Canvas).GetComponent<WorldUI_Controller>());
		this.ui_ctrls.Add(UnityEngine.Object.Instantiate<RectTransform>(this.interactor_Canvas).GetComponent<WorldUI_Controller>());
		int num = 0;
		foreach (WorldUI_Controller worldUI_Controller in this.ui_ctrls)
		{
			worldUI_Controller.gameObject.SetActive(true);
			worldUI_Controller._Awake();
			worldUI_Controller.playerId = num;
			num++;
			worldUI_Controller._Start();
		}
		this.interactor_Canvas.gameObject.SetActive(false);
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x00016CF0 File Offset: 0x00014EF0
	private void LateUpdate()
	{
		this.UpdateInteractorPos();
		this.UpdateCashierInteractors();
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00016CFE File Offset: 0x00014EFE
	public GameObject GetPlacePoint(int _player_index)
	{
		return this.ui_ctrls[_player_index].currentPlacePoint;
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00016D14 File Offset: 0x00014F14
	private void UpdateInteractorPos()
	{
		for (int i = 0; i < this.ui_ctrls.Count; i++)
		{
			if (i >= Player_Manager.instance.playerControllerList.Count)
			{
				if (this.ui_ctrls[i].gameObject.activeSelf)
				{
					this.ui_ctrls[i].gameObject.SetActive(false);
				}
			}
			else
			{
				if (!this.ui_ctrls[i].gameObject.activeSelf)
				{
					this.ui_ctrls[i].gameObject.SetActive(true);
				}
				Player_Controller playerController = Player_Manager.instance.GetPlayerController(i);
				Interaction_Controller interaction_Controller = playerController.currentInterative;
				this.ui_ctrls[i].currentPlacePoint = null;
				if (playerController.boxController && Menu_Manager.instance.GetMenuName() == "MainMenu" && !playerController.GetPlayerWalking())
				{
					if (playerController.boxController.isShelf || playerController.boxController.isDecor || playerController.boxController.isUtil)
					{
						Inv_Manager.instance.SetNearPlacePointWallPaint(null);
						Inv_Manager.instance.SetNearPlacePointFloor(null);
						this.ui_ctrls[i].currentPlacePoint = Nav_Manager.instance.FindNearPlacePoint(playerController.boxController, playerController.Get_Interaction_Position());
						if (this.ui_ctrls[i].currentPlacePoint)
						{
							Vector3 v = this.cam_Game.WorldToScreenPoint(this.ui_ctrls[i].currentPlacePoint.transform.position + Vector3.up * 0.5f);
							v.z = 0f;
							this.ui_ctrls[i].int_Place.anchoredPosition = this.GetPosAfterRender(v, false);
							this.floor_interactor.transform.position = this.ui_ctrls[i].currentPlacePoint.transform.position;
						}
					}
					else if (playerController.boxController.isWall)
					{
						Nav_Manager.instance.SetNearPlacePoint(null);
						this.ui_ctrls[i].currentPlacePoint = Inv_Manager.instance.FindNearPlacePointWallPaint(playerController.boxController, playerController.Get_Interaction_Position());
						if (this.ui_ctrls[i].currentPlacePoint)
						{
							Vector3 v2 = this.cam_Game.WorldToScreenPoint(this.ui_ctrls[i].currentPlacePoint.transform.position + Vector3.up * 1.5f);
							v2.z = 0f;
							this.ui_ctrls[i].int_Place.anchoredPosition = this.GetPosAfterRender(v2, false);
							this.floor_interactor.transform.position = this.ui_ctrls[i].currentPlacePoint.transform.position;
						}
					}
					else if (playerController.boxController.isFloor)
					{
						Nav_Manager.instance.SetNearPlacePoint(null);
						this.ui_ctrls[i].currentPlacePoint = Inv_Manager.instance.FindNearPlacePointFloor(playerController.boxController, playerController.Get_Interaction_Position());
						if (this.ui_ctrls[i].currentPlacePoint)
						{
							Vector3 v3 = this.cam_Game.WorldToScreenPoint(this.ui_ctrls[i].currentPlacePoint.transform.position + Vector3.up * 0.5f);
							v3.z = 0f;
							this.ui_ctrls[i].int_Place.anchoredPosition = this.GetPosAfterRender(v3, false);
							this.floor_interactor.transform.position = this.ui_ctrls[i].currentPlacePoint.transform.position;
						}
					}
					else
					{
						this.ui_ctrls[i].currentPlacePoint = null;
						Nav_Manager.instance.SetNearPlacePoint(null);
						Inv_Manager.instance.SetNearPlacePointWallPaint(null);
						Inv_Manager.instance.SetNearPlacePointFloor(null);
					}
				}
				else
				{
					this.ui_ctrls[i].currentPlacePoint = null;
					Inv_Manager.instance.SetNearPlacePointWallPaint(null);
					Inv_Manager.instance.SetNearPlacePointFloor(null);
					this.ui_ctrls[i].int_Place.gameObject.SetActive(false);
					this.floor_interactor.gameObject.SetActive(false);
				}
				if (playerController.boxController && this.ui_ctrls[i].currentPlacePoint && interaction_Controller)
				{
					Vector3 a = playerController.boxController.transform.position + playerController.boxController.transform.forward * 0.5f;
					float num = Vector3.Distance(a, this.ui_ctrls[i].currentPlacePoint.transform.position);
					float num2 = Vector3.Distance(a, interaction_Controller.transform.position);
					if (num >= num2)
					{
						this.ui_ctrls[i].currentPlacePoint = null;
					}
					else
					{
						interaction_Controller = null;
					}
				}
				if (this.ui_ctrls[i].currentPlacePoint == null)
				{
					this.ui_ctrls[i].int_Place.gameObject.SetActive(false);
					this.floor_interactor.gameObject.SetActive(false);
				}
				else
				{
					this.ui_ctrls[i].int_Place.gameObject.SetActive(true);
					this.floor_interactor.gameObject.SetActive(true);
				}
				if (interaction_Controller && Menu_Manager.instance.GetMenuName() == "MainMenu" && !playerController.GetPlayerWalking())
				{
					if (!interaction_Controller.isShelf && !interaction_Controller.isBox && (!interaction_Controller.isDecor || interaction_Controller.isDecorPlant) && !interaction_Controller.isUtil)
					{
						Vector3 v4;
						if (interaction_Controller.ui_Pos[0])
						{
							v4 = this.cam_Game.WorldToScreenPoint(interaction_Controller.ui_Pos[0].transform.position);
						}
						else
						{
							v4 = this.cam_Game.WorldToScreenPoint(interaction_Controller.transform.position);
						}
						v4.z = 0f;
						this.ui_ctrls[i].int_Simple.anchoredPosition = this.GetPosAfterRender(v4, true);
					}
					if (interaction_Controller.isBox)
					{
						Vector3 v5;
						if (interaction_Controller.ui_Pos[0])
						{
							v5 = this.cam_Game.WorldToScreenPoint(interaction_Controller.ui_Pos[0].transform.position);
						}
						else
						{
							v5 = this.cam_Game.WorldToScreenPoint(interaction_Controller.transform.position);
						}
						v5.z = 0f;
						this.ui_ctrls[i].int_Prod[0].anchoredPosition = this.GetPosAfterRender(v5, false);
					}
					if (interaction_Controller.isShelf)
					{
						Shelf_Controller component = interaction_Controller.gameObject.GetComponent<Shelf_Controller>();
						if (interaction_Controller.isMachinery && interaction_Controller.isBroken && Player_Manager.instance.GetPlayerController(i).tools_Controller)
						{
							Vector3 v6;
							if (interaction_Controller.ui_Pos[0])
							{
								v6 = this.cam_Game.WorldToScreenPoint(interaction_Controller.ui_Pos[0].transform.position);
							}
							else
							{
								v6 = this.cam_Game.WorldToScreenPoint(interaction_Controller.transform.position);
							}
							v6.z = 0f;
							this.ui_ctrls[i].int_Simple.anchoredPosition = this.GetPosAfterRender(v6, false);
						}
						else if (component.GetIsEmpty() && component.isShelfProd && playerController.GetIsEmptyOrHasSameShelfBox(component.shelfIndex))
						{
							Vector3 v7;
							if (interaction_Controller.ui_Pos[0])
							{
								v7 = this.cam_Game.WorldToScreenPoint(interaction_Controller.ui_Pos[0].transform.position);
							}
							else
							{
								v7 = this.cam_Game.WorldToScreenPoint(interaction_Controller.transform.position);
							}
							v7.z = 0f;
							this.ui_ctrls[i].int_Build.anchoredPosition = this.GetPosAfterRender(v7, false);
						}
						else
						{
							Vector3[] array = new Vector3[component.height];
							for (int j = 0; j < component.height; j++)
							{
								array[j] = this.cam_Game.WorldToScreenPoint(component.boxPlace[j].transform.position);
								array[j].z = 0f;
								this.ui_ctrls[i].int_Prod[j].anchoredPosition = this.GetPosAfterRender(array[j], false);
							}
						}
					}
					if (interaction_Controller.isDecor)
					{
						Decor_Controller component2 = interaction_Controller.gameObject.GetComponent<Decor_Controller>();
						if (playerController.GetIsEmptyOrHasSameDecorBox(component2.decorIndex))
						{
							Vector3 v8;
							if (interaction_Controller.ui_Pos[0])
							{
								v8 = this.cam_Game.WorldToScreenPoint(interaction_Controller.ui_Pos[0].transform.position);
							}
							else
							{
								v8 = this.cam_Game.WorldToScreenPoint(interaction_Controller.transform.position);
							}
							v8.z = 0f;
							this.ui_ctrls[i].int_Build.anchoredPosition = this.GetPosAfterRender(v8, false);
						}
					}
					if (interaction_Controller.isUtil)
					{
						Util_Controller component3 = interaction_Controller.gameObject.GetComponent<Util_Controller>();
						if (interaction_Controller.isMachinery && interaction_Controller.isBroken && Player_Manager.instance.GetPlayerController(i).tools_Controller)
						{
							Vector3 v9;
							if (interaction_Controller.ui_Pos[0])
							{
								v9 = this.cam_Game.WorldToScreenPoint(interaction_Controller.ui_Pos[0].transform.position);
							}
							else
							{
								v9 = this.cam_Game.WorldToScreenPoint(interaction_Controller.transform.position);
							}
							v9.z = 0f;
							this.ui_ctrls[i].int_Simple.anchoredPosition = this.GetPosAfterRender(v9, false);
						}
						else if (component3.GetIsEmpty() && playerController.GetIsEmptyOrHasSameUtilBox(component3.utilIndex))
						{
							Vector3 v10;
							if (interaction_Controller.ui_Pos[0])
							{
								v10 = this.cam_Game.WorldToScreenPoint(interaction_Controller.ui_Pos[0].transform.position);
							}
							else
							{
								v10 = this.cam_Game.WorldToScreenPoint(interaction_Controller.transform.position);
							}
							v10.z = 0f;
							this.ui_ctrls[i].int_Build.anchoredPosition = this.GetPosAfterRender(v10, false);
						}
						else if (component3.boxPlace)
						{
							Vector3 v11 = this.cam_Game.WorldToScreenPoint(component3.boxPlace.transform.position);
							v11.z = 0f;
							this.ui_ctrls[i].int_Prod[0].anchoredPosition = this.GetPosAfterRender(v11, false);
						}
					}
				}
				else
				{
					this.ui_ctrls[i].int_Simple.gameObject.SetActive(false);
					this.ui_ctrls[i].int_Build.gameObject.SetActive(false);
					for (int k = 0; k < this.ui_ctrls[i].int_Prod.Count; k++)
					{
						if (this.ui_ctrls[i].int_Prod[k].gameObject.activeInHierarchy)
						{
							this.ui_ctrls[i].int_Prod[k].gameObject.SetActive(false);
						}
					}
					for (int l = 0; l < this.ui_ctrls[i].int_Discount.Count; l++)
					{
						if (this.ui_ctrls[i].int_Discount[l].gameObject.activeInHierarchy)
						{
							this.ui_ctrls[i].int_Discount[l].gameObject.SetActive(false);
						}
					}
				}
				Box_Controller boxController = Player_Manager.instance.GetPlayerController(i).boxController;
				if (boxController != null)
				{
					if (!this.ui_ctrls[i].int_BoxQnt.gameObject.activeInHierarchy)
					{
						this.ui_ctrls[i].int_BoxQnt.gameObject.SetActive(true);
					}
					if (boxController.isProd)
					{
						Prod_Controller prodPrefab = Inv_Manager.instance.GetProdPrefab(boxController.itemIndex);
						this.ui_ctrls[i].boxQntColor = prodPrefab.prodColors[0];
					}
					else if (boxController.isShelf)
					{
						Shelf_Controller itemShelf = Inv_Manager.instance.GetItemShelf(boxController.itemIndex);
						this.ui_ctrls[i].boxQntColor = itemShelf.itemColor[0];
					}
					else if (boxController.isDecor)
					{
						Decor_Controller itemDecor = Inv_Manager.instance.GetItemDecor(boxController.itemIndex);
						this.ui_ctrls[i].boxQntColor = itemDecor.itemColor[0];
					}
					else if (boxController.isWall)
					{
						WallPaint_Controller itemWallPaint = Inv_Manager.instance.GetItemWallPaint(boxController.itemIndex);
						this.ui_ctrls[i].boxQntColor = itemWallPaint.itemColor[0];
					}
					else if (boxController.isFloor)
					{
						Floor_Controller itemFloor = Inv_Manager.instance.GetItemFloor(boxController.itemIndex);
						this.ui_ctrls[i].boxQntColor = itemFloor.itemColor[0];
					}
					else if (boxController.isUtil)
					{
						Util_Controller itemUtil = Inv_Manager.instance.GetItemUtil(boxController.itemIndex);
						this.ui_ctrls[i].boxQntColor = itemUtil.itemColor[0];
					}
					this.ui_ctrls[i].int_BoxQnt_PanelQnt[0].color = this.ui_ctrls[i].boxQntColor;
					this.ui_ctrls[i].int_BoxQnt_PanelQnt[1].color = this.ui_ctrls[i].boxQntColor;
					if (this.ui_ctrls[i].int_BoxQnt_CurrentQnt != boxController.prodQnt)
					{
						this.ui_ctrls[i].int_BoxQnt_CurrentQnt = boxController.prodQnt;
						this.ui_ctrls[i].int_BoxQnt_ItemQnt.text = "x" + this.ui_ctrls[i].int_BoxQnt_CurrentQnt.ToString();
					}
					if (this.ui_ctrls[i].int_BoxQnt_ItemSprite.sprite != boxController.prodImage_Image.sprite)
					{
						this.ui_ctrls[i].int_BoxQnt_ItemSprite.sprite = boxController.prodImage_Image.sprite;
					}
					Vector3 v12 = this.cam_Game.WorldToScreenPoint(boxController.transform.position);
					v12.z = 0f;
					this.ui_ctrls[i].int_BoxQnt.anchoredPosition = this.GetPosAfterRender(v12, false);
				}
				else if (this.ui_ctrls[i].int_BoxQnt.gameObject.activeInHierarchy)
				{
					this.ui_ctrls[i].int_BoxQnt.gameObject.SetActive(false);
				}
				DiscountPaper_Controller discountPaper_Controller = Player_Manager.instance.GetPlayerController(i).discountPaper_Controller;
				if (discountPaper_Controller != null)
				{
					if (!this.ui_ctrls[i].int_DiscNew.gameObject.activeInHierarchy)
					{
						this.ui_ctrls[i].int_DiscNew.gameObject.SetActive(true);
						this.ui_ctrls[i].int_DiscNew_Index = -99;
					}
					int num3 = discountPaper_Controller.GetDiscountLevel();
					if (num3 != this.ui_ctrls[i].int_DiscNew_Index)
					{
						num3++;
						for (int m = 0; m < 4; m++)
						{
							if (m == num3)
							{
								this.ui_ctrls[i].int_DiscNew_Options[m].transform.localScale = Vector2.one;
								this.ui_ctrls[i].int_DiscNew_OptionsOff[m].gameObject.SetActive(false);
							}
							else
							{
								this.ui_ctrls[i].int_DiscNew_Options[m].transform.localScale = Vector2.one * 0.9f;
								this.ui_ctrls[i].int_DiscNew_OptionsOff[m].gameObject.SetActive(true);
							}
						}
					}
					Vector3 v13 = this.cam_Game.WorldToScreenPoint(discountPaper_Controller.transform.position);
					v13.z = 0f;
					this.ui_ctrls[i].int_DiscNew.anchoredPosition = this.GetPosAfterRender(v13, false);
				}
				else if (this.ui_ctrls[i].int_DiscNew.gameObject.activeInHierarchy)
				{
					this.ui_ctrls[i].int_DiscNew.gameObject.SetActive(false);
				}
				Vector3 v14 = this.cam_Game.WorldToScreenPoint(playerController.transform.position);
				v14.z = 0f;
				this.ui_ctrls[i].int_Hints.anchoredPosition = this.GetPosAfterRender(v14, false);
			}
		}
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00017F54 File Offset: 0x00016154
	public void RefreshInputHints()
	{
		for (int i = 0; i < this.ui_ctrls.Count; i++)
		{
			if (i >= Player_Manager.instance.playerControllerList.Count)
			{
				return;
			}
			this.RefreshInputHints(i);
		}
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x00017F94 File Offset: 0x00016194
	public void RefreshInputHints(int _player_index)
	{
		Player_Controller playerController = Player_Manager.instance.GetPlayerController(_player_index);
		if (playerController.boxController || playerController.discountPaper_Controller || playerController.tools_Controller || playerController.cart_Controller)
		{
			if (playerController.GetPlayerWalking())
			{
				this.ui_ctrls[_player_index].int_Hint_Drop.SetActive(false);
				this.ui_ctrls[_player_index].int_Hint_Run.SetActive(true);
			}
			else
			{
				this.ui_ctrls[_player_index].int_Hint_Drop.SetActive(true);
				this.ui_ctrls[_player_index].int_Hint_Run.SetActive(false);
			}
		}
		else
		{
			this.ui_ctrls[_player_index].int_Hint_Drop.SetActive(false);
			this.ui_ctrls[_player_index].int_Hint_Run.SetActive(false);
		}
		if (playerController.cashier_Controller)
		{
			this.ui_ctrls[_player_index].int_Hint_Leave.SetActive(true);
		}
		else
		{
			this.ui_ctrls[_player_index].int_Hint_Leave.SetActive(false);
		}
		if (playerController.cart_Controller || playerController.discountPaper_Controller)
		{
			this.ui_ctrls[_player_index].int_Hint_ChangeItem.SetActive(true);
		}
		else
		{
			this.ui_ctrls[_player_index].int_Hint_ChangeItem.SetActive(false);
		}
		Input_Manager.instance.RefreshInputHintsActive();
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00018108 File Offset: 0x00016308
	public Vector2 GetPosAfterRender(Vector2 _pos, bool _debug = false)
	{
		float num = Settings_Manager.instance.menuScale_WorldRes_UI.y / Settings_Manager.instance.menuScale_WorldRes_Game.y;
		Vector2 result;
		result.x = num * _pos.x;
		result.y = num * _pos.y;
		return result;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x00018154 File Offset: 0x00016354
	public void SetInteractive(Interaction_Controller _interactive, int _player_index)
	{
		if (_interactive != this.ui_ctrls[_player_index].oldInteractive)
		{
			this.ui_ctrls[_player_index].int_Simple.gameObject.SetActive(false);
			this.ui_ctrls[_player_index].int_Build.gameObject.SetActive(false);
			for (int i = 0; i < this.ui_ctrls[_player_index].int_Prod.Count; i++)
			{
				this.ui_ctrls[_player_index].int_Prod[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < this.ui_ctrls[_player_index].int_Discount.Count; j++)
			{
				this.ui_ctrls[_player_index].int_Discount[j].gameObject.SetActive(false);
			}
			this.ui_ctrls[_player_index].oldInteractive = _interactive;
		}
		this.RefreshInteractorType(_player_index);
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00018254 File Offset: 0x00016454
	public void RefreshInteractorType(int _player_index)
	{
		Interaction_Controller interaction_Controller = Player_Manager.instance.GetPlayerController(_player_index).currentInterative;
		if (this.ui_ctrls[_player_index].currentPlacePoint)
		{
			interaction_Controller = null;
		}
		if (!interaction_Controller)
		{
			return;
		}
		if (interaction_Controller.isBox)
		{
			for (int i = 0; i < this.ui_ctrls[_player_index].int_Prod.Count; i++)
			{
				if (i <= 0)
				{
					this.ui_ctrls[_player_index].int_Prod[i].gameObject.SetActive(true);
					this.ui_ctrls[_player_index].int_PanelProd[i].SetActive(true);
					this.FeedProdUI(i, interaction_Controller.gameObject.GetComponent<Box_Controller>(), _player_index);
				}
				else
				{
					this.ui_ctrls[_player_index].int_Prod[i].gameObject.SetActive(false);
				}
				this.ui_ctrls[_player_index].int_Discount[i].gameObject.SetActive(false);
			}
			this.ui_ctrls[_player_index].int_Simple.gameObject.SetActive(false);
			this.ui_ctrls[_player_index].int_Build.gameObject.SetActive(false);
		}
		else if (interaction_Controller.isShelf)
		{
			Shelf_Controller component = interaction_Controller.gameObject.GetComponent<Shelf_Controller>();
			if (interaction_Controller.isMachinery && interaction_Controller.isBroken && Player_Manager.instance.GetPlayerController(_player_index).tools_Controller)
			{
				this.ui_ctrls[_player_index].int_Simple.gameObject.SetActive(true);
				this.ui_ctrls[_player_index].int_Build.gameObject.SetActive(false);
				for (int j = 0; j < this.ui_ctrls[_player_index].int_Prod.Count; j++)
				{
					this.ui_ctrls[_player_index].int_Prod[j].gameObject.SetActive(false);
					this.ui_ctrls[_player_index].int_Discount[j].gameObject.SetActive(false);
				}
			}
			else if (component.GetIsEmpty() && component.isShelfProd && Player_Manager.instance.GetPlayerController(_player_index).GetIsEmptyOrHasSameShelfBox(component.shelfIndex))
			{
				this.ui_ctrls[_player_index].int_Simple.gameObject.SetActive(false);
				this.ui_ctrls[_player_index].int_Build.gameObject.SetActive(true);
				this.ui_ctrls[_player_index].int_Build_Name.text = Inv_Manager.instance.GetItemName(Inv_Manager.instance.shelfProd_Prefabs[component.shelfIndex].gameObject, true);
				for (int k = 0; k < this.ui_ctrls[_player_index].int_Prod.Count; k++)
				{
					this.ui_ctrls[_player_index].int_Prod[k].gameObject.SetActive(false);
					this.ui_ctrls[_player_index].int_Discount[k].gameObject.SetActive(false);
				}
			}
			else if (Player_Manager.instance.GetPlayerController(_player_index).boxController)
			{
				Box_Controller boxController = Player_Manager.instance.GetPlayerController(_player_index).boxController;
				if (boxController.isProd || component.isShelfInv)
				{
					for (int l = 0; l < 3; l++)
					{
						if (l < component.height)
						{
							if ((component.isShelfInv && !component.boxControllers[l]) || (component.isShelfProd && !component.prodControllers[l, 0]))
							{
								this.ui_ctrls[_player_index].int_Prod[l].gameObject.SetActive(true);
								this.ui_ctrls[_player_index].int_PanelProd[l].SetActive(false);
								this.ui_ctrls[_player_index].int_ShelfDiscount[l].gameObject.SetActive(false);
							}
							else if ((component.isShelfInv && !component.boxControllers[l]) || (component.isShelfProd && !component.prodControllers[l, 0]))
							{
								this.ui_ctrls[_player_index].int_Prod[l].gameObject.SetActive(true);
								this.ui_ctrls[_player_index].int_PanelProd[l].SetActive(false);
								this.ui_ctrls[_player_index].int_ShelfDiscount[l].gameObject.SetActive(false);
							}
							else if ((component.isShelfInv && component.boxControllers[l].itemIndex == boxController.itemIndex && component.boxControllers[l].prodQnt < 8) || (component.isShelfProd && component.prodControllers[l, 0].prodIndex == boxController.itemIndex && (boxController.prodQnt < 8 || (!component.prodControllers[l, 1] && boxController.prodQnt <= 8))))
							{
								this.ui_ctrls[_player_index].int_Prod[l].gameObject.SetActive(true);
								this.ui_ctrls[_player_index].int_PanelProd[l].SetActive(true);
								this.ui_ctrls[_player_index].int_ShelfDiscount[l].gameObject.SetActive(false);
								if (component.isShelfInv && component.boxControllers[l].itemIndex == boxController.itemIndex && component.boxControllers[l].prodQnt < 8)
								{
									this.ui_ctrls[_player_index].int_Prod_StoringImage[l].sprite = this.ui_ctrls[_player_index].int_Prod_StoringSprites[1];
								}
								else if (component.isShelfProd && component.prodControllers[l, 1])
								{
									this.ui_ctrls[_player_index].int_Prod_StoringImage[l].sprite = this.ui_ctrls[_player_index].int_Prod_StoringSprites[0];
								}
								else
								{
									this.ui_ctrls[_player_index].int_Prod_StoringImage[l].sprite = this.ui_ctrls[_player_index].int_Prod_StoringSprites[1];
								}
								if (component.isShelfInv)
								{
									this.FeedProdUI(l, component.boxControllers[l], _player_index);
								}
								else if (component.isShelfProd)
								{
									this.FeedProdUI(l, component.prodControllers[l, 0], component.width - component.GetAvailableProdSpaces(l), _player_index);
								}
							}
							else
							{
								this.ui_ctrls[_player_index].int_Prod[l].gameObject.SetActive(false);
							}
						}
						else
						{
							this.ui_ctrls[_player_index].int_Prod[l].gameObject.SetActive(false);
						}
						this.ui_ctrls[_player_index].int_Discount[l].gameObject.SetActive(false);
						this.ui_ctrls[_player_index].int_ShelfDiscount[l].SetActive(false);
					}
				}
				else
				{
					this.ui_ctrls[_player_index].int_Simple.gameObject.SetActive(false);
					this.ui_ctrls[_player_index].int_Build.gameObject.SetActive(false);
					for (int m = 0; m < this.ui_ctrls[_player_index].int_Prod.Count; m++)
					{
						this.ui_ctrls[_player_index].int_Prod[m].gameObject.SetActive(false);
						this.ui_ctrls[_player_index].int_Discount[m].gameObject.SetActive(false);
					}
				}
			}
			else if (Player_Manager.instance.GetPlayerController(_player_index).discountPaper_Controller)
			{
				DiscountPaper_Controller discountPaper_Controller = Player_Manager.instance.GetPlayerController(_player_index).discountPaper_Controller;
				for (int n = 0; n < 3; n++)
				{
					if (n < component.height)
					{
						if (component.isShelfProd)
						{
							if (component.prodControllers[n, 0])
							{
								this.ui_ctrls[_player_index].int_Prod[n].gameObject.SetActive(true);
								this.ui_ctrls[_player_index].int_PanelProd[n].gameObject.SetActive(false);
								if (component.GetDiscountPaperLevel(n) == -1 && discountPaper_Controller.GetDiscountLevel() == -1)
								{
									this.ui_ctrls[_player_index].int_Prod[n].gameObject.SetActive(false);
									this.ui_ctrls[_player_index].int_ShelfDiscount[n].gameObject.SetActive(false);
									this.ui_ctrls[_player_index].int_ShelfDiscountRemove[n].gameObject.SetActive(false);
								}
								else if (component.GetDiscountPaperLevel(n) == -1 || (discountPaper_Controller.GetDiscountLevel() >= 0 && discountPaper_Controller.GetDiscountLevel() != component.GetDiscountPaperLevel(n)))
								{
									this.ui_ctrls[_player_index].int_ShelfDiscount[n].gameObject.SetActive(true);
									this.ui_ctrls[_player_index].int_ShelfDiscountRemove[n].gameObject.SetActive(false);
								}
								else
								{
									this.ui_ctrls[_player_index].int_ShelfDiscount[n].gameObject.SetActive(true);
									this.ui_ctrls[_player_index].int_ShelfDiscountRemove[n].gameObject.SetActive(true);
								}
							}
							else
							{
								this.ui_ctrls[_player_index].int_Prod[n].gameObject.SetActive(false);
							}
						}
						else
						{
							this.ui_ctrls[_player_index].int_ShelfDiscount[n].gameObject.SetActive(false);
						}
					}
					else
					{
						this.ui_ctrls[_player_index].int_Prod[n].gameObject.SetActive(false);
					}
					this.ui_ctrls[_player_index].int_Discount[n].gameObject.SetActive(false);
				}
			}
			else
			{
				for (int num = 0; num < 3; num++)
				{
					if (num < component.height)
					{
						if ((component.isShelfInv && !component.boxControllers[num]) || (component.isShelfProd && !component.prodControllers[num, 0]))
						{
							this.ui_ctrls[_player_index].int_Prod[num].gameObject.SetActive(false);
						}
						else
						{
							this.ui_ctrls[_player_index].int_Prod[num].gameObject.SetActive(true);
							this.ui_ctrls[_player_index].int_PanelProd[num].SetActive(true);
							this.ui_ctrls[_player_index].int_ShelfDiscount[num].gameObject.SetActive(false);
							this.ui_ctrls[_player_index].int_Prod_StoringImage[num].sprite = this.ui_ctrls[_player_index].int_Prod_StoringSprites[0];
							if (component.isShelfInv)
							{
								this.FeedProdUI(num, component.boxControllers[num], _player_index);
							}
							else if (component.isShelfProd)
							{
								this.FeedProdUI(num, component.prodControllers[num, 0], component.width - component.GetAvailableProdSpaces(num), _player_index);
							}
						}
					}
					else
					{
						this.ui_ctrls[_player_index].int_Prod[num].gameObject.SetActive(false);
					}
					this.ui_ctrls[_player_index].int_Discount[num].gameObject.SetActive(false);
					this.ui_ctrls[_player_index].int_ShelfDiscount[num].SetActive(false);
				}
			}
		}
		else if (interaction_Controller.isDecor && (!Player_Manager.instance.GetPlayerController(_player_index).tools_Controller || !interaction_Controller.isDecorPlant))
		{
			Decor_Controller component2 = interaction_Controller.gameObject.GetComponent<Decor_Controller>();
			if (Player_Manager.instance.GetPlayerController(_player_index).GetIsEmptyOrHasSameDecorBox(component2.decorIndex))
			{
				this.ui_ctrls[_player_index].int_Simple.gameObject.SetActive(false);
				this.ui_ctrls[_player_index].int_Build.gameObject.SetActive(true);
				this.ui_ctrls[_player_index].int_Build_Name.text = Inv_Manager.instance.GetItemName(Inv_Manager.instance.decor_Prefabs[component2.decorIndex].gameObject, true);
				for (int num2 = 0; num2 < this.ui_ctrls[_player_index].int_Prod.Count; num2++)
				{
					this.ui_ctrls[_player_index].int_Prod[num2].gameObject.SetActive(false);
					this.ui_ctrls[_player_index].int_Discount[num2].gameObject.SetActive(false);
				}
			}
			else
			{
				this.ui_ctrls[_player_index].int_Simple.gameObject.SetActive(false);
				this.ui_ctrls[_player_index].int_Build.gameObject.SetActive(false);
				for (int num3 = 0; num3 < this.ui_ctrls[_player_index].int_Prod.Count; num3++)
				{
					this.ui_ctrls[_player_index].int_Prod[num3].gameObject.SetActive(false);
					this.ui_ctrls[_player_index].int_Discount[num3].gameObject.SetActive(false);
				}
			}
		}
		else if (interaction_Controller.isUtil)
		{
			Util_Controller component3 = interaction_Controller.gameObject.GetComponent<Util_Controller>();
			if (interaction_Controller.isMachinery && interaction_Controller.isBroken && Player_Manager.instance.GetPlayerController(_player_index).tools_Controller)
			{
				this.ui_ctrls[_player_index].int_Simple.gameObject.SetActive(true);
				this.ui_ctrls[_player_index].int_Build.gameObject.SetActive(false);
				for (int num4 = 0; num4 < this.ui_ctrls[_player_index].int_Prod.Count; num4++)
				{
					this.ui_ctrls[_player_index].int_Prod[num4].gameObject.SetActive(false);
					this.ui_ctrls[_player_index].int_Discount[num4].gameObject.SetActive(false);
				}
			}
			else if (component3.GetIsEmpty() && Player_Manager.instance.GetPlayerController(_player_index).GetIsEmptyOrHasSameUtilBox(component3.utilIndex))
			{
				this.ui_ctrls[_player_index].int_Simple.gameObject.SetActive(false);
				this.ui_ctrls[_player_index].int_Build.gameObject.SetActive(true);
				this.ui_ctrls[_player_index].int_Build_Name.text = Inv_Manager.instance.GetItemName(Inv_Manager.instance.util_Prefabs[component3.utilIndex].gameObject, true);
				for (int num5 = 0; num5 < this.ui_ctrls[_player_index].int_Prod.Count; num5++)
				{
					this.ui_ctrls[_player_index].int_Prod[num5].gameObject.SetActive(false);
					this.ui_ctrls[_player_index].int_Discount[num5].gameObject.SetActive(false);
				}
			}
			else if (Player_Manager.instance.GetPlayerController(_player_index).boxController)
			{
				Box_Controller boxController2 = Player_Manager.instance.GetPlayerController(_player_index).boxController;
				if (boxController2.isProd && component3.needsSource && component3.CheckItemAcceptance(boxController2))
				{
					for (int num6 = 0; num6 < 3; num6++)
					{
						if (num6 < 1)
						{
							if (component3.needsSource && !component3.shelfController.boxControllers[0])
							{
								this.ui_ctrls[_player_index].int_Prod[num6].gameObject.SetActive(true);
								this.ui_ctrls[_player_index].int_PanelProd[num6].SetActive(false);
								this.ui_ctrls[_player_index].int_ShelfDiscount[num6].gameObject.SetActive(false);
							}
							else if (component3.needsSource && component3.shelfController.boxControllers[0].itemIndex == boxController2.itemIndex && component3.shelfController.boxControllers[0].prodQnt < 8)
							{
								this.ui_ctrls[_player_index].int_Prod[num6].gameObject.SetActive(true);
								this.ui_ctrls[_player_index].int_PanelProd[num6].SetActive(true);
								this.ui_ctrls[_player_index].int_ShelfDiscount[num6].gameObject.SetActive(false);
								if (component3.needsSource && component3.shelfController.boxControllers[0].itemIndex == boxController2.itemIndex && component3.shelfController.boxControllers[0].prodQnt < 8)
								{
									this.ui_ctrls[_player_index].int_Prod_StoringImage[num6].sprite = this.ui_ctrls[_player_index].int_Prod_StoringSprites[0];
								}
								else
								{
									this.ui_ctrls[_player_index].int_Prod_StoringImage[num6].sprite = this.ui_ctrls[_player_index].int_Prod_StoringSprites[1];
								}
								if (component3.needsSource)
								{
									this.FeedProdUI(num6, component3.shelfController.boxControllers[0], _player_index);
								}
							}
							else
							{
								this.ui_ctrls[_player_index].int_Prod[num6].gameObject.SetActive(false);
							}
						}
						else
						{
							this.ui_ctrls[_player_index].int_Prod[num6].gameObject.SetActive(false);
						}
						this.ui_ctrls[_player_index].int_Discount[num6].gameObject.SetActive(false);
						this.ui_ctrls[_player_index].int_ShelfDiscount[num6].SetActive(false);
					}
				}
				else
				{
					this.ui_ctrls[_player_index].int_Simple.gameObject.SetActive(false);
					this.ui_ctrls[_player_index].int_Build.gameObject.SetActive(false);
					for (int num7 = 0; num7 < this.ui_ctrls[_player_index].int_Prod.Count; num7++)
					{
						this.ui_ctrls[_player_index].int_Prod[num7].gameObject.SetActive(false);
						this.ui_ctrls[_player_index].int_Discount[num7].gameObject.SetActive(false);
					}
				}
			}
			else
			{
				for (int num8 = 0; num8 < 3; num8++)
				{
					if (num8 < 1)
					{
						if (component3.needsSource && !component3.shelfController.boxControllers[0])
						{
							this.ui_ctrls[_player_index].int_Prod[num8].gameObject.SetActive(false);
						}
						else
						{
							this.ui_ctrls[_player_index].int_Prod[num8].gameObject.SetActive(true);
							this.ui_ctrls[_player_index].int_PanelProd[num8].SetActive(true);
							this.ui_ctrls[_player_index].int_ShelfDiscount[num8].gameObject.SetActive(false);
							this.ui_ctrls[_player_index].int_Prod_StoringImage[num8].sprite = this.ui_ctrls[_player_index].int_Prod_StoringSprites[0];
							if (component3.needsSource)
							{
								this.FeedProdUI(num8, component3.shelfController.boxControllers[0], _player_index);
							}
						}
					}
					else
					{
						this.ui_ctrls[_player_index].int_Prod[num8].gameObject.SetActive(false);
					}
					this.ui_ctrls[_player_index].int_Discount[num8].gameObject.SetActive(false);
					this.ui_ctrls[_player_index].int_ShelfDiscount[num8].SetActive(false);
				}
			}
		}
		else
		{
			this.ui_ctrls[_player_index].int_Simple.gameObject.SetActive(true);
			this.ui_ctrls[_player_index].int_Build.gameObject.SetActive(false);
			for (int num9 = 0; num9 < this.ui_ctrls[_player_index].int_Prod.Count; num9++)
			{
				this.ui_ctrls[_player_index].int_Prod[num9].gameObject.SetActive(false);
				this.ui_ctrls[_player_index].int_Discount[num9].gameObject.SetActive(false);
			}
		}
		Input_Manager.instance.RefreshInputHintsActive();
	}

	// Token: 0x060002DA RID: 730 RVA: 0x000198A0 File Offset: 0x00017AA0
	private void FeedProdUI(int _i, Box_Controller _box, int _player_index)
	{
		if (!_box.isProd)
		{
			this.ui_ctrls[_player_index].int_Prod_LifeSpanPanel[_i].gameObject.SetActive(false);
		}
		if (_box.isProd)
		{
			Prod_Controller prodPrefab = Inv_Manager.instance.GetProdPrefab(_box.itemIndex);
			this.ui_ctrls[_player_index].int_Prod_PanelQnt[_i].gameObject.SetActive(true);
			this.ui_ctrls[_player_index].int_Prod_PanelQnt[_i].color = prodPrefab.prodColors[1];
			this.ui_ctrls[_player_index].int_Prod_ItemPrice[_i].transform.parent.gameObject.SetActive(true);
			this.ui_ctrls[_player_index].int_Prod_ItemPrice[_i].text = Inv_Manager.instance.GetProdSellPrice(_box.itemIndex).ToString();
			this.ui_ctrls[_player_index].int_Prod_ItemQnt[_i].text = "x" + _box.prodQnt.ToString();
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].gameObject.SetActive(true);
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].sprite = Inv_Manager.instance.GetProdSprite(_box.itemIndex);
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].SetNativeSize();
			this.ui_ctrls[_player_index].int_Prod_PanelName[_i].color = prodPrefab.prodColors[0];
			this.ui_ctrls[_player_index].int_Prod_ItemName[_i].text = Inv_Manager.instance.GetProdName(_box.itemIndex, true);
			if (prodPrefab.needsRefrigerator)
			{
				this.ui_ctrls[_player_index].int_Prod_RefrigeratedImage[_i].gameObject.SetActive(true);
				this.ui_ctrls[_player_index].int_Prod_RefrigeratedImage[_i].color = prodPrefab.prodColors[0];
			}
			else
			{
				this.ui_ctrls[_player_index].int_Prod_RefrigeratedImage[_i].gameObject.SetActive(false);
			}
			this.FeedProdDiscount(_i, _box.itemIndex, _player_index);
			this.FeedLifeSpanUI(_i, _box, _player_index);
			return;
		}
		if (_box.isShelf)
		{
			Shelf_Controller itemShelf = Inv_Manager.instance.GetItemShelf(_box.itemIndex);
			this.ui_ctrls[_player_index].int_Prod_PanelQnt[_i].gameObject.SetActive(true);
			this.ui_ctrls[_player_index].int_Prod_PanelQnt[_i].color = itemShelf.itemColor[1];
			this.ui_ctrls[_player_index].int_Prod_ItemPrice[_i].transform.parent.gameObject.SetActive(false);
			this.ui_ctrls[_player_index].int_Prod_ItemQnt[_i].text = "x" + _box.prodQnt.ToString();
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].gameObject.SetActive(true);
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].sprite = itemShelf.itemSprite;
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].SetNativeSize();
			this.ui_ctrls[_player_index].int_Prod_PanelName[_i].color = itemShelf.itemColor[0];
			this.ui_ctrls[_player_index].int_Prod_ItemName[_i].text = "Shelf";
			if (itemShelf.isRefrigerated)
			{
				this.ui_ctrls[_player_index].int_Prod_RefrigeratedImage[_i].gameObject.SetActive(true);
				this.ui_ctrls[_player_index].int_Prod_RefrigeratedImage[_i].color = itemShelf.itemColor[0];
			}
			else
			{
				this.ui_ctrls[_player_index].int_Prod_RefrigeratedImage[_i].gameObject.SetActive(false);
			}
			this.FeedProdDiscount(_i, _box.itemIndex, _player_index);
			return;
		}
		if (_box.isDecor)
		{
			Decor_Controller itemDecor = Inv_Manager.instance.GetItemDecor(_box.itemIndex);
			this.ui_ctrls[_player_index].int_Prod_PanelQnt[_i].gameObject.SetActive(true);
			this.ui_ctrls[_player_index].int_Prod_PanelQnt[_i].color = itemDecor.itemColor[1];
			this.ui_ctrls[_player_index].int_Prod_ItemPrice[_i].transform.parent.gameObject.SetActive(false);
			this.ui_ctrls[_player_index].int_Prod_ItemQnt[_i].text = "x" + _box.prodQnt.ToString();
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].gameObject.SetActive(true);
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].sprite = itemDecor.itemSprite;
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].SetNativeSize();
			this.ui_ctrls[_player_index].int_Prod_PanelName[_i].color = itemDecor.itemColor[0];
			this.ui_ctrls[_player_index].int_Prod_ItemName[_i].text = "Decoration";
			this.ui_ctrls[_player_index].int_Prod_RefrigeratedImage[_i].gameObject.SetActive(false);
			this.FeedProdDiscount(_i, _box.itemIndex, _player_index);
			return;
		}
		if (_box.isWall)
		{
			WallPaint_Controller itemWallPaint = Inv_Manager.instance.GetItemWallPaint(_box.itemIndex);
			this.ui_ctrls[_player_index].int_Prod_PanelQnt[_i].gameObject.SetActive(true);
			this.ui_ctrls[_player_index].int_Prod_PanelQnt[_i].color = itemWallPaint.itemColor[1];
			this.ui_ctrls[_player_index].int_Prod_ItemPrice[_i].transform.parent.gameObject.SetActive(false);
			this.ui_ctrls[_player_index].int_Prod_ItemQnt[_i].text = "x" + _box.prodQnt.ToString();
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].gameObject.SetActive(true);
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].sprite = itemWallPaint.itemSprite;
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].SetNativeSize();
			this.ui_ctrls[_player_index].int_Prod_PanelName[_i].color = itemWallPaint.itemColor[0];
			this.ui_ctrls[_player_index].int_Prod_ItemName[_i].text = "Paint";
			this.ui_ctrls[_player_index].int_Prod_RefrigeratedImage[_i].gameObject.SetActive(false);
			this.FeedProdDiscount(_i, -1, _player_index);
			return;
		}
		if (_box.isFloor)
		{
			Floor_Controller itemFloor = Inv_Manager.instance.GetItemFloor(_box.itemIndex);
			this.ui_ctrls[_player_index].int_Prod_PanelQnt[_i].gameObject.SetActive(true);
			this.ui_ctrls[_player_index].int_Prod_PanelQnt[_i].color = itemFloor.itemColor[1];
			this.ui_ctrls[_player_index].int_Prod_ItemPrice[_i].transform.parent.gameObject.SetActive(false);
			this.ui_ctrls[_player_index].int_Prod_ItemQnt[_i].text = "x" + _box.prodQnt.ToString();
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].gameObject.SetActive(true);
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].sprite = itemFloor.itemSprite;
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].SetNativeSize();
			this.ui_ctrls[_player_index].int_Prod_PanelName[_i].color = itemFloor.itemColor[0];
			this.ui_ctrls[_player_index].int_Prod_ItemName[_i].text = "Floor";
			this.ui_ctrls[_player_index].int_Prod_RefrigeratedImage[_i].gameObject.SetActive(false);
			this.FeedProdDiscount(_i, -1, _player_index);
			return;
		}
		if (_box.isUtil)
		{
			Util_Controller itemUtil = Inv_Manager.instance.GetItemUtil(_box.itemIndex);
			this.ui_ctrls[_player_index].int_Prod_PanelQnt[_i].gameObject.SetActive(true);
			this.ui_ctrls[_player_index].int_Prod_PanelQnt[_i].color = itemUtil.itemColor[1];
			this.ui_ctrls[_player_index].int_Prod_ItemPrice[_i].transform.parent.gameObject.SetActive(false);
			this.ui_ctrls[_player_index].int_Prod_ItemQnt[_i].text = "x" + _box.prodQnt.ToString();
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].gameObject.SetActive(true);
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].sprite = itemUtil.itemSprite;
			this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].SetNativeSize();
			this.ui_ctrls[_player_index].int_Prod_PanelName[_i].color = itemUtil.itemColor[0];
			this.ui_ctrls[_player_index].int_Prod_ItemName[_i].text = Inv_Manager.instance.GetItemName(Inv_Manager.instance.GetItemUtil(_box.itemIndex).gameObject, true);
			this.ui_ctrls[_player_index].int_Prod_RefrigeratedImage[_i].gameObject.SetActive(false);
			this.FeedProdDiscount(_i, _box.itemIndex, _player_index);
		}
	}

	// Token: 0x060002DB RID: 731 RVA: 0x0001A3A8 File Offset: 0x000185A8
	private void FeedProdUI(int _i, Prod_Controller _prod, int _qnt, int _player_index)
	{
		this.ui_ctrls[_player_index].int_Prod_PanelQnt[_i].gameObject.SetActive(true);
		this.ui_ctrls[_player_index].int_Prod_PanelQnt[_i].color = _prod.prodColors[1];
		this.ui_ctrls[_player_index].int_Prod_ItemPrice[_i].transform.parent.gameObject.SetActive(true);
		this.ui_ctrls[_player_index].int_Prod_ItemPrice[_i].text = Inv_Manager.instance.GetProdSellPrice(_prod.prodIndex).ToString();
		this.ui_ctrls[_player_index].int_Prod_ItemQnt[_i].text = "x" + _qnt.ToString();
		this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].sprite = Inv_Manager.instance.GetProdSprite(_prod.prodIndex);
		this.ui_ctrls[_player_index].int_Prod_ItemImage[_i].SetNativeSize();
		this.ui_ctrls[_player_index].int_Prod_PanelName[_i].color = _prod.prodColors[0];
		this.ui_ctrls[_player_index].int_Prod_ItemName[_i].text = Inv_Manager.instance.GetProdName(_prod.prodIndex, true);
		if (_prod.needsRefrigerator)
		{
			this.ui_ctrls[_player_index].int_Prod_RefrigeratedImage[_i].gameObject.SetActive(true);
			this.ui_ctrls[_player_index].int_Prod_RefrigeratedImage[_i].color = _prod.prodColors[0];
		}
		else
		{
			this.ui_ctrls[_player_index].int_Prod_RefrigeratedImage[_i].gameObject.SetActive(false);
		}
		this.FeedProdDiscount(_i, _prod.prodIndex, _player_index);
		this.FeedLifeSpanUI(_i, _prod, _player_index);
	}

	// Token: 0x060002DC RID: 732 RVA: 0x0001A5C4 File Offset: 0x000187C4
	private void FeedProdDiscount(int _i, int _prodIndex, int _player_index)
	{
		if (_prodIndex == -1)
		{
			this.ui_ctrls[_player_index].int_Prod_DiscountPanel[_i].gameObject.SetActive(false);
			return;
		}
		int prodDiscountLevel = Inv_Manager.instance.GetProdDiscountLevel(_prodIndex);
		if (prodDiscountLevel == -1)
		{
			this.ui_ctrls[_player_index].int_Prod_DiscountPanel[_i].gameObject.SetActive(false);
			return;
		}
		this.ui_ctrls[_player_index].int_Prod_DiscountPanelOff[_i].color = this.ui_ctrls[_player_index].int_Discount_ColorBackground[prodDiscountLevel];
		this.ui_ctrls[_player_index].int_Prod_DiscountValue[_i].text = Inv_Manager.instance.prod_DiscountValue[prodDiscountLevel].ToString() + "%";
		this.ui_ctrls[_player_index].int_Prod_DiscountValue[_i].color = this.ui_ctrls[_player_index].int_Discount_ColorText[prodDiscountLevel];
		this.ui_ctrls[_player_index].int_Prod_DiscountPrice[_i].text = Inv_Manager.instance.GetProdSellPriceDiscounted(_prodIndex).ToString();
		this.ui_ctrls[_player_index].int_Prod_DiscountPanel[_i].gameObject.SetActive(true);
	}

	// Token: 0x060002DD RID: 733 RVA: 0x0001A720 File Offset: 0x00018920
	private void FeedDiscountUI(int _discountLevel, int _player_index)
	{
		for (int i = 0; i < 3; i++)
		{
			this.ui_ctrls[_player_index].int_PanelDiscount[i].color = this.ui_ctrls[_player_index].int_Discount_ColorBackground[_discountLevel];
			this.ui_ctrls[_player_index].int_Discount_PanelOff[i].color = this.ui_ctrls[_player_index].int_Discount_ColorText[_discountLevel];
			this.ui_ctrls[_player_index].int_Discount_Value[i].color = this.ui_ctrls[_player_index].int_Discount_ColorText[_discountLevel];
			this.ui_ctrls[_player_index].int_Discount_Value[i].text = Inv_Manager.instance.prod_DiscountValue[_discountLevel].ToString();
			this.ui_ctrls[_player_index].int_Discount_Percentage[i].color = this.ui_ctrls[_player_index].int_Discount_ColorText[_discountLevel];
			this.ui_ctrls[_player_index].int_Discount_Off[i].color = this.ui_ctrls[_player_index].int_Discount_ColorBackground[_discountLevel];
		}
	}

	// Token: 0x060002DE RID: 734 RVA: 0x0001A870 File Offset: 0x00018A70
	private void ResetDiscountUI(int _player_index)
	{
		for (int i = 0; i < 3; i++)
		{
			this.ui_ctrls[_player_index].int_PanelDiscount[i].color = this.ui_ctrls[_player_index].int_Discount_ColorBackground[i];
			this.ui_ctrls[_player_index].int_Discount_PanelOff[i].color = this.ui_ctrls[_player_index].int_Discount_ColorText[i];
			this.ui_ctrls[_player_index].int_Discount_Value[i].color = this.ui_ctrls[_player_index].int_Discount_ColorText[i];
			this.ui_ctrls[_player_index].int_Discount_Value[i].text = Inv_Manager.instance.prod_DiscountValue[i].ToString();
			this.ui_ctrls[_player_index].int_Discount_Percentage[i].color = this.ui_ctrls[_player_index].int_Discount_ColorText[i];
			this.ui_ctrls[_player_index].int_Discount_Off[i].color = this.ui_ctrls[_player_index].int_Discount_ColorBackground[i];
		}
	}

	// Token: 0x060002DF RID: 735 RVA: 0x0001A9C0 File Offset: 0x00018BC0
	private void FeedLifeSpanUI(int _i, Box_Controller _box, int _player_index)
	{
		if (!Inv_Manager.instance.GetProdPrefab(_box.itemIndex).lifeSpan)
		{
			this.ui_ctrls[_player_index].int_Prod_LifeSpanPanel[_i].gameObject.SetActive(false);
			return;
		}
		this.ui_ctrls[_player_index].int_Prod_LifeSpanPanel[_i].gameObject.SetActive(true);
		if (_box.lifeSpanIndex >= Inv_Manager.instance.GetLifeSpanLevels(0))
		{
			this.ui_ctrls[_player_index].int_Prod_LifeSpanImage[_i].sprite = this.ui_ctrls[_player_index].int_Prod_LifeSpanSprites[0];
			return;
		}
		if (_box.lifeSpanIndex >= Inv_Manager.instance.GetLifeSpanLevels(1))
		{
			this.ui_ctrls[_player_index].int_Prod_LifeSpanImage[_i].sprite = this.ui_ctrls[_player_index].int_Prod_LifeSpanSprites[1];
			return;
		}
		if (_box.lifeSpanIndex >= Inv_Manager.instance.GetLifeSpanLevels(2))
		{
			this.ui_ctrls[_player_index].int_Prod_LifeSpanImage[_i].sprite = this.ui_ctrls[_player_index].int_Prod_LifeSpanSprites[2];
			return;
		}
		this.ui_ctrls[_player_index].int_Prod_LifeSpanImage[_i].sprite = this.ui_ctrls[_player_index].int_Prod_LifeSpanSprites[3];
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x0001AB34 File Offset: 0x00018D34
	private void FeedLifeSpanUI(int _i, Prod_Controller _prod, int _player_index)
	{
		if (!_prod.lifeSpan)
		{
			this.ui_ctrls[_player_index].int_Prod_LifeSpanPanel[_i].gameObject.SetActive(false);
			return;
		}
		this.ui_ctrls[_player_index].int_Prod_LifeSpanPanel[_i].gameObject.SetActive(true);
		if (_prod.lifeSpanIndex >= Inv_Manager.instance.GetLifeSpanLevels(0))
		{
			this.ui_ctrls[_player_index].int_Prod_LifeSpanImage[_i].sprite = this.ui_ctrls[_player_index].int_Prod_LifeSpanSprites[0];
			return;
		}
		if (_prod.lifeSpanIndex >= Inv_Manager.instance.GetLifeSpanLevels(1))
		{
			this.ui_ctrls[_player_index].int_Prod_LifeSpanImage[_i].sprite = this.ui_ctrls[_player_index].int_Prod_LifeSpanSprites[1];
			return;
		}
		if (_prod.lifeSpanIndex >= Inv_Manager.instance.GetLifeSpanLevels(2))
		{
			this.ui_ctrls[_player_index].int_Prod_LifeSpanImage[_i].sprite = this.ui_ctrls[_player_index].int_Prod_LifeSpanSprites[2];
			return;
		}
		this.ui_ctrls[_player_index].int_Prod_LifeSpanImage[_i].sprite = this.ui_ctrls[_player_index].int_Prod_LifeSpanSprites[3];
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x0001AC99 File Offset: 0x00018E99
	public void AddCashierController(Cashier_Controller _cashier, int _player_index)
	{
		if (_cashier && !this.ui_ctrls[_player_index].cashier_Controllers.Contains(_cashier))
		{
			this.ui_ctrls[_player_index].cashier_Controllers.Add(_cashier);
		}
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x0001ACD4 File Offset: 0x00018ED4
	public Cashier_Controller GetCashier(int _player_index)
	{
		if (this.ui_ctrls[_player_index].cashier_Controllers.Count > 0 && this.ui_ctrls[_player_index].cashier_Controllers[0])
		{
			return this.ui_ctrls[_player_index].cashier_Controllers[0];
		}
		return null;
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x0001AD34 File Offset: 0x00018F34
	public void UpdateCashierInteractors()
	{
		for (int i = 0; i < this.ui_ctrls.Count; i++)
		{
			if (i >= Player_Manager.instance.playerControllerList.Count)
			{
				return;
			}
			bool flag = false;
			if (this.ui_ctrls[i].cashier_Controllers.Count > 0)
			{
				for (int j = 0; j < this.ui_ctrls[i].cashier_Controllers.Count; j++)
				{
					if (this.ui_ctrls[i].cashier_Controllers[j])
					{
						Cashier_Controller cashier_Controller = this.ui_ctrls[i].cashier_Controllers[j];
						if (cashier_Controller.GetHasPlayer() && cashier_Controller.player_Controller.playerIndex == i)
						{
							flag = true;
							List<Prod_Controller> list = new List<Prod_Controller>(this.ui_ctrls[i].cashier_Controllers[j].GetProdList());
							List<int> list2 = new List<int>(this.ui_ctrls[i].cashier_Controllers[j].GetButtonList());
							Sprite[] buttonSprites = this.GetButtonSprites(i);
							for (int k = 0; k < this.ui_ctrls[i].int_Cashier_Simple.Count; k++)
							{
								if (k < list.Count && Menu_Manager.instance.GetMenuName() == "MainMenu")
								{
									this.ui_ctrls[i].int_Cashier_Simple[k].gameObject.SetActive(true);
									Vector3 v = this.cam_Game.WorldToScreenPoint(list[k].transform.position + Vector3.up * this.ui_ctrls[i].int_Cashier_positionToAdd_y);
									v.z = 0f;
									this.ui_ctrls[i].int_Cashier_Simple[k].anchoredPosition = this.GetPosAfterRender(v, false);
									this.ui_ctrls[i].int_Cashier_ButtonHint[k].sprite = buttonSprites[list2[k]];
									this.ui_ctrls[i].int_Cashier_ButtonHint[k].SetNativeSize();
									if (k == 0)
									{
										this.ui_ctrls[i].int_Cashier_Simple[k].localScale = Vector3.one;
									}
									else
									{
										this.ui_ctrls[i].int_Cashier_Simple[k].localScale = Vector3.one * 0.5f;
									}
								}
								else if (this.ui_ctrls[i].int_Cashier_Simple[k].gameObject.activeInHierarchy)
								{
									this.ui_ctrls[i].int_Cashier_Simple[k].gameObject.SetActive(false);
								}
							}
						}
					}
				}
			}
			if (!flag)
			{
				for (int l = 0; l < this.ui_ctrls[i].int_Cashier_Simple.Count; l++)
				{
					if (this.ui_ctrls[i].int_Cashier_Simple[l].gameObject.activeInHierarchy)
					{
						this.ui_ctrls[i].int_Cashier_Simple[l].gameObject.SetActive(false);
					}
				}
			}
		}
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x0001B09C File Offset: 0x0001929C
	public void SetCashierAnimator(string _price, string _animation, Cashier_Controller _cashier, int _player_index)
	{
		Vector3 v;
		if (_animation != "Tip")
		{
			v = this.cam_Game.WorldToScreenPoint(_cashier.GetComponent<Interaction_Controller>().ui_Pos[0].transform.position);
			v.z = 0f;
			this.ui_ctrls[_player_index].int_Cashier_Money.anchoredPosition = this.GetPosAfterRender(v, false);
			this.ui_ctrls[_player_index].int_Cashier_MoneyText.text = _price;
			this.ui_ctrls[_player_index].int_Cashier_MoneyAnim.PlayInFixedTime(_animation, -1, 0f);
			return;
		}
		Vector3 position = _cashier.GetComponent<Interaction_Controller>().ui_Pos[0].transform.position;
		position.y += 1f;
		v = this.cam_Game.WorldToScreenPoint(position);
		v.z = 0f;
		this.ui_ctrls[_player_index].int_Cashier_Tip.anchoredPosition = this.GetPosAfterRender(v, false);
		this.ui_ctrls[_player_index].int_Cashier_TipText.text = _price;
		this.ui_ctrls[_player_index].int_Cashier_TipAnim.PlayInFixedTime(_animation, -1, 0f);
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x0001B1E6 File Offset: 0x000193E6
	private Sprite[] GetButtonSprites(int _player_index)
	{
		if (Input_Manager.instance.GetScheme(_player_index) == "Keyboard&Mouse")
		{
			return this.ui_ctrls[_player_index].int_Prod_HintSprite_Keyboard;
		}
		return this.ui_ctrls[_player_index].int_Prod_HintSprite_Joystick;
	}

	// Token: 0x04000368 RID: 872
	public static Interactor_Manager instance;

	// Token: 0x04000369 RID: 873
	[SerializeField]
	private Camera cam_Game;

	// Token: 0x0400036A RID: 874
	[SerializeField]
	private Camera cam_UI;

	// Token: 0x0400036B RID: 875
	[SerializeField]
	private RectTransform interactor_Canvas;

	// Token: 0x0400036C RID: 876
	[SerializeField]
	private GameObject floor_interactor;

	// Token: 0x0400036D RID: 877
	public List<WorldUI_Controller> ui_ctrls = new List<WorldUI_Controller>();
}
