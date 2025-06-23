using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

// Token: 0x02000048 RID: 72
public class Menu_Manager : MonoBehaviour
{
	// Token: 0x06000394 RID: 916 RVA: 0x0002091C File Offset: 0x0001EB1C
	private void Awake()
	{
		if (!Menu_Manager.instance)
		{
			Menu_Manager.instance = this;
		}
		for (int i = 0; i < this.canvases.Count; i++)
		{
			if (!(this.canvases[i] == null))
			{
				this.canvases[i].gameObject.SetActive(true);
			}
		}
		this.CreateReferences_SaveLoad();
		this.CreateReferences_Locker();
		this.CreateReferences_ChooseItem();
		this.CreateReferences_LoadingCalendar();
		this.CreateRefs_Multiplayer_Menu();
		GameObject.Find("Text_Version").GetComponent<Text>().text = "v" + Application.version.ToString();
	}

	// Token: 0x06000395 RID: 917 RVA: 0x000209C2 File Offset: 0x0001EBC2
	private void Start()
	{
		this.SetMenuName("MainMenu");
		Input_Manager.instance.ResetScheme();
		base.InvokeRepeating("Tasks_Refresh_Panels", 1f, 1f);
	}

	// Token: 0x06000396 RID: 918 RVA: 0x000209EE File Offset: 0x0001EBEE
	private void Update()
	{
	}

	// Token: 0x06000397 RID: 919 RVA: 0x000209F0 File Offset: 0x0001EBF0
	public void PCPostProcess()
	{
		this.pc_camera.GetUniversalAdditionalCameraData().renderPostProcessing = false;
	}

	// Token: 0x06000398 RID: 920 RVA: 0x00020A04 File Offset: 0x0001EC04
	public void UpdateMenu()
	{
		Input_Manager.instance.SetCooldown();
		bool[] array = new bool[this.canvases.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = false;
		}
		if (Input_Manager.instance.GetScheme(-1) == "Keyboard&Mouse")
		{
			Menu_Manager.instance.SetSelector(null, false);
			if (this.menuName != "MainMenu")
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Confined;
			}
		}
		else
		{
			Cursor.visible = false;
		}
		if (this.menuName == "Pause")
		{
			Player_Manager.instance.Set_All_Players_Anim_Update_Mode(AnimatorUpdateMode.Normal);
		}
		if (this.menuName == "PC")
		{
			this.pc_camera.GetUniversalAdditionalCameraData().renderPostProcessing = true;
			this.pc_turnoff_image.SetActive(false);
		}
		else
		{
			Invoker.InvokeDelayed(new Invokable(this.PCPostProcess), 0.25f);
		}
		string text = this.menuName;
		if (text != null)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 1627017944U)
			{
				if (num <= 1157218093U)
				{
					if (num <= 622793711U)
					{
						if (num != 182978943U)
						{
							if (num != 261807733U)
							{
								if (num == 622793711U)
								{
									if (text == "Locations")
									{
										array[18] = true;
										this.RefreshChooseItems();
										this.SetSelector(this.buttonSelectors[18], false);
									}
								}
							}
							else if (text == "Credits")
							{
								array[25] = true;
								array[7] = true;
								this.SetSelector(null, false);
							}
						}
						else if (text == "Start")
						{
							array[7] = true;
							this.SetSelector(this.buttonSelectors[7], false);
						}
					}
					else if (num <= 771752996U)
					{
						if (num != 624200823U)
						{
							if (num == 771752996U)
							{
								if (text == "PC")
								{
									Game_Manager.instance.PauseGame(true);
									array[1] = true;
									array[14] = true;
									array[15] = true;
									PC_Manager.instance.SetTab(PC_Manager.instance.lastTabSelected);
									this.Set_MenuSizeAndPosition(this.canvases[1], this.menuName);
									Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_OpenWindow);
								}
							}
						}
						else if (text == "Discounts")
						{
							array[23] = true;
							this.Refresh_Discounts();
							this.SetSelector(this.buttonSelectors[23], false);
						}
					}
					else if (num != 935656591U)
					{
						if (num == 1157218093U)
						{
							if (text == "Pause")
							{
								Game_Manager.instance.PauseGame(true);
								array[2] = true;
								this.SetSelector(this.buttonSelectors[2], false);
								bool martOpen = Game_Manager.instance.GetMartOpen();
								bool waitingCustomersToLeave = Game_Manager.instance.GetWaitingCustomersToLeave();
								if (!martOpen && !waitingCustomersToLeave)
								{
									this.Set_PauseMenu_SaveButton_State(true);
								}
								else
								{
									this.Set_PauseMenu_SaveButton_State(false);
								}
								this.Refresh_PauseMenu_Buttons_State();
							}
						}
					}
					else if (text == "Warning")
					{
						array[4] = true;
						if (!Game_Manager.instance.MayRun())
						{
							array[7] = true;
						}
						if (this.buttonSelectors[4].gameObject.activeSelf)
						{
							this.SetSelector(this.buttonSelectors[4], false);
						}
						else
						{
							this.SetSelector(this.buttonSelectors[4].GetComponent<MenuNav_Controller>().nav_Right.GetComponent<Button>(), false);
						}
					}
				}
				else if (num <= 1365591205U)
				{
					if (num != 1201566402U)
					{
						if (num != 1258653480U)
						{
							if (num == 1365591205U)
							{
								if (text == "LoadingCalendar")
								{
									array[20] = true;
									array[13] = true;
									this.Refresh_LoadingCalendar();
									this.SetSelector(null, false);
								}
							}
						}
						else if (text == "Settings")
						{
							array[3] = true;
							if (!Game_Manager.instance.MayRun())
							{
								array[7] = true;
							}
							Settings_Manager.instance.RefreshSettingsMenu();
							this.SetSelector(this.buttonSelectors[3], false);
						}
					}
					else if (text == "EE_Game")
					{
						if (Game_Manager.instance.Get_PausedGame())
						{
							Game_Manager.instance.PauseGame(false);
						}
						array[26] = true;
						this.SetSelector(null, false);
					}
				}
				else if (num <= 1439972320U)
				{
					if (num != 1404011351U)
					{
						if (num == 1439972320U)
						{
							if (text == "SettingsUIScale")
							{
								array[8] = true;
								if (!Game_Manager.instance.MayRun())
								{
									array[7] = true;
								}
								Settings_Manager.instance.SetSubMenuUIScale();
							}
						}
					}
					else if (text == "Loading")
					{
						array[11] = true;
						this.SetSelector(null, false);
					}
				}
				else if (num != 1449820501U)
				{
					if (num == 1627017944U)
					{
						if (text == "MiniGame")
						{
							Input_Manager.instance.RefreshInputHints();
							this.SetSelector(null, false);
							Cursor.visible = false;
							Cursor.lockState = CursorLockMode.Locked;
							this.RefreshMainHints();
						}
					}
				}
				else if (text == "Locker")
				{
					Game_Manager.instance.PauseGame(true);
					array[16] = true;
					this.RefreshLocker();
					this.SetSelector(this.locker_Buttons[0].GetComponent<Button>(), false);
					this.Set_MenuSizeAndPosition(this.canvases[16], this.menuName);
				}
			}
			else if (num <= 2583711347U)
			{
				if (num <= 2140782046U)
				{
					if (num != 1825497675U)
					{
						if (num != 2083766451U)
						{
							if (num == 2140782046U)
							{
								if (text == "DayEnding")
								{
									array[12] = true;
									this.SetSelector(this.buttonSelectors[12], false);
									this.RefreshReceiptData();
								}
							}
						}
						else if (text == "LoadingDay")
						{
							array[13] = true;
							this.SetSelector(null, false);
						}
					}
					else if (text == "Multiplayer")
					{
						array[24] = true;
						this.Reset_Multiplayer_Timer();
						this.Refresh_Multiplayer_Menu();
						this.SetSelector(this.class_list_multiplayer_panel[1].button_remove_player, false);
						if (Input_Manager.instance.Get_PlayerQnt() <= 1)
						{
							Input_Manager.instance.RemoveControllers();
						}
					}
				}
				else if (num <= 2471243807U)
				{
					if (num != 2226376863U)
					{
						if (num == 2471243807U)
						{
							if (text == "SettingsScreenMode")
							{
								array[8] = true;
								if (!Game_Manager.instance.MayRun())
								{
									array[7] = true;
								}
								Settings_Manager.instance.SetSubMenuScreenMode();
							}
						}
					}
					else if (text == "Accessibility")
					{
						array[19] = true;
						if (!Game_Manager.instance.MayRun())
						{
							array[7] = true;
						}
						this.SetSelector(this.buttonSelectors[19], false);
					}
				}
				else if (num != 2530449775U)
				{
					if (num == 2583711347U)
					{
						if (text == "SettingsLanguages")
						{
							array[8] = true;
							if (!Game_Manager.instance.MayRun())
							{
								array[7] = true;
							}
							Settings_Manager.instance.SetSubMenuLanguages();
						}
					}
				}
				else if (text == "Talk")
				{
					array[6] = true;
					this.SetSelector(this.buttonSelectors[6], false);
				}
			}
			else if (num <= 2968366269U)
			{
				if (num <= 2881816346U)
				{
					if (num != 2811408835U)
					{
						if (num == 2881816346U)
						{
							if (text == "SettingsResolution")
							{
								array[8] = true;
								if (!Game_Manager.instance.MayRun())
								{
									array[7] = true;
								}
								Settings_Manager.instance.SetSubMenuResolutions();
							}
						}
					}
					else if (text == "DebugTools")
					{
						array[9] = true;
						this.SetSelector(this.buttonSelectors[9], false);
						Cheat_Manager.instance.RefreshDebugMenu();
					}
				}
				else if (num != 2951256388U)
				{
					if (num == 2968366269U)
					{
						if (text == "SettingsPixelation")
						{
							array[8] = true;
							if (!Game_Manager.instance.MayRun())
							{
								array[7] = true;
							}
							Settings_Manager.instance.SetSubMenuPixelation();
						}
					}
				}
				else if (text == "SaveLoad")
				{
					array[10] = true;
					if (!Game_Manager.instance.MayRun())
					{
						array[7] = true;
					}
					this.SetSelector(this.buttonSelectors[10], false);
					this.RefreshSaveLoadInfo();
				}
			}
			else if (num <= 3533655343U)
			{
				if (num != 3421767782U)
				{
					if (num == 3533655343U)
					{
						if (text == "MainMenu")
						{
							if (Game_Manager.instance.Get_PausedGame())
							{
								Game_Manager.instance.PauseGame(false);
							}
							if (Cheat_Manager.instance.GetHideUI() == 0)
							{
								array[0] = true;
							}
							Input_Manager.instance.RefreshInputHints();
							this.SetSelector(null, false);
							Cursor.visible = false;
							Cursor.lockState = CursorLockMode.Locked;
							this.RefreshMainHints();
							this.RefreshSignStateOpen();
							PC_Manager.instance.Refresh_PC_Controller();
							Input_Manager.instance.SetCooldown(0.2f);
							this.Tasks_Refresh_Panels();
							if (this.task_State)
							{
								array[22] = true;
							}
						}
					}
				}
				else if (text == "News")
				{
					array[5] = true;
					this.SetSelector(this.buttonSelectors[5], false);
				}
			}
			else if (num != 3820031919U)
			{
				if (num == 3941134439U)
				{
					if (text == "Friendship")
					{
						array[21] = true;
						this.Refresh_Friendship();
						this.SetSelector(null, false);
					}
				}
			}
			else if (text == "ChooseItem")
			{
				array[17] = true;
				this.RefreshChooseItems();
				this.SetSelector(this.chooseItem_Buttons[0].GetComponent<Button>(), false);
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (!(this.canvases[j] == null))
			{
				this.canvases[j].SetBool("Bool", array[j]);
				if (array[j])
				{
					this.menuIndex = j;
					LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.canvases[j].transform);
				}
			}
		}
		if (Cheat_Manager.instance.GetShowAlphaInfo() == 0)
		{
			this.alphaWarning.SetActive(false);
		}
		else
		{
			this.alphaWarning.SetActive(true);
		}
		IEnumerator routine = this.RefreshCoroutine();
		base.StartCoroutine(routine);
	}

	// Token: 0x06000399 RID: 921 RVA: 0x00021573 File Offset: 0x0001F773
	private IEnumerator RefreshCoroutine()
	{
		int num;
		for (int i = 0; i < 5; i = num + 1)
		{
			yield return null;
			num = i;
		}
		Input_Manager.instance.RefreshInputHintsActive();
		yield break;
	}

	// Token: 0x0600039A RID: 922 RVA: 0x0002157C File Offset: 0x0001F77C
	public void Set_MenuSizeAndPosition(Animator _anim_canvas, string _menu_name)
	{
	}

	// Token: 0x0600039B RID: 923 RVA: 0x00021589 File Offset: 0x0001F789
	public string GetMenuName()
	{
		return this.menuName;
	}

	// Token: 0x0600039C RID: 924 RVA: 0x00021591 File Offset: 0x0001F791
	public int GetMenuIndex()
	{
		return this.menuIndex;
	}

	// Token: 0x0600039D RID: 925 RVA: 0x00021599 File Offset: 0x0001F799
	public void SetMenuName(string _menuName)
	{
		this.SetMenuName(_menuName, this.specific_player_index);
	}

	// Token: 0x0600039E RID: 926 RVA: 0x000215A8 File Offset: 0x0001F7A8
	public void SetMenuName(string _menuName, int _specific_player_index = -1)
	{
		if (_menuName == "MainMenu" || _menuName == "Pause" || _menuName == "Settings" || _menuName == "Multiplayer" || _menuName == "EE_Game" || _menuName == "Start" || _menuName == "Credits" || _menuName == "Cheats" || _menuName == "Debug" || _menuName == "SaveLoad")
		{
			_specific_player_index = -1;
		}
		this.specific_player_index = _specific_player_index;
		if (this.menuName == "PC" && _menuName != "PC" && Cheat_Manager.instance.GetAutoDelivery() == 1f && Inv_Manager.instance.prod_deliveryIndexes.Count > 0 && this.backMenu == "")
		{
			Inv_Manager.instance.DeliverProds_Now();
		}
		if (EasterEgg_Manager.instance.Get_Is_MiniGaming() && _menuName == "MainMenu")
		{
			_menuName = "EE_Game";
		}
		this.menuName = _menuName;
		if (this.backMenu != "")
		{
			this.menuName = this.backMenu;
		}
		if (this.menuName == "MainMenu")
		{
			if (Game_Manager.instance.GetGameMode() != 0)
			{
				this.menuName = "None";
			}
			Inv_Manager.instance.DeliverProds_Today();
		}
		if (this.menuName == "MainMenu" && Time.timeScale == 0f && Cheat_Manager.instance.GetFreezeTimeScale() == 0f)
		{
			Time.timeScale = 1f * Cheat_Manager.instance.GetUltraTimeScaleValue();
		}
		this.UpdateMenu();
		this.backMenu = "";
		Input_Manager.instance.UpdateScheme(true);
	}

	// Token: 0x0600039F RID: 927 RVA: 0x00021778 File Offset: 0x0001F978
	public void SetMenuName(string _menuName, string _backMenu, int _specific_player_index = -1)
	{
		this.specific_player_index = _specific_player_index;
		this.menuName = _menuName;
		if (_backMenu != "")
		{
			this.backMenu = _backMenu;
		}
		if (this.menuName == "MainMenu" && Time.timeScale == 0f)
		{
			Time.timeScale = 1f;
		}
		this.UpdateMenu();
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x000217D8 File Offset: 0x0001F9D8
	public void SetSelector(Button _button, bool _verticalMovement)
	{
		if (Player_Manager.instance.playerControllerList.Count > 1 && Input_Manager.instance.GetScheme(0) != "Joystick" && (this.GetMenuName() == "Pause" || this.GetMenuName() == "Settings" || this.GetMenuName() == "Multiplayer"))
		{
			this.buttonSelected = _button;
			this.selector.gameObject.SetActive(false);
			return;
		}
		if (Input_Manager.instance.GetScheme(-1) != "Joystick")
		{
			this.buttonSelected = _button;
			this.selector.gameObject.SetActive(false);
			return;
		}
		if (_button == null)
		{
			this.buttonSelected = null;
			this.selector.gameObject.SetActive(false);
			this.selector.transform.SetParent(null);
			return;
		}
		this.buttonSelected = _button;
		this.selector.transform.SetParent(this.buttonSelected.transform);
		this.selector.gameObject.SetActive(true);
		this.selector.transform.localPosition = Vector3.zero;
		this.selector.transform.localScale = Vector3.one;
		this.Invoke_RefreshSelector();
		if (this.GetMenuName() != "Talk")
		{
			base.Invoke("Invoke_RefreshSelector", 0.2f);
		}
		if (!_verticalMovement)
		{
			this.selector.PlayInFixedTime("Move", -1, 0f);
		}
		else
		{
			this.selector.PlayInFixedTime("MoveVer", -1, 0f);
		}
		if (this.menuName == "PC")
		{
			if (PC_Manager.instance.GetTab() == 0)
			{
				this.PressButton();
				return;
			}
			if (PC_Manager.instance.GetTab() == 1)
			{
				this.PressButton();
				return;
			}
			if (PC_Manager.instance.GetTab() == 5)
			{
				this.PressButton();
				return;
			}
			if (PC_Manager.instance.GetTab() == 8)
			{
				this.PressButton();
			}
		}
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x000219D8 File Offset: 0x0001FBD8
	private void Invoke_RefreshSelector()
	{
		RectTransform component = this.selector.GetComponent<RectTransform>();
		component.anchorMin = new Vector2(0f, 0f);
		component.anchorMax = new Vector2(1f, 1f);
		component.pivot = new Vector2(0.5f, 0.5f);
		component.sizeDelta = Vector2.zero;
		component.localPosition = Vector3.zero;
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x00021A44 File Offset: 0x0001FC44
	public void PressButton()
	{
		if (!this.buttonSelected)
		{
			return;
		}
		if (this.buttonSelected.interactable)
		{
			this.selector.PlayInFixedTime("Press", -1, 0f);
			this.buttonSelected.onClick.Invoke();
		}
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x00021A94 File Offset: 0x0001FC94
	public void BackMenu()
	{
		Debug.Log("PressedBack");
		if (this.warningList.Count > 1)
		{
			this.FinishWarning();
			return;
		}
		if (this.menuName == "MainMenu")
		{
			return;
		}
		if (this.menuName == "PC")
		{
			this.SetMenuName("MainMenu");
			return;
		}
		if (this.menuName == "Pause")
		{
			this.SetMenuName("MainMenu");
			return;
		}
		if (this.menuName == "Settings")
		{
			Save_Manager.instance.SaveSettings();
			if (Game_Manager.instance.MayRun())
			{
				this.SetMenuName("Pause");
				return;
			}
			this.SetMenuName("Start");
			return;
		}
		else
		{
			if (this.menuName == "Warning")
			{
				this.SetMenuName("MainMenu");
				return;
			}
			if (this.menuName == "News")
			{
				this.SetMenuName("MainMenu");
				return;
			}
			if (this.menuName == "Talk")
			{
				if (Game_Manager.instance.GetGameMode() == 0)
				{
					this.SetMenuName("MainMenu");
					return;
				}
			}
			else
			{
				if (this.menuName == "SettingsScreenMode")
				{
					this.SetMenuName("Settings");
					return;
				}
				if (this.menuName == "SettingsResolution")
				{
					this.SetMenuName("Settings");
					return;
				}
				if (this.menuName == "SettingsLanguages")
				{
					this.SetMenuName("Settings");
					return;
				}
				if (this.menuName == "SettingsPixelation")
				{
					this.SetMenuName("Settings");
					return;
				}
				if (this.menuName == "SettingsUIScale")
				{
					this.SetMenuName("Settings");
					return;
				}
				if (this.menuName == "DebugTools")
				{
					this.SetMenuName("MainMenu");
					return;
				}
				if (this.menuName == "SaveLoad")
				{
					if (Game_Manager.instance.MayRun())
					{
						this.SetMenuName("Pause");
						return;
					}
					this.SetMenuName("Start");
					return;
				}
				else
				{
					if (this.menuName == "Locker")
					{
						this.SetMenuName("MainMenu");
						return;
					}
					if (this.menuName == "ChooseItem")
					{
						this.SetMenuName("PC");
						return;
					}
					if (this.menuName == "Locations")
					{
						this.SetMenuName("PC");
						return;
					}
					if (this.menuName == "Accessibility")
					{
						if (Game_Manager.instance.MayRun())
						{
							this.SetMenuName("Pause");
							return;
						}
						this.SetMenuName("Start");
						return;
					}
					else
					{
						if (this.menuName == "Friendship")
						{
							this.SetMenuName("MainMenu");
							return;
						}
						if (this.menuName == "Multiplayer")
						{
							this.SetMenuName("Pause");
							return;
						}
						if (this.menuName == "Credits")
						{
							this.SetMenuName("Start");
						}
					}
				}
			}
			return;
		}
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x00021D8C File Offset: 0x0001FF8C
	public void MenuNav(int _x, int _y)
	{
		if (!this.buttonSelected)
		{
			return;
		}
		GameObject button = this.buttonSelected.GetComponent<MenuNav_Controller>().GetButton(_x, _y);
		if (button)
		{
			bool verticalMovement = false;
			if (_x != 0)
			{
				verticalMovement = true;
			}
			this.SetSelector(button.GetComponent<Button>(), verticalMovement);
			if (this.menuName == "SaveLoad")
			{
				this.PressButton();
			}
		}
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x00021DEE File Offset: 0x0001FFEE
	public Button GetButtonSelected()
	{
		if (this.buttonSelected)
		{
			return this.buttonSelected.GetComponent<Button>();
		}
		return null;
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x00021E0A File Offset: 0x0002000A
	public bool Get_Same_Specific_Player_Index(int _player_index)
	{
		return Player_Manager.instance.playerControllerList.Count <= 1 || this.specific_player_index == -1 || this.specific_player_index == _player_index;
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x00021E38 File Offset: 0x00020038
	public void RefreshMoney(float _money)
	{
		for (int i = 0; i < this.text_Money.Count; i++)
		{
			if (this.text_Money[i])
			{
				this.text_Money[i].text = _money.ToString();
			}
		}
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x00021E88 File Offset: 0x00020088
	public void RefreshTime(float _amount)
	{
		this.image_TimeLeft[0].fillAmount = _amount;
		this.image_TimeLeft[1].fillAmount = (float)World_Manager.instance.startTime_NoDelivery_InSeconds / (float)World_Manager.instance.startTimeInSeconds + 0.02f;
		this.image_TimeLeft[2].fillAmount = (float)World_Manager.instance.startTime_NoDelivery_InSeconds / (float)World_Manager.instance.startTimeInSeconds;
		this.image_TimeLeft[3].fillAmount = Mathf.Clamp(_amount, 0f, (float)World_Manager.instance.startTime_NoDelivery_InSeconds / (float)World_Manager.instance.startTimeInSeconds);
		this.RefreshDeliveryState();
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x00021F28 File Offset: 0x00020128
	public void RefreshDeliveryState()
	{
		int deliveryAvailable_State = World_Manager.instance.Get_DeliveryAvailable_State();
		if (this.image_delivery_state.sprite != this.delivery_state_sprites[deliveryAvailable_State])
		{
			this.image_delivery_state.sprite = this.delivery_state_sprites[deliveryAvailable_State];
		}
	}

	// Token: 0x060003AA RID: 938 RVA: 0x00021F70 File Offset: 0x00020170
	public void RefreshSignStateOpen()
	{
		bool martOpen = Game_Manager.instance.GetMartOpen();
		bool waitingCustomersToLeave = Game_Manager.instance.GetWaitingCustomersToLeave();
		this.panel_OpenState.gameObject.SetActive(martOpen);
		this.RebuildCanvas(this.panel_OpenState.transform.parent.GetComponent<ContentSizeFitter>());
		this.panel_WaitCustomers.gameObject.SetActive(waitingCustomersToLeave);
		this.RebuildCanvas(this.panel_WaitCustomers.transform.parent.GetComponent<ContentSizeFitter>());
		bool active = false;
		if (!martOpen && !waitingCustomersToLeave)
		{
			active = true;
		}
		this.panel_ClosedState.gameObject.SetActive(active);
		this.RebuildCanvas(this.panel_ClosedState.transform.parent.GetComponent<ContentSizeFitter>());
	}

	// Token: 0x060003AB RID: 939 RVA: 0x00022021 File Offset: 0x00020221
	public void RebuildCanvas(ContentSizeFitter _contentSizeFitter)
	{
		_contentSizeFitter.enabled = false;
		_contentSizeFitter.SetLayoutVertical();
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_contentSizeFitter.transform);
		_contentSizeFitter.enabled = true;
	}

	// Token: 0x060003AC RID: 940 RVA: 0x00022047 File Offset: 0x00020247
	public void RefreshMainHints()
	{
		Interactor_Manager.instance.RefreshInputHints();
		base.Invoke("Inv_RefreshInputHintsActive", 0.2f);
	}

	// Token: 0x060003AD RID: 941 RVA: 0x00022063 File Offset: 0x00020263
	public void Inv_RefreshInputHintsActive()
	{
		Input_Manager.instance.RefreshInputHintsActive();
	}

	// Token: 0x060003AE RID: 942 RVA: 0x00022070 File Offset: 0x00020270
	public void SetWarning(List<string> _title, List<string> _string, int _specific_player_index)
	{
		this.warningList = new List<string>(_string);
		this.warningTitleList = new List<string>(_title);
		if (_title[0] != "")
		{
			this.text_WarningTitle.text = _title[0];
			this.text_WarningTitle.gameObject.SetActive(true);
		}
		else
		{
			this.text_WarningTitle.gameObject.SetActive(false);
		}
		this.text_Warning.text = this.warningList[0];
		this.SetWarningButtonsActive(true);
		this.SetMenuName("Warning", _specific_player_index);
		this.RebuidWarningCanvas();
	}

	// Token: 0x060003AF RID: 943 RVA: 0x00022110 File Offset: 0x00020310
	public void SetWarning(string _title, string _string, string _backMenu, bool _simpleWarning, int _specific_player_index)
	{
		this.warningList.Clear();
		this.warningTitleList.Clear();
		if (_title != "")
		{
			this.text_WarningTitle.text = _title;
			this.text_WarningTitle.gameObject.SetActive(true);
		}
		else
		{
			this.text_WarningTitle.gameObject.SetActive(false);
		}
		this.text_Warning.text = _string;
		this.SetWarningButtonsActive(_simpleWarning);
		this.SetMenuName("Warning", _backMenu, this.specific_player_index);
		this.RebuidWarningCanvas();
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x0002219C File Offset: 0x0002039C
	public Button SetWarningConfirmation(string _string, int _specific_player_index, string _backMenu)
	{
		this.button_WarningConfirmation[2].gameObject.SetActive(true);
		string text = Language_Manager.instance.GetText("Are you sure?");
		_string = Language_Manager.instance.GetText(_string);
		if (_backMenu == "")
		{
			_backMenu = this.GetMenuName();
		}
		this.SetWarning(text, _string, _backMenu, false, _specific_player_index);
		this.button_WarningConfirmation[2].onClick.RemoveAllListeners();
		return this.button_WarningConfirmation[2];
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x00022214 File Offset: 0x00020414
	public void SetWarningButtonsActive(bool _simpleWarning)
	{
		for (int i = 0; i < this.button_WarningConfirmation.Length; i++)
		{
			if (_simpleWarning && i == 0)
			{
				this.button_WarningConfirmation[i].gameObject.SetActive(true);
			}
			else if (_simpleWarning || i == 0)
			{
				this.button_WarningConfirmation[i].gameObject.SetActive(false);
			}
			else
			{
				this.button_WarningConfirmation[i].gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x0002227D File Offset: 0x0002047D
	private void RebuidWarningCanvas()
	{
		ContentSizeFitter component = this.text_Warning.transform.parent.GetComponent<ContentSizeFitter>();
		component.enabled = false;
		component.SetLayoutVertical();
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)component.transform);
		component.enabled = true;
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x000222B8 File Offset: 0x000204B8
	public void FinishWarning()
	{
		if (this.warningList.Count > 1)
		{
			this.warningTitleList.RemoveAt(0);
			this.warningList.RemoveAt(0);
			this.SetWarning(this.warningTitleList, this.warningList, this.specific_player_index);
			return;
		}
		this.BackMenu();
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x0002230A File Offset: 0x0002050A
	public void AnimateMoneyUI(bool _addMoney)
	{
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0002230C File Offset: 0x0002050C
	public void StartScreen(int _stage)
	{
		if (_stage == 0)
		{
			this.canvas_StartScreen.PlayInFixedTime("On", -1, 0f);
			return;
		}
		if (_stage == 1)
		{
			this.canvas_StartScreen.PlayInFixedTime("Off", -1, 0f);
		}
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x00022344 File Offset: 0x00020544
	public void SetNotification(string _title, string _content, Sprite _itemSprite, float _timer, bool _translate)
	{
		if (Cheat_Manager.instance.GetDebugNotification() == 0)
		{
			return;
		}
		if (_title == null)
		{
			_title = "Attention!";
		}
		if (_content == null)
		{
			return;
		}
		if (_translate)
		{
			_title = Language_Manager.instance.GetText(_title);
			_content = Language_Manager.instance.GetText(_content);
		}
		Notification_Controller notification_Controller = UnityEngine.Object.Instantiate<Notification_Controller>(this.notification_Default.gameObject.GetComponent<Notification_Controller>());
		notification_Controller.transform.Find("Text_Title").GetComponent<Text>().text = _title;
		notification_Controller.transform.Find("Text_Content").GetComponent<Text>().text = _content;
		notification_Controller.SetTimer(_timer);
		notification_Controller.transform.SetParent(this.notification_Default.transform.parent);
		notification_Controller.gameObject.SetActive(true);
		this.notification_List.Add(notification_Controller);
		Image component = notification_Controller.transform.Find("Image_Item").GetComponent<Image>();
		if (_itemSprite)
		{
			component.gameObject.SetActive(true);
			component.sprite = _itemSprite;
			component.SetNativeSize();
		}
		else
		{
			component.gameObject.SetActive(false);
		}
		ContentSizeFitter component2 = notification_Controller.GetComponent<ContentSizeFitter>();
		if (component2)
		{
			component2.enabled = false;
			component2.SetLayoutVertical();
			component2.SetLayoutHorizontal();
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)component2.transform);
			component2.enabled = true;
		}
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x00022491 File Offset: 0x00020691
	public void SetNotification(string _title, string _text, bool _translate)
	{
		this.SetNotification(_title, _text, null, this.defaultNotificationTimer, _translate);
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x000224A3 File Offset: 0x000206A3
	public void SetNotification(string _title, string _text, bool _translate, float _timer)
	{
		this.SetNotification(_title, _text, null, _timer, _translate);
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x000224B1 File Offset: 0x000206B1
	public bool DeleteNotification(Notification_Controller _notification)
	{
		if (this.notification_List.Contains(_notification))
		{
			this.notification_List.Remove(_notification);
			UnityEngine.Object.Destroy(_notification.gameObject);
			return true;
		}
		return false;
	}

	// Token: 0x060003BA RID: 954 RVA: 0x000224DC File Offset: 0x000206DC
	public void SetNotificationSaving()
	{
		this.notification_Saving.SetActive(true);
	}

	// Token: 0x060003BB RID: 955 RVA: 0x000224EC File Offset: 0x000206EC
	public void RefreshCalendar()
	{
		this.calendar_MainMenu_Arrow.transform.SetParent(this.calendar_MainMenu_WeekDays[World_Manager.instance.GetDayIndex()].transform);
		this.calendar_MainMenu_Arrow.transform.localPosition = Vector3.zero;
		this.calendar_MainMenu_Arrow.transform.localScale = Vector3.one;
	}

	// Token: 0x060003BC RID: 956 RVA: 0x00022550 File Offset: 0x00020750
	private void CreateReferences_SaveLoad()
	{
		for (int i = 0; i < this.button_SaveLoad_GOs.Count; i++)
		{
			this.button_SaveLoad_PanelInfo.Add(this.button_SaveLoad_GOs[i].transform.Find("Panel_Info").gameObject);
			this.button_SaveLoad_PanelAdd.Add(this.button_SaveLoad_GOs[i].transform.Find("Panel_Add").gameObject);
			this.button_SaveLoad_TextMartName.Add(this.button_SaveLoad_PanelInfo[i].transform.Find("Text_MartName").GetComponent<Text>());
			Transform transform = this.button_SaveLoad_PanelInfo[i].transform.Find("Panel_0");
			this.button_SaveLoad_TextMoney.Add(transform.Find("Panel_Money").Find("Text").GetComponent<Text>());
			this.button_SaveLoad_TextTimePlayed.Add(transform.Find("Panel_Time").Find("Text").GetComponent<Text>());
			this.button_SaveLoad_TextEmployees.Add(transform.Find("Panel_Employees").Find("Text").GetComponent<Text>());
			this.button_SaveLoad_TextCustomers.Add(transform.Find("Panel_Customers").Find("Text").GetComponent<Text>());
			this.button_SaveLoad_TextAwards.Add(transform.Find("Panel_Awards").Find("Text").GetComponent<Text>());
		}
	}

	// Token: 0x060003BD RID: 957 RVA: 0x000226D0 File Offset: 0x000208D0
	public void RefreshSaveLoadInfo()
	{
		for (int i = 0; i < this.button_SaveLoad_GOs.Count; i++)
		{
			ArrayList arrayList = Save_Manager.instance.LoadGameInfo(i);
			if ((int)arrayList[0] == 1)
			{
				Debug.Log((int)arrayList[0]);
				this.button_SaveLoad_PanelInfo[i].SetActive(true);
				this.button_SaveLoad_PanelAdd[i].SetActive(false);
				this.button_SaveLoad_TextMartName[i].text = (string)arrayList[1];
				this.button_SaveLoad_TextMoney[i].text = ((int)((float)arrayList[2])).ToString() + " " + Language_Manager.instance.GetText("coins");
				this.button_SaveLoad_TextTimePlayed[i].text = World_Manager.instance.GetPlayTimeStringConverted((float)arrayList[3]);
				this.button_SaveLoad_TextEmployees[i].text = ((int)arrayList[4]).ToString();
				this.button_SaveLoad_TextCustomers[i].text = ((int)arrayList[5]).ToString();
				this.button_SaveLoad_TextAwards[i].text = string.Concat(new string[]
				{
					((int)arrayList[6]).ToString(),
					"/",
					Missions_Manager.instance.GetTotalAwardsNumber().ToString(),
					" ",
					Language_Manager.instance.GetText("awards")
				});
			}
			else
			{
				this.button_SaveLoad_PanelInfo[i].SetActive(false);
				this.button_SaveLoad_PanelAdd[i].SetActive(true);
			}
		}
	}

	// Token: 0x060003BE RID: 958 RVA: 0x000228B0 File Offset: 0x00020AB0
	public void RefreshReceiptData()
	{
		this.text_Receipt_MartName.text = "MiniMart";
		this.text_Receipt_Season.text = World_Manager.instance.GetSeasonName();
		this.text_Receipt_Day.text = World_Manager.instance.GetDayName();
		this.text_Receipt_Week.text = Language_Manager.instance.GetText("week") + " " + World_Manager.instance.GetWeekName();
		string text = " " + Language_Manager.instance.GetText("Coins");
		this.text_Receipt_ProdBought.text = "-" + Finances_Manager.instance.GetList_OutProds()[0].ToString() + text;
		this.text_Receipt_Furniture.text = "-" + Finances_Manager.instance.GetList_OutFurniture()[0].ToString() + text;
		this.text_Receipt_Staff.text = "-" + Finances_Manager.instance.GetList_OutStaff()[0].ToString() + text;
		this.text_Receipt_Expansion.text = "-" + Finances_Manager.instance.GetList_OutExpansion()[0].ToString() + text;
		this.text_Receipt_Marketing.text = "-" + Finances_Manager.instance.GetList_OutMarketing()[0].ToString() + text;
		this.text_Receipt_Operational.text = "-" + Finances_Manager.instance.GetList_OutOperational()[0].ToString() + text;
		this.text_Receipt_ProdSold.text = "+" + Finances_Manager.instance.GetList_InSales()[0].ToString() + text;
		float num = Finances_Manager.instance.GetMoneyInByIndex(0) - Finances_Manager.instance.GetMoneyOutByIndex(0);
		if (num > 0f)
		{
			this.text_Receipt_CoinsBalance.text = "+" + num.ToString() + text;
			return;
		}
		this.text_Receipt_CoinsBalance.text = num.ToString() + text;
	}

	// Token: 0x060003BF RID: 959 RVA: 0x00022AD8 File Offset: 0x00020CD8
	public void Locker_SelectButton(int _index)
	{
		Debug.Log("MenuManager - Locker index: " + _index.ToString());
		if (this.locker_Tab_Index == 0)
		{
			Player_Manager.instance.SetPlayerSkinColor(_index, this.locker_player_index);
		}
		else if (this.locker_Tab_Index == 1)
		{
			Player_Manager.instance.SetPlayerOutfit(_index, this.locker_player_index);
		}
		else if (this.locker_Tab_Index == 2)
		{
			Player_Manager.instance.SetPlayerHairColor(_index, this.locker_player_index);
		}
		else if (this.locker_Tab_Index == 3)
		{
			Player_Manager.instance.SetPlayerHair(_index, this.locker_player_index);
		}
		else if (this.locker_Tab_Index == 4)
		{
			Player_Manager.instance.SetPlayerEyes(_index, this.locker_player_index);
		}
		else if (this.locker_Tab_Index == 5)
		{
			Player_Manager.instance.SetPlayerHat(_index, this.locker_player_index);
		}
		this.RefreshLockerSelection();
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x00022BAC File Offset: 0x00020DAC
	private void CreateReferences_Locker()
	{
		for (int i = 0; i < 100; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.locker_ButtonPrefab);
			gameObject.transform.SetParent(this.locker_ButtonPrefab.transform.parent);
			this.locker_Buttons.Add(gameObject.GetComponent<Image>());
			this.locker_Buttons_BUTTONS.Add(gameObject.GetComponent<Button>());
			this.locker_ButtonIcons.Add(gameObject.transform.Find("Image_").GetComponent<Image>());
			int _index = i;
			gameObject.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.Locker_SelectButton(_index);
			});
		}
		this.locker_ButtonPrefab.SetActive(false);
		this.locker_Tabs[0].transform.parent.parent.Find("Panel_Title").Find("Button_Back").GetComponent<MenuNav_Controller>().nav_Down = this.locker_Buttons[0].gameObject;
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x00022CBC File Offset: 0x00020EBC
	public void RefreshLocker()
	{
		this.locker_TabText.text = Language_Manager.instance.GetText(this.locker_TabNames[this.locker_Tab_Index]);
		Player_Manager.instance.GetPlayerController(this.locker_player_index).skin_.GetCompleteCustomization();
		for (int i = 0; i < this.locker_Tabs.Count; i++)
		{
			if (i == this.locker_Tab_Index)
			{
				this.locker_Tabs[i].color = this.locker_ButtonColors[1];
			}
			else
			{
				this.locker_Tabs[i].color = this.locker_ButtonColors[0];
			}
		}
		List<Sprite> list = new List<Sprite>();
		if (this.locker_Tab_Index == 0)
		{
			list = this.locker_SkinColor_Sprites;
		}
		else if (this.locker_Tab_Index == 1)
		{
			list = this.locker_Outfit_Sprites;
		}
		else if (this.locker_Tab_Index == 2)
		{
			list = this.locker_HairColor_Sprites;
		}
		else if (this.locker_Tab_Index == 3)
		{
			list = this.locker_Hair_Sprites;
		}
		else if (this.locker_Tab_Index == 4)
		{
			list = this.locker_Eyes_Sprites;
		}
		else if (this.locker_Tab_Index == 5)
		{
			list = this.locker_Hats_Sprites;
		}
		for (int j = 0; j < this.locker_Buttons.Count; j++)
		{
			if (j < list.Count)
			{
				this.locker_Buttons[j].gameObject.SetActive(true);
				this.locker_Buttons[j].transform.localScale = Vector3.one;
				this.locker_ButtonIcons[j].sprite = list[j];
				this.locker_ButtonIcons[j].SetNativeSize();
				if (Unlock_Manager.instance.item_Unlocked[this.locker_Tab_Index + 6, j])
				{
					this.locker_ButtonIcons[j].color = PC_Manager.instance.shop_UnlockedColors[1];
					this.locker_Buttons[j].enabled = true;
					this.locker_Buttons_BUTTONS[j].interactable = true;
				}
				else
				{
					this.locker_ButtonIcons[j].color = PC_Manager.instance.shop_UnlockedColors[0];
					this.locker_Buttons[j].enabled = false;
					this.locker_Buttons_BUTTONS[j].interactable = false;
				}
			}
			else
			{
				this.locker_Buttons[j].gameObject.SetActive(false);
			}
		}
		int num = this.locker_Width;
		List<GameObject> list2 = new List<GameObject>();
		for (int k = 0; k < this.locker_Buttons.Count; k++)
		{
			if (this.locker_Buttons[k].gameObject.activeSelf)
			{
				list2.Add(this.locker_Buttons[k].gameObject);
			}
		}
		for (int l = 0; l < list2.Count; l++)
		{
			list2[l].GetComponent<MenuNav_Controller>().ResetButtons();
			if (l >= num)
			{
				list2[l].GetComponent<MenuNav_Controller>().nav_Up = list2[l - num];
			}
			if (l > 0)
			{
				list2[l].GetComponent<MenuNav_Controller>().nav_Left = list2[l - 1];
			}
			if (l < list2.Count - 1)
			{
				list2[l].GetComponent<MenuNav_Controller>().nav_Right = list2[l + 1];
			}
			if (l < list2.Count - num)
			{
				list2[l].GetComponent<MenuNav_Controller>().nav_Down = list2[l + num];
			}
		}
		this.RefreshLockerSelection();
		this.SetSelector(list2[0].GetComponent<Button>(), false);
		this.locker_scroll_rect_auto_scroll._Start();
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x00023064 File Offset: 0x00021264
	public void RefreshLockerSelection()
	{
		int[] completeCustomizationUI = Player_Manager.instance.GetPlayerController(this.locker_player_index).skin_.GetCompleteCustomizationUI();
		for (int i = 0; i < this.locker_Buttons.Count; i++)
		{
			if (i == completeCustomizationUI[this.locker_Tab_Index])
			{
				this.locker_Buttons[i].color = this.locker_ButtonColors[1];
			}
			else
			{
				this.locker_Buttons[i].color = this.locker_ButtonColors[0];
			}
		}
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x000230E9 File Offset: 0x000212E9
	public void SetLockerTab(int _index)
	{
		this.locker_Tab_Index = _index;
		this.RefreshLocker();
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x0002310C File Offset: 0x0002130C
	public void NavLockerTab(int _nav)
	{
		this.locker_Tab_Index += _nav;
		if (this.locker_Tab_Index < 0)
		{
			this.locker_Tab_Index = this.locker_Tabs.Count - 1;
		}
		else if (this.locker_Tab_Index >= this.locker_Tabs.Count)
		{
			this.locker_Tab_Index = 0;
		}
		this.RefreshLocker();
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_ClickButton);
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x0002317C File Offset: 0x0002137C
	public void CreateReferences_ChooseItem()
	{
		for (int i = 0; i < 150; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.button_ChooseItem_Prefab);
			gameObject.transform.SetParent(this.button_ChooseItem_Prefab.transform.parent);
			gameObject.transform.localScale = Vector3.one;
			gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
			this.chooseItem_Buttons.Add(gameObject);
			this.chooseItem_TextNames.Add(gameObject.transform.Find("Text").GetComponent<Text>());
			this.chooseItem_ItemImages.Add(gameObject.transform.Find("Image").GetComponent<Image>());
		}
		this.button_ChooseItem_Prefab.SetActive(false);
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x0002323E File Offset: 0x0002143E
	public void SelectChooseItem(int _index)
	{
		Finances_Manager.instance.AddMoney((float)(-(float)Inv_Manager.instance.news_Deals_Price));
		Finances_Manager.instance.AddTo_OutMarketing((float)Inv_Manager.instance.news_Deals_Price);
		this.BackMenu();
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x00023274 File Offset: 0x00021474
	public void RefreshChooseItems()
	{
		for (int i = 0; i < this.chooseItem_Buttons.Count; i++)
		{
			this.chooseItem_Buttons[i].GetComponent<Button>().onClick.RemoveAllListeners();
			bool flag = true;
			List<Prod_Controller> ownedProds = Inv_Manager.instance.GetOwnedProds();
			if (i < ownedProds.Count)
			{
				flag = false;
				this.chooseItem_Buttons[i].SetActive(true);
				int _index = Inv_Manager.instance.GetItemIndex(ownedProds[i].gameObject);
				this.chooseItem_TextNames[i].text = Inv_Manager.instance.GetProdName(_index, true);
				this.chooseItem_ItemImages[i].sprite = Inv_Manager.instance.GetProdSprite(_index);
				this.chooseItem_ItemImages[i].SetNativeSize();
				this.chooseItem_Buttons[i].GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.SelectChooseItem(_index);
				});
			}
			if (flag)
			{
				this.chooseItem_Buttons[i].SetActive(false);
			}
		}
		int num = 8;
		List<GameObject> list = new List<GameObject>();
		for (int j = 0; j < this.chooseItem_Buttons.Count; j++)
		{
			if (this.chooseItem_Buttons[j].activeSelf)
			{
				list.Add(this.chooseItem_Buttons[j]);
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			list[k].GetComponent<MenuNav_Controller>().ResetButtons();
			if (k >= num)
			{
				list[k].GetComponent<MenuNav_Controller>().nav_Up = list[k - num];
			}
			if (k > 0)
			{
				list[k].GetComponent<MenuNav_Controller>().nav_Left = list[k - 1];
			}
			if (k < list.Count - 1)
			{
				list[k].GetComponent<MenuNav_Controller>().nav_Right = list[k + 1];
			}
			if (k < list.Count - num)
			{
				list[k].GetComponent<MenuNav_Controller>().nav_Down = list[k + num];
			}
		}
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x000234B0 File Offset: 0x000216B0
	public void CreateReferences_LoadingCalendar()
	{
		for (int i = 0; i < 3; i++)
		{
			Class_LoadingCalendar class_LoadingCalendar = new Class_LoadingCalendar();
			class_LoadingCalendar.mainPanel = this.panelParent_LoadingCalendar.transform.Find("Image_Calendar (" + i.ToString() + ")");
			Transform transform = class_LoadingCalendar.mainPanel.Find("Panel");
			class_LoadingCalendar.text_Season = transform.Find("Text_Season").GetComponent<TMP_Text>();
			class_LoadingCalendar.image_Forecast = transform.Find("Image_Forecast").GetComponent<Image>();
			class_LoadingCalendar.text_Day = class_LoadingCalendar.mainPanel.Find("Text_Day").GetComponent<TMP_Text>();
			this.class_LoadingCalendars.Add(class_LoadingCalendar);
		}
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x00023568 File Offset: 0x00021768
	public void Refresh_LoadingCalendar()
	{
		for (int i = 0; i < this.class_LoadingCalendars.Count; i++)
		{
			int num = World_Manager.instance.GetDayIndex() + i;
			int num2 = World_Manager.instance.GetSeasonIndex();
			int weekIndex = World_Manager.instance.GetWeekIndex();
			if (num > 6)
			{
				num -= 7;
				if (weekIndex == World_Manager.instance.GetWeekMax())
				{
					num2++;
					if (num2 >= World_Manager.instance.GetSeasonMax())
					{
						num2 = 0;
					}
				}
			}
			string dayNameByIndex = World_Manager.instance.GetDayNameByIndex(num);
			string seasonNameByIndex = World_Manager.instance.GetSeasonNameByIndex(num2);
			Class_LoadingCalendar class_LoadingCalendar = this.class_LoadingCalendars[i];
			class_LoadingCalendar.text_Season.text = seasonNameByIndex;
			class_LoadingCalendar.text_Day.text = dayNameByIndex;
			class_LoadingCalendar.image_Forecast.sprite = World_Manager.instance.GetForecastSpriteByIndex(World_Manager.instance.climate_Indexes[i]);
		}
	}

	// Token: 0x060003CA RID: 970 RVA: 0x00023640 File Offset: 0x00021840
	public void SetFriendship(Char_Controller _char)
	{
		this.friendship_Image.sprite = _char.charFriendshipPhoto;
		this.friendship_Image.SetNativeSize();
		this.SetMenuName("Friendship");
	}

	// Token: 0x060003CB RID: 971 RVA: 0x00023669 File Offset: 0x00021869
	public void Refresh_Friendship()
	{
	}

	// Token: 0x060003CC RID: 972 RVA: 0x0002366C File Offset: 0x0002186C
	public void Refresh_Discounts()
	{
		int prodDiscountLevel = Inv_Manager.instance.GetProdDiscountLevel(Inv_Manager.instance.discount_prod_index);
		bool active = false;
		if (prodDiscountLevel != -1)
		{
			active = true;
		}
		foreach (GameObject gameObject in this.discounts_remove_gos)
		{
			gameObject.SetActive(active);
		}
	}

	// Token: 0x060003CD RID: 973 RVA: 0x000236D8 File Offset: 0x000218D8
	public void Select_Discount(int _index)
	{
		Inv_Manager.instance.Select_Discount_Button(_index);
		this.SetMenuName("MainMenu");
	}

	// Token: 0x060003CE RID: 974 RVA: 0x000236F0 File Offset: 0x000218F0
	public void Set_PauseMenu_SaveButton_State(bool _b)
	{
		this.pauseMenu_SaveButton_GO.SetActive(_b);
	}

	// Token: 0x060003CF RID: 975 RVA: 0x00023700 File Offset: 0x00021900
	public void Refresh_PauseMenu_Buttons_State()
	{
		this.pauseMenu_MultiplayerButton_On_GO.SetActive(false);
		this.pauseMenu_MultiplayerButton_Off_GO.SetActive(false);
		if (Game_Manager.instance.permit_multiplayer && !EasterEgg_Manager.instance.Get_Is_MiniGaming())
		{
			this.pauseMenu_MultiplayerButton_On_GO.SetActive(true);
		}
		this.pauseMenu_EE_SoccerMiniGame_Start.SetActive(false);
		if (!EasterEgg_Manager.instance.Get_Is_MiniGaming() && !Game_Manager.instance.GetMartOpen())
		{
			this.pauseMenu_EE_SoccerMiniGame_Start.SetActive(true);
		}
		this.pauseMenu_EE_MiniGame_Stop.SetActive(false);
		if (EasterEgg_Manager.instance.Get_Is_MiniGaming())
		{
			this.pauseMenu_EE_MiniGame_Stop.SetActive(true);
		}
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x000237A0 File Offset: 0x000219A0
	public void CreateRefs_Multiplayer_Menu()
	{
		for (int i = 0; i < 4; i++)
		{
			Menu_Manager.Class_Multiplayer_Panel class_Multiplayer_Panel = new Menu_Manager.Class_Multiplayer_Panel();
			this.class_list_multiplayer_panel.Add(class_Multiplayer_Panel);
			class_Multiplayer_Panel.CreateRefs(this.multiplayer_main_panel_ref, i);
		}
		this.multiplayer_main_panel_ref.SetActive(false);
		for (int j = 0; j < this.class_list_multiplayer_panel.Count; j++)
		{
			Menu_Manager.Class_Multiplayer_Panel class_Multiplayer_Panel2 = this.class_list_multiplayer_panel[j];
			if (j != 0)
			{
				if (j == 1)
				{
					class_Multiplayer_Panel2.button_remove_player.GetComponent<MenuNav_Controller>().nav_Right = this.class_list_multiplayer_panel[j + 1].button_remove_player.gameObject;
				}
				else if (j < this.class_list_multiplayer_panel.Count - 1)
				{
					class_Multiplayer_Panel2.button_remove_player.GetComponent<MenuNav_Controller>().nav_Left = this.class_list_multiplayer_panel[j - 1].button_remove_player.gameObject;
					class_Multiplayer_Panel2.button_remove_player.GetComponent<MenuNav_Controller>().nav_Right = this.class_list_multiplayer_panel[j + 1].button_remove_player.gameObject;
				}
				else
				{
					class_Multiplayer_Panel2.button_remove_player.GetComponent<MenuNav_Controller>().nav_Left = this.class_list_multiplayer_panel[j - 1].button_remove_player.gameObject;
				}
			}
		}
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x000238D0 File Offset: 0x00021AD0
	public void Refresh_Multiplayer_Menu()
	{
		if (this.menuName == "Multiplayer")
		{
			for (int i = 0; i < this.class_list_multiplayer_panel.Count; i++)
			{
				if (i < Player_Manager.instance.playerControllerList.Count)
				{
					this.class_list_multiplayer_panel[i].Set_Player(Player_Manager.instance.GetPlayerController(i), this.multiplayer_player_texture);
				}
				else
				{
					this.class_list_multiplayer_panel[i].Set_Player(null, null);
				}
			}
			Player_Manager.instance.Set_All_Players_Anim_Update_Mode(AnimatorUpdateMode.UnscaledTime);
			this.SetSelector(this.class_list_multiplayer_panel[1].button_remove_player, false);
		}
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x00023974 File Offset: 0x00021B74
	public void Reset_Multiplayer_Timer()
	{
		this.multiplayer_timer = this.multiplayer_timer_start;
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x00023982 File Offset: 0x00021B82
	public void Tasks_Set_State(bool _b)
	{
		this.task_State = _b;
		this.UpdateMenu();
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x00023991 File Offset: 0x00021B91
	public void Tasks_Cycle_State()
	{
		this.task_State = !this.task_State;
		this.Tasks_Set_State(this.task_State);
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x000239B0 File Offset: 0x00021BB0
	public void Tasks_Refresh_Panels()
	{
		if (this.task_State)
		{
			this.tasks_ButtonOpener.color = this.tasks_ButtonColor[1];
		}
		else
		{
			this.tasks_ButtonOpener.color = this.tasks_ButtonColor[0];
		}
		if (!this.task_State)
		{
			return;
		}
		List<TaskData> all_TaskDatas_Current = Missions_Manager.instance.Get_All_TaskDatas_Current();
		List<GameObject> list = new List<GameObject>(this.tasks_Panels);
		this.tasks_Panels.Clear();
		for (int i = 0; i < all_TaskDatas_Current.Count; i++)
		{
			GameObject gameObject = all_TaskDatas_Current[i].UI;
			if (!gameObject)
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this.tasks_PanelPrefab);
			}
			all_TaskDatas_Current[i].UI = gameObject;
			this.tasks_Panels.Add(gameObject);
			gameObject.transform.SetParent(this.tasks_PanelParent.transform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
			gameObject.transform.localScale = Vector3.one;
			gameObject.SetActive(true);
			if (list.Contains(gameObject))
			{
				list.Remove(gameObject);
			}
		}
		list.TrimExcess();
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j])
			{
				list[j].GetComponent<TaskUI_Controller>().MayRun();
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.tasks_PanelParent.transform);
		this.Tasks_Refresh_Description();
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x00023B2C File Offset: 0x00021D2C
	public void Tasks_Refresh_Description()
	{
		List<TaskData> list = new List<TaskData>(Missions_Manager.instance.Get_All_TaskDatas_Current());
		for (int i = 0; i < list.Count; i++)
		{
			TaskData taskData = list[i];
			GameObject ui = list[i].UI;
			Text component = ui.transform.Find("Text_Title").GetComponent<Text>();
			Text component2 = ui.transform.Find("Text_Content").GetComponent<Text>();
			Text component3 = ui.transform.Find("Text_Qnt").GetComponent<Text>();
			Image component4 = ui.transform.Find("Image_Owner").GetComponent<Image>();
			Transform transform = ui.transform.Find("Panel_Rewards");
			List<Image> list2 = new List<Image>();
			for (int j = 0; j < 5; j++)
			{
				list2.Add(transform.transform.Find("Image (" + j.ToString() + ")").GetComponent<Image>());
			}
			Image component5 = ui.transform.Find("Panel_Time").GetComponent<Image>();
			Text component6 = component5.transform.Find("Panel").Find("Text_").GetComponent<Text>();
			Image component7 = ui.transform.Find("Panel_Done").GetComponent<Image>();
			Image component8 = ui.transform.Find("Panel_Finished").GetComponent<Image>();
			Image component9 = ui.transform.Find("Panel_CheckMail").GetComponent<Image>();
			component.text = taskData.title;
			if (taskData.procedural)
			{
				bool[] translating = new bool[]
				{
					true
				};
				component2.text = Language_Manager.instance.GetText(taskData.text, taskData.tags, taskData.values, translating);
				component3.enabled = true;
				component3.text = taskData.needsCurrent.ToString() + "/" + taskData.needsQnt.ToString();
			}
			else
			{
				component2.text = Language_Manager.instance.GetText(taskData.text);
				component3.text = "";
				component3.enabled = false;
			}
			component4.sprite = taskData.sprite;
			transform.gameObject.SetActive(false);
			List<GameObject> list3 = new List<GameObject>(taskData.rewards_Unlock);
			list3.TrimExcess();
			for (int k = 0; k < 5; k++)
			{
				if (k < list3.Count)
				{
					list2[k].gameObject.SetActive(true);
				}
				else
				{
					list2[k].gameObject.SetActive(false);
				}
			}
			component5.gameObject.SetActive(false);
			component6.text = "";
			if (taskData.state == 2)
			{
				component7.gameObject.SetActive(true);
				component8.gameObject.SetActive(false);
				component9.gameObject.SetActive(false);
			}
			else if (taskData.state == 3)
			{
				component7.gameObject.SetActive(false);
				component8.gameObject.SetActive(true);
				component9.gameObject.SetActive(true);
			}
			else
			{
				component7.gameObject.SetActive(false);
				component8.gameObject.SetActive(false);
				component9.gameObject.SetActive(false);
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.tasks_PanelParent.transform);
	}

	// Token: 0x040003DB RID: 987
	public static Menu_Manager instance;

	// Token: 0x040003DC RID: 988
	public int specific_player_index = -1;

	// Token: 0x040003DD RID: 989
	[SerializeField]
	private string menuName = "";

	// Token: 0x040003DE RID: 990
	[Header("General")]
	[SerializeField]
	private Animator selector;

	// Token: 0x040003DF RID: 991
	[SerializeField]
	private List<Animator> canvases;

	// Token: 0x040003E0 RID: 992
	[SerializeField]
	private List<Button> buttonSelectors;

	// Token: 0x040003E1 RID: 993
	[SerializeField]
	private Button buttonSelected;

	// Token: 0x040003E2 RID: 994
	private int menuIndex;

	// Token: 0x040003E3 RID: 995
	private string backMenu = "";

	// Token: 0x040003E4 RID: 996
	[Header("PC")]
	[SerializeField]
	private Camera pc_camera;

	// Token: 0x040003E5 RID: 997
	[SerializeField]
	private GameObject pc_turnoff_image;

	// Token: 0x040003E6 RID: 998
	[Header("Dark Mode Predefined Colors")]
	[SerializeField]
	public Color[] darkModePredefinedColors = new Color[2];

	// Token: 0x040003E7 RID: 999
	[Header("Start Screen")]
	[SerializeField]
	private Animator canvas_StartScreen;

	// Token: 0x040003E8 RID: 1000
	[SerializeField]
	private Button buttonSelector_StartScreen;

	// Token: 0x040003E9 RID: 1001
	[Header("HINTS")]
	[SerializeField]
	private GameObject hint_DropBox;

	// Token: 0x040003EA RID: 1002
	[SerializeField]
	private GameObject hint_Exit;

	// Token: 0x040003EB RID: 1003
	[Header("General")]
	[Header("Alpha Warnings")]
	[SerializeField]
	private GameObject alphaWarning;

	// Token: 0x040003EC RID: 1004
	[Header("HUD")]
	[SerializeField]
	private List<Text> text_Money;

	// Token: 0x040003ED RID: 1005
	[SerializeField]
	private Image[] image_TimeLeft;

	// Token: 0x040003EE RID: 1006
	[SerializeField]
	private Image panel_OpenState;

	// Token: 0x040003EF RID: 1007
	[SerializeField]
	private Image panel_WaitCustomers;

	// Token: 0x040003F0 RID: 1008
	[SerializeField]
	private Image panel_ClosedState;

	// Token: 0x040003F1 RID: 1009
	[SerializeField]
	private Text text_OpenState;

	// Token: 0x040003F2 RID: 1010
	[SerializeField]
	private Image panel_delivery;

	// Token: 0x040003F3 RID: 1011
	[SerializeField]
	private Image image_delivery_state;

	// Token: 0x040003F4 RID: 1012
	[SerializeField]
	private Sprite[] delivery_state_sprites = new Sprite[2];

	// Token: 0x040003F5 RID: 1013
	private string[] string_OpenState = new string[]
	{
		"open",
		"closed",
		"managing"
	};

	// Token: 0x040003F6 RID: 1014
	[Header("Warning")]
	[SerializeField]
	private Text text_WarningTitle;

	// Token: 0x040003F7 RID: 1015
	[SerializeField]
	private Text text_Warning;

	// Token: 0x040003F8 RID: 1016
	private List<string> warningList = new List<string>();

	// Token: 0x040003F9 RID: 1017
	private List<string> warningTitleList = new List<string>();

	// Token: 0x040003FA RID: 1018
	[SerializeField]
	private Button[] button_WarningConfirmation;

	// Token: 0x040003FB RID: 1019
	[Header("Notification")]
	[SerializeField]
	private Notification_Controller notification_Default;

	// Token: 0x040003FC RID: 1020
	[SerializeField]
	private List<Notification_Controller> notification_List = new List<Notification_Controller>();

	// Token: 0x040003FD RID: 1021
	[SerializeField]
	private GameObject notification_Saving;

	// Token: 0x040003FE RID: 1022
	private float defaultNotificationTimer = 3f;

	// Token: 0x040003FF RID: 1023
	[Header("Calendar")]
	[SerializeField]
	private List<GameObject> calendar_MainMenu_WeekDays = new List<GameObject>();

	// Token: 0x04000400 RID: 1024
	[SerializeField]
	private GameObject calendar_MainMenu_Arrow;

	// Token: 0x04000401 RID: 1025
	[Header("Save and Load")]
	[SerializeField]
	private List<GameObject> button_SaveLoad_GOs = new List<GameObject>();

	// Token: 0x04000402 RID: 1026
	private List<GameObject> button_SaveLoad_PanelInfo = new List<GameObject>();

	// Token: 0x04000403 RID: 1027
	private List<GameObject> button_SaveLoad_PanelAdd = new List<GameObject>();

	// Token: 0x04000404 RID: 1028
	private List<Text> button_SaveLoad_TextMartName = new List<Text>();

	// Token: 0x04000405 RID: 1029
	private List<Text> button_SaveLoad_TextMoney = new List<Text>();

	// Token: 0x04000406 RID: 1030
	private List<Text> button_SaveLoad_TextTimePlayed = new List<Text>();

	// Token: 0x04000407 RID: 1031
	private List<Text> button_SaveLoad_TextEmployees = new List<Text>();

	// Token: 0x04000408 RID: 1032
	private List<Text> button_SaveLoad_TextCustomers = new List<Text>();

	// Token: 0x04000409 RID: 1033
	private List<Text> button_SaveLoad_TextAwards = new List<Text>();

	// Token: 0x0400040A RID: 1034
	[Header("Receipt Day Ending")]
	[SerializeField]
	private Text text_Receipt_MartName;

	// Token: 0x0400040B RID: 1035
	[SerializeField]
	private Text text_Receipt_Season;

	// Token: 0x0400040C RID: 1036
	[SerializeField]
	private Text text_Receipt_Day;

	// Token: 0x0400040D RID: 1037
	[SerializeField]
	private Text text_Receipt_Week;

	// Token: 0x0400040E RID: 1038
	[SerializeField]
	private Text text_Receipt_ProdBought;

	// Token: 0x0400040F RID: 1039
	[SerializeField]
	private Text text_Receipt_Furniture;

	// Token: 0x04000410 RID: 1040
	[SerializeField]
	private Text text_Receipt_Staff;

	// Token: 0x04000411 RID: 1041
	[SerializeField]
	private Text text_Receipt_Expansion;

	// Token: 0x04000412 RID: 1042
	[SerializeField]
	private Text text_Receipt_Marketing;

	// Token: 0x04000413 RID: 1043
	[SerializeField]
	private Text text_Receipt_Operational;

	// Token: 0x04000414 RID: 1044
	[SerializeField]
	private Text text_Receipt_ProdSold;

	// Token: 0x04000415 RID: 1045
	[SerializeField]
	private Text text_Receipt_CoinsBalance;

	// Token: 0x04000416 RID: 1046
	[Header("Locker")]
	[SerializeField]
	private Color[] locker_ButtonColors = new Color[3];

	// Token: 0x04000417 RID: 1047
	[SerializeField]
	public List<Sprite> locker_SkinColor_Sprites = new List<Sprite>();

	// Token: 0x04000418 RID: 1048
	[SerializeField]
	public List<Sprite> locker_Outfit_Sprites = new List<Sprite>();

	// Token: 0x04000419 RID: 1049
	[SerializeField]
	public List<Sprite> locker_HairColor_Sprites = new List<Sprite>();

	// Token: 0x0400041A RID: 1050
	[SerializeField]
	public List<Sprite> locker_Hair_Sprites = new List<Sprite>();

	// Token: 0x0400041B RID: 1051
	[SerializeField]
	public List<Sprite> locker_Eyes_Sprites = new List<Sprite>();

	// Token: 0x0400041C RID: 1052
	[SerializeField]
	public List<Sprite> locker_Hats_Sprites = new List<Sprite>();

	// Token: 0x0400041D RID: 1053
	[SerializeField]
	private GameObject locker_ButtonPrefab;

	// Token: 0x0400041E RID: 1054
	[SerializeField]
	private ScrollRectAutoScroll locker_scroll_rect_auto_scroll;

	// Token: 0x0400041F RID: 1055
	private List<Image> locker_Buttons = new List<Image>();

	// Token: 0x04000420 RID: 1056
	private List<Button> locker_Buttons_BUTTONS = new List<Button>();

	// Token: 0x04000421 RID: 1057
	private List<Image> locker_ButtonIcons = new List<Image>();

	// Token: 0x04000422 RID: 1058
	[SerializeField]
	private List<Image> locker_Tabs = new List<Image>();

	// Token: 0x04000423 RID: 1059
	[SerializeField]
	private Text locker_TabText;

	// Token: 0x04000424 RID: 1060
	private List<string> locker_TabNames = new List<string>
	{
		"Skin Color",
		"Outfit",
		"Hair Color",
		"Hair Style",
		"Eyes",
		"Hats"
	};

	// Token: 0x04000425 RID: 1061
	private int locker_Width = 4;

	// Token: 0x04000426 RID: 1062
	private int locker_Tab_Index;

	// Token: 0x04000427 RID: 1063
	public int locker_player_index;

	// Token: 0x04000428 RID: 1064
	[Header("Choose Item")]
	[SerializeField]
	private GameObject button_ChooseItem_Prefab;

	// Token: 0x04000429 RID: 1065
	private List<GameObject> chooseItem_Buttons = new List<GameObject>();

	// Token: 0x0400042A RID: 1066
	private List<Text> chooseItem_TextNames = new List<Text>();

	// Token: 0x0400042B RID: 1067
	private List<Image> chooseItem_ItemImages = new List<Image>();

	// Token: 0x0400042C RID: 1068
	[Header("Loading Calendar")]
	[SerializeField]
	private GameObject panelParent_LoadingCalendar;

	// Token: 0x0400042D RID: 1069
	private List<Class_LoadingCalendar> class_LoadingCalendars = new List<Class_LoadingCalendar>();

	// Token: 0x0400042E RID: 1070
	[Header("Friendship")]
	[SerializeField]
	private Image friendship_Image;

	// Token: 0x0400042F RID: 1071
	[Header("Discounts")]
	[SerializeField]
	private List<GameObject> discounts_remove_gos;

	// Token: 0x04000430 RID: 1072
	[Header("Pause Menu")]
	[SerializeField]
	private GameObject pauseMenu_SaveButton_GO;

	// Token: 0x04000431 RID: 1073
	[SerializeField]
	private GameObject pauseMenu_MultiplayerButton_On_GO;

	// Token: 0x04000432 RID: 1074
	[SerializeField]
	private GameObject pauseMenu_MultiplayerButton_Off_GO;

	// Token: 0x04000433 RID: 1075
	[SerializeField]
	private GameObject pauseMenu_EE_SoccerMiniGame_Start;

	// Token: 0x04000434 RID: 1076
	[SerializeField]
	private GameObject pauseMenu_EE_MiniGame_Stop;

	// Token: 0x04000435 RID: 1077
	[Header("Multiplater Menu")]
	[SerializeField]
	private Text multiplayer_timer_Text;

	// Token: 0x04000436 RID: 1078
	private float multiplayer_timer_start = 5f;

	// Token: 0x04000437 RID: 1079
	private float multiplayer_timer;

	// Token: 0x04000438 RID: 1080
	[SerializeField]
	private RenderTexture multiplayer_player_texture;

	// Token: 0x04000439 RID: 1081
	private List<Menu_Manager.Class_Multiplayer_Panel> class_list_multiplayer_panel = new List<Menu_Manager.Class_Multiplayer_Panel>();

	// Token: 0x0400043A RID: 1082
	[SerializeField]
	private GameObject multiplayer_main_panel_ref;

	// Token: 0x0400043B RID: 1083
	[Header("Tasks Menu")]
	[SerializeField]
	private Color[] tasks_ButtonColor = new Color[2];

	// Token: 0x0400043C RID: 1084
	[SerializeField]
	private Image tasks_ButtonOpener;

	// Token: 0x0400043D RID: 1085
	[SerializeField]
	private GameObject tasks_PanelParent;

	// Token: 0x0400043E RID: 1086
	[SerializeField]
	private GameObject tasks_PanelPrefab;

	// Token: 0x0400043F RID: 1087
	[SerializeField]
	private List<GameObject> tasks_Panels = new List<GameObject>();

	// Token: 0x04000440 RID: 1088
	private bool task_State = true;

	// Token: 0x02000080 RID: 128
	private class Class_Multiplayer_Panel
	{
		// Token: 0x060005B4 RID: 1460 RVA: 0x0003476C File Offset: 0x0003296C
		public void CreateRefs(GameObject _ref, int _index)
		{
			this.panel = UnityEngine.Object.Instantiate<GameObject>(_ref);
			this.panel.SetActive(true);
			this.panel.transform.SetParent(_ref.transform.parent);
			this.panel_main = this.panel.transform.Find("Panel_Main").gameObject;
			this.raw_image = this.panel_main.transform.Find("RawImage_Player").GetComponent<RawImage>();
			this.text_player_name = this.panel_main.transform.Find("Panel").Find("Text").GetComponent<Text>();
			this.button_remove_player = this.panel_main.transform.Find("Panel").Find("Button_Remove").GetComponent<Button>();
			this.button_remove_player.onClick.AddListener(delegate()
			{
				Input_Manager.instance.Delete_InputControllers(_index);
			});
			this.panel_add_player = this.panel.transform.Find("Panel_Add").gameObject;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0003488C File Offset: 0x00032A8C
		public void Set_Player(Player_Controller _player, RenderTexture _render_texture = null)
		{
			if (_player == null)
			{
				this.panel_main.SetActive(false);
				this.panel_add_player.SetActive(true);
				return;
			}
			this.panel_main.SetActive(true);
			this.panel_add_player.SetActive(false);
			if (_player.playerIndex == 0)
			{
				this.button_remove_player.gameObject.SetActive(false);
			}
			this.text_player_name.text = Language_Manager.instance.GetText("Player") + " " + (_player.playerIndex + 1).ToString();
			RenderTexture renderTexture = new RenderTexture(_render_texture);
			renderTexture.Create();
			_player.camera_player_char.targetTexture = renderTexture;
			this.raw_image.texture = renderTexture;
		}

		// Token: 0x040006C4 RID: 1732
		public GameObject panel;

		// Token: 0x040006C5 RID: 1733
		public GameObject panel_main;

		// Token: 0x040006C6 RID: 1734
		public RawImage raw_image;

		// Token: 0x040006C7 RID: 1735
		public Text text_player_name;

		// Token: 0x040006C8 RID: 1736
		public Button button_remove_player;

		// Token: 0x040006C9 RID: 1737
		public GameObject panel_add_player;
	}
}
