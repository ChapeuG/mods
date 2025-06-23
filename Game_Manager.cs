using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x0200003F RID: 63
public class Game_Manager : MonoBehaviour
{
	// Token: 0x06000287 RID: 647 RVA: 0x00015243 File Offset: 0x00013443
	private void Awake()
	{
		if (!Game_Manager.instance)
		{
			Game_Manager.instance = this;
		}
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00015257 File Offset: 0x00013457
	private void Start()
	{
		this.pp_Game_Volume.TryGet<Vignette>(out this.pp_Game_Vignette);
		this.SetMartName("Super Mini Mart");
	}

	// Token: 0x06000289 RID: 649 RVA: 0x00015276 File Offset: 0x00013476
	private void Update()
	{
		this.UpdateGame();
		this.Update_Cooldown();
		this.PP_Update();
	}

	// Token: 0x0600028A RID: 650 RVA: 0x0001528C File Offset: 0x0001348C
	private void PP_Update()
	{
		float num = this.pp_Game_Vignette_IntensityNormal;
		if (Menu_Manager.instance.GetMenuName() == "Locker")
		{
			num = this.pp_Game_Vignette_IntensityLocker;
		}
		if (this.pp_Game_Vignette.intensity.value != num)
		{
			ClampedFloatParameter intensity = this.pp_Game_Vignette.intensity;
			intensity.value = Mathf.Lerp(intensity.value, num, this.pp_Game_Vignette_Speed * Time.unscaledDeltaTime);
		}
	}

	// Token: 0x0600028B RID: 651 RVA: 0x000152F8 File Offset: 0x000134F8
	private void UpdateGame()
	{
		if (this.cooldown_Timer > 0f)
		{
			return;
		}
		if (this.gameStage == 0)
		{
			Menu_Manager.instance.RefreshTime(0f);
			Unlock_Manager.instance.CreateReferences();
			Save_Manager.instance.LoadSettings();
			this.SetGameStage(1);
			return;
		}
		if (this.gameStage == 1)
		{
			Menu_Manager.instance.SetMenuName("Start");
			Input_Manager.instance.RefreshInputHints();
			Shader.SetGlobalInt("_personMode", 0);
			this.SetModeStage(-1);
			this.SetGameStage(9);
			Settings_Manager.instance.SetNextMasterVolume(1f);
			return;
		}
		if (this.gameStage != 9)
		{
			if (this.gameStage == 50)
			{
				Menu_Manager.instance.SetMenuName("Loading");
				this.SetCooldown(2f);
				this.SetGameStage(51);
				Settings_Manager.instance.SetNextMasterVolume(0f);
				return;
			}
			if (this.gameStage == 51)
			{
				Finances_Manager.instance.SetMoney(500f);
				World_Manager.instance.CreateLevel(0);
				World_Manager.instance.SetNewForecast();
				Inv_Manager.instance.SetSellPrices();
				Finances_Manager.instance.SetNewList();
				Player_Manager.instance.GetPlayerController(0).ResetPos();
				Player_Manager.instance.GetPlayerController(0).transform.localPosition = Vector3.zero;
				Player_Manager.instance.GetPlayerController(0).transform.localRotation = Quaternion.Euler(Vector3.zero);
				Player_Manager.instance.GetPlayerController(0).skin_.SetCompleteCustomization(0, 0, 0, 0, 0, 0, 1);
				this.SetGameStage(52);
				return;
			}
			if (this.gameStage == 52)
			{
				Inv_Manager.instance.RandomnlyCreateItems();
				Char_Manager.instance.ResetAllQueue();
				Char_Manager.instance.AddStoryCharToQueue(4);
				this.firsDay = true;
				Char_Manager.instance.SetSpawnFromQueue();
				Missions_Manager.instance.mayCreateTask = true;
				this.SetCinematicMode(false);
				this.SetGameStage(53);
				return;
			}
			if (this.gameStage == 53)
			{
				World_Manager.instance.SetExpansionIndex(0, 0);
				this.SetMartOpen(false);
				this.SetGameStage(98);
				return;
			}
			if (this.gameStage == 60)
			{
				Menu_Manager.instance.SetMenuName("Loading");
				this.SetCooldown(2f);
				this.SetGameStage(61);
				Settings_Manager.instance.SetNextMasterVolume(0f);
				return;
			}
			if (this.gameStage == 61)
			{
				Inv_Manager.instance.ResetInventory(true);
				SaveData loadedData = Save_Manager.instance.loadedData;
				Finances_Manager.instance.SetMoney(loadedData.money_SD);
				World_Manager.instance.CreateLevel(loadedData.currentLevelIndex_SD);
				World_Manager.instance.SetTime((float)loadedData.worldTime_SD[0]);
				World_Manager.instance.LoadDay(loadedData);
				World_Manager.instance.LoadWeek(loadedData);
				World_Manager.instance.LoadSeason(loadedData);
				World_Manager.instance.SetRawPlayTime(loadedData.playTime_SD);
				Score_Manager.instance.Load_TaigaScore(loadedData.taigaScore);
				Score_Manager.instance.Load_TaigaAwards_AwardsWonQnt(loadedData.taigaAwards_AwardsWonQnt);
				Unlock_Manager.instance.Load_ItemsUnlockState(loadedData);
				Missions_Manager.instance.Load_Tasks(loadedData);
				EasterEgg_Manager.instance.Load_SaveData(loadedData);
				this.SetMartOpen(loadedData.martOpen_SD);
				this.SetGameStage(62);
				return;
			}
			if (this.gameStage == 62)
			{
				SaveData loadedData2 = Save_Manager.instance.loadedData;
				World_Manager.instance.SetExpansionIndex(loadedData2.currentExpansionIndex_SD, loadedData2.currentExpansionRemainingDays_SD);
				this.SetGameStage(63);
				return;
			}
			if (this.gameStage == 63)
			{
				SaveData loadedData3 = Save_Manager.instance.loadedData;
				Player_Manager.instance.GetPlayerController(0).SetNewPos(new Vector3(loadedData3.playerPosX_SD, loadedData3.playerPosY_SD, loadedData3.playerPosZ_SD));
				Player_Manager.instance.GetPlayerController(0).transform.rotation = Quaternion.Euler(Vector3.zero);
				Player_Manager.instance.GetPlayerController(0).WakeUp();
				int playerSkinMatIndex_SD = loadedData3.playerSkinMatIndex_SD;
				int playerSkinMatIndex_SD2 = loadedData3.playerSkinMatIndex_SD;
				int playerEyesMatIndex_SD = loadedData3.playerEyesMatIndex_SD;
				int playerEyesMatIndex_SD2 = loadedData3.playerEyesMatIndex_SD;
				int playerClothesMatIndex_SD = loadedData3.playerClothesMatIndex_SD;
				int playerClothesMatIndex_SD2 = loadedData3.playerClothesMatIndex_SD;
				int playerHairMatIndex_SD = loadedData3.playerHairMatIndex_SD;
				int playerHairMatIndex_SD2 = loadedData3.playerHairMatIndex_SD;
				int playerHairMeshIndex_SD = loadedData3.playerHairMeshIndex_SD;
				int playerHairMeshIndex_SD2 = loadedData3.playerHairMeshIndex_SD;
				int playerHatMatIndex_SD = loadedData3.playerHatMatIndex_SD;
				int playerHatMatIndex_SD2 = loadedData3.playerHatMatIndex_SD;
				int playerHatGoIndex_SD = loadedData3.playerHatGoIndex_SD;
				int playerHatGoIndex_SD2 = loadedData3.playerHatGoIndex_SD;
				Player_Manager.instance.GetPlayerController(0).skin_.SetCompleteCustomization(playerSkinMatIndex_SD2, playerClothesMatIndex_SD2, playerHairMatIndex_SD2, playerHairMeshIndex_SD2, playerHatMatIndex_SD2, playerEyesMatIndex_SD2, playerHatGoIndex_SD2);
				Camera_Controller.instance.transform.position = Player_Manager.instance.GetPlayerController(0).transform.position;
				Inv_Manager.instance.SetSellPrices();
				Inv_Manager.instance.LoadShelves(loadedData3);
				Inv_Manager.instance.LoadDecor(loadedData3);
				Inv_Manager.instance.LoadWalls(loadedData3);
				Inv_Manager.instance.LoadFloors(loadedData3);
				Inv_Manager.instance.LoadUtil(loadedData3);
				Inv_Manager.instance.LoadNewspaperDeals(loadedData3);
				Inv_Manager.instance.DeleteAllDirtControllers();
				Finances_Manager.instance.LoadList(loadedData3);
				Mail_Manager.instance.Load_Data(loadedData3);
				this.SetGameStage(64);
				return;
			}
			if (this.gameStage == 64)
			{
				SaveData loadedData4 = Save_Manager.instance.loadedData;
				Inv_Manager.instance.LoadProds(loadedData4);
				Char_Manager.instance.LoadCustomers(loadedData4);
				Char_Manager.instance.LoadStaff(loadedData4);
				Char_Manager.instance.LoadCustomerLifeAchievement(loadedData4);
				Char_Manager.instance.LoadCustomerProdWantedNow(loadedData4);
				Char_Manager.instance.Load_LocalCustomer_Datas(loadedData4);
				Inv_Manager.instance.Load_DeliveryProds(loadedData4);
				World_Manager.instance.SetNewSeason(loadedData4.currentSeasonIndex_SD);
				this.firsDay = false;
				this.SetCooldown(1f);
				this.SetGameStage(98);
				return;
			}
			if (this.gameStage == 70)
			{
				Menu_Manager.instance.SetMenuName("LoadingDay");
				this.SetCooldown(2f);
				Camera_Controller.instance.mainCamera.SetParent(null);
				if (this.gameMode == 0)
				{
					Inv_Manager.instance.DeliverProds();
					this.SetGameStage(98);
					return;
				}
				this.SetModeStage(0);
				this.SetGameStage(71);
				return;
			}
			else
			{
				if (this.gameStage == 71)
				{
					World_Manager.instance.CreateOtherModesLevels(this.gameMode);
					this.SetCooldown(2f);
					this.SetGameStage(79);
					return;
				}
				if (this.gameStage == 79)
				{
					if (this.modeStage == 10)
					{
						this.SetGameStage(99);
						return;
					}
				}
				else
				{
					if (this.gameStage == 90)
					{
						Menu_Manager.instance.SetMenuName("LoadingDay");
						this.SetCooldown(2f);
						this.SetGameStage(91);
						return;
					}
					if (this.gameStage == 91)
					{
						Menu_Manager.instance.SetMenuName("LoadingCalendar");
						this.SetCooldown(4f);
						if (this.moveToLevelIndex != -1)
						{
							this.SetGameStage(92);
							return;
						}
						this.SetGameStage(94);
						return;
					}
					else
					{
						if (this.gameStage == 92)
						{
							Inv_Manager.instance.StoreBoxesToMoveOut();
							Inv_Manager.instance.ResetInventory(true);
							World_Manager.instance.CreateLevel(this.moveToLevelIndex);
							this.SetCooldown(1f);
							this.SetGameStage(93);
							return;
						}
						if (this.gameStage == 93)
						{
							Inv_Manager.instance.ResetInventory(false);
							this.SetGameStage(94);
							return;
						}
						if (this.gameStage == 94)
						{
							this.SetCinematicMode(false);
							Player_Manager.instance.GetPlayerController(0).SetCashier(null);
							Char_Manager.instance.SetSpawnFromQueue();
							Char_Manager.instance.DestroyAllCustomers();
							Player_Manager.instance.GetPlayerController(0).ResetPos();
							Camera_Controller.instance.ResetPosToPlayer();
							Inv_Manager.instance.RefreshIce(true);
							World_Manager.instance.SetNextDay();
							World_Manager.instance.SetNewForecast();
							Finances_Manager.instance.SetNextList();
							Char_Manager.instance.DestroyAllStaffChars();
							World_Manager.instance.currentLevel.CreateReferences();
							Inv_Manager.instance.Reset_CeilingLostBoxes();
							Inv_Manager.instance.DeliverProds();
							Inv_Manager.instance.RefreshIce(false);
							Inv_Manager.instance.Check_IfPlayerNeedHelpBoxes();
							this.waitingCustomersToLeave = false;
							Missions_Manager.instance.mayCreateTask = true;
							Save_Manager.instance.SaveGame();
							this.SetGameStage(95);
							return;
						}
						if (this.gameStage == 95)
						{
							Inv_Manager.instance.CreateDirt(0, 10f);
							this.SetGameStage(98);
							return;
						}
						if (this.gameStage == 98)
						{
							this.moveToLevelIndex = -1;
							World_Manager.instance.currentLevel.CreateReferences();
							World_Manager.instance.DeleteOtherModesLevels();
							Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
							Menu_Manager.instance.RefreshCalendar();
							World_Manager.instance.RefreshSeason();
							Input_Manager.instance.ResetScheme();
							Inv_Manager.instance.RefreshProdIndexesUnlocked();
							this.SetModeStage(-1);
							this.SetGameStage(99);
							if (Camera_Controller.instance.mainCamera.parent == null)
							{
								Camera_Controller.instance.mainCamera.transform.position = Vector3.zero;
								this.SetCooldown(2f);
								return;
							}
						}
						else if (this.gameStage == 99)
						{
							if (World_Manager.instance.activateTaigaAwards)
							{
								World_Manager.instance.activateTaigaAwards = false;
								this.SetGameMode(2);
								return;
							}
							PC_Manager.instance.Refresh_PC_Controller();
							Settings_Manager.instance.SetNextMasterVolume(1f);
							Nav_Manager.instance.DeactivateNavSpheresByInteractbles();
							Menu_Manager.instance.SetMenuName("MainMenu");
							this.SetGameStage(100);
							return;
						}
						else if (this.gameStage == 100)
						{
							if (this.GetGameMode() == 0 && this.waitingCustomersToLeave && Char_Manager.instance.customer_Controllers.Count <= 0)
							{
								Finances_Manager.instance.AddMoney((float)(-(float)World_Manager.instance.currentLevel.operational_cost));
								Finances_Manager.instance.AddTo_OutOperational((float)World_Manager.instance.currentLevel.operational_cost);
								Menu_Manager.instance.SetMenuName("DayEnding");
								this.waitingCustomersToLeave = false;
								return;
							}
						}
						else
						{
							if (this.gameStage == 990)
							{
								Menu_Manager.instance.SetMenuName("LoadingDay");
								this.SetCooldown(2f);
								this.SetGameStage(991);
								return;
							}
							if (this.gameStage == 991)
							{
								Input_Manager.instance.UpdateScheme(true);
								this.SetGameStage(999);
								return;
							}
							if (this.gameStage == 40)
							{
								Menu_Manager.instance.SetMenuName("Loading");
								this.SetCooldown(2f);
								this.SetGameStage(41);
								return;
							}
							if (this.gameStage == 41)
							{
								Application.LoadLevelAsync("MainGame");
								this.SetGameStage(49);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x0600028C RID: 652 RVA: 0x00015D48 File Offset: 0x00013F48
	public void SetGameStage(int _index)
	{
		this.gameStage = _index;
	}

	// Token: 0x0600028D RID: 653 RVA: 0x00015D51 File Offset: 0x00013F51
	public int GetGameStage()
	{
		return this.gameStage;
	}

	// Token: 0x0600028E RID: 654 RVA: 0x00015D59 File Offset: 0x00013F59
	public void NewGame()
	{
		this.SetGameStage(50);
	}

	// Token: 0x0600028F RID: 655 RVA: 0x00015D63 File Offset: 0x00013F63
	public void LoadGame()
	{
		this.SetGameStage(60);
	}

	// Token: 0x06000290 RID: 656 RVA: 0x00015D6D File Offset: 0x00013F6D
	public void PauseGame()
	{
		if (this.paused)
		{
			this.PauseGame(false);
			return;
		}
		this.PauseGame(true);
	}

	// Token: 0x06000291 RID: 657 RVA: 0x00015D88 File Offset: 0x00013F88
	public void PauseGame(bool _b)
	{
		if (this.GetGameMode() != 0)
		{
			return;
		}
		if (!_b)
		{
			if (Cheat_Manager.instance.GetFreezeTimeScale() == 0f)
			{
				Time.timeScale = 1f * Cheat_Manager.instance.GetUltraTimeScaleValue();
			}
			this.paused = false;
			return;
		}
		Time.timeScale = 0f;
		this.paused = true;
	}

	// Token: 0x06000292 RID: 658 RVA: 0x00015DE0 File Offset: 0x00013FE0
	public bool Get_PausedGame()
	{
		return this.paused;
	}

	// Token: 0x06000293 RID: 659 RVA: 0x00015DE8 File Offset: 0x00013FE8
	public void SetCinematicMode(bool _b)
	{
		this.cinematicMode = _b;
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00015DF1 File Offset: 0x00013FF1
	public bool GetCinematicMode()
	{
		return this.cinematicMode;
	}

	// Token: 0x06000295 RID: 661 RVA: 0x00015DF9 File Offset: 0x00013FF9
	public bool MayRun()
	{
		return this.gameStage >= 100 && this.gameStage <= 199;
	}

	// Token: 0x06000296 RID: 662 RVA: 0x00015E18 File Offset: 0x00014018
	public void ExitToMainMenu()
	{
		if (Save_Manager.instance.is_saving)
		{
			return;
		}
		string text = Language_Manager.instance.GetText("_warning_exit_mainmenu_00");
		Menu_Manager.instance.SetWarningConfirmation(text, -1, "").onClick.AddListener(new UnityAction(this.ExitToMainMenu2));
	}

	// Token: 0x06000297 RID: 663 RVA: 0x00015E69 File Offset: 0x00014069
	private void ExitToMainMenu2()
	{
		this.SetGameStage(40);
	}

	// Token: 0x06000298 RID: 664 RVA: 0x00015E74 File Offset: 0x00014074
	public void ExitGame()
	{
		string text = Language_Manager.instance.GetText("_warning_quit_game_00");
		Menu_Manager.instance.SetWarningConfirmation(text, -1, "").onClick.AddListener(new UnityAction(Application.Quit));
	}

	// Token: 0x06000299 RID: 665 RVA: 0x00015EB8 File Offset: 0x000140B8
	public void BuyExpansionIndex(bool _free = false)
	{
		if (World_Manager.instance.currentExpansionIndex < World_Manager.instance.currentLevel.expansions_GOs.Count && World_Manager.instance.expansion_RemainingDays <= 0)
		{
			if (Finances_Manager.instance.CheckHasMoney((float)World_Manager.instance.Get_ExpansionPrice()) || _free)
			{
				string text = Language_Manager.instance.GetText("_warning_expansion_00", "[number]", World_Manager.instance.days_to_expand[World_Manager.instance.currentExpansionIndex].ToString());
				Menu_Manager.instance.SetWarningConfirmation(text, Menu_Manager.instance.specific_player_index, "PC").onClick.AddListener(delegate()
				{
					this.BuyExpansionIndex2(_free);
				});
				return;
			}
			string text2 = Language_Manager.instance.GetText("Attention!");
			string text3 = Language_Manager.instance.GetText("Insufficient Coins");
			Menu_Manager.instance.SetNotification(text2, text3, true);
		}
	}

	// Token: 0x0600029A RID: 666 RVA: 0x00015FBC File Offset: 0x000141BC
	public void BuyExpansionIndex2(bool _free = false)
	{
		World_Manager.instance.SetExpansionIndex(World_Manager.instance.currentExpansionIndex + 1, World_Manager.instance.days_to_expand[World_Manager.instance.currentExpansionIndex]);
		if (!_free)
		{
			int expansionPrice = World_Manager.instance.Get_ExpansionPrice();
			Finances_Manager.instance.AddMoney((float)(-(float)expansionPrice));
			Finances_Manager.instance.AddTo_OutExpansion((float)expansionPrice);
		}
	}

	// Token: 0x0600029B RID: 667 RVA: 0x0001601C File Offset: 0x0001421C
	public void BuyRelocation(int _index)
	{
		if (!Finances_Manager.instance.CheckHasMoney((float)World_Manager.instance.Get_LevelPrice_ByIndex(_index)))
		{
			string text = Language_Manager.instance.GetText("Attention!");
			string text2 = Language_Manager.instance.GetText("Insufficient Coins");
			Menu_Manager.instance.SetNotification(text, text2, true);
			return;
		}
		string text3 = Language_Manager.instance.GetText("_warning_location_00");
		string str = "\n\n" + Language_Manager.instance.GetText("_warning_location_01");
		if (Inv_Manager.instance.GetIfAllItemsInBoxex())
		{
			Menu_Manager.instance.SetWarningConfirmation(text3, Menu_Manager.instance.specific_player_index, "PC").onClick.AddListener(delegate()
			{
				this.BuyRelocation2(_index);
			});
			return;
		}
		Menu_Manager.instance.SetWarningConfirmation(text3 + str, Menu_Manager.instance.specific_player_index, "PC").gameObject.SetActive(false);
	}

	// Token: 0x0600029C RID: 668 RVA: 0x00016120 File Offset: 0x00014320
	public void BuyRelocation2(int _index)
	{
		int num = World_Manager.instance.Get_LevelPrice_ByIndex(_index);
		Finances_Manager.instance.AddMoney((float)(-(float)num));
		Finances_Manager.instance.AddTo_OutExpansion((float)num);
		World_Manager.instance.SetExpansionIndex(0, 0);
		this.moveToLevelIndex = _index;
		this.SetGameStage(90);
	}

	// Token: 0x0600029D RID: 669 RVA: 0x00016170 File Offset: 0x00014370
	public void SetHiringAd()
	{
		if (Finances_Manager.instance.CheckHasMoney(0f))
		{
			Menu_Manager.instance.SetWarningConfirmation("You will set a hiring ad?", -1, "").onClick.AddListener(new UnityAction(this.SetHiringAd2));
			return;
		}
		Menu_Manager.instance.SetNotification("Attention!", "Insufficient Coins", true);
	}

	// Token: 0x0600029E RID: 670 RVA: 0x000161CF File Offset: 0x000143CF
	public void SetHiringAd2()
	{
		this.isHiring = true;
	}

	// Token: 0x0600029F RID: 671 RVA: 0x000161D8 File Offset: 0x000143D8
	public bool GetIsHiring()
	{
		return this.isHiring;
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x000161E0 File Offset: 0x000143E0
	public void SetMartOpen(bool _b)
	{
		if (_b)
		{
			Save_Manager.instance.SaveGame();
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_StartGame);
		}
		if (this.GetCinematicMode())
		{
			return;
		}
		if (this.waitingCustomersToLeave)
		{
			return;
		}
		this.martOpen = _b;
		if (_b)
		{
			this.waitingCustomersToLeave = false;
			Char_Manager.instance.SpawnEmployees();
			Inv_Manager.instance.DeliverProds_Today();
		}
		Menu_Manager.instance.RefreshTime(0f);
		Menu_Manager.instance.RefreshSignStateOpen();
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0001625E File Offset: 0x0001445E
	public void SetMartOpenByPlayer()
	{
		if (this.martOpen)
		{
			this.CloseMart();
			return;
		}
		this.SetMartOpen(true);
		Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.open_Store, -1);
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x00016282 File Offset: 0x00014482
	public bool GetMartOpen()
	{
		return this.martOpen;
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x0001628A File Offset: 0x0001448A
	public bool GetWaitingCustomersToLeave()
	{
		return this.waitingCustomersToLeave;
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x00016292 File Offset: 0x00014492
	public void CloseMart()
	{
		this.martOpen = false;
		this.waitingCustomersToLeave = true;
		Menu_Manager.instance.RefreshTime(0f);
		Menu_Manager.instance.RefreshSignStateOpen();
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x000162BB File Offset: 0x000144BB
	public void NextDay()
	{
		this.SetGameStage(90);
		this.waitingCustomersToLeave = false;
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x000162CC File Offset: 0x000144CC
	public void PrepareForNextDay()
	{
		if (Char_Manager.instance.customer_Controllers.Count == 0)
		{
			Finances_Manager.instance.AddMoney((float)(-(float)World_Manager.instance.currentLevel.operational_cost));
			Finances_Manager.instance.AddTo_OutOperational((float)World_Manager.instance.currentLevel.operational_cost);
			Menu_Manager.instance.SetMenuName("DayEnding");
			this.waitingCustomersToLeave = false;
		}
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x00016335 File Offset: 0x00014535
	public void LeaveMart()
	{
		this.SetGameStage(100);
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x0001633F File Offset: 0x0001453F
	public void SetMartName(string _NewName)
	{
		this.martName = _NewName;
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x00016348 File Offset: 0x00014548
	public string GetMartName()
	{
		return this.martName;
	}

	// Token: 0x060002AA RID: 682 RVA: 0x00016350 File Offset: 0x00014550
	public void SetGameMode(int _mode)
	{
		this.SetModeStage(-1);
		this.gameMode = _mode;
		this.SetGameStage(70);
	}

	// Token: 0x060002AB RID: 683 RVA: 0x00016368 File Offset: 0x00014568
	public void SetGameMode_DEBUG(int _mode)
	{
		if (_mode == 2)
		{
			Score_Manager.instance.IncreaseTaigaScore(100f);
		}
		this.SetGameMode(_mode);
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00016384 File Offset: 0x00014584
	public int GetGameMode()
	{
		return this.gameMode;
	}

	// Token: 0x060002AD RID: 685 RVA: 0x0001638C File Offset: 0x0001458C
	public void SetModeStage(int _index)
	{
		this.modeStage = _index;
	}

	// Token: 0x060002AE RID: 686 RVA: 0x00016395 File Offset: 0x00014595
	public int GetModeStage()
	{
		return this.modeStage;
	}

	// Token: 0x060002AF RID: 687 RVA: 0x0001639D File Offset: 0x0001459D
	private void Update_Cooldown()
	{
		if (this.cooldown_Timer > 0f)
		{
			this.cooldown_Timer -= Time.unscaledDeltaTime;
		}
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x000163BE File Offset: 0x000145BE
	public void SetCooldown(float _secondsToWait)
	{
		this.cooldown_Timer = _secondsToWait;
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x000163C7 File Offset: 0x000145C7
	public void OpenURL(string _url)
	{
		Application.OpenURL(_url);
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x000163CF File Offset: 0x000145CF
	public Tween_Controller Create_Tween()
	{
		GameObject gameObject = new GameObject();
		gameObject.AddComponent<Tween_Controller>();
		return gameObject.GetComponent<Tween_Controller>();
	}

	// Token: 0x04000349 RID: 841
	public static Game_Manager instance;

	// Token: 0x0400034A RID: 842
	[Header("General")]
	public bool permit_multiplayer;

	// Token: 0x0400034B RID: 843
	private bool martOpen;

	// Token: 0x0400034C RID: 844
	private bool waitingCustomersToLeave;

	// Token: 0x0400034D RID: 845
	private int modeStage;

	// Token: 0x0400034E RID: 846
	private int gameStage;

	// Token: 0x0400034F RID: 847
	private GameObject player;

	// Token: 0x04000350 RID: 848
	private bool paused;

	// Token: 0x04000351 RID: 849
	private bool cinematicMode;

	// Token: 0x04000352 RID: 850
	private float cooldown_Timer;

	// Token: 0x04000353 RID: 851
	private string martName;

	// Token: 0x04000354 RID: 852
	private int moveToLevelIndex = -1;

	// Token: 0x04000355 RID: 853
	private bool isHiring;

	// Token: 0x04000356 RID: 854
	public bool tutorialHappening;

	// Token: 0x04000357 RID: 855
	public bool firsDay = true;

	// Token: 0x04000358 RID: 856
	[Header("Game Modes")]
	private int gameMode;

	// Token: 0x04000359 RID: 857
	public VolumeProfile pp_Game_Volume;

	// Token: 0x0400035A RID: 858
	private Vignette pp_Game_Vignette;

	// Token: 0x0400035B RID: 859
	private float pp_Game_Vignette_Speed = 2f;

	// Token: 0x0400035C RID: 860
	private float pp_Game_Vignette_IntensityNormal;

	// Token: 0x0400035D RID: 861
	private float pp_Game_Vignette_IntensityLocker = 0.4f;
}
