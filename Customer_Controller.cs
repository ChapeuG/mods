using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class Customer_Controller : MonoBehaviour
{
	// Token: 0x0600006C RID: 108 RVA: 0x00005728 File Offset: 0x00003928
	private void Awake()
	{
		this.char_Controller = base.GetComponent<Char_Controller>();
		this.talk_Controller = base.GetComponent<Talk_Controller>();
		this.skin_Controller = base.GetComponent<Skin_Controller>();
		if (this.talk_Controller)
		{
			this.talk_Controller.cust_Controller = this;
		}
		this.char_Controller.moveSpeedRun = this.char_Controller.moveSpeedWalk * 2f;
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00005790 File Offset: 0x00003990
	private void Start()
	{
		if (this.char_Controller.animator_)
		{
			this.char_Controller.animator_.SetFloat("Chair", this.char_Controller.chairAnim);
		}
		if (this.basket_Anim)
		{
			this.basket_Anim.gameObject.SetActive(true);
		}
		if (this.paperBag_Anim)
		{
			this.paperBag_Anim.gameObject.SetActive(false);
		}
		this.SetProdShoppingQnt();
		this.SetCooldown(1f);
		if (!this.char_Controller.isGenCustomer)
		{
			this.prodWantedNow_Index = Char_Manager.instance.GetAllCustomerProdWantedNow()[this.char_Controller.GetID()];
		}
		else
		{
			this.prodWantedNow_Index = -1;
		}
		if (this.prodWantedNow_Index <= -1)
		{
			float num = (float)UnityEngine.Random.Range(0, 100);
			if (this.char_Controller.isGenCustomer || num <= 50f)
			{
				this.CreateProdWantedNowNeed(false, true, 50f);
			}
			else
			{
				this.CreateProdWantedNowNeed(false, false, 50f);
			}
		}
		else
		{
			this.SetProdWantedIndex(this.prodWantedNow_Index);
		}
		this.cashierTime_Timer = 0f;
		if (this.char_Controller.isGenCustomer)
		{
			return;
		}
		Char_Manager.instance.GetLocalCustomer_Data(this.char_Controller.GetID()).visitIndex++;
		this.CreateReferences_FulfillFriendshipNeeds();
	}

	// Token: 0x0600006E RID: 110 RVA: 0x000058E5 File Offset: 0x00003AE5
	private void Update()
	{
		if (!Game_Manager.instance)
		{
			return;
		}
		this.UpdateAI();
		this.UpdateAwaits();
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00005900 File Offset: 0x00003B00
	private void UpdateAI()
	{
		if (!Game_Manager.instance.MayRun() && Game_Manager.instance.GetGameMode() != 0)
		{
			return;
		}
		if (!Game_Manager.instance.GetMartOpen())
		{
			if (!this.doneShopping)
			{
				this.GetNavPath(false);
			}
			this.doneShopping = true;
		}
		this.cooldown_Timer -= Time.deltaTime;
		if (this.cooldown_Timer > 0f)
		{
			return;
		}
		if (!this.prod_Interest)
		{
			this.GetProdInterest(true);
		}
		if ((this.prod_Interest || this.doneShopping || this.doneCashier) && this.clientPlaceIndex == -1)
		{
			if (this.navPath == null)
			{
				this.navPath = new List<GameObject>();
			}
			if (this.navPath.Count == 0)
			{
				this.GetNavPath(true);
				return;
			}
			if (this.navPathIndex < this.navPath.Count)
			{
				Vector3 position = base.transform.position;
				position.y = 0f;
				if (this.char_Controller.player_Controller)
				{
					Vector3 normalized = (this.char_Controller.player_Controller.transform.position - base.transform.position).normalized;
					this.char_Controller.Rotate(normalized);
					this.char_Controller.animator_.SetBool("Thinking", false);
					this.char_Controller.copyMove = normalized;
					return;
				}
				if (Vector3.Distance(position, this.navPath[this.navPathIndex].transform.position) > this.navPathDistance)
				{
					Vector3 normalized2 = (this.navPath[this.navPathIndex].transform.position - base.transform.position).normalized;
					this.char_Controller.Move(normalized2);
					this.char_Controller.animator_.SetBool("Thinking", false);
					this.char_Controller.copyMove = normalized2;
					this.char_Controller.findNewPathTime_Timer += Time.deltaTime;
					if (this.char_Controller.findNewPathTime_Timer > this.char_Controller.findNewPathTime * 2f)
					{
						if (this.prod_Interest)
						{
							this.GetProdInterest(true);
							return;
						}
						this.GetNavPath(true);
						return;
					}
				}
				else if (this.NextNavSphere())
				{
					if (this.doneCashier)
					{
						this.char_Controller.DestroyChar();
						return;
					}
					if (this.doneShopping)
					{
						if (this.cashier_Controller)
						{
							if (this.prodWantedNow_Index >= 0 && this.prodWanted_ctrl)
							{
								this.prodWanted_ctrl.DestroyCtrl();
							}
							this.cashier_Controller.SetClient(this);
							return;
						}
					}
					else if (this.util_Interest)
					{
						Vector3 normalized3 = (this.util_Interest.transform.position - base.transform.position).normalized;
						this.char_Controller.Rotate(normalized3);
						this.char_Controller.copyMove = normalized3;
						if (Vector3.Angle(base.transform.forward, this.util_Interest.transform.position - position) < 10f)
						{
							this.ThinkAboutUtil();
							return;
						}
					}
					else
					{
						Vector3 normalized4 = (this.prod_Interest.shelf_Controller.transform.position - base.transform.position).normalized;
						this.char_Controller.Rotate(normalized4);
						this.char_Controller.copyMove = normalized4;
						if (Vector3.Angle(base.transform.forward, this.prod_Interest.shelf_Controller.transform.position - position) < 10f)
						{
							this.ThinkAboutProd();
							return;
						}
					}
				}
			}
		}
		else if (this.clientPlaceIndex != -1)
		{
			this.emotion_Angry_Timer += Time.deltaTime;
			if (this.emotion_Angry_Timer >= this.emotion_Andry_TIMER)
			{
				this.emotion_Angry_Timer = 0f;
				this.SetEmotionSum(-1, 100f);
			}
			GameObject clientPlace = this.cashier_Controller.GetClientPlace(this.clientPlaceIndex);
			if (clientPlace)
			{
				Vector3 position2 = base.transform.position;
				position2.y = 0f;
				if (Vector3.Distance(position2, clientPlace.transform.position) > this.navPathDistance * 0.5f)
				{
					Vector3 normalized5 = (clientPlace.transform.position - position2).normalized;
					this.char_Controller.Move(normalized5);
					this.char_Controller.animator_.SetBool("Thinking", false);
					this.char_Controller.copyMove = normalized5;
					return;
				}
				if (this.clientPlaceIndex == 0)
				{
					Vector3 normalized6 = (this.cashier_Controller.transform.position - position2).normalized;
					this.char_Controller.Rotate(normalized6);
					this.char_Controller.copyMove = normalized6;
					this.cashierTime_Timer += Time.deltaTime;
					return;
				}
				Vector3 normalized7 = (this.cashier_Controller.GetClientPlace(this.clientPlaceIndex - 1).transform.position - position2).normalized;
				this.char_Controller.Rotate(normalized7);
				this.char_Controller.copyMove = normalized7;
			}
		}
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00005E4C File Offset: 0x0000404C
	private void GetNavPath(bool _b)
	{
		this.navPathIndex = 0;
		if (!_b)
		{
			this.navPath.Clear();
			return;
		}
		if (Inv_Manager.instance.GetProdListOnShelves().Count <= 0)
		{
			this.doneShopping = true;
		}
		if (this.doneCashier)
		{
			this.navPath = Nav_Manager.instance.GetNavPathExit(base.transform.position, false);
			return;
		}
		if (this.doneShopping)
		{
			if (this.prod_BuyList.Count <= 0)
			{
				this.doneCashier = true;
				this.navPath = Nav_Manager.instance.GetNavPathExit(base.transform.position, false);
				return;
			}
			this.cashier_Controller = Interactor_Manager.instance.GetCashier(0);
			if (this.cashier_Controller)
			{
				this.navPath = Nav_Manager.instance.GetNavPathCashier(this.cashier_Controller, base.transform.position, false);
				return;
			}
			this.GetNavPath(true);
			return;
		}
		else
		{
			if (this.util_Interest)
			{
				Vector3 endPos = this.util_Interest.transform.position + this.util_Interest.transform.forward * 2f;
				this.navPath = Nav_Manager.instance.GetNavPath(base.transform.position, endPos, false);
				return;
			}
			if (this.prod_Interest && this.prod_Interest.shelf_Controller)
			{
				Vector3 endPos2 = this.prod_Interest.shelf_Controller.transform.position + this.prod_Interest.shelf_Controller.transform.forward * 2f;
				this.navPath = Nav_Manager.instance.GetNavPath(base.transform.position, endPos2, false);
			}
			return;
		}
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00006004 File Offset: 0x00004204
	private bool NextNavSphere()
	{
		if (this.navPath.Count == 0)
		{
			return false;
		}
		this.char_Controller.findNewPathTime_Timer = 0f;
		bool result = false;
		if (this.navPathIndex < this.navPath.Count - 1)
		{
			this.navPathIndex++;
		}
		if (this.navPathIndex >= this.navPath.Count - 1)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00006070 File Offset: 0x00004270
	private void ThinkAboutProd()
	{
		if (!this.prod_Interest)
		{
			return;
		}
		if (this.prodThinking_Timer == 0f)
		{
			this.prodThinking_Timer = UnityEngine.Random.Range(this.prodThinking_TimeMin, this.prodThinking_TimeMax);
		}
		this.char_Controller.animator_.SetBool("Thinking", true);
		this.prodThinking_Timer -= Time.deltaTime;
		if (this.prodThinking_Timer <= 0f)
		{
			if (this.prod_Interest.shelf_Controller.GetHasAnyEmptyHeight())
			{
				this.fs_Found_EmptyShelf = true;
			}
			if (this.prod_Interest.shelf_Controller.Get_HasAnyProdDifferentFromCategory(Inv_Manager.ProdType.Sport))
			{
				this.fs_Found_ExclusiveCategory_Other = true;
			}
			if (Inv_Manager.instance.GetProdDiscountLevel(this.prod_Interest.prodIndex) == -1)
			{
				this.fs_Found_UndiscountedProd = true;
			}
			this.prodThinking_Timer = 0f;
			float num = (float)UnityEngine.Random.Range(0, 100);
			if (this.GetProdPredilectionIndexes().Contains(Inv_Manager.instance.GetItemIndex(this.prod_Interest.gameObject)))
			{
				num = 0f;
			}
			if (this.prodsConsumedForFreeIndexes.Contains(Inv_Manager.instance.GetItemIndex(this.prod_Interest.gameObject)))
			{
				num = 0f;
			}
			int prodDiscountLevel = Inv_Manager.instance.GetProdDiscountLevel(this.prod_Interest.prodIndex);
			if (this.prod_Interest != null && this.prod_Interest.lifeSpan && this.prod_Interest.lifeSpanIndex <= 0)
			{
				this.GetProdInterest(false);
				if (this.prod_Interest != null && this.prod_Interest.shelf_Controller != null)
				{
					this.Set_BadInteraction(this.prod_Interest.shelf_Controller.GetComponent<Interaction_Controller>(), -1);
				}
			}
			else if (num <= Inv_Manager.instance.Get_ProdThinking_BuyOdd(this.prod_Interest.prodIndex) || this.prod_Interest.prodIndex == this.prodWantedNow_Index)
			{
				if (this.prod_Interest.prodIndex == this.prodWantedNow_Index)
				{
					this.SetEmotionSum(1, 30f);
					this.SetProdWantedIndex(-2);
				}
				this.BuyProd();
				if (prodDiscountLevel == 0)
				{
					this.SetEmotionSum(0, 100f);
				}
				else if (prodDiscountLevel == 1)
				{
					this.SetEmotionSum(1, 100f);
				}
				else if (prodDiscountLevel == 2)
				{
					this.SetEmotionSum(2, 100f);
				}
			}
			else
			{
				this.GetProdInterest(false);
			}
			if (UnityEngine.Random.Range(0, 100) > Char_Manager.instance.Get_ContinueBuyingOdd(this.emotionIndex))
			{
				this.doneShopping = true;
			}
		}
	}

	// Token: 0x06000073 RID: 115 RVA: 0x000062E0 File Offset: 0x000044E0
	private void GetProdInterest(bool _b)
	{
		if (this.doneShopping || this.doneCashier)
		{
			return;
		}
		if (this.prod_BuyList.Count >= this.prodShopping_Qnt)
		{
			this.prod_Interest = null;
			this.GoToPayProds();
			return;
		}
		if (!_b)
		{
			this.prod_Interest = null;
			this.GetNavPath(false);
			return;
		}
		List<Prod_Controller> list = new List<Prod_Controller>(Inv_Manager.instance.GetProdListOnShelvesWithDailyDealsAndProdWantedNow(5, this.prodWantedNow_Index, this.prodPredilection_NeedMultiplier));
		foreach (Prod_Controller prod_Controller in new List<Prod_Controller>(list))
		{
			int num = Inv_Manager.instance.Get_ProdMaxBuyQnt(prod_Controller.prodIndex);
			int num2 = 0;
			foreach (int num3 in this.prod_BuyList)
			{
				if (prod_Controller.prodIndex == num3)
				{
					num2++;
				}
			}
			if (num2 >= num)
			{
				list.Remove(prod_Controller);
			}
		}
		foreach (Prod_Controller prod_Controller2 in new List<Prod_Controller>(Char_Manager.instance.GetAllCustomersInterests()))
		{
			list.Remove(prod_Controller2);
			Shelf_Controller shelf_Controller = prod_Controller2.shelf_Controller;
			if (shelf_Controller != null)
			{
				for (int i = 0; i < shelf_Controller.height; i++)
				{
					for (int j = 0; j < shelf_Controller.width; j++)
					{
						if (shelf_Controller.prodControllers[i, j] != null)
						{
							list.Remove(shelf_Controller.prodControllers[i, j]);
						}
					}
				}
			}
		}
		if (list.Count > 0)
		{
			foreach (Prod_Controller prod_Controller3 in list)
			{
				if (prod_Controller3.prodIndex == this.prodWantedNow_Index)
				{
					this.prod_Interest = prod_Controller3;
				}
			}
			if (this.prod_Interest == null)
			{
				this.prod_Interest = list[UnityEngine.Random.Range(0, list.Count - 1)];
			}
		}
		else if (!this.doneShopping)
		{
			if (this.prod_BuyList.Count > 0)
			{
				this.prod_Interest = null;
				this.clientPlaceIndex = -1;
				this.GoToPayProds();
				return;
			}
			this.doneShopping = true;
			this.doneCashier = true;
			this.clientPlaceIndex = -1;
			this.GetNavPath(true);
			return;
		}
		if (this.prod_Interest)
		{
			this.GetNavPath(true);
			return;
		}
		this.GetNavPath(false);
	}

	// Token: 0x06000074 RID: 116 RVA: 0x000065A4 File Offset: 0x000047A4
	private void SetProdShoppingQnt()
	{
		this.prodShopping_Qnt = UnityEngine.Random.Range(this.prodShopping_Min, this.prodShopping_Max + 1);
	}

	// Token: 0x06000075 RID: 117 RVA: 0x000065C0 File Offset: 0x000047C0
	private void BuyProd()
	{
		if (!this.prod_Interest)
		{
			return;
		}
		if (!this.prod_Interest.shelf_Controller)
		{
			return;
		}
		if (this.prod_Interest.shelf_Controller.BuyProd(this.prod_Interest))
		{
			this.AddProdToBuyList(this.prod_Interest.prodIndex);
			this.GetProdInterest(false);
			this.SetCooldown();
		}
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00006624 File Offset: 0x00004824
	private void AddProdToBuyList(int _prodIndex)
	{
		this.prod_BuyList.Add(_prodIndex);
		this.AddProductToFulfillFriendship(Inv_Manager.instance.GetProdPrefab(_prodIndex));
		this.Check_LifeAchievement();
		if (this.basket_Anim)
		{
			this.basket_Anim.PlayInFixedTime("AddProd", -1, 0f);
		}
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00006677 File Offset: 0x00004877
	private void ClearProdToBuyList()
	{
		this.prod_BuyList.Clear();
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00006684 File Offset: 0x00004884
	private void GoToPayProds()
	{
		this.doneShopping = true;
		this.GetNavPath(true);
		this.cashierTime_Bonus = (float)this.prod_BuyList.Count * 1.2f;
	}

	// Token: 0x06000079 RID: 121 RVA: 0x000066AC File Offset: 0x000048AC
	private int Get_TipByBuyListValue(float _margin = 0.1f)
	{
		float num = 0f;
		foreach (int index in this.prod_BuyList)
		{
			num += (float)Inv_Manager.instance.prod_Prefabs[index].prodPrice;
		}
		return Mathf.Clamp(Mathf.CeilToInt(num * _margin), 1, 99999999);
	}

	// Token: 0x0600007A RID: 122 RVA: 0x0000672C File Offset: 0x0000492C
	public void FinishedCashier()
	{
		this.clientPlaceIndex = -1;
		this.doneCashier = true;
		if (this.emotionIndex >= 1 && this.cashier_Controller.got_right_buttons)
		{
			this.SetEmotionSum(1, 100f);
			int num = this.Get_TipByBuyListValue(0.1f);
			if (!this.char_Controller.isGenCustomer && Char_Manager.instance.GetCustomerLifeAchievementState(this.char_Controller.charID))
			{
				num = this.Get_TipByBuyListValue(0.2f);
			}
			if (this.cashier_Controller.player_Controller == null)
			{
				num = Mathf.FloorToInt((float)num * 0.5f);
			}
			Finances_Manager.instance.AddMoney((float)num);
			Finances_Manager.instance.AddTo_InSales((float)num);
			int player_index = 0;
			if (this.cashier_Controller.player_Controller != null)
			{
				player_index = this.cashier_Controller.player_Controller.playerIndex;
			}
			Interactor_Manager.instance.SetCashierAnimator("+" + num.ToString(), "Tip", this.cashier_Controller, player_index);
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_Cashier_RightButton);
		}
		this.GiveTaigaRate();
		this.SetBasket(false, true);
		this.GetNavPath(true);
		this.Check_LifeAchievement();
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00006860 File Offset: 0x00004A60
	public void ReceiveNewClientPlaceIndex(int _index, Cashier_Controller _cashier)
	{
		if (this.clientPlaceIndex == _index)
		{
			return;
		}
		this.clientPlaceIndex = _index;
		if (_index == 0)
		{
			_cashier.SetProdList(this.prod_BuyList);
		}
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00006882 File Offset: 0x00004A82
	public void SetBasket(bool _b, bool _c)
	{
		this.basket_Anim.gameObject.SetActive(_b);
		this.paperBag_Anim.gameObject.SetActive(_c);
	}

	// Token: 0x0600007D RID: 125 RVA: 0x000068A6 File Offset: 0x00004AA6
	public void SetCooldown(float _cooldown)
	{
		this.cooldown_Timer = _cooldown;
	}

	// Token: 0x0600007E RID: 126 RVA: 0x000068AF File Offset: 0x00004AAF
	public void SetCooldown()
	{
		this.cooldown_Timer = this.cooldown;
	}

	// Token: 0x0600007F RID: 127 RVA: 0x000068C0 File Offset: 0x00004AC0
	public void SetEmotionIndex(int _value, float _odd = 100f)
	{
		if ((float)UnityEngine.Random.Range(0, 100) <= _odd)
		{
			this.emotionIndex = _value;
		}
		Sprite newSprite = Char_Manager.instance.emojiEmotion_Sprites[this.emotionIndex];
		this.emotion_ctrl.SetEmojiSprite(newSprite, false);
		Material material_Eye = Char_Manager.instance.faceEmotion_Sprites[this.emotionIndex];
		if (this.skin_Controller)
		{
			this.skin_Controller.SetMaterial_Eye(material_Eye);
			return;
		}
		if (this.face_Controller)
		{
			this.face_Controller.SetMaterial_Eye(material_Eye);
		}
	}

	// Token: 0x06000080 RID: 128 RVA: 0x0000694C File Offset: 0x00004B4C
	public void SetEmotionSum(int _value, float _odd = 100f)
	{
		if ((float)UnityEngine.Random.Range(0, 100) <= _odd)
		{
			this.emotionIndex += _value;
			if (_value > 0)
			{
				this.prodShopping_Qnt = Mathf.Clamp(this.prodShopping_Qnt + 1, 0, this.prodShopping_Max + 2);
			}
		}
		this.emotionIndex = Mathf.Clamp(this.emotionIndex, 0, Char_Manager.instance.emojiEmotion_Sprites.Count - 1);
		this.SetEmotionIndex(this.emotionIndex, 100f);
	}

	// Token: 0x06000081 RID: 129 RVA: 0x000069C7 File Offset: 0x00004BC7
	public void GiveTaigaRate()
	{
		this.SetEmotionIndex(this.emotionIndex, 100f);
		Score_Manager.instance.ReceiveTaigaRateIndex(this.emotionIndex);
		this.gaveTaigaRate = true;
	}

	// Token: 0x06000082 RID: 130 RVA: 0x000069F1 File Offset: 0x00004BF1
	public List<Prod_Controller> GetProdPredilectionList()
	{
		return this.prodPredilection_Prefabs;
	}

	// Token: 0x06000083 RID: 131 RVA: 0x000069FC File Offset: 0x00004BFC
	public List<int> GetProdPredilectionIndexes()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.prodPredilection_Prefabs.Count; i++)
		{
			if (this.prodPredilection_Prefabs[i])
			{
				list.Add(Inv_Manager.instance.GetItemIndex(this.prodPredilection_Prefabs[i].gameObject));
			}
		}
		return list;
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00006A5A File Offset: 0x00004C5A
	public int GetProdWantedIndex()
	{
		return this.prodWantedNow_Index;
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00006A64 File Offset: 0x00004C64
	public void SetProdWantedIndex(int _index)
	{
		this.prodWantedNow_Index = _index;
		if (!this.char_Controller.isGenCustomer)
		{
			Char_Manager.instance.SetCustomerProdWantedNow(this.char_Controller.GetID(), this.prodWantedNow_Index);
		}
		if ((_index <= -1 && this.prodWanted_ctrl) || this.prodWanted_ctrl || this.doneShopping)
		{
			this.prodWanted_ctrl.DestroyCtrl();
			this.prodWanted_ctrl = null;
		}
		if (_index <= -1 || this.doneShopping)
		{
			return;
		}
		if (this.prodWanted_ctrl == null)
		{
			Char_Manager.instance.Create_ProdWantedCtrl(this.char_Controller);
		}
		Sprite prodSprite = Inv_Manager.instance.GetProdSprite(this.prodWantedNow_Index);
		this.prodWanted_ctrl.SetEmojiSprite(prodSprite, true);
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00006B24 File Offset: 0x00004D24
	public void CreateProdWantedNowNeed(bool _onlyByPredilection, bool _onlyInStore, float _odd = 100f)
	{
		if ((float)UnityEngine.Random.Range(0, 100) > _odd)
		{
			return;
		}
		if (this.prodWantedNow_Index != -1)
		{
			return;
		}
		List<int> list = new List<int>();
		if (_onlyByPredilection)
		{
			for (int i = 0; i < this.prodPredilection_Prefabs.Count; i++)
			{
				if (this.prodPredilection_Prefabs[i])
				{
					list.Add(Inv_Manager.instance.GetItemIndex(this.prodPredilection_Prefabs[i].gameObject));
				}
			}
		}
		else if (_onlyInStore)
		{
			list = Inv_Manager.instance.GetProdIndexesInStore();
		}
		else
		{
			foreach (int num in new List<int>(Inv_Manager.instance.unlockedProdsTillThisDay))
			{
				if (Inv_Manager.instance.prod_Prefabs[num].prodType != Inv_Manager.ProdType.Valentines && Inv_Manager.instance.prod_Prefabs[num].prodType != Inv_Manager.ProdType.Christmas && Inv_Manager.instance.prod_Prefabs[num].prodType != Inv_Manager.ProdType.Halloween && Inv_Manager.instance.prod_Prefabs[num].prodType != Inv_Manager.ProdType.Easter)
				{
					list.Add(num);
				}
			}
		}
		if (list.Count > 0)
		{
			int num2 = UnityEngine.Random.Range(0, list.Count);
			this.SetProdWantedIndex(list[num2]);
			if (_onlyByPredilection)
			{
				Char_Manager.instance.SetProdPreferenceUnlocked(this.char_Controller.GetID(), num2, true);
			}
		}
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00006CA8 File Offset: 0x00004EA8
	public void InteractWithUtil(Util_Controller _util)
	{
		int num = -1;
		int num2 = 0;
		if (_util.shelfController && _util.shelfController.boxControllers[0])
		{
			num = _util.shelfController.boxControllers[0].itemIndex;
			num2 = _util.shelfController.boxControllers[0].lifeSpanIndex;
		}
		if (_util.Interact(this))
		{
			if (num != -1)
			{
				this.prodsConsumedForFreeIndexes.Add(num);
				if (num == -1)
				{
					this.SetEmotionSum(1, 100f);
				}
				else if (num2 <= 0)
				{
					this.SetEmotionSum(-2, 100f);
				}
				else
				{
					this.SetEmotionSum(1, 50f);
					this.AddFreeProductToFulfillFriendship(Inv_Manager.instance.GetProdPrefab(num));
				}
			}
			this.prod_Interest = null;
			this.GetNavPath(false);
		}
		this.util_Interest = null;
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00006D70 File Offset: 0x00004F70
	public void TryUtilInterest(Util_Controller _util)
	{
		if (this.utilsChecked.Contains(_util))
		{
			return;
		}
		List<Char_Controller> list = new List<Char_Controller>(Char_Manager.instance.customer_Controllers);
		for (int i = 0; i < list.Count; i++)
		{
			Util_Controller util_Controller = list[i].customer_Controller.util_Interest;
			if (util_Controller && util_Controller == _util)
			{
				return;
			}
		}
		int num = UnityEngine.Random.Range(0, 100);
		if (this.freeProdsForFriendship.Count > 0 && _util.shelfController && _util.shelfController.boxControllers[0])
		{
			int itemIndex = _util.shelfController.boxControllers[0].itemIndex;
			for (int j = 0; j < this.freeProdsForFriendship.Count; j++)
			{
				if (this.freeProdsForFriendship[j] && Inv_Manager.instance.GetItemIndex(this.freeProdsForFriendship[j].gameObject) == itemIndex)
				{
					num = 0;
				}
			}
		}
		if (num <= 30)
		{
			if (_util.shelfController && _util.shelfController.boxControllers[0])
			{
				this.utilsChecked.Add(_util);
				int itemIndex2 = _util.shelfController.boxControllers[0].itemIndex;
				if (!this.prodsConsumedForFreeIndexes.Contains(itemIndex2))
				{
					this.util_Interest = _util;
					this.prod_Interest = null;
				}
				else
				{
					this.util_Interest = null;
				}
			}
			else if (!_util.shelfController)
			{
				this.util_Interest = _util;
				this.prod_Interest = null;
			}
			else
			{
				this.util_Interest = null;
				this.prod_Interest = null;
			}
		}
		else
		{
			this.utilsChecked.Add(_util);
		}
		this.GetNavPath(true);
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00006F24 File Offset: 0x00005124
	private void ThinkAboutUtil()
	{
		if (!this.util_Interest)
		{
			return;
		}
		if (this.prodThinking_Timer == 0f)
		{
			this.prodThinking_Timer = UnityEngine.Random.Range(this.prodThinking_TimeMin, this.prodThinking_TimeMax);
		}
		this.char_Controller.animator_.SetBool("Thinking", true);
		this.prodThinking_Timer -= Time.deltaTime;
		if (this.prodThinking_Timer <= 0f)
		{
			this.prodThinking_Timer = 0f;
			this.InteractWithUtil(this.util_Interest);
		}
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00006FB0 File Offset: 0x000051B0
	public bool ReceiveItem(Prod_Controller _prod, int _lifeSpan, int _player_index)
	{
		this.GetProdPredilectionIndexes().Contains(Inv_Manager.instance.GetItemIndex(_prod.gameObject));
		string charText = "";
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		bool flag = false;
		if (Inv_Manager.instance.GetItemIndex(_prod.gameObject) == this.prodWantedNow_Index)
		{
			if (_lifeSpan <= 0)
			{
				this.SetEmotionSum(-2, 100f);
			}
			else
			{
				this.SetEmotionSum(1, 60f);
				this.AddProdToBuyList(this.prodWantedNow_Index);
			}
			this.SetProdWantedIndex(-2);
			return true;
		}
		if (Inv_Manager.instance.GetItemIndex(_prod.gameObject) != this.prodWantedNow_Index && this.char_Controller.isGenCustomer)
		{
			return false;
		}
		if (Inv_Manager.instance.GetItemIndex(_prod.gameObject) == 138)
		{
			this.Set_LifeAchievementBool(true);
			return false;
		}
		if (this.prodForTrade && Inv_Manager.instance.GetItemIndex(this.prodForTrade.gameObject) == Inv_Manager.instance.GetItemIndex(_prod.gameObject))
		{
			if (this.prodToGive)
			{
				Inv_Manager.instance.AddDelivery(Inv_Manager.instance.GetItemIndex(this.prodToGive.gameObject), 0, 1, true, 4);
			}
			else if (this.decorToGive)
			{
				Inv_Manager.instance.AddDelivery(Inv_Manager.instance.GetItemIndex(this.decorToGive.gameObject), 2, 1, true, 4);
			}
			charText = "Thank you! This is everything I needed. I'll have something special to be delivered to you soon!";
			list.Add("Awesome!");
			list2.Add("FinishTalk");
		}
		Talk_Manager.instance.Talk(charText, list, list2, 0, this.char_Controller, _player_index);
		if (!this.receiveItem_Received)
		{
			this.AddFreeProductToFulfillFriendship(_prod);
		}
		if (!this.receiveItem_Received)
		{
			this.AddProductToFulfillFriendship(_prod);
		}
		this.Check_LifeAchievement();
		this.receiveItem_Received = true;
		return !flag;
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00007180 File Offset: 0x00005380
	public void CreateReferences_FulfillFriendshipNeeds()
	{
		LocalCustomer_Data localCustomer_Data = Char_Manager.instance.GetLocalCustomer_Data(this.char_Controller.GetID());
		if (this.prodsForFriendship.Count != localCustomer_Data.prodQntForFriendship.Length)
		{
			localCustomer_Data.prodQntForFriendship = new int[this.prodsForFriendship.Count];
		}
		if (this.decorsForFriendship.Count != localCustomer_Data.decorsQntForFriendship.Length)
		{
			localCustomer_Data.decorsQntForFriendship = new int[this.decorsForFriendship.Count];
		}
		if (this.freeProdsForFriendship.Count != localCustomer_Data.freeProdQntForFriendship.Length)
		{
			localCustomer_Data.freeProdQntForFriendship = new int[this.freeProdsForFriendship.Count];
		}
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00007224 File Offset: 0x00005424
	public void AddProductToFulfillFriendship(Prod_Controller _prod)
	{
		if (this.prodsForFriendship.Count > 0)
		{
			LocalCustomer_Data localCustomer_Data = Char_Manager.instance.GetLocalCustomer_Data(this.char_Controller.GetID());
			for (int i = 0; i < this.prodsForFriendship.Count; i++)
			{
				if (this.prodsForFriendship[i] && Inv_Manager.instance.GetItemIndex(_prod.gameObject) == Inv_Manager.instance.GetItemIndex(this.prodsForFriendship[i].gameObject))
				{
					localCustomer_Data.prodQntForFriendship[i]++;
				}
			}
		}
	}

	// Token: 0x0600008D RID: 141 RVA: 0x000072BC File Offset: 0x000054BC
	public void AddDecorToFulfillFriendship(Decor_Controller _decor)
	{
		if (this.decorsForFriendship.Count > 0)
		{
			LocalCustomer_Data localCustomer_Data = Char_Manager.instance.GetLocalCustomer_Data(this.char_Controller.GetID());
			for (int i = 0; i < this.decorsForFriendship.Count; i++)
			{
				if (this.decorsForFriendship[i] && Inv_Manager.instance.GetItemIndex(_decor.gameObject) == Inv_Manager.instance.GetItemIndex(this.decorsForFriendship[i].gameObject))
				{
					localCustomer_Data.decorsQntForFriendship[i]++;
				}
			}
			this.Check_LifeAchievement();
		}
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00007360 File Offset: 0x00005560
	public void AddFreeProductToFulfillFriendship(Prod_Controller _prod)
	{
		if (this.freeProdsForFriendship.Count > 0)
		{
			LocalCustomer_Data localCustomer_Data = Char_Manager.instance.GetLocalCustomer_Data(this.char_Controller.GetID());
			for (int i = 0; i < this.freeProdsForFriendship.Count; i++)
			{
				if (this.freeProdsForFriendship[i] && Inv_Manager.instance.GetItemIndex(_prod.gameObject) == Inv_Manager.instance.GetItemIndex(this.freeProdsForFriendship[i].gameObject))
				{
					localCustomer_Data.freeProdQntForFriendship[i]++;
				}
			}
			this.Check_LifeAchievement();
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00007401 File Offset: 0x00005601
	public void AddActionsToFulfillFriendship()
	{
		if (this.actionsQntNeededForFriendship > 0)
		{
			Char_Manager.instance.GetLocalCustomer_Data(this.char_Controller.GetID()).actionsForFriendship++;
			this.Check_LifeAchievement();
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00007434 File Offset: 0x00005634
	public void Check_LifeAchievement()
	{
		if (this.prodsForFriendship.Count > 0)
		{
			bool flag = false;
			LocalCustomer_Data localCustomer_Data = Char_Manager.instance.GetLocalCustomer_Data(this.char_Controller.GetID());
			for (int i = 0; i < this.prodQntNeededForFriendship.Count; i++)
			{
				if (localCustomer_Data.prodQntForFriendship[i] >= this.prodQntNeededForFriendship[i])
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.Set_LifeAchievementBool(true);
			}
		}
		if (this.decorsForFriendship.Count > 0)
		{
			bool flag2 = false;
			LocalCustomer_Data localCustomer_Data2 = Char_Manager.instance.GetLocalCustomer_Data(this.char_Controller.GetID());
			for (int j = 0; j < this.decorsQntNeededForFriendship.Count; j++)
			{
				if (localCustomer_Data2.decorsQntForFriendship[j] >= this.decorsQntNeededForFriendship[j])
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				this.Set_LifeAchievementBool(true);
			}
		}
		if (this.freeProdsForFriendship.Count > 0)
		{
			bool flag3 = false;
			LocalCustomer_Data localCustomer_Data3 = Char_Manager.instance.GetLocalCustomer_Data(this.char_Controller.GetID());
			for (int k = 0; k < this.freeProdQntNeededForFriendship.Count; k++)
			{
				if (localCustomer_Data3.freeProdQntForFriendship[k] >= this.freeProdQntNeededForFriendship[k])
				{
					flag3 = true;
				}
			}
			if (flag3)
			{
				this.Set_LifeAchievementBool(true);
			}
		}
		if (this.actionsQntNeededForFriendship > 0)
		{
			bool flag4 = false;
			if (Char_Manager.instance.GetLocalCustomer_Data(this.char_Controller.GetID()).actionsForFriendship >= this.actionsQntNeededForFriendship)
			{
				flag4 = true;
			}
			if (flag4)
			{
				this.Set_LifeAchievementBool(true);
			}
		}
		if (this.char_Controller.GetID() == 2 && this.doneCashier && !this.fs_Found_EmptyShelf)
		{
			this.Set_LifeAchievementBool(true);
		}
		if (this.char_Controller.GetID() == 3)
		{
			bool flag5 = true;
			for (int l = 0; l < this.GetProdPredilectionIndexes().Count; l++)
			{
				if (!this.prod_BuyList.Contains(this.GetProdPredilectionIndexes()[l]))
				{
					flag5 = false;
					break;
				}
			}
			if (flag5)
			{
				this.Set_LifeAchievementBool(true);
			}
		}
		if (this.char_Controller.GetID() == 30 && this.doneCashier && !this.fs_Found_ExclusiveCategory_Other)
		{
			this.Set_LifeAchievementBool(true);
		}
		if (this.char_Controller.GetID() == 49 && this.doneCashier && !this.fs_Found_UndiscountedProd)
		{
			this.Set_LifeAchievementBool(true);
		}
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00007670 File Offset: 0x00005870
	public void Set_BadInteraction(Interaction_Controller _inter, int _emotion_value)
	{
		if (this.badInteractions_Controllers.Contains(_inter))
		{
			return;
		}
		this.SetEmotionSum(_emotion_value, 100f);
		this.badInteractions_Controllers.Add(_inter);
	}

	// Token: 0x06000092 RID: 146 RVA: 0x0000769C File Offset: 0x0000589C
	public void Set_LifeAchievementBool(bool _b)
	{
		if (Char_Manager.instance.GetCustomerLifeAchievementState(this.char_Controller.GetID()))
		{
			return;
		}
		Char_Manager.instance.SetCustomerLifeAchievement(this.char_Controller.GetID(), _b);
		this.SetEmotionSum(2, 100f);
		Menu_Manager.instance.SetFriendship(this.char_Controller);
		Debug.Log("FRIENDSHIP!!!");
	}

	// Token: 0x06000093 RID: 147 RVA: 0x000076FD File Offset: 0x000058FD
	public void MayUpdateAwait()
	{
		this.mayUpdateAwait = true;
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00007708 File Offset: 0x00005908
	private void UpdateAwaits()
	{
		if (!this.mayUpdateAwait)
		{
			return;
		}
		if (Game_Manager.instance.GetGameStage() != 999)
		{
			return;
		}
		if (this.char_Controller.GetID() == 31)
		{
			int num = UnityEngine.Random.Range(0, Player_Manager.instance.mesh_Hairs.Count);
			int num2 = UnityEngine.Random.Range(0, Player_Manager.instance.material_HairColors.Count);
			Player_Manager.instance.SetPlayerHair(num, 0);
			Player_Manager.instance.SetPlayerHairColor(num2, 0);
			LocalCustomer_Data localCustomer_Data = Char_Manager.instance.GetLocalCustomer_Data(this.char_Controller.GetID());
			localCustomer_Data.hairCutIndex = num;
			localCustomer_Data.hairColorIndex = num2;
			localCustomer_Data.hairChangeVisitIndex = localCustomer_Data.visitIndex;
		}
		this.mayUpdateAwait = false;
		Game_Manager.instance.SetCooldown(2f);
		Game_Manager.instance.SetGameStage(99);
	}

	// Token: 0x06000095 RID: 149 RVA: 0x000077D3 File Offset: 0x000059D3
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Door")
		{
			other.GetComponent<Door_Controller>().OpenDoor(base.transform.position, false);
		}
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00007804 File Offset: 0x00005A04
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Interactive")
		{
			Interaction_Controller component = other.gameObject.GetComponent<Interaction_Controller>();
			if (this.badInteractions_Controllers.Contains(component))
			{
				return;
			}
			if (component.isBox)
			{
				if (!component.GetComponent<Box_Controller>().isHeld)
				{
					this.Set_BadInteraction(component, -1);
					return;
				}
			}
			else
			{
				if (component.isDirt)
				{
					this.Set_BadInteraction(component, -1);
					return;
				}
				if (component.isUtil)
				{
					this.TryUtilInterest(component.gameObject.GetComponent<Util_Controller>());
					return;
				}
				if (component.isDecor)
				{
					this.AddDecorToFulfillFriendship(component.decor_Controller);
					if (component.isDecorPlant && this.char_Controller.GetID() == 11 && Inv_Manager.instance.GetAllDecorControllersDisplayed_Plants().Count >= 5)
					{
						this.Set_LifeAchievementBool(true);
						return;
					}
				}
			}
		}
		else if (other.gameObject.tag == "Player")
		{
			Player_Controller player_Controller = null;
			other.gameObject.TryGetComponent<Player_Controller>(out player_Controller);
			if (player_Controller == null)
			{
				return;
			}
			if (this.char_Controller.GetID() == 10 && player_Controller.GetIsRunning() && !this.actionsForFriendship_HappenedAlready)
			{
				this.actionsForFriendship_HappenedAlready = true;
				this.AddActionsToFulfillFriendship();
			}
			if (this.char_Controller.GetID() == 31)
			{
				if (player_Controller.skin_.hairMesh_Index == Char_Manager.instance.localCustomer_Datas[this.char_Controller.GetID()].hairCutIndex && player_Controller.skin_.hairMat_Index == Char_Manager.instance.localCustomer_Datas[this.char_Controller.GetID()].hairColorIndex && Char_Manager.instance.GetLocalCustomer_Data(this.char_Controller.GetID()).visitIndex > Char_Manager.instance.GetLocalCustomer_Data(this.char_Controller.GetID()).hairChangeVisitIndex)
				{
					this.Set_LifeAchievementBool(true);
					return;
				}
			}
			else if (this.char_Controller.GetID() == 55 && player_Controller.skin_.clothesMat_Index == 6)
			{
				this.Set_LifeAchievementBool(true);
			}
		}
	}

	// Token: 0x0400008E RID: 142
	[SerializeField]
	private Animator basket_Anim;

	// Token: 0x0400008F RID: 143
	[SerializeField]
	private Animator paperBag_Anim;

	// Token: 0x04000090 RID: 144
	public Char_Controller char_Controller;

	// Token: 0x04000091 RID: 145
	private Talk_Controller talk_Controller;

	// Token: 0x04000092 RID: 146
	[Header("AI")]
	[SerializeField]
	public Util_Controller util_Interest;

	// Token: 0x04000093 RID: 147
	[SerializeField]
	public List<int> prod_BuyList = new List<int>();

	// Token: 0x04000094 RID: 148
	[SerializeField]
	public Prod_Controller prod_Interest;

	// Token: 0x04000095 RID: 149
	[SerializeField]
	private List<GameObject> navPath = new List<GameObject>();

	// Token: 0x04000096 RID: 150
	[SerializeField]
	private int navPathIndex;

	// Token: 0x04000097 RID: 151
	[SerializeField]
	private int clientPlaceIndex = -1;

	// Token: 0x04000098 RID: 152
	public bool doneShopping;

	// Token: 0x04000099 RID: 153
	private float navPathDistance = 0.8f;

	// Token: 0x0400009A RID: 154
	public bool doneCashier;

	// Token: 0x0400009B RID: 155
	public float cashierTime_Timer;

	// Token: 0x0400009C RID: 156
	private float cashierTime_Bonus = 3f;

	// Token: 0x0400009D RID: 157
	private float cashierTime_Bonus_Odd = 50f;

	// Token: 0x0400009E RID: 158
	private Cashier_Controller cashier_Controller;

	// Token: 0x0400009F RID: 159
	private float cooldown = 1.5f;

	// Token: 0x040000A0 RID: 160
	private float cooldown_Timer;

	// Token: 0x040000A1 RID: 161
	[Header("Ai Prod Decision")]
	[SerializeField]
	private float prodThinking_TimeMin = 2f;

	// Token: 0x040000A2 RID: 162
	[SerializeField]
	private float prodThinking_TimeMax = 10f;

	// Token: 0x040000A3 RID: 163
	private float prodThinking_Timer;

	// Token: 0x040000A4 RID: 164
	[Header("Prod Shopping")]
	[SerializeField]
	private int prodShopping_Min = 3;

	// Token: 0x040000A5 RID: 165
	[SerializeField]
	private int prodShopping_Max = 8;

	// Token: 0x040000A6 RID: 166
	private int prodShopping_Qnt = 1;

	// Token: 0x040000A7 RID: 167
	[Header("Emotions")]
	[SerializeField]
	public Emoji_Controller emotion_ctrl;

	// Token: 0x040000A8 RID: 168
	[SerializeField]
	private float emotion_Angry_Timer;

	// Token: 0x040000A9 RID: 169
	private float emotion_Andry_TIMER = 20f;

	// Token: 0x040000AA RID: 170
	public int emotionIndex = 2;

	// Token: 0x040000AB RID: 171
	public Skin_Controller skin_Controller;

	// Token: 0x040000AC RID: 172
	public Face_Controller face_Controller;

	// Token: 0x040000AD RID: 173
	private bool gaveTaigaRate;

	// Token: 0x040000AE RID: 174
	[SerializeField]
	private List<Prod_Controller> prodPredilection_Prefabs = new List<Prod_Controller>();

	// Token: 0x040000AF RID: 175
	private int prodPredilection_NeedMultiplier = 10;

	// Token: 0x040000B0 RID: 176
	[SerializeField]
	public Emoji_Controller prodWanted_ctrl;

	// Token: 0x040000B1 RID: 177
	public int prodWantedNow_Index = -1;

	// Token: 0x040000B2 RID: 178
	private List<int> prodsConsumedForFreeIndexes = new List<int>();

	// Token: 0x040000B3 RID: 179
	private List<Util_Controller> utilsChecked = new List<Util_Controller>();

	// Token: 0x040000B4 RID: 180
	[SerializeField]
	public bool receiveItem_Received;

	// Token: 0x040000B5 RID: 181
	[SerializeField]
	private List<Prod_Controller> prodsForFriendship = new List<Prod_Controller>();

	// Token: 0x040000B6 RID: 182
	[SerializeField]
	private List<int> prodQntNeededForFriendship = new List<int>();

	// Token: 0x040000B7 RID: 183
	[SerializeField]
	private List<Decor_Controller> decorsForFriendship = new List<Decor_Controller>();

	// Token: 0x040000B8 RID: 184
	[SerializeField]
	private List<int> decorsQntNeededForFriendship = new List<int>();

	// Token: 0x040000B9 RID: 185
	[SerializeField]
	private List<Prod_Controller> freeProdsForFriendship = new List<Prod_Controller>();

	// Token: 0x040000BA RID: 186
	[SerializeField]
	private int actionsQntNeededForFriendship;

	// Token: 0x040000BB RID: 187
	[SerializeField]
	private bool actionsForFriendship_HappenedAlready;

	// Token: 0x040000BC RID: 188
	[SerializeField]
	private List<int> freeProdQntNeededForFriendship = new List<int>();

	// Token: 0x040000BD RID: 189
	[SerializeField]
	private Prod_Controller prodForTrade;

	// Token: 0x040000BE RID: 190
	[SerializeField]
	private Prod_Controller prodToGive;

	// Token: 0x040000BF RID: 191
	[SerializeField]
	private Decor_Controller decorToGive;

	// Token: 0x040000C0 RID: 192
	public bool fs_Found_EmptyShelf;

	// Token: 0x040000C1 RID: 193
	public bool fs_Found_UndiscountedProd;

	// Token: 0x040000C2 RID: 194
	public bool fs_Found_ExclusiveCategory_Other;

	// Token: 0x040000C3 RID: 195
	private bool mayUpdateAwait;

	// Token: 0x040000C4 RID: 196
	private List<Interaction_Controller> badInteractions_Controllers = new List<Interaction_Controller>();
}
