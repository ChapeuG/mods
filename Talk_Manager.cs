using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000058 RID: 88
public class Talk_Manager : MonoBehaviour
{
	// Token: 0x060004CA RID: 1226 RVA: 0x0002FEB0 File Offset: 0x0002E0B0
	private void Awake()
	{
		if (!Talk_Manager.instance)
		{
			Talk_Manager.instance = this;
		}
		for (int i = 0; i < this.buttonAnswers_Buttons.Count; i++)
		{
			this.buttonAnswers_Texts.Add(this.buttonAnswers_Buttons[i].transform.Find("Text").GetComponent<Text>());
		}
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x0002FF10 File Offset: 0x0002E110
	public List<Button> Talk(string _charText, List<string> _answers, List<string> _actions, int _answerIndex, Char_Controller _controller, int _player_index)
	{
		Input_Manager.instance.SetCooldown(0, 1.8f);
		if (_controller.isCustomer)
		{
			this.panelLocals.SetActive(false);
			Customer_Controller component = _controller.GetComponent<Customer_Controller>();
			this.friendship_Image.gameObject.SetActive(Char_Manager.instance.GetCustomerLifeAchievementState(_controller.GetID()));
			List<Prod_Controller> list = new List<Prod_Controller>(component.GetProdPredilectionList());
			List<bool> list2 = new List<bool>(Char_Manager.instance.localCustomer_Datas[_controller.GetID()].prodPreferenceUnlocked);
			for (int i = 0; i < this.prodPreference_Images.Count; i++)
			{
				if (i < list.Count && list[i])
				{
					this.prodPreference_Images[i].gameObject.SetActive(true);
					Sprite sprite = Inv_Manager.instance.GetProdSprite(Inv_Manager.instance.GetItemIndex(list[i].gameObject));
					if (i < list2.Count && !list2[i])
					{
						sprite = Char_Manager.instance.prod_UnknownSprite;
					}
					this.prodPreference_Images[i].sprite = sprite;
					this.prodPreference_Images[i].SetNativeSize();
				}
				else
				{
					this.prodPreference_Images[i].gameObject.SetActive(false);
				}
			}
		}
		else
		{
			this.panelLocals.SetActive(false);
		}
		List<Button> list3 = new List<Button>();
		if (!_controller)
		{
			Menu_Manager.instance.SetMenuName("MainMenu");
			return list3;
		}
		this.currentTalkingChar = _controller;
		this.panel_Name.color = _controller.charColor;
		this.text_Name.text = _controller.charName;
		this.talk_Image.sprite = _controller.charSprite;
		this.talk_Image.SetNativeSize();
		this.text_Talk.text = _charText;
		UnityAction <>9__1;
		for (int j = 0; j < this.buttonAnswers_Buttons.Count; j++)
		{
			this.buttonAnswers_Buttons[j].onClick.RemoveAllListeners();
			if (j < _answers.Count)
			{
				this.buttonAnswers_Buttons[j].gameObject.SetActive(true);
				string _action = _actions[j];
				if (_action == "DestroyChar")
				{
					UnityEvent onClick = this.buttonAnswers_Buttons[j].onClick;
					UnityAction call;
					if ((call = <>9__1) == null)
					{
						call = (<>9__1 = delegate()
						{
							_controller.DestroyChar();
						});
					}
					onClick.AddListener(call);
				}
				else
				{
					this.buttonAnswers_Buttons[j].onClick.AddListener(delegate()
					{
						this.Invoke(_action, 0f);
					});
				}
				this.buttonAnswers_Texts[j].text = _answers[j] + "       ";
				list3.Add(this.buttonAnswers_Buttons[j]);
			}
			else
			{
				this.buttonAnswers_Buttons[j].gameObject.SetActive(false);
			}
		}
		if (_answers.Count == 0)
		{
			this.buttonAnswers_Buttons[0].gameObject.SetActive(true);
			this.buttonAnswers_Buttons[0].onClick.AddListener(delegate()
			{
				this.FinishTalk();
			});
			this.buttonAnswers_Texts[0].text = Language_Manager.instance.GetText("Ok");
		}
		this.talk_Anim.PlayInFixedTime("Normal", -1, 0f);
		List<ContentSizeFitter> list4 = new List<ContentSizeFitter>();
		list4.Add(this.text_Talk.gameObject.GetComponent<ContentSizeFitter>());
		list4.Add(this.panel_Name.gameObject.GetComponent<ContentSizeFitter>());
		for (int k = 0; k < list4.Count; k++)
		{
			list4[k].enabled = false;
			list4[k].SetLayoutVertical();
			list4[k].SetLayoutHorizontal();
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)list4[k].transform);
			list4[k].enabled = true;
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.buttonAnswers_Buttons[0].transform.parent);
		Menu_Manager.instance.SetMenuName("Talk", _player_index);
		this.panelTalk_Anim.SetBool("Main", true);
		this.panelTalk_Anim.PlayInFixedTime("Talk_Waiting", -1, 0f);
		return list3;
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x000303E6 File Offset: 0x0002E5E6
	public void AnimateTalker()
	{
		this.talk_Anim.PlayInFixedTime("Normal", -1, 0f);
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x000303FE File Offset: 0x0002E5FE
	public void TalkAnswer(int _index)
	{
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x00030414 File Offset: 0x0002E614
	public void FinishTalk()
	{
		if (Input_Manager.instance.inputControllers[0].coolDownTime_Timer > 0f)
		{
			return;
		}
		Menu_Manager.instance.BackMenu();
		Game_Manager.instance.SetCinematicMode(false);
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x00030448 File Offset: 0x0002E648
	public void RestartTalk()
	{
		if (Input_Manager.instance.inputControllers[0].coolDownTime_Timer > 0f)
		{
			return;
		}
		this.currentTalkingChar.talk_Controller.NextTalk(0);
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x00030478 File Offset: 0x0002E678
	public void NextTalk()
	{
		if (Input_Manager.instance.inputControllers[0].coolDownTime_Timer > 0f)
		{
			return;
		}
		this.currentTalkingChar.talk_Controller.NextTalk();
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x000304A8 File Offset: 0x0002E6A8
	public void NextPosition()
	{
		if (Input_Manager.instance.inputControllers[0].coolDownTime_Timer > 0f)
		{
			return;
		}
		this.currentTalkingChar.talk_Controller.NextPosition();
		Menu_Manager.instance.BackMenu();
		Game_Manager.instance.SetCinematicMode(false);
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x000304F8 File Offset: 0x0002E6F8
	public void StartLeaving()
	{
		if (Input_Manager.instance.inputControllers[0].coolDownTime_Timer > 0f)
		{
			return;
		}
		this.currentTalkingChar.talk_Controller.StartLeaving();
		Menu_Manager.instance.BackMenu();
		Game_Manager.instance.SetCinematicMode(false);
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x00030547 File Offset: 0x0002E747
	public void HowIsLife()
	{
		if (Input_Manager.instance.inputControllers[0].coolDownTime_Timer > 0f)
		{
			return;
		}
		this.currentTalkingChar.talk_Controller.NextTalk(2);
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00030577 File Offset: 0x0002E777
	public void MayIHelpYou()
	{
		if (Input_Manager.instance.inputControllers[0].coolDownTime_Timer > 0f)
		{
			return;
		}
		this.currentTalkingChar.talk_Controller.NextTalk(1);
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x000305A7 File Offset: 0x0002E7A7
	public void FinishTaigaAwards()
	{
		if (Input_Manager.instance.inputControllers[0].coolDownTime_Timer > 0f)
		{
			return;
		}
		Score_Manager.instance.GiveTaigaRewards();
		Game_Manager.instance.SetGameMode(0);
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x000305DB File Offset: 0x0002E7DB
	public void ExitGameMode()
	{
		if (Input_Manager.instance.inputControllers[0].coolDownTime_Timer > 0f)
		{
			return;
		}
		Game_Manager.instance.SetGameMode(0);
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x00030605 File Offset: 0x0002E805
	public void WaitForProductToBeGiven()
	{
		if (Input_Manager.instance.inputControllers[0].coolDownTime_Timer > 0f)
		{
			return;
		}
		this.FinishTalk();
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x0003062A File Offset: 0x0002E82A
	public void CantHelpWithProduct()
	{
		if (Input_Manager.instance.inputControllers[0].coolDownTime_Timer > 0f)
		{
			return;
		}
		this.currentTalkingChar.customer_Controller.SetProdWantedIndex(-2);
		this.FinishTalk();
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x00030661 File Offset: 0x0002E861
	public void MayUpdateAwait()
	{
		if (Input_Manager.instance.inputControllers[0].coolDownTime_Timer > 0f)
		{
			return;
		}
		Game_Manager.instance.SetGameStage(990);
		this.currentTalkingChar.customer_Controller.MayUpdateAwait();
	}

	// Token: 0x040005FC RID: 1532
	public static Talk_Manager instance;

	// Token: 0x040005FD RID: 1533
	private Char_Controller currentTalkingChar;

	// Token: 0x040005FE RID: 1534
	[Header("Talk")]
	[SerializeField]
	private Animator panelTalk_Anim;

	// Token: 0x040005FF RID: 1535
	[SerializeField]
	private Image panel_Name;

	// Token: 0x04000600 RID: 1536
	[SerializeField]
	private Text text_Name;

	// Token: 0x04000601 RID: 1537
	[SerializeField]
	private Text text_Talk;

	// Token: 0x04000602 RID: 1538
	[SerializeField]
	private Image talk_Image;

	// Token: 0x04000603 RID: 1539
	[SerializeField]
	private Animator talk_Anim;

	// Token: 0x04000604 RID: 1540
	[SerializeField]
	private List<Button> buttonAnswers_Buttons = new List<Button>();

	// Token: 0x04000605 RID: 1541
	[SerializeField]
	private List<Text> buttonAnswers_Texts = new List<Text>();

	// Token: 0x04000606 RID: 1542
	[SerializeField]
	private GameObject panelLocals;

	// Token: 0x04000607 RID: 1543
	[SerializeField]
	private Image affection_Image;

	// Token: 0x04000608 RID: 1544
	[SerializeField]
	private List<Image> prodPreference_Images = new List<Image>();

	// Token: 0x04000609 RID: 1545
	[SerializeField]
	private Image friendship_Image;
}
