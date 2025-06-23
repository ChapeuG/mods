using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000042 RID: 66
public class Inv_Manager : MonoBehaviour
{
	// Token: 0x060002E7 RID: 743 RVA: 0x0001B238 File Offset: 0x00019438
	public int Get_ProdDemandIndex(int _prod_index)
	{
		int result = 2;
		for (int i = 0; i < this.prodThinking_BuyOdd_Values.Length; i++)
		{
			if ((float)this.prod_Prefabs[_prod_index].prodPrice <= this.prodThinking_BuyOdd_Values[i])
			{
				result = i;
				break;
			}
		}
		return result;
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x0001B27C File Offset: 0x0001947C
	public float Get_ProdThinking_BuyOdd(int _prod_index)
	{
		int prodDiscountLevel = this.GetProdDiscountLevel(_prod_index);
		int num = this.Get_ProdDemandIndex(_prod_index);
		if (num == 0)
		{
			return this.prodThinking_BuyOdd_High[prodDiscountLevel + 1];
		}
		if (num == 1)
		{
			return this.prodThinking_BuyOdd_Normal[prodDiscountLevel + 1];
		}
		return this.prodThinking_BuyOdd_Low[prodDiscountLevel + 1];
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x0001B2C0 File Offset: 0x000194C0
	public int Get_ProdMaxBuyQnt(int _prod_index)
	{
		return this.prodBuyMaxQnt_ByRawDemand[this.Get_ProdDemandIndex(_prod_index)];
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x060002EA RID: 746 RVA: 0x0001B2D0 File Offset: 0x000194D0
	// (set) Token: 0x060002EB RID: 747 RVA: 0x0001B2D8 File Offset: 0x000194D8
	public string[] prodTypes_Names { get; private set; }

	// Token: 0x060002EC RID: 748 RVA: 0x0001B2E4 File Offset: 0x000194E4
	private void Awake()
	{
		if (!Inv_Manager.instance)
		{
			Inv_Manager.instance = this;
		}
		Prod_Controller[] array = Resources.LoadAll<Prod_Controller>("Sellables/Prods/Prefabs");
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < array.Length; j++)
			{
				if (int.Parse(array[j].name.Split(new char[]
				{
					char.Parse("_")
				})[0]) == i)
				{
					this.prod_Prefabs.Add(array[j]);
					this.prod_DiscountLevel.Add(-1);
					break;
				}
			}
		}
		this.prod_Sprites = Resources.LoadAll<Sprite>("Sellables/Prods/");
		this.SetSellPrices();
	}

	// Token: 0x060002ED RID: 749 RVA: 0x0001B388 File Offset: 0x00019588
	private void Update()
	{
		if (Cheat_Manager.instance.GetDropBoxes() == 1)
		{
			this.dropBoxes_Timer -= Time.deltaTime;
			if (this.dropBoxes_Timer <= 0f && Cheat_Manager.instance.dropedBoxesQnt < 100)
			{
				this.dropBoxes_Timer = 0.05f;
				Box_Controller box_Controller = this.CreateBox_Prod(UnityEngine.Random.Range(0, 20), 8, this.lifeSpan_StartValue, false);
				GameObject gameObject = this.deliveryPlaces[UnityEngine.Random.Range(0, this.deliveryPlaces.Count)];
				box_Controller.transform.position = gameObject.transform.position;
				box_Controller.transform.rotation = gameObject.transform.rotation;
				Cheat_Manager.instance.dropedBoxesQnt++;
				return;
			}
			if (Cheat_Manager.instance.dropedBoxesQnt >= 100)
			{
				Cheat_Manager.instance.SetDropBoxes(0);
			}
		}
	}

	// Token: 0x060002EE RID: 750 RVA: 0x0001B46A File Offset: 0x0001966A
	public void RefreshReferences()
	{
		this.shop_Deliv_Qnt = 8;
	}

	// Token: 0x060002EF RID: 751 RVA: 0x0001B474 File Offset: 0x00019674
	public void GetDeliveryPlaces()
	{
		this.deliveryPlaces.Clear();
		GameObject[] array = GameObject.FindGameObjectsWithTag("DeliveryPlace");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i])
			{
				this.deliveryPlaces.Add(array[i]);
			}
		}
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x0001B4C0 File Offset: 0x000196C0
	public void SetSellPrices()
	{
		this.prod_SellPrices = new int[this.prod_Prefabs.Count];
		for (int i = 0; i < this.prod_SellPrices.Length; i++)
		{
			if (i < this.prod_SellPrices.Length && this.prod_Prefabs[i])
			{
				this.prod_SellPrices[i] = Mathf.CeilToInt(this.GetBoxPrice(i) / (float)this.boxSize * this.prodSellPriceMultiplierTemp);
			}
		}
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x0001B538 File Offset: 0x00019738
	public Sprite GetProdSprite(int _index)
	{
		Sprite result = null;
		if (_index >= this.prod_Sprites.Length)
		{
			return result;
		}
		return this.prod_Sprites[_index];
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x0001B55E File Offset: 0x0001975E
	public Prod_Controller GetProdPrefab(int _index)
	{
		return this.prod_Prefabs[_index];
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x0001B56C File Offset: 0x0001976C
	public List<int> GetAllBuyableProdIndexes()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.prod_Prefabs.Count; i++)
		{
			if (this.prod_Prefabs[i].buyable)
			{
				list.Add(this.GetItemIndex(this.prod_Prefabs[i].gameObject));
			}
		}
		return list;
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x0001B5C6 File Offset: 0x000197C6
	public Shelf_Controller GetItemShelf(int _index)
	{
		return this.shelfProd_Prefabs[_index];
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x0001B5D4 File Offset: 0x000197D4
	public Decor_Controller GetItemDecor(int _index)
	{
		return this.decor_Prefabs[_index];
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x0001B5E2 File Offset: 0x000197E2
	public WallPaint_Controller GetItemWallPaint(int _index)
	{
		return this.wallPaint_Prefabs[_index];
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x0001B5F0 File Offset: 0x000197F0
	public Floor_Controller GetItemFloor(int _index)
	{
		return this.floor_Prefabs[_index];
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x0001B5FE File Offset: 0x000197FE
	public Util_Controller GetItemUtil(int _index)
	{
		return this.util_Prefabs[_index];
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x0001B60C File Offset: 0x0001980C
	public int GetItemIndex(GameObject _item)
	{
		return int.Parse(_item.gameObject.name.Split(new char[]
		{
			"_"[0]
		})[0]);
	}

	// Token: 0x060002FA RID: 762 RVA: 0x0001B63C File Offset: 0x0001983C
	public List<int> GetItemIndex_List(List<GameObject> _item)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < _item.Count; i++)
		{
			string[] array = _item[i].gameObject.name.Split(new char[]
			{
				"_"[0]
			});
			list.Add(int.Parse(array[0]));
		}
		return list;
	}

	// Token: 0x060002FB RID: 763 RVA: 0x0001B69C File Offset: 0x0001989C
	public List<int> GetItemIndex_List(List<Prod_Controller> _item)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < _item.Count; i++)
		{
			list.Add(_item[i].gameObject);
		}
		return this.GetItemIndex_List(list);
	}

	// Token: 0x060002FC RID: 764 RVA: 0x0001B6DC File Offset: 0x000198DC
	public string GetProdName(int _index, bool _translate)
	{
		string[] array = this.prod_Prefabs[_index].gameObject.name.Split(new char[]
		{
			"_"[0]
		});
		string text = array[array.Length - 1];
		if (_translate)
		{
			text = Language_Manager.instance.GetText(text);
		}
		return text;
	}

	// Token: 0x060002FD RID: 765 RVA: 0x0001B738 File Offset: 0x00019938
	public string GetProdName(Prod_Controller _prod, bool _translate)
	{
		string[] array = _prod.gameObject.name.Split(new char[]
		{
			"_"[0]
		});
		string text = array[array.Length - 1];
		if (_translate)
		{
			text = Language_Manager.instance.GetText(text);
		}
		return text;
	}

	// Token: 0x060002FE RID: 766 RVA: 0x0001B788 File Offset: 0x00019988
	public string GetItemName(GameObject _item, bool _translate)
	{
		string[] array = _item.gameObject.name.Split(new char[]
		{
			"_"[0]
		});
		string text = array[array.Length - 1];
		if (_translate)
		{
			text = Language_Manager.instance.GetText(text);
		}
		return text;
	}

	// Token: 0x060002FF RID: 767 RVA: 0x0001B7D6 File Offset: 0x000199D6
	public float GetBoxPrice(int _index)
	{
		return (float)this.prod_Prefabs[_index].prodPrice;
	}

	// Token: 0x06000300 RID: 768 RVA: 0x0001B7EA File Offset: 0x000199EA
	public float GetShelfBoxPrice(int _index)
	{
		return (float)this.shelfProd_Prefabs[_index].itemPrice;
	}

	// Token: 0x06000301 RID: 769 RVA: 0x0001B7FE File Offset: 0x000199FE
	public float GetDecorBoxPrice(int _index)
	{
		return (float)this.decor_Prefabs[_index].itemPrice;
	}

	// Token: 0x06000302 RID: 770 RVA: 0x0001B812 File Offset: 0x00019A12
	public float GetWallPaintBoxPrice(int _index)
	{
		return (float)this.wallPaint_Prefabs[_index].itemPrice;
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0001B826 File Offset: 0x00019A26
	public float GetFloorBoxPrice(int _index)
	{
		return (float)this.floor_Prefabs[_index].itemPrice;
	}

	// Token: 0x06000304 RID: 772 RVA: 0x0001B83A File Offset: 0x00019A3A
	public float GetUtilBoxPrice(int _index)
	{
		return (float)this.util_Prefabs[_index].itemPrice;
	}

	// Token: 0x06000305 RID: 773 RVA: 0x0001B84E File Offset: 0x00019A4E
	public int GetProdSellPrice(int _index)
	{
		return this.prod_SellPrices[_index] * 100 / 100;
	}

	// Token: 0x06000306 RID: 774 RVA: 0x0001B860 File Offset: 0x00019A60
	public int GetProdSellPriceDiscounted(int _index)
	{
		int prodDiscountLevel = this.GetProdDiscountLevel(_index);
		if (prodDiscountLevel == -1)
		{
			return this.GetProdSellPrice(_index);
		}
		return (int)((float)this.GetProdSellPrice(_index) * ((100f - this.prod_DiscountValue[prodDiscountLevel]) / 100f)) * 100 / 100;
	}

	// Token: 0x06000307 RID: 775 RVA: 0x0001B8A5 File Offset: 0x00019AA5
	public void IncreaseProdSellPrice(int _index, int _priceIncrease)
	{
		this.prod_SellPrices[_index] += _priceIncrease;
	}

	// Token: 0x06000308 RID: 776 RVA: 0x0001B8B8 File Offset: 0x00019AB8
	public bool AddDelivery(int _index, int _category, int _qnt, bool indefinetely, int _supplier)
	{
		bool result = false;
		if (this.prod_deliveryIndexes.Count < this.shop_Deliv_Qnt || indefinetely)
		{
			this.prod_deliveryIndexes.Add(_index);
			this.prod_deliveryCategories.Add(_category);
			this.prod_deliveryQnt.Add(_qnt);
			this.prod_deliverySupplierIndexes.Add(_supplier);
			this.prod_deliveryDaysIndexes.Add(this.prod_deliveryDays_Available[_supplier]);
			this.prod_deliveryLifeSpanIndexes.Add(this.prod_deliveryLifeSpan_Available[_supplier]);
			result = true;
		}
		return result;
	}

	// Token: 0x06000309 RID: 777 RVA: 0x0001B93C File Offset: 0x00019B3C
	public void DeleteLastDeliveryProd()
	{
		if (this.prod_deliveryIndexes.Count > 0)
		{
			int num = 0;
			int num2 = this.prod_deliveryCategories[this.prod_deliveryCategories.Count - 1];
			if (num2 == 0)
			{
				num = this.prod_Prefabs[this.prod_deliveryIndexes[this.prod_deliveryIndexes.Count - 1]].prodPrice;
			}
			else if (num2 == 1)
			{
				num = this.shelfProd_Prefabs[this.prod_deliveryIndexes[this.prod_deliveryIndexes.Count - 1]].itemPrice;
			}
			else if (num2 == 2)
			{
				num = this.decor_Prefabs[this.prod_deliveryIndexes[this.prod_deliveryIndexes.Count - 1]].itemPrice;
			}
			else if (num2 == 3)
			{
				num = this.wallPaint_Prefabs[this.prod_deliveryIndexes[this.prod_deliveryIndexes.Count - 1]].itemPrice;
			}
			else if (num2 == 4)
			{
				num = this.floor_Prefabs[this.prod_deliveryIndexes[this.prod_deliveryIndexes.Count - 1]].itemPrice;
			}
			else if (num2 == 5)
			{
				num = this.util_Prefabs[this.prod_deliveryIndexes[this.prod_deliveryIndexes.Count - 1]].itemPrice;
			}
			num = Mathf.FloorToInt((float)num * this.prod_deliveryPrice_Available[this.prod_deliverySupplierIndexes[this.prod_deliveryIndexes.Count - 1]]);
			Finances_Manager.instance.AddMoney((float)num);
			if (num2 == 0)
			{
				Finances_Manager.instance.AddTo_OutProds((float)(-(float)num));
			}
			else
			{
				Finances_Manager.instance.AddTo_OutFurniture((float)(-(float)num));
			}
			this.prod_deliveryIndexes.RemoveAt(this.prod_deliveryIndexes.Count - 1);
			this.prod_deliveryCategories.RemoveAt(this.prod_deliveryCategories.Count - 1);
			this.prod_deliveryQnt.RemoveAt(this.prod_deliveryQnt.Count - 1);
			this.prod_deliverySupplierIndexes.RemoveAt(this.prod_deliverySupplierIndexes.Count - 1);
			this.prod_deliveryDaysIndexes.RemoveAt(this.prod_deliveryDaysIndexes.Count - 1);
			this.prod_deliveryLifeSpanIndexes.RemoveAt(this.prod_deliveryLifeSpanIndexes.Count - 1);
		}
		PC_Manager.instance.RefreshTomorrowDelivery();
	}

	// Token: 0x0600030A RID: 778 RVA: 0x0001BB84 File Offset: 0x00019D84
	public void DeliverProds()
	{
		if (this.prod_deliveryIndexes.Count <= 0)
		{
			return;
		}
		for (int i = this.prod_deliveryIndexes.Count - 1; i >= 0; i--)
		{
			if (this.prod_deliveryDaysIndexes[i] <= 1)
			{
				Box_Controller box_Controller = null;
				if (this.prod_deliveryCategories[i] == 0)
				{
					box_Controller = this.CreateBox_Prod(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i], this.prod_deliveryLifeSpanIndexes[i], false);
				}
				else if (this.prod_deliveryCategories[i] == 1)
				{
					box_Controller = this.CreateBox_Shelf(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i]);
				}
				else if (this.prod_deliveryCategories[i] == 2)
				{
					box_Controller = this.CreateBox_Decor(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i], this.plantHealth_StartValue);
				}
				else if (this.prod_deliveryCategories[i] == 3)
				{
					box_Controller = this.CreateBox_Wall(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i]);
				}
				else if (this.prod_deliveryCategories[i] == 4)
				{
					box_Controller = this.CreateBox_Floor(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i]);
				}
				else if (this.prod_deliveryCategories[i] == 5)
				{
					box_Controller = this.CreateBox_Util(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i]);
				}
				box_Controller.transform.position = this.deliveryPlaces[UnityEngine.Random.Range(0, this.deliveryPlaces.Count)].transform.position + new Vector3(0f, (float)(i + 1), 0f);
				box_Controller.transform.rotation = this.deliveryPlaces[UnityEngine.Random.Range(0, this.deliveryPlaces.Count)].transform.rotation;
				box_Controller.Set_EE_BoxHight_State(-1);
				this.prod_deliveryIndexes.RemoveAt(i);
				this.prod_deliveryCategories.RemoveAt(i);
				this.prod_deliveryQnt.RemoveAt(i);
				this.prod_deliverySupplierIndexes.RemoveAt(i);
				this.prod_deliveryDaysIndexes.RemoveAt(i);
				this.prod_deliveryLifeSpanIndexes.RemoveAt(i);
			}
			else
			{
				List<int> list = this.prod_deliveryDaysIndexes;
				int index = i;
				int num = list[index];
				list[index] = num - 1;
			}
		}
		this.prod_deliveryIndexes.TrimExcess();
		this.prod_deliveryCategories.TrimExcess();
		this.prod_deliveryQnt.TrimExcess();
		this.prod_deliverySupplierIndexes.TrimExcess();
		this.prod_deliveryDaysIndexes.TrimExcess();
		this.prod_deliveryLifeSpanIndexes.TrimExcess();
	}

	// Token: 0x0600030B RID: 779 RVA: 0x0001BE34 File Offset: 0x0001A034
	public void DeliverProds_Today()
	{
		if (this.prod_deliveryIndexes.Count <= 0)
		{
			return;
		}
		for (int i = this.prod_deliveryIndexes.Count - 1; i >= 0; i--)
		{
			if (this.prod_deliveryDaysIndexes[i] <= 0)
			{
				Box_Controller box_Controller = null;
				if (this.prod_deliveryCategories[i] == 0)
				{
					box_Controller = this.CreateBox_Prod(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i], this.prod_deliveryLifeSpanIndexes[i], false);
				}
				else if (this.prod_deliveryCategories[i] == 1)
				{
					box_Controller = this.CreateBox_Shelf(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i]);
				}
				else if (this.prod_deliveryCategories[i] == 2)
				{
					box_Controller = this.CreateBox_Decor(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i], this.plantHealth_StartValue);
				}
				else if (this.prod_deliveryCategories[i] == 3)
				{
					box_Controller = this.CreateBox_Wall(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i]);
				}
				else if (this.prod_deliveryCategories[i] == 4)
				{
					box_Controller = this.CreateBox_Floor(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i]);
				}
				else if (this.prod_deliveryCategories[i] == 5)
				{
					box_Controller = this.CreateBox_Util(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i]);
				}
				box_Controller.transform.position = this.deliveryPlaces[UnityEngine.Random.Range(0, this.deliveryPlaces.Count)].transform.position + new Vector3(0f, (float)(i + 1), 0f);
				box_Controller.transform.rotation = this.deliveryPlaces[UnityEngine.Random.Range(0, this.deliveryPlaces.Count)].transform.rotation;
				box_Controller.Set_EE_BoxHight_State(-1);
				this.prod_deliveryIndexes.RemoveAt(i);
				this.prod_deliveryCategories.RemoveAt(i);
				this.prod_deliveryQnt.RemoveAt(i);
				this.prod_deliverySupplierIndexes.RemoveAt(i);
				this.prod_deliveryDaysIndexes.RemoveAt(i);
				this.prod_deliveryLifeSpanIndexes.RemoveAt(i);
			}
		}
		this.prod_deliveryIndexes.TrimExcess();
		this.prod_deliveryCategories.TrimExcess();
		this.prod_deliveryQnt.TrimExcess();
		this.prod_deliverySupplierIndexes.TrimExcess();
		this.prod_deliveryDaysIndexes.TrimExcess();
		this.prod_deliveryLifeSpanIndexes.TrimExcess();
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0001C0C8 File Offset: 0x0001A2C8
	public void DeliverProds_Now()
	{
		if (this.prod_deliveryIndexes.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < this.prod_deliveryIndexes.Count; i++)
		{
			Box_Controller box_Controller = null;
			if (this.prod_deliveryCategories[i] == 0)
			{
				box_Controller = this.CreateBox_Prod(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i], this.lifeSpan_StartValue, false);
			}
			else if (this.prod_deliveryCategories[i] == 1)
			{
				box_Controller = this.CreateBox_Shelf(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i]);
			}
			else if (this.prod_deliveryCategories[i] == 2)
			{
				box_Controller = this.CreateBox_Decor(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i], this.plantHealth_StartValue);
			}
			else if (this.prod_deliveryCategories[i] == 3)
			{
				box_Controller = this.CreateBox_Wall(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i]);
			}
			else if (this.prod_deliveryCategories[i] == 4)
			{
				box_Controller = this.CreateBox_Floor(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i]);
			}
			else if (this.prod_deliveryCategories[i] == 5)
			{
				box_Controller = this.CreateBox_Util(this.prod_deliveryIndexes[i], this.prod_deliveryQnt[i]);
			}
			box_Controller.transform.position = this.deliveryPlaces[UnityEngine.Random.Range(0, this.deliveryPlaces.Count)].transform.position + new Vector3(0f, (float)(i + 1), 0f);
			box_Controller.transform.rotation = this.deliveryPlaces[UnityEngine.Random.Range(0, this.deliveryPlaces.Count)].transform.rotation;
			box_Controller.Set_EE_BoxHight_State(-1);
		}
		this.prod_deliveryIndexes.Clear();
		this.prod_deliveryCategories.Clear();
		this.prod_deliveryQnt.Clear();
		this.prod_deliverySupplierIndexes.Clear();
		this.prod_deliveryDaysIndexes.Clear();
		this.prod_deliveryLifeSpanIndexes.Clear();
	}

	// Token: 0x0600030D RID: 781 RVA: 0x0001C2F8 File Offset: 0x0001A4F8
	public void Load_DeliveryProds(SaveData _data)
	{
		if (_data.deliveryIndexes_SD == null)
		{
			return;
		}
		this.prod_deliveryIndexes = new List<int>(_data.deliveryIndexes_SD);
		this.prod_deliveryCategories = new List<int>(_data.deliveryCategories_SD);
		this.prod_deliveryQnt = new List<int>(_data.deliveryQnt_SD);
		this.prod_deliverySupplierIndexes = new List<int>(_data.deliverySupplierIndexes_SD);
		this.prod_deliveryDaysIndexes = new List<int>(_data.deliveryDaysIndexes_SD);
		this.prod_deliveryLifeSpanIndexes = new List<int>(_data.deliveryLifeSpanIndexes_SD);
	}

	// Token: 0x0600030E RID: 782 RVA: 0x0001C374 File Offset: 0x0001A574
	public void RefreshIce(bool _only_freeze = false)
	{
		foreach (Box_Controller box_Controller in this.box_Controllers)
		{
			box_Controller.Check_Freezer(_only_freeze);
		}
	}

	// Token: 0x0600030F RID: 783 RVA: 0x0001C3C8 File Offset: 0x0001A5C8
	public Box_Controller CreateBox_Prod(int _prodIndex, int _prodQnt, int _lifeSpan, bool _frozen)
	{
		Box_Controller box_Controller;
		if (this.GetProdPrefab(_prodIndex).needsRefrigerator)
		{
			box_Controller = UnityEngine.Object.Instantiate<Box_Controller>(this.box_Prefab_ProdRefrigerated);
		}
		else
		{
			box_Controller = UnityEngine.Object.Instantiate<Box_Controller>(this.box_Prefab_ProdNormal);
		}
		box_Controller.CreateBox(_prodIndex, _prodQnt, _lifeSpan, _frozen);
		if (!this.box_Controllers.Contains(box_Controller))
		{
			this.box_Controllers.Add(box_Controller);
		}
		return box_Controller;
	}

	// Token: 0x06000310 RID: 784 RVA: 0x0001C428 File Offset: 0x0001A628
	public Box_Controller CreateBox_Shelf(int _shelfIndex, int _prodQnt)
	{
		Box_Controller box_Controller = UnityEngine.Object.Instantiate<Box_Controller>(this.box_Prefab_Shelf);
		box_Controller.CreateBox(_shelfIndex, _prodQnt, 0, false);
		if (!this.box_Controllers.Contains(box_Controller))
		{
			this.box_Controllers.Add(box_Controller);
		}
		return box_Controller;
	}

	// Token: 0x06000311 RID: 785 RVA: 0x0001C468 File Offset: 0x0001A668
	public Box_Controller CreateBox_Decor(int _decorIndex, int _prodQnt, int _lifeSpanIndex)
	{
		Box_Controller box_Controller = UnityEngine.Object.Instantiate<Box_Controller>(this.box_Prefab_Decoration);
		box_Controller.CreateBox(_decorIndex, _prodQnt, _lifeSpanIndex, false);
		if (!this.box_Controllers.Contains(box_Controller))
		{
			this.box_Controllers.Add(box_Controller);
		}
		return box_Controller;
	}

	// Token: 0x06000312 RID: 786 RVA: 0x0001C4A8 File Offset: 0x0001A6A8
	public Box_Controller CreateBox_Wall(int _itemIndex, int _prodQnt)
	{
		Box_Controller box_Controller = UnityEngine.Object.Instantiate<Box_Controller>(this.box_Prefab_Wall);
		box_Controller.CreateBox(_itemIndex, _prodQnt, 0, false);
		if (!this.box_Controllers.Contains(box_Controller))
		{
			this.box_Controllers.Add(box_Controller);
		}
		return box_Controller;
	}

	// Token: 0x06000313 RID: 787 RVA: 0x0001C4E8 File Offset: 0x0001A6E8
	public Box_Controller CreateBox_Floor(int _itemIndex, int _prodQnt)
	{
		Box_Controller box_Controller = UnityEngine.Object.Instantiate<Box_Controller>(this.box_Prefab_Floor);
		box_Controller.CreateBox(_itemIndex, _prodQnt, 0, false);
		if (!this.box_Controllers.Contains(box_Controller))
		{
			this.box_Controllers.Add(box_Controller);
		}
		return box_Controller;
	}

	// Token: 0x06000314 RID: 788 RVA: 0x0001C528 File Offset: 0x0001A728
	public Box_Controller CreateBox_Util(int _itemIndex, int _prodQnt)
	{
		Box_Controller box_Controller = UnityEngine.Object.Instantiate<Box_Controller>(this.box_Prefab_Util);
		box_Controller.CreateBox(_itemIndex, _prodQnt, 0, false);
		if (!this.box_Controllers.Contains(box_Controller))
		{
			this.box_Controllers.Add(box_Controller);
		}
		return box_Controller;
	}

	// Token: 0x06000315 RID: 789 RVA: 0x0001C568 File Offset: 0x0001A768
	public void DeleteBox(Box_Controller _boxController, int _player_index)
	{
		if (this.box_Controllers.Contains(_boxController))
		{
			if (Player_Manager.instance.GetPlayerController(_player_index).boxController == _boxController)
			{
				Player_Manager.instance.GetPlayerController(_player_index).RemoveBox();
			}
			this.box_Controllers.Remove(_boxController);
			UnityEngine.Object.Destroy(_boxController.gameObject);
		}
	}

	// Token: 0x06000316 RID: 790 RVA: 0x0001C5C4 File Offset: 0x0001A7C4
	public Prod_Controller[] CreateProd(int _prodIndex, int _prodQnt, bool _buyable, int _lifeSpanIndex)
	{
		Prod_Controller[] array = new Prod_Controller[_prodQnt];
		for (int i = 0; i < _prodQnt; i++)
		{
			array[i] = UnityEngine.Object.Instantiate<Prod_Controller>(this.prod_Prefabs[_prodIndex]);
			array[i].SetIndex(_prodIndex);
			array[i].buyableByCustomers = _buyable;
			array[i].lifeSpanIndex = _lifeSpanIndex;
			if (!this.prod_Controllers.Contains(array[i]))
			{
				this.prod_Controllers.Add(array[i]);
			}
		}
		return array;
	}

	// Token: 0x06000317 RID: 791 RVA: 0x0001C634 File Offset: 0x0001A834
	public Prod_Controller CreateProd(int _prodIndex, bool _buyable, int _lifeSpanIndex)
	{
		Prod_Controller prod_Controller = UnityEngine.Object.Instantiate<Prod_Controller>(this.prod_Prefabs[_prodIndex]);
		prod_Controller.SetIndex(_prodIndex);
		prod_Controller.buyableByCustomers = _buyable;
		prod_Controller.lifeSpanIndex = _lifeSpanIndex;
		if (!this.prod_Controllers.Contains(prod_Controller))
		{
			this.prod_Controllers.Add(prod_Controller);
		}
		return prod_Controller;
	}

	// Token: 0x06000318 RID: 792 RVA: 0x0001C683 File Offset: 0x0001A883
	public void DeleteProd(Prod_Controller _prod)
	{
		if (this.prod_Controllers.Contains(_prod))
		{
			this.prod_Controllers.Remove(_prod);
			UnityEngine.Object.Destroy(_prod.gameObject);
		}
	}

	// Token: 0x06000319 RID: 793 RVA: 0x0001C6AC File Offset: 0x0001A8AC
	public Shelf_Controller PlaceShelf(int _index, GameObject _place, int _player_index)
	{
		Vector3 position = Nav_Manager.instance.FindNearSphere(Player_Manager.instance.GetPlayerController(_player_index).gameObject).transform.position;
		Vector3 position2 = _place.transform.position;
		Quaternion quaternion = Quaternion.LookRotation(position - position2);
		if (Vector3.Distance(position, position2) > 2.1f)
		{
			quaternion.eulerAngles = Vector3.zero;
		}
		quaternion.x = 0f;
		quaternion.z = 0f;
		return this.PlaceShelf(_index, _place.transform.position, quaternion.eulerAngles);
	}

	// Token: 0x0600031A RID: 794 RVA: 0x0001C740 File Offset: 0x0001A940
	public Shelf_Controller PlaceShelf(int _index, Vector3 _pos, Vector3 _rot)
	{
		Shelf_Controller shelf_Controller = UnityEngine.Object.Instantiate<Shelf_Controller>(this.shelfProd_Prefabs[_index]);
		shelf_Controller.transform.position = _pos;
		shelf_Controller.transform.localScale = Vector3.one;
		shelf_Controller.transform.rotation = Quaternion.Euler(_rot);
		this.AddShelfProdControllers(shelf_Controller);
		return shelf_Controller;
	}

	// Token: 0x0600031B RID: 795 RVA: 0x0001C794 File Offset: 0x0001A994
	public Decor_Controller PlaceDecor(int _index, GameObject _place, int _lifeSpanIndex, int _player_index)
	{
		Quaternion quaternion = Quaternion.LookRotation(Nav_Manager.instance.FindNearSphere(Player_Manager.instance.GetPlayerController(_player_index).gameObject).transform.position - _place.transform.position);
		quaternion.x = 0f;
		quaternion.z = 0f;
		return this.PlaceDecor(_index, _place.transform.position, quaternion.eulerAngles, _lifeSpanIndex);
	}

	// Token: 0x0600031C RID: 796 RVA: 0x0001C810 File Offset: 0x0001AA10
	public Decor_Controller PlaceDecor(int _index, Vector3 _pos, Vector3 _rot, int _lifeSpanIndex)
	{
		Decor_Controller decor_Controller = UnityEngine.Object.Instantiate<Decor_Controller>(this.decor_Prefabs[_index]);
		decor_Controller.transform.position = _pos;
		decor_Controller.transform.localScale = Vector3.one;
		decor_Controller.transform.rotation = Quaternion.Euler(_rot);
		if (decor_Controller.GetComponent<Interaction_Controller>().isDecorPlant)
		{
			decor_Controller.GetComponent<Plant_Controller>().SetLifeSpan(_lifeSpanIndex);
		}
		this.AddDecorControllers(decor_Controller);
		return decor_Controller;
	}

	// Token: 0x0600031D RID: 797 RVA: 0x0001C880 File Offset: 0x0001AA80
	public WallPaint_Controller PlaceWallPaint(int _index, WallPaint_Controller _old)
	{
		WallPaint_Controller wallPaint_Controller = UnityEngine.Object.Instantiate<WallPaint_Controller>(this.wallPaint_Prefabs[_index]);
		wallPaint_Controller.transform.position = _old.transform.position;
		wallPaint_Controller.transform.localScale = _old.transform.localScale;
		wallPaint_Controller.transform.rotation = _old.transform.rotation;
		wallPaint_Controller.transform.SetParent(_old.transform.parent);
		this.ReplaceWallPaintControllers(wallPaint_Controller, _old);
		return wallPaint_Controller;
	}

	// Token: 0x0600031E RID: 798 RVA: 0x0001C900 File Offset: 0x0001AB00
	public Floor_Controller PlaceFloor(int _index, Floor_Controller _old)
	{
		Floor_Controller floor_Controller = UnityEngine.Object.Instantiate<Floor_Controller>(this.floor_Prefabs[_index]);
		floor_Controller.transform.position = _old.transform.position;
		floor_Controller.transform.localScale = _old.transform.localScale;
		if (floor_Controller.transform.localScale.x == floor_Controller.transform.localScale.y)
		{
			floor_Controller.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		}
		else
		{
			floor_Controller.transform.rotation = _old.transform.rotation;
		}
		floor_Controller.transform.SetParent(_old.transform.parent);
		this.ReplaceFloorControllers(floor_Controller, _old);
		return floor_Controller;
	}

	// Token: 0x0600031F RID: 799 RVA: 0x0001C9C8 File Offset: 0x0001ABC8
	public Util_Controller PlaceUtil(int _index, GameObject _place, int _player_index)
	{
		Quaternion quaternion = Quaternion.LookRotation(Nav_Manager.instance.FindNearSphere(Player_Manager.instance.GetPlayerController(_player_index).gameObject).transform.position - _place.transform.position);
		quaternion.x = 0f;
		quaternion.z = 0f;
		return this.PlaceUtil(_index, _place.transform.position, quaternion.eulerAngles);
	}

	// Token: 0x06000320 RID: 800 RVA: 0x0001CA40 File Offset: 0x0001AC40
	public Util_Controller PlaceUtil(int _index, Vector3 _pos, Vector3 _rot)
	{
		Util_Controller util_Controller = UnityEngine.Object.Instantiate<Util_Controller>(this.util_Prefabs[_index]);
		util_Controller.transform.position = _pos;
		util_Controller.transform.localScale = Vector3.one;
		util_Controller.transform.rotation = Quaternion.Euler(_rot);
		this.AddUtilControllers(util_Controller);
		return util_Controller;
	}

	// Token: 0x06000321 RID: 801 RVA: 0x0001CA94 File Offset: 0x0001AC94
	public int GetProdDiscountLevel(int _index)
	{
		return this.prod_DiscountLevel[_index];
	}

	// Token: 0x06000322 RID: 802 RVA: 0x0001CAA4 File Offset: 0x0001ACA4
	public List<int> GetProdIndexesInStore()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.box_Controllers.Count; i++)
		{
			if (this.box_Controllers[i] && this.box_Controllers[i].isProd)
			{
				list.Add(this.box_Controllers[i].itemIndex);
			}
		}
		for (int j = 0; j < this.shelfProd_Controllers.Count; j++)
		{
			if (this.shelfProd_Controllers[j])
			{
				for (int k = 0; k < this.shelfProd_Controllers[j].height; k++)
				{
					Prod_Controller prod_Controller = this.shelfProd_Controllers[j].prodControllers[k, 0];
					if (prod_Controller)
					{
						list.Add(prod_Controller.prodIndex);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06000323 RID: 803 RVA: 0x0001CB80 File Offset: 0x0001AD80
	public void RefreshProdIndexesUnlocked()
	{
		this.unlockedProdsTillThisDay = this.GetProdIndexesUnlocked();
	}

	// Token: 0x06000324 RID: 804 RVA: 0x0001CB90 File Offset: 0x0001AD90
	public List<int> GetProdIndexesUnlocked()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < Inv_Manager.instance.prod_Prefabs.Count; i++)
		{
			if (Inv_Manager.instance.prod_Prefabs[i].buyable && Unlock_Manager.instance.item_Unlocked[0, i])
			{
				list.Add(i);
			}
		}
		return list;
	}

	// Token: 0x06000325 RID: 805 RVA: 0x0001CBF0 File Offset: 0x0001ADF0
	public List<Prod_Controller> GetProdListOnShelves()
	{
		List<Prod_Controller> list = new List<Prod_Controller>();
		for (int i = 0; i < this.prod_Controllers.Count; i++)
		{
			if (this.prod_Controllers[i] && this.prod_Controllers[i].shelf_Controller && this.prod_Controllers[i].buyableByCustomers)
			{
				list.Add(this.prod_Controllers[i]);
			}
		}
		return list;
	}

	// Token: 0x06000326 RID: 806 RVA: 0x0001CC6C File Offset: 0x0001AE6C
	public List<Prod_Controller> GetProdListOnShelvesByDailyDeals(int _qnt)
	{
		List<Prod_Controller> prodListOnShelves = this.GetProdListOnShelves();
		List<Prod_Controller> list = new List<Prod_Controller>();
		for (int i = 0; i < prodListOnShelves.Count; i++)
		{
			for (int j = 0; j < this.news_Deals_ProdIndexes.Count; j++)
			{
				if (this.news_Deals_ProdIndexes[j] == prodListOnShelves[i].prodIndex)
				{
					for (int k = 0; k < _qnt; k++)
					{
						list.Add(prodListOnShelves[i]);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06000327 RID: 807 RVA: 0x0001CCE8 File Offset: 0x0001AEE8
	public List<Prod_Controller> GetProdListOnShelvesWithDailyDeals(int _qnt)
	{
		IEnumerable<Prod_Controller> prodListOnShelves = this.GetProdListOnShelves();
		List<Prod_Controller> prodListOnShelvesByDailyDeals = this.GetProdListOnShelvesByDailyDeals(_qnt);
		List<Prod_Controller> list = new List<Prod_Controller>(prodListOnShelves);
		for (int i = 0; i < prodListOnShelvesByDailyDeals.Count; i++)
		{
			list.Add(prodListOnShelvesByDailyDeals[i]);
		}
		return list;
	}

	// Token: 0x06000328 RID: 808 RVA: 0x0001CD28 File Offset: 0x0001AF28
	public List<Prod_Controller> GetProdListOnShelvesWithDailyDealsAndProdPredilection(int _qntDailyDeals, List<Prod_Controller> _predilectionPrefabs, int _qntPredilection)
	{
		List<Prod_Controller> prodListOnShelves = this.GetProdListOnShelves();
		List<Prod_Controller> list = new List<Prod_Controller>(this.GetProdListOnShelvesWithDailyDeals(_qntDailyDeals));
		for (int i = 0; i < _predilectionPrefabs.Count; i++)
		{
			if (_predilectionPrefabs[i])
			{
				int itemIndex = this.GetItemIndex(_predilectionPrefabs[i].gameObject);
				for (int j = 0; j < prodListOnShelves.Count; j++)
				{
					if (itemIndex == prodListOnShelves[j].prodIndex)
					{
						for (int k = 0; k < _qntPredilection; k++)
						{
							list.Add(prodListOnShelves[i]);
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06000329 RID: 809 RVA: 0x0001CDC0 File Offset: 0x0001AFC0
	public List<Prod_Controller> GetProdListOnShelvesWithDailyDealsAndProdWantedNow(int _qntDailyDeals, int _wanted_index, int _qnt_wanted)
	{
		List<Prod_Controller> prodListOnShelves = this.GetProdListOnShelves();
		List<Prod_Controller> list = new List<Prod_Controller>(this.GetProdListOnShelvesWithDailyDeals(_qntDailyDeals));
		for (int i = 0; i < prodListOnShelves.Count; i++)
		{
			if (_wanted_index == prodListOnShelves[i].prodIndex)
			{
				for (int j = 0; j < _qnt_wanted; j++)
				{
					list.Add(prodListOnShelves[i]);
				}
			}
		}
		return list;
	}

	// Token: 0x0600032A RID: 810 RVA: 0x0001CE1C File Offset: 0x0001B01C
	public List<int> GetProdQntList()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.prod_Prefabs.Count; i++)
		{
			list.Add(this.GetProdQnt_OnBox(i) + this.GetProdQnt_OnShelf(i));
		}
		return list;
	}

	// Token: 0x0600032B RID: 811 RVA: 0x0001CE5C File Offset: 0x0001B05C
	public List<Prod_Controller> GetOwnedProds()
	{
		List<Prod_Controller> list = new List<Prod_Controller>();
		for (int i = 0; i < this.prod_Prefabs.Count; i++)
		{
			if (this.GetProdQnt_OnShelf(i) != 0 || this.GetProdQnt_OnBox(i) != 0)
			{
				list.Add(this.prod_Prefabs[i]);
			}
		}
		return list;
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0001CEAC File Offset: 0x0001B0AC
	public int GetProdQnt_OnShelf(int _index)
	{
		int num = 0;
		for (int i = 0; i < this.prod_Controllers.Count; i++)
		{
			if (this.prod_Controllers[i].prodIndex == _index)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600032D RID: 813 RVA: 0x0001CEEC File Offset: 0x0001B0EC
	public int GetProdQnt_OnBox(int _index)
	{
		int num = 0;
		for (int i = 0; i < this.box_Controllers.Count; i++)
		{
			if (this.box_Controllers[i].isProd && this.box_Controllers[i].itemIndex == _index)
			{
				num += this.box_Controllers[i].prodQnt;
			}
		}
		return num;
	}

	// Token: 0x0600032E RID: 814 RVA: 0x0001CF50 File Offset: 0x0001B150
	public int GetShelfQnt_OnBox(int _index)
	{
		int num = 0;
		for (int i = 0; i < this.box_Controllers.Count; i++)
		{
			if (this.box_Controllers[i].isShelf && this.box_Controllers[i].itemIndex == _index)
			{
				num += this.box_Controllers[i].prodQnt;
			}
		}
		return num;
	}

	// Token: 0x0600032F RID: 815 RVA: 0x0001CFB4 File Offset: 0x0001B1B4
	public int GetDecorQnt_OnBox(int _index)
	{
		int num = 0;
		for (int i = 0; i < this.box_Controllers.Count; i++)
		{
			if (this.box_Controllers[i].isDecor && this.box_Controllers[i].itemIndex == _index)
			{
				num += this.box_Controllers[i].prodQnt;
			}
		}
		return num;
	}

	// Token: 0x06000330 RID: 816 RVA: 0x0001D018 File Offset: 0x0001B218
	public int GetWallPaintQnt_OnBox(int _index)
	{
		int num = 0;
		for (int i = 0; i < this.box_Controllers.Count; i++)
		{
			if (this.box_Controllers[i].isWall && this.box_Controllers[i].itemIndex == _index)
			{
				num += this.box_Controllers[i].prodQnt;
			}
		}
		return num;
	}

	// Token: 0x06000331 RID: 817 RVA: 0x0001D07C File Offset: 0x0001B27C
	public int GetFloorQnt_OnBox(int _index)
	{
		int num = 0;
		for (int i = 0; i < this.box_Controllers.Count; i++)
		{
			if (this.box_Controllers[i].isFloor && this.box_Controllers[i].itemIndex == _index)
			{
				num += this.box_Controllers[i].prodQnt;
			}
		}
		return num;
	}

	// Token: 0x06000332 RID: 818 RVA: 0x0001D0E0 File Offset: 0x0001B2E0
	public int GetUtilQnt_OnBox(int _index)
	{
		int num = 0;
		for (int i = 0; i < this.box_Controllers.Count; i++)
		{
			if (this.box_Controllers[i].isUtil && this.box_Controllers[i].itemIndex == _index)
			{
				num += this.box_Controllers[i].prodQnt;
			}
		}
		return num;
	}

	// Token: 0x06000333 RID: 819 RVA: 0x0001D144 File Offset: 0x0001B344
	public bool GetIfStoreIsFull(bool _isRefrigerated)
	{
		bool result = true;
		for (int i = 0; i < this.shelfProd_Controllers.Count; i++)
		{
			if (this.shelfProd_Controllers[i] && this.shelfProd_Controllers[i].isRefrigerated == _isRefrigerated)
			{
				for (int j = 0; j < this.shelfProd_Controllers[i].height; j++)
				{
					if (!this.shelfProd_Controllers[i].prodControllers[j, 0])
					{
						return false;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06000334 RID: 820 RVA: 0x0001D1D0 File Offset: 0x0001B3D0
	public bool GetIfInvIsFull(bool _isRefrigerated)
	{
		bool result = true;
		for (int i = 0; i < this.shelfInv_Controllers.Count; i++)
		{
			if (this.shelfInv_Controllers[i] && this.shelfInv_Controllers[i].isRefrigerated == _isRefrigerated)
			{
				for (int j = 0; j < this.shelfInv_Controllers[i].height; j++)
				{
					if (!this.shelfInv_Controllers[i].boxControllers[j])
					{
						return false;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06000335 RID: 821 RVA: 0x0001D255 File Offset: 0x0001B455
	public void AddShelfProdControllers(Shelf_Controller _shelfController)
	{
		if (!this.shelfProd_Controllers.Contains(_shelfController))
		{
			this.shelfProd_Controllers.Add(_shelfController);
		}
		Nav_Manager.instance.AddInteractable(_shelfController.GetComponent<Interaction_Controller>());
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
	}

	// Token: 0x06000336 RID: 822 RVA: 0x0001D28B File Offset: 0x0001B48B
	public void DeleteShelfProdController(Shelf_Controller _shelfController)
	{
		if (this.shelfProd_Controllers.Contains(_shelfController))
		{
			this.shelfProd_Controllers.Remove(_shelfController);
		}
		Nav_Manager.instance.RemoveInteractable(_shelfController.GetComponent<Interaction_Controller>());
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
	}

	// Token: 0x06000337 RID: 823 RVA: 0x0001D2C2 File Offset: 0x0001B4C2
	public void AddShelfInvControllers(Shelf_Controller _shelfController)
	{
		if (!this.shelfInv_Controllers.Contains(_shelfController))
		{
			this.shelfInv_Controllers.Add(_shelfController);
		}
	}

	// Token: 0x06000338 RID: 824 RVA: 0x0001D2DE File Offset: 0x0001B4DE
	public void AddDecorControllers(Decor_Controller _decorController)
	{
		if (!this.decor_Controllers.Contains(_decorController))
		{
			this.decor_Controllers.Add(_decorController);
		}
		Nav_Manager.instance.AddInteractable(_decorController.GetComponent<Interaction_Controller>());
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
	}

	// Token: 0x06000339 RID: 825 RVA: 0x0001D314 File Offset: 0x0001B514
	public void DeleteDecorController(Decor_Controller _decorController)
	{
		if (this.decor_Controllers.Contains(_decorController))
		{
			this.decor_Controllers.Remove(_decorController);
		}
		Nav_Manager.instance.RemoveInteractable(_decorController.GetComponent<Interaction_Controller>());
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
	}

	// Token: 0x0600033A RID: 826 RVA: 0x0001D34B File Offset: 0x0001B54B
	public void AddWallPaintControllers(WallPaint_Controller _wallPaintController)
	{
		if (!this.wallPaint_Controllers.Contains(_wallPaintController))
		{
			this.wallPaint_Controllers.Add(_wallPaintController);
		}
		Nav_Manager.instance.AddInteractable(_wallPaintController.GetComponent<Interaction_Controller>());
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
	}

	// Token: 0x0600033B RID: 827 RVA: 0x0001D384 File Offset: 0x0001B584
	public void ReplaceWallPaintControllers(WallPaint_Controller _wallPaintController, WallPaint_Controller _old)
	{
		if (this.wallPaint_Controllers.Contains(_old))
		{
			int index = this.wallPaint_Controllers.IndexOf(_old);
			this.wallPaint_Controllers[index] = _wallPaintController;
			UnityEngine.Object.Destroy(_old.gameObject);
		}
		Nav_Manager.instance.AddInteractable(_wallPaintController.GetComponent<Interaction_Controller>());
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
	}

	// Token: 0x0600033C RID: 828 RVA: 0x0001D3DE File Offset: 0x0001B5DE
	public void DeleteWallPaintController(WallPaint_Controller _wallPaintController)
	{
		if (this.wallPaint_Controllers.Contains(_wallPaintController))
		{
			this.wallPaint_Controllers.Remove(_wallPaintController);
		}
		Nav_Manager.instance.RemoveInteractable(_wallPaintController.GetComponent<Interaction_Controller>());
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
	}

	// Token: 0x0600033D RID: 829 RVA: 0x0001D415 File Offset: 0x0001B615
	public void AddFloorControllers(Floor_Controller _floorController)
	{
		if (!this.floor_Controllers.Contains(_floorController))
		{
			this.floor_Controllers.Add(_floorController);
		}
		Nav_Manager.instance.AddInteractable(_floorController.GetComponent<Interaction_Controller>());
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
	}

	// Token: 0x0600033E RID: 830 RVA: 0x0001D44C File Offset: 0x0001B64C
	public void ReplaceFloorControllers(Floor_Controller _floorController, Floor_Controller _old)
	{
		if (this.floor_Controllers.Contains(_old))
		{
			int index = this.floor_Controllers.IndexOf(_old);
			this.floor_Controllers[index] = _floorController;
			UnityEngine.Object.Destroy(_old.gameObject);
		}
		Nav_Manager.instance.AddInteractable(_floorController.GetComponent<Interaction_Controller>());
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
	}

	// Token: 0x0600033F RID: 831 RVA: 0x0001D4A6 File Offset: 0x0001B6A6
	public void DeleteFloorController(Floor_Controller _floorController)
	{
		if (this.floor_Controllers.Contains(_floorController))
		{
			this.floor_Controllers.Remove(_floorController);
		}
		Nav_Manager.instance.RemoveInteractable(_floorController.GetComponent<Interaction_Controller>());
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
	}

	// Token: 0x06000340 RID: 832 RVA: 0x0001D4DD File Offset: 0x0001B6DD
	public void AddUtilControllers(Util_Controller _controller)
	{
		if (!this.util_Controllers.Contains(_controller))
		{
			this.util_Controllers.Add(_controller);
		}
		Nav_Manager.instance.AddInteractable(_controller.GetComponent<Interaction_Controller>());
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
	}

	// Token: 0x06000341 RID: 833 RVA: 0x0001D513 File Offset: 0x0001B713
	public void DeleteUtilController(Util_Controller _controller)
	{
		if (this.util_Controllers.Contains(_controller))
		{
			this.util_Controllers.Remove(_controller);
		}
		Nav_Manager.instance.RemoveInteractable(_controller.GetComponent<Interaction_Controller>());
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
	}

	// Token: 0x06000342 RID: 834 RVA: 0x0001D54C File Offset: 0x0001B74C
	public List<Decor_Controller> GetAllDecorControllersDisplayed_Plants()
	{
		List<Decor_Controller> list = new List<Decor_Controller>();
		for (int i = 0; i < this.decor_Controllers.Count; i++)
		{
			if (this.decor_Controllers[i].GetComponent<Interaction_Controller>().isDecorPlant)
			{
				list.Add(this.decor_Controllers[i]);
			}
		}
		return list;
	}

	// Token: 0x06000343 RID: 835 RVA: 0x0001D5A0 File Offset: 0x0001B7A0
	public bool GetIfAllItemsInBoxex()
	{
		using (List<Shelf_Controller>.Enumerator enumerator = this.shelfProd_Controllers.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				Shelf_Controller shelf_Controller = enumerator.Current;
				return false;
			}
		}
		using (List<Decor_Controller>.Enumerator enumerator2 = this.decor_Controllers.GetEnumerator())
		{
			if (enumerator2.MoveNext())
			{
				Decor_Controller decor_Controller = enumerator2.Current;
				return false;
			}
		}
		using (List<Util_Controller>.Enumerator enumerator3 = this.util_Controllers.GetEnumerator())
		{
			if (enumerator3.MoveNext())
			{
				Util_Controller util_Controller = enumerator3.Current;
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000344 RID: 836 RVA: 0x0001D674 File Offset: 0x0001B874
	public bool GetIfProdIsEventType(Prod_Controller _prod)
	{
		return _prod.prodType == Inv_Manager.ProdType.Valentines || _prod.prodType == Inv_Manager.ProdType.Christmas || _prod.prodType == Inv_Manager.ProdType.Halloween || _prod.prodType == Inv_Manager.ProdType.Easter;
	}

	// Token: 0x06000345 RID: 837 RVA: 0x0001D6A8 File Offset: 0x0001B8A8
	public void RefreshDiscountPapers()
	{
		for (int i = 0; i < this.shelfProd_Controllers.Count; i++)
		{
			this.shelfProd_Controllers[i].RefreshDiscountPaper();
		}
	}

	// Token: 0x06000346 RID: 838 RVA: 0x0001D6E0 File Offset: 0x0001B8E0
	public DiscountPaper_Controller CreateDiscountPaper(bool _isBlock)
	{
		DiscountPaper_Controller result;
		if (_isBlock)
		{
			result = UnityEngine.Object.Instantiate<DiscountPaper_Controller>(this.discount_Block);
		}
		else
		{
			result = UnityEngine.Object.Instantiate<DiscountPaper_Controller>(this.discount_Paper);
		}
		return result;
	}

	// Token: 0x06000347 RID: 839 RVA: 0x0001D70D File Offset: 0x0001B90D
	public void SetProdDiscount(int _prodIndex, int _discountLevel, bool _refresh)
	{
		this.prod_DiscountLevel[_prodIndex] = _discountLevel;
		if (_refresh)
		{
			this.RefreshDiscountPapers();
		}
	}

	// Token: 0x06000348 RID: 840 RVA: 0x0001D728 File Offset: 0x0001B928
	public void ResetProdDiscounts()
	{
		for (int i = 0; i < this.prod_DiscountLevel.Count; i++)
		{
			this.prod_DiscountLevel[i] = -1;
		}
		this.RefreshDiscountPapers();
	}

	// Token: 0x06000349 RID: 841 RVA: 0x0001D75E File Offset: 0x0001B95E
	public void Start_DiscountMenu(int _prod_index)
	{
		this.discount_prod_index = _prod_index;
		Menu_Manager.instance.SetMenuName("Discounts", "MainMenu", -1);
	}

	// Token: 0x0600034A RID: 842 RVA: 0x0001D77C File Offset: 0x0001B97C
	public void Select_Discount_Button(int _button_index)
	{
		this.prod_DiscountLevel[this.discount_prod_index] = _button_index;
		this.RefreshDiscountPapers();
	}

	// Token: 0x0600034B RID: 843 RVA: 0x0001D796 File Offset: 0x0001B996
	public void Select_Discount_Level(int _prod_index, int _level)
	{
		this.discount_prod_index = _prod_index;
		this.prod_DiscountLevel[this.discount_prod_index] = _level;
		this.RefreshDiscountPapers();
	}

	// Token: 0x0600034C RID: 844 RVA: 0x0001D7B7 File Offset: 0x0001B9B7
	public void Check_DiscountedProdQntForPurchase(int _index)
	{
		if (this.Get_DiscountedProdQntForPurchase(_index) <= 1)
		{
			this.prod_DiscountLevel[_index] = -1;
		}
	}

	// Token: 0x0600034D RID: 845 RVA: 0x0001D7D0 File Offset: 0x0001B9D0
	public int Get_DiscountedProdQntForPurchase(int _index)
	{
		int num = 0;
		foreach (Prod_Controller prod_Controller in this.prod_Controllers)
		{
			if (prod_Controller.shelf_Controller != null && prod_Controller.prodIndex == _index)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600034E RID: 846 RVA: 0x0001D83C File Offset: 0x0001BA3C
	public GameObject FindNearPlacePointWallPaint(Box_Controller _boxController, Vector3 _interaction_pos)
	{
		WallPaint_Controller wallPaint_Controller = null;
		List<WallPaint_Controller> list = new List<WallPaint_Controller>(this.wallPaint_Controllers);
		float num = 10000f;
		for (int i = 0; i < list.Count; i++)
		{
			Vector3 position = list[i].transform.position;
			position.y = 0f;
			float num2 = Vector3.Distance(_interaction_pos, position);
			if (num2 <= this.nearPlacePointDistanceWallPaint && num2 < num && list[i].itemIndex != _boxController.itemIndex && list[i].gameObject.activeInHierarchy)
			{
				wallPaint_Controller = list[i];
				num = num2;
			}
		}
		this.nearPlacePointWallPaint = wallPaint_Controller;
		if (wallPaint_Controller == null)
		{
			return null;
		}
		return wallPaint_Controller.gameObject;
	}

	// Token: 0x0600034F RID: 847 RVA: 0x0001D8F0 File Offset: 0x0001BAF0
	public GameObject FindNearPlacePointFloor(Box_Controller _boxController, Vector3 _interaction_pos)
	{
		Floor_Controller floor_Controller = null;
		List<Floor_Controller> list = new List<Floor_Controller>(this.floor_Controllers);
		float num = 10000f;
		for (int i = 0; i < list.Count; i++)
		{
			float num2 = Vector3.Distance(_interaction_pos, list[i].transform.position);
			if (num2 <= this.nearPlacePointDistanceFloor && num2 < num && list[i].itemIndex != _boxController.itemIndex && list[i].gameObject.activeInHierarchy)
			{
				floor_Controller = list[i];
				num = num2;
			}
		}
		this.nearPlacePointFloor = floor_Controller;
		if (floor_Controller == null)
		{
			return null;
		}
		return floor_Controller.gameObject;
	}

	// Token: 0x06000350 RID: 848 RVA: 0x0001D994 File Offset: 0x0001BB94
	public WallPaint_Controller GetNearPlacePointWallPaint()
	{
		if (this.nearPlacePointWallPaint && !this.nearPlacePointWallPaint.gameObject.activeInHierarchy)
		{
			this.nearPlacePointWallPaint = null;
		}
		return this.nearPlacePointWallPaint;
	}

	// Token: 0x06000351 RID: 849 RVA: 0x0001D9C2 File Offset: 0x0001BBC2
	public Floor_Controller GetNearPlacePointFloor()
	{
		if (this.nearPlacePointFloor && !this.nearPlacePointFloor.gameObject.activeInHierarchy)
		{
			this.nearPlacePointFloor = null;
		}
		return this.nearPlacePointFloor;
	}

	// Token: 0x06000352 RID: 850 RVA: 0x0001D9F0 File Offset: 0x0001BBF0
	public void SetNearPlacePointWallPaint(WallPaint_Controller _controller)
	{
		this.nearPlacePointWallPaint = _controller;
	}

	// Token: 0x06000353 RID: 851 RVA: 0x0001D9F9 File Offset: 0x0001BBF9
	public void SetNearPlacePointFloor(Floor_Controller _controller)
	{
		this.nearPlacePointFloor = _controller;
	}

	// Token: 0x06000354 RID: 852 RVA: 0x0001DA04 File Offset: 0x0001BC04
	public void LoadShelves(SaveData _data)
	{
		this.shelfProd_Controllers.TrimExcess();
		for (int i = 0; i < this.shelfProd_Controllers.Count; i++)
		{
			UnityEngine.Object.Destroy(this.shelfProd_Controllers[i].gameObject);
		}
		this.shelfProd_Controllers.Clear();
		for (int j = 0; j < _data.shelfProdIndex_SD.Count; j++)
		{
			if (_data.shelfProdIndex_SD[j] >= 0)
			{
				Shelf_Controller shelf_Controller = this.PlaceShelf(_data.shelfProdIndex_SD[j], _data.shelfProdPosition_SD[j], _data.shelfProdRotation_SD[j]);
				if (shelf_Controller && _data.shelfProdBrokenState_SD != null)
				{
					shelf_Controller.GetComponent<Interaction_Controller>().SetBrokenState(_data.shelfProdBrokenState_SD[j], false);
				}
			}
		}
		this.shelfInv_Controllers.TrimExcess();
		for (int k = 0; k < this.shelfInv_Controllers.Count; k++)
		{
			for (int l = 0; l < this.shelfInv_Controllers[k].height; l++)
			{
				this.shelfInv_Controllers[k].DeleteBox(l);
			}
		}
	}

	// Token: 0x06000355 RID: 853 RVA: 0x0001DB2C File Offset: 0x0001BD2C
	public void LoadProds(SaveData _data)
	{
		this.prod_Controllers.Clear();
		for (int i = 0; i < this.shelfProd_Controllers.Count; i++)
		{
			if (_data.shelfProdProductsIndex_SD[i].x >= 0f)
			{
				this.shelfProd_Controllers[i].StoreProd(0, this.CreateProd((int)_data.shelfProdProductsIndex_SD[i].x, (int)_data.shelfProdProductsQnt_SD[i].x, true, 7));
			}
			if (_data.shelfProdProductsIndex_SD[i].y >= 0f)
			{
				this.shelfProd_Controllers[i].StoreProd(1, this.CreateProd((int)_data.shelfProdProductsIndex_SD[i].y, (int)_data.shelfProdProductsQnt_SD[i].y, true, 7));
			}
			if (_data.shelfProdProductsIndex_SD[i].z >= 0f)
			{
				this.shelfProd_Controllers[i].StoreProd(2, this.CreateProd((int)_data.shelfProdProductsIndex_SD[i].z, (int)_data.shelfProdProductsQnt_SD[i].z, true, 7));
			}
		}
		this.box_Controllers.Clear();
		for (int j = 0; j < this.shelfInv_Controllers.Count; j++)
		{
			if (j < _data.shelfInvBoxIndex_SD.Count)
			{
				if (_data.shelfInvBoxIndex_SD[j].x >= 0f)
				{
					Box_Controller box = null;
					int num = (int)_data.shelfInvBoxIndex_SD[j].x;
					int prodQnt = (int)_data.shelfInvBoxQnt_SD[j].x;
					int num2 = this.plantHealth_StartValue;
					bool frozen = false;
					if (_data.shelfInvBoxFrozen_SD != null && (int)_data.shelfInvBoxFrozen_SD[j].x == 1)
					{
						frozen = true;
					}
					if (_data.shelfInvBoxLifeSpanIndex_SD != null)
					{
						num2 = (int)_data.shelfInvBoxLifeSpanIndex_SD[j].x;
					}
					if (_data.shelfInvBoxType_SD[j].x == 0f)
					{
						box = this.CreateBox_Prod(num, prodQnt, num2, frozen);
					}
					else if (_data.shelfInvBoxType_SD[j].x == 1f)
					{
						box = this.CreateBox_Shelf(num, prodQnt);
					}
					else if (_data.shelfInvBoxType_SD[j].x == 2f)
					{
						box = this.CreateBox_Decor(num, prodQnt, num2);
					}
					else if (_data.shelfInvBoxType_SD[j].x == 3f)
					{
						box = this.CreateBox_Wall(num, prodQnt);
					}
					else if (_data.shelfInvBoxType_SD[j].x == 4f)
					{
						box = this.CreateBox_Floor(num, prodQnt);
					}
					else if (_data.shelfInvBoxType_SD[j].x == 5f)
					{
						box = this.CreateBox_Util(num, prodQnt);
					}
					this.shelfInv_Controllers[j].StoreBox(0, box);
				}
				if (_data.shelfInvBoxIndex_SD[j].y >= 0f)
				{
					Box_Controller box2 = null;
					int num3 = (int)_data.shelfInvBoxIndex_SD[j].y;
					int prodQnt2 = (int)_data.shelfInvBoxQnt_SD[j].y;
					int num4 = this.plantHealth_StartValue;
					bool frozen2 = false;
					if (_data.shelfInvBoxFrozen_SD != null && (int)_data.shelfInvBoxFrozen_SD[j].y == 1)
					{
						frozen2 = true;
					}
					if (_data.shelfInvBoxLifeSpanIndex_SD != null)
					{
						num4 = (int)_data.shelfInvBoxLifeSpanIndex_SD[j].y;
					}
					if (_data.shelfInvBoxType_SD[j].y == 0f)
					{
						box2 = this.CreateBox_Prod(num3, prodQnt2, num4, frozen2);
					}
					else if (_data.shelfInvBoxType_SD[j].y == 1f)
					{
						box2 = this.CreateBox_Shelf(num3, prodQnt2);
					}
					else if (_data.shelfInvBoxType_SD[j].y == 2f)
					{
						box2 = this.CreateBox_Decor(num3, prodQnt2, num4);
					}
					else if (_data.shelfInvBoxType_SD[j].y == 3f)
					{
						box2 = this.CreateBox_Wall(num3, prodQnt2);
					}
					else if (_data.shelfInvBoxType_SD[j].y == 4f)
					{
						box2 = this.CreateBox_Floor(num3, prodQnt2);
					}
					else if (_data.shelfInvBoxType_SD[j].y == 5f)
					{
						box2 = this.CreateBox_Util(num3, prodQnt2);
					}
					this.shelfInv_Controllers[j].StoreBox(1, box2);
				}
				if (_data.shelfInvBoxIndex_SD[j].z >= 0f)
				{
					Box_Controller box3 = null;
					int num5 = (int)_data.shelfInvBoxIndex_SD[j].z;
					int prodQnt3 = (int)_data.shelfInvBoxQnt_SD[j].z;
					int num6 = this.plantHealth_StartValue;
					bool frozen3 = false;
					if (_data.shelfInvBoxFrozen_SD != null && (int)_data.shelfInvBoxFrozen_SD[j].z == 1)
					{
						frozen3 = true;
					}
					if (_data.shelfInvBoxLifeSpanIndex_SD != null)
					{
						num6 = (int)_data.shelfInvBoxLifeSpanIndex_SD[j].z;
					}
					if (_data.shelfInvBoxType_SD[j].z == 0f)
					{
						box3 = this.CreateBox_Prod(num5, prodQnt3, num6, frozen3);
					}
					else if (_data.shelfInvBoxType_SD[j].z == 1f)
					{
						box3 = this.CreateBox_Shelf(num5, prodQnt3);
					}
					else if (_data.shelfInvBoxType_SD[j].z == 2f)
					{
						box3 = this.CreateBox_Decor(num5, prodQnt3, num6);
					}
					else if (_data.shelfInvBoxType_SD[j].z == 3f)
					{
						box3 = this.CreateBox_Wall(num5, prodQnt3);
					}
					else if (_data.shelfInvBoxType_SD[j].z == 4f)
					{
						box3 = this.CreateBox_Floor(num5, prodQnt3);
					}
					else if (_data.shelfInvBoxType_SD[j].z == 5f)
					{
						box3 = this.CreateBox_Util(num5, prodQnt3);
					}
					this.shelfInv_Controllers[j].StoreBox(2, box3);
				}
			}
		}
		for (int k = 0; k < this.util_Controllers.Count; k++)
		{
			if (_data.utilBoxIndex_SD != null && _data.utilBoxIndex_SD[k].x >= 0f)
			{
				Box_Controller box4 = null;
				int num7 = (int)_data.utilBoxIndex_SD[k].x;
				int prodQnt4 = (int)_data.utilBoxQnt_SD[k].x;
				int num8 = this.plantHealth_StartValue;
				if (_data.utilLifeSpanIndex_SD != null)
				{
					num8 = (int)_data.utilLifeSpanIndex_SD[k].x;
				}
				if (_data.utilBoxType_SD[k].x == 0f)
				{
					box4 = this.CreateBox_Prod(num7, prodQnt4, num8, false);
				}
				else if (_data.utilBoxType_SD[k].x == 1f)
				{
					box4 = this.CreateBox_Shelf(num7, prodQnt4);
				}
				else if (_data.utilBoxType_SD[k].x == 2f)
				{
					box4 = this.CreateBox_Decor(num7, prodQnt4, num8);
				}
				else if (_data.utilBoxType_SD[k].x == 3f)
				{
					box4 = this.CreateBox_Wall(num7, prodQnt4);
				}
				else if (_data.utilBoxType_SD[k].x == 4f)
				{
					box4 = this.CreateBox_Floor(num7, prodQnt4);
				}
				else if (_data.utilBoxType_SD[k].x == 5f)
				{
					box4 = this.CreateBox_Util(num7, prodQnt4);
				}
				if (this.util_Controllers[k].shelfController)
				{
					this.util_Controllers[k].shelfController.StoreBox(0, box4);
				}
			}
		}
		for (int l = 0; l < _data.boxIndex_SD.Count; l++)
		{
			Box_Controller box_Controller = null;
			int num9 = _data.boxIndex_SD[l];
			int num10 = _data.boxType_SD[l];
			int prodQnt5 = _data.boxQnt_SD[l];
			int num11 = this.plantHealth_StartValue;
			bool frozen4 = false;
			if (_data.boxFrozen_SD != null)
			{
				frozen4 = _data.boxFrozen_SD[l];
			}
			if (_data.boxLifeSpanIndex_SD != null)
			{
				num11 = _data.boxLifeSpanIndex_SD[l];
			}
			if (num10 == 0)
			{
				box_Controller = this.CreateBox_Prod(num9, prodQnt5, num11, frozen4);
			}
			else if (num10 == 1)
			{
				box_Controller = this.CreateBox_Shelf(num9, prodQnt5);
			}
			else if (num10 == 2)
			{
				box_Controller = this.CreateBox_Decor(num9, prodQnt5, num11);
			}
			else if (num10 == 3)
			{
				box_Controller = this.CreateBox_Wall(num9, prodQnt5);
			}
			else if (num10 == 4)
			{
				box_Controller = this.CreateBox_Floor(num9, prodQnt5);
			}
			else if (num10 == 5)
			{
				box_Controller = this.CreateBox_Util(num9, prodQnt5);
			}
			box_Controller.transform.position = _data.boxPosition_SD[l];
			box_Controller.transform.rotation = Quaternion.Euler(_data.boxRotation_SD[l]);
		}
	}

	// Token: 0x06000356 RID: 854 RVA: 0x0001E438 File Offset: 0x0001C638
	public void LoadDecor(SaveData _data)
	{
		this.decor_Controllers.TrimExcess();
		for (int i = 0; i < this.decor_Controllers.Count; i++)
		{
			UnityEngine.Object.Destroy(this.decor_Controllers[i].gameObject);
		}
		this.decor_Controllers.Clear();
		for (int j = 0; j < _data.decorIndex_SD.Count; j++)
		{
			if (_data.decorIndex_SD[j] >= 0)
			{
				int lifeSpanIndex = this.plantHealth_StartValue;
				if (_data.decorLifeSpanIndex_SD != null)
				{
					lifeSpanIndex = _data.decorLifeSpanIndex_SD[j];
				}
				this.PlaceDecor(_data.decorIndex_SD[j], _data.decorPosition_SD[j], _data.decorRotation_SD[j], lifeSpanIndex);
			}
		}
		this.decor_Controllers.TrimExcess();
	}

	// Token: 0x06000357 RID: 855 RVA: 0x0001E50C File Offset: 0x0001C70C
	public void LoadWalls(SaveData _data)
	{
		for (int i = 0; i < this.wallPaint_Controllers.Count; i++)
		{
			if (i < _data.wallIndex_SD.Count)
			{
				this.PlaceWallPaint(_data.wallIndex_SD[i], this.wallPaint_Controllers[i]);
			}
		}
	}

	// Token: 0x06000358 RID: 856 RVA: 0x0001E55C File Offset: 0x0001C75C
	public void LoadFloors(SaveData _data)
	{
		for (int i = 0; i < this.floor_Controllers.Count; i++)
		{
			if (i < _data.floorIndex_SD.Count)
			{
				this.PlaceFloor(_data.floorIndex_SD[i], this.floor_Controllers[i]);
			}
		}
	}

	// Token: 0x06000359 RID: 857 RVA: 0x0001E5AC File Offset: 0x0001C7AC
	public void LoadUtil(SaveData _data)
	{
		if (_data.utilIndex_SD == null)
		{
			return;
		}
		this.util_Controllers.TrimExcess();
		for (int i = 0; i < this.util_Controllers.Count; i++)
		{
			UnityEngine.Object.Destroy(this.util_Controllers[i].gameObject);
		}
		this.util_Controllers.Clear();
		for (int j = 0; j < _data.utilIndex_SD.Count; j++)
		{
			if (_data.utilIndex_SD[j] >= 0)
			{
				Util_Controller util_Controller = this.PlaceUtil(_data.utilIndex_SD[j], _data.utilPosition_SD[j], _data.utilRotation_SD[j]);
				if (util_Controller && _data.utilBrokenState_SD != null)
				{
					util_Controller.GetComponent<Interaction_Controller>().SetBrokenState(_data.utilBrokenState_SD[j], false);
				}
			}
		}
		this.util_Controllers.TrimExcess();
	}

	// Token: 0x0600035A RID: 858 RVA: 0x0001E694 File Offset: 0x0001C894
	public void StoreBoxesToMoveOut()
	{
		for (int i = 0; i < this.box_Controllers.Count; i++)
		{
			if (this.box_Controllers[i])
			{
				this.AddDelivery(this.box_Controllers[i].itemIndex, this.box_Controllers[i].GetBoxType(), this.box_Controllers[i].prodQnt, true, 4);
			}
		}
	}

	// Token: 0x0600035B RID: 859 RVA: 0x0001E708 File Offset: 0x0001C908
	public void CreateDirtNextToObject(GameObject _gameObject, int _index, int _qnt)
	{
		int num = 0;
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
		List<GameObject> list = new List<GameObject>(Nav_Manager.instance.GetActiveNavSpheres());
		float[] array = new float[]
		{
			0f,
			90f,
			180f,
			270f
		};
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].gameObject.activeSelf && !World_Manager.instance.currentLevel.GetUndirtableNavSpheres().Contains(list[i]))
			{
				if (Vector3.Distance(_gameObject.transform.position, list[i].transform.position) <= 2.5f)
				{
					this.CreateDirt(_index, list[i].transform.position, array[UnityEngine.Random.Range(0, 4)]);
					num++;
				}
				if (num >= _qnt)
				{
					break;
				}
			}
		}
	}

	// Token: 0x0600035C RID: 860 RVA: 0x0001E7DC File Offset: 0x0001C9DC
	public void CreateDirt(int _index, float _spawnOdd)
	{
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
		List<GameObject> list = new List<GameObject>(Nav_Manager.instance.GetActiveNavSpheres());
		float[] array = new float[]
		{
			0f,
			90f,
			180f,
			270f
		};
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].gameObject.activeSelf && !World_Manager.instance.currentLevel.GetUndirtableNavSpheres().Contains(list[i]) && (float)UnityEngine.Random.Range(0, 100) <= _spawnOdd)
			{
				this.CreateDirt(_index, list[i].transform.position, array[UnityEngine.Random.Range(0, 4)]);
			}
		}
	}

	// Token: 0x0600035D RID: 861 RVA: 0x0001E884 File Offset: 0x0001CA84
	public void CreateDirt(int _index, Vector3 _pos, float _eulerAngleY)
	{
		Interaction_Controller component = UnityEngine.Object.Instantiate<Interaction_Controller>(this.dirt_Prefabs[_index]).GetComponent<Interaction_Controller>();
		component.transform.localScale = Vector3.one;
		component.transform.position = _pos;
		component.transform.rotation = Quaternion.Euler(0f, _eulerAngleY, 0f);
		this.AddDirtControllers(component);
	}

	// Token: 0x0600035E RID: 862 RVA: 0x0001E8E6 File Offset: 0x0001CAE6
	public void AddDirtControllers(Interaction_Controller _dirtController)
	{
		if (!this.dirt_Controllers.Contains(_dirtController))
		{
			this.dirt_Controllers.Add(_dirtController);
		}
		Nav_Manager.instance.AddInteractable(_dirtController.GetComponent<Interaction_Controller>());
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
	}

	// Token: 0x0600035F RID: 863 RVA: 0x0001E91C File Offset: 0x0001CB1C
	public void DeleteDirtController(Interaction_Controller _dirtController)
	{
		if (this.dirt_Controllers.Contains(_dirtController))
		{
			this.dirt_Controllers.Remove(_dirtController);
		}
		Nav_Manager.instance.RemoveInteractable(_dirtController.GetComponent<Interaction_Controller>());
		Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
		UnityEngine.Object.Destroy(_dirtController.gameObject);
	}

	// Token: 0x06000360 RID: 864 RVA: 0x0001E96C File Offset: 0x0001CB6C
	public void DeleteAllDirtControllers()
	{
		List<Interaction_Controller> list = new List<Interaction_Controller>(this.dirt_Controllers);
		for (int i = 0; i < list.Count; i++)
		{
			this.DeleteDirtController(list[i]);
		}
	}

	// Token: 0x06000361 RID: 865 RVA: 0x0001E9A4 File Offset: 0x0001CBA4
	public void ResetInventory(bool _includeWallsAndFloors)
	{
		for (int i = 0; i < this.prod_Controllers.Count; i++)
		{
			if (this.prod_Controllers[i])
			{
				UnityEngine.Object.Destroy(this.prod_Controllers[i].gameObject);
			}
		}
		for (int j = 0; j < this.box_Controllers.Count; j++)
		{
			if (this.box_Controllers[j])
			{
				UnityEngine.Object.Destroy(this.box_Controllers[j].gameObject);
			}
		}
		for (int k = 0; k < this.shelfProd_Controllers.Count; k++)
		{
			if (this.shelfProd_Controllers[k])
			{
				UnityEngine.Object.Destroy(this.shelfProd_Controllers[k].gameObject);
			}
		}
		for (int l = 0; l < this.decor_Controllers.Count; l++)
		{
			if (this.decor_Controllers[l])
			{
				UnityEngine.Object.Destroy(this.decor_Controllers[l].gameObject);
			}
		}
		for (int m = 0; m < this.dirt_Controllers.Count; m++)
		{
			if (this.dirt_Controllers[m])
			{
				UnityEngine.Object.Destroy(this.dirt_Controllers[m].gameObject);
			}
		}
		if (_includeWallsAndFloors)
		{
			for (int n = 0; n < this.shelfInv_Controllers.Count; n++)
			{
				if (this.shelfInv_Controllers[n])
				{
					UnityEngine.Object.Destroy(this.shelfInv_Controllers[n].gameObject);
				}
			}
			for (int num = 0; num < this.wallPaint_Controllers.Count; num++)
			{
				if (this.wallPaint_Controllers[num])
				{
					UnityEngine.Object.Destroy(this.wallPaint_Controllers[num].gameObject);
				}
			}
			for (int num2 = 0; num2 < this.floor_Controllers.Count; num2++)
			{
				if (this.floor_Controllers[num2])
				{
					UnityEngine.Object.Destroy(this.floor_Controllers[num2].gameObject);
				}
			}
			this.shelfInv_Controllers.Clear();
			this.wallPaint_Controllers.Clear();
			this.floor_Controllers.Clear();
		}
		for (int num3 = 0; num3 < this.util_Controllers.Count; num3++)
		{
			if (this.util_Controllers[num3])
			{
				UnityEngine.Object.Destroy(this.util_Controllers[num3].gameObject);
			}
		}
		this.prod_Controllers.Clear();
		this.box_Controllers.Clear();
		this.shelfProd_Controllers.Clear();
		this.decor_Controllers.Clear();
		this.dirt_Controllers.Clear();
		this.util_Controllers.Clear();
	}

	// Token: 0x06000362 RID: 866 RVA: 0x0001EC6F File Offset: 0x0001CE6F
	public void SetNewspaperDeals(int _dealIndex, int _itemIndex, int _daysLeft)
	{
		if (this.news_Deals_ProdIndexes[_dealIndex] >= 0)
		{
			return;
		}
		this.news_Deals_ProdIndexes[_dealIndex] = _itemIndex;
		this.news_Deals_DaysLeft[_dealIndex] = _daysLeft;
	}

	// Token: 0x06000363 RID: 867 RVA: 0x0001EC9C File Offset: 0x0001CE9C
	public void UpdateNewspaperDeals()
	{
		for (int i = 0; i < this.news_Deals_DaysLeft.Count; i++)
		{
			if (this.news_Deals_DaysLeft[i] > 0)
			{
				List<int> list = this.news_Deals_DaysLeft;
				int index = i;
				int num = list[index];
				list[index] = num - 1;
			}
			if (this.news_Deals_DaysLeft[i] <= 0)
			{
				this.news_Deals_DaysLeft[i] = 0;
				this.news_Deals_ProdIndexes[i] = -1;
			}
		}
	}

	// Token: 0x06000364 RID: 868 RVA: 0x0001ED10 File Offset: 0x0001CF10
	public void LoadNewspaperDeals(SaveData _data)
	{
		if (_data.inv_Deals_DaysLeft == null || _data.inv_Deals_Indexes == null)
		{
			return;
		}
		for (int i = 0; i < _data.inv_Deals_Indexes.Count; i++)
		{
			this.news_Deals_ProdIndexes[i] = _data.inv_Deals_Indexes[i];
			this.news_Deals_DaysLeft[i] = _data.inv_Deals_DaysLeft[i];
		}
	}

	// Token: 0x06000365 RID: 869 RVA: 0x0001ED74 File Offset: 0x0001CF74
	public void RefreshBrokenStates()
	{
		for (int i = 0; i < this.util_Controllers.Count; i++)
		{
			this.util_Controllers[i].GetComponent<Interaction_Controller>().RefreshBrokenState();
		}
		for (int j = 0; j < this.shelfProd_Controllers.Count; j++)
		{
			this.shelfProd_Controllers[j].GetComponent<Interaction_Controller>().RefreshBrokenState();
		}
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0001EDDC File Offset: 0x0001CFDC
	public void SetAllBrokenStates(bool _b)
	{
		for (int i = 0; i < this.util_Controllers.Count; i++)
		{
			this.util_Controllers[i].GetComponent<Interaction_Controller>().SetBrokenState(_b, false);
		}
		for (int j = 0; j < this.shelfProd_Controllers.Count; j++)
		{
			this.shelfProd_Controllers[j].GetComponent<Interaction_Controller>().SetBrokenState(_b, false);
		}
	}

	// Token: 0x06000367 RID: 871 RVA: 0x0001EE48 File Offset: 0x0001D048
	public void Reset_CeilingLostBoxes()
	{
		foreach (Box_Controller box_Controller in this.box_Controllers)
		{
			if (!box_Controller.isHeld && box_Controller.transform.position.y >= 4f)
			{
				int itemIndex = box_Controller.itemIndex;
				int boxType = box_Controller.GetBoxType();
				int prodQnt = box_Controller.prodQnt;
				this.AddDelivery(itemIndex, boxType, prodQnt, false, 1);
				box_Controller.DeleteBox(0);
			}
		}
	}

	// Token: 0x06000368 RID: 872 RVA: 0x0001EEE0 File Offset: 0x0001D0E0
	public void DecreaseLifeSpan()
	{
		for (int i = 0; i < this.box_Controllers.Count; i++)
		{
			if (!this.box_Controllers[i].frozen)
			{
				this.box_Controllers[i].DecreaseLifeSpan();
			}
		}
		for (int j = 0; j < this.prod_Controllers.Count; j++)
		{
			this.prod_Controllers[j].DecreaseLifeSpan();
		}
		for (int k = 0; k < this.decor_Controllers.Count; k++)
		{
			if (this.decor_Controllers[k])
			{
				Interaction_Controller component = this.decor_Controllers[k].GetComponent<Interaction_Controller>();
				if (component.isDecorPlant)
				{
					component.GetComponent<Plant_Controller>().DecreaseLifeSpan();
				}
			}
		}
	}

	// Token: 0x06000369 RID: 873 RVA: 0x0001EF9C File Offset: 0x0001D19C
	public int GetLifeSpanLevels(int _index)
	{
		return this.prod_LifeSpan_Levels[_index];
	}

	// Token: 0x0600036A RID: 874 RVA: 0x0001EFA8 File Offset: 0x0001D1A8
	public void Check_IfPlayerNeedHelpBoxes()
	{
		if (Finances_Manager.instance.GetMoney() > 30f)
		{
			return;
		}
		foreach (Box_Controller box_Controller in this.box_Controllers)
		{
			if (box_Controller.isProd && box_Controller.prodQnt >= 2 && box_Controller.lifeSpanIndex >= 1)
			{
				return;
			}
		}
		if (this.prod_Controllers.Count >= 2)
		{
			return;
		}
		List<Prod_Controller> list = new List<Prod_Controller>();
		list.Add(Inv_Manager.instance.prod_Prefabs[0]);
		list.Add(Inv_Manager.instance.prod_Prefabs[1]);
		list.Add(Inv_Manager.instance.prod_Prefabs[3]);
		list.Add(Inv_Manager.instance.prod_Prefabs[4]);
		list.Add(Inv_Manager.instance.prod_Prefabs[6]);
		list.Add(Inv_Manager.instance.prod_Prefabs[9]);
		Prod_Controller prod_Controller = list[UnityEngine.Random.Range(0, list.Count - 1)];
		this.AddDelivery(this.GetItemIndex(prod_Controller.gameObject), 0, this.shop_Deliv_Qnt, false, 1);
		this.DeliverProds();
	}

	// Token: 0x0600036B RID: 875 RVA: 0x0001F0F8 File Offset: 0x0001D2F8
	public void RandomnlyCreateItems()
	{
		List<int> list = new List<int>();
		list.Add(0);
		list.Add(1);
		list.Add(3);
		list.Add(4);
		list.Add(6);
		list.Add(9);
		int num = Mathf.CeilToInt((float)this.shelfProd_Controllers.Count * 0.7f);
		for (int i = 0; i < num; i++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			if (this.prod_Prefabs[list[index]].buyable)
			{
				int index2 = UnityEngine.Random.Range(0, this.shelfProd_Controllers.Count);
				int num2 = UnityEngine.Random.Range(0, this.shelfProd_Controllers[index2].height);
				if (this.shelfProd_Controllers[index2].prodControllers[num2, 0] == null)
				{
					this.shelfProd_Controllers[index2].StoreProd(num2, Inv_Manager.instance.CreateProd(list[index], 2, true, 7));
				}
			}
		}
		int num3 = Mathf.CeilToInt((float)this.shelfInv_Controllers.Count * 0.7f);
		for (int j = 0; j < num3; j++)
		{
			int index3 = UnityEngine.Random.Range(0, list.Count);
			if (this.prod_Prefabs[list[index3]].buyable)
			{
				int index4 = UnityEngine.Random.Range(0, this.shelfInv_Controllers.Count);
				int num4 = UnityEngine.Random.Range(0, this.shelfInv_Controllers[index4].height);
				if (this.shelfInv_Controllers[index4].boxControllers[num4] == null)
				{
					this.shelfInv_Controllers[index4].StoreBox(num4, Inv_Manager.instance.CreateBox_Prod(list[index3], 8, 7, false));
				}
			}
		}
	}

	// Token: 0x0600036C RID: 876 RVA: 0x0001F2CC File Offset: 0x0001D4CC
	public Inv_Manager()
	{
		int[] array = new int[5];
		array[0] = 1;
		this.prod_deliveryDays_Available = array;
		this.prod_deliveryLifeSpan_Available = new int[]
		{
			7,
			7,
			7,
			7,
			7
		};
		this.prod_deliveryPrice_Available = new float[]
		{
			1f,
			2f,
			2f,
			2f
		};
		this.shelfProd_Prefabs = new List<Shelf_Controller>();
		this.shelfProd_Controllers = new List<Shelf_Controller>();
		this.shelfInv_Prefabs = new List<Shelf_Controller>();
		this.shelfInv_Controllers = new List<Shelf_Controller>();
		this.decor_Prefabs = new List<Decor_Controller>();
		this.decor_Controllers = new List<Decor_Controller>();
		this.wallPaint_Prefabs = new List<WallPaint_Controller>();
		this.wallPaint_Controllers = new List<WallPaint_Controller>();
		this.floor_Prefabs = new List<Floor_Controller>();
		this.floor_Controllers = new List<Floor_Controller>();
		this.util_Prefabs = new List<Util_Controller>();
		this.util_Controllers = new List<Util_Controller>();
		this.box_Controllers = new List<Box_Controller>();
		this.boxSize_Sprites = new List<Sprite>();
		this.deliveryPlaces = new List<GameObject>();
		this.prod_LifeSpan_Levels = new int[]
		{
			5,
			2,
			1,
			0
		};
		this.lifeSpan_StartValue = 7;
		this.lifeSpan_DailyLoss_RightShelf = 1;
		this.lifeSpan_DailyLoss_WrongShelf = 3;
		this.lifeSpan_DailyLoss_NoShelf = 2;
		this.plantHealth_StartValue = 3;
		this.prodTypes_Names = new string[]
		{
			"Null",
			"Fruit",
			"Liquids",
			"Food",
			"Vegetable",
			"Tools",
			"Meat",
			"Hygiene",
			"Electronics",
			"Sport",
			"valentines",
			"easter",
			"halloween",
			"christmas"
		};
		this.boxSize = 8;
		this.boxSize_ShelvesAndDecor = 1;
		this.prodSellPriceMultiplierTemp = 5f;
		this.prod_SellPrices = new int[0];
		this.nearPlacePointDistanceWallPaint = 2f;
		this.nearPlacePointDistanceFloor = 1.5f;
		this.dirt_Prefabs = new List<Interaction_Controller>();
		this.dirt_Controllers = new List<Interaction_Controller>();
		this.news_Deals_ProdIndexes = new List<int>
		{
			-1,
			-1
		};
		this.news_Deals_DaysLeft = new List<int>
		{
			0,
			0
		};
		this.news_Deals_Price = 20;
		base..ctor();
	}

	// Token: 0x0400036E RID: 878
	public static Inv_Manager instance;

	// Token: 0x0400036F RID: 879
	[Header("Prod Odds")]
	[SerializeField]
	[Range(0f, 200f)]
	private float[] prodThinking_BuyOdd_Values;

	// Token: 0x04000370 RID: 880
	[SerializeField]
	[Range(0f, 100f)]
	private float[] prodThinking_BuyOdd_Low;

	// Token: 0x04000371 RID: 881
	[SerializeField]
	[Range(0f, 100f)]
	private float[] prodThinking_BuyOdd_Normal;

	// Token: 0x04000372 RID: 882
	[SerializeField]
	[Range(0f, 100f)]
	private float[] prodThinking_BuyOdd_High;

	// Token: 0x04000373 RID: 883
	public int[] prodBuyMaxQnt_ByRawDemand = new int[]
	{
		99,
		2,
		1
	};

	// Token: 0x04000374 RID: 884
	[Header("Prods")]
	[SerializeField]
	public List<Prod_Controller> prod_Prefabs = new List<Prod_Controller>();

	// Token: 0x04000375 RID: 885
	[SerializeField]
	public Texture2D prod_SpritesSheet;

	// Token: 0x04000376 RID: 886
	[SerializeField]
	public Sprite[] prod_Sprites;

	// Token: 0x04000377 RID: 887
	[SerializeField]
	public List<Prod_Controller> prod_Controllers = new List<Prod_Controller>();

	// Token: 0x04000378 RID: 888
	[SerializeField]
	public ParticleSystem prod_SpoiledParticles;

	// Token: 0x04000379 RID: 889
	private List<int> prod_DiscountLevel = new List<int>();

	// Token: 0x0400037A RID: 890
	public float[] prod_DiscountValue = new float[]
	{
		30f,
		50f,
		70f
	};

	// Token: 0x0400037B RID: 891
	public List<int> unlockedProdsTillThisDay = new List<int>();

	// Token: 0x0400037C RID: 892
	[SerializeField]
	public List<int> prod_deliveryIndexes = new List<int>();

	// Token: 0x0400037D RID: 893
	[SerializeField]
	public List<int> prod_deliveryCategories = new List<int>();

	// Token: 0x0400037E RID: 894
	[SerializeField]
	public List<int> prod_deliveryQnt = new List<int>();

	// Token: 0x0400037F RID: 895
	[SerializeField]
	public List<int> prod_deliverySupplierIndexes = new List<int>();

	// Token: 0x04000380 RID: 896
	[SerializeField]
	public List<int> prod_deliveryDaysIndexes = new List<int>();

	// Token: 0x04000381 RID: 897
	[SerializeField]
	public List<int> prod_deliveryLifeSpanIndexes = new List<int>();

	// Token: 0x04000382 RID: 898
	[SerializeField]
	public int shop_Deliv_Qnt;

	// Token: 0x04000383 RID: 899
	public readonly int[] prod_deliveryDays_Available;

	// Token: 0x04000384 RID: 900
	public readonly int[] prod_deliveryLifeSpan_Available;

	// Token: 0x04000385 RID: 901
	public readonly float[] prod_deliveryPrice_Available;

	// Token: 0x04000386 RID: 902
	[Header("Shelves")]
	[SerializeField]
	public List<Shelf_Controller> shelfProd_Prefabs;

	// Token: 0x04000387 RID: 903
	[SerializeField]
	public List<Shelf_Controller> shelfProd_Controllers;

	// Token: 0x04000388 RID: 904
	[SerializeField]
	public List<Shelf_Controller> shelfInv_Prefabs;

	// Token: 0x04000389 RID: 905
	[SerializeField]
	public List<Shelf_Controller> shelfInv_Controllers;

	// Token: 0x0400038A RID: 906
	[Header("Decor")]
	[SerializeField]
	public List<Decor_Controller> decor_Prefabs;

	// Token: 0x0400038B RID: 907
	[SerializeField]
	public List<Decor_Controller> decor_Controllers;

	// Token: 0x0400038C RID: 908
	[Header("WallPaint")]
	[SerializeField]
	public List<WallPaint_Controller> wallPaint_Prefabs;

	// Token: 0x0400038D RID: 909
	[SerializeField]
	public List<WallPaint_Controller> wallPaint_Controllers;

	// Token: 0x0400038E RID: 910
	[Header("Floor")]
	[SerializeField]
	public List<Floor_Controller> floor_Prefabs;

	// Token: 0x0400038F RID: 911
	[SerializeField]
	public List<Floor_Controller> floor_Controllers;

	// Token: 0x04000390 RID: 912
	[Header("Util")]
	[SerializeField]
	public List<Util_Controller> util_Prefabs;

	// Token: 0x04000391 RID: 913
	[SerializeField]
	public List<Util_Controller> util_Controllers;

	// Token: 0x04000392 RID: 914
	[Header("Boxes")]
	[SerializeField]
	public List<Box_Controller> box_Controllers;

	// Token: 0x04000393 RID: 915
	[SerializeField]
	public List<Sprite> boxSize_Sprites;

	// Token: 0x04000394 RID: 916
	[SerializeField]
	private Box_Controller box_Prefab_ProdNormal;

	// Token: 0x04000395 RID: 917
	[SerializeField]
	private Box_Controller box_Prefab_ProdRefrigerated;

	// Token: 0x04000396 RID: 918
	[SerializeField]
	private Box_Controller box_Prefab_Shelf;

	// Token: 0x04000397 RID: 919
	[SerializeField]
	private Box_Controller box_Prefab_Decoration;

	// Token: 0x04000398 RID: 920
	[SerializeField]
	private Box_Controller box_Prefab_Wall;

	// Token: 0x04000399 RID: 921
	[SerializeField]
	private Box_Controller box_Prefab_Floor;

	// Token: 0x0400039A RID: 922
	[SerializeField]
	private Box_Controller box_Prefab_Util;

	// Token: 0x0400039B RID: 923
	[Header("Delivery")]
	[SerializeField]
	public List<GameObject> deliveryPlaces;

	// Token: 0x0400039C RID: 924
	[Header("Food LifeSpan")]
	private int[] prod_LifeSpan_Levels;

	// Token: 0x0400039D RID: 925
	[SerializeField]
	public Color[] prod_LifeSpan_Colors;

	// Token: 0x0400039E RID: 926
	public int lifeSpan_StartValue;

	// Token: 0x0400039F RID: 927
	public int lifeSpan_DailyLoss_RightShelf;

	// Token: 0x040003A0 RID: 928
	public int lifeSpan_DailyLoss_WrongShelf;

	// Token: 0x040003A1 RID: 929
	public int lifeSpan_DailyLoss_NoShelf;

	// Token: 0x040003A2 RID: 930
	[Header("Plant Life Span")]
	[SerializeField]
	public Color[] plantHealth_Colors;

	// Token: 0x040003A3 RID: 931
	public int plantHealth_StartValue;

	// Token: 0x040003A5 RID: 933
	public int boxSize;

	// Token: 0x040003A6 RID: 934
	public int boxSize_ShelvesAndDecor;

	// Token: 0x040003A7 RID: 935
	private float prodSellPriceMultiplierTemp;

	// Token: 0x040003A8 RID: 936
	private float dropBoxes_Timer;

	// Token: 0x040003A9 RID: 937
	[HideInInspector]
	public int[] prod_SellPrices;

	// Token: 0x040003AA RID: 938
	[Header("Discount")]
	[SerializeField]
	private DiscountPaper_Controller discount_Block;

	// Token: 0x040003AB RID: 939
	[SerializeField]
	private DiscountPaper_Controller discount_Paper;

	// Token: 0x040003AC RID: 940
	[SerializeField]
	public Material[] discount_PaperMaterials;

	// Token: 0x040003AD RID: 941
	[HideInInspector]
	public int discount_prod_index;

	// Token: 0x040003AE RID: 942
	[Header("Place Point")]
	[SerializeField]
	private WallPaint_Controller nearPlacePointWallPaint;

	// Token: 0x040003AF RID: 943
	[SerializeField]
	private Floor_Controller nearPlacePointFloor;

	// Token: 0x040003B0 RID: 944
	private float nearPlacePointDistanceWallPaint;

	// Token: 0x040003B1 RID: 945
	private float nearPlacePointDistanceFloor;

	// Token: 0x040003B2 RID: 946
	[Header("Dirt")]
	[SerializeField]
	private List<Interaction_Controller> dirt_Prefabs;

	// Token: 0x040003B3 RID: 947
	[SerializeField]
	public List<Interaction_Controller> dirt_Controllers;

	// Token: 0x040003B4 RID: 948
	[Header("Newspaper Deals")]
	[HideInInspector]
	public List<int> news_Deals_ProdIndexes;

	// Token: 0x040003B5 RID: 949
	[HideInInspector]
	public List<int> news_Deals_DaysLeft;

	// Token: 0x040003B6 RID: 950
	public int news_Deals_Price;

	// Token: 0x0200007D RID: 125
	public enum ProdType
	{
		// Token: 0x040006AE RID: 1710
		Null,
		// Token: 0x040006AF RID: 1711
		Fruit,
		// Token: 0x040006B0 RID: 1712
		Liquids,
		// Token: 0x040006B1 RID: 1713
		Food,
		// Token: 0x040006B2 RID: 1714
		Vegetable,
		// Token: 0x040006B3 RID: 1715
		Tools,
		// Token: 0x040006B4 RID: 1716
		Meat,
		// Token: 0x040006B5 RID: 1717
		Hygiene,
		// Token: 0x040006B6 RID: 1718
		Electronics,
		// Token: 0x040006B7 RID: 1719
		Sport,
		// Token: 0x040006B8 RID: 1720
		Valentines,
		// Token: 0x040006B9 RID: 1721
		Easter,
		// Token: 0x040006BA RID: 1722
		Halloween,
		// Token: 0x040006BB RID: 1723
		Christmas
	}
}
