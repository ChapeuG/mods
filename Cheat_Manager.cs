using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// Token: 0x0200003B RID: 59
public class Cheat_Manager : MonoBehaviour
{
	// Token: 0x06000219 RID: 537 RVA: 0x000136C4 File Offset: 0x000118C4
	private void Awake()
	{
		if (!Cheat_Manager.instance)
		{
			Cheat_Manager.instance = this;
		}
	}

	// Token: 0x0600021A RID: 538 RVA: 0x000136D8 File Offset: 0x000118D8
	private void Start()
	{
		if (!this.showAlphaInfo_bool)
		{
			this.showAlphaInfo_Index = 0;
			return;
		}
		this.showAlphaInfo_Index = 1;
	}

	// Token: 0x0600021B RID: 539 RVA: 0x000136F1 File Offset: 0x000118F1
	private void Update()
	{
		this.Update_DebugMenu();
	}

	// Token: 0x0600021C RID: 540 RVA: 0x000136FC File Offset: 0x000118FC
	public void SetDebug(int _index)
	{
		if (_index == 0)
		{
			this.ResetDebugTools();
			return;
		}
		if (_index == 1)
		{
			this.SetDebugNotifications();
			return;
		}
		if (_index == 2)
		{
			this.SetFirstPerson();
			return;
		}
		if (_index == 3)
		{
			this.SetFreeCamera();
			return;
		}
		if (_index == 4)
		{
			this.SetHideUI();
			Menu_Manager.instance.UpdateMenu();
			return;
		}
		if (_index == 5)
		{
			this.SetHideWorldUI();
			return;
		}
		if (_index == 6)
		{
			this.SetColorFilters();
			return;
		}
		if (_index == 7)
		{
			this.SetFreezeTimeScale();
			return;
		}
		if (_index == 8)
		{
			this.SetUltraTimeScale();
			return;
		}
		if (_index == 9)
		{
			this.SetFixedButtons();
		}
	}

	// Token: 0x0600021D RID: 541 RVA: 0x00013780 File Offset: 0x00011980
	public void SetDebug2(int _index)
	{
		if (_index == 1)
		{
			this.SetHidePlayer();
			return;
		}
		if (_index == 2)
		{
			this.SetSpawnCustomer();
			return;
		}
		if (_index == 3)
		{
			this.SetDeleteAllCustomers();
			return;
		}
		if (_index == 4)
		{
			this.SetNextTimeOfDay();
			return;
		}
		if (_index == 5)
		{
			this.SetNextSeason();
			return;
		}
		if (_index == 6)
		{
			this.SetAutoDelivery();
			return;
		}
		if (_index == 7)
		{
			this.SetThirdPerson();
			return;
		}
		if (_index == 8)
		{
			this.SetDropBoxes();
		}
	}

	// Token: 0x0600021E RID: 542 RVA: 0x000137E4 File Offset: 0x000119E4
	public void ResetDebugTools()
	{
		this.SetDebugNotifications(1);
		this.SetFirstPerson(0);
		this.SetFreeCamera(0);
		this.SetHideUI(0);
		this.SetHideWorldUI(0);
		this.SetColorFilters(0);
		this.SetFreezeTimeScale(0);
		this.SetUltraTimeScale(0);
		this.SetFixedButtons(0);
		this.SetHidePlayer(0);
		this.SetAutoDelivery(0);
		this.SetThirdPerson(0);
		this.SetDropBoxes(0);
		this.RefreshDebugMenu();
	}

	// Token: 0x0600021F RID: 543 RVA: 0x00013854 File Offset: 0x00011A54
	public void RefreshDebugMenu()
	{
		this.debugNotifications_Image.sprite = Settings_Manager.instance.toggleSprites[this.debugNotifications_Index];
		this.firstPerson_Image.sprite = Settings_Manager.instance.toggleSprites[this.firstPerson_Index];
		this.freeCamera_Image.sprite = Settings_Manager.instance.toggleSprites[this.freeCamera_Index];
		this.hideUI_Image.sprite = Settings_Manager.instance.toggleSprites[this.hideUI_Index];
		this.hideWorldUI_Image.sprite = Settings_Manager.instance.toggleSprites[this.hideWorldUI_Index];
		this.colorFilters_Text.text = "Profile " + this.colorFilters_Index.ToString();
		this.freezeTimeScale_Image.sprite = Settings_Manager.instance.toggleSprites[this.freezeTimeScale_Index];
		this.ultraTimeScale_Text.text = "x" + this.ultraTimeScale_List[this.ultraTimeScale_Index].ToString();
		this.fixedButtons_Text.text = this.fixedButtonNames[this.fixedButtons_Index];
		this.hidePlayer_Image.sprite = Settings_Manager.instance.toggleSprites[this.hidePlayer_Index];
		this.autoDelivery_Image.sprite = Settings_Manager.instance.toggleSprites[this.autoDelivery_Index];
		this.thirdPerson_Image.sprite = Settings_Manager.instance.toggleSprites[this.thirdPerson_Index];
		this.dropBoxes_Image.sprite = Settings_Manager.instance.toggleSprites[this.dropBoxes_Index];
		this.boxesNeverRunOut_Image.sprite = Settings_Manager.instance.toggleSprites[this.boxesNeverRunOut_Index];
		this.showAlphaInfo_Image.sprite = Settings_Manager.instance.toggleSprites[this.showAlphaInfo_Index];
	}

	// Token: 0x06000220 RID: 544 RVA: 0x00013A14 File Offset: 0x00011C14
	public void SetDebugNotifications()
	{
		if (this.debugNotifications_Index == 0)
		{
			this.debugNotifications_Index++;
		}
		else
		{
			this.debugNotifications_Index = 0;
		}
		this.SetDebugNotifications(this.debugNotifications_Index);
	}

	// Token: 0x06000221 RID: 545 RVA: 0x00013A41 File Offset: 0x00011C41
	private void SetDebugNotifications(int _index)
	{
		this.debugNotifications_Index = _index;
		this.RefreshDebugMenu();
		Menu_Manager.instance.SetNotification("Debug Tools", "Debug Notifications turned " + this.onOff[_index], null, 2f, false);
	}

	// Token: 0x06000222 RID: 546 RVA: 0x00013A78 File Offset: 0x00011C78
	public int GetDebugNotification()
	{
		return this.debugNotifications_Index;
	}

	// Token: 0x06000223 RID: 547 RVA: 0x00013A80 File Offset: 0x00011C80
	public void SetFirstPerson()
	{
		if (this.firstPerson_Index == 0)
		{
			this.firstPerson_Index++;
		}
		else
		{
			this.firstPerson_Index = 0;
		}
		if (this.GetFreeCamera() == 1)
		{
			this.SetFreeCamera(0);
		}
		if (this.GetThirdPerson() == 1)
		{
			this.SetThirdPerson(0);
		}
		this.SetFirstPerson(this.firstPerson_Index);
	}

	// Token: 0x06000224 RID: 548 RVA: 0x00013AD8 File Offset: 0x00011CD8
	private void SetFirstPerson(int _index)
	{
		if (_index == 0)
		{
			this.SetHidePlayer(0);
		}
		else
		{
			this.SetHidePlayer(1);
		}
		Shader.SetGlobalInt("_personMode", _index);
		this.firstPerson_Index = _index;
		this.RefreshDebugMenu();
		Menu_Manager.instance.SetNotification("Debug Tools", "First Person Camera turned " + this.onOff[_index], null, 2f, false);
	}

	// Token: 0x06000225 RID: 549 RVA: 0x00013B38 File Offset: 0x00011D38
	public int GetFirstPerson()
	{
		return this.firstPerson_Index;
	}

	// Token: 0x06000226 RID: 550 RVA: 0x00013B40 File Offset: 0x00011D40
	public void SetFreeCamera()
	{
		if (this.freeCamera_Index == 0)
		{
			this.freeCamera_Index++;
		}
		else
		{
			this.freeCamera_Index = 0;
		}
		if (this.GetFirstPerson() == 1)
		{
			this.SetFirstPerson(0);
		}
		if (this.GetThirdPerson() == 1)
		{
			this.SetThirdPerson(0);
		}
		this.SetFreeCamera(this.freeCamera_Index);
	}

	// Token: 0x06000227 RID: 551 RVA: 0x00013B98 File Offset: 0x00011D98
	private void SetFreeCamera(int _index)
	{
		this.freeCamera_Index = _index;
		this.RefreshDebugMenu();
		if (_index == 1)
		{
			Menu_Manager.instance.SetNotification("Debug Tools", "Free Camera turned " + this.onOff[_index] + "\n\n\nMouse Inputs:\n\nLeft: X/Z Movement\nCenter: Rotate\nRight: X/Y Movement\nScroll: Zoom", null, 4f, false);
			return;
		}
		Menu_Manager.instance.SetNotification("Debug Tools", "Free Camera turned " + this.onOff[_index], null, 2f, false);
	}

	// Token: 0x06000228 RID: 552 RVA: 0x00013C0C File Offset: 0x00011E0C
	public int GetFreeCamera()
	{
		return this.freeCamera_Index;
	}

	// Token: 0x06000229 RID: 553 RVA: 0x00013C14 File Offset: 0x00011E14
	public void SetHideUI()
	{
		if (this.hideUI_Index == 0)
		{
			this.hideUI_Index++;
		}
		else
		{
			this.hideUI_Index = 0;
		}
		this.SetHideUI(this.hideUI_Index);
	}

	// Token: 0x0600022A RID: 554 RVA: 0x00013C41 File Offset: 0x00011E41
	private void SetHideUI(int _index)
	{
		this.hideUI_Index = _index;
		this.RefreshDebugMenu();
		Menu_Manager.instance.SetNotification("Debug Tools", "Hide UI turned " + this.onOff[_index], null, 2f, false);
	}

	// Token: 0x0600022B RID: 555 RVA: 0x00013C78 File Offset: 0x00011E78
	public int GetHideUI()
	{
		return this.hideUI_Index;
	}

	// Token: 0x0600022C RID: 556 RVA: 0x00013C80 File Offset: 0x00011E80
	public void SetHideWorldUI()
	{
		if (this.hideWorldUI_Index == 0)
		{
			this.hideWorldUI_Index++;
		}
		else
		{
			this.hideWorldUI_Index = 0;
		}
		this.SetHideWorldUI(this.hideWorldUI_Index);
	}

	// Token: 0x0600022D RID: 557 RVA: 0x00013CB0 File Offset: 0x00011EB0
	private void SetHideWorldUI(int _index)
	{
		if (_index == 0)
		{
			this.worldUI.SetActive(true);
		}
		else
		{
			this.worldUI.SetActive(false);
		}
		this.hideWorldUI_Index = _index;
		this.RefreshDebugMenu();
		Menu_Manager.instance.SetNotification("Debug Tools", "Hide WORLD UI turned " + this.onOff[_index], null, 2f, false);
	}

	// Token: 0x0600022E RID: 558 RVA: 0x00013D0F File Offset: 0x00011F0F
	public int GetHideWorldUI()
	{
		return this.hideWorldUI_Index;
	}

	// Token: 0x0600022F RID: 559 RVA: 0x00013D17 File Offset: 0x00011F17
	public void SetColorFilters()
	{
		if (this.colorFilters_Index < this.colorFilter_Profiles.Length - 1)
		{
			this.colorFilters_Index++;
		}
		else
		{
			this.colorFilters_Index = 0;
		}
		this.SetColorFilters(this.colorFilters_Index);
	}

	// Token: 0x06000230 RID: 560 RVA: 0x00013D50 File Offset: 0x00011F50
	private void SetColorFilters(int _index)
	{
		this.colorFilters_Index = _index;
		this.colorFilter_Volume.profile = this.colorFilter_Profiles[_index];
		this.RefreshDebugMenu();
		Menu_Manager.instance.SetNotification("Debug Tools", "Color Filter set to Profile " + _index.ToString(), null, 2f, false);
	}

	// Token: 0x06000231 RID: 561 RVA: 0x00013DA4 File Offset: 0x00011FA4
	public void SetFreezeTimeScale()
	{
		if (this.freezeTimeScale_Index == 0)
		{
			this.freezeTimeScale_Index++;
		}
		else
		{
			this.freezeTimeScale_Index = 0;
		}
		this.SetFreezeTimeScale(this.freezeTimeScale_Index);
	}

	// Token: 0x06000232 RID: 562 RVA: 0x00013DD4 File Offset: 0x00011FD4
	private void SetFreezeTimeScale(int _index)
	{
		if (_index == 0)
		{
			Time.timeScale = 1f * this.ultraTimeScale_Value;
		}
		else
		{
			Time.timeScale = 0f;
		}
		this.freezeTimeScale_Index = _index;
		this.RefreshDebugMenu();
		Menu_Manager.instance.SetNotification("Debug Tools", "Time Scale set to " + Time.timeScale.ToString(), null, 2f, false);
	}

	// Token: 0x06000233 RID: 563 RVA: 0x00013E3B File Offset: 0x0001203B
	public float GetFreezeTimeScale()
	{
		return (float)this.freezeTimeScale_Index;
	}

	// Token: 0x06000234 RID: 564 RVA: 0x00013E44 File Offset: 0x00012044
	public void SetUltraTimeScale()
	{
		if (this.ultraTimeScale_Index < this.ultraTimeScale_List.Length - 1)
		{
			this.ultraTimeScale_Index++;
		}
		else
		{
			this.ultraTimeScale_Index = 0;
		}
		this.SetUltraTimeScale(this.ultraTimeScale_Index);
	}

	// Token: 0x06000235 RID: 565 RVA: 0x00013E7C File Offset: 0x0001207C
	private void SetUltraTimeScale(int _index)
	{
		this.ultraTimeScale_Value = this.ultraTimeScale_List[_index];
		this.ultraTimeScale_Index = _index;
		Time.timeScale = this.ultraTimeScale_Value;
		this.RefreshDebugMenu();
		Menu_Manager.instance.SetNotification("Debug Tools", "Time Scale multiplier set to " + this.ultraTimeScale_Value.ToString(), null, 2f, false);
	}

	// Token: 0x06000236 RID: 566 RVA: 0x00013EDA File Offset: 0x000120DA
	public float GetUltraTimeScaleValue()
	{
		return this.ultraTimeScale_Value;
	}

	// Token: 0x06000237 RID: 567 RVA: 0x00013EE2 File Offset: 0x000120E2
	public void SetFixedButtons()
	{
		if (this.fixedButtons_Index < this.fixedButtonNames.Length - 1)
		{
			this.fixedButtons_Index++;
		}
		else
		{
			this.fixedButtons_Index = 0;
		}
		this.SetFixedButtons(this.fixedButtons_Index);
	}

	// Token: 0x06000238 RID: 568 RVA: 0x00013F1C File Offset: 0x0001211C
	private void SetFixedButtons(int _index)
	{
		this.fixedButtons_Index = _index;
		Input_Manager.instance.RefreshInputHints();
		this.RefreshDebugMenu();
		Menu_Manager.instance.SetNotification("Debug Tools", "Fixed Buttons set to " + this.fixedButtonNames[_index], null, 2f, false);
	}

	// Token: 0x06000239 RID: 569 RVA: 0x00013F68 File Offset: 0x00012168
	public string GetFixedButtons()
	{
		return this.fixedButtonNames[this.fixedButtons_Index];
	}

	// Token: 0x0600023A RID: 570 RVA: 0x00013F77 File Offset: 0x00012177
	public void SetHidePlayer()
	{
		if (this.hidePlayer_Index == 0)
		{
			this.hidePlayer_Index++;
		}
		else
		{
			this.hidePlayer_Index = 0;
		}
		this.SetHidePlayer(this.hidePlayer_Index);
	}

	// Token: 0x0600023B RID: 571 RVA: 0x00013FA4 File Offset: 0x000121A4
	private void SetHidePlayer(int _index)
	{
		Player_Controller playerController = Player_Manager.instance.GetPlayerController(0);
		if (_index == 0)
		{
			playerController.SetPlayerMeshVisibility(true);
		}
		else
		{
			playerController.SetPlayerMeshVisibility(false);
		}
		this.hidePlayer_Index = _index;
		this.RefreshDebugMenu();
		Menu_Manager.instance.SetNotification("Debug Tools", "Hide player turned " + this.onOff[_index], null, 2f, false);
	}

	// Token: 0x0600023C RID: 572 RVA: 0x00014005 File Offset: 0x00012205
	public float GetHidePlayer()
	{
		return (float)this.hidePlayer_Index;
	}

	// Token: 0x0600023D RID: 573 RVA: 0x0001400E File Offset: 0x0001220E
	public void SetSpawnCustomer()
	{
		Char_Manager.instance.CreateCustomerByPossibility();
		Menu_Manager.instance.SetNotification("Debug Tools", "Customer Spawned", null, 1f, false);
	}

	// Token: 0x0600023E RID: 574 RVA: 0x00014035 File Offset: 0x00012235
	public void SetDeleteAllCustomers()
	{
		Char_Manager.instance.DestroyAllCustomers();
		Menu_Manager.instance.SetNotification("Debug Tools", "All Customers Deleted", null, 1f, false);
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0001405C File Offset: 0x0001225C
	public void SetNextTimeOfDay()
	{
		World_Manager.instance.SetTime(World_Manager.instance.GetTime() - 60f);
		Menu_Manager.instance.SetNotification("Debug Tools", "Time of Day Increased", null, 2f, false);
	}

	// Token: 0x06000240 RID: 576 RVA: 0x00014093 File Offset: 0x00012293
	public void SetNextSeason()
	{
		World_Manager.instance.SetNextSeason();
		Menu_Manager.instance.SetNotification("Debug Tools", "Season set to " + World_Manager.instance.GetSeasonName(), null, 1f, false);
	}

	// Token: 0x06000241 RID: 577 RVA: 0x000140C9 File Offset: 0x000122C9
	public void SetAutoDelivery()
	{
		if (this.autoDelivery_Index == 0)
		{
			this.autoDelivery_Index++;
		}
		else
		{
			this.autoDelivery_Index = 0;
		}
		this.RefreshDebugMenu();
		this.SetAutoDelivery(this.autoDelivery_Index);
	}

	// Token: 0x06000242 RID: 578 RVA: 0x000140FC File Offset: 0x000122FC
	public void SetAutoDelivery(int _index)
	{
		this.autoDelivery_Index = _index;
		this.RefreshDebugMenu();
		Menu_Manager.instance.SetNotification("Debug Tools", "Auto Delivery turned " + this.onOff[_index], null, 2f, false);
	}

	// Token: 0x06000243 RID: 579 RVA: 0x00014133 File Offset: 0x00012333
	public float GetAutoDelivery()
	{
		return (float)this.autoDelivery_Index;
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0001413C File Offset: 0x0001233C
	public void SetThirdPerson()
	{
		if (this.thirdPerson_Index == 0)
		{
			this.thirdPerson_Index++;
		}
		else
		{
			this.thirdPerson_Index = 0;
		}
		if (this.GetFreeCamera() == 1)
		{
			this.SetFreeCamera(0);
		}
		if (this.GetFirstPerson() == 1)
		{
			this.SetFirstPerson(0);
		}
		this.SetThirdPerson(this.thirdPerson_Index);
	}

	// Token: 0x06000245 RID: 581 RVA: 0x00014194 File Offset: 0x00012394
	private void SetThirdPerson(int _index)
	{
		this.thirdPerson_Index = _index;
		this.RefreshDebugMenu();
		Menu_Manager.instance.SetNotification("Debug Tools", "First Person Zoom turned " + this.onOff[_index], null, 2f, false);
	}

	// Token: 0x06000246 RID: 582 RVA: 0x000141CB File Offset: 0x000123CB
	public int GetThirdPerson()
	{
		return this.thirdPerson_Index;
	}

	// Token: 0x06000247 RID: 583 RVA: 0x000141D3 File Offset: 0x000123D3
	public void SetDropBoxes()
	{
		if (this.dropBoxes_Index == 0)
		{
			this.dropBoxes_Index++;
		}
		else
		{
			this.dropBoxes_Index = 0;
		}
		this.SetDropBoxes(this.dropBoxes_Index);
	}

	// Token: 0x06000248 RID: 584 RVA: 0x00014200 File Offset: 0x00012400
	public void SetDropBoxes(int _index)
	{
		if (_index == 0)
		{
			this.dropedBoxesQnt = 0;
		}
		this.dropBoxes_Index = _index;
		this.RefreshDebugMenu();
		Menu_Manager.instance.SetNotification("Debug Tools", "Drop Boxes Indefinitely turned " + this.onOff[_index], null, 2f, false);
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0001424C File Offset: 0x0001244C
	public int GetDropBoxes()
	{
		return this.dropBoxes_Index;
	}

	// Token: 0x0600024A RID: 586 RVA: 0x00014254 File Offset: 0x00012454
	public void SetBoxesNeverRunOut()
	{
		if (this.boxesNeverRunOut_Index == 0)
		{
			this.boxesNeverRunOut_Index++;
		}
		else
		{
			this.boxesNeverRunOut_Index = 0;
		}
		this.SetBoxesNeverRunOut(this.boxesNeverRunOut_Index);
	}

	// Token: 0x0600024B RID: 587 RVA: 0x00014281 File Offset: 0x00012481
	public void SetBoxesNeverRunOut(int _index)
	{
		this.boxesNeverRunOut_Index = _index;
		this.RefreshDebugMenu();
		Menu_Manager.instance.SetNotification("Debug Tools", "Boxes never run out turned " + this.onOff[_index], null, 2f, false);
	}

	// Token: 0x0600024C RID: 588 RVA: 0x000142B8 File Offset: 0x000124B8
	public int GetBoxesNeverRunOut()
	{
		return this.boxesNeverRunOut_Index;
	}

	// Token: 0x0600024D RID: 589 RVA: 0x000142C0 File Offset: 0x000124C0
	public void SetShowAlphaInfo()
	{
		if (this.showAlphaInfo_Index == 0)
		{
			this.showAlphaInfo_Index++;
		}
		else
		{
			this.showAlphaInfo_Index = 0;
		}
		this.SetShowAlphaInfo(this.showAlphaInfo_Index);
	}

	// Token: 0x0600024E RID: 590 RVA: 0x000142ED File Offset: 0x000124ED
	public void SetShowAlphaInfo(int _index)
	{
		this.showAlphaInfo_Index = _index;
		this.RefreshDebugMenu();
		Menu_Manager.instance.SetNotification("Debug Tools", "Show Alpha Warning turned " + this.onOff[_index], null, 2f, false);
	}

	// Token: 0x0600024F RID: 591 RVA: 0x00014324 File Offset: 0x00012524
	public int GetShowAlphaInfo()
	{
		return this.showAlphaInfo_Index;
	}

	// Token: 0x06000250 RID: 592 RVA: 0x0001432C File Offset: 0x0001252C
	public void SetAddMoney()
	{
		Finances_Manager.instance.AddMoney(5000f);
		Menu_Manager.instance.SetNotification("Debug Tools", "5000 coins added", null, 1f, false);
	}

	// Token: 0x06000251 RID: 593 RVA: 0x00014358 File Offset: 0x00012558
	public void SetRemoveMoney()
	{
		Finances_Manager.instance.SetMoney(0f);
		Menu_Manager.instance.SetNotification("Debug Tools", "Removed all coins", null, 1f, false);
	}

	// Token: 0x06000252 RID: 594 RVA: 0x00014384 File Offset: 0x00012584
	public void SpawnOnlyThisCustomer_Add(int _add)
	{
		List<Char_Controller> allCustomerPrefabs = Char_Manager.instance.GetAllCustomerPrefabs();
		int count = allCustomerPrefabs.Count;
		List<int> list = new List<int>();
		for (int i = 0; i < allCustomerPrefabs.Count; i++)
		{
			if (allCustomerPrefabs[i] && allCustomerPrefabs[i].inGame)
			{
				list.Add(i);
			}
		}
		this.spawnOnlyCustomer_Index += _add;
		if (this.spawnOnlyCustomer_Index < -1)
		{
			this.spawnOnlyCustomer_Index = list.Count - 1;
		}
		if (this.spawnOnlyCustomer_Index >= list.Count)
		{
			this.spawnOnlyCustomer_Index = -1;
		}
		if (this.spawnOnlyCustomer_Index == -1)
		{
			this.spawnOnlyCustomer = -1;
		}
		else
		{
			this.spawnOnlyCustomer = list[this.spawnOnlyCustomer_Index];
		}
		this.spawnOnlyCustomer_Text.text = this.spawnOnlyCustomer.ToString();
		this.SetDeleteAllCustomers();
	}

	// Token: 0x06000253 RID: 595 RVA: 0x00014458 File Offset: 0x00012658
	public void Do56Missions()
	{
		int qnt = UnityEngine.Random.Range(7, 7);
		for (int i = 0; i < 56; i++)
		{
			Unlock_Manager.instance.Set_UnlockRandomItem(qnt, false, 1);
		}
	}

	// Token: 0x06000254 RID: 596 RVA: 0x00014488 File Offset: 0x00012688
	public void DeleteAuditorTasks()
	{
		foreach (TaskData taskData in Missions_Manager.instance.task_Datas[1])
		{
			taskData.Finish_Task(true);
		}
		Missions_Manager.instance.mayCreateTask = true;
	}

	// Token: 0x06000255 RID: 597 RVA: 0x000144F0 File Offset: 0x000126F0
	public void Change_Level(int _level_index)
	{
		Game_Manager.instance.BuyRelocation(_level_index);
	}

	// Token: 0x06000256 RID: 598 RVA: 0x000144FD File Offset: 0x000126FD
	public void Expand_Level()
	{
		Game_Manager.instance.BuyExpansionIndex(true);
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0001450A File Offset: 0x0001270A
	private void Update_DebugMenu()
	{
		if (this.debug_menu_match != "!")
		{
			this.debug_menu_TIMER -= Time.unscaledDeltaTime;
			if (this.debug_menu_TIMER <= 0f)
			{
				this.DebugMenu_Reset();
			}
		}
	}

	// Token: 0x06000258 RID: 600 RVA: 0x00014543 File Offset: 0x00012743
	private void DebugMenu_Reset()
	{
		this.debug_menu_match = "";
		this.debug_menu_TIMER = this.debug_menu_time;
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0001455C File Offset: 0x0001275C
	private void CheckMatch()
	{
		if (this.debug_menu_string == this.debug_menu_match)
		{
			Menu_Manager.instance.SetMenuName("DebugTools");
		}
		this.DebugMenu_Reset();
	}

	// Token: 0x0600025A RID: 602 RVA: 0x00014586 File Offset: 0x00012786
	public void DebugMenu_Set(string _input)
	{
		Menu_Manager.instance.GetMenuName() != "MainMenu";
		if (_input == "!")
		{
			this.CheckMatch();
			return;
		}
		this.debug_menu_match += _input;
	}

	// Token: 0x040002FE RID: 766
	public static Cheat_Manager instance;

	// Token: 0x040002FF RID: 767
	private string[] onOff = new string[]
	{
		"OFF",
		"ON"
	};

	// Token: 0x04000300 RID: 768
	[SerializeField]
	private Image debugNotifications_Image;

	// Token: 0x04000301 RID: 769
	private int debugNotifications_Index = 1;

	// Token: 0x04000302 RID: 770
	[SerializeField]
	private Image firstPerson_Image;

	// Token: 0x04000303 RID: 771
	[SerializeField]
	private GameObject playerMesh;

	// Token: 0x04000304 RID: 772
	private int firstPerson_Index;

	// Token: 0x04000305 RID: 773
	[SerializeField]
	private Image freeCamera_Image;

	// Token: 0x04000306 RID: 774
	private int freeCamera_Index;

	// Token: 0x04000307 RID: 775
	[Header("Spawn Only this customer")]
	public Text spawnOnlyCustomer_Text;

	// Token: 0x04000308 RID: 776
	public int spawnOnlyCustomer = -1;

	// Token: 0x04000309 RID: 777
	private int spawnOnlyCustomer_Index = -1;

	// Token: 0x0400030A RID: 778
	[SerializeField]
	private bool showAlphaInfo_bool;

	// Token: 0x0400030B RID: 779
	[SerializeField]
	private Image showAlphaInfo_Image;

	// Token: 0x0400030C RID: 780
	private int showAlphaInfo_Index;

	// Token: 0x0400030D RID: 781
	[SerializeField]
	private Image boxesNeverRunOut_Image;

	// Token: 0x0400030E RID: 782
	private int boxesNeverRunOut_Index;

	// Token: 0x0400030F RID: 783
	[SerializeField]
	private Image dropBoxes_Image;

	// Token: 0x04000310 RID: 784
	public int dropedBoxesQnt;

	// Token: 0x04000311 RID: 785
	private int dropBoxes_Index;

	// Token: 0x04000312 RID: 786
	[SerializeField]
	private Image thirdPerson_Image;

	// Token: 0x04000313 RID: 787
	private int thirdPerson_Index;

	// Token: 0x04000314 RID: 788
	[SerializeField]
	private Image hideUI_Image;

	// Token: 0x04000315 RID: 789
	private int hideUI_Index;

	// Token: 0x04000316 RID: 790
	[SerializeField]
	private Image hideWorldUI_Image;

	// Token: 0x04000317 RID: 791
	[SerializeField]
	private GameObject worldUI;

	// Token: 0x04000318 RID: 792
	private int hideWorldUI_Index;

	// Token: 0x04000319 RID: 793
	[SerializeField]
	private Image autoDelivery_Image;

	// Token: 0x0400031A RID: 794
	private int autoDelivery_Index;

	// Token: 0x0400031B RID: 795
	[SerializeField]
	private Image hidePlayer_Image;

	// Token: 0x0400031C RID: 796
	private int hidePlayer_Index;

	// Token: 0x0400031D RID: 797
	[SerializeField]
	private Text fixedButtons_Text;

	// Token: 0x0400031E RID: 798
	private string[] fixedButtonNames = new string[]
	{
		"AUTO",
		"Keyboard&Mouse",
		"Joystick"
	};

	// Token: 0x0400031F RID: 799
	private int fixedButtons_Index;

	// Token: 0x04000320 RID: 800
	[SerializeField]
	private Text ultraTimeScale_Text;

	// Token: 0x04000321 RID: 801
	[HideInInspector]
	public float ultraTimeScale_Value = 1f;

	// Token: 0x04000322 RID: 802
	private float[] ultraTimeScale_List = new float[]
	{
		1f,
		10f,
		30f
	};

	// Token: 0x04000323 RID: 803
	private int ultraTimeScale_Index;

	// Token: 0x04000324 RID: 804
	[SerializeField]
	private Text colorFilters_Text;

	// Token: 0x04000325 RID: 805
	[SerializeField]
	private Volume colorFilter_Volume;

	// Token: 0x04000326 RID: 806
	[SerializeField]
	private VolumeProfile[] colorFilter_Profiles;

	// Token: 0x04000327 RID: 807
	private int colorFilters_Index;

	// Token: 0x04000328 RID: 808
	[SerializeField]
	private Image freezeTimeScale_Image;

	// Token: 0x04000329 RID: 809
	private int freezeTimeScale_Index;

	// Token: 0x0400032A RID: 810
	public float debug_menu_time = 5f;

	// Token: 0x0400032B RID: 811
	public float debug_menu_TIMER = 5f;

	// Token: 0x0400032C RID: 812
	private string debug_menu_string = "fdebug";

	// Token: 0x0400032D RID: 813
	public string debug_menu_match = "";
}
