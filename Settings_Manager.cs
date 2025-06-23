using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Token: 0x02000057 RID: 87
public class Settings_Manager : MonoBehaviour
{
	// Token: 0x060004A1 RID: 1185 RVA: 0x0002E9D8 File Offset: 0x0002CBD8
	private void Awake()
	{
		if (!Settings_Manager.instance)
		{
			Settings_Manager.instance = this;
		}
		this.GetResFactor();
		this.buttonSubMenu_Parent = this.buttonSubMenu_Default.transform.parent.gameObject;
		this.buttonSubMenu_Default.gameObject.SetActive(false);
		this.SetMasterVolume(0f);
		this.SetNextMasterVolume(0f);
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x0002EA3F File Offset: 0x0002CC3F
	private void Start()
	{
		Save_Manager.instance.LoadSettings();
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x0002EA4B File Offset: 0x0002CC4B
	private void Update()
	{
		this.UpdateMenuScale();
		this.UpdateMasterVolume();
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x0002EA5C File Offset: 0x0002CC5C
	public void RefreshSettingsMenu()
	{
		this.language_Text.text = Language_Manager.instance.GetLanguageName();
		this.menuScale_Text.text = this.menuScale_Names[this.menuScale_Index];
		if (Input_Manager.instance.GetScheme(-1) == "Keyboard&Mouse")
		{
			this.mouseSensitivity_Panel.SetActive(true);
			this.mouseSensitivity_Slider.minValue = this.mouseSensitivity_MinMax.x;
			this.mouseSensitivity_Slider.maxValue = this.mouseSensitivity_MinMax.y;
			this.mouseSensitivity_Slider.value = this.mouseSensitivity_Value;
		}
		else
		{
			this.mouseSensitivity_Panel.SetActive(false);
		}
		this.screenMode_Text.text = this.availableScreenModeNames[this.screenMode_Index];
		this.gameRes_Text.text = this.currentGameResName;
		string text = "Off";
		if (this.currentFrameLimitIndex > 0)
		{
			text = this.availableFrameLimits[this.currentFrameLimitIndex].ToString() + " fps";
		}
		this.frameLimit_Text.text = text;
		bool[] array = new bool[]
		{
			true,
			true
		};
		if (this.currentAudioMusicIndex == 0)
		{
			array[0] = false;
		}
		if (this.currentAudioMusicIndex >= 10)
		{
			array[1] = false;
		}
		this.audioMusic_Arrows[0].gameObject.SetActive(array[0]);
		this.audioMusic_Arrows[1].gameObject.SetActive(array[1]);
		this.audioMusic_Text.text = this.currentAudioMusicIndex.ToString();
		bool[] array2 = new bool[]
		{
			true,
			true
		};
		if (this.currentAudioSfxIndex == 0)
		{
			array2[0] = false;
		}
		if (this.currentAudioSfxIndex >= 10)
		{
			array2[1] = false;
		}
		this.audioSfx_Arrows[0].gameObject.SetActive(array2[0]);
		this.audioSfx_Arrows[1].gameObject.SetActive(array2[1]);
		this.audioSfx_Text.text = this.currentAudioSfxIndex.ToString();
		if (Input_Manager.instance.GetScheme(-1) == "Joystick")
		{
			this.gamepadSensitivity_Panel.SetActive(true);
			bool[] array3 = new bool[]
			{
				true,
				true
			};
			if (this.currentGamepadSensitivity == 0)
			{
				array3[0] = false;
			}
			if (this.currentGamepadSensitivity >= this.availableGamepadSensitivity.Length - 1)
			{
				array3[1] = false;
			}
			this.gamepadSensitivity_Arrows[0].gameObject.SetActive(array3[0]);
			this.gamepadSensitivity_Arrows[1].gameObject.SetActive(array3[1]);
			this.gamepadSensitivity_Text.text = (this.currentGamepadSensitivity + 1).ToString();
			return;
		}
		this.gamepadSensitivity_Panel.SetActive(false);
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x0002ED00 File Offset: 0x0002CF00
	public void SetMenuScale()
	{
		if (this.menuScale_Index == this.menuScale_Presets.Length - 1)
		{
			this.menuScale_Index = 0;
		}
		else
		{
			this.menuScale_Index++;
		}
		for (int i = 0; i < this.menuScale_Canvases.Count; i++)
		{
			if (this.menuScale_Canvases[i])
			{
				if (Menu_Manager.instance.GetMenuIndex() != i)
				{
					this.menuScale_Canvases[i].referenceResolution = new Vector2(1920f, (float)this.menuScale_Presets[this.menuScale_Index]);
				}
				else
				{
					this.menuScale_GradualCanvas = this.menuScale_Canvases[i];
				}
			}
		}
		foreach (WorldUI_Controller worldUI_Controller in Interactor_Manager.instance.ui_ctrls)
		{
			worldUI_Controller.gameObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2((float)this.menuScale_WorldPresets[this.menuScale_Index] * this.resWidthFactor, (float)this.menuScale_WorldPresets[this.menuScale_Index]);
		}
		this.RefreshSettingsMenu();
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x0002EE3C File Offset: 0x0002D03C
	public void SetMenuScale(int _n)
	{
		this.menuScale_Index = _n;
		if (this.menuScale_Index >= this.menuScale_Presets.Length)
		{
			this.menuScale_Index = 0;
		}
		int num = Mathf.FloorToInt((float)this.menuScale_Presets[this.menuScale_Index] * this.resWidthFactor);
		for (int i = 0; i < this.menuScale_Canvases.Count; i++)
		{
			this.menuScale_Canvases[i].referenceResolution = new Vector2((float)num, (float)this.menuScale_Presets[this.menuScale_Index]);
		}
		foreach (WorldUI_Controller worldUI_Controller in Interactor_Manager.instance.ui_ctrls)
		{
			CanvasScaler component = worldUI_Controller.gameObject.GetComponent<CanvasScaler>();
			component.referenceResolution = new Vector2((float)this.menuScale_WorldPresets[this.menuScale_Index] * this.resWidthFactor, (float)this.menuScale_WorldPresets[this.menuScale_Index]);
			this.menuScale_WorldRes_UI = component.referenceResolution;
		}
		this.menuScale_GradualCanvas = null;
		this.RefreshSettingsMenu();
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x0002EF68 File Offset: 0x0002D168
	public void UpdateMenuScale()
	{
		if (this.menuScale_GradualCanvas != null)
		{
			if (this.menuScale_GradualCanvas.referenceResolution.y != (float)this.menuScale_Presets[this.menuScale_Index])
			{
				Vector2 referenceResolution = this.menuScale_GradualCanvas.referenceResolution;
				Vector2 b = new Vector2(1920f, (float)this.menuScale_Presets[this.menuScale_Index]);
				Vector2 vector = Vector2.Lerp(referenceResolution, b, this.menuScale_Speed * Time.unscaledDeltaTime);
				this.menuScale_GradualCanvas.referenceResolution = new Vector2(vector.x, vector.y);
				return;
			}
			this.FinishMenuScale();
		}
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x0002EFFE File Offset: 0x0002D1FE
	public void FinishMenuScale()
	{
		this.SetMenuScale(this.menuScale_Index);
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x0002F00C File Offset: 0x0002D20C
	public int GetMenuScaleWorld()
	{
		return this.menuScale_WorldPresets[this.menuScale_Index];
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x0002F01C File Offset: 0x0002D21C
	public void Set_ScreenMode(int _index)
	{
		if (_index == this.screenMode_Index)
		{
			return;
		}
		this.screenMode_Index = _index;
		if (_index == 1)
		{
			this.SetGameRes(this.currentGameResIndex - 1, false);
		}
		else
		{
			this.SetGameRes(this.availableGameRes.Length - 1, true);
		}
		if (Menu_Manager.instance.GetMenuName() == "SettingsScreenMode")
		{
			Menu_Manager.instance.SetMenuName("Settings");
		}
		this.RefreshSettingsMenu();
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x0002F0A0 File Offset: 0x0002D2A0
	private void GetResFactor()
	{
		Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
		this.resWidthFactor = vector.x / vector.y;
		this.startRes = Display.main.systemHeight;
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x0002F0E4 File Offset: 0x0002D2E4
	public void SetGameRes(int _resIndex, bool _firstTime = false)
	{
		if (this.availableGameRes[_resIndex] > this.startRes && _resIndex > 0)
		{
			this.SetGameRes(_resIndex - 1, false);
			return;
		}
		_resIndex = Mathf.Clamp(_resIndex, 0, this.availableGameRes.Length - 1);
		Resolution[] resolutions = Screen.resolutions;
		if (this.availableGameRes[_resIndex] > resolutions[resolutions.Length - 1].height && _resIndex > 0 && _firstTime)
		{
			this.SetGameRes(_resIndex - 1, true);
			return;
		}
		Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
		this.resWidthFactor = vector.x / vector.y;
		int num = this.availableGameRes[_resIndex];
		int num2 = Mathf.FloorToInt((float)this.availableGameRes[_resIndex] * this.resWidthFactor);
		Screen.SetResolution(num2, num, this.availableScreenMode[this.screenMode_Index]);
		this.currentGameResIndex = _resIndex;
		this.currentGameResName = num.ToString() + "p";
		Vector2 vector2 = new Vector2((float)num2, (float)num);
		this.menuScale_WorldRes_Game = vector2;
		this.SetMenuScale(this.menuScale_Index);
		if (Menu_Manager.instance.GetMenuName() == "SettingsResolution")
		{
			Menu_Manager.instance.SetMenuName("Settings");
		}
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x0002F228 File Offset: 0x0002D428
	public void SetMaxGameRes()
	{
		for (int i = this.availableGameRes.Length - 1; i >= 0; i--)
		{
			if (this.startRes <= this.availableGameRes[i])
			{
				this.SetGameRes(i, true);
				return;
			}
		}
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x0002F263 File Offset: 0x0002D463
	public void SetPixelation(int index)
	{
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x0002F265 File Offset: 0x0002D465
	public void SetPixelation()
	{
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x0002F268 File Offset: 0x0002D468
	public void SetFrameLimit()
	{
		if (this.currentFrameLimitIndex == this.availableFrameLimits.Length - 1)
		{
			this.currentFrameLimitIndex = 0;
		}
		else
		{
			this.currentFrameLimitIndex++;
		}
		this.SetFrameLimit(this.currentFrameLimitIndex);
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x0002F2BE File Offset: 0x0002D4BE
	public void SetFrameLimit(int index)
	{
		this.currentFrameLimitIndex = index;
		Application.targetFrameRate = this.availableFrameLimits[this.currentFrameLimitIndex];
		this.RefreshSettingsMenu();
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x0002F2E0 File Offset: 0x0002D4E0
	public void SetMouseSensitivity()
	{
		this.mouseSensitivity_Value = this.mouseSensitivity_Slider.value;
		this.mouseSensitivity_Text.text = this.mouseSensitivity_Slider.value.ToString();
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x0002F31C File Offset: 0x0002D51C
	public void SetMouseSensitivityValue(float f)
	{
		this.mouseSensitivity_Value = f;
		this.mouseSensitivity_Slider.value = this.mouseSensitivity_Value;
		this.mouseSensitivity_Text.text = this.mouseSensitivity_Slider.value.ToString();
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x0002F35F File Offset: 0x0002D55F
	public float GetMouseSensitivity(float _multiplier)
	{
		return this.mouseSensitivity_Value * _multiplier;
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x0002F36C File Offset: 0x0002D56C
	public void SetLanguage(int _langIndex)
	{
		Language_Manager.instance.ChangeLanguage(_langIndex);
		this.language_Text.text = Language_Manager.instance.GetLanguageName();
		if (Menu_Manager.instance.GetMenuName() == "SettingsLanguages")
		{
			Menu_Manager.instance.SetMenuName("Settings");
		}
		this.RefreshSettingsMenu();
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x0002F3D8 File Offset: 0x0002D5D8
	public void SetSubMenuButtons(List<string> _buttonNames)
	{
		int num = -1;
		for (int i = 0; i < this.buttonSubMenu_List.Count; i++)
		{
			if (this.buttonSubMenu_List[i] != null)
			{
				UnityEngine.Object.Destroy(this.buttonSubMenu_List[i].gameObject);
			}
		}
		this.buttonSubMenu_List.Clear();
		int index = 0;
		for (int j = 0; j < _buttonNames.Count; j++)
		{
			this.buttonSubMenu_List.Add(UnityEngine.Object.Instantiate<GameObject>(this.buttonSubMenu_Default.gameObject).GetComponent<Button>());
			this.buttonSubMenu_List[j].transform.SetParent(this.buttonSubMenu_Parent.transform);
			this.buttonSubMenu_List[j].transform.localScale = Vector3.one;
			this.buttonSubMenu_List[j].gameObject.SetActive(true);
			this.buttonSubMenu_List[j].transform.Find("Text").GetComponent<Text>().text = _buttonNames[j];
			if (Menu_Manager.instance.GetMenuName() == "SettingsScreenMode" && j == this.screenMode_Index)
			{
				index = j;
			}
			if (Menu_Manager.instance.GetMenuName() == "SettingsResolution" && j == this.currentGameResIndex)
			{
				index = j;
			}
			if (Menu_Manager.instance.GetMenuName() == "SettingsLanguages" && j == Language_Manager.instance.lang_Selected)
			{
				index = j;
			}
			if (Menu_Manager.instance.GetMenuName() == "SettingsUIScale" && j == this.menuScale_Index)
			{
				index = j;
			}
			if (j == 0)
			{
				this.buttonSubMenu_List[j].transform.parent.parent.parent.transform.Find("Panel_Title").Find("Button_Back").GetComponent<MenuNav_Controller>().nav_Down = this.buttonSubMenu_List[j].gameObject;
				this.buttonSubMenu_List[j].GetComponent<MenuNav_Controller>().nav_Up = this.buttonSubMenu_List[j].transform.parent.parent.parent.transform.Find("Panel_Title").Find("Button_Back").gameObject;
			}
			else
			{
				this.buttonSubMenu_List[j - 1].GetComponent<MenuNav_Controller>().nav_Down = this.buttonSubMenu_List[j].gameObject;
				this.buttonSubMenu_List[j].GetComponent<MenuNav_Controller>().nav_Up = this.buttonSubMenu_List[j - 1].gameObject;
			}
			if (Menu_Manager.instance.GetMenuName() == "SettingsScreenMode")
			{
				int buttonIndex = j;
				this.buttonSubMenu_List[j].GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.Set_ScreenMode(buttonIndex);
				});
				this.subMenu_Text.text = Language_Manager.instance.GetText("Screen Mode");
			}
			else if (Menu_Manager.instance.GetMenuName() == "SettingsResolution")
			{
				int buttonIndex = j;
				this.buttonSubMenu_List[j].GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.SetGameRes(buttonIndex, false);
				});
				Resolution[] resolutions = Screen.resolutions;
				if (this.availableGameRes[j] > resolutions[resolutions.Length - 1].height)
				{
					this.buttonSubMenu_List[j].gameObject.SetActive(false);
				}
				else if (num == -1)
				{
					num = j;
				}
				this.subMenu_Text.text = Language_Manager.instance.GetText("Resolutions");
			}
			else if (Menu_Manager.instance.GetMenuName() == "SettingsLanguages")
			{
				int buttonIndex = j;
				this.buttonSubMenu_List[j].GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.SetLanguage(buttonIndex);
				});
				this.subMenu_Text.text = Language_Manager.instance.GetText("Languages");
			}
			else if (Menu_Manager.instance.GetMenuName() == "SettingsPixelation")
			{
				int buttonIndex = j;
				this.buttonSubMenu_List[j].GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.SetPixelation(buttonIndex);
				});
				this.subMenu_Text.text = Language_Manager.instance.GetText("Pixelation");
			}
			else if (Menu_Manager.instance.GetMenuName() == "SettingsUIScale")
			{
				int buttonIndex = j;
				this.buttonSubMenu_List[j].GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.SetMenuScale(buttonIndex);
				});
				this.subMenu_Text.text = Language_Manager.instance.GetText("UI Scale");
			}
			int _buttonIndex = j;
			this.buttonSubMenu_List[j].GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.RefreshSubMenuColors(_buttonIndex);
			});
		}
		this.RefreshSubMenuColors(index);
		if (num != -1)
		{
			this.buttonSubMenu_List[num].transform.parent.parent.parent.transform.Find("Panel_Title").Find("Button_Back").GetComponent<MenuNav_Controller>().nav_Down = this.buttonSubMenu_List[num].gameObject;
			this.buttonSubMenu_List[num].GetComponent<MenuNav_Controller>().nav_Up = this.buttonSubMenu_List[num].transform.parent.parent.parent.transform.Find("Panel_Title").Find("Button_Back").gameObject;
		}
		Menu_Manager.instance.SetSelector(this.buttonSubMenu_List[index], true);
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x0002FA08 File Offset: 0x0002DC08
	public void RefreshSubMenuColors(int _index)
	{
		for (int i = 0; i < this.buttonSubMenu_List.Count; i++)
		{
			if (this.buttonSubMenu_List[i] != null)
			{
				if (i == _index)
				{
					this.buttonSubMenu_List[i].GetComponent<Image>().color = this.buttonSelected_Colors[1];
				}
				else
				{
					this.buttonSubMenu_List[i].GetComponent<Image>().color = this.buttonSelected_Colors[0];
				}
			}
		}
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x0002FA8C File Offset: 0x0002DC8C
	public void SetSubMenuScreenMode()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < this.availableScreenModeNames.Length; i++)
		{
			list.Add(this.availableScreenModeNames[i]);
		}
		this.SetSubMenuButtons(list);
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x0002FAC8 File Offset: 0x0002DCC8
	public void SetSubMenuResolutions()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < this.availableGameRes.Length; i++)
		{
			list.Add(this.availableGameRes[i].ToString() + "p");
		}
		this.SetSubMenuButtons(list);
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x0002FB18 File Offset: 0x0002DD18
	public void SetSubMenuLanguages()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < Language_Manager.instance.lang_Languages.Count; i++)
		{
			list.Add(Language_Manager.instance.lang_Languages[i]);
		}
		this.SetSubMenuButtons(list);
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x0002FB62 File Offset: 0x0002DD62
	public void SetSubMenuPixelation()
	{
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x0002FB64 File Offset: 0x0002DD64
	public void SetSubMenuUIScale()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < this.menuScale_Names.Length; i++)
		{
			list.Add(this.menuScale_Names[i]);
		}
		this.SetSubMenuButtons(list);
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x0002FB9F File Offset: 0x0002DD9F
	private void UpdateMasterVolume()
	{
		if (this.currentMasterVolume != this.nextMasterVolume)
		{
			this.currentMasterVolume = Mathf.Lerp(this.currentMasterVolume, this.nextMasterVolume, Time.unscaledDeltaTime);
			this.SetMasterVolume(this.currentMasterVolume);
		}
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x0002FBD8 File Offset: 0x0002DDD8
	public void SetMasterVolume(float _volume)
	{
		this.currentMasterVolume = _volume;
		RuntimeManager.GetVCA("vca:/Master").setVolume(_volume);
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x0002FC00 File Offset: 0x0002DE00
	public void SetNextMasterVolume(float _volume)
	{
		this.nextMasterVolume = _volume;
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x0002FC09 File Offset: 0x0002DE09
	public void SetCurrentMasterVolume(float _volume)
	{
		this.currentMasterVolume = _volume;
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x0002FC12 File Offset: 0x0002DE12
	public void SetAudioMusic_Dir(int _dir)
	{
		this.currentAudioMusicIndex = Mathf.Clamp(this.currentAudioMusicIndex + _dir, 0, 10);
		this.SetAudioMusic(this.currentAudioMusicIndex);
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x0002FC4C File Offset: 0x0002DE4C
	public void SetAudioMusic(int index)
	{
		this.currentAudioMusicIndex = index;
		RuntimeManager.GetVCA("vca:/Music").setVolume(0.1f * (float)this.currentAudioMusicIndex);
		this.RefreshSettingsMenu();
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x0002FC86 File Offset: 0x0002DE86
	public void SetAudioSfx_Dir(int _dir)
	{
		this.currentAudioSfxIndex = Mathf.Clamp(this.currentAudioSfxIndex + _dir, 0, 10);
		this.SetAudioSfx(this.currentAudioSfxIndex);
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x0002FCC0 File Offset: 0x0002DEC0
	public void SetAudioSfx(int index)
	{
		this.currentAudioSfxIndex = index;
		RuntimeManager.GetVCA("vca:/SFX").setVolume(0.1f * (float)this.currentAudioSfxIndex);
		RuntimeManager.GetVCA("vca:/UI").setVolume(0.1f * (float)this.currentAudioSfxIndex);
		this.RefreshSettingsMenu();
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x0002FD1A File Offset: 0x0002DF1A
	public void SetGamepadSensitivity_Dir(int _dir)
	{
		this.currentGamepadSensitivity = Mathf.Clamp(this.currentGamepadSensitivity + _dir, 0, this.availableGamepadSensitivity.Length - 1);
		this.SetGamepadSensitivity(this.currentGamepadSensitivity);
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x0002FD5A File Offset: 0x0002DF5A
	public void SetGamepadSensitivity(int index)
	{
		this.currentGamepadSensitivity = index;
		this.RefreshSettingsMenu();
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x0002FD69 File Offset: 0x0002DF69
	public float GetGamepadSensitivity(float _multiplier)
	{
		return this.availableGamepadSensitivity[this.currentGamepadSensitivity] * _multiplier;
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0002FD7C File Offset: 0x0002DF7C
	public void Button_HorizontalMovementHandler(GameObject _button_GO, int _dir)
	{
		if (_button_GO == this.gamepadSensitivity_Button)
		{
			this.SetGamepadSensitivity_Dir(_dir);
			return;
		}
		if (_button_GO == this.audioMusic_Button)
		{
			this.SetAudioMusic_Dir(_dir);
			return;
		}
		if (_button_GO == this.audioSfx_Button)
		{
			this.SetAudioSfx_Dir(_dir);
		}
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x0002FDCC File Offset: 0x0002DFCC
	public Settings_Manager()
	{
		bool[] array = new bool[2];
		array[0] = true;
		this.availableScreenMode = array;
		this.availableScreenModeNames = new string[]
		{
			"Fullscreen",
			"Windowed"
		};
		this.availableGameRes = new int[]
		{
			480,
			720,
			800,
			900,
			1080,
			1440,
			2160
		};
		this.availableFrameLimits = new int[]
		{
			0,
			30,
			60
		};
		this.mouseSensitivity_MinMax = new Vector2(1f, 50f);
		this.mouseSensitivity_Value = 5f;
		this.buttonSubMenu_List = new List<Button>();
		this.audioMusic_Arrows = new List<Image>();
		this.audioSfx_Arrows = new List<Image>();
		this.gamepadSensitivity_Arrows = new List<Image>();
		this.availableGamepadSensitivity = new float[]
		{
			0.1f,
			0.25f,
			0.5f,
			1.2f,
			1.7f,
			2.3f,
			3f,
			4f,
			4.5f,
			5f
		};
		base..ctor();
	}

	// Token: 0x040005C1 RID: 1473
	public static Settings_Manager instance;

	// Token: 0x040005C2 RID: 1474
	[Header("General")]
	[SerializeField]
	private Color[] buttonSelected_Colors;

	// Token: 0x040005C3 RID: 1475
	[Header("Sprites")]
	[SerializeField]
	private Sprite[] levelSprites;

	// Token: 0x040005C4 RID: 1476
	[SerializeField]
	private Sprite[] levelSpritesZero;

	// Token: 0x040005C5 RID: 1477
	[SerializeField]
	public Sprite[] toggleSprites;

	// Token: 0x040005C6 RID: 1478
	[Header("Menu Scale")]
	[SerializeField]
	private List<CanvasScaler> menuScale_Canvases;

	// Token: 0x040005C7 RID: 1479
	[SerializeField]
	public CanvasScaler menuScale_WorldCanvas;

	// Token: 0x040005C8 RID: 1480
	[SerializeField]
	public CanvasScaler menuScale_PCCanvas;

	// Token: 0x040005C9 RID: 1481
	[SerializeField]
	private Text menuScale_Text;

	// Token: 0x040005CA RID: 1482
	[SerializeField]
	private int[] menuScale_Presets;

	// Token: 0x040005CB RID: 1483
	[SerializeField]
	private int[] menuScale_WorldPresets;

	// Token: 0x040005CC RID: 1484
	[SerializeField]
	private string[] menuScale_Names;

	// Token: 0x040005CD RID: 1485
	[SerializeField]
	public int menuScale_Index = 1;

	// Token: 0x040005CE RID: 1486
	[SerializeField]
	private float menuScale_Speed = 10f;

	// Token: 0x040005CF RID: 1487
	[SerializeField]
	private CanvasScaler menuScale_GradualCanvas;

	// Token: 0x040005D0 RID: 1488
	[SerializeField]
	private CanvasScaler menuScale_MainCanvas_PC;

	// Token: 0x040005D1 RID: 1489
	public Vector2 menuScale_WorldRes_Game;

	// Token: 0x040005D2 RID: 1490
	public Vector2 menuScale_WorldRes_UI;

	// Token: 0x040005D3 RID: 1491
	[Header("Screen Mode")]
	public int screenMode_Index;

	// Token: 0x040005D4 RID: 1492
	[SerializeField]
	private Text screenMode_Text;

	// Token: 0x040005D5 RID: 1493
	private bool[] availableScreenMode;

	// Token: 0x040005D6 RID: 1494
	private string[] availableScreenModeNames;

	// Token: 0x040005D7 RID: 1495
	[HideInInspector]
	public int currentGameResIndex;

	// Token: 0x040005D8 RID: 1496
	[HideInInspector]
	public string currentGameResName;

	// Token: 0x040005D9 RID: 1497
	[Header("Resolution")]
	[SerializeField]
	public Text gameRes_Text;

	// Token: 0x040005DA RID: 1498
	private int[] availableGameRes;

	// Token: 0x040005DB RID: 1499
	public float resWidthFactor;

	// Token: 0x040005DC RID: 1500
	private int startRes;

	// Token: 0x040005DD RID: 1501
	[HideInInspector]
	public int currentFrameLimitIndex;

	// Token: 0x040005DE RID: 1502
	[Header("Frame Limit")]
	[SerializeField]
	public Text frameLimit_Text;

	// Token: 0x040005DF RID: 1503
	private int[] availableFrameLimits;

	// Token: 0x040005E0 RID: 1504
	[Header("Mouse Sensitivity")]
	[SerializeField]
	private Slider mouseSensitivity_Slider;

	// Token: 0x040005E1 RID: 1505
	[SerializeField]
	private GameObject mouseSensitivity_Panel;

	// Token: 0x040005E2 RID: 1506
	private Vector2 mouseSensitivity_MinMax;

	// Token: 0x040005E3 RID: 1507
	[SerializeField]
	private float mouseSensitivity_Value;

	// Token: 0x040005E4 RID: 1508
	[SerializeField]
	private Text mouseSensitivity_Text;

	// Token: 0x040005E5 RID: 1509
	[Header("Language")]
	[SerializeField]
	public Text language_Text;

	// Token: 0x040005E6 RID: 1510
	[Header("SubMenu")]
	[SerializeField]
	private Text subMenu_Text;

	// Token: 0x040005E7 RID: 1511
	[SerializeField]
	private Button buttonSubMenu_Default;

	// Token: 0x040005E8 RID: 1512
	private GameObject buttonSubMenu_Parent;

	// Token: 0x040005E9 RID: 1513
	[SerializeField]
	private List<Button> buttonSubMenu_List;

	// Token: 0x040005EA RID: 1514
	private float currentMasterVolume;

	// Token: 0x040005EB RID: 1515
	private float nextMasterVolume;

	// Token: 0x040005EC RID: 1516
	[HideInInspector]
	public int currentAudioMusicIndex;

	// Token: 0x040005ED RID: 1517
	[Header("Audio Music")]
	[SerializeField]
	public GameObject audioMusic_Button;

	// Token: 0x040005EE RID: 1518
	[SerializeField]
	public Text audioMusic_Text;

	// Token: 0x040005EF RID: 1519
	[SerializeField]
	public List<Image> audioMusic_Arrows;

	// Token: 0x040005F0 RID: 1520
	[SerializeField]
	private AudioMixer audioMusic_AM;

	// Token: 0x040005F1 RID: 1521
	[HideInInspector]
	public int currentAudioSfxIndex;

	// Token: 0x040005F2 RID: 1522
	[Header("Audio SFX")]
	[SerializeField]
	public GameObject audioSfx_Button;

	// Token: 0x040005F3 RID: 1523
	[SerializeField]
	public Text audioSfx_Text;

	// Token: 0x040005F4 RID: 1524
	[SerializeField]
	public List<Image> audioSfx_Arrows;

	// Token: 0x040005F5 RID: 1525
	[SerializeField]
	private AudioMixer audioSfx_AM;

	// Token: 0x040005F6 RID: 1526
	[HideInInspector]
	public int currentGamepadSensitivity;

	// Token: 0x040005F7 RID: 1527
	[Header("Gamepad Sensitivity")]
	[SerializeField]
	public GameObject gamepadSensitivity_Button;

	// Token: 0x040005F8 RID: 1528
	[SerializeField]
	private GameObject gamepadSensitivity_Panel;

	// Token: 0x040005F9 RID: 1529
	[SerializeField]
	public Text gamepadSensitivity_Text;

	// Token: 0x040005FA RID: 1530
	[SerializeField]
	public List<Image> gamepadSensitivity_Arrows;

	// Token: 0x040005FB RID: 1531
	private float[] availableGamepadSensitivity;
}
