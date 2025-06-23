using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class Mail_Manager : MonoBehaviour
{
	// Token: 0x06000382 RID: 898 RVA: 0x0002042D File Offset: 0x0001E62D
	private void Awake()
	{
		if (Mail_Manager.instance == null)
		{
			Mail_Manager.instance = this;
		}
	}

	// Token: 0x06000383 RID: 899 RVA: 0x00020442 File Offset: 0x0001E642
	public void Load_Data(SaveData _data)
	{
		this.mails = new List<Mail_Data>(_data.mail_data_SD);
	}

	// Token: 0x06000384 RID: 900 RVA: 0x00020458 File Offset: 0x0001E658
	public List<Mail_Data> Get_Mail_List_By_Category(int _cat_index)
	{
		List<Mail_Data> list = new List<Mail_Data>();
		for (int i = this.mails.Count - 1; i >= 0; i--)
		{
			if (_cat_index == 0)
			{
				list.Add(this.mails[i]);
			}
			else if (this.mails[i].category == _cat_index)
			{
				list.Add(this.mails[i]);
			}
		}
		return list;
	}

	// Token: 0x06000385 RID: 901 RVA: 0x000204C4 File Offset: 0x0001E6C4
	public void Create_Mail(int _owner_index, int _owner_cat, Mail_Manager.MailCategory _category, Mail_Manager.MailType _type, string _title, string _message, int _date, int _time_to_expire, List<int> _rewards_cats, List<int> _rewards_indexes, bool _procedural, string[] _tag, string[] _tag_value, bool _has_staff_data, Staff_Data _staff)
	{
		this.Create_Mail(new Mail_Data
		{
			owner_index = _owner_index,
			owner_cat = _owner_cat,
			category = (int)_category,
			type = (int)_type,
			title = _title,
			message = _message,
			date = _date,
			time_to_expire = _time_to_expire,
			reward_cats = _rewards_cats,
			reward_indexes = _rewards_indexes,
			task_procedural = _procedural,
			task_tag = _tag,
			task_tag_value = _tag_value,
			has_staff_data = _has_staff_data,
			staff_data = _staff
		});
	}

	// Token: 0x06000386 RID: 902 RVA: 0x00020553 File Offset: 0x0001E753
	public void Create_Mail(Mail_Data _mail_data)
	{
		this.mails.Add(_mail_data);
		this.new_mails = true;
		PC_Manager.instance.Refresh_PC_Controller();
	}

	// Token: 0x06000387 RID: 903 RVA: 0x00020574 File Offset: 0x0001E774
	public void Delete_Staff_Mails()
	{
		List<Mail_Data> list = new List<Mail_Data>();
		foreach (Mail_Data mail_Data in this.mails)
		{
			if (mail_Data.has_staff_data)
			{
				list.Add(mail_Data);
			}
		}
		foreach (Mail_Data item in list)
		{
			this.mails.Remove(item);
		}
	}

	// Token: 0x06000388 RID: 904 RVA: 0x00020618 File Offset: 0x0001E818
	public bool Get_Has_Unread_Mails()
	{
		bool flag = false;
		using (List<Mail_Data>.Enumerator enumerator = this.mails.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.opened)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			this.new_mails = false;
		}
		return flag;
	}

	// Token: 0x06000389 RID: 905 RVA: 0x0002067C File Offset: 0x0001E87C
	public void Send_Mail_Possible_Staff()
	{
		if (Char_Manager.instance.staff_Possible_Staff_Data.Count <= 0 || Char_Manager.instance.staff_Possible_Staff_Data == null)
		{
			return;
		}
		for (int i = 0; i < Char_Manager.instance.staff_Possible_Staff_Data.Count; i++)
		{
			Staff_Data staff = Char_Manager.instance.staff_Possible_Staff_Data[i];
			this.Create_Mail(6, 1, Mail_Manager.MailCategory.MANAGERS, Mail_Manager.MailType.INFO, "Staff", "_staff_mail_applicant_0", 0, -1, null, null, false, null, null, true, staff);
		}
		Char_Manager.instance.staff_Possible_Staff_Data.Clear();
	}

	// Token: 0x0600038A RID: 906 RVA: 0x00020700 File Offset: 0x0001E900
	public void Send_Mail_Rewards(int _owner_index, int _owner_cat, string _text, List<int> _rewards_cats, List<int> _rewards_indexes, bool _procedural, string[] _tags, string[] _values)
	{
		this.Create_Mail(_owner_index, _owner_cat, Mail_Manager.MailCategory.REWARDS, Mail_Manager.MailType.INFO, "Completed", _text, 0, -1, _rewards_cats, _rewards_indexes, _procedural, _tags, _values, false, null);
	}

	// Token: 0x0600038B RID: 907 RVA: 0x0002072C File Offset: 0x0001E92C
	public void Send_Mail_Auditor_Weekly()
	{
		string[] tag = new string[]
		{
			"[number]",
			"[max]"
		};
		string[] tag_value = new string[]
		{
			Score_Manager.instance.taigaScore.ToString(),
			"100"
		};
		this.Create_Mail(6, 1, Mail_Manager.MailCategory.MANAGERS, Mail_Manager.MailType.INFO, "_auditor_name_00", "_auditor_talk_01_a", 0, -1, null, null, false, tag, tag_value, false, null);
	}

	// Token: 0x0600038C RID: 908 RVA: 0x00020791 File Offset: 0x0001E991
	public void Send_Mail_Auditor_AutoQuests()
	{
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00020793 File Offset: 0x0001E993
	public void Send_Mail_OldMan_NextQuest()
	{
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00020798 File Offset: 0x0001E998
	public void Send_Mail_OldMan_Welcome()
	{
		this.Create_Mail(4, 1, Mail_Manager.MailCategory.MANAGERS, Mail_Manager.MailType.INFO, "_gen_hello_04", "_tutorial_mail_00", 0, -1, null, null, false, null, null, false, null);
	}

	// Token: 0x0600038F RID: 911 RVA: 0x000207C4 File Offset: 0x0001E9C4
	public void Send_Mail_Customer_Needs_By_Odd()
	{
		List<int> list = Char_Manager.instance.CreateProdWantedNowNeed_ByOdd(50);
		if (list.Count <= 0)
		{
			return;
		}
		string title = "Products";
		string[] array = new string[]
		{
			"_mail_cust_need_00_a",
			"_mail_cust_need_01_a",
			"_mail_cust_need_02_a",
			"_mail_cust_need_03_a",
			"_mail_cust_need_04_a",
			"_mail_cust_need_05_a"
		};
		string message = array[UnityEngine.Random.Range(0, array.Length)];
		string[] tag = new string[]
		{
			"[item]"
		};
		string[] tag_value = new string[]
		{
			Inv_Manager.instance.GetProdName(list[1], false)
		};
		this.Create_Mail(list[0], 2, Mail_Manager.MailCategory.CUSTOMERS, Mail_Manager.MailType.INFO, title, message, 0, -1, null, null, true, tag, tag_value, false, null);
	}

	// Token: 0x040003C6 RID: 966
	public static Mail_Manager instance;

	// Token: 0x040003C7 RID: 967
	[SerializeField]
	public List<Mail_Data> mails = new List<Mail_Data>();

	// Token: 0x040003C8 RID: 968
	public bool new_mails;

	// Token: 0x0200007E RID: 126
	public enum MailCategory
	{
		// Token: 0x040006BD RID: 1725
		ALL,
		// Token: 0x040006BE RID: 1726
		MANAGERS,
		// Token: 0x040006BF RID: 1727
		CUSTOMERS,
		// Token: 0x040006C0 RID: 1728
		REWARDS
	}

	// Token: 0x0200007F RID: 127
	public enum MailType
	{
		// Token: 0x040006C2 RID: 1730
		INFO,
		// Token: 0x040006C3 RID: 1731
		QUESTION
	}
}
