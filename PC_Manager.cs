using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000050 RID: 80
public class PC_Manager : MonoBehaviour
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600041D RID: 1053 RVA: 0x000266FD File Offset: 0x000248FD
	// (set) Token: 0x0600041E RID: 1054 RVA: 0x00026705 File Offset: 0x00024905
	public List<string> shopCatNames { get; private set; } = new List<string>
	{
		"products",
		"shelves",
		"decoration",
		"wall paint",
		"floor",
		"utilities"
	};

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600041F RID: 1055 RVA: 0x0002670E File Offset: 0x0002490E
	// (set) Token: 0x06000420 RID: 1056 RVA: 0x00026716 File Offset: 0x00024916
	public List<string> eventCatNames { get; private set; } = new List<string>
	{
		"valentines",
		"easter",
		"halloween",
		"christmas"
	};

	// Token: 0x06000421 RID: 1057 RVA: 0x0002671F File Offset: 0x0002491F
	private void Awake()
	{
		if (!PC_Manager.instance)
		{
			PC_Manager.instance = this;
		}
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x00026733 File Offset: 0x00024933
	private void Start()
	{
		this.CreateReferences_Shop();
		this.CreateReferences_Inv();
		this.CreateReferences_Finances();
		this.CreateReferences_Staff();
		this.CreateReferences_LocalCustomersTab();
		this.CreateReferences_Expand();
		this.CreateReferences_Mail();
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x0002675F File Offset: 0x0002495F
	private void Update()
	{
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x00026764 File Offset: 0x00024964
	private void Set_Buttons_MenuNav_Connections(int _gridSize, List<Button> _list)
	{
		List<GameObject> list = new List<GameObject>();
		foreach (Button button in _list)
		{
			list.Add(button.gameObject);
		}
		Debug.Log("First step = " + list.Count.ToString());
		this.Set_Buttons_MenuNav_Connections(_gridSize, list);
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x000267E4 File Offset: 0x000249E4
	private void Set_Buttons_MenuNav_Connections(int _gridSize, List<GameObject> _list)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < _list.Count; i++)
		{
			if (_list[i].activeSelf)
			{
				list.Add(_list[i]);
			}
		}
		Debug.Log("Second step = " + list.Count.ToString());
		for (int j = 0; j < list.Count; j++)
		{
			list[j].GetComponent<MenuNav_Controller>().ResetButtons();
			if (j >= _gridSize)
			{
				list[j].GetComponent<MenuNav_Controller>().nav_Up = list[j - _gridSize];
			}
			if (j > 0)
			{
				list[j].GetComponent<MenuNav_Controller>().nav_Left = list[j - 1];
			}
			if (j < list.Count - 1)
			{
				list[j].GetComponent<MenuNav_Controller>().nav_Right = list[j + 1];
			}
			if (j < list.Count - _gridSize)
			{
				list[j].GetComponent<MenuNav_Controller>().nav_Down = list[j + _gridSize];
			}
		}
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x000268F0 File Offset: 0x00024AF0
	private void CreateReferences_Shop()
	{
		for (int i = 0; i < 250; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.shop_Button_Prefab);
			gameObject.transform.SetParent(this.shop_Button_Prefab.transform.parent);
			gameObject.transform.localScale = Vector3.one;
			gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
			this.shop_Button_GOs.Add(gameObject);
			this.shop_Button_Buttons.Add(gameObject.GetComponent<Button>());
			this.shop_Button_GOs_Image.Add(gameObject.GetComponent<Image>());
			this.shop_Button_ItemNamePanel.Add(gameObject.transform.Find("Panel_Name").GetComponent<Image>());
			this.shop_Button_ItemText.Add(gameObject.transform.Find("Panel_Name").Find("Text_Name").GetComponent<Text>());
			this.shop_Button_ItemImage.Add(gameObject.transform.Find("Image_Item").GetComponent<Image>());
			this.shop_Button_ItemPrice.Add(gameObject.transform.Find("Panel_Price").Find("Text_Price").GetComponent<Text>());
			this.shop_Button_NeedsRefrigerator.Add(gameObject.transform.Find("Image_NeedsRefrigerator").GetComponent<Image>());
			int _index = i;
			gameObject.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.SelectShopItem(_index);
			});
		}
		this.shop_Button_Prefab.SetActive(false);
		for (int j = 0; j < Inv_Manager.instance.shop_Deliv_Qnt; j++)
		{
			Image image = UnityEngine.Object.Instantiate<Image>(this.shop_Deliv_Prefab);
			image.transform.SetParent(this.shop_Deliv_Prefab.transform.parent);
			image.transform.localScale = Vector3.one;
			image.GetComponent<RectTransform>().localPosition = Vector3.zero;
			this.shop_Deliv_ItemPanel.Add(image.GetComponent<Image>());
			this.shop_Deliv_ItemImage.Add(image.transform.Find("Image_Item").GetComponent<Image>());
			this.shop_Deliv_QntImage.Add(image.transform.Find("Panel_Qnt").GetComponent<Image>());
			this.shop_Deliv_QntText.Add(image.transform.Find("Panel_Qnt").Find("Text_Qnt").GetComponent<Text>());
			this.shop_Deliv_SupPanel.Add(image.transform.Find("Panel_Sup").GetComponent<Image>());
			this.shop_Deliv_SupImage.Add(image.transform.Find("Panel_Sup").Find("Image").GetComponent<Image>());
			this.shop_Deliv_SupText.Add(image.transform.Find("Panel_Sup").Find("Text").GetComponent<Text>());
			this.shop_Deliv_ItemImage[j].gameObject.SetActive(false);
			this.shop_Deliv_QntImage[j].gameObject.SetActive(false);
			image.color = this.shop_Deliv_NoColor;
		}
		this.shop_Deliv_Prefab.gameObject.SetActive(false);
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x00026C20 File Offset: 0x00024E20
	private void CreateReferences_Inv()
	{
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x00026C24 File Offset: 0x00024E24
	private void CreateReferences_Finances()
	{
		for (int i = 0; i < 8; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.fin_PanelDataPrefab);
			gameObject.transform.SetParent(this.fin_PanelDataPrefab.transform.parent);
			gameObject.transform.localScale = Vector3.one;
			gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
			this.fin_GOs.Add(gameObject);
			this.fin_TextTitle.Add(gameObject.transform.Find("Panel_Title").Find("Text").GetComponent<Text>());
			this.fin_TextProds.Add(gameObject.transform.Find("Panel_Prods").Find("Text").GetComponent<Text>());
			this.fin_TextFurniture.Add(gameObject.transform.Find("Panel_Furniture").Find("Text").GetComponent<Text>());
			this.fin_TextStaff.Add(gameObject.transform.Find("Panel_Staff").Find("Text").GetComponent<Text>());
			this.fin_TextOperational.Add(gameObject.transform.Find("Panel_Operational").Find("Text").GetComponent<Text>());
			this.fin_TextExpansion.Add(gameObject.transform.Find("Panel_Expansion").Find("Text").GetComponent<Text>());
			this.fin_TextMarketing.Add(gameObject.transform.Find("Panel_Marketing").Find("Text").GetComponent<Text>());
			this.fin_TextSales.Add(gameObject.transform.Find("Panel_Sales").Find("Text").GetComponent<Text>());
			this.fin_TextPrizes.Add(gameObject.transform.Find("Panel_Prizes").Find("Text").GetComponent<Text>());
			this.fin_TextBalance.Add(gameObject.transform.Find("Panel_Total").Find("Text").GetComponent<Text>());
		}
		this.fin_PanelDataPrefab.SetActive(false);
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x00026E48 File Offset: 0x00025048
	public void SetTab(int _tab)
	{
		this.tabIndexSelected = _tab;
		this.lastTabSelected = _tab;
		this.UpdateTab();
		if (Player_Manager.instance.playerControllerList.Count > 1)
		{
			this.pc_background_frame.SetActive(true);
		}
		else
		{
			this.pc_background_frame.SetActive(true);
		}
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x00026EA9 File Offset: 0x000250A9
	public int GetTab()
	{
		return this.tabIndexSelected;
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x00026EB4 File Offset: 0x000250B4
	public void NavTab(int _nav)
	{
		this.tabIndexSelected += _nav;
		if (this.tabIndexSelected < 0)
		{
			this.tabIndexSelected = this.tab_GOs.Count - 1;
		}
		else if (this.tabIndexSelected >= this.tab_GOs.Count)
		{
			this.tabIndexSelected = 0;
		}
		if (!this.tab_GOs[this.tabIndexSelected].active)
		{
			this.NavTab(_nav);
			return;
		}
		this.SetTab(this.tabIndexSelected);
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x00026F34 File Offset: 0x00025134
	public void UpdateTab()
	{
		this.tab_NameText.text = Language_Manager.instance.GetText(this.tabNames[this.tabIndexSelected]);
		for (int i = 0; i < this.tab_Panels.Count; i++)
		{
			if (this.tabIndexSelected == i)
			{
				this.tab_Panels[i].SetActive(true);
			}
			else
			{
				this.tab_Panels[i].SetActive(false);
			}
		}
		if (this.tabIndexSelected == 0)
		{
			Menu_Manager.instance.SetSelector(null, false);
		}
		if (this.tabIndexSelected == 1)
		{
			this.SetShopCat(0);
			this.RefreshTomorrowDelivery();
			this.SelectShopSupIndex(0);
			this.eventCat_Image.sprite = this.eventCat_Sprites[World_Manager.instance.GetSeasonIndex()];
		}
		else if (this.tabIndexSelected != 2)
		{
			if (this.tabIndexSelected == 3)
			{
				this.RefreshFinancesTab();
				Menu_Manager.instance.SetSelector(null, false);
			}
			else if (this.tabIndexSelected == 4)
			{
				this.RefreshStaffTab();
				if (Char_Manager.instance.staff_Data.Count > 0)
				{
					Menu_Manager.instance.SetSelector(this.staff_Button_GiveDaysOff[0].GetComponent<Button>(), false);
				}
				else
				{
					Menu_Manager.instance.SetSelector(this.staff_Button_GetApplicants, false);
				}
			}
			else if (this.tabIndexSelected == 5)
			{
				this.RefreshLocalCustomerTab();
				Menu_Manager.instance.SetSelector(this.cust_Buttons[0], false);
			}
			else if (this.tabIndexSelected == 7)
			{
				this.RefreshExpandTab();
				Invoker.InvokeDelayed(new Invokable(this.RefreshExpandTab), 0.1f);
				Menu_Manager.instance.SetSelector(this.news_Expand_Button, false);
			}
			else if (this.tabIndexSelected == 8)
			{
				this.Mail_Refresh();
				if (this.mail_class_list[0].button != null)
				{
					Menu_Manager.instance.SetSelector(this.mail_class_list[0].button, false);
				}
			}
			else
			{
				Menu_Manager.instance.SetSelector(null, false);
			}
		}
		for (int j = 0; j < this.tab_GOs.Count; j++)
		{
			int num = 0;
			if (j == this.tabIndexSelected)
			{
				num = 1;
			}
			this.tab_GOs[j].GetComponent<Image>().color = this.pC_Tabs_Colors[num];
		}
		this.pC_TabSelected_Image.sprite = this.tab_GOs[this.tabIndexSelected].transform.Find("Image").GetComponent<Image>().sprite;
		this.pC_TabSelected_Image.SetNativeSize();
		this.RefreshHints();
		Input_Manager.instance.RefreshInputHints();
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x000271D0 File Offset: 0x000253D0
	public void SetShopCat(int _index)
	{
		this.shopCatIndexSelected = _index;
		this.UpdateShopCat();
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x000271F3 File Offset: 0x000253F3
	public void SetProdCat(int _index)
	{
		this.shopCatIndexSelected = 0;
		this.prodCatIndexSelected = _index;
		this.UpdateShopCat();
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x0002721D File Offset: 0x0002541D
	public void SetSeasonalCat()
	{
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x0002721F File Offset: 0x0002541F
	public int GetShopCat()
	{
		return this.shopCatIndexSelected;
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x00027228 File Offset: 0x00025428
	public void NavShopCat(int _nav)
	{
		if (this.tabIndexSelected != 1)
		{
			return;
		}
		this.shopIndexJoystick += _nav;
		if (this.shopIndexJoystick < 0)
		{
			this.shopIndexJoystick = this.shopcatavailable.Length - 1;
		}
		else if (this.shopIndexJoystick >= this.shopcatavailable.Length)
		{
			this.shopIndexJoystick = 0;
		}
		if (this.shopcatavailable[this.shopIndexJoystick] < 1f)
		{
			this.shopCatIndexSelected = 0;
			this.prodCatIndexSelected = Mathf.RoundToInt(this.shopcatavailable[this.shopIndexJoystick] * 10f);
		}
		else
		{
			this.shopCatIndexSelected = Mathf.RoundToInt(this.shopcatavailable[this.shopIndexJoystick]);
		}
		this.UpdateShopCat();
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x000272EC File Offset: 0x000254EC
	public void UpdateShopCat()
	{
		Transform transform = this.shopCat_GOs[this.shopCatIndexSelected].transform;
		string text = Language_Manager.instance.GetText(this.shopCatNames[this.shopCatIndexSelected]);
		if (this.shopCatIndexSelected == 0)
		{
			transform = this.prodCat_GOs[this.prodCatIndexSelected].transform;
			text = Language_Manager.instance.GetText(this.prodCatNames[this.prodCatIndexSelected]);
			if (this.prodCatIndexSelected == 7)
			{
				text = Language_Manager.instance.GetText(this.eventCatNames[World_Manager.instance.GetSeasonIndex()]);
			}
		}
		this.shopCat_NamePanel.gameObject.SetActive(true);
		this.shopCat_NamePanel.transform.SetParent(transform);
		this.shopCat_NamePanel.transform.localPosition = Vector3.zero;
		this.shopCat_NamePanel.transform.localRotation = Quaternion.Euler(Vector3.zero);
		this.shopCat_NamePanel.transform.localScale = Vector3.one;
		this.shopCat_NameText.text = text;
		for (int i = 0; i < this.shopCat_GOs.Count; i++)
		{
			if (this.shopCat_GOs[i])
			{
				if (i == this.shopCatIndexSelected)
				{
					this.shopCat_GOs[i].GetComponentInChildren<Image>().color = this.shopCat_Colors[1];
				}
				else
				{
					this.shopCat_GOs[i].GetComponentInChildren<Image>().color = this.shopCat_Colors[0];
				}
			}
		}
		if (this.shopCatIndexSelected == 0)
		{
			for (int j = 0; j < this.shopCat_GOs.Count; j++)
			{
				if (this.shopCat_GOs[j])
				{
					this.shopCat_GOs[j].GetComponentInChildren<Image>().color = this.shopCat_Colors[0];
				}
			}
			for (int k = 0; k < this.prodCat_GOs.Count; k++)
			{
				if (this.prodCat_GOs[k])
				{
					if (k == this.prodCatIndexSelected)
					{
						this.prodCat_GOs[k].GetComponentInChildren<Image>().color = this.shopCat_Colors[1];
					}
					else
					{
						this.prodCat_GOs[k].GetComponentInChildren<Image>().color = this.shopCat_Colors[0];
					}
				}
			}
		}
		else
		{
			for (int l = 0; l < this.prodCat_GOs.Count; l++)
			{
				if (this.prodCat_GOs[l])
				{
					this.prodCat_GOs[l].GetComponentInChildren<Image>().color = this.shopCat_Colors[0];
				}
			}
		}
		this.prodCatTypes.Clear();
		for (int m = 0; m < this.prodCatNames.Count; m++)
		{
			this.prodCatTypes.Add(new List<Inv_Manager.ProdType>());
		}
		for (int n = 0; n < Enum.GetNames(typeof(Inv_Manager.ProdType)).Length; n++)
		{
			this.prodCatTypes[0].Add((Inv_Manager.ProdType)n);
		}
		this.prodCatTypes[0].Remove(Inv_Manager.ProdType.Valentines);
		this.prodCatTypes[0].Remove(Inv_Manager.ProdType.Christmas);
		this.prodCatTypes[0].Remove(Inv_Manager.ProdType.Halloween);
		this.prodCatTypes[0].Remove(Inv_Manager.ProdType.Easter);
		this.prodCatTypes[1].Add(Inv_Manager.ProdType.Fruit);
		this.prodCatTypes[1].Add(Inv_Manager.ProdType.Vegetable);
		this.prodCatTypes[2].Add(Inv_Manager.ProdType.Food);
		this.prodCatTypes[2].Add(Inv_Manager.ProdType.Meat);
		this.prodCatTypes[2].Add(Inv_Manager.ProdType.Liquids);
		this.prodCatTypes[3].Add(Inv_Manager.ProdType.Tools);
		this.prodCatTypes[4].Add(Inv_Manager.ProdType.Hygiene);
		this.prodCatTypes[5].Add(Inv_Manager.ProdType.Electronics);
		this.prodCatTypes[6].Add(Inv_Manager.ProdType.Sport);
		if (World_Manager.instance.GetSeasonIndex() == 0)
		{
			this.prodCatTypes[7].Add(Inv_Manager.ProdType.Valentines);
		}
		if (World_Manager.instance.GetSeasonIndex() == 1)
		{
			this.prodCatTypes[7].Add(Inv_Manager.ProdType.Easter);
		}
		if (World_Manager.instance.GetSeasonIndex() == 2)
		{
			this.prodCatTypes[7].Add(Inv_Manager.ProdType.Halloween);
		}
		if (World_Manager.instance.GetSeasonIndex() == 3)
		{
			this.prodCatTypes[7].Add(Inv_Manager.ProdType.Christmas);
		}
		int num = this.RefreshShopItems();
		this.shopIndexSelected = -1;
		int num2 = this.ShopItems_GetFirstUnlockedButtonIndex();
		if (num2 == -1)
		{
			num2 = num;
		}
		this.SelectShopItem(num2);
		Menu_Manager.instance.SetSelector(this.shop_Button_GOs[num2].GetComponent<Button>(), false);
		this.shop_scroll_rect_auto_scroll._Start();
		this.RefreshHints();
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x000277DC File Offset: 0x000259DC
	public bool Check_IfProdBelongsToProdCat(Inv_Manager.ProdType _type)
	{
		if (this.prodCatIndexSelected == 0)
		{
			return true;
		}
		List<Inv_Manager.ProdType> list = new List<Inv_Manager.ProdType>();
		if (this.prodCatIndexSelected == 1)
		{
			list.Add(Inv_Manager.ProdType.Fruit);
			list.Add(Inv_Manager.ProdType.Vegetable);
		}
		else if (this.prodCatIndexSelected == 2)
		{
			list.Add(Inv_Manager.ProdType.Food);
			list.Add(Inv_Manager.ProdType.Liquids);
			list.Add(Inv_Manager.ProdType.Meat);
		}
		else if (this.prodCatIndexSelected == 3)
		{
			list.Add(Inv_Manager.ProdType.Tools);
		}
		else if (this.prodCatIndexSelected == 4)
		{
			list.Add(Inv_Manager.ProdType.Hygiene);
		}
		else if (this.prodCatIndexSelected == 5)
		{
			list.Add(Inv_Manager.ProdType.Electronics);
		}
		else if (this.prodCatIndexSelected == 6)
		{
			list.Add(Inv_Manager.ProdType.Sport);
		}
		return false;
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x0002787C File Offset: 0x00025A7C
	private int RefreshShopItems()
	{
		if (this.shopCatIndexSelected == 0)
		{
			for (int i = 0; i < this.shop_Button_GOs.Count; i++)
			{
				bool flag = true;
				bool flag2 = true;
				if (this.prodCatIndexSelected != 0)
				{
					flag2 = false;
				}
				if (i < Inv_Manager.instance.prod_Prefabs.Count)
				{
					Inv_Manager.ProdType prodType = Inv_Manager.instance.prod_Prefabs[i].prodType;
					bool flag3 = false;
					if (prodType == Inv_Manager.ProdType.Valentines)
					{
						flag3 = true;
					}
					if (prodType == Inv_Manager.ProdType.Easter)
					{
						flag3 = true;
					}
					if (prodType == Inv_Manager.ProdType.Halloween)
					{
						flag3 = true;
					}
					if (prodType == Inv_Manager.ProdType.Christmas)
					{
						flag3 = true;
					}
					if (Inv_Manager.instance.prod_Prefabs[i].buyable && ((flag2 && !flag3) || this.prodCatTypes[this.prodCatIndexSelected].Contains(Inv_Manager.instance.prod_Prefabs[i].prodType)))
					{
						flag = false;
						this.shop_Button_GOs[i].SetActive(true);
						int itemIndex = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.prod_Prefabs[i].gameObject);
						this.shop_Button_ItemText[i].text = Inv_Manager.instance.GetProdName(itemIndex, true);
						this.shop_Button_ItemNamePanel[i].color = Inv_Manager.instance.GetProdPrefab(itemIndex).prodColors[0];
						this.shop_Button_GOs_Image[i].color = this.shop_UnlockedColors[1];
						this.shop_Button_ItemImage[i].gameObject.SetActive(true);
						this.shop_Button_ItemImage[i].sprite = Inv_Manager.instance.GetProdSprite(itemIndex);
						this.shop_Button_ItemImage[i].SetNativeSize();
						this.shop_Button_ItemPrice[i].text = Mathf.FloorToInt(Inv_Manager.instance.GetBoxPrice(itemIndex) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected]).ToString();
						bool active = false;
						if (Inv_Manager.instance.GetProdPrefab(itemIndex).needsRefrigerator)
						{
							active = true;
						}
						this.shop_Button_NeedsRefrigerator[i].gameObject.SetActive(active);
						if (Unlock_Manager.instance.item_Unlocked[this.shopCatIndexSelected, itemIndex])
						{
							this.shop_Button_ItemImage[i].color = this.shop_UnlockedColors[1];
							this.shop_Button_ItemPrice[i].transform.parent.gameObject.SetActive(true);
						}
						else
						{
							flag = false;
							this.shop_Button_ItemImage[i].color = this.shop_UnlockedColors[0];
							this.shop_Button_ItemNamePanel[i].color = this.shop_UnlockedColors[3];
							this.shop_Button_GOs_Image[i].color = this.shop_UnlockedColors[2];
							this.shop_Button_ItemText[i].text = "";
							this.shop_Button_NeedsRefrigerator[i].gameObject.SetActive(false);
							this.shop_Button_ItemPrice[i].transform.parent.gameObject.SetActive(false);
						}
					}
					else
					{
						flag = true;
					}
				}
				if (flag)
				{
					this.shop_Button_GOs[i].SetActive(false);
				}
			}
		}
		else if (this.shopCatIndexSelected == 1)
		{
			for (int j = 0; j < this.shop_Button_GOs.Count; j++)
			{
				bool flag4 = true;
				if (j < Inv_Manager.instance.shelfProd_Prefabs.Count)
				{
					if (Inv_Manager.instance.shelfProd_Prefabs[j].buyable)
					{
						flag4 = false;
						this.shop_Button_GOs[j].SetActive(true);
						int itemIndex2 = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.shelfProd_Prefabs[j].gameObject);
						this.shop_Button_ItemText[j].text = Inv_Manager.instance.GetItemName(Inv_Manager.instance.shelfProd_Prefabs[j].gameObject, true);
						this.shop_Button_ItemNamePanel[j].color = Inv_Manager.instance.GetItemShelf(itemIndex2).itemColor[0];
						this.shop_Button_GOs_Image[j].color = this.shop_UnlockedColors[1];
						this.shop_Button_ItemImage[j].gameObject.SetActive(true);
						this.shop_Button_ItemImage[j].sprite = Inv_Manager.instance.GetItemShelf(itemIndex2).itemSprite;
						this.shop_Button_ItemImage[j].SetNativeSize();
						this.shop_Button_ItemPrice[j].text = Mathf.FloorToInt(Inv_Manager.instance.GetShelfBoxPrice(itemIndex2) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected]).ToString();
						bool active2 = false;
						if (Inv_Manager.instance.GetItemShelf(itemIndex2).isRefrigerated)
						{
							active2 = true;
						}
						this.shop_Button_NeedsRefrigerator[j].gameObject.SetActive(active2);
						bool flag5 = false;
						this.shop_Button_ItemPrice[j].gameObject.SetActive(!flag5);
						if (Unlock_Manager.instance.item_Unlocked[this.shopCatIndexSelected, itemIndex2])
						{
							this.shop_Button_ItemImage[j].color = this.shop_UnlockedColors[1];
							this.shop_Button_ItemPrice[j].transform.parent.gameObject.SetActive(true);
						}
						else
						{
							flag4 = false;
							this.shop_Button_ItemImage[j].color = this.shop_UnlockedColors[0];
							this.shop_Button_ItemNamePanel[j].color = this.shop_UnlockedColors[3];
							this.shop_Button_GOs_Image[j].color = this.shop_UnlockedColors[2];
							this.shop_Button_ItemText[j].text = "";
							this.shop_Button_NeedsRefrigerator[j].gameObject.SetActive(false);
							this.shop_Button_ItemPrice[j].transform.parent.gameObject.SetActive(false);
						}
					}
					else
					{
						flag4 = true;
					}
				}
				else
				{
					this.shop_Button_GOs[j].SetActive(false);
				}
				if (flag4)
				{
					this.shop_Button_GOs[j].SetActive(false);
				}
			}
		}
		else if (this.shopCatIndexSelected == 2)
		{
			for (int k = 0; k < this.shop_Button_GOs.Count; k++)
			{
				bool flag6 = true;
				if (k < Inv_Manager.instance.decor_Prefabs.Count)
				{
					if (Inv_Manager.instance.decor_Prefabs[k].buyable)
					{
						flag6 = false;
						this.shop_Button_GOs[k].SetActive(true);
						int itemIndex3 = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.decor_Prefabs[k].gameObject);
						this.shop_Button_ItemText[k].text = Inv_Manager.instance.GetItemName(Inv_Manager.instance.decor_Prefabs[k].gameObject, true);
						this.shop_Button_ItemNamePanel[k].color = Inv_Manager.instance.GetItemDecor(itemIndex3).itemColor[0];
						this.shop_Button_GOs_Image[k].color = this.shop_UnlockedColors[1];
						this.shop_Button_ItemImage[k].gameObject.SetActive(true);
						this.shop_Button_ItemImage[k].sprite = Inv_Manager.instance.GetItemDecor(itemIndex3).itemSprite;
						this.shop_Button_ItemImage[k].SetNativeSize();
						this.shop_Button_ItemPrice[k].text = Mathf.FloorToInt(Inv_Manager.instance.GetDecorBoxPrice(itemIndex3) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected]).ToString();
						this.shop_Button_NeedsRefrigerator[k].gameObject.SetActive(false);
						this.shop_Button_ItemPrice[k].gameObject.SetActive(true);
						if (Unlock_Manager.instance.item_Unlocked[this.shopCatIndexSelected, itemIndex3])
						{
							this.shop_Button_ItemImage[k].color = this.shop_UnlockedColors[1];
							this.shop_Button_ItemPrice[k].transform.parent.gameObject.SetActive(true);
						}
						else
						{
							flag6 = false;
							this.shop_Button_ItemImage[k].color = this.shop_UnlockedColors[0];
							this.shop_Button_ItemNamePanel[k].color = this.shop_UnlockedColors[3];
							this.shop_Button_GOs_Image[k].color = this.shop_UnlockedColors[2];
							this.shop_Button_ItemText[k].text = "";
							this.shop_Button_NeedsRefrigerator[k].gameObject.SetActive(false);
							this.shop_Button_ItemPrice[k].transform.parent.gameObject.SetActive(false);
						}
					}
					else
					{
						flag6 = true;
					}
				}
				else
				{
					this.shop_Button_GOs[k].SetActive(false);
				}
				if (flag6)
				{
					this.shop_Button_GOs[k].SetActive(false);
				}
			}
		}
		else if (this.shopCatIndexSelected == 3)
		{
			for (int l = 0; l < this.shop_Button_GOs.Count; l++)
			{
				bool flag7 = true;
				if (l < Inv_Manager.instance.wallPaint_Prefabs.Count)
				{
					if (Inv_Manager.instance.wallPaint_Prefabs[l].buyable)
					{
						flag7 = false;
						this.shop_Button_GOs[l].SetActive(true);
						int itemIndex4 = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.wallPaint_Prefabs[l].gameObject);
						this.shop_Button_ItemText[l].text = Inv_Manager.instance.GetItemName(Inv_Manager.instance.wallPaint_Prefabs[l].gameObject, true);
						this.shop_Button_ItemNamePanel[l].color = Inv_Manager.instance.GetItemWallPaint(itemIndex4).itemColor[0];
						this.shop_Button_GOs_Image[l].color = this.shop_UnlockedColors[1];
						this.shop_Button_ItemImage[l].gameObject.SetActive(true);
						this.shop_Button_ItemImage[l].sprite = Inv_Manager.instance.GetItemWallPaint(itemIndex4).itemSprite;
						this.shop_Button_ItemImage[l].SetNativeSize();
						this.shop_Button_ItemPrice[l].text = Mathf.FloorToInt(Inv_Manager.instance.GetWallPaintBoxPrice(itemIndex4) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected]).ToString();
						this.shop_Button_NeedsRefrigerator[l].gameObject.SetActive(false);
						this.shop_Button_ItemPrice[l].gameObject.SetActive(true);
						if (Unlock_Manager.instance.item_Unlocked[this.shopCatIndexSelected, itemIndex4])
						{
							this.shop_Button_ItemImage[l].color = this.shop_UnlockedColors[1];
							this.shop_Button_ItemPrice[l].transform.parent.gameObject.SetActive(true);
						}
						else
						{
							flag7 = false;
							this.shop_Button_ItemImage[l].color = this.shop_UnlockedColors[0];
							this.shop_Button_ItemNamePanel[l].color = this.shop_UnlockedColors[3];
							this.shop_Button_GOs_Image[l].color = this.shop_UnlockedColors[2];
							this.shop_Button_ItemText[l].text = "";
							this.shop_Button_NeedsRefrigerator[l].gameObject.SetActive(false);
							this.shop_Button_ItemPrice[l].transform.parent.gameObject.SetActive(false);
						}
					}
					else
					{
						flag7 = true;
					}
				}
				else
				{
					this.shop_Button_GOs[l].SetActive(false);
				}
				if (flag7)
				{
					this.shop_Button_GOs[l].SetActive(false);
				}
			}
		}
		else if (this.shopCatIndexSelected == 4)
		{
			for (int m = 0; m < this.shop_Button_GOs.Count; m++)
			{
				bool flag8 = true;
				if (m < Inv_Manager.instance.floor_Prefabs.Count)
				{
					if (Inv_Manager.instance.floor_Prefabs[m].buyable)
					{
						flag8 = false;
						this.shop_Button_GOs[m].SetActive(true);
						int itemIndex5 = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.floor_Prefabs[m].gameObject);
						this.shop_Button_ItemText[m].text = Inv_Manager.instance.GetItemName(Inv_Manager.instance.floor_Prefabs[m].gameObject, true);
						this.shop_Button_ItemNamePanel[m].color = Inv_Manager.instance.GetItemFloor(itemIndex5).itemColor[0];
						this.shop_Button_GOs_Image[m].color = this.shop_UnlockedColors[1];
						this.shop_Button_ItemImage[m].gameObject.SetActive(true);
						this.shop_Button_ItemImage[m].sprite = Inv_Manager.instance.GetItemFloor(itemIndex5).itemSprite;
						this.shop_Button_ItemImage[m].SetNativeSize();
						this.shop_Button_ItemPrice[m].text = Mathf.FloorToInt(Inv_Manager.instance.GetFloorBoxPrice(itemIndex5) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected]).ToString();
						this.shop_Button_NeedsRefrigerator[m].gameObject.SetActive(false);
						bool flag9 = false;
						this.shop_Button_ItemPrice[m].gameObject.SetActive(!flag9);
						if (Unlock_Manager.instance.item_Unlocked[this.shopCatIndexSelected, itemIndex5])
						{
							this.shop_Button_ItemImage[m].color = this.shop_UnlockedColors[1];
							this.shop_Button_ItemPrice[m].transform.parent.gameObject.SetActive(true);
						}
						else
						{
							flag8 = false;
							this.shop_Button_ItemImage[m].color = this.shop_UnlockedColors[0];
							this.shop_Button_ItemNamePanel[m].color = this.shop_UnlockedColors[3];
							this.shop_Button_GOs_Image[m].color = this.shop_UnlockedColors[2];
							this.shop_Button_ItemText[m].text = "";
							this.shop_Button_NeedsRefrigerator[m].gameObject.SetActive(false);
							this.shop_Button_ItemPrice[m].transform.parent.gameObject.SetActive(false);
						}
					}
					else
					{
						flag8 = true;
					}
				}
				else
				{
					this.shop_Button_GOs[m].SetActive(false);
				}
				if (flag8)
				{
					this.shop_Button_GOs[m].SetActive(false);
				}
			}
		}
		else if (this.shopCatIndexSelected == 5)
		{
			for (int n = 0; n < this.shop_Button_GOs.Count; n++)
			{
				bool flag10 = true;
				if (n < Inv_Manager.instance.util_Prefabs.Count)
				{
					if (Inv_Manager.instance.util_Prefabs[n].buyable)
					{
						flag10 = false;
						this.shop_Button_GOs[n].SetActive(true);
						int itemIndex6 = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.util_Prefabs[n].gameObject);
						this.shop_Button_ItemText[n].text = Inv_Manager.instance.GetItemName(Inv_Manager.instance.util_Prefabs[n].gameObject, true);
						this.shop_Button_GOs_Image[n].color = this.shop_UnlockedColors[1];
						this.shop_Button_ItemNamePanel[n].color = Inv_Manager.instance.GetItemUtil(itemIndex6).itemColor[0];
						this.shop_Button_ItemImage[n].gameObject.SetActive(true);
						this.shop_Button_ItemImage[n].sprite = Inv_Manager.instance.GetItemUtil(itemIndex6).itemSprite;
						this.shop_Button_ItemImage[n].SetNativeSize();
						this.shop_Button_ItemPrice[n].text = Mathf.FloorToInt(Inv_Manager.instance.GetUtilBoxPrice(itemIndex6) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected]).ToString();
						this.shop_Button_NeedsRefrigerator[n].gameObject.SetActive(false);
						bool flag11 = false;
						this.shop_Button_ItemPrice[n].gameObject.SetActive(!flag11);
						if (Unlock_Manager.instance.item_Unlocked[this.shopCatIndexSelected, itemIndex6])
						{
							this.shop_Button_ItemImage[n].color = this.shop_UnlockedColors[1];
							this.shop_Button_ItemPrice[n].transform.parent.gameObject.SetActive(true);
						}
						else
						{
							flag10 = false;
							this.shop_Button_ItemImage[n].color = this.shop_UnlockedColors[0];
							this.shop_Button_ItemNamePanel[n].color = this.shop_UnlockedColors[3];
							this.shop_Button_GOs_Image[n].color = this.shop_UnlockedColors[2];
							this.shop_Button_ItemText[n].text = "";
							this.shop_Button_NeedsRefrigerator[n].gameObject.SetActive(false);
							this.shop_Button_ItemPrice[n].transform.parent.gameObject.SetActive(false);
						}
					}
					else
					{
						flag10 = true;
					}
				}
				else
				{
					this.shop_Button_GOs[n].SetActive(false);
				}
				if (flag10)
				{
					this.shop_Button_GOs[n].SetActive(false);
				}
			}
		}
		Transform parent = this.shop_Button_GOs[0].transform.parent;
		int num = -1;
		int num2 = 0;
		List<GameObject> list = new List<GameObject>();
		List<GameObject> list2 = new List<GameObject>();
		for (int num3 = 0; num3 < this.shop_Button_GOs.Count; num3++)
		{
			if (this.shop_Button_GOs_Image[num3].color == this.shop_UnlockedColors[2])
			{
				this.shop_Button_GOs[num3].transform.SetAsLastSibling();
				list.Add(this.shop_Button_GOs[num3]);
			}
			else
			{
				if (num == -1)
				{
					num = num3;
				}
				this.shop_Button_GOs[num3].transform.SetSiblingIndex(num2);
				num2++;
				list2.Add(this.shop_Button_GOs[num3]);
			}
		}
		list2.AddRange(list);
		this.Set_Buttons_MenuNav_Connections(6, list2);
		return num;
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x00028C40 File Offset: 0x00026E40
	public int ShopItems_GetFirstUnlockedButtonIndex()
	{
		for (int i = 0; i < this.shop_Button_GOs.Count; i++)
		{
			if (this.shop_Button_GOs[i].activeSelf && this.shop_Button_GOs_Image[i].color == this.shop_UnlockedColors[1])
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x00028CA0 File Offset: 0x00026EA0
	public int ShopItems_GetFirstActiveButtonIndex()
	{
		int num = 0;
		using (List<GameObject>.Enumerator enumerator = this.shop_Button_GOs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.activeSelf)
				{
					break;
				}
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x00028CFC File Offset: 0x00026EFC
	public void SelectShopItem(int _index)
	{
		this.shop_selected_index = _index;
		if (_index == -1)
		{
			this.RefreshShopDescription(false);
			return;
		}
		if (this.shop_Button_GOs_Image[_index].color == this.shop_UnlockedColors[2])
		{
			this.RefreshShopDescription(false);
			return;
		}
		if (!this.shop_Button_GOs[_index].activeSelf)
		{
			_index = this.ShopItems_GetFirstActiveButtonIndex();
		}
		this.shopIndexSelected = _index;
		this.RefreshShopDescription(true);
		if (Input_Manager.instance.GetScheme(-1) != "Joystick")
		{
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
		}
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x00028D9C File Offset: 0x00026F9C
	private void RefreshShopDescription(bool _bool = true)
	{
		this.RefreshShopSup();
		if (!_bool)
		{
			this.shop_Desc_Panel.gameObject.SetActive(false);
			this.shop_Desc_Panel_Off.gameObject.SetActive(true);
			this.shop_may_buy = false;
			return;
		}
		this.shop_Desc_Panel.gameObject.SetActive(true);
		this.shop_Desc_Panel_Off.gameObject.SetActive(false);
		this.shop_may_buy = true;
		if (this.shopCatIndexSelected == 0)
		{
			int itemIndex = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.prod_Prefabs[this.shopIndexSelected].gameObject);
			this.shop_Desc_Panel.color = Inv_Manager.instance.GetProdPrefab(itemIndex).prodColors[0];
			this.shop_Desc_ItemName.text = Inv_Manager.instance.GetProdName(itemIndex, true);
			this.shop_Desc_ItemImage.sprite = Inv_Manager.instance.GetProdSprite(itemIndex);
			this.shop_Desc_ItemImage.SetNativeSize();
			this.shop_Desc_Price.text = Mathf.FloorToInt(Inv_Manager.instance.GetBoxPrice(itemIndex) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected]).ToString();
			if (Finances_Manager.instance.GetMoney() < Inv_Manager.instance.GetBoxPrice(itemIndex))
			{
				this.shop_Desc_NoMoney.SetActive(true);
			}
			else
			{
				this.shop_Desc_NoMoney.SetActive(false);
			}
			bool active = false;
			if (Inv_Manager.instance.GetProdPrefab(itemIndex).needsRefrigerator)
			{
				active = true;
			}
			this.shop_Desc_NeedsRefrigerator.gameObject.SetActive(active);
			this.shop_Desc_QntInv_Text.text = "x" + Inv_Manager.instance.GetProdQnt_OnBox(itemIndex).ToString();
			this.shop_Desc_QntSale_Panel.SetActive(true);
			this.shop_Desc_QntSale_Text.text = "x" + Inv_Manager.instance.GetProdQnt_OnShelf(itemIndex).ToString();
			return;
		}
		if (this.shopCatIndexSelected == 1)
		{
			int itemIndex2 = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.shelfProd_Prefabs[this.shopIndexSelected].gameObject);
			this.shop_Desc_Panel.color = Inv_Manager.instance.GetItemShelf(itemIndex2).itemColor[0];
			this.shop_Desc_ItemName.text = "Shelf";
			this.shop_Desc_ItemImage.sprite = Inv_Manager.instance.GetItemShelf(itemIndex2).itemSprite;
			this.shop_Desc_ItemImage.SetNativeSize();
			this.shop_Desc_Price.text = Mathf.FloorToInt(Inv_Manager.instance.GetShelfBoxPrice(itemIndex2) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected]).ToString();
			if (Finances_Manager.instance.GetMoney() < Inv_Manager.instance.GetShelfBoxPrice(itemIndex2))
			{
				this.shop_Desc_NoMoney.SetActive(true);
			}
			else
			{
				this.shop_Desc_NoMoney.SetActive(false);
			}
			bool active2 = false;
			if (Inv_Manager.instance.GetItemShelf(itemIndex2).isRefrigerated)
			{
				active2 = true;
			}
			this.shop_Desc_NeedsRefrigerator.gameObject.SetActive(active2);
			this.shop_Desc_QntInv_Text.text = "x" + Inv_Manager.instance.GetShelfQnt_OnBox(itemIndex2).ToString();
			this.shop_Desc_QntSale_Panel.SetActive(false);
			this.shop_Desc_PanelDown.gameObject.SetActive(true);
			return;
		}
		if (this.shopCatIndexSelected == 2)
		{
			int itemIndex3 = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.decor_Prefabs[this.shopIndexSelected].gameObject);
			this.shop_Desc_Panel.color = Inv_Manager.instance.GetItemDecor(itemIndex3).itemColor[0];
			this.shop_Desc_ItemName.text = "Decoration";
			this.shop_Desc_ItemImage.sprite = Inv_Manager.instance.GetItemDecor(itemIndex3).itemSprite;
			this.shop_Desc_ItemImage.SetNativeSize();
			this.shop_Desc_Price.text = Mathf.FloorToInt(Inv_Manager.instance.GetDecorBoxPrice(itemIndex3) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected]).ToString();
			if (Finances_Manager.instance.GetMoney() < Inv_Manager.instance.GetDecorBoxPrice(itemIndex3))
			{
				this.shop_Desc_NoMoney.SetActive(true);
			}
			else
			{
				this.shop_Desc_NoMoney.SetActive(false);
			}
			this.shop_Desc_NeedsRefrigerator.gameObject.SetActive(false);
			this.shop_Desc_QntInv_Text.text = "x" + Inv_Manager.instance.GetDecorQnt_OnBox(itemIndex3).ToString();
			this.shop_Desc_QntSale_Panel.SetActive(false);
			this.shop_Desc_PanelDown.gameObject.SetActive(true);
			return;
		}
		if (this.shopCatIndexSelected == 3)
		{
			int itemIndex4 = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.wallPaint_Prefabs[this.shopIndexSelected].gameObject);
			this.shop_Desc_Panel.color = Inv_Manager.instance.GetItemWallPaint(itemIndex4).itemColor[0];
			this.shop_Desc_ItemName.text = "Paint";
			this.shop_Desc_ItemImage.sprite = Inv_Manager.instance.GetItemWallPaint(itemIndex4).itemSprite;
			this.shop_Desc_ItemImage.SetNativeSize();
			this.shop_Desc_Price.text = Mathf.FloorToInt(Inv_Manager.instance.GetWallPaintBoxPrice(itemIndex4) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected]).ToString();
			if (Finances_Manager.instance.GetMoney() < Inv_Manager.instance.GetWallPaintBoxPrice(itemIndex4))
			{
				this.shop_Desc_NoMoney.SetActive(true);
			}
			else
			{
				this.shop_Desc_NoMoney.SetActive(false);
			}
			this.shop_Desc_NeedsRefrigerator.gameObject.SetActive(false);
			this.shop_Desc_QntInv_Text.text = "x" + Inv_Manager.instance.GetWallPaintQnt_OnBox(itemIndex4).ToString();
			this.shop_Desc_QntSale_Panel.SetActive(false);
			this.shop_Desc_PanelDown.gameObject.SetActive(true);
			return;
		}
		if (this.shopCatIndexSelected == 4)
		{
			int itemIndex5 = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.floor_Prefabs[this.shopIndexSelected].gameObject);
			this.shop_Desc_Panel.color = Inv_Manager.instance.GetItemFloor(itemIndex5).itemColor[0];
			this.shop_Desc_ItemName.text = "Floor";
			this.shop_Desc_ItemImage.sprite = Inv_Manager.instance.GetItemFloor(itemIndex5).itemSprite;
			this.shop_Desc_ItemImage.SetNativeSize();
			this.shop_Desc_Price.text = Mathf.FloorToInt(Inv_Manager.instance.GetFloorBoxPrice(itemIndex5) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected]).ToString();
			if (Finances_Manager.instance.GetMoney() < Inv_Manager.instance.GetFloorBoxPrice(itemIndex5))
			{
				this.shop_Desc_NoMoney.SetActive(true);
			}
			else
			{
				this.shop_Desc_NoMoney.SetActive(false);
			}
			this.shop_Desc_NeedsRefrigerator.gameObject.SetActive(false);
			this.shop_Desc_QntInv_Text.text = "x" + Inv_Manager.instance.GetFloorQnt_OnBox(itemIndex5).ToString();
			this.shop_Desc_QntSale_Panel.SetActive(false);
			this.shop_Desc_PanelDown.gameObject.SetActive(true);
			return;
		}
		if (this.shopCatIndexSelected == 5)
		{
			int itemIndex6 = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.util_Prefabs[this.shopIndexSelected].gameObject);
			this.shop_Desc_Panel.color = Inv_Manager.instance.GetItemUtil(itemIndex6).itemColor[0];
			this.shop_Desc_ItemName.text = "Utility";
			this.shop_Desc_ItemImage.sprite = Inv_Manager.instance.GetItemUtil(itemIndex6).itemSprite;
			this.shop_Desc_ItemImage.SetNativeSize();
			this.shop_Desc_Price.text = Mathf.FloorToInt(Inv_Manager.instance.GetUtilBoxPrice(itemIndex6) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected]).ToString();
			if (Finances_Manager.instance.GetMoney() < Inv_Manager.instance.GetUtilBoxPrice(itemIndex6))
			{
				this.shop_Desc_NoMoney.SetActive(true);
			}
			else
			{
				this.shop_Desc_NoMoney.SetActive(false);
			}
			this.shop_Desc_NeedsRefrigerator.gameObject.SetActive(false);
			this.shop_Desc_QntInv_Text.text = "x" + Inv_Manager.instance.GetUtilQnt_OnBox(itemIndex6).ToString();
			this.shop_Desc_QntSale_Panel.SetActive(false);
			this.shop_Desc_PanelDown.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x00029604 File Offset: 0x00027804
	public void RefreshTomorrowDelivery()
	{
		for (int i = 0; i < Inv_Manager.instance.shop_Deliv_Qnt; i++)
		{
			if (i < Inv_Manager.instance.prod_deliveryIndexes.Count)
			{
				int num = Inv_Manager.instance.prod_deliveryCategories[i];
				if (num == 0)
				{
					Inv_Manager.instance.GetProdPrefab(Inv_Manager.instance.prod_deliveryIndexes[i]);
					this.shop_Deliv_ItemPanel[i].color = this.shop_Sup_Colors[1];
					this.shop_Deliv_ItemImage[i].sprite = Inv_Manager.instance.GetProdSprite(Inv_Manager.instance.prod_deliveryIndexes[i]);
					this.shop_Deliv_ItemImage[i].gameObject.SetActive(true);
					this.shop_Deliv_QntImage[i].color = this.shop_Sup_Colors[1];
					this.shop_Deliv_QntImage[i].gameObject.SetActive(true);
					this.shop_Deliv_QntText[i].text = "x" + Inv_Manager.instance.boxSize.ToString();
					this.shop_Deliv_SupPanel[i].gameObject.SetActive(true);
					this.shop_Deliv_SupImage[i].sprite = this.shop_Deliv_Sup_Sprites[Inv_Manager.instance.prod_deliverySupplierIndexes[i]];
					this.shop_Deliv_SupPanel[i].color = this.shop_Sup_Colors[1];
					string text = Language_Manager.instance.GetText("today");
					string text2 = Language_Manager.instance.GetText("days");
					int num2 = Inv_Manager.instance.prod_deliveryDaysIndexes[i];
					if (num2 == 1)
					{
						text = Language_Manager.instance.GetText("tomorrow");
						this.shop_Deliv_SupPanel[i].color = this.shop_Sup_Colors[0];
						this.shop_Deliv_ItemPanel[i].color = this.shop_Sup_Colors[0];
						this.shop_Deliv_QntImage[i].color = this.shop_Sup_Colors[0];
					}
					else if (num2 > 1)
					{
						if (num2 == 1)
						{
							text2 = Language_Manager.instance.GetText("day");
						}
						text = num2.ToString() + " " + text2;
					}
					this.shop_Deliv_SupText[i].text = text.ToLower();
				}
				else if (num == 1)
				{
					Shelf_Controller itemShelf = Inv_Manager.instance.GetItemShelf(Inv_Manager.instance.prod_deliveryIndexes[i]);
					this.shop_Deliv_ItemPanel[i].color = this.shop_Sup_Colors[1];
					this.shop_Deliv_ItemImage[i].sprite = itemShelf.itemSprite;
					this.shop_Deliv_ItemImage[i].gameObject.SetActive(true);
					this.shop_Deliv_QntImage[i].color = this.shop_Sup_Colors[1];
					this.shop_Deliv_QntImage[i].gameObject.SetActive(true);
					this.shop_Deliv_QntText[i].text = "x" + Inv_Manager.instance.boxSize_ShelvesAndDecor.ToString();
					this.shop_Deliv_SupPanel[i].gameObject.SetActive(true);
					this.shop_Deliv_SupImage[i].sprite = this.shop_Deliv_Sup_Sprites[Inv_Manager.instance.prod_deliverySupplierIndexes[i]];
					this.shop_Deliv_SupPanel[i].color = this.shop_Sup_Colors[1];
					string text3 = Language_Manager.instance.GetText("today");
					string text4 = Language_Manager.instance.GetText("days");
					int num3 = Inv_Manager.instance.prod_deliveryDaysIndexes[i];
					if (num3 == 1)
					{
						text3 = Language_Manager.instance.GetText("tomorrow");
						this.shop_Deliv_SupPanel[i].color = this.shop_Sup_Colors[0];
						this.shop_Deliv_ItemPanel[i].color = this.shop_Sup_Colors[0];
						this.shop_Deliv_QntImage[i].color = this.shop_Sup_Colors[0];
					}
					else if (num3 > 1)
					{
						if (num3 == 1)
						{
							text4 = Language_Manager.instance.GetText("day");
						}
						text3 = num3.ToString() + " " + text4;
					}
					this.shop_Deliv_SupText[i].text = text3.ToLower();
				}
				else if (num == 2)
				{
					Decor_Controller itemDecor = Inv_Manager.instance.GetItemDecor(Inv_Manager.instance.prod_deliveryIndexes[i]);
					this.shop_Deliv_ItemPanel[i].color = this.shop_Sup_Colors[1];
					this.shop_Deliv_ItemImage[i].sprite = itemDecor.itemSprite;
					this.shop_Deliv_ItemImage[i].gameObject.SetActive(true);
					this.shop_Deliv_QntImage[i].color = this.shop_Sup_Colors[1];
					this.shop_Deliv_QntImage[i].gameObject.SetActive(true);
					this.shop_Deliv_QntText[i].text = "x" + Inv_Manager.instance.boxSize_ShelvesAndDecor.ToString();
					this.shop_Deliv_SupPanel[i].gameObject.SetActive(true);
					this.shop_Deliv_SupImage[i].sprite = this.shop_Deliv_Sup_Sprites[Inv_Manager.instance.prod_deliverySupplierIndexes[i]];
					this.shop_Deliv_SupPanel[i].color = this.shop_Sup_Colors[1];
					string text5 = Language_Manager.instance.GetText("today");
					string text6 = Language_Manager.instance.GetText("days");
					int num4 = Inv_Manager.instance.prod_deliveryDaysIndexes[i];
					if (num4 == 1)
					{
						text5 = Language_Manager.instance.GetText("tomorrow");
						this.shop_Deliv_SupPanel[i].color = this.shop_Sup_Colors[0];
						this.shop_Deliv_ItemPanel[i].color = this.shop_Sup_Colors[0];
						this.shop_Deliv_QntImage[i].color = this.shop_Sup_Colors[0];
					}
					else if (num4 > 1)
					{
						if (num4 == 1)
						{
							text6 = Language_Manager.instance.GetText("day");
						}
						text5 = num4.ToString() + " " + text6;
					}
					this.shop_Deliv_SupText[i].text = text5.ToLower();
				}
				else if (num == 3)
				{
					WallPaint_Controller itemWallPaint = Inv_Manager.instance.GetItemWallPaint(Inv_Manager.instance.prod_deliveryIndexes[i]);
					this.shop_Deliv_ItemPanel[i].color = this.shop_Sup_Colors[1];
					this.shop_Deliv_ItemImage[i].sprite = itemWallPaint.itemSprite;
					this.shop_Deliv_ItemImage[i].gameObject.SetActive(true);
					this.shop_Deliv_QntImage[i].color = this.shop_Sup_Colors[1];
					this.shop_Deliv_QntImage[i].gameObject.SetActive(true);
					this.shop_Deliv_QntText[i].text = "x" + Inv_Manager.instance.boxSize.ToString();
					this.shop_Deliv_SupPanel[i].gameObject.SetActive(true);
					this.shop_Deliv_SupImage[i].sprite = this.shop_Deliv_Sup_Sprites[Inv_Manager.instance.prod_deliverySupplierIndexes[i]];
					this.shop_Deliv_SupPanel[i].color = this.shop_Sup_Colors[1];
					string text7 = Language_Manager.instance.GetText("today");
					string text8 = Language_Manager.instance.GetText("days");
					int num5 = Inv_Manager.instance.prod_deliveryDaysIndexes[i];
					if (num5 == 1)
					{
						text7 = Language_Manager.instance.GetText("tomorrow");
						this.shop_Deliv_SupPanel[i].color = this.shop_Sup_Colors[0];
						this.shop_Deliv_ItemPanel[i].color = this.shop_Sup_Colors[0];
						this.shop_Deliv_QntImage[i].color = this.shop_Sup_Colors[0];
					}
					else if (num5 > 1)
					{
						if (num5 == 1)
						{
							text8 = Language_Manager.instance.GetText("day");
						}
						text7 = num5.ToString() + " " + text8;
					}
					this.shop_Deliv_SupText[i].text = text7.ToLower();
				}
				else if (num == 4)
				{
					Floor_Controller itemFloor = Inv_Manager.instance.GetItemFloor(Inv_Manager.instance.prod_deliveryIndexes[i]);
					this.shop_Deliv_ItemPanel[i].color = this.shop_Sup_Colors[1];
					this.shop_Deliv_ItemImage[i].sprite = itemFloor.itemSprite;
					this.shop_Deliv_ItemImage[i].gameObject.SetActive(true);
					this.shop_Deliv_QntImage[i].color = this.shop_Sup_Colors[1];
					this.shop_Deliv_QntImage[i].gameObject.SetActive(true);
					this.shop_Deliv_QntText[i].text = "x" + Inv_Manager.instance.boxSize.ToString();
					this.shop_Deliv_SupPanel[i].gameObject.SetActive(true);
					this.shop_Deliv_SupImage[i].sprite = this.shop_Deliv_Sup_Sprites[Inv_Manager.instance.prod_deliverySupplierIndexes[i]];
					this.shop_Deliv_SupPanel[i].color = this.shop_Sup_Colors[1];
					string text9 = Language_Manager.instance.GetText("today");
					string text10 = Language_Manager.instance.GetText("days");
					int num6 = Inv_Manager.instance.prod_deliveryDaysIndexes[i];
					if (num6 == 1)
					{
						text9 = Language_Manager.instance.GetText("tomorrow");
						this.shop_Deliv_SupPanel[i].color = this.shop_Sup_Colors[0];
						this.shop_Deliv_ItemPanel[i].color = this.shop_Sup_Colors[0];
						this.shop_Deliv_QntImage[i].color = this.shop_Sup_Colors[0];
					}
					else if (num6 > 1)
					{
						if (num6 == 1)
						{
							text10 = Language_Manager.instance.GetText("day");
						}
						text9 = num6.ToString() + " " + text10;
					}
					this.shop_Deliv_SupText[i].text = text9.ToLower();
				}
				else if (num == 5)
				{
					Util_Controller itemUtil = Inv_Manager.instance.GetItemUtil(Inv_Manager.instance.prod_deliveryIndexes[i]);
					this.shop_Deliv_ItemPanel[i].color = this.shop_Sup_Colors[1];
					this.shop_Deliv_ItemImage[i].sprite = itemUtil.itemSprite;
					this.shop_Deliv_ItemImage[i].gameObject.SetActive(true);
					this.shop_Deliv_QntImage[i].color = this.shop_Sup_Colors[1];
					this.shop_Deliv_QntImage[i].gameObject.SetActive(true);
					this.shop_Deliv_QntText[i].text = "x" + Inv_Manager.instance.boxSize_ShelvesAndDecor.ToString();
					this.shop_Deliv_SupPanel[i].gameObject.SetActive(true);
					this.shop_Deliv_SupImage[i].sprite = this.shop_Deliv_Sup_Sprites[Inv_Manager.instance.prod_deliverySupplierIndexes[i]];
					this.shop_Deliv_SupPanel[i].color = this.shop_Sup_Colors[1];
					string text11 = Language_Manager.instance.GetText("today");
					string text12 = Language_Manager.instance.GetText("days");
					int num7 = Inv_Manager.instance.prod_deliveryDaysIndexes[i];
					if (num7 == 1)
					{
						text11 = Language_Manager.instance.GetText("tomorrow");
						this.shop_Deliv_SupPanel[i].color = this.shop_Sup_Colors[0];
						this.shop_Deliv_ItemPanel[i].color = this.shop_Sup_Colors[0];
						this.shop_Deliv_QntImage[i].color = this.shop_Sup_Colors[0];
					}
					else if (num7 > 1)
					{
						if (num7 == 1)
						{
							text12 = Language_Manager.instance.GetText("day");
						}
						text11 = num7.ToString() + " " + text12;
					}
					this.shop_Deliv_SupText[i].text = text11.ToLower();
				}
			}
			else
			{
				this.shop_Deliv_ItemPanel[i].color = this.shop_Deliv_NoColor;
				this.shop_Deliv_ItemImage[i].gameObject.SetActive(false);
				this.shop_Deliv_QntImage[i].gameObject.SetActive(false);
				this.shop_Deliv_SupPanel[i].gameObject.SetActive(false);
			}
		}
		if (Inv_Manager.instance.prod_deliveryIndexes.Count > 0)
		{
			this.shop_Deliv_DeleteButton.SetActive(true);
			return;
		}
		this.shop_Deliv_DeleteButton.SetActive(false);
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x0002A38E File Offset: 0x0002858E
	public void SelectShopSupIndex(int _index)
	{
		this.shopSupIndexSelected = _index;
		this.RefreshShopDescription(true);
		this.RefreshShopItems();
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x0002A3A8 File Offset: 0x000285A8
	public void ChangeShopSupIndex(int _direction = 1)
	{
		if (!this.shop_may_buy)
		{
			return;
		}
		this.shopSupIndexSelected += _direction;
		if (this.shopSupIndexSelected >= this.shop_Sup_Buttons.Count)
		{
			this.shopSupIndexSelected = 0;
		}
		else if (this.shopSupIndexSelected < 0)
		{
			this.shopSupIndexSelected = this.shop_Sup_Buttons.Count - 1;
		}
		this.SelectShopSupIndex(this.shopSupIndexSelected);
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x0002A410 File Offset: 0x00028610
	public void RefreshShopSup()
	{
		for (int i = 0; i < this.shop_Sup_Buttons.Count; i++)
		{
			if (i == this.shopSupIndexSelected)
			{
				this.shop_Sup_Buttons[i].color = this.shop_Sup_Colors[i];
			}
			else
			{
				this.shop_Sup_Buttons[i].color = this.shop_UnlockedColors[0];
			}
		}
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x0002A478 File Offset: 0x00028678
	private void RefreshInvItems()
	{
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x0002A47A File Offset: 0x0002867A
	private void RefreshInvItemPrices(int _index)
	{
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x0002A47C File Offset: 0x0002867C
	public void SelectInvItem(int _index)
	{
		this.invIndexSelected = _index;
		this.RefreshInvDescription();
		if (Input_Manager.instance.GetScheme(-1) != "Joystick")
		{
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
		}
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x0002A4B6 File Offset: 0x000286B6
	public void Inv_IncreasePrice()
	{
		Inv_Manager.instance.IncreaseProdSellPrice(this.invIndexSelected, 1);
		this.RefreshInvItemPrices(this.invIndexSelected);
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x0002A4D5 File Offset: 0x000286D5
	public void Inv_DecreasePrice()
	{
		Inv_Manager.instance.IncreaseProdSellPrice(this.invIndexSelected, -1);
		this.RefreshInvItemPrices(this.invIndexSelected);
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x0002A4F4 File Offset: 0x000286F4
	public void RefreshInvDescription()
	{
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x0002A4F8 File Offset: 0x000286F8
	public void RefreshFinancesTab()
	{
		int num = World_Manager.instance.GetDayIndex();
		for (int i = 0; i < this.fin_GOs.Count; i++)
		{
			string text = World_Manager.instance.GetDayNameByIndex(num);
			if (i == 0)
			{
				text = Language_Manager.instance.GetText("Today");
			}
			num--;
			if (num < 0)
			{
				num = 6;
			}
			if (i < Finances_Manager.instance.GetList_InSales().Count)
			{
				this.fin_TextTitle[i].text = text;
				if (Finances_Manager.instance.GetList_OutProds()[i] == 0f)
				{
					this.fin_TextProds[i].text = "-";
				}
				else
				{
					this.fin_TextProds[i].text = "-" + Finances_Manager.instance.GetList_OutProds()[i].ToString();
				}
				if (Finances_Manager.instance.GetList_OutFurniture()[i] == 0f)
				{
					this.fin_TextFurniture[i].text = "-";
				}
				else
				{
					this.fin_TextFurniture[i].text = "-" + Finances_Manager.instance.GetList_OutFurniture()[i].ToString();
				}
				if (Finances_Manager.instance.GetList_OutStaff()[i] == 0f)
				{
					this.fin_TextStaff[i].text = "-";
				}
				else
				{
					this.fin_TextStaff[i].text = "-" + Finances_Manager.instance.GetList_OutStaff()[i].ToString();
				}
				if (Finances_Manager.instance.GetList_OutOperational()[i] == 0f)
				{
					this.fin_TextOperational[i].text = "-";
				}
				else
				{
					this.fin_TextOperational[i].text = "-" + Finances_Manager.instance.GetList_OutOperational()[i].ToString();
				}
				if (Finances_Manager.instance.GetList_OutExpansion()[i] == 0f)
				{
					this.fin_TextExpansion[i].text = "-";
				}
				else
				{
					this.fin_TextExpansion[i].text = "-" + Finances_Manager.instance.GetList_OutExpansion()[i].ToString();
				}
				if (Finances_Manager.instance.GetList_OutMarketing()[i] == 0f)
				{
					this.fin_TextMarketing[i].text = "-";
				}
				else
				{
					this.fin_TextMarketing[i].text = "-" + Finances_Manager.instance.GetList_OutMarketing()[i].ToString();
				}
				if (Finances_Manager.instance.GetList_InSales()[i] == 0f)
				{
					this.fin_TextSales[i].text = "-";
				}
				else
				{
					this.fin_TextSales[i].text = "+" + Finances_Manager.instance.GetList_InSales()[i].ToString();
				}
				if (Finances_Manager.instance.GetList_InPrizes()[i] == 0f)
				{
					this.fin_TextPrizes[i].text = "-";
				}
				else
				{
					this.fin_TextPrizes[i].text = "+" + Finances_Manager.instance.GetList_InPrizes()[i].ToString();
				}
				float num2 = Finances_Manager.instance.GetMoneyInByIndex(i) - Finances_Manager.instance.GetMoneyOutByIndex(i);
				if (num2 == 0f)
				{
					this.fin_TextBalance[i].text = "-";
				}
				else if (num2 > 0f)
				{
					this.fin_TextBalance[i].text = "+" + num2.ToString();
				}
				else
				{
					this.fin_TextBalance[i].text = num2.ToString();
				}
			}
			else
			{
				this.fin_TextTitle[i].text = "";
				this.fin_TextProds[i].text = "";
				this.fin_TextFurniture[i].text = "";
				this.fin_TextStaff[i].text = "";
				this.fin_TextOperational[i].text = "";
				this.fin_TextExpansion[i].text = "";
				this.fin_TextMarketing[i].text = "";
				this.fin_TextSales[i].text = "";
				this.fin_TextPrizes[i].text = "";
				this.fin_TextBalance[i].text = "";
			}
		}
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x0002A9F8 File Offset: 0x00028BF8
	public void CreateReferences_Staff()
	{
		for (int i = 0; i < this.staff_MainPanel.Count; i++)
		{
			GameObject gameObject = this.staff_MainPanel[i].transform.Find("Panel_Main").gameObject;
			this.staff_Name_Text.Add(gameObject.transform.Find("Text_Name").GetComponent<Text>());
			this.staff_Energy.Add(gameObject.transform.Find("Panel_Energy").Find("Image").Find("Image_Energy").GetComponent<Image>());
			this.staff_Panel_Off.Add(gameObject.transform.Find("Panel_Off").gameObject);
			this.staff_Text_OutToday.Add(gameObject.transform.Find("Panel_Off").Find("Text").GetComponent<Text>());
			this.staff_Button_GiveDaysOff.Add(gameObject.transform.Find("Button_GiveDaysOff").gameObject);
			this.staff_Button_Fire.Add(gameObject.transform.Find("Button_Fire").gameObject);
			this.staff_Panel_No_Staff.Add(this.staff_MainPanel[i].transform.Find("Image_No_Staff").gameObject);
			this.staff_price.Add(gameObject.transform.Find("Panel_Skills").Find("Panel_Price").Find("Text").GetComponent<Text>());
			List<Button> list = new List<Button>();
			List<Image> list2 = new List<Image>();
			List<Image> list3 = new List<Image>();
			for (int j = 0; j < 3; j++)
			{
				list.Add(gameObject.transform.Find("Panel_Skills").Find("Layout").Find("Skill (" + j.ToString() + ")").Find("Button").GetComponent<Button>());
				int _i = i;
				int _o = j;
				list[j].onClick.AddListener(delegate()
				{
					Char_Manager.instance.SetStaffTasks(_i, _o);
				});
				list2.Add(gameObject.transform.Find("Panel_Skills").Find("Layout").Find("Skill (" + j.ToString() + ")").Find("Button").Find("Image").GetComponent<Image>());
				list3.Add(gameObject.transform.Find("Panel_Skills").Find("Layout").Find("Skill (" + j.ToString() + ")").Find("Image").Find("Image").GetComponent<Image>());
			}
			this.staff_Tasks_Buttons.Add(list);
			this.staff_Tasks_Image.Add(list2);
			this.staff_Skills_Image.Add(list3);
			int _index = i;
			this.staff_Button_GiveDaysOff[i].GetComponent<Button>().onClick.AddListener(delegate()
			{
				Char_Manager.instance.SetStaffDaysOff(_index);
			});
			this.staff_Button_Fire[i].GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.Staff_FireStaff(_index);
			});
		}
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x0002AD5C File Offset: 0x00028F5C
	public void RefreshStaffTab()
	{
		if (Char_Manager.instance.staff_Possible_Staff_Data == null || Char_Manager.instance.staff_Possible_Staff_Data.Count <= 0)
		{
			this.staff_Button_GetApplicants.gameObject.SetActive(true);
			this.staff_Panel_WaitEmail.gameObject.SetActive(false);
		}
		else
		{
			this.staff_Button_GetApplicants.gameObject.SetActive(false);
			this.staff_Panel_WaitEmail.gameObject.SetActive(true);
		}
		for (int i = 0; i < this.staff_MainPanel.Count; i++)
		{
			if (i < Char_Manager.instance.staff_Data.Count)
			{
				this.staff_MainPanel[i].transform.Find("Panel_Main").gameObject.SetActive(true);
				Staff_Data staff_Data = Char_Manager.instance.staff_Data[i];
				this.staff_Name_Text[i].text = staff_Data.name;
				this.staff_Energy[i].fillAmount = staff_Data.energy;
				this.staff_price[i].text = staff_Data.price.ToString();
				if (Char_Manager.instance.staff_Data[i].daysOff == 0 && ((Game_Manager.instance.GetMartOpen() && Char_Manager.instance.Get_StaffInStore(Char_Manager.instance.staff_Data[i])) || !Game_Manager.instance.GetMartOpen()))
				{
					this.staff_Panel_Off[i].SetActive(false);
					this.staff_Button_GiveDaysOff[i].SetActive(true);
				}
				else if (Char_Manager.instance.staff_Data[i].daysOff > 1)
				{
					this.staff_Panel_Off[i].SetActive(true);
					this.staff_Button_GiveDaysOff[i].SetActive(false);
					this.staff_Text_OutToday[i].text = Language_Manager.instance.GetText("Out tomorrow");
				}
				else
				{
					this.staff_Panel_Off[i].SetActive(true);
					this.staff_Button_GiveDaysOff[i].SetActive(false);
					this.staff_Text_OutToday[i].text = Language_Manager.instance.GetText("Out today");
				}
				for (int j = 0; j < this.staff_Tasks_Image.Count; j++)
				{
					this.staff_Tasks_Image[i][j].gameObject.SetActive(staff_Data.tasks[j]);
					this.staff_Skills_Image[i][j].fillAmount = staff_Data.skills[j];
				}
				this.staff_Panel_No_Staff[i].gameObject.SetActive(false);
				this.staff_Button_Fire[i].SetActive(true);
			}
			else
			{
				this.staff_MainPanel[i].transform.Find("Panel_Main").gameObject.SetActive(false);
				this.staff_Panel_No_Staff[i].gameObject.SetActive(true);
				this.staff_Button_GiveDaysOff[i].SetActive(false);
				this.staff_Button_Fire[i].SetActive(false);
			}
		}
		for (int k = 0; k < this.news_ClimateImages.Count; k++)
		{
			this.news_ClimateImages[k].sprite = World_Manager.instance.GetForecastSpriteByIndex(World_Manager.instance.climate_Indexes[k]);
		}
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x0002B0BB File Offset: 0x000292BB
	public void Staff_GiveDaysOff()
	{
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x0002B0C0 File Offset: 0x000292C0
	public void Staff_FireStaff(int _index)
	{
		Debug.Log("Trashing confirmation");
		Menu_Manager.instance.SetWarningConfirmation("Do you want to fire this employee?", 0, "PC").onClick.AddListener(delegate()
		{
			Char_Manager.instance.FireStaff(_index);
		});
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x0002B10F File Offset: 0x0002930F
	public void CreateReferences_Expand()
	{
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x0002B114 File Offset: 0x00029314
	public void RefreshExpandTab()
	{
		int currentExpansionIndex = World_Manager.instance.currentExpansionIndex;
		for (int i = 0; i < this.news_Expand_Expanded.Count; i++)
		{
			if (i < currentExpansionIndex)
			{
				this.news_Expand_Expanded[i].SetActive(true);
			}
			else
			{
				this.news_Expand_Expanded[i].SetActive(false);
			}
			if (i < World_Manager.instance.currentLevel.expansions_GOs.Count)
			{
				this.news_Expand_Unavailable[i].SetActive(false);
			}
			else
			{
				this.news_Expand_Unavailable[i].SetActive(true);
			}
		}
		if (currentExpansionIndex >= World_Manager.instance.currentLevel.expansions_GOs.Count)
		{
			this.news_Expand_CompletedPanel.SetActive(true);
		}
		else
		{
			this.news_Expand_CompletedPanel.SetActive(false);
		}
		this.news_Expand_Button.transform.Find("Panel_Expand").Find("Panel").Find("Text_Price").GetComponent<Text>().text = World_Manager.instance.Get_ExpansionPrice().ToString();
		this.Refresh_Manage_Locations();
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x0002B228 File Offset: 0x00029428
	public void Refresh_Manage_Locations()
	{
		for (int i = 0; i < this.manage_panel_locations_buttons.Count; i++)
		{
			bool flag = true;
			if (World_Manager.instance.currentLevelIndex == i)
			{
				flag = false;
			}
			this.manage_panel_locations_buttons[i].gameObject.SetActive(flag);
			if (flag)
			{
				this.manage_panel_locations_buttons[i].transform.Find("Panel_Price").Find("Text_Price").GetComponent<Text>().text = World_Manager.instance.Get_LevelPrice_ByIndex(i).ToString();
			}
		}
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x0002B2B8 File Offset: 0x000294B8
	public void CreateReferences_LocalCustomersTab()
	{
		List<Char_Controller> list = new List<Char_Controller>(Char_Manager.instance.GetAllCustomerPrefabs());
		Transform parent = this.cust_Buttons[0].transform.parent;
		Button button = this.cust_Buttons[0];
		this.cust_Buttons.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			Button button2 = UnityEngine.Object.Instantiate<Button>(button);
			this.cust_Buttons.Add(button2);
			button2.transform.SetParent(parent);
			button2.transform.localScale = Vector3.one;
			button2.transform.localPosition = Vector3.zero;
			button2.transform.localRotation = Quaternion.Euler(Vector3.zero);
			int _index = i;
			button2.onClick.AddListener(delegate()
			{
				this.SetCustomerDescription(_index);
			});
			if (list[i])
			{
				button2.gameObject.SetActive(true);
			}
			else
			{
				button2.gameObject.SetActive(false);
			}
		}
		button.gameObject.SetActive(false);
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x0002B3DC File Offset: 0x000295DC
	public void RefreshLocalCustomerTab()
	{
		for (int i = 0; i < this.cust_Buttons.Count; i++)
		{
			Char_Controller char_Controller = Char_Manager.instance.GetAllCustomerPrefabs()[i];
			if (char_Controller)
			{
				this.cust_Buttons[i].transform.Find("Panel_Name").GetComponent<Image>().color = char_Controller.GetCharColor();
				this.cust_Buttons[i].transform.Find("Panel_Name").Find("Text_Name").GetComponent<Text>().text = char_Controller.GetCharName();
				Image component = this.cust_Buttons[i].transform.Find("Image_Cust").GetComponent<Image>();
				component.sprite = char_Controller.GetCharSprite();
				component.SetNativeSize();
				this.cust_Buttons[i].transform.Find("Image_Friendship").GetComponent<Image>().gameObject.SetActive(Char_Manager.instance.GetCustomerLifeAchievementState(i));
			}
		}
		this.Set_Buttons_MenuNav_Connections(4, this.cust_Buttons);
		this.SetCustomerDescription(0);
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x0002B4FC File Offset: 0x000296FC
	public void SetCustomerDescription(int _index)
	{
		Char_Controller customerPrefab = Char_Manager.instance.GetCustomerPrefab(_index);
		this.cust_Desc_Name_Text.text = customerPrefab.GetCharName();
		this.cust_Desc_Name_Panel.color = customerPrefab.GetCharColor();
		this.cust_Desc_Image.sprite = customerPrefab.GetCharSprite();
		this.cust_Desc_Image.SetNativeSize();
		List<Prod_Controller> list = new List<Prod_Controller>(customerPrefab.GetComponent<Customer_Controller>().GetProdPredilectionList());
		List<bool> list2 = new List<bool>(Char_Manager.instance.localCustomer_Datas[customerPrefab.GetID()].prodPreferenceUnlocked);
		for (int i = 0; i < this.cust_Predilections_Images.Count; i++)
		{
			if (i < list.Count && list[i])
			{
				this.cust_Predilections_Images[i].gameObject.SetActive(true);
				Sprite sprite = Inv_Manager.instance.GetProdSprite(Inv_Manager.instance.GetItemIndex(list[i].gameObject));
				if (i < list2.Count && !list2[i])
				{
					sprite = Char_Manager.instance.prod_UnknownSprite;
				}
				this.cust_Predilections_Images[i].sprite = sprite;
				this.cust_Predilections_Images[i].SetNativeSize();
			}
			else
			{
				this.cust_Predilections_Images[i].gameObject.SetActive(false);
			}
		}
		bool customerLifeAchievementState = Char_Manager.instance.GetCustomerLifeAchievementState(_index);
		this.cust_LifeAchievement_PanelImage.SetActive(customerLifeAchievementState);
		this.cust_LifeAchievement_PanelInfo.SetActive(!customerLifeAchievementState);
		if (customerLifeAchievementState)
		{
			this.cust_LifeAchievement_Photo.sprite = customerPrefab.charFriendshipPhoto;
			this.cust_LifeAchievement_Photo.SetNativeSize();
		}
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x0002B6A4 File Offset: 0x000298A4
	public void CreateReferences_Mail()
	{
		this.mail_button_ref.gameObject.SetActive(false);
		this.mail_mails_list_parent = this.mail_button_ref.transform.parent;
		foreach (PC_Manager.Mail_Class mail_Class in this.mail_class_list)
		{
			UnityEngine.Object.Destroy(mail_Class.button.gameObject);
		}
		this.mail_class_list.Clear();
		for (int i = 0; i < 200; i++)
		{
			PC_Manager.Mail_Class mail_Class2 = new PC_Manager.Mail_Class();
			mail_Class2.Create_References(i);
			mail_Class2.button.transform.SetParent(this.mail_mails_list_parent);
			mail_Class2.button.transform.localScale = Vector3.one;
			mail_Class2.button.transform.localPosition = Vector3.zero;
			this.mail_class_list.Add(mail_Class2);
		}
		for (int j = 0; j < this.mail_class_list.Count; j++)
		{
			if (j > 0 && j < this.mail_class_list.Count - 1)
			{
				this.mail_class_list[j].button.GetComponent<MenuNav_Controller>().nav_Up = this.mail_class_list[j - 1].button.gameObject;
				this.mail_class_list[j].button.GetComponent<MenuNav_Controller>().nav_Down = this.mail_class_list[j + 1].button.gameObject;
			}
			else if (j == 0)
			{
				this.mail_class_list[j].button.GetComponent<MenuNav_Controller>().nav_Down = this.mail_class_list[j + 1].button.gameObject;
			}
			else
			{
				this.mail_class_list[j].button.GetComponent<MenuNav_Controller>().nav_Up = this.mail_class_list[j - 1].button.gameObject;
			}
		}
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x0002B89C File Offset: 0x00029A9C
	public void Mail_Refresh()
	{
		int num = 0;
		foreach (Button button in this.mail_tab.category_buttons)
		{
			if (this.mail_cat_index == num)
			{
				button.GetComponent<Image>().color = this.mail_cat_colors[num];
			}
			else
			{
				button.GetComponent<Image>().color = this.mail_cat_off_color;
			}
			num++;
		}
		List<Mail_Data> list = new List<Mail_Data>(Mail_Manager.instance.Get_Mail_List_By_Category(this.mail_cat_index));
		for (int i = 0; i < this.mail_class_list.Count; i++)
		{
			if (i < list.Count)
			{
				this.mail_class_list[i].button.gameObject.SetActive(true);
				this.mail_class_list[i].Set_Data(list[i]);
			}
			else
			{
				this.mail_class_list[i].data = null;
				this.mail_class_list[i].button.gameObject.SetActive(false);
			}
		}
		this.Mail_SelectMail(0);
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x0002B9D4 File Offset: 0x00029BD4
	public void Mail_Refresh_Description()
	{
		Mail_Data data = this.mail_class_list[this.mail_mail_index].data;
		this.mail_tab.Set_Data(data);
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x0002BA04 File Offset: 0x00029C04
	public void Mail_SelectCat(int _index)
	{
		this.mail_cat_index = _index;
		this.Mail_Refresh();
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x0002BA14 File Offset: 0x00029C14
	public void Mail_NavCat(int _direction)
	{
		this.mail_cat_index += _direction;
		if (this.mail_cat_index >= this.mail_tab.category_buttons.Count)
		{
			this.mail_cat_index = 0;
		}
		else if (this.mail_cat_index < 0)
		{
			this.mail_cat_index = this.mail_tab.category_buttons.Count - 1;
		}
		this.Mail_SelectCat(this.mail_cat_index);
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x0002BA80 File Offset: 0x00029C80
	public void Mail_SelectMail(int _index)
	{
		this.mail_mail_index = _index;
		this.Mail_Refresh_Description();
		if (this.mail_class_list[_index].data != null)
		{
			this.mail_class_list[_index].data.opened = true;
			this.mail_class_list[_index].Refresh();
		}
		Mail_Manager.instance.Get_Has_Unread_Mails();
		this.Refresh_Mail_Unread_Icon();
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x0002BAE6 File Offset: 0x00029CE6
	public void Refresh_Mail_Unread_Icon()
	{
		if (Mail_Manager.instance.new_mails)
		{
			this.mail_image_alert_taskbar.gameObject.SetActive(true);
			return;
		}
		this.mail_image_alert_taskbar.gameObject.SetActive(false);
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x0002BB17 File Offset: 0x00029D17
	public void Button_Start_Task()
	{
		if (this.mail_class_list[this.mail_mail_index].data.owner_index != 4)
		{
			int owner_index = this.mail_class_list[this.mail_mail_index].data.owner_index;
		}
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x0002BB55 File Offset: 0x00029D55
	public void Button_Collect_Reward()
	{
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x0002BB58 File Offset: 0x00029D58
	public void Mail_Hire_Staff()
	{
		if (this.mail_tab.data == null || !this.mail_tab.data.has_staff_data)
		{
			return;
		}
		Char_Manager.instance.HireStaff(this.mail_tab.data.staff_data);
		this.SetTab(4);
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x0002BBA6 File Offset: 0x00029DA6
	public Button GetActiveButton()
	{
		return this.shop_Button_GOs[0].GetComponent<Button>();
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x0002BBBC File Offset: 0x00029DBC
	public void UseButton(int n)
	{
		if (n == 3)
		{
			this.BackButton();
			return;
		}
		if (this.tabIndexSelected == 1)
		{
			if (n == 0)
			{
				this.BuyProduct();
			}
			if (n == 1)
			{
				this.DeleteLastDeliveryProd();
			}
			if (n == 2)
			{
				this.ChangeShopSupIndex(1);
				return;
			}
		}
		else if (this.tabIndexSelected == 2)
		{
			if (n == 1)
			{
				this.Inv_DecreasePrice();
			}
			if (n == 2)
			{
				this.Inv_IncreasePrice();
				return;
			}
		}
		else if (this.tabIndexSelected == 4)
		{
			if (n == 0)
			{
				Menu_Manager.instance.PressButton();
				return;
			}
		}
		else if (this.tabIndexSelected == 7)
		{
			if (n == 0)
			{
				Menu_Manager.instance.PressButton();
				return;
			}
		}
		else if (this.tabIndexSelected == 8 && n == 0)
		{
			if (this.mail_tab.data == null || !this.mail_tab.data.has_staff_data)
			{
				return;
			}
			this.Mail_Hire_Staff();
		}
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x0002BC7F File Offset: 0x00029E7F
	public void BackButton()
	{
		Menu_Manager.instance.BackMenu();
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_CloseWindow);
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x0002BCA0 File Offset: 0x00029EA0
	public void BuyProduct()
	{
		if (!this.shop_may_buy)
		{
			return;
		}
		if (Inv_Manager.instance.prod_deliveryIndexes.Count >= Inv_Manager.instance.shop_Deliv_Qnt)
		{
			Menu_Manager.instance.SetWarning("Max capacity reached", "Please, wait for the next delivery before buying again.", "PC", true, Menu_Manager.instance.specific_player_index);
			return;
		}
		int index = 0;
		float num = 0f;
		if (this.shopCatIndexSelected == 0)
		{
			index = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.prod_Prefabs[this.shopIndexSelected].gameObject);
			num = Inv_Manager.instance.GetBoxPrice(index) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected];
		}
		else if (this.shopCatIndexSelected == 1)
		{
			index = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.shelfProd_Prefabs[this.shopIndexSelected].gameObject);
			num = Inv_Manager.instance.GetShelfBoxPrice(index) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected];
		}
		else if (this.shopCatIndexSelected == 2)
		{
			index = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.decor_Prefabs[this.shopIndexSelected].gameObject);
			num = Inv_Manager.instance.GetDecorBoxPrice(index) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected];
		}
		else if (this.shopCatIndexSelected == 3)
		{
			index = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.wallPaint_Prefabs[this.shopIndexSelected].gameObject);
			num = Inv_Manager.instance.GetWallPaintBoxPrice(index) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected];
		}
		else if (this.shopCatIndexSelected == 4)
		{
			index = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.floor_Prefabs[this.shopIndexSelected].gameObject);
			num = Inv_Manager.instance.GetFloorBoxPrice(index) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected];
		}
		else if (this.shopCatIndexSelected == 5)
		{
			index = Inv_Manager.instance.GetItemIndex(Inv_Manager.instance.decor_Prefabs[this.shopIndexSelected].gameObject);
			num = Inv_Manager.instance.GetUtilBoxPrice(index) * Inv_Manager.instance.prod_deliveryPrice_Available[this.shopSupIndexSelected];
		}
		num = (float)Mathf.FloorToInt(num);
		if (Finances_Manager.instance.GetMoney() >= num)
		{
			int qnt = Inv_Manager.instance.boxSize;
			if (this.shopCatIndexSelected == 1 || this.shopCatIndexSelected == 2 || this.shopCatIndexSelected == 5)
			{
				qnt = Inv_Manager.instance.boxSize_ShelvesAndDecor;
			}
			if (Inv_Manager.instance.AddDelivery(index, this.shopCatIndexSelected, qnt, false, this.shopSupIndexSelected))
			{
				Finances_Manager.instance.AddMoney(-num);
				if (this.shopCatIndexSelected == 0)
				{
					Finances_Manager.instance.AddTo_OutProds(num);
				}
				else
				{
					Finances_Manager.instance.AddTo_OutFurniture(num);
				}
				if (this.shopCatIndexSelected == 0)
				{
					Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.buy_Prod, -1);
				}
				if (this.shopCatIndexSelected == 1)
				{
					Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.buy_shelf, -1);
				}
				if (this.shopCatIndexSelected == 2)
				{
					Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.buy_decor, -1);
				}
				if (this.shopCatIndexSelected == 3)
				{
					Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.buy_wall, -1);
				}
				if (this.shopCatIndexSelected == 4)
				{
					Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.buy_floor, -1);
				}
				if (this.shopCatIndexSelected == 5)
				{
					Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.buy_util, -1);
				}
			}
			this.RefreshTomorrowDelivery();
			this.RefreshShopDescription(true);
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_AddItem);
			return;
		}
		Menu_Manager.instance.SetNotification("Attention!", "Insufficient Coins", true);
		this.RefreshShopDescription(true);
		Menu_Manager.instance.AnimateMoneyUI(false);
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x0002C035 File Offset: 0x0002A235
	public void ChangeBoxSize()
	{
		Menu_Manager.instance.SetWarning("Box Size", "Larger boxes have a price discount, but be aware of products expiration date.\nPay attention to your demand before buying them.\n\n(Not available in this build)", "PC", true, Menu_Manager.instance.specific_player_index);
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x0002C05B File Offset: 0x0002A25B
	public void DeleteLastDeliveryProd()
	{
		Inv_Manager.instance.DeleteLastDeliveryProd();
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_RemoveItem);
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0002C07B File Offset: 0x0002A27B
	public void Refresh_PC_Controller()
	{
		if (this.pc_controller != null)
		{
			this.pc_controller.RefreshPCTexture();
		}
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x0002C098 File Offset: 0x0002A298
	public void RefreshHints()
	{
		string scheme = Input_Manager.instance.GetScheme(-1);
		if (this.tabIndexSelected == 1)
		{
			this.hintJoystick_DecreasePrice.SetActive(false);
			this.hintJoystick_IncreasePrice.SetActive(false);
			return;
		}
		if (this.tabIndexSelected == 2)
		{
			if (scheme == "Joystick")
			{
				this.hintJoystick_DecreasePrice.SetActive(true);
				this.hintJoystick_IncreasePrice.SetActive(true);
				return;
			}
		}
		else
		{
			this.hintJoystick_DecreasePrice.SetActive(false);
			this.hintJoystick_IncreasePrice.SetActive(false);
		}
	}

	// Token: 0x040004B1 RID: 1201
	public static PC_Manager instance;

	// Token: 0x040004B2 RID: 1202
	public Interaction_Controller pc_controller;

	// Token: 0x040004B3 RID: 1203
	private List<string> tabNames = new List<string>
	{
		"Taiga News",
		"Shop",
		"Inventory",
		"Finances",
		"Manage",
		"Customers",
		"Missions & Rewards",
		"Expand",
		"Mail"
	};

	// Token: 0x040004B4 RID: 1204
	public int tabIndexSelected;

	// Token: 0x040004B5 RID: 1205
	private int shopCatIndexSelected;

	// Token: 0x040004B6 RID: 1206
	private int prodCatIndexSelected;

	// Token: 0x040004B7 RID: 1207
	private int shopIndexSelected;

	// Token: 0x040004B8 RID: 1208
	private int shopBoxSizeSelected;

	// Token: 0x040004B9 RID: 1209
	private int shopSupIndexSelected;

	// Token: 0x040004BA RID: 1210
	private int invIndexSelected;

	// Token: 0x040004BB RID: 1211
	[SerializeField]
	public int lastTabSelected = 8;

	// Token: 0x040004BC RID: 1212
	[SerializeField]
	private GameObject pc_background_frame;

	// Token: 0x040004BD RID: 1213
	[Header("Tabs")]
	[SerializeField]
	private Canvas canvas_PC;

	// Token: 0x040004BE RID: 1214
	[SerializeField]
	private Color[] tab_Colors;

	// Token: 0x040004BF RID: 1215
	[SerializeField]
	private Animator tab_NamePanel;

	// Token: 0x040004C0 RID: 1216
	[SerializeField]
	private Image tab_Image;

	// Token: 0x040004C1 RID: 1217
	[SerializeField]
	private Text tab_NameText;

	// Token: 0x040004C2 RID: 1218
	[SerializeField]
	private List<GameObject> tab_GOs;

	// Token: 0x040004C3 RID: 1219
	[SerializeField]
	private List<GameObject> tab_Panels;

	// Token: 0x040004C4 RID: 1220
	[Header("Shop Categories")]
	[SerializeField]
	private Color[] shopCat_Colors;

	// Token: 0x040004C5 RID: 1221
	[SerializeField]
	private GameObject shopCat_NamePanel;

	// Token: 0x040004C6 RID: 1222
	[SerializeField]
	private Text shopCat_NameText;

	// Token: 0x040004C7 RID: 1223
	[SerializeField]
	private List<Image> shopCat_GOs;

	// Token: 0x040004C8 RID: 1224
	[SerializeField]
	private List<Image> prodCat_GOs;

	// Token: 0x040004CA RID: 1226
	private List<string> prodCatNames = new List<string>
	{
		"all products",
		"fruits & vegetables",
		"food",
		"tools",
		"hygiene",
		"electronics",
		"sport",
		"events"
	};

	// Token: 0x040004CB RID: 1227
	private List<List<Inv_Manager.ProdType>> prodCatTypes = new List<List<Inv_Manager.ProdType>>();

	// Token: 0x040004CC RID: 1228
	[SerializeField]
	private ScrollRectAutoScroll shop_scroll_rect_auto_scroll;

	// Token: 0x040004CD RID: 1229
	[Header("Shop Events")]
	[SerializeField]
	public List<Sprite> eventCat_Sprites = new List<Sprite>();

	// Token: 0x040004CF RID: 1231
	[SerializeField]
	public GameObject eventCat_GO;

	// Token: 0x040004D0 RID: 1232
	[SerializeField]
	public Image eventCat_Image;

	// Token: 0x040004D1 RID: 1233
	[Header("Shop Items")]
	public bool shop_may_buy;

	// Token: 0x040004D2 RID: 1234
	public int shop_selected_index = -1;

	// Token: 0x040004D3 RID: 1235
	[SerializeField]
	public Color[] shop_UnlockedColors = new Color[2];

	// Token: 0x040004D4 RID: 1236
	[SerializeField]
	private GameObject shop_Button_Prefab;

	// Token: 0x040004D5 RID: 1237
	private List<GameObject> shop_Button_GOs = new List<GameObject>();

	// Token: 0x040004D6 RID: 1238
	private List<Button> shop_Button_Buttons = new List<Button>();

	// Token: 0x040004D7 RID: 1239
	private List<Image> shop_Button_GOs_Image = new List<Image>();

	// Token: 0x040004D8 RID: 1240
	private List<Image> shop_Button_ItemNamePanel = new List<Image>();

	// Token: 0x040004D9 RID: 1241
	private List<Text> shop_Button_ItemText = new List<Text>();

	// Token: 0x040004DA RID: 1242
	private List<Image> shop_Button_ItemImage = new List<Image>();

	// Token: 0x040004DB RID: 1243
	private List<Text> shop_Button_ItemPrice = new List<Text>();

	// Token: 0x040004DC RID: 1244
	private List<Image> shop_Button_NeedsRefrigerator = new List<Image>();

	// Token: 0x040004DD RID: 1245
	[Header("Shop Delivery")]
	[SerializeField]
	private List<Sprite> shop_Deliv_Sup_Sprites = new List<Sprite>();

	// Token: 0x040004DE RID: 1246
	[SerializeField]
	private Color shop_Deliv_NoColor;

	// Token: 0x040004DF RID: 1247
	[SerializeField]
	private Image shop_Deliv_Prefab;

	// Token: 0x040004E0 RID: 1248
	private List<Image> shop_Deliv_ItemPanel = new List<Image>();

	// Token: 0x040004E1 RID: 1249
	private List<Image> shop_Deliv_ItemImage = new List<Image>();

	// Token: 0x040004E2 RID: 1250
	private List<Image> shop_Deliv_QntImage = new List<Image>();

	// Token: 0x040004E3 RID: 1251
	private List<Text> shop_Deliv_QntText = new List<Text>();

	// Token: 0x040004E4 RID: 1252
	private List<Image> shop_Deliv_SupPanel = new List<Image>();

	// Token: 0x040004E5 RID: 1253
	private List<Image> shop_Deliv_SupImage = new List<Image>();

	// Token: 0x040004E6 RID: 1254
	private List<Text> shop_Deliv_SupText = new List<Text>();

	// Token: 0x040004E7 RID: 1255
	[SerializeField]
	private GameObject shop_Deliv_DeleteButton;

	// Token: 0x040004E8 RID: 1256
	[Header("Shop Description")]
	[SerializeField]
	private Image shop_Desc_Panel;

	// Token: 0x040004E9 RID: 1257
	[SerializeField]
	private Image shop_Desc_Panel_Off;

	// Token: 0x040004EA RID: 1258
	[SerializeField]
	private Text shop_Desc_ItemName;

	// Token: 0x040004EB RID: 1259
	[SerializeField]
	private GameObject shop_Desc_NeedsRefrigerator;

	// Token: 0x040004EC RID: 1260
	[SerializeField]
	private Image shop_Desc_ItemImage;

	// Token: 0x040004ED RID: 1261
	[SerializeField]
	private Text shop_Desc_BoxSize;

	// Token: 0x040004EE RID: 1262
	[SerializeField]
	private Text shop_Desc_Price;

	// Token: 0x040004EF RID: 1263
	[SerializeField]
	private GameObject shop_Desc_NoMoney;

	// Token: 0x040004F0 RID: 1264
	[SerializeField]
	private Text shop_Desc_QntInv_Text;

	// Token: 0x040004F1 RID: 1265
	[SerializeField]
	private GameObject shop_Desc_QntSale_Panel;

	// Token: 0x040004F2 RID: 1266
	[SerializeField]
	private Text shop_Desc_QntSale_Text;

	// Token: 0x040004F3 RID: 1267
	[SerializeField]
	private GameObject shop_Desc_PanelDown;

	// Token: 0x040004F4 RID: 1268
	[Header("Shop Suppliers")]
	[SerializeField]
	private List<Image> shop_Sup_Buttons = new List<Image>();

	// Token: 0x040004F5 RID: 1269
	[SerializeField]
	private List<Color> shop_Sup_Colors = new List<Color>();

	// Token: 0x040004F6 RID: 1270
	[Header("Finances")]
	[SerializeField]
	private GameObject fin_PanelDataPrefab;

	// Token: 0x040004F7 RID: 1271
	private List<GameObject> fin_GOs = new List<GameObject>();

	// Token: 0x040004F8 RID: 1272
	private List<Text> fin_TextTitle = new List<Text>();

	// Token: 0x040004F9 RID: 1273
	private List<Text> fin_TextProds = new List<Text>();

	// Token: 0x040004FA RID: 1274
	private List<Text> fin_TextFurniture = new List<Text>();

	// Token: 0x040004FB RID: 1275
	private List<Text> fin_TextStaff = new List<Text>();

	// Token: 0x040004FC RID: 1276
	private List<Text> fin_TextOperational = new List<Text>();

	// Token: 0x040004FD RID: 1277
	private List<Text> fin_TextExpansion = new List<Text>();

	// Token: 0x040004FE RID: 1278
	private List<Text> fin_TextMarketing = new List<Text>();

	// Token: 0x040004FF RID: 1279
	private List<Text> fin_TextSales = new List<Text>();

	// Token: 0x04000500 RID: 1280
	private List<Text> fin_TextPrizes = new List<Text>();

	// Token: 0x04000501 RID: 1281
	private List<Text> fin_TextBalance = new List<Text>();

	// Token: 0x04000502 RID: 1282
	[Header("PC Tabs")]
	[SerializeField]
	private Color[] pC_Tabs_Colors;

	// Token: 0x04000503 RID: 1283
	[SerializeField]
	private Image pC_TabSelected_Image;

	// Token: 0x04000504 RID: 1284
	[SerializeField]
	private List<Button> pC_TabSelected_StartButtons = new List<Button>();

	// Token: 0x04000505 RID: 1285
	private int shopIndexJoystick;

	// Token: 0x04000506 RID: 1286
	private float[] shopcatavailable = new float[]
	{
		0f,
		0.1f,
		0.2f,
		0.3f,
		0.4f,
		0.5f,
		0.6f,
		0.7f,
		1f,
		2f,
		3f,
		4f,
		5f
	};

	// Token: 0x04000507 RID: 1287
	[Header("Staff")]
	[SerializeField]
	private Button staff_Button_GetApplicants;

	// Token: 0x04000508 RID: 1288
	[SerializeField]
	private GameObject staff_Panel_WaitEmail;

	// Token: 0x04000509 RID: 1289
	[SerializeField]
	private List<GameObject> staff_Panel_No_Staff = new List<GameObject>();

	// Token: 0x0400050A RID: 1290
	[SerializeField]
	private List<GameObject> staff_MainPanel = new List<GameObject>();

	// Token: 0x0400050B RID: 1291
	[SerializeField]
	private List<Text> staff_Name_Text = new List<Text>();

	// Token: 0x0400050C RID: 1292
	[SerializeField]
	private List<Image> staff_Energy = new List<Image>();

	// Token: 0x0400050D RID: 1293
	[SerializeField]
	private List<GameObject> staff_Panel_Off = new List<GameObject>();

	// Token: 0x0400050E RID: 1294
	[SerializeField]
	private List<GameObject> staff_Button_GiveDaysOff = new List<GameObject>();

	// Token: 0x0400050F RID: 1295
	[SerializeField]
	private List<GameObject> staff_Button_Fire = new List<GameObject>();

	// Token: 0x04000510 RID: 1296
	[SerializeField]
	private List<Text> staff_price = new List<Text>();

	// Token: 0x04000511 RID: 1297
	[SerializeField]
	private List<List<Image>> staff_Tasks_Image = new List<List<Image>>();

	// Token: 0x04000512 RID: 1298
	[SerializeField]
	private List<List<Image>> staff_Skills_Image = new List<List<Image>>();

	// Token: 0x04000513 RID: 1299
	[SerializeField]
	private List<List<Button>> staff_Tasks_Buttons = new List<List<Button>>();

	// Token: 0x04000514 RID: 1300
	[SerializeField]
	private GameObject manage_panel_locations;

	// Token: 0x04000515 RID: 1301
	[SerializeField]
	private List<Image> news_ClimateImages = new List<Image>();

	// Token: 0x04000516 RID: 1302
	[SerializeField]
	private Image news_WeekCurrentDay;

	// Token: 0x04000517 RID: 1303
	[SerializeField]
	public List<GameObject> news_WeekDays = new List<GameObject>();

	// Token: 0x04000518 RID: 1304
	private List<Text> staff_Text_OutToday = new List<Text>();

	// Token: 0x04000519 RID: 1305
	[Header("Expand")]
	[SerializeField]
	public Button news_Expand_Button;

	// Token: 0x0400051A RID: 1306
	[SerializeField]
	public List<GameObject> news_Expand_Expanded = new List<GameObject>();

	// Token: 0x0400051B RID: 1307
	[SerializeField]
	public List<GameObject> news_Expand_Unavailable = new List<GameObject>();

	// Token: 0x0400051C RID: 1308
	[SerializeField]
	private GameObject news_Expand_CompletedPanel;

	// Token: 0x0400051D RID: 1309
	[SerializeField]
	private List<Button> manage_panel_locations_buttons = new List<Button>();

	// Token: 0x0400051E RID: 1310
	[Header("Locals")]
	[SerializeField]
	private List<Button> cust_Buttons = new List<Button>();

	// Token: 0x0400051F RID: 1311
	[SerializeField]
	private Image cust_Desc_Name_Panel;

	// Token: 0x04000520 RID: 1312
	[SerializeField]
	private Text cust_Desc_Name_Text;

	// Token: 0x04000521 RID: 1313
	[SerializeField]
	private Image cust_Desc_Image;

	// Token: 0x04000522 RID: 1314
	[SerializeField]
	private Button cust_Button;

	// Token: 0x04000523 RID: 1315
	[SerializeField]
	private List<Image> cust_Predilections_Images = new List<Image>();

	// Token: 0x04000524 RID: 1316
	[SerializeField]
	private GameObject cust_LifeAchievement_PanelImage;

	// Token: 0x04000525 RID: 1317
	[SerializeField]
	private GameObject cust_LifeAchievement_PanelInfo;

	// Token: 0x04000526 RID: 1318
	[SerializeField]
	private Image cust_LifeAchievement_Photo;

	// Token: 0x04000527 RID: 1319
	[SerializeField]
	private Image cust_Desc_Affection_Image;

	// Token: 0x04000528 RID: 1320
	[SerializeField]
	private Image cust_Desc_Friendship_Image;

	// Token: 0x04000529 RID: 1321
	[Header("Mail")]
	[SerializeField]
	private Color mail_cat_off_color;

	// Token: 0x0400052A RID: 1322
	[SerializeField]
	private List<Color> mail_cat_colors = new List<Color>();

	// Token: 0x0400052B RID: 1323
	[SerializeField]
	private List<Color> mail_info_colors = new List<Color>();

	// Token: 0x0400052C RID: 1324
	[SerializeField]
	private List<Color> mail_background_colors = new List<Color>();

	// Token: 0x0400052D RID: 1325
	[SerializeField]
	private Button mail_button_ref;

	// Token: 0x0400052E RID: 1326
	private Transform mail_mails_list_parent;

	// Token: 0x0400052F RID: 1327
	private List<PC_Manager.Mail_Class> mail_class_list = new List<PC_Manager.Mail_Class>();

	// Token: 0x04000530 RID: 1328
	public int mail_cat_index;

	// Token: 0x04000531 RID: 1329
	public int mail_mail_index;

	// Token: 0x04000532 RID: 1330
	public PC_Manager.Mail_Tab mail_tab = new PC_Manager.Mail_Tab();

	// Token: 0x04000533 RID: 1331
	public Image mail_image_alert_taskbar;

	// Token: 0x04000534 RID: 1332
	[Header("Hints")]
	[SerializeField]
	private GameObject hintJoystick_IncreasePrice;

	// Token: 0x04000535 RID: 1333
	[SerializeField]
	private GameObject hintJoystick_DecreasePrice;

	// Token: 0x02000089 RID: 137
	private class Mail_Class
	{
		// Token: 0x060005CC RID: 1484 RVA: 0x00034B2C File Offset: 0x00032D2C
		public void Create_References(int _index)
		{
			this.button = UnityEngine.Object.Instantiate<GameObject>(PC_Manager.instance.mail_button_ref.gameObject).GetComponent<Button>();
			this.button.onClick.AddListener(delegate()
			{
				PC_Manager.instance.Mail_SelectMail(_index);
			});
			this.image_unread = this.button.transform.Find("Image_Unread").GetComponent<Image>();
			this.image_header = this.button.transform.Find("Image_Accent_Color").GetComponent<Image>();
			this.image_character = this.button.transform.Find("Image_Character").GetComponent<Image>();
			this.text_title = this.button.transform.Find("Text_Title").GetComponent<Text>();
			this.text_message = this.button.transform.Find("Text_Message").GetComponent<Text>();
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00034C24 File Offset: 0x00032E24
		public void Set_Data(Mail_Data _data)
		{
			this.data = _data;
			this.text_title.text = Language_Manager.instance.GetText(this.data.title);
			bool[] translating = new bool[]
			{
				true,
				true,
				true,
				true,
				true,
				true,
				true
			};
			this.text_message.text = Language_Manager.instance.GetText(this.data.message, _data.task_tag, _data.task_tag_value, translating);
			this.image_header.color = PC_Manager.instance.mail_cat_colors[this.data.category];
			this.image_character.sprite = Char_Manager.instance.Get_Char_Sprite(_data.owner_cat, _data.owner_index);
			this.image_character.SetNativeSize();
			if (this.image_character != null)
			{
				this.image_character.gameObject.SetActive(true);
			}
			else
			{
				this.image_character.gameObject.SetActive(false);
			}
			this.Refresh();
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00034D20 File Offset: 0x00032F20
		public void Refresh()
		{
			if (this.data == null)
			{
				return;
			}
			this.image_unread.gameObject.SetActive(!this.data.opened);
			if (this.data.category == 3)
			{
				if (this.data.task_state == 0)
				{
					this.text_title.text = "Task";
					return;
				}
				if (this.data.task_state == 1)
				{
					this.text_title.text = "Task - Active";
					return;
				}
				if (this.data.task_state == 2)
				{
					this.text_title.text = "Task - Collect Reward";
					return;
				}
				this.text_title.text = "Task - Finished";
			}
		}

		// Token: 0x040006F1 RID: 1777
		public Mail_Data data;

		// Token: 0x040006F2 RID: 1778
		public Button button;

		// Token: 0x040006F3 RID: 1779
		public Image image_unread;

		// Token: 0x040006F4 RID: 1780
		public Image image_header;

		// Token: 0x040006F5 RID: 1781
		public Image image_character;

		// Token: 0x040006F6 RID: 1782
		public Text text_title;

		// Token: 0x040006F7 RID: 1783
		public Text text_message;
	}

	// Token: 0x0200008A RID: 138
	[Serializable]
	public class Mail_Tab
	{
		// Token: 0x060005D0 RID: 1488 RVA: 0x00034DD8 File Offset: 0x00032FD8
		public void Set_Data(Mail_Data _data)
		{
			if (_data == null)
			{
				_data = new Mail_Data();
				this.image_no_mail.gameObject.SetActive(true);
			}
			else
			{
				this.image_no_mail.gameObject.SetActive(false);
			}
			this.data = _data;
			this.text_date.text = Char_Manager.instance.Get_Char_Name(_data.owner_cat, _data.owner_index);
			this.text_title.text = Language_Manager.instance.GetText(this.data.title);
			bool[] translating = new bool[]
			{
				true,
				true,
				true,
				true,
				true,
				true
			};
			this.text_message.text = Language_Manager.instance.GetText(this.data.message, _data.task_tag, _data.task_tag_value, translating);
			this.image_header.color = PC_Manager.instance.mail_cat_colors[this.data.category];
			this.image_background.color = PC_Manager.instance.mail_info_colors[this.data.category];
			this.image_character.sprite = Char_Manager.instance.Get_Char_Sprite(_data.owner_cat, _data.owner_index);
			this.image_character.SetNativeSize();
			if (this.image_character != null)
			{
				this.image_character.gameObject.SetActive(true);
			}
			else
			{
				this.image_character.gameObject.SetActive(false);
			}
			this.image_mail_list_background.color = PC_Manager.instance.mail_background_colors[PC_Manager.instance.mail_cat_index];
			this.image_mail_list_handle.color = PC_Manager.instance.mail_background_colors[PC_Manager.instance.mail_cat_index];
			this.image_no_mail.color = PC_Manager.instance.mail_info_colors[PC_Manager.instance.mail_cat_index];
			if (this.data.category != 3)
			{
				if (this.data.has_staff_data)
				{
					Staff_Data staff_data = this.data.staff_data;
					this.go_panel_staff.gameObject.SetActive(true);
					this.go_panel_staff.transform.Find("Text_Name").GetComponent<Text>().text = staff_data.name;
					Transform transform = this.go_panel_staff.transform.Find("Panel_Skills");
					transform.Find("Panel_Price").Find("Text").GetComponent<Text>().text = staff_data.price.ToString();
					for (int i = 0; i < 3; i++)
					{
						transform.Find("Layout").Find("Skill (" + i.ToString() + ")").Find("Image").Find("Image").GetComponent<Image>().fillAmount = staff_data.skills[i];
					}
					this.image_button_hire_staff.gameObject.SetActive(true);
				}
				else
				{
					this.go_panel_staff.gameObject.SetActive(false);
					this.image_button_hire_staff.gameObject.SetActive(false);
				}
				this.iamge_button_collect_reward.gameObject.SetActive(false);
				this.image_panel_rewards.gameObject.SetActive(false);
				return;
			}
			this.go_panel_staff.gameObject.SetActive(false);
			this.image_button_hire_staff.gameObject.SetActive(false);
			this.iamge_button_collect_reward.gameObject.SetActive(false);
			this.image_panel_rewards.gameObject.SetActive(true);
			for (int j = 0; j < this.image_rewards_images.Count; j++)
			{
				if (j < _data.reward_indexes.Count)
				{
					this.image_rewards_images[j].gameObject.SetActive(true);
					int cat = _data.reward_cats[j];
					int index = _data.reward_indexes[j];
					this.image_rewards_images[j].sprite = Unlock_Manager.instance.Set_ItemUnlockState(cat, index, true, false, true);
				}
				else
				{
					this.image_rewards_images[j].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x040006F8 RID: 1784
		public List<Button> category_buttons = new List<Button>();

		// Token: 0x040006F9 RID: 1785
		public Mail_Data data;

		// Token: 0x040006FA RID: 1786
		public Image image_header;

		// Token: 0x040006FB RID: 1787
		public Image image_background;

		// Token: 0x040006FC RID: 1788
		public Image image_character;

		// Token: 0x040006FD RID: 1789
		public Text text_date;

		// Token: 0x040006FE RID: 1790
		public Text text_title;

		// Token: 0x040006FF RID: 1791
		public Text text_message;

		// Token: 0x04000700 RID: 1792
		public Image image_no_mail;

		// Token: 0x04000701 RID: 1793
		public Image image_mail_list_background;

		// Token: 0x04000702 RID: 1794
		public Image image_mail_list_handle;

		// Token: 0x04000703 RID: 1795
		public Image image_button_hire_staff;

		// Token: 0x04000704 RID: 1796
		public GameObject go_panel_staff;

		// Token: 0x04000705 RID: 1797
		public Image iamge_button_collect_reward;

		// Token: 0x04000706 RID: 1798
		public Image image_panel_rewards;

		// Token: 0x04000707 RID: 1799
		public List<Image> image_rewards_images = new List<Image>();
	}
}
