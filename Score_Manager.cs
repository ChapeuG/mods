using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class Score_Manager : MonoBehaviour
{
	// Token: 0x06000483 RID: 1155 RVA: 0x0002E64E File Offset: 0x0002C84E
	private void Awake()
	{
		if (!Score_Manager.instance)
		{
			Score_Manager.instance = this;
		}
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x0002E662 File Offset: 0x0002C862
	private void Start()
	{
		this.CreateReferences_TaigaAwards_AwardsWonQnt();
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000485 RID: 1157 RVA: 0x0002E66A File Offset: 0x0002C86A
	// (set) Token: 0x06000486 RID: 1158 RVA: 0x0002E672 File Offset: 0x0002C872
	public float taigaScore { get; private set; }

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000487 RID: 1159 RVA: 0x0002E67B File Offset: 0x0002C87B
	// (set) Token: 0x06000488 RID: 1160 RVA: 0x0002E683 File Offset: 0x0002C883
	public List<int> taigaMembershipLevels { get; private set; } = new List<int>
	{
		0,
		30,
		80
	};

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000489 RID: 1161 RVA: 0x0002E68C File Offset: 0x0002C88C
	// (set) Token: 0x0600048A RID: 1162 RVA: 0x0002E694 File Offset: 0x0002C894
	public int taigaMembershipLevelIndex { get; private set; }

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600048B RID: 1163 RVA: 0x0002E69D File Offset: 0x0002C89D
	public float[] ratePossibilities { get; } = new float[]
	{
		-5f,
		-2f,
		0f,
		0.5f,
		1f
	};

	// Token: 0x0600048C RID: 1164 RVA: 0x0002E6A5 File Offset: 0x0002C8A5
	public void Load_TaigaScore(float _score)
	{
		this.taigaScore = _score;
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x0002E6AE File Offset: 0x0002C8AE
	public void ReceiveTaigaRateIndex(int _index)
	{
		_index = Mathf.Clamp(_index, 0, this.ratePossibilities.Length - 1);
		this.IncreaseTaigaScore(this.ratePossibilities[_index]);
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x0002E6D1 File Offset: 0x0002C8D1
	public void IncreaseTaigaScore(float _value)
	{
		this.taigaScore = Mathf.Clamp(this.taigaScore + _value, 0f, this.scoreMax);
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x0002E6F4 File Offset: 0x0002C8F4
	public int RefreshTaigaMembershipIndex()
	{
		for (int i = 0; i < this.taigaMembershipLevels.Count; i++)
		{
			if (this.taigaScore >= (float)this.taigaMembershipLevels[i])
			{
				this.taigaMembershipLevelIndex = i;
			}
		}
		return this.taigaMembershipLevelIndex;
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x0002E73C File Offset: 0x0002C93C
	public void GiveTaigaRewards()
	{
		for (int i = 0; i < this.taigaAwards_Won.Count; i++)
		{
			if (this.taigaAwards_Won[i])
			{
				Inv_Manager.instance.AddDelivery(Inv_Manager.instance.GetItemIndex(this.taigaAwards_Rewards[i].reward.gameObject), 2, this.taigaAwards_Rewards[i].qnt, true, 4);
			}
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000491 RID: 1169 RVA: 0x0002E7AC File Offset: 0x0002C9AC
	// (set) Token: 0x06000492 RID: 1170 RVA: 0x0002E7B4 File Offset: 0x0002C9B4
	public List<bool> taigaAwards_Won { get; private set; } = new List<bool>();

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000493 RID: 1171 RVA: 0x0002E7BD File Offset: 0x0002C9BD
	// (set) Token: 0x06000494 RID: 1172 RVA: 0x0002E7C5 File Offset: 0x0002C9C5
	public List<string> taigaAwards_WinnerNames { get; private set; } = new List<string>();

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000495 RID: 1173 RVA: 0x0002E7CE File Offset: 0x0002C9CE
	public List<string> taigaAwards_CompetitorsNames { get; } = new List<string>
	{
		"Floormart",
		"Seven-Twelve",
		"Intershop",
		"Toscor Mart",
		"Walt's Club",
		"Priceco",
		"Foodway"
	};

	// Token: 0x06000496 RID: 1174 RVA: 0x0002E7D8 File Offset: 0x0002C9D8
	public void ComputeTaigaAwards()
	{
		this.taigaAwards_Won.Clear();
		this.taigaAwards_WinnerNames.Clear();
		if (this.taigaScore >= 98f)
		{
			this.AddTaigaAwardsWinner(true, Game_Manager.instance.GetMartName());
			List<int> taigaAwards_AwardsWonQnt = this.taigaAwards_AwardsWonQnt;
			int num = taigaAwards_AwardsWonQnt[0];
			taigaAwards_AwardsWonQnt[0] = num + 1;
			return;
		}
		this.AddTaigaAwardsWinner(false, this.GetRandomCompetitorName());
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x0002E83E File Offset: 0x0002CA3E
	public void AddTaigaAwardsWinner(bool _won, string _winnerName)
	{
		this.taigaAwards_Won.Add(_won);
		this.taigaAwards_WinnerNames.Add(_winnerName);
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x0002E858 File Offset: 0x0002CA58
	public string GetRandomCompetitorName()
	{
		int index = UnityEngine.Random.Range(0, this.taigaAwards_CompetitorsNames.Count - 1);
		return this.taigaAwards_CompetitorsNames[index];
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000499 RID: 1177 RVA: 0x0002E885 File Offset: 0x0002CA85
	// (set) Token: 0x0600049A RID: 1178 RVA: 0x0002E88D File Offset: 0x0002CA8D
	public List<int> taigaAwards_AwardsWonQnt { get; private set; } = new List<int>
	{
		0
	};

	// Token: 0x0600049B RID: 1179 RVA: 0x0002E898 File Offset: 0x0002CA98
	private void CreateReferences_TaigaAwards_AwardsWonQnt()
	{
		for (int i = 0; i < this.taigaAwards_Rewards.Count; i++)
		{
			this.taigaAwards_AwardsWonQnt.Add(0);
		}
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x0002E8C7 File Offset: 0x0002CAC7
	public void Load_TaigaAwards_AwardsWonQnt(List<int> _list)
	{
		if (_list == null)
		{
			return;
		}
		this.taigaAwards_AwardsWonQnt = new List<int>(_list);
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x0002E8D9 File Offset: 0x0002CAD9
	public void NextDay()
	{
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x0002E8DB File Offset: 0x0002CADB
	public void NextSeason()
	{
	}

	// Token: 0x040005B3 RID: 1459
	public static Score_Manager instance;

	// Token: 0x040005B4 RID: 1460
	private readonly float scoreMax = 100f;

	// Token: 0x040005B8 RID: 1464
	public int rateDefaultIndex = 2;

	// Token: 0x040005BA RID: 1466
	[Header("Taiga Awards")]
	[SerializeField]
	public List<TaigaAwards_Rewards> taigaAwards_Rewards = new List<TaigaAwards_Rewards>();
}
