using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class Unlock_Manager : MonoBehaviour
{
	// Token: 0x060004DB RID: 1243 RVA: 0x000306C8 File Offset: 0x0002E8C8
	private void Awake()
	{
		if (!Unlock_Manager.instance)
		{
			Unlock_Manager.instance = this;
		}
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x000306DC File Offset: 0x0002E8DC
	private void Start()
	{
		for (int i = 0; i < this.cat_odds_multiplier.Length; i++)
		{
			this.cat_odds_multiplier[i] = 1;
		}
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x00030705 File Offset: 0x0002E905
	public void CreateReferences()
	{
		this.Set_NewGameItemUnlocked();
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x00030710 File Offset: 0x0002E910
	public void Set_NewGameItemUnlocked()
	{
		this.item_Unlocked[0, 0] = true;
		this.item_Unlocked[0, 1] = true;
		this.item_Unlocked[0, 3] = true;
		this.item_Unlocked[0, 4] = true;
		this.item_Unlocked[0, 6] = true;
		this.item_Unlocked[0, 9] = true;
		this.item_Unlocked[0, 10] = true;
		this.item_Unlocked[0, 182] = true;
		this.item_Unlocked[0, 180] = true;
		this.item_Unlocked[0, 141] = true;
		this.item_Unlocked[0, 77] = true;
		this.item_Unlocked[0, 64] = true;
		this.item_Unlocked[0, 69] = true;
		this.item_Unlocked[0, 192] = true;
		this.item_Unlocked[0, 196] = true;
		this.item_Unlocked[0, 69] = true;
		this.item_Unlocked[1, 0] = true;
		this.item_Unlocked[1, 1] = true;
		this.item_Unlocked[1, 4] = true;
		this.item_Unlocked[2, 0] = true;
		this.item_Unlocked[2, 1] = true;
		this.item_Unlocked[3, 0] = true;
		this.item_Unlocked[3, 1] = true;
		this.item_Unlocked[4, 0] = true;
		this.item_Unlocked[4, 1] = true;
		this.item_Unlocked[5, 0] = true;
		for (int i = 0; i < 999; i++)
		{
			this.item_Unlocked[6, i] = true;
			this.item_Unlocked[8, i] = true;
		}
		this.item_Unlocked[7, 0] = true;
		this.item_Unlocked[7, 1] = true;
		this.item_Unlocked[9, 0] = true;
		this.item_Unlocked[9, 1] = true;
		this.item_Unlocked[9, 2] = true;
		this.item_Unlocked[10, 0] = true;
		this.item_Unlocked[10, 1] = true;
		this.item_Unlocked[11, 0] = true;
		this.item_Unlocked[11, 1] = true;
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x00030954 File Offset: 0x0002EB54
	public Sprite Set_ItemUnlockState(int _cat, int _index, bool _unlocked, bool _notify, bool _get_sprite)
	{
		if (_unlocked && !this.item_Unlocked[_cat, _index])
		{
			this.item_NewlyUnlocked[_cat, _index] = _unlocked;
		}
		this.item_Unlocked[_cat, _index] = _unlocked;
		Sprite sprite = null;
		if (_notify || _get_sprite)
		{
			if (_cat == 0)
			{
				sprite = Inv_Manager.instance.GetProdSprite(_index);
			}
			else if (_cat == 1)
			{
				sprite = Inv_Manager.instance.shelfProd_Prefabs[_index].itemSprite;
			}
			else if (_cat == 2)
			{
				sprite = Inv_Manager.instance.decor_Prefabs[_index].itemSprite;
			}
			else if (_cat == 3)
			{
				sprite = Inv_Manager.instance.wallPaint_Prefabs[_index].itemSprite;
			}
			else if (_cat == 4)
			{
				sprite = Inv_Manager.instance.floor_Prefabs[_index].itemSprite;
			}
			else if (_cat == 5)
			{
				sprite = Inv_Manager.instance.util_Prefabs[_index].itemSprite;
			}
			else if (_cat == 6)
			{
				sprite = Menu_Manager.instance.locker_SkinColor_Sprites[_index];
			}
			else if (_cat == 7)
			{
				sprite = Menu_Manager.instance.locker_Outfit_Sprites[_index];
			}
			else if (_cat == 8)
			{
				sprite = Menu_Manager.instance.locker_HairColor_Sprites[_index];
			}
			else if (_cat == 9)
			{
				sprite = Menu_Manager.instance.locker_Hair_Sprites[_index];
			}
			else if (_cat == 10)
			{
				sprite = Menu_Manager.instance.locker_Eyes_Sprites[_index];
			}
			else if (_cat == 11)
			{
				sprite = Menu_Manager.instance.locker_Hats_Sprites[_index];
			}
		}
		if (_notify)
		{
			Menu_Manager.instance.SetNotification("Item Unlocked!", "It will be available for purchasing on your PC.", sprite, 10f, false);
		}
		return sprite;
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x00030AF5 File Offset: 0x0002ECF5
	public void Load_ItemsUnlockState(SaveData _data)
	{
		if (_data.item_Unlocked_SD == null)
		{
			this.Set_NewGameItemUnlocked();
			return;
		}
		this.item_Unlocked = _data.item_Unlocked_SD;
		this.item_NewlyUnlocked = _data.item_NewlyUnlocked_SD;
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x00030B20 File Offset: 0x0002ED20
	public void Set_UnlockAllItems(bool _b)
	{
		for (int i = 0; i < 12; i++)
		{
			for (int j = 0; j < this.item_Unlocked.GetLength(1); j++)
			{
				this.Set_ItemUnlockState(i, j, _b, false, false);
			}
		}
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x00030B60 File Offset: 0x0002ED60
	public int[] Set_UnlockRandomItem(bool _notify, bool _forceProds)
	{
		int[] array = new int[2];
		int num = 0;
		List<List<int>> list = new List<List<int>>();
		list.Add(new List<int>());
		list.Add(new List<int>());
		list.Add(new List<int>());
		list.Add(new List<int>());
		list.Add(new List<int>());
		list.Add(new List<int>());
		list.Add(new List<int>());
		list.Add(new List<int>());
		list.Add(new List<int>());
		list.Add(new List<int>());
		list.Add(new List<int>());
		list.Add(new List<int>());
		if (num == 0)
		{
			for (int i = 0; i < Inv_Manager.instance.prod_Prefabs.Count; i++)
			{
				if (Inv_Manager.instance.prod_Prefabs[i].buyable && !this.item_Unlocked[num, i])
				{
					list[num].Add(Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.prod_Prefabs[i].gameObject));
				}
			}
			num++;
		}
		if (num == 1)
		{
			for (int j = 0; j < Inv_Manager.instance.shelfProd_Prefabs.Count; j++)
			{
				if (Inv_Manager.instance.shelfProd_Prefabs[j].buyable && !this.item_Unlocked[num, j])
				{
					list[num].Add(Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.shelfProd_Prefabs[j].gameObject));
				}
			}
			num++;
		}
		if (num == 2)
		{
			for (int k = 0; k < Inv_Manager.instance.decor_Prefabs.Count; k++)
			{
				if (Inv_Manager.instance.decor_Prefabs[k].buyable && !this.item_Unlocked[num, k])
				{
					list[num].Add(Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.decor_Prefabs[k].gameObject));
				}
			}
			num++;
		}
		if (num == 3)
		{
			for (int l = 0; l < Inv_Manager.instance.wallPaint_Prefabs.Count; l++)
			{
				if (Inv_Manager.instance.wallPaint_Prefabs[l].buyable && !this.item_Unlocked[num, l])
				{
					list[num].Add(Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.wallPaint_Prefabs[l].gameObject));
				}
			}
			num++;
		}
		if (num == 4)
		{
			for (int m = 0; m < Inv_Manager.instance.floor_Prefabs.Count; m++)
			{
				if (Inv_Manager.instance.floor_Prefabs[m].buyable && !this.item_Unlocked[num, m])
				{
					list[num].Add(Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.floor_Prefabs[m].gameObject));
				}
			}
			num++;
		}
		if (num == 5)
		{
			for (int n = 0; n < Inv_Manager.instance.util_Prefabs.Count; n++)
			{
				if (Inv_Manager.instance.util_Prefabs[n].buyable && !this.item_Unlocked[num, n])
				{
					list[num].Add(Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.util_Prefabs[n].gameObject));
				}
			}
			num = 7;
		}
		if (num == 7)
		{
			for (int num2 = 0; num2 < Menu_Manager.instance.locker_Outfit_Sprites.Count; num2++)
			{
				if (!this.item_Unlocked[num, num2] && num2 != 6)
				{
					list[num].Add(num2);
				}
			}
			num = 9;
		}
		if (num == 9)
		{
			for (int num3 = 0; num3 < Menu_Manager.instance.locker_Hair_Sprites.Count; num3++)
			{
				if (!this.item_Unlocked[num, num3])
				{
					list[num].Add(num3);
				}
			}
			num++;
		}
		if (num == 10)
		{
			for (int num4 = 0; num4 < Menu_Manager.instance.locker_Eyes_Sprites.Count; num4++)
			{
				if (!this.item_Unlocked[num, num4])
				{
					list[num].Add(num4);
				}
			}
			num++;
		}
		if (num == 11)
		{
			for (int num5 = 0; num5 < Menu_Manager.instance.locker_Hats_Sprites.Count; num5++)
			{
				if (!this.item_Unlocked[num, num5])
				{
					list[num].Add(num5);
				}
			}
		}
		List<int> list2 = new List<int>();
		for (int num6 = 0; num6 < list.Count; num6++)
		{
			if (list[num6].Count > 0)
			{
				for (int num7 = 0; num7 < this.cat_odds_multiplier[num6]; num7++)
				{
					list2.Add(num6);
				}
			}
		}
		if (list2.Count > 0)
		{
			num = list2[UnityEngine.Random.Range(0, list2.Count)];
			if (_forceProds && list[0].Count > 0)
			{
				num = 0;
			}
			for (int num8 = 0; num8 < this.cat_odds_multiplier.Length; num8++)
			{
				int num9 = 1;
				if (num8 == 0)
				{
					num9 = 10;
				}
				if (num8 == 1)
				{
					num9 = 2;
				}
				if (num8 == 2)
				{
					num9 = 4;
				}
				if (num8 == 3)
				{
					num9 = 2;
				}
				if (num8 == 4)
				{
					num9 = 2;
				}
				this.cat_odds_multiplier[num8] += num9;
			}
			this.cat_odds_multiplier[num] = 1;
		}
		if (list[num].Count > 0)
		{
			int num10 = list[num][UnityEngine.Random.Range(0, list[num].Count)];
			this.Set_ItemUnlockState(num, num10, true, _notify, false);
			array[0] = num;
			array[1] = num10;
			return array;
		}
		return null;
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x00031110 File Offset: 0x0002F310
	public List<List<int>> Set_UnlockRandomItem(int _qnt, bool _notify, int _qntProds)
	{
		List<List<int>> list = new List<List<int>>();
		for (int i = 0; i < _qnt; i++)
		{
			bool forceProds = false;
			if (i < _qntProds)
			{
				forceProds = true;
			}
			int[] array = this.Set_UnlockRandomItem(_notify, forceProds);
			if (array == null)
			{
				return null;
			}
			list.Add(new List<int>
			{
				array[0],
				array[1]
			});
		}
		return list;
	}

	// Token: 0x0400060A RID: 1546
	public static Unlock_Manager instance;

	// Token: 0x0400060B RID: 1547
	[SerializeField]
	public bool[,] item_Unlocked = new bool[99, 999];

	// Token: 0x0400060C RID: 1548
	[SerializeField]
	public bool[,] item_NewlyUnlocked = new bool[99, 999];

	// Token: 0x0400060D RID: 1549
	public int[] cat_odds_multiplier = new int[20];
}
