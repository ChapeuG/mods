using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000014 RID: 20
public class InputHint_Controller : MonoBehaviour
{
	// Token: 0x060000B9 RID: 185 RVA: 0x00008727 File Offset: 0x00006927
	private void Awake()
	{
		if (!this.imageHint)
		{
			this.imageHint = base.gameObject.GetComponent<Image>();
		}
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00008747 File Offset: 0x00006947
	private void OnEnable()
	{
	}

	// Token: 0x060000BB RID: 187 RVA: 0x0000874C File Offset: 0x0000694C
	private void Start()
	{
		WorldUI_Controller componentInParent = base.GetComponentInParent<WorldUI_Controller>();
		if (componentInParent != null)
		{
			this.playerId = componentInParent.playerId;
		}
		Input_Manager.instance.AddInputHint(this);
		this.RefreshInputHint();
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00008787 File Offset: 0x00006987
	private IEnumerator RefreshCoroutine()
	{
		yield return new WaitForSeconds(0.2f);
		this.RefreshInputHint();
		yield break;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00008798 File Offset: 0x00006998
	public bool RefreshInputHint()
	{
		this.currentScheme = Input_Manager.instance.GetScheme(this.playerId);
		if (Cheat_Manager.instance.GetFixedButtons() != "AUTO")
		{
			this.currentScheme = Cheat_Manager.instance.GetFixedButtons();
		}
		if (this.currentScheme == "Joystick")
		{
			if (this.onlyOnKeyboard.Length != 0 || this.onlyOnJoystick.Length != 0)
			{
				for (int i = 0; i < this.onlyOnKeyboard.Length; i++)
				{
					if (this.onlyOnKeyboard[i])
					{
						this.onlyOnKeyboard[i].SetActive(false);
					}
				}
				for (int j = 0; j < this.onlyOnJoystick.Length; j++)
				{
					if (this.onlyOnJoystick[j])
					{
						this.onlyOnJoystick[j].SetActive(true);
					}
				}
				Invoker.InvokeDelayed(new Invokable(this.SizeFitter), 0.2f);
				this.SizeFitter();
				return true;
			}
			if (this.sprite_Joystick)
			{
				this.imageHint.enabled = true;
				this.imageHint.sprite = this.sprite_Joystick;
				if (!this.imageHint.preserveAspect)
				{
					this.imageHint.SetNativeSize();
				}
				this.SizeFitter();
				return true;
			}
			this.imageHint.enabled = false;
		}
		else if (this.currentScheme == "Keyboard&Mouse")
		{
			if (this.onlyOnKeyboard.Length != 0 || this.onlyOnJoystick.Length != 0)
			{
				for (int k = 0; k < this.onlyOnKeyboard.Length; k++)
				{
					if (this.onlyOnKeyboard[k])
					{
						this.onlyOnKeyboard[k].SetActive(true);
					}
				}
				for (int l = 0; l < this.onlyOnJoystick.Length; l++)
				{
					if (this.onlyOnJoystick[l])
					{
						this.onlyOnJoystick[l].SetActive(false);
					}
				}
				Invoker.InvokeDelayed(new Invokable(this.SizeFitter), 0.2f);
				this.SizeFitter();
				return true;
			}
			if (this.sprite_Keyboard)
			{
				this.imageHint.enabled = true;
				this.imageHint.sprite = this.sprite_Keyboard;
				if (!this.imageHint.preserveAspect)
				{
					this.imageHint.SetNativeSize();
				}
				this.SizeFitter();
				return true;
			}
			this.imageHint.enabled = false;
		}
		return false;
	}

	// Token: 0x060000BE RID: 190 RVA: 0x000089E0 File Offset: 0x00006BE0
	public void SizeFitter()
	{
		if (this.sizeFitter)
		{
			this.sizeFitter.enabled = false;
			this.sizeFitter.SetLayoutVertical();
			this.sizeFitter.SetLayoutHorizontal();
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.sizeFitter.transform);
			this.sizeFitter.enabled = true;
			return;
		}
		if (this.onlyOnKeyboard.Length != 0 || this.onlyOnJoystick.Length != 0)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)base.transform);
		}
	}

	// Token: 0x040000FE RID: 254
	public int playerId;

	// Token: 0x040000FF RID: 255
	public string currentScheme;

	// Token: 0x04000100 RID: 256
	[SerializeField]
	private Image imageHint;

	// Token: 0x04000101 RID: 257
	[Space(10f)]
	[SerializeField]
	private GameObject[] onlyOnJoystick;

	// Token: 0x04000102 RID: 258
	[SerializeField]
	private GameObject[] onlyOnKeyboard;

	// Token: 0x04000103 RID: 259
	[Space(10f)]
	[SerializeField]
	public Sprite sprite_Joystick;

	// Token: 0x04000104 RID: 260
	[SerializeField]
	public Sprite sprite_Keyboard;

	// Token: 0x04000105 RID: 261
	[Header("Size Fitter")]
	[SerializeField]
	public ContentSizeFitter sizeFitter;
}
