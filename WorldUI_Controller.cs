using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000035 RID: 53
public class WorldUI_Controller : MonoBehaviour
{
	// Token: 0x060001BF RID: 447 RVA: 0x00010BF8 File Offset: 0x0000EDF8
	public void _Awake()
	{
		this.int_Simple.gameObject.SetActive(false);
		this.int_Build.gameObject.SetActive(false);
		this.int_Place.gameObject.SetActive(false);
		for (int i = 0; i < 3; i++)
		{
			if (i > 0)
			{
				this.int_Prod.Add(UnityEngine.Object.Instantiate<RectTransform>(this.int_Prod[0]));
				this.int_Prod[i].transform.SetParent(this.int_Prod[0].transform.parent);
				this.int_Prod[i].transform.localScale = Vector3.one;
			}
			this.int_Prod[i].gameObject.name = "InteractorProd - " + i.ToString();
			Transform transform = this.int_Prod[i].transform.Find("Master");
			Transform transform2 = transform.Find("Panel_Prod");
			this.int_PanelProd.Add(transform2.gameObject);
			this.int_Prod_ItemImage.Add(transform2.transform.Find("Image_Item").GetComponent<Image>());
			this.int_Prod_PanelName.Add(transform2.transform.Find("Panel_ItemName").GetComponent<Image>());
			this.int_Prod_ItemName.Add(transform2.transform.Find("Text_ItemName").GetComponent<Text>());
			this.int_Prod_PanelQnt.Add(transform2.transform.Find("Panel_ItemQnt").GetComponent<Image>());
			this.int_Prod_ItemPrice.Add(transform2.transform.Find("Panel_Price").Find("Text_Price").GetComponent<Text>());
			this.int_Prod_ItemQnt.Add(this.int_Prod_PanelQnt[i].transform.Find("Text_ItemQnt").GetComponent<Text>());
			this.int_Prod_RefrigeratedImage.Add(transform2.transform.Find("Image_Refrigerated").GetComponent<Image>());
			this.int_Prod_InputHint.Add(transform.transform.Find("Image_Hint").GetComponent<InputHint_Controller>());
			this.int_Prod_InputHint[i].sprite_Joystick = this.int_Prod_HintSprite_Joystick[i];
			this.int_Prod_InputHint[i].sprite_Keyboard = this.int_Prod_HintSprite_Keyboard[i];
			this.int_Prod_StoringImage.Add(transform2.transform.Find("Image_Storing").GetComponent<Image>());
			this.int_Prod_DiscountPanel.Add(this.int_PanelProd[i].transform.Find("Panel_Discount").GetComponent<Image>());
			this.int_Prod_DiscountArrow.Add(this.int_Prod_DiscountPanel[i].transform.Find("Image_Arrow").GetComponent<Image>());
			this.int_Prod_DiscountPanelOff.Add(this.int_Prod_DiscountPanel[i].transform.Find("Panel_Off").GetComponent<Image>());
			this.int_Prod_DiscountValue.Add(this.int_Prod_DiscountPanelOff[i].transform.Find("Text_Value").GetComponent<Text>());
			this.int_Prod_DiscountPrice.Add(this.int_Prod_DiscountPanel[i].transform.Find("Panel_Price").Find("Text_Price").GetComponent<Text>());
			this.int_ShelfDiscount.Add(transform.Find("Panel_ShelfDiscount").gameObject);
			this.int_ShelfDiscountRemove.Add(this.int_ShelfDiscount[i].transform.Find("Image_Remove").gameObject);
			this.int_Prod_LifeSpanPanel.Add(this.int_PanelProd[i].transform.Find("Panel_LifeSpan").GetComponent<Image>());
			this.int_Prod_LifeSpanImage.Add(this.int_Prod_LifeSpanPanel[i].transform.Find("Image").GetComponent<Image>());
			this.int_Prod[i].gameObject.SetActive(false);
		}
		this.int_BoxQnt_PanelQnt.Add(this.int_BoxQnt.transform.Find("Master").transform.Find("Panel_ItemQnt").GetComponent<Image>());
		this.int_BoxQnt_PanelQnt.Add(this.int_BoxQnt_PanelQnt[0].transform.Find("Image").GetComponent<Image>());
		this.int_BoxQnt_ItemQnt = this.int_BoxQnt_PanelQnt[0].transform.Find("Text_ItemQnt").GetComponent<Text>();
		this.int_BoxQnt_ItemSprite = this.int_BoxQnt_PanelQnt[0].transform.Find("Image_ItemSprite").GetComponent<Image>();
		for (int j = 0; j < 4; j++)
		{
			this.int_DiscNew_Options.Add(this.int_DiscNew.transform.Find("Master").Find("Panel_Options").Find("Panel_Discount (" + j.ToString() + ")").GetComponent<Image>());
			this.int_DiscNew_OptionsOff.Add(this.int_DiscNew_Options[j].transform.Find("Image").GetComponent<Image>());
		}
		for (int k = 0; k < 3; k++)
		{
			if (k > 0)
			{
				this.int_Discount.Add(UnityEngine.Object.Instantiate<RectTransform>(this.int_Discount[0]));
				this.int_Discount[k].transform.SetParent(this.int_Discount[0].transform.parent);
				this.int_Discount[k].transform.localScale = Vector3.one;
			}
			Transform transform3 = this.int_Discount[k].transform.Find("Master");
			Transform transform4 = transform3.Find("Panel_Discount");
			this.int_PanelDiscount.Add(transform4.GetComponent<Image>());
			this.int_Discount_PanelOff.Add(transform4.Find("Panel_Off").GetComponent<Image>());
			this.int_Discount_Value.Add(transform4.Find("Text_Value").GetComponent<Text>());
			this.int_Discount_Percentage.Add(transform4.Find("Text_Percentage").GetComponent<Text>());
			this.int_Discount_Off.Add(this.int_Discount_PanelOff[k].transform.Find("Text_Off").GetComponent<Text>());
			this.int_Prod_Discount_InputHint.Add(transform3.transform.Find("Image_Hint").GetComponent<InputHint_Controller>());
			this.int_Prod_Discount_InputHint[k].sprite_Joystick = this.int_Prod_HintSprite_Joystick[k];
			this.int_Prod_Discount_InputHint[k].sprite_Keyboard = this.int_Prod_HintSprite_Keyboard[k];
		}
		for (int l = 0; l < this.int_Cashier_Simple.Count; l++)
		{
			this.int_Cashier_ButtonHint.Add(this.int_Cashier_Simple[l].transform.Find("Master").Find("Panel_SimpleInteraction").Find("Image_Hint").GetComponent<Image>());
		}
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x00011328 File Offset: 0x0000F528
	public void _Start()
	{
		for (int i = 0; i < 3; i++)
		{
			this.int_PanelDiscount[i].color = this.int_Discount_ColorBackground[i];
			this.int_Discount_PanelOff[i].color = this.int_Discount_ColorText[i];
			this.int_Discount_Value[i].color = this.int_Discount_ColorText[i];
			this.int_Discount_Value[i].text = Inv_Manager.instance.prod_DiscountValue[i].ToString();
			this.int_Discount_Percentage[i].color = this.int_Discount_ColorText[i];
			this.int_Discount_Off[i].color = this.int_Discount_ColorBackground[i];
		}
	}

	// Token: 0x04000251 RID: 593
	public int playerId;

	// Token: 0x04000252 RID: 594
	[SerializeField]
	public MiniGame_Manager miniGame_manager;

	// Token: 0x04000253 RID: 595
	[Header("Interactor Simple")]
	[SerializeField]
	public RectTransform int_Simple;

	// Token: 0x04000254 RID: 596
	[SerializeField]
	public GameObject int_Simple_NeedTools;

	// Token: 0x04000255 RID: 597
	[Header("Interactor Hints")]
	[SerializeField]
	public RectTransform int_Hints;

	// Token: 0x04000256 RID: 598
	[SerializeField]
	public GameObject int_Hint_Leave;

	// Token: 0x04000257 RID: 599
	[SerializeField]
	public GameObject int_Hint_Drop;

	// Token: 0x04000258 RID: 600
	[SerializeField]
	public GameObject int_Hint_ChangeItem;

	// Token: 0x04000259 RID: 601
	[SerializeField]
	public GameObject int_Hint_Run;

	// Token: 0x0400025A RID: 602
	[Header("Interactor Build")]
	[SerializeField]
	public RectTransform int_Build;

	// Token: 0x0400025B RID: 603
	[SerializeField]
	public Text int_Build_Name;

	// Token: 0x0400025C RID: 604
	[Header("Interactor Place")]
	[SerializeField]
	public RectTransform int_Place;

	// Token: 0x0400025D RID: 605
	[Header("Interactor Prods")]
	[SerializeField]
	public List<Sprite> int_Prod_StoringSprites = new List<Sprite>();

	// Token: 0x0400025E RID: 606
	[SerializeField]
	public List<RectTransform> int_Prod = new List<RectTransform>();

	// Token: 0x0400025F RID: 607
	[HideInInspector]
	public List<GameObject> int_PanelProd = new List<GameObject>();

	// Token: 0x04000260 RID: 608
	[HideInInspector]
	public List<Image> int_Prod_ItemImage = new List<Image>();

	// Token: 0x04000261 RID: 609
	[HideInInspector]
	public List<Image> int_Prod_PanelName = new List<Image>();

	// Token: 0x04000262 RID: 610
	[HideInInspector]
	public List<Text> int_Prod_ItemName = new List<Text>();

	// Token: 0x04000263 RID: 611
	[HideInInspector]
	public List<Image> int_Prod_PanelQnt = new List<Image>();

	// Token: 0x04000264 RID: 612
	[HideInInspector]
	public List<Text> int_Prod_ItemPrice = new List<Text>();

	// Token: 0x04000265 RID: 613
	[HideInInspector]
	public List<Text> int_Prod_ItemQnt = new List<Text>();

	// Token: 0x04000266 RID: 614
	[HideInInspector]
	public List<Image> int_Prod_RefrigeratedImage = new List<Image>();

	// Token: 0x04000267 RID: 615
	[HideInInspector]
	public List<Image> int_Prod_StoringImage = new List<Image>();

	// Token: 0x04000268 RID: 616
	[HideInInspector]
	public List<Image> int_Prod_DiscountPanel = new List<Image>();

	// Token: 0x04000269 RID: 617
	[HideInInspector]
	public List<Image> int_Prod_DiscountArrow = new List<Image>();

	// Token: 0x0400026A RID: 618
	[HideInInspector]
	public List<Image> int_Prod_DiscountPanelOff = new List<Image>();

	// Token: 0x0400026B RID: 619
	[HideInInspector]
	public List<Text> int_Prod_DiscountValue = new List<Text>();

	// Token: 0x0400026C RID: 620
	[HideInInspector]
	public List<Text> int_Prod_DiscountPrice = new List<Text>();

	// Token: 0x0400026D RID: 621
	[HideInInspector]
	public List<GameObject> int_ShelfDiscount = new List<GameObject>();

	// Token: 0x0400026E RID: 622
	[HideInInspector]
	public List<GameObject> int_ShelfDiscountRemove = new List<GameObject>();

	// Token: 0x0400026F RID: 623
	[Header("Prod Life Span")]
	[SerializeField]
	public List<Image> int_Prod_LifeSpanPanel = new List<Image>();

	// Token: 0x04000270 RID: 624
	[SerializeField]
	public List<Image> int_Prod_LifeSpanImage = new List<Image>();

	// Token: 0x04000271 RID: 625
	[SerializeField]
	public List<Sprite> int_Prod_LifeSpanSprites = new List<Sprite>();

	// Token: 0x04000272 RID: 626
	[Header("Interactor Box Qnt")]
	[SerializeField]
	public RectTransform int_BoxQnt;

	// Token: 0x04000273 RID: 627
	[HideInInspector]
	public List<Image> int_BoxQnt_PanelQnt = new List<Image>();

	// Token: 0x04000274 RID: 628
	[HideInInspector]
	public Text int_BoxQnt_ItemQnt;

	// Token: 0x04000275 RID: 629
	[HideInInspector]
	public Image int_BoxQnt_ItemSprite;

	// Token: 0x04000276 RID: 630
	[HideInInspector]
	public int int_BoxQnt_CurrentQnt = -1;

	// Token: 0x04000277 RID: 631
	[HideInInspector]
	public Color boxQntColor = Color.black;

	// Token: 0x04000278 RID: 632
	[Header("Interactor Discount NEW")]
	[SerializeField]
	public RectTransform int_DiscNew;

	// Token: 0x04000279 RID: 633
	[HideInInspector]
	public int int_DiscNew_Index = -99;

	// Token: 0x0400027A RID: 634
	[HideInInspector]
	public List<Image> int_DiscNew_Options = new List<Image>();

	// Token: 0x0400027B RID: 635
	[HideInInspector]
	public List<Image> int_DiscNew_OptionsOff = new List<Image>();

	// Token: 0x0400027C RID: 636
	[Header("Interactor Discount")]
	[SerializeField]
	public List<Color> int_Discount_ColorText = new List<Color>();

	// Token: 0x0400027D RID: 637
	[SerializeField]
	public List<Color> int_Discount_ColorBackground = new List<Color>();

	// Token: 0x0400027E RID: 638
	[SerializeField]
	public List<Color> int_Discount_ColorPriceBackground = new List<Color>();

	// Token: 0x0400027F RID: 639
	[SerializeField]
	public List<RectTransform> int_Discount = new List<RectTransform>();

	// Token: 0x04000280 RID: 640
	[HideInInspector]
	public List<Image> int_PanelDiscount = new List<Image>();

	// Token: 0x04000281 RID: 641
	[HideInInspector]
	public List<Image> int_Discount_PanelOff = new List<Image>();

	// Token: 0x04000282 RID: 642
	[HideInInspector]
	public List<Text> int_Discount_Value = new List<Text>();

	// Token: 0x04000283 RID: 643
	[HideInInspector]
	public List<Text> int_Discount_Percentage = new List<Text>();

	// Token: 0x04000284 RID: 644
	[HideInInspector]
	public List<Text> int_Discount_Off = new List<Text>();

	// Token: 0x04000285 RID: 645
	[HideInInspector]
	public List<InputHint_Controller> int_Prod_InputHint = new List<InputHint_Controller>();

	// Token: 0x04000286 RID: 646
	[HideInInspector]
	public List<InputHint_Controller> int_Prod_Discount_InputHint = new List<InputHint_Controller>();

	// Token: 0x04000287 RID: 647
	[Header("Other")]
	[SerializeField]
	public Sprite[] int_Prod_HintSprite_Joystick;

	// Token: 0x04000288 RID: 648
	[SerializeField]
	public Sprite[] int_Prod_HintSprite_Keyboard;

	// Token: 0x04000289 RID: 649
	[SerializeField]
	public GameObject currentPlacePoint;

	// Token: 0x0400028A RID: 650
	[HideInInspector]
	public Interaction_Controller oldInteractive;

	// Token: 0x0400028B RID: 651
	[Header("Cashier")]
	[SerializeField]
	public List<RectTransform> int_Cashier_Simple = new List<RectTransform>();

	// Token: 0x0400028C RID: 652
	[SerializeField]
	public RectTransform int_Cashier_Money;

	// Token: 0x0400028D RID: 653
	[SerializeField]
	public RectTransform int_Cashier_Tip;

	// Token: 0x0400028E RID: 654
	[SerializeField]
	public Animator int_Cashier_MoneyAnim;

	// Token: 0x0400028F RID: 655
	[SerializeField]
	public Animator int_Cashier_TipAnim;

	// Token: 0x04000290 RID: 656
	[SerializeField]
	public Text int_Cashier_MoneyText;

	// Token: 0x04000291 RID: 657
	[SerializeField]
	public Text int_Cashier_TipText;

	// Token: 0x04000292 RID: 658
	[HideInInspector]
	public List<Image> int_Cashier_ButtonHint = new List<Image>();

	// Token: 0x04000293 RID: 659
	[HideInInspector]
	public float int_Cashier_positionToAdd_y = 1.2f;

	// Token: 0x04000294 RID: 660
	[HideInInspector]
	public List<Cashier_Controller> cashier_Controllers = new List<Cashier_Controller>();
}
