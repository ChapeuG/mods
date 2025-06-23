using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000038 RID: 56
public class Char_Manager : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x060001D0 RID: 464 RVA: 0x00011A24 File Offset: 0x0000FC24
	public float affectionValue_Max { get; } = 100f;

	// Token: 0x060001D1 RID: 465 RVA: 0x00011A2C File Offset: 0x0000FC2C
	private void Awake()
	{
		if (!Char_Manager.instance)
		{
			Char_Manager.instance = this;
		}
		this.CreateReferences_Customers();
		this.CreateReferences_LocalCustomer_Datas();
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00011A4C File Offset: 0x0000FC4C
	private void Update()
	{
		this.UpdateCustomerSystem();
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x00011A54 File Offset: 0x0000FC54
	public string Get_Char_Name(int _type, int _index)
	{
		string result = "";
		if (_type == 1)
		{
			result = this.story_Prefabs[_index].charName;
		}
		else if (_type == 2)
		{
			result = this.customer_Prefabs_Available[_index].charName;
		}
		return result;
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x00011A98 File Offset: 0x0000FC98
	public Sprite Get_Char_Sprite(int _type, int _index)
	{
		Sprite result = null;
		if (_type == 1)
		{
			result = this.story_Prefabs[_index].charSprite;
		}
		else if (_type == 2)
		{
			result = this.customer_Prefabs_Available[_index].charSprite;
		}
		return result;
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x00011AD8 File Offset: 0x0000FCD8
	public void CreateReferences_LocalCustomer_Datas()
	{
		this.localCustomer_Datas.Clear();
		for (int i = 0; i < this.customer_Prefabs_Available.Count; i++)
		{
			LocalCustomer_Data localCustomer_Data = new LocalCustomer_Data();
			if (!this.customer_Prefabs_Available[i])
			{
				this.localCustomer_Datas.Add(localCustomer_Data);
			}
			else
			{
				List<Prod_Controller> list = new List<Prod_Controller>(this.customer_Prefabs_Available[i].GetComponent<Customer_Controller>().GetProdPredilectionList());
				List<bool> list2 = new List<bool>();
				foreach (Prod_Controller prod_Controller in list)
				{
					list2.Add(false);
				}
				localCustomer_Data.prodPreferenceUnlocked = list2;
				this.localCustomer_Datas.Add(localCustomer_Data);
			}
		}
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x00011BA8 File Offset: 0x0000FDA8
	public void Load_LocalCustomer_Datas(SaveData _data)
	{
		if (_data.localCustomer_Datas == null)
		{
			return;
		}
		for (int i = 0; i < this.customer_Prefabs_Available.Count; i++)
		{
			if (i < _data.localCustomer_Datas.Count && _data.localCustomer_Datas[i] != null)
			{
				this.localCustomer_Datas[i] = _data.localCustomer_Datas[i];
			}
		}
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00011C08 File Offset: 0x0000FE08
	public void SetProdPreferenceUnlocked(int _custIndex, int _prefIndex, bool _bool)
	{
		this.localCustomer_Datas[_custIndex].prodPreferenceUnlocked[_prefIndex] = _bool;
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x00011C22 File Offset: 0x0000FE22
	public LocalCustomer_Data GetLocalCustomer_Data(int _index)
	{
		return this.localCustomer_Datas[_index];
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x00011C30 File Offset: 0x0000FE30
	private void CreateReferences_Customers()
	{
		GameObject[] array = Resources.LoadAll<GameObject>("Characters/Customers/Prefabs");
		for (int i = 0; i < array.Length; i++)
		{
			Char_Controller char_Controller = Resources.Load<Char_Controller>("Characters/Customers/Prefabs/Client_" + i.ToString());
			if (char_Controller.inGame)
			{
				this.customer_Prefabs_Available.Add(char_Controller);
			}
			else
			{
				this.customer_Prefabs_Available.Add(null);
			}
			this.customerLifeAchievements.Add(false);
			this.customerProdWantedNow.Add(-1);
		}
		for (int j = 0; j < 2; j++)
		{
			this.lastCustomerCreated.Add(-1);
		}
	}

	// Token: 0x060001DA RID: 474 RVA: 0x00011CC0 File Offset: 0x0000FEC0
	private void UpdateCustomerSystem()
	{
		if (!Game_Manager.instance.MayRun())
		{
			return;
		}
		if (!Game_Manager.instance.GetMartOpen())
		{
			return;
		}
		if (this.customer_Controllers.Count < World_Manager.instance.GetLevelMaxCustomerQnt())
		{
			this.createCustomerTime_Timer -= Time.deltaTime;
			if (this.createCustomerTime_Timer <= 0f)
			{
				Score_Manager.instance.RefreshTaigaMembershipIndex();
				this.RandomizeCreateCustomerTime(World_Manager.instance.GetLevelSpawnTimeMax());
				this.CreateCustomerByPossibility();
			}
		}
	}

	// Token: 0x060001DB RID: 475 RVA: 0x00011D40 File Offset: 0x0000FF40
	public void CreateCustomerByPossibility()
	{
		if (Inv_Manager.instance.GetProdListOnShelves().Count <= 0)
		{
			return;
		}
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < this.customer_Prefabs_Available.Count; i++)
		{
			if (this.customer_Prefabs_Available[i])
			{
				list.Add(i);
			}
		}
		for (int j = 0; j < this.customer_Controllers.Count; j++)
		{
			int id = this.customer_Controllers[j].GetID();
			list2.Add(id);
			if (list.Contains(id))
			{
				list.Remove(id);
			}
		}
		if (Cheat_Manager.instance.spawnOnlyCustomer != -1)
		{
			if (list2.Contains(Cheat_Manager.instance.spawnOnlyCustomer))
			{
				return;
			}
			this.CreateCustomer(Cheat_Manager.instance.spawnOnlyCustomer);
			return;
		}
		else
		{
			int num = World_Manager.instance.GetLevelMaxCustomerQnt() - this.customer_Controllers.Count;
			int num2 = this.customer_Prefabs_Available.Count - this.customer_Controllers.Count - num;
			for (int k = 0; k < this.lastCustomerCreated.Count; k++)
			{
				if (k < num2 && list.Contains(this.lastCustomerCreated[k]))
				{
					list.Remove(this.lastCustomerCreated[k]);
				}
			}
			float[] array = new float[]
			{
				30f,
				60f,
				80f
			};
			if ((float)UnityEngine.Random.Range(0, 100) > array[Score_Manager.instance.RefreshTaigaMembershipIndex()])
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				this.CreateCustomer(list[index]);
				return;
			}
			this.CreateGenericCustomer();
			return;
		}
	}

	// Token: 0x060001DC RID: 476 RVA: 0x00011EE6 File Offset: 0x000100E6
	public void RandomizeCreateCustomerTime(float _max)
	{
		this.createCustomerTime_Timer = UnityEngine.Random.Range(this.createCustomerTime_Min, _max);
	}

	// Token: 0x060001DD RID: 477 RVA: 0x00011EFA File Offset: 0x000100FA
	public void ZeroCreateCustomerTime()
	{
		this.createCustomerTime_Timer = 0f;
	}

	// Token: 0x060001DE RID: 478 RVA: 0x00011F08 File Offset: 0x00010108
	private async void RefreshLastCustomerCreated(int _Index)
	{
		for (int i = this.lastCustomerCreated.Count - 1; i < 1; i--)
		{
			this.lastCustomerCreated[i] = this.lastCustomerCreated[i - 1];
		}
		this.lastCustomerCreated[0] = _Index;
	}

	// Token: 0x060001DF RID: 479 RVA: 0x00011F4C File Offset: 0x0001014C
	public Char_Controller CreateGenericCustomer()
	{
		GameObject navExitPointRandom = Nav_Manager.instance.GetNavExitPointRandom();
		Char_Controller char_Controller = UnityEngine.Object.Instantiate<Char_Controller>(this.genCustomer_Prefab.gameObject.GetComponent<Char_Controller>());
		char_Controller.transform.position = navExitPointRandom.transform.position + Vector3.up;
		char_Controller.transform.rotation = navExitPointRandom.transform.rotation;
		char_Controller.transform.localScale = Vector3.zero;
		char_Controller.gameObject.SetActive(true);
		if (!this.customer_Controllers.Contains(char_Controller))
		{
			this.customer_Controllers.Add(char_Controller);
		}
		this.Create_EmojiCtrl(char_Controller);
		return char_Controller;
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x00011FF0 File Offset: 0x000101F0
	public Char_Controller CreateCustomer(int _index)
	{
		GameObject navExitPointRandom = Nav_Manager.instance.GetNavExitPointRandom();
		Char_Controller char_Controller = UnityEngine.Object.Instantiate<Char_Controller>(this.customer_Prefabs_Available[_index].gameObject.GetComponent<Char_Controller>());
		char_Controller.transform.position = navExitPointRandom.transform.position + Vector3.up;
		char_Controller.transform.rotation = navExitPointRandom.transform.rotation;
		char_Controller.transform.localScale = Vector3.zero;
		char_Controller.gameObject.SetActive(true);
		char_Controller.SetID(_index);
		if (!this.customer_Controllers.Contains(char_Controller))
		{
			this.customer_Controllers.Add(char_Controller);
		}
		this.RefreshLastCustomerCreated(_index);
		this.Create_EmojiCtrl(char_Controller);
		return char_Controller;
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x000120A8 File Offset: 0x000102A8
	public void Create_EmojiCtrl(Char_Controller _char)
	{
		Emoji_Controller component = UnityEngine.Object.Instantiate<GameObject>(this.emoji_Prefab).GetComponent<Emoji_Controller>();
		component.SetObjToFollow(_char.transform.gameObject);
		_char.customer_Controller.emotion_ctrl = component;
		_char.customer_Controller.emotionIndex = 2;
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x000120F0 File Offset: 0x000102F0
	public void Create_ProdWantedCtrl(Char_Controller _char)
	{
		Emoji_Controller component = UnityEngine.Object.Instantiate<GameObject>(this.prodWanted_Prefab).GetComponent<Emoji_Controller>();
		component.SetObjToFollow(_char.transform.gameObject);
		_char.customer_Controller.prodWanted_ctrl = component;
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x0001212B File Offset: 0x0001032B
	public void DestroyCustomer(Char_Controller _controller)
	{
		if (this.customer_Controllers.Contains(_controller))
		{
			this.customer_Controllers.Remove(_controller);
		}
		UnityEngine.Object.Destroy(_controller.gameObject);
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x00012154 File Offset: 0x00010354
	public void DestroyAllCustomers()
	{
		for (int i = 0; i < this.customer_Controllers.Count; i++)
		{
			if (this.customer_Controllers[i])
			{
				UnityEngine.Object.Destroy(this.customer_Controllers[i].gameObject);
			}
		}
		this.customer_Controllers.Clear();
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x000121AB File Offset: 0x000103AB
	public Char_Controller GetCustomerPrefab(int _index)
	{
		return this.customer_Prefabs_Available[_index];
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x000121B9 File Offset: 0x000103B9
	public List<Char_Controller> GetAllCustomerPrefabs()
	{
		return this.customer_Prefabs_Available;
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x000121C4 File Offset: 0x000103C4
	public void LoadCustomers(SaveData _data)
	{
		for (int i = 0; i < _data.customerIndex_SD.Count; i++)
		{
			Char_Controller char_Controller = this.CreateCustomer(_data.customerIndex_SD[i]);
			char_Controller.transform.position = _data.customerPosition_SD[i];
			if (_data.customerProdList_SD[i] != "null")
			{
				List<int> list = new List<int>();
				string[] array = _data.customerProdList_SD[i].Split(new char[]
				{
					char.Parse("_")
				});
				for (int j = 0; j < array.Length; j++)
				{
					list.Add(int.Parse(array[j]));
				}
				char_Controller.customer_Controller.prod_BuyList = new List<int>(list);
			}
			char_Controller.customer_Controller.doneShopping = _data.customerDoneShopping[i];
			char_Controller.customer_Controller.doneCashier = _data.customerDoneCashier[i];
		}
		this.createCustomerTime_Timer = _data.createCustomerTimer_SD;
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x000122CB File Offset: 0x000104CB
	public List<bool> GetAllCustomerLifeAchievements()
	{
		return this.customerLifeAchievements;
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x000122D3 File Offset: 0x000104D3
	public List<int> GetAllCustomerProdWantedNow()
	{
		return this.customerProdWantedNow;
	}

	// Token: 0x060001EA RID: 490 RVA: 0x000122DC File Offset: 0x000104DC
	public void LoadCustomerLifeAchievement(SaveData _data)
	{
		if (this.customerLifeAchievements.Count == _data.customerLifeAchievementsByIndex.Count)
		{
			this.customerLifeAchievements = _data.customerLifeAchievementsByIndex;
			return;
		}
		for (int i = 0; i < _data.customerLifeAchievementsByIndex.Count; i++)
		{
			if (this.customerLifeAchievements[i] && _data.customerLifeAchievementsByIndex[i])
			{
				this.customerLifeAchievements[i] = _data.customerLifeAchievementsByIndex[i];
			}
		}
	}

	// Token: 0x060001EB RID: 491 RVA: 0x00012358 File Offset: 0x00010558
	public void LoadCustomerProdWantedNow(SaveData _data)
	{
		if (_data.customerProdWantedNowByIndex == null)
		{
			return;
		}
		if (this.customerProdWantedNow.Count == _data.customerProdWantedNowByIndex.Count)
		{
			this.customerProdWantedNow = _data.customerProdWantedNowByIndex;
			return;
		}
		for (int i = 0; i < _data.customerProdWantedNowByIndex.Count; i++)
		{
			this.customerProdWantedNow[i] = _data.customerProdWantedNowByIndex[i];
		}
	}

	// Token: 0x060001EC RID: 492 RVA: 0x000123C1 File Offset: 0x000105C1
	public bool GetCustomerLifeAchievementState(int _index)
	{
		return this.customerLifeAchievements[_index];
	}

	// Token: 0x060001ED RID: 493 RVA: 0x000123CF File Offset: 0x000105CF
	public void SetCustomerLifeAchievement(int _index, bool _b)
	{
		this.customerLifeAchievements[_index] = _b;
	}

	// Token: 0x060001EE RID: 494 RVA: 0x000123E0 File Offset: 0x000105E0
	public void UnlockAllCustomerLifeAchievement()
	{
		for (int i = 0; i <= this.customerLifeAchievements.Count - 1; i++)
		{
			this.customerLifeAchievements[i] = true;
		}
	}

	// Token: 0x060001EF RID: 495 RVA: 0x00012412 File Offset: 0x00010612
	public void SetCustomerProdWantedNow(int _index, int _value)
	{
		this.customerProdWantedNow[_index] = _value;
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x00012424 File Offset: 0x00010624
	public List<int> CreateProdWantedNowNeed_ByOdd(int _odd)
	{
		List<int> list = new List<int>();
		if (UnityEngine.Random.Range(0, 100) > _odd)
		{
			return list;
		}
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		for (int i = 0; i < this.customer_Prefabs_Available.Count; i++)
		{
			if (this.customer_Prefabs_Available[i])
			{
				list2.Add(i);
			}
		}
		for (int j = 0; j < this.customer_Controllers.Count; j++)
		{
			int id = this.customer_Controllers[j].GetID();
			list3.Add(id);
			if (list2.Contains(id))
			{
				list2.Remove(id);
			}
		}
		int num = list2[UnityEngine.Random.Range(0, list2.Count)];
		List<int> list4 = new List<int>();
		foreach (int num2 in new List<int>(Inv_Manager.instance.unlockedProdsTillThisDay))
		{
			if (Inv_Manager.instance.prod_Prefabs[num2].prodType != Inv_Manager.ProdType.Valentines && Inv_Manager.instance.prod_Prefabs[num2].prodType != Inv_Manager.ProdType.Christmas && Inv_Manager.instance.prod_Prefabs[num2].prodType != Inv_Manager.ProdType.Halloween && Inv_Manager.instance.prod_Prefabs[num2].prodType != Inv_Manager.ProdType.Easter)
			{
				list4.Add(num2);
			}
		}
		int index = UnityEngine.Random.Range(0, list4.Count);
		if (list4.Count > 0)
		{
			this.SetCustomerProdWantedNow(num, list4[index]);
		}
		list.Add(num);
		list.Add(list4[index]);
		return list;
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x000125E8 File Offset: 0x000107E8
	public Char_Controller CreateStoryChar(int _index)
	{
		GameObject gameObject = GameObject.Find("QueueSpawner");
		Char_Controller char_Controller = UnityEngine.Object.Instantiate<Char_Controller>(this.story_Prefabs[_index].gameObject.GetComponent<Char_Controller>());
		char_Controller.transform.position = gameObject.transform.position + Vector3.up;
		char_Controller.transform.rotation = gameObject.transform.rotation;
		char_Controller.transform.localScale = Vector3.zero;
		char_Controller.gameObject.SetActive(true);
		char_Controller.SetID(_index);
		if (!this.story_Controllers.Contains(char_Controller))
		{
			this.story_Controllers.Add(char_Controller);
		}
		this.RefreshLastCustomerCreated(_index);
		return char_Controller;
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x00012697 File Offset: 0x00010897
	public void DestroyStoryChar(Char_Controller _controller)
	{
		if (this.story_Controllers.Contains(_controller))
		{
			this.story_Controllers.Remove(_controller);
		}
		UnityEngine.Object.Destroy(_controller.gameObject);
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x000126C0 File Offset: 0x000108C0
	public void DestroyAllStoryChars()
	{
		for (int i = 0; i < this.story_Controllers.Count; i++)
		{
			if (this.story_Controllers[i])
			{
				UnityEngine.Object.Destroy(this.story_Controllers[i].gameObject);
			}
		}
		this.story_Controllers.Clear();
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x00012717 File Offset: 0x00010917
	public int GetMaxStaffQnt()
	{
		return this.staff_MaxStaffQnt;
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x0001271F File Offset: 0x0001091F
	public void Set_JobAvailable()
	{
		this.Create_PossibleStaff();
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x00012728 File Offset: 0x00010928
	public void Create_PossibleStaff()
	{
		this.staff_Possible_Staff_Data.Clear();
		for (int i = 0; i < 3; i++)
		{
			Staff_Data staff_Data = new Staff_Data();
			staff_Data.Randomize();
			this.staff_Possible_Staff_Data.Add(staff_Data);
		}
		Mail_Manager.instance.Delete_Staff_Mails();
		PC_Manager.instance.RefreshStaffTab();
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x00012778 File Offset: 0x00010978
	public void HireStaff(Staff_Data _staff = null)
	{
		if (this.staff_Data.Count >= this.GetMaxStaffQnt())
		{
			return;
		}
		if (_staff == null)
		{
			_staff = new Staff_Data();
			_staff.Randomize();
		}
		this.staff_Data.Add(_staff);
		PC_Manager.instance.RefreshStaffTab();
		Mail_Manager.instance.Delete_Staff_Mails();
		PC_Manager.instance.Mail_Refresh();
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x000127D3 File Offset: 0x000109D3
	public void FireStaff(int _index)
	{
		this.staff_Data.RemoveAt(_index);
		PC_Manager.instance.RefreshStaffTab();
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x000127EC File Offset: 0x000109EC
	public void DecreaseStaffEnergy()
	{
		foreach (Staff_Data staff_Data in this.staff_Data)
		{
			staff_Data.energy = Mathf.Clamp(staff_Data.energy - UnityEngine.Random.Range(0.05f, 0.15f), 0.1f, 1f);
		}
	}

	// Token: 0x060001FA RID: 506 RVA: 0x00012864 File Offset: 0x00010A64
	public void SetStaffTasks(int _staffIndex, int _taskIndex)
	{
		this.staff_Data[_staffIndex].tasks[_taskIndex] = !this.staff_Data[_staffIndex].tasks[_taskIndex];
		PC_Manager.instance.RefreshStaffTab();
	}

	// Token: 0x060001FB RID: 507 RVA: 0x0001289C File Offset: 0x00010A9C
	public Char_Controller CreateStaffChar(int _index)
	{
		Debug.Log("Creating Staff Char");
		GameObject navExitPointRandom = Nav_Manager.instance.GetNavExitPointRandom();
		Char_Controller char_Controller = UnityEngine.Object.Instantiate<Char_Controller>(this.staff_Prefab);
		char_Controller.transform.position = navExitPointRandom.transform.position + Vector3.up;
		char_Controller.transform.rotation = navExitPointRandom.transform.rotation;
		char_Controller.transform.localScale = Vector3.zero;
		char_Controller.gameObject.SetActive(true);
		char_Controller.SetID(_index);
		if (!this.staff_Controllers.Contains(char_Controller))
		{
			this.staff_Controllers.Add(char_Controller);
		}
		char_Controller.gameObject.GetComponent<Staff_Controller>().staffData = this.staff_Data[_index];
		return char_Controller;
	}

	// Token: 0x060001FC RID: 508 RVA: 0x0001295A File Offset: 0x00010B5A
	public void DestroyStaffChar(Char_Controller _controller)
	{
		if (this.staff_Controllers.Contains(_controller))
		{
			this.staff_Controllers.IndexOf(_controller);
			this.staff_Controllers.Remove(_controller);
		}
		UnityEngine.Object.Destroy(_controller.gameObject);
	}

	// Token: 0x060001FD RID: 509 RVA: 0x00012990 File Offset: 0x00010B90
	public void DestroyAllStaffChars()
	{
		for (int i = 0; i < this.staff_Controllers.Count; i++)
		{
			if (this.staff_Controllers[i])
			{
				if (this.staff_Controllers[i].staff_Controller.boxController)
				{
					this.staff_Controllers[i].staff_Controller.boxController.ThrowBox();
				}
				UnityEngine.Object.Destroy(this.staff_Controllers[i].gameObject);
			}
		}
		this.staff_Controllers.Clear();
	}

	// Token: 0x060001FE RID: 510 RVA: 0x00012A20 File Offset: 0x00010C20
	public void LoadStaff(SaveData _data)
	{
		if (_data.staff_Data != null)
		{
			this.staff_Data = new List<Staff_Data>(_data.staff_Data);
			foreach (Staff_SaveData staff_SaveData in _data.staff_SaveData)
			{
				this.CreateStaffChar(staff_SaveData.id).transform.position = staff_SaveData.position;
			}
		}
		if (_data.staff_Possible_Data != null)
		{
			this.staff_Possible_Staff_Data = new List<Staff_Data>(_data.staff_Possible_Data);
		}
	}

	// Token: 0x060001FF RID: 511 RVA: 0x00012AC0 File Offset: 0x00010CC0
	public bool Get_StaffInStore(Staff_Data _staff_data)
	{
		bool result = false;
		using (List<Char_Controller>.Enumerator enumerator = this.staff_Controllers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.staff_Controller.staffData == _staff_data)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06000200 RID: 512 RVA: 0x00012B20 File Offset: 0x00010D20
	public Char_Controller WhoIsGoingToCashier(Cashier_Controller _cashier)
	{
		foreach (Char_Controller char_Controller in this.staff_Controllers)
		{
			Staff_Controller component = char_Controller.gameObject.GetComponent<Staff_Controller>();
			if (component.task != null && component.task.GetTargetCashier() == _cashier)
			{
				return char_Controller;
			}
		}
		return null;
	}

	// Token: 0x06000201 RID: 513 RVA: 0x00012B9C File Offset: 0x00010D9C
	public void SpawnEmployees()
	{
		this.DestroyAllStaffChars();
		for (int i = 0; i < this.staff_Data.Count; i++)
		{
			if (this.staff_Data[i].workingDays[World_Manager.instance.GetDayIndex()])
			{
				if (this.staff_Data[i].daysOff <= 0)
				{
					this.CreateStaffChar(i);
					Finances_Manager.instance.AddMoney((float)(-(float)this.staff_Data[i].price));
					Finances_Manager.instance.AddTo_OutStaff((float)this.staff_Data[i].price);
				}
				else
				{
					this.staff_Data[i].daysOff--;
					this.staff_Data[i].energy = 1f;
				}
			}
		}
	}

	// Token: 0x06000202 RID: 514 RVA: 0x00012C74 File Offset: 0x00010E74
	public List<Prod_Controller> GetAllCustomersInterests()
	{
		List<Prod_Controller> list = new List<Prod_Controller>();
		foreach (Char_Controller char_Controller in this.customer_Controllers)
		{
			Customer_Controller customer_Controller = char_Controller.customer_Controller;
			if (customer_Controller != null && customer_Controller.prod_Interest != null)
			{
				list.Add(customer_Controller.prod_Interest);
			}
		}
		return list;
	}

	// Token: 0x06000203 RID: 515 RVA: 0x00012CF0 File Offset: 0x00010EF0
	public void SetStaffDaysOff(int _index)
	{
		int num = this.staff_DefaultDaysOffQnt;
		if (this.Get_StaffInStore(this.staff_Data[_index]))
		{
			num++;
		}
		this.staff_Data[_index].daysOff = num;
		PC_Manager.instance.RefreshStaffTab();
	}

	// Token: 0x06000204 RID: 516 RVA: 0x00012D38 File Offset: 0x00010F38
	public void AddStoryCharToQueue(int _index)
	{
		this.story_QueueIndexes.Add(_index);
	}

	// Token: 0x06000205 RID: 517 RVA: 0x00012D46 File Offset: 0x00010F46
	public void ResetStoryCharQueue()
	{
		this.story_QueueIndexes.Clear();
	}

	// Token: 0x06000206 RID: 518 RVA: 0x00012D54 File Offset: 0x00010F54
	public Char_Controller CreateStoryCharFromQueue()
	{
		Char_Controller result = null;
		if (this.story_QueueIndexes.Count > 0)
		{
			result = this.CreateStoryChar(this.story_QueueIndexes[0]);
			this.story_QueueIndexes.RemoveAt(0);
			return result;
		}
		return result;
	}

	// Token: 0x06000207 RID: 519 RVA: 0x00012D93 File Offset: 0x00010F93
	public void ResetAllQueue()
	{
		this.ResetStoryCharQueue();
	}

	// Token: 0x06000208 RID: 520 RVA: 0x00012D9B File Offset: 0x00010F9B
	public void InvokeSpawnFromQueue()
	{
		base.CancelInvoke("SetSpawnFromQueue");
		base.Invoke("SetSpawnFromQueue", (float)UnityEngine.Random.Range(5, 10));
	}

	// Token: 0x06000209 RID: 521 RVA: 0x00012DBC File Offset: 0x00010FBC
	public void SetSpawnFromQueue()
	{
		this.DestroyAllStoryChars();
		if (this.CreateStoryCharFromQueue())
		{
			Game_Manager.instance.SetCinematicMode(true);
		}
	}

	// Token: 0x0600020A RID: 522 RVA: 0x00012DDC File Offset: 0x00010FDC
	public int Get_ContinueBuyingOdd(int _emotion_index)
	{
		return this.continueBuyingOdd[_emotion_index];
	}

	// Token: 0x040002C2 RID: 706
	public static Char_Manager instance;

	// Token: 0x040002C3 RID: 707
	[Header("Emotions")]
	[SerializeField]
	public GameObject emoji_Prefab;

	// Token: 0x040002C4 RID: 708
	[SerializeField]
	public List<Sprite> emojiEmotion_Sprites = new List<Sprite>();

	// Token: 0x040002C5 RID: 709
	[SerializeField]
	public List<Material> faceEmotion_Sprites = new List<Material>();

	// Token: 0x040002C6 RID: 710
	public List<string> emotion_Names = new List<string>
	{
		"Horrible",
		"Bad",
		"Normal",
		"Nice",
		"Awesome"
	};

	// Token: 0x040002C7 RID: 711
	private int[] continueBuyingOdd = new int[]
	{
		40,
		60,
		80,
		90,
		100
	};

	// Token: 0x040002C8 RID: 712
	[Header("ProdWanted")]
	[SerializeField]
	public GameObject prodWanted_Prefab;

	// Token: 0x040002C9 RID: 713
	[Header("Customer Prod Predilection Sprites")]
	[SerializeField]
	public Sprite prod_UnknownSprite;

	// Token: 0x040002CA RID: 714
	public List<LocalCustomer_Data> localCustomer_Datas = new List<LocalCustomer_Data>();

	// Token: 0x040002CB RID: 715
	[Header("Customers")]
	[SerializeField]
	public float createCustomerTime_Timer = 10f;

	// Token: 0x040002CC RID: 716
	[SerializeField]
	public List<Char_Controller> customer_Prefabs_Available = new List<Char_Controller>();

	// Token: 0x040002CD RID: 717
	[SerializeField]
	private float createCustomerTime_Min = 2f;

	// Token: 0x040002CE RID: 718
	[SerializeField]
	private List<int> lastCustomerCreated = new List<int>();

	// Token: 0x040002CF RID: 719
	[SerializeField]
	private List<bool> customerLifeAchievements = new List<bool>();

	// Token: 0x040002D0 RID: 720
	[SerializeField]
	private List<int> customerProdWantedNow = new List<int>();

	// Token: 0x040002D1 RID: 721
	public List<Char_Controller> customer_Controllers = new List<Char_Controller>();

	// Token: 0x040002D3 RID: 723
	private List<float> affectionIndexes_Normal = new List<float>
	{
		-5f,
		-1f,
		2f,
		5f
	};

	// Token: 0x040002D4 RID: 724
	private List<float> affectionIndexes_Friend = new List<float>
	{
		-2.5f,
		-0.5f,
		4f,
		10f
	};

	// Token: 0x040002D5 RID: 725
	[Header("Generic Customers")]
	[SerializeField]
	public List<Material> genCustomer_Material_Clothes = new List<Material>();

	// Token: 0x040002D6 RID: 726
	[SerializeField]
	public List<Material> genCustomer_Material_HairColors = new List<Material>();

	// Token: 0x040002D7 RID: 727
	[SerializeField]
	public List<Mesh> genCustomer_Mesh_Hairs = new List<Mesh>();

	// Token: 0x040002D8 RID: 728
	[SerializeField]
	private Char_Controller genCustomer_Prefab;

	// Token: 0x040002D9 RID: 729
	[Header("Story Chars")]
	[SerializeField]
	public List<Char_Controller> story_Prefabs = new List<Char_Controller>();

	// Token: 0x040002DA RID: 730
	[SerializeField]
	private List<Char_Controller> story_Controllers = new List<Char_Controller>();

	// Token: 0x040002DB RID: 731
	[SerializeField]
	private List<int> story_QueueIndexes = new List<int>();

	// Token: 0x040002DC RID: 732
	[Header("Staff Chars")]
	public string[] staff_names = new string[]
	{
		"James",
		"Michael",
		"John",
		"Robert",
		"William",
		"Richard",
		"Thomas",
		"Chris",
		"Daniel",
		"Mark",
		"Jason",
		"Floyd",
		"Matt",
		"Rick",
		"Steve",
		"Brian",
		"Tim",
		"Eddy",
		"Mary",
		"Patricia",
		"Linda",
		"Beth",
		"Sarah",
		"Nancy",
		"Ash",
		"Sandra",
		"Emily",
		"Kim",
		"Karen",
		"Susan",
		"Jess",
		"Barbara",
		"Sharon"
	};

	// Token: 0x040002DD RID: 733
	[SerializeField]
	public List<Char_Controller> staff_Controllers = new List<Char_Controller>();

	// Token: 0x040002DE RID: 734
	[SerializeField]
	public List<Staff_Data> staff_Data = new List<Staff_Data>();

	// Token: 0x040002DF RID: 735
	[SerializeField]
	private Char_Controller staff_Prefab;

	// Token: 0x040002E0 RID: 736
	private int staff_MaxStaffQnt = 3;

	// Token: 0x040002E1 RID: 737
	private int staff_DefaultDaysOffQnt = 1;

	// Token: 0x040002E2 RID: 738
	[SerializeField]
	public List<Staff_Data> staff_Possible_Staff_Data = new List<Staff_Data>();
}
