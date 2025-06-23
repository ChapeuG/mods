using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000052 RID: 82
public class Save_Manager : MonoBehaviour
{
	// Token: 0x0600046F RID: 1135 RVA: 0x0002CAA8 File Offset: 0x0002ACA8
	private void Awake()
	{
		if (!Save_Manager.instance)
		{
			Save_Manager.instance = this;
		}
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0002CABC File Offset: 0x0002ACBC
	private void Start()
	{
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x0002CABE File Offset: 0x0002ACBE
	private void Update()
	{
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x0002CAC0 File Offset: 0x0002ACC0
	public void SaveSettings()
	{
		Menu_Manager.instance.SetNotificationSaving();
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Create(Application.persistentDataPath + "/SettingsData_.sav");
		binaryFormatter.Serialize(fileStream, new SettingsData
		{
			menuScale_Index = Settings_Manager.instance.menuScale_Index,
			mouseSensitivity_Value = Settings_Manager.instance.GetMouseSensitivity(1f),
			currentPixRes = 0,
			screenMode_Index = Settings_Manager.instance.screenMode_Index,
			currentGameResIndex = Settings_Manager.instance.currentGameResIndex,
			currentFrameLimitIndex = Settings_Manager.instance.currentFrameLimitIndex,
			lang_Selected = Language_Manager.instance.lang_Selected,
			currentAudioMusicIndex = Settings_Manager.instance.currentAudioMusicIndex,
			currentAudioSfxIndex = Settings_Manager.instance.currentAudioSfxIndex,
			currentGamepadSensitivity = Settings_Manager.instance.currentGamepadSensitivity
		});
		fileStream.Close();
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x0002CBA0 File Offset: 0x0002ADA0
	public void LoadSettings()
	{
		string text = Application.persistentDataPath + "/SettingsData_.dat";
		string text2 = Application.persistentDataPath + "/SettingsData_.sav";
		if (File.Exists(text))
		{
			File.Copy(text, text2, true);
			File.Delete(text);
		}
		if (File.Exists(text2))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = File.Open(text2, FileMode.Open);
			SettingsData settingsData = (SettingsData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
			Settings_Manager.instance.SetMenuScale(settingsData.menuScale_Index);
			Settings_Manager.instance.SetMouseSensitivityValue(settingsData.mouseSensitivity_Value);
			Settings_Manager.instance.SetPixelation(settingsData.currentPixRes);
			int screenMode_Index = settingsData.screenMode_Index;
			Settings_Manager.instance.Set_ScreenMode(settingsData.screenMode_Index);
			Settings_Manager.instance.SetGameRes(settingsData.currentGameResIndex, false);
			Settings_Manager.instance.SetFrameLimit(settingsData.currentFrameLimitIndex);
			Settings_Manager.instance.SetLanguage(settingsData.lang_Selected);
			Settings_Manager.instance.SetAudioMusic(settingsData.currentAudioMusicIndex);
			Settings_Manager.instance.SetAudioSfx(settingsData.currentAudioSfxIndex);
			Settings_Manager.instance.SetGamepadSensitivity(settingsData.currentGamepadSensitivity);
			return;
		}
		Settings_Manager.instance.SetMenuScale(1);
		Settings_Manager.instance.SetMouseSensitivityValue(5f);
		Settings_Manager.instance.SetPixelation(0);
		Settings_Manager.instance.Set_ScreenMode(0);
		Settings_Manager.instance.SetMaxGameRes();
		Settings_Manager.instance.SetFrameLimit(2);
		Settings_Manager.instance.SetLanguage(0);
		Settings_Manager.instance.SetAudioMusic(5);
		Settings_Manager.instance.SetAudioSfx(5);
		Settings_Manager.instance.SetGamepadSensitivity(4);
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x0002CD27 File Offset: 0x0002AF27
	public void Stop_Is_Saving()
	{
		this.is_saving = false;
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x0002CD30 File Offset: 0x0002AF30
	public void SaveGame()
	{
		this.is_saving = true;
		Menu_Manager.instance.SetNotificationSaving();
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		string str = "/MySaveData_" + this.gameIndex.ToString() + ".dat";
		if (File.Exists(Application.persistentDataPath + "/MySaveData_" + this.gameIndex.ToString() + ".dat"))
		{
			FileStream fileStream = File.Open(Application.persistentDataPath + "/MySaveData_" + this.gameIndex.ToString() + ".dat", FileMode.Open);
			if (fileStream.Length > 0L)
			{
				fileStream.Close();
				File.Copy(Application.persistentDataPath + "/MySaveData_" + this.gameIndex.ToString() + ".dat", Application.persistentDataPath + "/MySaveData_Backup" + this.gameIndex.ToString() + ".dat", true);
			}
			else
			{
				fileStream.Close();
				Menu_Manager.instance.SetNotification("Error!", "Unable to save file.", false);
				str = "/MySaveData_Forced" + this.gameIndex.ToString() + ".dat";
			}
		}
		FileStream fileStream2 = File.Create(Application.persistentDataPath + str);
		SaveData saveData = new SaveData();
		saveData.saveDate_SD = string.Concat(new string[]
		{
			DateTime.Now.ToString("yyyy-MM-dd"),
			"  ",
			DateTime.Now.Hour.ToString(),
			":",
			DateTime.Now.Minute.ToString()
		});
		saveData.playTime_SD = World_Manager.instance.GetRawPlayTime();
		saveData.martName_SD = Game_Manager.instance.GetMartName();
		saveData.awards_SD = Missions_Manager.instance.GetTotalAwardsWon();
		saveData.money_SD = (float)((int)Finances_Manager.instance.GetMoney());
		saveData.martOpen_SD = Game_Manager.instance.GetMartOpen();
		saveData.playerPosX_SD = Player_Manager.instance.GetPlayerController(0).transform.position.x;
		saveData.playerPosY_SD = Player_Manager.instance.GetPlayerController(0).transform.position.y;
		saveData.playerPosZ_SD = Player_Manager.instance.GetPlayerController(0).transform.position.z;
		saveData.playerSkinMatIndex_SD = Player_Manager.instance.GetPlayerController(0).skin_.skinMat_Index;
		saveData.playerEyesMatIndex_SD = Player_Manager.instance.GetPlayerController(0).skin_.eyeMat_Index;
		saveData.playerClothesMatIndex_SD = Player_Manager.instance.GetPlayerController(0).skin_.clothesMat_Index;
		saveData.playerHairMatIndex_SD = Player_Manager.instance.GetPlayerController(0).skin_.hairMat_Index;
		saveData.playerHairMeshIndex_SD = Player_Manager.instance.GetPlayerController(0).skin_.hairMesh_Index;
		saveData.playerHatMatIndex_SD = Player_Manager.instance.GetPlayerController(0).skin_.hatMat_Index;
		saveData.playerHatGoIndex_SD = Player_Manager.instance.GetPlayerController(0).skin_.hatGo_Index;
		int[] array = new int[2];
		array[0] = Mathf.RoundToInt(World_Manager.instance.GetTime());
		int[] worldTime_SD = array;
		saveData.worldTime_SD = worldTime_SD;
		saveData.prodSellPrice_SD = Inv_Manager.instance.prod_SellPrices;
		saveData.inv_Deals_Indexes = Inv_Manager.instance.news_Deals_ProdIndexes;
		saveData.inv_Deals_DaysLeft = Inv_Manager.instance.news_Deals_DaysLeft;
		saveData.currentLevelIndex_SD = World_Manager.instance.currentLevelIndex;
		saveData.currentExpansionIndex_SD = World_Manager.instance.currentExpansionIndex;
		saveData.currentExpansionRemainingDays_SD = World_Manager.instance.expansion_RemainingDays;
		saveData.currentSeasonIndex_SD = World_Manager.instance.GetSeasonIndex();
		List<Shelf_Controller> list = new List<Shelf_Controller>(Inv_Manager.instance.shelfProd_Controllers);
		list.TrimExcess();
		for (int i = 0; i < list.Count; i++)
		{
			Shelf_Controller shelf_Controller = list[i];
			int item = -1;
			Vector3 rValue = Vector3.zero;
			Vector3 rValue2 = Vector3.zero;
			int[] array2 = new int[]
			{
				-1,
				-1,
				-1
			};
			int[] array3 = new int[3];
			if (shelf_Controller != null)
			{
				item = shelf_Controller.shelfIndex;
				rValue = shelf_Controller.transform.position;
				rValue2 = shelf_Controller.transform.rotation.eulerAngles;
				for (int j = 0; j < shelf_Controller.height; j++)
				{
					if (shelf_Controller.prodControllers[j, 0])
					{
						array2[j] = shelf_Controller.prodControllers[j, 0].prodIndex;
						for (int k = 0; k < shelf_Controller.width; k++)
						{
							if (shelf_Controller.prodControllers[j, k])
							{
								array3[j]++;
							}
						}
					}
				}
			}
			saveData.shelfProdIndex_SD.Add(item);
			saveData.shelfProdPosition_SD.Add(rValue);
			saveData.shelfProdRotation_SD.Add(rValue2);
			saveData.shelfProdProductsIndex_SD.Add(new Vector3((float)array2[0], (float)array2[1], (float)array2[2]));
			saveData.shelfProdProductsQnt_SD.Add(new Vector3((float)array3[0], (float)array3[1], (float)array3[2]));
			bool isBroken = shelf_Controller.GetComponent<Interaction_Controller>().isBroken;
			saveData.shelfProdBrokenState_SD.Add(isBroken);
		}
		List<Shelf_Controller> list2 = new List<Shelf_Controller>(Inv_Manager.instance.shelfInv_Controllers);
		list2.TrimExcess();
		for (int l = 0; l < list2.Count; l++)
		{
			Shelf_Controller shelf_Controller2 = list2[l];
			Vector3 rValue3 = Vector3.one * -1f;
			Vector3 rValue4 = new Vector3(-1f, -1f, -1f);
			Vector3 one = Vector3.one;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			if (shelf_Controller2.boxControllers[0])
			{
				rValue3.x = (float)shelf_Controller2.boxControllers[0].GetBoxType();
				rValue4.x = (float)shelf_Controller2.boxControllers[0].itemIndex;
				one.x = (float)shelf_Controller2.boxControllers[0].prodQnt;
				zero.x = (float)shelf_Controller2.boxControllers[0].lifeSpanIndex;
				if (shelf_Controller2.boxControllers[0].frozen)
				{
					zero2.x = 1f;
				}
				else
				{
					zero2.x = 0f;
				}
			}
			if (shelf_Controller2.boxControllers[1])
			{
				rValue3.y = (float)shelf_Controller2.boxControllers[1].GetBoxType();
				rValue4.y = (float)shelf_Controller2.boxControllers[1].itemIndex;
				one.y = (float)shelf_Controller2.boxControllers[1].prodQnt;
				zero.y = (float)shelf_Controller2.boxControllers[1].lifeSpanIndex;
				if (shelf_Controller2.boxControllers[1].frozen)
				{
					zero2.y = 1f;
				}
				else
				{
					zero2.y = 0f;
				}
			}
			if (shelf_Controller2.boxControllers[2])
			{
				rValue3.z = (float)shelf_Controller2.boxControllers[2].GetBoxType();
				rValue4.z = (float)shelf_Controller2.boxControllers[2].itemIndex;
				one.z = (float)shelf_Controller2.boxControllers[2].prodQnt;
				zero.z = (float)shelf_Controller2.boxControllers[2].lifeSpanIndex;
				if (shelf_Controller2.boxControllers[2].frozen)
				{
					zero2.z = 1f;
				}
				else
				{
					zero2.z = 0f;
				}
			}
			saveData.shelfInvBoxType_SD.Add(rValue3);
			saveData.shelfInvBoxIndex_SD.Add(rValue4);
			saveData.shelfInvBoxQnt_SD.Add(one);
			saveData.shelfInvBoxLifeSpanIndex_SD.Add(zero);
			saveData.shelfInvBoxFrozen_SD.Add(zero2);
		}
		List<Decor_Controller> list3 = new List<Decor_Controller>(Inv_Manager.instance.decor_Controllers);
		for (int m = 0; m < list3.Count; m++)
		{
			Decor_Controller decor_Controller = list3[m];
			if (decor_Controller != null)
			{
				Vector3 rValue5 = Vector3.zero;
				Vector3 rValue6 = Vector3.zero;
				int decorIndex = decor_Controller.decorIndex;
				rValue5 = decor_Controller.transform.position;
				rValue6 = decor_Controller.transform.rotation.eulerAngles;
				saveData.decorIndex_SD.Add(decorIndex);
				saveData.decorPosition_SD.Add(rValue5);
				saveData.decorRotation_SD.Add(rValue6);
				int item2 = Inv_Manager.instance.plantHealth_StartValue;
				if (decor_Controller.GetComponent<Interaction_Controller>().isDecorPlant)
				{
					item2 = decor_Controller.GetComponent<Plant_Controller>().GetLifeSpanIndex();
				}
				saveData.decorLifeSpanIndex_SD.Add(item2);
			}
		}
		List<WallPaint_Controller> list4 = new List<WallPaint_Controller>(Inv_Manager.instance.wallPaint_Controllers);
		for (int n = 0; n < list4.Count; n++)
		{
			WallPaint_Controller wallPaint_Controller = list4[n];
			saveData.wallIndex_SD.Add(list4[n].itemIndex);
		}
		List<Floor_Controller> list5 = new List<Floor_Controller>(Inv_Manager.instance.floor_Controllers);
		for (int num = 0; num < list5.Count; num++)
		{
			Floor_Controller floor_Controller = list5[num];
			saveData.floorIndex_SD.Add(list5[num].itemIndex);
		}
		List<Util_Controller> list6 = new List<Util_Controller>(Inv_Manager.instance.util_Controllers);
		list6.TrimExcess();
		for (int num2 = 0; num2 < list6.Count; num2++)
		{
			Util_Controller util_Controller = list6[num2];
			if (util_Controller != null)
			{
				Vector3 rValue7 = Vector3.zero;
				Vector3 rValue8 = Vector3.zero;
				int utilIndex = util_Controller.utilIndex;
				rValue7 = util_Controller.transform.position;
				rValue8 = util_Controller.transform.rotation.eulerAngles;
				saveData.utilIndex_SD.Add(utilIndex);
				saveData.utilPosition_SD.Add(rValue7);
				saveData.utilRotation_SD.Add(rValue8);
				Vector3 rValue9 = Vector3.one * -1f;
				Vector3 rValue10 = new Vector3(-1f, -1f, -1f);
				Vector3 one2 = Vector3.one;
				Vector3 zero3 = Vector3.zero;
				if (util_Controller.shelfController && util_Controller.shelfController.boxControllers[0])
				{
					rValue9.x = (float)util_Controller.shelfController.boxControllers[0].GetBoxType();
					rValue10.x = (float)util_Controller.shelfController.boxControllers[0].itemIndex;
					one2.x = (float)util_Controller.shelfController.boxControllers[0].prodQnt;
					zero3.x = (float)util_Controller.shelfController.boxControllers[0].lifeSpanIndex;
				}
				saveData.utilBoxType_SD.Add(rValue9);
				saveData.utilBoxIndex_SD.Add(rValue10);
				saveData.utilBoxQnt_SD.Add(one2);
				saveData.utilLifeSpanIndex_SD.Add(zero3);
				bool isBroken2 = util_Controller.GetComponent<Interaction_Controller>().isBroken;
				saveData.utilBrokenState_SD.Add(isBroken2);
			}
		}
		List<Box_Controller> list7 = new List<Box_Controller>(Inv_Manager.instance.box_Controllers);
		list7.TrimExcess();
		for (int num3 = 0; num3 < list7.Count; num3++)
		{
			Box_Controller box_Controller = list7[num3];
			if (!box_Controller.isHeld || box_Controller.cart_controller != null || box_Controller.shelf_Controller_Holding == null)
			{
				saveData.boxIndex_SD.Add(box_Controller.itemIndex);
				saveData.boxType_SD.Add(box_Controller.GetBoxType());
				saveData.boxQnt_SD.Add(box_Controller.prodQnt);
				saveData.boxPosition_SD.Add(box_Controller.transform.position);
				saveData.boxRotation_SD.Add(box_Controller.transform.rotation.eulerAngles);
				saveData.boxLifeSpanIndex_SD.Add(box_Controller.lifeSpanIndex);
				saveData.boxFrozen_SD.Add(box_Controller.frozen);
			}
		}
		saveData.item_Unlocked_SD = Unlock_Manager.instance.item_Unlocked;
		saveData.item_NewlyUnlocked_SD = Unlock_Manager.instance.item_NewlyUnlocked;
		saveData.deliveryIndexes_SD = new List<int>(Inv_Manager.instance.prod_deliveryIndexes);
		saveData.deliveryCategories_SD = new List<int>(Inv_Manager.instance.prod_deliveryCategories);
		saveData.deliveryQnt_SD = new List<int>(Inv_Manager.instance.prod_deliveryQnt);
		saveData.deliverySupplierIndexes_SD = new List<int>(Inv_Manager.instance.prod_deliverySupplierIndexes);
		saveData.deliveryDaysIndexes_SD = new List<int>(Inv_Manager.instance.prod_deliveryDaysIndexes);
		saveData.deliveryLifeSpanIndexes_SD = new List<int>(Inv_Manager.instance.prod_deliveryLifeSpanIndexes);
		saveData.createCustomerTimer_SD = Char_Manager.instance.createCustomerTime_Timer;
		List<Char_Controller> list8 = new List<Char_Controller>(Char_Manager.instance.customer_Controllers);
		list8.TrimExcess();
		for (int num4 = 0; num4 < list8.Count; num4++)
		{
			saveData.customerIndex_SD.Add(list8[num4].GetID());
			saveData.customerPosition_SD.Add(list8[num4].transform.position);
			saveData.customerDoneShopping.Add(list8[num4].customer_Controller.doneShopping);
			saveData.customerDoneCashier.Add(list8[num4].customer_Controller.doneCashier);
			string text = "null";
			for (int num5 = 0; num5 < list8[num4].customer_Controller.prod_BuyList.Count; num5++)
			{
				if (list8[num4].customer_Controller.prod_BuyList[num5] >= 0)
				{
					if (num5 == 0)
					{
						text = list8[num4].customer_Controller.prod_BuyList[num5].ToString();
					}
					else
					{
						text = text + "_" + list8[num4].customer_Controller.prod_BuyList[num5].ToString();
					}
				}
			}
			saveData.customerProdList_SD.Add(text);
		}
		saveData.customerLifeAchievementsByIndex = new List<bool>(Char_Manager.instance.GetAllCustomerLifeAchievements());
		saveData.customerProdWantedNowByIndex = new List<int>(Char_Manager.instance.GetAllCustomerProdWantedNow());
		saveData.localCustomer_Datas = new List<LocalCustomer_Data>(Char_Manager.instance.localCustomer_Datas);
		saveData.taigaScore = Score_Manager.instance.taigaScore;
		saveData.taigaAwards_AwardsWonQnt = Score_Manager.instance.taigaAwards_AwardsWonQnt;
		saveData.finances_OutProd = new List<float>(Finances_Manager.instance.GetList_OutProds());
		saveData.finances_OutFurniture = new List<float>(Finances_Manager.instance.GetList_OutFurniture());
		saveData.finances_OutStaff = new List<float>(Finances_Manager.instance.GetList_OutStaff());
		saveData.finances_OutExpansion = new List<float>(Finances_Manager.instance.GetList_OutExpansion());
		saveData.finances_OutMarketing = new List<float>(Finances_Manager.instance.GetList_OutMarketing());
		saveData.finances_OutOperational = new List<float>(Finances_Manager.instance.GetList_OutOperational());
		saveData.finances_InSales = new List<float>(Finances_Manager.instance.GetList_InSales());
		saveData.finances_InPrizes = new List<float>(Finances_Manager.instance.GetList_InPrizes());
		saveData.world_DayOverall = World_Manager.instance.day_overall;
		saveData.world_DayIndex = World_Manager.instance.GetDayIndex();
		saveData.world_WeekIndex = World_Manager.instance.GetWeekIndex();
		saveData.world_SeasonIndex = World_Manager.instance.GetSeasonIndex();
		saveData.world_Forecast = World_Manager.instance.climate_Indexes;
		saveData.staff_Data = new List<Staff_Data>(Char_Manager.instance.staff_Data);
		saveData.staff_Possible_Data = new List<Staff_Data>(Char_Manager.instance.staff_Possible_Staff_Data);
		List<Char_Controller> list9 = new List<Char_Controller>(Char_Manager.instance.staff_Controllers);
		list9.TrimExcess();
		foreach (Char_Controller char_Controller in list9)
		{
			saveData.staff_SaveData.Add(char_Controller.gameObject.GetComponent<Staff_Controller>().GetSaveData());
		}
		saveData.taskSaveData_SD = new List<List<TaskSaveData>>();
		for (int num6 = 0; num6 < Missions_Manager.instance.task_Datas.Count; num6++)
		{
			saveData.taskSaveData_SD.Add(new List<TaskSaveData>());
			for (int num7 = 0; num7 < Missions_Manager.instance.task_Datas[num6].Count; num7++)
			{
				TaskSaveData taskSaveData = new TaskSaveData();
				taskSaveData.state = Missions_Manager.instance.task_Datas[num6][num7].state;
				taskSaveData.daysCurrent = Missions_Manager.instance.task_Datas[num6][num7].daysCurrent;
				taskSaveData.needsCurrent = Missions_Manager.instance.task_Datas[num6][num7].needsCurrent;
				taskSaveData.needsQnt = Missions_Manager.instance.task_Datas[num6][num7].needsQnt;
				taskSaveData.prod_Indexes = Inv_Manager.instance.GetItemIndex_List(Missions_Manager.instance.task_Datas[num6][num7].task_sell_Prods);
				taskSaveData.tags = Missions_Manager.instance.task_Datas[num6][num7].tags;
				taskSaveData.values = Missions_Manager.instance.task_Datas[num6][num7].values;
				saveData.taskSaveData_SD[num6].Add(taskSaveData);
			}
		}
		saveData.mail_data_SD = new List<Mail_Data>(Mail_Manager.instance.mails);
		saveData.easterEgg_data_SD = EasterEgg_Manager.instance.main_data;
		binaryFormatter.Serialize(fileStream2, saveData);
		fileStream2.Close();
		if (Menu_Manager.instance.GetMenuName() == "Pause")
		{
			Menu_Manager.instance.SetMenuName("MainMenu");
		}
		Invoker.InvokeDelayed(new Invokable(this.Stop_Is_Saving), 2f);
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0002DF80 File Offset: 0x0002C180
	private void NotifySaveGame()
	{
		Menu_Manager.instance.SetNotification("Attention!", "Game data saved!", null, 2f, true);
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0002DF9D File Offset: 0x0002C19D
	public void TryLoadGame(int _index)
	{
		if (Input_Manager.instance.GetScheme(-1) == "Joystick")
		{
			this.joystickIndexing = _index;
			return;
		}
		this.LoadGame(_index);
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x0002DFC8 File Offset: 0x0002C1C8
	public void LoadGame(int _index)
	{
		this.gameIndex = _index;
		if (!File.Exists(Application.persistentDataPath + "/MySaveData_" + _index.ToString() + ".dat"))
		{
			if (!Game_Manager.instance.MayRun())
			{
				Game_Manager.instance.NewGame();
			}
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_StartGame);
			return;
		}
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Open(Application.persistentDataPath + "/MySaveData_" + _index.ToString() + ".dat", FileMode.Open);
		if (fileStream.Length > 0L)
		{
			SaveData saveData = (SaveData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
			this.loadedData = saveData;
			if (!Game_Manager.instance.MayRun())
			{
				Game_Manager.instance.LoadGame();
			}
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_StartGame);
			return;
		}
		fileStream.Close();
		Menu_Manager.instance.SetWarning("Error!", "Corrupted Save File.\n\nPlease contact the development team through official channels for assistance on how to proceed.", "Start", true, -1);
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0002E0C4 File Offset: 0x0002C2C4
	public ArrayList LoadGameInfo(int _index)
	{
		ArrayList arrayList = new ArrayList();
		if (File.Exists(Application.persistentDataPath + "/MySaveData_" + _index.ToString() + ".dat"))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = File.Open(Application.persistentDataPath + "/MySaveData_" + _index.ToString() + ".dat", FileMode.Open);
			if (fileStream.Length <= 0L)
			{
				arrayList.Add(0);
				fileStream.Close();
				return arrayList;
			}
			SaveData saveData = (SaveData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
			arrayList.Add(1);
			if (saveData.martName_SD != null)
			{
				arrayList.Add(saveData.martName_SD);
			}
			else
			{
				arrayList.Add("Mart Name");
			}
			arrayList.Add(saveData.money_SD);
			arrayList.Add(saveData.playTime_SD);
			arrayList.Add(0);
			arrayList.Add(0);
			int awards_SD = saveData.awards_SD;
			arrayList.Add(saveData.awards_SD);
		}
		else
		{
			arrayList.Add(0);
		}
		return arrayList;
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x0002E1ED File Offset: 0x0002C3ED
	public void DeleteGame(int _index)
	{
		Menu_Manager.instance.SetWarningConfirmation("_delete_game_data_00", -1, "").onClick.AddListener(new UnityAction(this.DeleteGame2));
		this.gameToDelete = _index;
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0002E221 File Offset: 0x0002C421
	public void DeleteGame2()
	{
		this.DeleteGame2(this.gameToDelete);
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0002E230 File Offset: 0x0002C430
	public void DeleteGame2(int _index)
	{
		if (File.Exists(Application.persistentDataPath + "/MySaveData_" + _index.ToString() + ".dat"))
		{
			File.Delete(Application.persistentDataPath + "/MySaveData_" + _index.ToString() + ".dat");
			Menu_Manager.instance.RefreshSaveLoadInfo();
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_RemoveItem);
		}
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x0002E2A0 File Offset: 0x0002C4A0
	public void ResetData(int _index)
	{
		if (File.Exists(Application.persistentDataPath + "/MySaveData_" + _index.ToString() + ".dat"))
		{
			File.Delete(Application.persistentDataPath + "/MySaveData_" + _index.ToString() + ".dat");
			return;
		}
		Debug.LogError("No save data to delete.");
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x0002E2FA File Offset: 0x0002C4FA
	public bool CheckFile(int _index)
	{
		return File.Exists(Application.persistentDataPath + "/MySaveData_" + _index.ToString() + ".dat");
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x0002E321 File Offset: 0x0002C521
	public int GetSize()
	{
		return 3;
	}

	// Token: 0x04000541 RID: 1345
	public static Save_Manager instance;

	// Token: 0x04000542 RID: 1346
	private int gameIndex;

	// Token: 0x04000543 RID: 1347
	public bool is_saving;

	// Token: 0x04000544 RID: 1348
	private float resetTimer;

	// Token: 0x04000545 RID: 1349
	public SaveData loadedData = new SaveData();

	// Token: 0x04000546 RID: 1350
	public int joystickIndexing;

	// Token: 0x04000547 RID: 1351
	private int gameToDelete;
}
