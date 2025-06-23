using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004E RID: 78
[Serializable]
public class TaskData
{
	// Token: 0x060003FB RID: 1019 RVA: 0x00025600 File Offset: 0x00023800
	public void Clone(TaskData _data)
	{
		this.owner_index = _data.owner_index;
		this.owner_cat = _data.owner_cat;
		this.state = _data.state;
		this.index = _data.index;
		this.category = _data.category;
		this.procedural = _data.procedural;
		this.tags = _data.tags;
		this.values = _data.values;
		this.daysMax = _data.daysMax;
		this.daysCurrent = _data.daysCurrent;
		this.needsQnt = _data.needsQnt;
		this.needsCurrent = _data.needsCurrent;
		this.sprite = _data.sprite;
		this.title = _data.title;
		this.text = _data.text;
		this.rewards_Random = _data.rewards_Random;
		this.rewards_Random_Max = _data.rewards_Random_Max;
		this.rewards_Unlock = _data.rewards_Unlock;
		this.unlock_Tasks = _data.unlock_Tasks;
		this.task_sell_Prods = _data.task_sell_Prods;
		this.actionType = _data.actionType;
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x00025709 File Offset: 0x00023909
	public void Reset_Task()
	{
		this.UI = null;
		this.state = 0;
		this.index = 0;
		this.daysCurrent = 0;
		this.needsCurrent = 0;
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x0002572E File Offset: 0x0002392E
	public void Set_DaysCurrent(int _days)
	{
		this.daysCurrent = _days;
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x00025737 File Offset: 0x00023937
	public void Decrease_DaysCurrent()
	{
		this.daysCurrent--;
		if (this.daysCurrent <= 0)
		{
			this.Finish_Task(false);
		}
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x00025757 File Offset: 0x00023957
	public void Set_Increase_Needs()
	{
		this.needsCurrent++;
		this.CheckNeeds();
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x0002576D File Offset: 0x0002396D
	public void Start_Task()
	{
		if (!Missions_Manager.instance.mayCreateTask && !this.forceCreation)
		{
			return;
		}
		Missions_Manager.instance.mayCreateTask = false;
		this.state = 1;
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x00025798 File Offset: 0x00023998
	public void Finish_Task(bool _inTime)
	{
		if (_inTime)
		{
			this.state = 3;
		}
		Menu_Manager.instance.Tasks_Refresh_Panels();
		this.state = 4;
		for (int i = 0; i < this.unlock_Tasks.Count; i++)
		{
			if (this.unlock_Tasks[i])
			{
				int num = int.Parse(this.unlock_Tasks[i].name.Split(new char[]
				{
					char.Parse("_")
				})[0]);
				Missions_Manager.instance.task_Datas[this.category][num].Start_Task();
			}
		}
		if (this.category == 0)
		{
			this.Create_Rewards();
		}
		else if (this.category == 1)
		{
			Score_Manager.instance.IncreaseTaigaScore(1f);
			this.Create_Rewards();
		}
		if (this.category == 1)
		{
			Missions_Manager.instance.task_Datas[1].Clear();
		}
		Menu_Manager.instance.Tasks_Set_State(true);
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00025894 File Offset: 0x00023A94
	public void Create_Rewards()
	{
		int qnt = this.rewards_Random;
		if (this.rewards_Random_Max > this.rewards_Random)
		{
			qnt = UnityEngine.Random.Range(this.rewards_Random, this.rewards_Random_Max + 1);
		}
		List<List<int>> list = Unlock_Manager.instance.Set_UnlockRandomItem(qnt, false, 1);
		if (list == null)
		{
			return;
		}
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		foreach (GameObject gameObject in this.rewards_Unlock)
		{
			int num = -1;
			int itemIndex = Inv_Manager.instance.GetItemIndex(gameObject);
			Prod_Controller prod_Controller;
			Shelf_Controller shelf_Controller;
			if (gameObject.TryGetComponent<Prod_Controller>(out prod_Controller))
			{
				num = 0;
			}
			else if (gameObject.TryGetComponent<Shelf_Controller>(out shelf_Controller))
			{
				num = 1;
			}
			if (num != -1)
			{
				list2.Add(num);
				list3.Add(itemIndex);
			}
		}
		foreach (List<int> list4 in list)
		{
			list2.Add(list4[0]);
			list3.Add(list4[1]);
		}
		Mail_Manager.instance.Send_Mail_Rewards(this.owner_index, this.owner_cat, this.text, list2, list3, this.procedural, this.tags, this.values);
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x000259F4 File Offset: 0x00023BF4
	public void CheckNeeds()
	{
		if (this.needsQnt == 0)
		{
			return;
		}
		if (this.needsCurrent >= this.needsQnt)
		{
			this.state = 2;
		}
	}

	// Token: 0x04000491 RID: 1169
	public GameObject UI;

	// Token: 0x04000492 RID: 1170
	[SerializeField]
	public int owner_index;

	// Token: 0x04000493 RID: 1171
	[SerializeField]
	public int owner_cat;

	// Token: 0x04000494 RID: 1172
	[SerializeField]
	public int state;

	// Token: 0x04000495 RID: 1173
	[SerializeField]
	public int index;

	// Token: 0x04000496 RID: 1174
	[SerializeField]
	public int category;

	// Token: 0x04000497 RID: 1175
	[SerializeField]
	public bool procedural;

	// Token: 0x04000498 RID: 1176
	[SerializeField]
	public string[] tags;

	// Token: 0x04000499 RID: 1177
	[SerializeField]
	public string[] values;

	// Token: 0x0400049A RID: 1178
	public int daysMax;

	// Token: 0x0400049B RID: 1179
	public bool forceCreation;

	// Token: 0x0400049C RID: 1180
	[SerializeField]
	public int daysCurrent;

	// Token: 0x0400049D RID: 1181
	public int needsQnt;

	// Token: 0x0400049E RID: 1182
	[SerializeField]
	public int needsCurrent;

	// Token: 0x0400049F RID: 1183
	public Sprite sprite;

	// Token: 0x040004A0 RID: 1184
	public string title;

	// Token: 0x040004A1 RID: 1185
	public string text;

	// Token: 0x040004A2 RID: 1186
	public int rewards_Random;

	// Token: 0x040004A3 RID: 1187
	public int rewards_Random_Max;

	// Token: 0x040004A4 RID: 1188
	public List<GameObject> rewards_Unlock = new List<GameObject>();

	// Token: 0x040004A5 RID: 1189
	public List<GameObject> rewards_Win = new List<GameObject>();

	// Token: 0x040004A6 RID: 1190
	public List<Task_Controller> unlock_Tasks = new List<Task_Controller>();

	// Token: 0x040004A7 RID: 1191
	public List<Prod_Controller> task_sell_Prods = new List<Prod_Controller>();

	// Token: 0x040004A8 RID: 1192
	[Header("Actions Needed")]
	public Missions_Manager.ActionType actionType;
}
