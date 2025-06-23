using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004B RID: 75
public class Missions_Manager : MonoBehaviour
{
	// Token: 0x060003EC RID: 1004 RVA: 0x00024E95 File Offset: 0x00023095
	private void Awake()
	{
		if (!Missions_Manager.instance)
		{
			Missions_Manager.instance = this;
		}
		this.CreateReferences_Tasks();
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x00024EAF File Offset: 0x000230AF
	private void Update()
	{
		this.Update_Tasks();
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x00024EB7 File Offset: 0x000230B7
	public int GetTotalAwardsNumber()
	{
		return 0;
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x00024EBA File Offset: 0x000230BA
	public int GetTotalAwardsWon()
	{
		return 0;
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x00024EC0 File Offset: 0x000230C0
	public void CreateReferences_Tasks()
	{
		this.task_Datas.Clear();
		for (int i = 0; i < 2; i++)
		{
			this.task_Datas.Add(new List<TaskData>());
		}
		Task_Controller[] array = Resources.LoadAll<Task_Controller>("Tasks/OldMan");
		for (int j = 0; j < array.Length; j++)
		{
			for (int k = 0; k < array.Length; k++)
			{
				if (int.Parse(array[k].name.Split(new char[]
				{
					char.Parse("_")
				})[0]) == j)
				{
					TaskData taskData = array[k].taskData;
					taskData.Reset_Task();
					taskData.index = j;
					this.task_Datas[0].Add(taskData);
					this.t_OldMan.Add(taskData);
					break;
				}
			}
		}
		Task_Controller task_Controller = Resources.Load<Task_Controller>("Tasks/Taiga/0_Task_Taiga 0");
		this.t_Taiga = task_Controller.taskData;
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x00024FA0 File Offset: 0x000231A0
	public void Set_NewGame()
	{
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x00024FA4 File Offset: 0x000231A4
	public void Load_Tasks(SaveData _data)
	{
		if (_data.taskSaveData_SD == null)
		{
			return;
		}
		this.Set_Create_Task_Taiga();
		for (int i = 0; i < _data.taskSaveData_SD.Count; i++)
		{
			for (int j = 0; j < _data.taskSaveData_SD[i].Count; j++)
			{
				TaskData taskData = this.task_Datas[i][j];
				TaskSaveData taskSaveData = _data.taskSaveData_SD[i][j];
				taskData.state = taskSaveData.state;
				taskData.daysCurrent = taskSaveData.daysCurrent;
				taskData.needsCurrent = taskSaveData.needsCurrent;
				taskData.needsQnt = taskSaveData.needsQnt;
				List<Prod_Controller> list = new List<Prod_Controller>();
				for (int k = 0; k < taskSaveData.prod_Indexes.Count; k++)
				{
					list.Add(Inv_Manager.instance.prod_Prefabs[taskSaveData.prod_Indexes[k]]);
				}
				taskData.task_sell_Prods = list;
				taskData.tags = taskSaveData.tags;
				taskData.values = taskSaveData.values;
			}
		}
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x000250BC File Offset: 0x000232BC
	public List<TaskData> Get_All_TaskDatas_Current()
	{
		List<TaskData> list = new List<TaskData>();
		for (int i = 0; i < this.task_Datas.Count; i++)
		{
			for (int j = 0; j < this.task_Datas[i].Count; j++)
			{
				if (this.task_Datas[i][j] != null && this.task_Datas[i][j].state != 0 && this.task_Datas[i][j].state != 4)
				{
					list.Add(this.task_Datas[i][j]);
				}
			}
		}
		return list;
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x00025164 File Offset: 0x00023364
	public void Update_Tasks()
	{
		if (Game_Manager.instance.firsDay)
		{
			return;
		}
		if (!Game_Manager.instance.MayRun())
		{
			return;
		}
		Game_Manager.instance.GetMartOpen();
		bool flag = true;
		for (int i = 0; i < this.t_OldMan.Count; i++)
		{
			if (this.t_OldMan[i].state == 1)
			{
				this.t_OldMan[i].CheckNeeds();
				flag = false;
			}
			else if (this.t_OldMan[i].state == 2)
			{
				this.t_OldMan[i].Finish_Task(true);
			}
			if (this.t_OldMan[i].state == 2)
			{
				flag = false;
			}
		}
		if (flag && this.mayCreateTask)
		{
			this.Set_Unlock_AnyTask_OldMan();
		}
		bool flag2 = true;
		for (int j = 0; j < this.task_Datas[1].Count; j++)
		{
			if (this.task_Datas[1][j].state == 1)
			{
				this.task_Datas[1][j].CheckNeeds();
				flag2 = false;
			}
			else if (this.task_Datas[1][j].state == 2)
			{
				this.task_Datas[1][j].Finish_Task(true);
			}
			if (this.task_Datas[1][j].state == 2)
			{
				flag2 = false;
			}
		}
		if (flag2 && this.mayCreateTask)
		{
			this.mayCreateTask = false;
			this.Set_Create_Task_Taiga();
		}
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x000252E4 File Offset: 0x000234E4
	private void Set_Unlock_AnyTask_OldMan()
	{
		for (int i = 1; i < this.t_OldMan.Count; i++)
		{
			if (this.t_OldMan[i].state == 0)
			{
				this.t_OldMan[i].Start_Task();
				return;
			}
		}
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x0002532C File Offset: 0x0002352C
	private void Set_Create_Task_Taiga()
	{
		if (this.task_Datas[1].Count >= 1)
		{
			this.task_Datas[1].Clear();
		}
		TaskData taskData = new TaskData();
		taskData.Clone(this.t_Taiga);
		taskData.state = 1;
		taskData.task_sell_Prods.Clear();
		List<Prod_Controller> list = new List<Prod_Controller>(Inv_Manager.instance.prod_Prefabs);
		List<Prod_Controller> list2 = new List<Prod_Controller>();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].buyable && Unlock_Manager.instance.item_Unlocked[0, i] && !Inv_Manager.instance.GetIfProdIsEventType(list[i]))
			{
				list2.Add(list[i]);
			}
		}
		Prod_Controller prod_Controller = list2[UnityEngine.Random.Range(0, list2.Count)];
		taskData.task_sell_Prods.Add(prod_Controller);
		taskData.needsQnt = UnityEngine.Random.Range(2, 11);
		taskData.needsCurrent = 0;
		taskData.text = "_gen_task_sell_00";
		string[] tags = new string[]
		{
			"[item]"
		};
		string[] values = new string[]
		{
			Inv_Manager.instance.GetProdName(prod_Controller, false)
		};
		taskData.tags = tags;
		taskData.values = values;
		this.task_Datas[1].Add(taskData);
		this.debugTaiga = taskData;
		Debug.Log("Created Taiga task");
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x0002548C File Offset: 0x0002368C
	private void Check_Product_Sold(Prod_Controller _prod)
	{
		Inv_Manager.instance.GetItemIndex(_prod.gameObject);
		for (int i = 0; i < this.task_Datas.Count; i++)
		{
			for (int j = 0; j < this.task_Datas[i].Count; j++)
			{
				int state = this.task_Datas[i][j].state;
			}
		}
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x000254F8 File Offset: 0x000236F8
	public void Set_DidAction(Missions_Manager.ActionType _action, int _prod_index = -1)
	{
		Debug.LogWarning("Did Action!");
		for (int i = 0; i < this.t_OldMan.Count; i++)
		{
			if (this.t_OldMan[i].state == 1 && this.t_OldMan[i].actionType == _action)
			{
				this.t_OldMan[i].Set_Increase_Needs();
			}
		}
		if (_action == Missions_Manager.ActionType.sell_Prod && _prod_index != -1)
		{
			for (int j = 0; j < this.task_Datas[1].Count; j++)
			{
				TaskData taskData = this.task_Datas[1][j];
				if (taskData != null && taskData.state == 1 && Inv_Manager.instance.GetItemIndex(taskData.task_sell_Prods[0].gameObject) == _prod_index)
				{
					taskData.needsCurrent++;
				}
			}
		}
		Menu_Manager.instance.Tasks_Refresh_Description();
	}

	// Token: 0x04000481 RID: 1153
	public static Missions_Manager instance;

	// Token: 0x04000482 RID: 1154
	public bool mayCreateTask;

	// Token: 0x04000483 RID: 1155
	public List<List<TaskData>> task_Datas = new List<List<TaskData>>();

	// Token: 0x04000484 RID: 1156
	public List<TaskData> t_OldMan = new List<TaskData>();

	// Token: 0x04000485 RID: 1157
	public TaskData t_Taiga;

	// Token: 0x04000486 RID: 1158
	public TaskData debugTaiga;

	// Token: 0x02000085 RID: 133
	public enum ActionType
	{
		// Token: 0x040006D2 RID: 1746
		none,
		// Token: 0x040006D3 RID: 1747
		clean_Dirt,
		// Token: 0x040006D4 RID: 1748
		open_Store,
		// Token: 0x040006D5 RID: 1749
		sell_Prod,
		// Token: 0x040006D6 RID: 1750
		buy_Prod,
		// Token: 0x040006D7 RID: 1751
		buy_shelf,
		// Token: 0x040006D8 RID: 1752
		buy_decor,
		// Token: 0x040006D9 RID: 1753
		buy_wall,
		// Token: 0x040006DA RID: 1754
		buy_floor,
		// Token: 0x040006DB RID: 1755
		buy_util,
		// Token: 0x040006DC RID: 1756
		finish_Customer,
		// Token: 0x040006DD RID: 1757
		work_Cashier,
		// Token: 0x040006DE RID: 1758
		store_Boxes_Inventory,
		// Token: 0x040006DF RID: 1759
		place_Prods,
		// Token: 0x040006E0 RID: 1760
		place_shelf,
		// Token: 0x040006E1 RID: 1761
		place_decor,
		// Token: 0x040006E2 RID: 1762
		place_util,
		// Token: 0x040006E3 RID: 1763
		place_wall,
		// Token: 0x040006E4 RID: 1764
		place_floor,
		// Token: 0x040006E5 RID: 1765
		trash_items,
		// Token: 0x040006E6 RID: 1766
		fixed_machine
	}
}
