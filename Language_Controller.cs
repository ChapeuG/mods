using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000018 RID: 24
public class Language_Controller : MonoBehaviour
{
	// Token: 0x060000DB RID: 219 RVA: 0x0000A608 File Offset: 0x00008808
	private void Awake()
	{
		this.text = base.GetComponent<Text>();
		this.text_TMP = base.GetComponent<TMP_Text>();
		if (this.text && this._string == "")
		{
			this._string = this.text.text;
			return;
		}
		if (this.text_TMP && this._string == "")
		{
			this._string = this.text_TMP.text;
		}
	}

	// Token: 0x060000DC RID: 220 RVA: 0x0000A68E File Offset: 0x0000888E
	private void OnEnable()
	{
		if (this.auto_add)
		{
			this.AddController();
		}
	}

	// Token: 0x060000DD RID: 221 RVA: 0x0000A69E File Offset: 0x0000889E
	public void AddController()
	{
		Language_Manager.instance.AddController(this);
	}

	// Token: 0x060000DE RID: 222 RVA: 0x0000A6AB File Offset: 0x000088AB
	public string GetText()
	{
		if (this.language_name_index > 0)
		{
			return Language_Manager.instance.GetText_LanguageName(this.language_name_index);
		}
		return Language_Manager.instance.GetText(this._string);
	}

	// Token: 0x060000DF RID: 223 RVA: 0x0000A6D8 File Offset: 0x000088D8
	public void SetText(string _text)
	{
		if (this.ToUpper)
		{
			_text = _text.ToUpper();
		}
		if (this.ToLower)
		{
			_text = _text.ToLower();
		}
		if (this.text)
		{
			this.text.text = _text;
			return;
		}
		if (this.text_TMP)
		{
			this.text_TMP.text = _text;
		}
	}

	// Token: 0x0400014B RID: 331
	[SerializeField]
	private bool auto_add = true;

	// Token: 0x0400014C RID: 332
	[SerializeField]
	private bool ToUpper;

	// Token: 0x0400014D RID: 333
	[SerializeField]
	private bool ToLower;

	// Token: 0x0400014E RID: 334
	public int language_name_index;

	// Token: 0x0400014F RID: 335
	private Text text;

	// Token: 0x04000150 RID: 336
	private TMP_Text text_TMP;

	// Token: 0x04000151 RID: 337
	[SerializeField]
	private string _string = "";
}
