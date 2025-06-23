using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class Language_Manager : MonoBehaviour
{
	// Token: 0x06000375 RID: 885 RVA: 0x0001F981 File Offset: 0x0001DB81
	private void Awake()
	{
		if (!Language_Manager.instance)
		{
			Language_Manager.instance = this;
		}
		this.GetFiles(1);
	}

	// Token: 0x06000376 RID: 886 RVA: 0x0001F99C File Offset: 0x0001DB9C
	private void Start()
	{
	}

	// Token: 0x06000377 RID: 887 RVA: 0x0001F9A0 File Offset: 0x0001DBA0
	private void GetFiles(int _index)
	{
		this.lang_Selected = _index;
		this.lang_Languages.Clear();
		this.lang_Languages.Add(this.langExcel.Languages[0].en.ToString());
		this.lang_Languages.Add(this.langExcel.Languages[0].ptbr.ToString());
		this.lang_Languages.Add(this.langExcel.Languages[0].es.ToString());
		this.lang_Languages.Add(this.langExcel.Languages[0].de.ToString());
		this.lang_Languages.Add(this.langExcel.Languages[0].fr.ToString());
		this.lang_Languages.Add(this.langExcel.Languages[0].it.ToString());
		this.lang_Languages.Add(this.langExcel.Languages[0].ja.ToString());
		this.lang_Languages.Add(this.langExcel.Languages[0].ko.ToString());
		this.lang_Languages.Add(this.langExcel.Languages[0].zh.ToString());
		this.lang_keys.Clear();
		this.lang_texts.Clear();
		this.lang_keys.Add(new List<string>());
		this.lang_texts.Add(new List<string>());
		this.lang_keys.Add(new List<string>());
		this.lang_texts.Add(new List<string>());
		for (int i = 0; i < this.langExcel.UI.Count; i++)
		{
			this.lang_keys[1].Add(this.langExcel.UI[i].key);
			switch (this.lang_Selected)
			{
			case 0:
				this.lang_texts[1].Add(this.langExcel.UI[i].en.ToString());
				break;
			case 1:
				this.lang_texts[1].Add(this.langExcel.UI[i].ptbr.ToString());
				break;
			case 2:
				this.lang_texts[1].Add(this.langExcel.UI[i].es.ToString());
				break;
			case 3:
				this.lang_texts[1].Add(this.langExcel.UI[i].de.ToString());
				break;
			case 4:
				this.lang_texts[1].Add(this.langExcel.UI[i].fr.ToString());
				break;
			case 5:
				this.lang_texts[1].Add(this.langExcel.UI[i].it.ToString());
				break;
			case 6:
				this.lang_texts[1].Add(this.langExcel.UI[i].ja.ToString());
				break;
			case 7:
				this.lang_texts[1].Add(this.langExcel.UI[i].ko.ToString());
				break;
			case 8:
				this.lang_texts[1].Add(this.langExcel.UI[i].zh.ToString());
				break;
			}
		}
		this.lang_keys.Add(new List<string>());
		this.lang_texts.Add(new List<string>());
		for (int j = 0; j < this.langExcel.ItemNames.Count; j++)
		{
			this.lang_keys[2].Add(this.langExcel.ItemNames[j].key);
			switch (this.lang_Selected)
			{
			case 0:
				this.lang_texts[2].Add(this.langExcel.ItemNames[j].en.ToString());
				break;
			case 1:
				this.lang_texts[2].Add(this.langExcel.ItemNames[j].ptbr.ToString());
				break;
			case 2:
				this.lang_texts[2].Add(this.langExcel.ItemNames[j].es.ToString());
				break;
			case 3:
				this.lang_texts[2].Add(this.langExcel.ItemNames[j].de.ToString());
				break;
			case 4:
				this.lang_texts[2].Add(this.langExcel.ItemNames[j].fr.ToString());
				break;
			case 5:
				this.lang_texts[2].Add(this.langExcel.ItemNames[j].it.ToString());
				break;
			case 6:
				this.lang_texts[2].Add(this.langExcel.ItemNames[j].ja.ToString());
				break;
			case 7:
				this.lang_texts[2].Add(this.langExcel.ItemNames[j].ko.ToString());
				break;
			case 8:
				this.lang_texts[2].Add(this.langExcel.ItemNames[j].zh.ToString());
				break;
			}
		}
		this.lang_keys.Add(new List<string>());
		this.lang_texts.Add(new List<string>());
		for (int k = 0; k < this.langExcel.Messages.Count; k++)
		{
			this.lang_keys[3].Add(this.langExcel.Messages[k].key);
			switch (this.lang_Selected)
			{
			case 0:
				this.lang_texts[3].Add(this.langExcel.Messages[k].en.ToString());
				break;
			case 1:
				this.lang_texts[3].Add(this.langExcel.Messages[k].ptbr.ToString());
				break;
			case 2:
				this.lang_texts[3].Add(this.langExcel.Messages[k].es.ToString());
				break;
			case 3:
				this.lang_texts[3].Add(this.langExcel.Messages[k].de.ToString());
				break;
			case 4:
				this.lang_texts[3].Add(this.langExcel.Messages[k].fr.ToString());
				break;
			case 5:
				this.lang_texts[3].Add(this.langExcel.Messages[k].it.ToString());
				break;
			case 6:
				this.lang_texts[3].Add(this.langExcel.Messages[k].ja.ToString());
				break;
			case 7:
				this.lang_texts[3].Add(this.langExcel.Messages[k].ko.ToString());
				break;
			case 8:
				this.lang_texts[3].Add(this.langExcel.Messages[k].zh.ToString());
				break;
			}
		}
	}

	// Token: 0x06000378 RID: 888 RVA: 0x0002022F File Offset: 0x0001E42F
	public void AddController(Language_Controller controller)
	{
		if (!this.controllers.Contains(controller))
		{
			this.controllers.Add(controller);
		}
		controller.SetText(controller.GetText());
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00020257 File Offset: 0x0001E457
	public string GetText_LanguageName(int _index)
	{
		return this.lang_Languages[_index];
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00020268 File Offset: 0x0001E468
	public string GetText(List<string> _list, int _line)
	{
		string text = "";
		if (_line < _list.Count)
		{
			text = _list[_line];
		}
		if (text == "")
		{
			text = "LANG_ERROR";
		}
		return text;
	}

	// Token: 0x0600037B RID: 891 RVA: 0x000202A0 File Offset: 0x0001E4A0
	public string GetText(string _key)
	{
		for (int i = 1; i < this.lang_keys.Count; i++)
		{
			for (int j = 0; j < this.lang_keys[i].Count; j++)
			{
				if (string.Equals(_key.Trim(), this.lang_keys[i][j].Trim(), StringComparison.OrdinalIgnoreCase))
				{
					return this.lang_texts[i][j];
				}
			}
		}
		return _key + "*";
	}

	// Token: 0x0600037C RID: 892 RVA: 0x00020324 File Offset: 0x0001E524
	public string GetText(string _key, string[] _tags, string[] _values, bool[] _translating = null)
	{
		string text = this.GetText(_key);
		if (_tags == null)
		{
			return text;
		}
		for (int i = 0; i < _tags.Length; i++)
		{
			string text2 = _values[i];
			int num;
			if (!int.TryParse(text2, out num) && _translating != null && _translating[0])
			{
				text2 = this.GetText(_values[i]);
			}
			text = text.Replace(_tags[i], text2);
		}
		return text;
	}

	// Token: 0x0600037D RID: 893 RVA: 0x0002037A File Offset: 0x0001E57A
	public string GetText(string _key, string _tag, string _value)
	{
		return this.GetText(_key).Replace(_tag, _value);
	}

	// Token: 0x0600037E RID: 894 RVA: 0x0002038A File Offset: 0x0001E58A
	public void ChangeLanguage(int _index)
	{
		this.GetFiles(_index);
		this.SetAllControllers();
	}

	// Token: 0x0600037F RID: 895 RVA: 0x00020399 File Offset: 0x0001E599
	public string GetLanguageName()
	{
		return this.lang_Languages[this.lang_Selected];
	}

	// Token: 0x06000380 RID: 896 RVA: 0x000203AC File Offset: 0x0001E5AC
	private void SetAllControllers()
	{
		for (int i = 0; i < this.controllers.Count; i++)
		{
			if (this.controllers[i])
			{
				this.controllers[i].SetText(this.controllers[i].GetText());
			}
		}
	}

	// Token: 0x040003BF RID: 959
	public static Language_Manager instance;

	// Token: 0x040003C0 RID: 960
	[SerializeField]
	private LangExcel langExcel;

	// Token: 0x040003C1 RID: 961
	private List<List<string>> lang_keys = new List<List<string>>();

	// Token: 0x040003C2 RID: 962
	private List<List<string>> lang_texts = new List<List<string>>();

	// Token: 0x040003C3 RID: 963
	public int lang_Selected;

	// Token: 0x040003C4 RID: 964
	public List<string> lang_Languages;

	// Token: 0x040003C5 RID: 965
	private List<Language_Controller> controllers = new List<Language_Controller>();
}
