using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class Finances_Manager : MonoBehaviour
{
	// Token: 0x0600026C RID: 620 RVA: 0x00014BA0 File Offset: 0x00012DA0
	private void Awake()
	{
		if (!Finances_Manager.instance)
		{
			Finances_Manager.instance = this;
		}
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00014BB4 File Offset: 0x00012DB4
	public void SetMoney(float _money)
	{
		this.gameMoney = _money;
		if (this.gameMoney < 0f)
		{
			this.gameMoney = 0f;
		}
		Menu_Manager.instance.RefreshMoney(this.gameMoney);
	}

	// Token: 0x0600026E RID: 622 RVA: 0x00014BE5 File Offset: 0x00012DE5
	public void AddMoney(float _money)
	{
		this.SetMoney(this.gameMoney + _money);
		if (_money > 0f)
		{
			Menu_Manager.instance.AnimateMoneyUI(true);
			return;
		}
		Menu_Manager.instance.AnimateMoneyUI(false);
	}

	// Token: 0x0600026F RID: 623 RVA: 0x00014C14 File Offset: 0x00012E14
	public float GetMoney()
	{
		return this.gameMoney;
	}

	// Token: 0x06000270 RID: 624 RVA: 0x00014C1C File Offset: 0x00012E1C
	public bool CheckHasMoney(float _money)
	{
		return this.GetMoney() >= _money;
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00014C2C File Offset: 0x00012E2C
	public void AddTo_OutProds(float _value)
	{
		List<float> list = this.list_OutProd;
		list[0] = list[0] + _value;
	}

	// Token: 0x06000272 RID: 626 RVA: 0x00014C50 File Offset: 0x00012E50
	public void AddTo_OutFurniture(float _value)
	{
		List<float> list = this.list_OutFurniture;
		list[0] = list[0] + _value;
	}

	// Token: 0x06000273 RID: 627 RVA: 0x00014C74 File Offset: 0x00012E74
	public void AddTo_OutStaff(float _value)
	{
		List<float> list = this.list_OutStaff;
		list[0] = list[0] + _value;
	}

	// Token: 0x06000274 RID: 628 RVA: 0x00014C98 File Offset: 0x00012E98
	public void AddTo_OutExpansion(float _value)
	{
		List<float> list = this.list_OutExpansion;
		list[0] = list[0] + _value;
	}

	// Token: 0x06000275 RID: 629 RVA: 0x00014CBC File Offset: 0x00012EBC
	public void AddTo_OutMarketing(float _value)
	{
		List<float> list = this.list_OutMarketing;
		list[0] = list[0] + _value;
	}

	// Token: 0x06000276 RID: 630 RVA: 0x00014CE0 File Offset: 0x00012EE0
	public void AddTo_OutOperational(float _value)
	{
		List<float> list = this.list_OutOperational;
		list[0] = list[0] + _value;
	}

	// Token: 0x06000277 RID: 631 RVA: 0x00014D04 File Offset: 0x00012F04
	public void AddTo_InSales(float _value)
	{
		List<float> list = this.list_InSales;
		list[0] = list[0] + _value;
	}

	// Token: 0x06000278 RID: 632 RVA: 0x00014D28 File Offset: 0x00012F28
	public void AddTo_InPrizes(float _value)
	{
		List<float> list = this.list_InPrizes;
		list[0] = list[0] + _value;
	}

	// Token: 0x06000279 RID: 633 RVA: 0x00014D4C File Offset: 0x00012F4C
	public List<float> GetList_OutProds()
	{
		return this.list_OutProd;
	}

	// Token: 0x0600027A RID: 634 RVA: 0x00014D54 File Offset: 0x00012F54
	public List<float> GetList_OutFurniture()
	{
		return this.list_OutFurniture;
	}

	// Token: 0x0600027B RID: 635 RVA: 0x00014D5C File Offset: 0x00012F5C
	public List<float> GetList_OutStaff()
	{
		return this.list_OutStaff;
	}

	// Token: 0x0600027C RID: 636 RVA: 0x00014D64 File Offset: 0x00012F64
	public List<float> GetList_OutExpansion()
	{
		return this.list_OutExpansion;
	}

	// Token: 0x0600027D RID: 637 RVA: 0x00014D6C File Offset: 0x00012F6C
	public List<float> GetList_OutMarketing()
	{
		return this.list_OutMarketing;
	}

	// Token: 0x0600027E RID: 638 RVA: 0x00014D74 File Offset: 0x00012F74
	public List<float> GetList_OutOperational()
	{
		return this.list_OutOperational;
	}

	// Token: 0x0600027F RID: 639 RVA: 0x00014D7C File Offset: 0x00012F7C
	public List<float> GetList_InSales()
	{
		return this.list_InSales;
	}

	// Token: 0x06000280 RID: 640 RVA: 0x00014D84 File Offset: 0x00012F84
	public List<float> GetList_InPrizes()
	{
		return this.list_InPrizes;
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00014D8C File Offset: 0x00012F8C
	public float GetMoneyOutByIndex(int _index)
	{
		return this.list_OutProd[_index] + this.list_OutFurniture[_index] + this.list_OutStaff[_index] + this.list_OutExpansion[_index] + this.list_OutMarketing[_index] + this.list_OutOperational[_index];
	}

	// Token: 0x06000282 RID: 642 RVA: 0x00014DE6 File Offset: 0x00012FE6
	public float GetMoneyInByIndex(int _index)
	{
		return this.list_InSales[_index] + this.list_InPrizes[_index];
	}

	// Token: 0x06000283 RID: 643 RVA: 0x00014E04 File Offset: 0x00013004
	public void SetNextList()
	{
		List<float> list = new List<float>();
		List<float> list2 = new List<float>();
		List<float> list3 = new List<float>();
		List<float> list4 = new List<float>();
		List<float> list5 = new List<float>();
		List<float> list6 = new List<float>();
		List<float> list7 = new List<float>();
		List<float> list8 = new List<float>();
		list.Add(0f);
		list2.Add(0f);
		list3.Add(0f);
		list4.Add(0f);
		list5.Add(0f);
		list6.Add(0f);
		list7.Add(0f);
		list8.Add(0f);
		for (int i = 0; i < 7; i++)
		{
			if (i < this.list_OutProd.Count)
			{
				list.Add(this.list_OutProd[i]);
			}
			if (i < this.list_OutFurniture.Count)
			{
				list2.Add(this.list_OutFurniture[i]);
			}
			if (i < this.list_OutStaff.Count)
			{
				list3.Add(this.list_OutStaff[i]);
			}
			if (i < this.list_OutExpansion.Count)
			{
				list4.Add(this.list_OutExpansion[i]);
			}
			if (i < this.list_OutMarketing.Count)
			{
				list5.Add(this.list_OutMarketing[i]);
			}
			if (i < this.list_OutOperational.Count)
			{
				list6.Add(this.list_OutOperational[i]);
			}
			if (i < this.list_InSales.Count)
			{
				list7.Add(this.list_InSales[i]);
			}
			if (i < this.list_InPrizes.Count)
			{
				list8.Add(this.list_InPrizes[i]);
			}
		}
		this.list_OutProd = new List<float>(list);
		this.list_OutFurniture = new List<float>(list2);
		this.list_OutStaff = new List<float>(list3);
		this.list_OutExpansion = new List<float>(list4);
		this.list_OutMarketing = new List<float>(list5);
		this.list_OutOperational = new List<float>(list6);
		this.list_InSales = new List<float>(list7);
		this.list_InPrizes = new List<float>(list8);
	}

	// Token: 0x06000284 RID: 644 RVA: 0x00015030 File Offset: 0x00013230
	public void SetNewList()
	{
		this.list_OutProd = new List<float>();
		this.list_OutFurniture = new List<float>();
		this.list_OutStaff = new List<float>();
		this.list_OutExpansion = new List<float>();
		this.list_OutMarketing = new List<float>();
		this.list_OutOperational = new List<float>();
		this.list_InSales = new List<float>();
		this.list_InPrizes = new List<float>();
		this.list_OutProd.Add(0f);
		this.list_OutFurniture.Add(0f);
		this.list_OutStaff.Add(0f);
		this.list_OutExpansion.Add(0f);
		this.list_OutMarketing.Add(0f);
		this.list_OutOperational.Add(0f);
		this.list_InSales.Add(0f);
		this.list_InPrizes.Add(0f);
	}

	// Token: 0x06000285 RID: 645 RVA: 0x00015118 File Offset: 0x00013318
	public void LoadList(SaveData _data)
	{
		if (_data.finances_OutFurniture == null)
		{
			this.SetNewList();
			return;
		}
		this.list_OutProd = new List<float>(_data.finances_OutProd);
		this.list_OutFurniture = new List<float>(_data.finances_OutFurniture);
		this.list_OutStaff = new List<float>(_data.finances_OutStaff);
		this.list_OutExpansion = new List<float>(_data.finances_OutExpansion);
		this.list_OutMarketing = new List<float>(_data.finances_OutMarketing);
		if (_data.finances_OutOperational == null)
		{
			this.list_OutOperational = new List<float>(this.list_OutMarketing);
		}
		else
		{
			this.list_OutOperational = new List<float>(_data.finances_OutOperational);
		}
		this.list_InSales = new List<float>(_data.finances_InSales);
		this.list_InPrizes = new List<float>(_data.finances_InPrizes);
	}

	// Token: 0x0400033F RID: 831
	public static Finances_Manager instance;

	// Token: 0x04000340 RID: 832
	private float gameMoney;

	// Token: 0x04000341 RID: 833
	private List<float> list_OutProd = new List<float>();

	// Token: 0x04000342 RID: 834
	private List<float> list_OutFurniture = new List<float>();

	// Token: 0x04000343 RID: 835
	private List<float> list_OutStaff = new List<float>();

	// Token: 0x04000344 RID: 836
	private List<float> list_OutExpansion = new List<float>();

	// Token: 0x04000345 RID: 837
	private List<float> list_OutMarketing = new List<float>();

	// Token: 0x04000346 RID: 838
	private List<float> list_OutOperational = new List<float>();

	// Token: 0x04000347 RID: 839
	private List<float> list_InSales = new List<float>();

	// Token: 0x04000348 RID: 840
	private List<float> list_InPrizes = new List<float>();
}
