using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003D RID: 61
public class EasterEgg_Manager : MonoBehaviour
{
	// Token: 0x0600025D RID: 605 RVA: 0x00014682 File Offset: 0x00012882
	private void Awake()
	{
		EasterEgg_Manager.instance = this;
	}

	// Token: 0x0600025E RID: 606 RVA: 0x0001468A File Offset: 0x0001288A
	private void Update()
	{
		this.Soccer_Update();
	}

	// Token: 0x0600025F RID: 607 RVA: 0x00014692 File Offset: 0x00012892
	public void Load_SaveData(SaveData _data)
	{
		if (_data.easterEgg_data_SD != null)
		{
			this.main_data = _data.easterEgg_data_SD;
		}
	}

	// Token: 0x06000260 RID: 608 RVA: 0x000146A8 File Offset: 0x000128A8
	public void Set_BoxHight_Hight(float _hight)
	{
		float num = _hight;
		string title = "Easter Egg - Box Hight";
		string text = "";
		if (num > this.main_data.boxHight_max_hight)
		{
			this.main_data.boxHight_max_hight = num;
			text = "NICE! NEW MAX SCORE!!! \n\n";
		}
		text = string.Concat(new string[]
		{
			text,
			"Now: ",
			num.ToString("F2"),
			" meters \nMax Score: ",
			this.main_data.boxHight_max_hight.ToString("F2"),
			" meters"
		});
		Menu_Manager.instance.SetNotification(title, text, false);
	}

	// Token: 0x06000261 RID: 609 RVA: 0x0001473F File Offset: 0x0001293F
	public bool Get_Is_MiniGaming()
	{
		return this.current_game_level != null;
	}

	// Token: 0x06000262 RID: 610 RVA: 0x00014754 File Offset: 0x00012954
	public void Start_Soccer(bool _start)
	{
		if (Game_Manager.instance.GetMartOpen())
		{
			return;
		}
		if (this.current_game_level != null)
		{
			UnityEngine.Object.Destroy(this.current_game_level.gameObject);
		}
		if (!_start)
		{
			foreach (Player_Controller player_Controller in Player_Manager.instance.playerControllerList)
			{
				player_Controller.BackToMartRandomPos();
			}
			World_Manager.instance.light_Parent.SetActive(true);
			Menu_Manager.instance.SetMenuName("Pause");
			Invoker.InvokeDelayed(new Invokable(Menu_Manager.instance.UpdateMenu), 0.25f);
			base.StopAllCoroutines();
			return;
		}
		World_Manager.instance.light_Parent.SetActive(false);
		for (int i = 0; i < this.soccer_score_current.Length; i++)
		{
			this.soccer_score_current[i] = 0;
		}
		this.Soccer_Refresh_ScoreTexts();
		if (this.current_game_level != null)
		{
			UnityEngine.Object.Destroy(this.current_game_level.gameObject);
		}
		this.current_game_level = UnityEngine.Object.Instantiate<GameObject>(this.soccer_level_res);
		this.current_game_level.transform.position = new Vector3(200f, 0f, 0f);
		this.camera_holder = this.current_game_level.transform.Find("Camera_Holder").gameObject;
		this.soccer_time_current = this.soccer_time_start;
		this.Soccer_Set_Players_Teams();
		base.StartCoroutine(this.Soccer_Reset_Players_Pos(0f));
		base.StartCoroutine(this.Soccer_Create_Ball(3f));
		Menu_Manager.instance.SetMenuName("EE_Game");
	}

	// Token: 0x06000263 RID: 611 RVA: 0x00014900 File Offset: 0x00012B00
	public void Soccer_Update()
	{
		this.soccer_time_text.text = this.Get_Seconds_In_Time(this.soccer_time_current);
		if (this.soccer_ball_current != null && this.soccer_time_current > 0f)
		{
			this.soccer_time_current -= Time.deltaTime;
			if (this.soccer_time_current <= 0f)
			{
				this.soccer_time_current = 0f;
				base.StartCoroutine(this.Soccer_Time_Out());
			}
		}
	}

	// Token: 0x06000264 RID: 612 RVA: 0x00014978 File Offset: 0x00012B78
	public string Get_Seconds_In_Time(float _raw_seconds)
	{
		int num = Mathf.FloorToInt(_raw_seconds / 60f);
		int num2 = Mathf.FloorToInt((_raw_seconds / 60f - (float)num) * 60f);
		string result = num.ToString() + ":" + num2.ToString();
		if (num2 < 10)
		{
			result = num.ToString() + ":0" + num2.ToString();
		}
		return result;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x000149E6 File Offset: 0x00012BE6
	private IEnumerator Soccer_Time_Out()
	{
		if (this.soccer_ball_current)
		{
			this.soccer_ball_current.DestroyBall();
		}
		yield return new WaitForSecondsRealtime(5f);
		this.Start_Soccer(false);
		yield break;
	}

	// Token: 0x06000266 RID: 614 RVA: 0x000149F8 File Offset: 0x00012BF8
	private void Soccer_Set_Players_Teams()
	{
		int count = Player_Manager.instance.playerControllerList.Count;
		List<int> list = new List<int>();
		list.Add(0);
		if (count == 2)
		{
			list.Add(1);
		}
		if (count == 3)
		{
			list.Add(0);
		}
		if (count == 4)
		{
			list.Add(1);
		}
		List<int> list2 = new List<int>(list);
		for (int i = 0; i < list.Count; i++)
		{
			int index = UnityEngine.Random.Range(0, list2.Count);
			this.soccer_player_teams.Add(list2[index]);
			list2.RemoveAt(index);
		}
	}

	// Token: 0x06000267 RID: 615 RVA: 0x00014A7F File Offset: 0x00012C7F
	private IEnumerator Soccer_Reset_Players_Pos(float _time)
	{
		yield return new WaitForSeconds(_time);
		this.soccer_waiting_ball = true;
		int[] array = new int[2];
		Vector3[,] array2 = new Vector3[2, 2];
		array2[0, 0] = new Vector3(-16f, 1f, -6f);
		array2[0, 1] = new Vector3(-16f, 1f, 6f);
		array2[1, 0] = new Vector3(16f, 1f, 6f);
		array2[1, 1] = new Vector3(16f, 1f, -6f);
		Vector3[,] array3 = array2;
		using (List<Player_Controller>.Enumerator enumerator = Player_Manager.instance.playerControllerList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Player_Controller player_Controller = enumerator.Current;
				int num = this.soccer_player_teams[player_Controller.playerIndex];
				Vector3 position = this.current_game_level.transform.position + array3[num, array[num]];
				player_Controller.transform.position = position;
				array[num]++;
				player_Controller.Set_Multiplayer_Color(this.soccer_player_colors[num]);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x06000268 RID: 616 RVA: 0x00014A98 File Offset: 0x00012C98
	public void Soccer_Set_Goal(int _index)
	{
		this.soccer_score_current[_index]++;
		this.Soccer_Refresh_ScoreTexts();
		base.StartCoroutine(this.Soccer_Reset_Players_Pos(2f));
		base.StartCoroutine(this.Soccer_Create_Ball(4f));
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_soccer_goal);
	}

	// Token: 0x06000269 RID: 617 RVA: 0x00014AF4 File Offset: 0x00012CF4
	private void Soccer_Refresh_ScoreTexts()
	{
		for (int i = 0; i < this.soccer_score_texts.Length; i++)
		{
			this.soccer_score_texts[i].text = this.soccer_score_current[i].ToString();
		}
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00014B32 File Offset: 0x00012D32
	private IEnumerator Soccer_Create_Ball(float _time)
	{
		if (this.soccer_ball_current != null)
		{
			this.soccer_ball_current.DestroyBall();
		}
		yield return new WaitForSeconds(_time);
		this.soccer_waiting_ball = false;
		this.soccer_ball_current = UnityEngine.Object.Instantiate<GameObject>(this.soccer_ball_res).GetComponent<Ball_Controller>();
		this.soccer_ball_current.transform.SetParent(this.current_game_level.transform);
		this.soccer_ball_current.transform.localPosition = new Vector3(0f, 5f, 0f);
		yield break;
	}

	// Token: 0x0400032F RID: 815
	public static EasterEgg_Manager instance;

	// Token: 0x04000330 RID: 816
	public EasterEgg_Data main_data = new EasterEgg_Data();

	// Token: 0x04000331 RID: 817
	public GameObject current_game_level;

	// Token: 0x04000332 RID: 818
	[HideInInspector]
	public GameObject camera_holder;

	// Token: 0x04000333 RID: 819
	[SerializeField]
	private GameObject soccer_level_res;

	// Token: 0x04000334 RID: 820
	private int[] soccer_score_current = new int[2];

	// Token: 0x04000335 RID: 821
	[SerializeField]
	private GameObject soccer_player_start_mark;

	// Token: 0x04000336 RID: 822
	[SerializeField]
	private Color[] soccer_player_colors = new Color[2];

	// Token: 0x04000337 RID: 823
	private float soccer_time_start = 180f;

	// Token: 0x04000338 RID: 824
	private float soccer_time_current;

	// Token: 0x04000339 RID: 825
	public Ball_Controller soccer_ball_current;

	// Token: 0x0400033A RID: 826
	public bool soccer_waiting_ball;

	// Token: 0x0400033B RID: 827
	[SerializeField]
	private Text[] soccer_score_texts = new Text[2];

	// Token: 0x0400033C RID: 828
	[SerializeField]
	private Text soccer_time_text;

	// Token: 0x0400033D RID: 829
	private List<int> soccer_player_teams = new List<int>();

	// Token: 0x0400033E RID: 830
	[SerializeField]
	private GameObject soccer_ball_res;
}
