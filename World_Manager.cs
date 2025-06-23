using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class World_Manager : MonoBehaviour
{
	// Token: 0x060004E5 RID: 1253 RVA: 0x000311A2 File Offset: 0x0002F3A2
	private void Awake()
	{
		if (!World_Manager.instance)
		{
			World_Manager.instance = this;
		}
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x000311B6 File Offset: 0x0002F3B6
	private void Start()
	{
		this.SetTime((float)this.startTimeInSeconds);
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x000311C5 File Offset: 0x0002F3C5
	private void Update()
	{
		this.Update_Time();
		this.Update_Light();
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x000311D3 File Offset: 0x0002F3D3
	public int Get_DeliveryAvailable_State()
	{
		if (this.currentTime <= (float)this.startTime_NoDelivery_InSeconds)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x000311E8 File Offset: 0x0002F3E8
	public void Update_Time()
	{
		if (Game_Manager.instance.MayRun())
		{
			this.playTime += Time.deltaTime;
		}
		if (Game_Manager.instance.MayRun() && Game_Manager.instance.GetGameMode() == 0)
		{
			this.RefreshTimeUI();
			if (this.reachedDayEnding || !Game_Manager.instance.GetMartOpen())
			{
				return;
			}
			this.currentTime -= this.timeScale_Normal * Time.deltaTime;
			if (this.currentTime <= 0f)
			{
				this.currentTime = 0f;
				this.reachedDayEnding = true;
				Game_Manager.instance.CloseMart();
			}
		}
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x00031288 File Offset: 0x0002F488
	public void SetRawPlayTime(float _PlayTime)
	{
		this.playTime = _PlayTime;
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x00031291 File Offset: 0x0002F491
	public float GetRawPlayTime()
	{
		return this.playTime;
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x00031299 File Offset: 0x0002F499
	public float TransformPlayTimeIn_Hours(float _PlayTime)
	{
		return _PlayTime / 60f / 60f;
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x000312A8 File Offset: 0x0002F4A8
	public string GetPlayTimeStringConverted(float _PlayTime)
	{
		if (_PlayTime < 60f)
		{
			return _PlayTime.ToString("F0") + " " + Language_Manager.instance.GetText("seconds").ToLower();
		}
		if (_PlayTime < 3600f)
		{
			return (_PlayTime / 60f).ToString("F0") + " " + Language_Manager.instance.GetText("minutes").ToLower();
		}
		return (_PlayTime / 60f / 60f).ToString("F0") + " " + Language_Manager.instance.GetText("hours").ToLower();
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x0003135B File Offset: 0x0002F55B
	public void SetTime(float _time)
	{
		this.currentTime = _time;
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00031364 File Offset: 0x0002F564
	public float GetTime()
	{
		return this.currentTime;
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x0003136C File Offset: 0x0002F56C
	private void RefreshTimeUI()
	{
		float amount = this.currentTime / (float)this.startTimeInSeconds;
		Menu_Manager.instance.RefreshTime(amount);
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x00031394 File Offset: 0x0002F594
	private void Update_Light()
	{
		float num = (float)this.startTimeInSeconds;
		float b = ((float)this.startTimeInSeconds - this.currentTime) / num;
		this.lightIndex = Mathf.Lerp(this.lightIndex, b, this.lightColor_LerpSpeed_Terrain * Time.deltaTime);
		this.light_Terrain.color = this.ligthColor_Terrain[this.seasonIndex].Evaluate(this.lightIndex);
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x00031401 File Offset: 0x0002F601
	public float GetIllumination()
	{
		return this.illumination;
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00031409 File Offset: 0x0002F609
	public void SetNextWeek()
	{
		if (this.week_Index < this.week_Max)
		{
			Mail_Manager.instance.Send_Mail_Auditor_Weekly();
			this.week_Index++;
			return;
		}
		Mail_Manager.instance.Send_Mail_Auditor_Weekly();
		this.week_Index = 0;
		this.SetNextSeason();
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00031449 File Offset: 0x0002F649
	public void SetWeek(int _index)
	{
		this.week_Index = Mathf.Clamp(_index, 0, this.week_Max);
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00031460 File Offset: 0x0002F660
	public string GetWeekName()
	{
		return (this.week_Index + 1).ToString() + "/" + (this.week_Max + 1).ToString();
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x00031497 File Offset: 0x0002F697
	public int GetWeekIndex()
	{
		return this.week_Index;
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x0003149F File Offset: 0x0002F69F
	public int GetWeekMax()
	{
		return this.week_Max;
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x000314A7 File Offset: 0x0002F6A7
	public void LoadWeek(SaveData _data)
	{
		int world_WeekIndex = _data.world_WeekIndex;
		this.SetWeek(_data.world_WeekIndex);
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x000314BC File Offset: 0x0002F6BC
	public void SetNextDay()
	{
		Score_Manager.instance.NextDay();
		if (this.dayIndex >= 6)
		{
			this.dayIndex = 0;
			this.SetNextWeek();
		}
		else
		{
			this.dayIndex++;
		}
		this.SetDay(this.dayIndex);
		this.day_overall++;
		if (this.day_overall == 1)
		{
			Mail_Manager.instance.Send_Mail_Auditor_AutoQuests();
		}
		Char_Manager.instance.RandomizeCreateCustomerTime(20f);
		Char_Manager.instance.DecreaseStaffEnergy();
		Inv_Manager.instance.UpdateNewspaperDeals();
		Inv_Manager.instance.DecreaseLifeSpan();
		Inv_Manager.instance.RefreshBrokenStates();
		PC_Manager.instance.Refresh_PC_Controller();
		PC_Manager.instance.lastTabSelected = 8;
		Mail_Manager.instance.Send_Mail_Customer_Needs_By_Odd();
		Mail_Manager.instance.Send_Mail_Possible_Staff();
		this.expansion_RemainingDays--;
		if (this.expansion_RemainingDays < 0)
		{
			this.expansion_RemainingDays = 0;
		}
		this.RefreshExpansions();
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x000315AA File Offset: 0x0002F7AA
	public void SetDay(int _index)
	{
		this.dayIndex = _index;
		Menu_Manager.instance.RefreshCalendar();
		this.reachedDayEnding = false;
		this.SetTime((float)this.startTimeInSeconds);
		this.Update_Light();
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x000315D7 File Offset: 0x0002F7D7
	public int GetDayIndex()
	{
		return this.dayIndex;
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x000315DF File Offset: 0x0002F7DF
	public string GetDayName()
	{
		return Language_Manager.instance.GetText(this.dayNames[this.dayIndex]);
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x000315F8 File Offset: 0x0002F7F8
	public string GetDayNameByIndex(int _index)
	{
		return Language_Manager.instance.GetText(this.dayNames[_index]);
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x0003160C File Offset: 0x0002F80C
	public void LoadDay(SaveData _data)
	{
		int world_DayIndex = _data.world_DayIndex;
		this.day_overall = _data.world_DayOverall;
		this.SetDay(_data.world_DayIndex);
		this.LoadForecast(_data);
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x00031634 File Offset: 0x0002F834
	public void CreateLevel(int _index)
	{
		this.DeleteCurrentLevel();
		this.DeleteOtherModesLevels();
		this.currentLevelIndex = _index;
		this.currentLevel = UnityEngine.Object.Instantiate<Level_Controller>(this.levels_GOs[this.currentLevelIndex].GetComponent<Level_Controller>());
		this.currentLevel.transform.position = Vector3.zero;
		this.currentLevel.transform.rotation = Quaternion.Euler(Vector3.zero);
		this.currentLevel.transform.localScale = Vector3.one;
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x000316B5 File Offset: 0x0002F8B5
	public void DeleteCurrentLevel()
	{
		if (!this.currentLevel)
		{
			return;
		}
		UnityEngine.Object.Destroy(this.currentLevel.gameObject);
		this.currentLevel = null;
		this.currentLevelIndex = 0;
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x000316E3 File Offset: 0x0002F8E3
	public Sprite GetLevelSpriteByIndex(int _index)
	{
		return this.levels_GOs[_index].GetComponent<Level_Controller>().levelSprite;
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x000316F7 File Offset: 0x0002F8F7
	public int Get_ExpansionPrice()
	{
		return this.currentLevel.expansion_price;
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x00031704 File Offset: 0x0002F904
	public void SetExpansionIndex(int _index, int _remainingDays)
	{
		this.currentExpansionIndex = _index;
		this.expansion_RemainingDays = _remainingDays;
		this.RefreshExpansions();
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x0003171A File Offset: 0x0002F91A
	public void LoadExpansionIndex(SaveData _data)
	{
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x0003171C File Offset: 0x0002F91C
	public void RefreshExpansions()
	{
		this.currentLevel.RefreshExpansions();
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x00031729 File Offset: 0x0002F929
	public int Get_LevelPrice_ByIndex(int _level_index)
	{
		return this.levels_GOs[_level_index].GetComponent<Level_Controller>().level_price;
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x0003173D File Offset: 0x0002F93D
	public int GetLevelMaxCustomerQnt()
	{
		this.debug_maxCharQnt = this.currentLevel.maxCharQnt[this.currentExpansionIndex];
		return this.currentLevel.maxCharQnt[this.currentExpansionIndex];
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00031774 File Offset: 0x0002F974
	public float GetLevelSpawnTimeMax()
	{
		float result = 40f - (float)this.GetLevelMaxCustomerQnt() * 2.5f;
		this.debug_spawnTimeMax = result;
		return result;
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x000317A0 File Offset: 0x0002F9A0
	public void CreateOtherModesLevels(int _index)
	{
		this.DeleteOtherModesLevels();
		this.curentOtherModesLevelsIndex = _index;
		this.currentOtherModesLevels = UnityEngine.Object.Instantiate<GameObject>(this.miniGameLevels_GOs[_index]);
		this.currentOtherModesLevels.transform.position = new Vector3(0f, -200f, 0f);
		this.currentOtherModesLevels.transform.rotation = Quaternion.Euler(Vector3.zero);
		this.currentOtherModesLevels.transform.localScale = Vector3.one;
		this.light_Parent.SetActive(false);
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x0003182C File Offset: 0x0002FA2C
	public void DeleteOtherModesLevels()
	{
		if (!this.currentOtherModesLevels)
		{
			return;
		}
		UnityEngine.Object.Destroy(this.currentOtherModesLevels.gameObject);
		this.currentOtherModesLevels = null;
		this.curentOtherModesLevelsIndex = 0;
		Game_Manager.instance.SetModeStage(0);
		this.light_Parent.SetActive(true);
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x0003187C File Offset: 0x0002FA7C
	public void SetNextSeason()
	{
		this.seasonIndex++;
		if (this.seasonIndex >= 4)
		{
			this.seasonIndex = 0;
			this.activateTaigaAwards = true;
		}
		Score_Manager.instance.NextSeason();
		this.RefreshSeason();
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x000318B3 File Offset: 0x0002FAB3
	public void SetNewSeason(int _index)
	{
		this.seasonIndex = _index;
		this.RefreshSeason();
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x000318C2 File Offset: 0x0002FAC2
	public void RefreshSeason()
	{
		this.currentLevel.SetNewSeason(this.seasonIndex, this.seasonTextures[this.seasonIndex], this.grassColor[this.seasonIndex]);
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x000318F3 File Offset: 0x0002FAF3
	public string GetSeasonName()
	{
		return Language_Manager.instance.GetText(this.seasonNames[this.seasonIndex]);
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x0003190C File Offset: 0x0002FB0C
	public string GetSeasonNameByIndex(int _index)
	{
		return Language_Manager.instance.GetText(this.seasonNames[_index]);
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x00031920 File Offset: 0x0002FB20
	public int GetSeasonIndex()
	{
		return this.seasonIndex;
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x00031928 File Offset: 0x0002FB28
	public int GetSeasonMax()
	{
		return this.seasonNames.Length;
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x00031932 File Offset: 0x0002FB32
	public void LoadSeason(SaveData _data)
	{
		int world_SeasonIndex = _data.world_SeasonIndex;
		this.SetNewSeason(_data.world_SeasonIndex);
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x00031948 File Offset: 0x0002FB48
	public void RefreshClimateEffect()
	{
		List<GameObject> list = new List<GameObject>();
		if (this.seasonIndex == 0)
		{
			list = this.climate_EffectsSpring;
		}
		else if (this.seasonIndex == 1)
		{
			list = this.climate_EffectsSummer;
		}
		else if (this.seasonIndex == 2)
		{
			list = this.climate_EffectsAutumn;
		}
		else if (this.seasonIndex == 3)
		{
			list = this.climate_EffectsWinter;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (this.climate_EffectsSummer[i])
			{
				this.climate_EffectsSummer[i].SetActive(false);
			}
			if (this.climate_EffectsAutumn[i])
			{
				this.climate_EffectsAutumn[i].SetActive(false);
			}
			if (this.climate_EffectsWinter[i])
			{
				this.climate_EffectsWinter[i].SetActive(false);
			}
			if (this.climate_EffectsSpring[i])
			{
				this.climate_EffectsSpring[i].SetActive(false);
			}
		}
		int index = this.climate_Indexes[0];
		if (list[index])
		{
			list[index].SetActive(true);
		}
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x00031A74 File Offset: 0x0002FC74
	public void SetNewForecast()
	{
		if (this.climate_Indexes.Count > 0)
		{
			this.climate_Indexes.RemoveAt(0);
		}
		for (int i = 0; i < 3; i++)
		{
			if (i == this.climate_Indexes.Count - 1)
			{
				int item = UnityEngine.Random.Range(0, this.climate_Sprites.Count);
				this.climate_Indexes.Add(item);
			}
		}
		this.RefreshClimateEffect();
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x00031ADC File Offset: 0x0002FCDC
	public void LoadForecast(SaveData _data)
	{
		if (_data.world_Forecast == null)
		{
			this.SetNewForecast();
			return;
		}
		this.climate_Indexes.Clear();
		for (int i = 0; i < 3; i++)
		{
			if (i <= _data.world_Forecast.Count)
			{
				this.climate_Indexes.Add(_data.world_Forecast[i]);
			}
			else
			{
				int item = UnityEngine.Random.Range(0, 2);
				this.climate_Indexes.Add(item);
			}
		}
		this.RefreshClimateEffect();
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x00031B50 File Offset: 0x0002FD50
	public Sprite GetForecastSpriteByIndex(int _index)
	{
		return this.climate_Sprites[_index];
	}

	// Token: 0x0400060E RID: 1550
	public static World_Manager instance;

	// Token: 0x0400060F RID: 1551
	[SerializeField]
	private float timeScale_Normal = 1f;

	// Token: 0x04000610 RID: 1552
	[SerializeField]
	private float currentTime;

	// Token: 0x04000611 RID: 1553
	[SerializeField]
	private Vector2 currentHourMin;

	// Token: 0x04000612 RID: 1554
	public int startTimeInSeconds;

	// Token: 0x04000613 RID: 1555
	public int startTime_NoDelivery_InSeconds;

	// Token: 0x04000614 RID: 1556
	private float playTime;

	// Token: 0x04000615 RID: 1557
	private bool reachedDayEnding;

	// Token: 0x04000616 RID: 1558
	[Header("Lights")]
	[SerializeField]
	public GameObject light_Parent;

	// Token: 0x04000617 RID: 1559
	[SerializeField]
	private Light light_Terrain;

	// Token: 0x04000618 RID: 1560
	[SerializeField]
	private Gradient[] ligthColor_Terrain;

	// Token: 0x04000619 RID: 1561
	[SerializeField]
	private float lightColor_LerpSpeed_Terrain;

	// Token: 0x0400061A RID: 1562
	[SerializeField]
	private float lightIndex;

	// Token: 0x0400061B RID: 1563
	[SerializeField]
	private float illumination = 1f;

	// Token: 0x0400061C RID: 1564
	public int day_overall;

	// Token: 0x0400061D RID: 1565
	[SerializeField]
	private int dayIndex;

	// Token: 0x0400061E RID: 1566
	[SerializeField]
	private int week_Index;

	// Token: 0x0400061F RID: 1567
	private int week_Max = 1;

	// Token: 0x04000620 RID: 1568
	private string[] dayNames = new string[]
	{
		"Sunday",
		"Monday",
		"Tuesday",
		"Wednesday",
		"Thursday",
		"Friday",
		"Saturday"
	};

	// Token: 0x04000621 RID: 1569
	[Header("Levels")]
	public int currentLevelIndex;

	// Token: 0x04000622 RID: 1570
	[SerializeField]
	public Level_Controller currentLevel;

	// Token: 0x04000623 RID: 1571
	[SerializeField]
	private GameObject[] levels_GOs;

	// Token: 0x04000624 RID: 1572
	public int currentExpansionIndex;

	// Token: 0x04000625 RID: 1573
	[SerializeField]
	public int expansion_RemainingDays;

	// Token: 0x04000626 RID: 1574
	[SerializeField]
	public int[] days_to_expand = new int[]
	{
		3,
		4,
		5,
		6,
		7,
		8,
		9,
		10
	};

	// Token: 0x04000627 RID: 1575
	[Header("Debug")]
	[SerializeField]
	private int debug_maxCharQnt;

	// Token: 0x04000628 RID: 1576
	[SerializeField]
	private float debug_spawnTimeMax;

	// Token: 0x04000629 RID: 1577
	[Header("Mini Game Levels")]
	public int curentOtherModesLevelsIndex;

	// Token: 0x0400062A RID: 1578
	[SerializeField]
	private GameObject currentOtherModesLevels;

	// Token: 0x0400062B RID: 1579
	[SerializeField]
	private GameObject[] miniGameLevels_GOs;

	// Token: 0x0400062C RID: 1580
	[Header("Seasons")]
	[SerializeField]
	private int seasonIndex;

	// Token: 0x0400062D RID: 1581
	public bool activateTaigaAwards;

	// Token: 0x0400062E RID: 1582
	[SerializeField]
	private Texture2D[] seasonTextures = new Texture2D[4];

	// Token: 0x0400062F RID: 1583
	[SerializeField]
	private Color[] grassColor = new Color[4];

	// Token: 0x04000630 RID: 1584
	private string[] seasonNames = new string[]
	{
		"Spring",
		"Summer",
		"Autumn",
		"Winter"
	};

	// Token: 0x04000631 RID: 1585
	[Header("Climate")]
	[SerializeField]
	private List<GameObject> climate_EffectsSummer = new List<GameObject>();

	// Token: 0x04000632 RID: 1586
	[SerializeField]
	private List<GameObject> climate_EffectsAutumn = new List<GameObject>();

	// Token: 0x04000633 RID: 1587
	[SerializeField]
	private List<GameObject> climate_EffectsWinter = new List<GameObject>();

	// Token: 0x04000634 RID: 1588
	[SerializeField]
	private List<GameObject> climate_EffectsSpring = new List<GameObject>();

	// Token: 0x04000635 RID: 1589
	[SerializeField]
	public List<int> climate_Indexes = new List<int>();

	// Token: 0x04000636 RID: 1590
	[SerializeField]
	public List<Sprite> climate_Sprites = new List<Sprite>();

	// Token: 0x04000637 RID: 1591
	public List<World_Manager.WorldEvents> worldEvents = new List<World_Manager.WorldEvents>();

	// Token: 0x02000098 RID: 152
	public class WorldEvents
	{
		// Token: 0x04000722 RID: 1826
		public int week;

		// Token: 0x04000723 RID: 1827
		public int day;

		// Token: 0x04000724 RID: 1828
		public Inv_Manager.ProdType prodType;
	}
}
