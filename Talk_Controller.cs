using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class Talk_Controller : MonoBehaviour
{
	// Token: 0x06000196 RID: 406 RVA: 0x0000FA0E File Offset: 0x0000DC0E
	public void Talk(int _startIndex, int _player_index = -1)
	{
		if (this.char_Controller.isTutorial)
		{
			_startIndex = this.talkIndex;
		}
		this.talkIndex = _startIndex;
		this.Talk("", new List<string>(), new List<string>(), _player_index);
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000FA44 File Offset: 0x0000DC44
	public void Talk(string _charText, List<string> _answers, List<string> _actions, int _player_index = -1)
	{
		if (_player_index != -1)
		{
			this.player_index = _player_index;
		}
		else
		{
			this.player_index = 0;
		}
		string charText = _charText;
		if (this.char_Controller.isCustomer)
		{
			if (this.talkIndex <= 2)
			{
				if (Char_Manager.instance.GetCustomerLifeAchievementState(this.GetID()))
				{
					string[] array = new string[]
					{
						Language_Manager.instance.GetText("_cust_all_ok_00"),
						Language_Manager.instance.GetText("_cust_all_ok_01"),
						Language_Manager.instance.GetText("_cust_all_ok_02")
					};
					charText = array[UnityEngine.Random.Range(0, array.Length)];
					string[] array2 = new string[]
					{
						Language_Manager.instance.GetText("_reply_all_ok_00"),
						Language_Manager.instance.GetText("_reply_all_ok_01"),
						Language_Manager.instance.GetText("_reply_all_ok_02")
					};
					_answers.Add(array2[UnityEngine.Random.Range(0, array2.Length)]);
					_actions.Add("FinishTalk");
				}
				else
				{
					if (this.GetID() == 0)
					{
						charText = Language_Manager.instance.GetText("_cust_00_talk_00");
					}
					if (this.GetID() == 1)
					{
						charText = Language_Manager.instance.GetText("_cust_01_talk_00");
					}
					if (this.GetID() == 2)
					{
						charText = Language_Manager.instance.GetText("_cust_02_talk_00");
					}
					if (this.GetID() == 3)
					{
						charText = Language_Manager.instance.GetText("_cust_03_talk_00");
					}
					if (this.GetID() == 8)
					{
						charText = Language_Manager.instance.GetText("_cust_08_talk_00");
					}
					if (this.GetID() == 10)
					{
						charText = Language_Manager.instance.GetText("_cust_10_talk_00");
					}
					if (this.GetID() == 11)
					{
						charText = Language_Manager.instance.GetText("_cust_11_talk_00");
					}
					if (this.GetID() == 16)
					{
						charText = Language_Manager.instance.GetText("_cust_16_talk_00");
					}
					if (this.GetID() == 19)
					{
						charText = Language_Manager.instance.GetText("_cust_19_talk_00");
					}
					if (this.GetID() == 24)
					{
						charText = Language_Manager.instance.GetText("_cust_24_talk_00");
					}
					if (this.GetID() == 26)
					{
						charText = Language_Manager.instance.GetText("_cust_26_talk_00");
					}
					if (this.GetID() == 29)
					{
						charText = Language_Manager.instance.GetText("_cust_29_talk_00");
					}
					if (this.GetID() == 30)
					{
						charText = Language_Manager.instance.GetText("_cust_30_talk_00");
					}
					if (this.GetID() == 31)
					{
						charText = Language_Manager.instance.GetText("_cust_31_talk_00");
						_answers.Add(Language_Manager.instance.GetText("_cust_31_reply_00"));
						_actions.Add("MayUpdateAwait");
						_answers.Add(Language_Manager.instance.GetText("_cust_31_reply_01"));
						_actions.Add("FinishTalk");
						Talk_Manager.instance.Talk(charText, _answers, _actions, this.talkIndex, this.char_Controller, this.player_index);
						return;
					}
					if (this.GetID() == 32)
					{
						charText = Language_Manager.instance.GetText("_cust_32_talk_00");
					}
					if (this.GetID() == 39)
					{
						charText = Language_Manager.instance.GetText("_cust_39_talk_00");
					}
					if (this.GetID() == 47)
					{
						charText = Language_Manager.instance.GetText("_cust_47_talk_00");
					}
					if (this.GetID() == 49)
					{
						charText = Language_Manager.instance.GetText("_cust_49_talk_00");
					}
					if (this.GetID() == 54)
					{
						charText = Language_Manager.instance.GetText("_cust_54_talk_00");
					}
					if (this.GetID() == 55)
					{
						charText = Language_Manager.instance.GetText("_cust_55_talk_00");
					}
					if (this.GetID() == 56)
					{
						charText = Language_Manager.instance.GetText("_cust_56_talk_00");
					}
					if (this.GetID() == 59)
					{
						charText = Language_Manager.instance.GetText("_cust_59_talk_00");
					}
					if (this.GetID() == 62)
					{
						charText = Language_Manager.instance.GetText("_cust_62_talk_00");
					}
					if (this.GetID() == 63)
					{
						charText = Language_Manager.instance.GetText("_cust_63_talk_00");
					}
					string[] array3 = new string[]
					{
						"...",
						"...",
						"..."
					};
					_answers.Add(array3[UnityEngine.Random.Range(0, array3.Length)]);
					_actions.Add("FinishTalk");
				}
			}
		}
		else if (this.char_Controller.isEvilGuy)
		{
			if (this.talkIndex == 0)
			{
				charText = "Hey, this is the evil guy. - place holder -";
				_answers.Add("Bye bye!");
				_actions.Add("StartLeaving");
			}
		}
		else if (this.char_Controller.isTutorial)
		{
			if (this.talkIndex == 0)
			{
				charText = Language_Manager.instance.GetText("_tutorial_talk_00");
				_answers.Add(Language_Manager.instance.GetText("_tutorial_reply_00"));
				_actions.Add("NextPosition");
				_answers.Add(Language_Manager.instance.GetText("_tutorial_reply_01"));
				_actions.Add("StartLeaving");
			}
			else if (this.talkIndex == 1)
			{
				charText = Language_Manager.instance.GetText("_tutorial_talk_01");
				_answers.Add(Language_Manager.instance.GetText("_tutorial_reply_02"));
				_actions.Add("NextPosition");
			}
			else if (this.talkIndex == 2)
			{
				charText = Language_Manager.instance.GetText("_tutorial_talk_02");
				_answers.Add(Language_Manager.instance.GetText("_tutorial_reply_02"));
				_actions.Add("NextPosition");
			}
			else if (this.talkIndex == 3)
			{
				charText = Language_Manager.instance.GetText("_tutorial_talk_03");
				_answers.Add(Language_Manager.instance.GetText("_tutorial_reply_02"));
				_actions.Add("NextPosition");
			}
			else if (this.talkIndex == 4)
			{
				charText = Language_Manager.instance.GetText("_tutorial_talk_04");
				_answers.Add(Language_Manager.instance.GetText("_tutorial_reply_02"));
				_actions.Add("NextPosition");
			}
			else if (this.talkIndex == 5)
			{
				charText = Language_Manager.instance.GetText("_tutorial_talk_05");
				_answers.Add(Language_Manager.instance.GetText("_tutorial_reply_02"));
				_actions.Add("NextPosition");
			}
			else if (this.talkIndex == 6)
			{
				charText = Language_Manager.instance.GetText("_tutorial_talk_06");
				_answers.Add(Language_Manager.instance.GetText("_tutorial_reply_03"));
				_actions.Add("StartLeaving");
			}
		}
		else if (this.char_Controller.isSeller)
		{
			if (this.talkIndex == 0)
			{
				charText = "Hey, do you wanna check out some special products? - place holder -";
				_answers.Add("Bye bye!");
				_actions.Add("StartLeaving");
			}
		}
		else if (this.char_Controller.isAuditor)
		{
			if (this.talkIndex == 0)
			{
				charText = this.CreateDialog(new string[]
				{
					string.Concat(new string[]
					{
						Language_Manager.instance.GetText("_auditor_talk_00_a"),
						" ",
						Score_Manager.instance.taigaScore.ToString(),
						" ",
						Language_Manager.instance.GetText("_auditor_talk_00_b")
					}),
					Language_Manager.instance.GetText("_auditor_talk_00_c")
				});
				_answers.Add(Language_Manager.instance.GetText("_reply_all_bye_00"));
				_actions.Add("StartLeaving");
			}
		}
		else if (this.char_Controller.isTaigaAward)
		{
			if (this.talkIndex == 0)
			{
				Score_Manager.instance.ComputeTaigaAwards();
				charText = this.CreateDialog(new string[]
				{
					Language_Manager.instance.GetText("_taigaaward_talk_00_a"),
					Language_Manager.instance.GetText("_taigaaward_talk_00_b")
				});
				_answers.Add("...");
				_actions.Add("NextTalk");
			}
			if (this.talkIndex == 1)
			{
				charText = this.CreateDialog(new string[]
				{
					Language_Manager.instance.GetText("_taigaaward_talk_01_a"),
					Language_Manager.instance.GetText("_taigaaward_talk_01_b"),
					Language_Manager.instance.GetText("_taigaaward_talk_01_c")
				});
				_answers.Add("...");
				_actions.Add("NextTalk");
			}
			else if (this.talkIndex == 2)
			{
				charText = this.CreateDialog(new string[]
				{
					Language_Manager.instance.GetText("_taigaaward_talk_02_a"),
					Language_Manager.instance.GetText("_taigaaward_talk_02_b")
				});
				_answers.Add("...");
				_actions.Add("NextTalk");
			}
			else if (this.talkIndex == 3)
			{
				charText = this.CreateDialog(new string[]
				{
					Score_Manager.instance.taigaAwards_WinnerNames[0] + "!!!",
					Language_Manager.instance.GetText("_taigaaward_talk_03_a")
				});
				_answers.Add("...");
				_actions.Add("NextTalk");
			}
			else if (this.talkIndex == 4)
			{
				charText = this.CreateDialog(new string[]
				{
					Language_Manager.instance.GetText("_taigaaward_talk_04_a"),
					Language_Manager.instance.GetText("_taigaaward_talk_04_b")
				});
				_answers.Add(Language_Manager.instance.GetText("_reply_all_bye_01"));
				_actions.Add("FinishTaigaAwards");
			}
		}
		Talk_Manager.instance.Talk(charText, _answers, _actions, this.talkIndex, this.char_Controller, this.player_index);
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00010355 File Offset: 0x0000E555
	public void NextTalk()
	{
		this.talkIndex++;
		this.Talk(this.talkIndex, -1);
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00010372 File Offset: 0x0000E572
	public void NextTalk(int _index)
	{
		this.talkIndex = _index;
		this.Talk(this.talkIndex, -1);
	}

	// Token: 0x0600019A RID: 410 RVA: 0x00010388 File Offset: 0x0000E588
	public void NextPosition()
	{
		this.talkIndex++;
		this.char_Controller.ResetNavPath();
		Vector3 position = this.char_Controller.navPoints[this.talkIndex].transform.position;
		position.y = 0f;
		this.char_Controller.navPath = Nav_Manager.instance.GetNavPath(base.transform.position, position, true);
	}

	// Token: 0x0600019B RID: 411 RVA: 0x000103FD File Offset: 0x0000E5FD
	public void StartLeaving()
	{
		this.char_Controller.StartLeaving();
		if (this.char_Controller.isTutorial)
		{
			Mail_Manager.instance.Send_Mail_OldMan_NextQuest();
		}
	}

	// Token: 0x0600019C RID: 412 RVA: 0x00010424 File Offset: 0x0000E624
	public string CreateDialog(string[] _stringArray)
	{
		string text = "";
		for (int i = 0; i < _stringArray.Length; i++)
		{
			text = text + "\n" + _stringArray[i];
		}
		return text;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x00010455 File Offset: 0x0000E655
	private int GetID()
	{
		return this.char_Controller.GetID();
	}

	// Token: 0x04000226 RID: 550
	[Header("Talk")]
	[SerializeField]
	public int talkIndex;

	// Token: 0x04000227 RID: 551
	public Char_Controller char_Controller;

	// Token: 0x04000228 RID: 552
	public Customer_Controller cust_Controller;

	// Token: 0x04000229 RID: 553
	private int player_index;
}
