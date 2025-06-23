using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200004A RID: 74
public class MiniGame_Manager : MonoBehaviour
{
	// Token: 0x060003D9 RID: 985 RVA: 0x000240B0 File Offset: 0x000222B0
	private void Start()
	{
		this.CreateReferences();
		this.HideMiniGameCanvas();
	}

	// Token: 0x060003DA RID: 986 RVA: 0x000240C0 File Offset: 0x000222C0
	private void Update()
	{
		Vector3 v = this.cam_Game.WorldToScreenPoint(this.lastInteractivePos);
		v.z = 0f;
		this.miniGame_Canvas.anchoredPosition = Interactor_Manager.instance.GetPosAfterRender(v, false);
		if (this.miniGame_Index == -1 || this.currentInteractive == null)
		{
			return;
		}
		this.miniGame_Updates[this.miniGame_Index]();
		if (this.currentInteractive.ui_Pos[0])
		{
			this.lastInteractivePos = this.currentInteractive.ui_Pos[0].transform.position;
		}
		else
		{
			this.lastInteractivePos = this.currentInteractive.transform.position;
		}
		float num = Vector3.Distance(this.currentInteractive.transform.position, Player_Manager.instance.GetPlayerController(this.player_index).transform.position);
		if (num > 3f)
		{
			Debug.Log(num);
			this.FinishMiniGame(false);
		}
	}

	// Token: 0x060003DB RID: 987 RVA: 0x000241D0 File Offset: 0x000223D0
	public void SetMiniGameStage(int _index)
	{
		this.miniGame_Stage = _index;
	}

	// Token: 0x060003DC RID: 988 RVA: 0x000241D9 File Offset: 0x000223D9
	public bool GetIsMiniGaming()
	{
		return this.miniGame_Index != -1;
	}

	// Token: 0x060003DD RID: 989 RVA: 0x000241E8 File Offset: 0x000223E8
	public void StartMiniGame(int _index, Interaction_Controller _interactive, int _player_index)
	{
		Debug.Log("Started MiniGame");
		base.CancelInvoke();
		this.miniGame_Canvas.gameObject.SetActive(false);
		this.miniGame_Canvas.gameObject.SetActive(true);
		this.miniGame_Index = _index;
		this.currentInteractive = _interactive;
		this.SetMiniGameStage(0);
		this.player_index = _player_index;
		Player_Manager.instance.GetPlayerController(_player_index).isMiniGaming = true;
		for (int i = 0; i < this.miniGame_Panels.Count; i++)
		{
			if (i == _index)
			{
				this.miniGame_Panels[i].SetActive(true);
			}
			else
			{
				this.miniGame_Panels[i].SetActive(false);
			}
		}
	}

	// Token: 0x060003DE RID: 990 RVA: 0x00024294 File Offset: 0x00022494
	public void FinishMiniGame(bool _right)
	{
		Input_Manager.instance.SetCooldown(0, 0.3f);
		base.Invoke("HideMiniGameCanvas", 2f);
		if (!_right)
		{
			if (this.miniGame_Index == 2)
			{
				Inv_Manager.instance.CreateDirtNextToObject(this.currentInteractive.gameObject, 0, 3);
			}
			if (this.miniGame_Index == 3)
			{
				Inv_Manager.instance.CreateDirtNextToObject(this.currentInteractive.gameObject, 1, 3);
			}
		}
		this.miniGame_Index = -1;
		this.SetMiniGameStage(-1);
		Player_Manager.instance.GetPlayerController(this.player_index).isMiniGaming = false;
		int num = 0;
		if (!_right)
		{
			num = 1;
		}
		for (int i = 0; i < this.miniGame_Panels.Count; i++)
		{
			if (i == num)
			{
				this.miniGame_Panels[i].SetActive(true);
			}
			else
			{
				this.miniGame_Panels[i].SetActive(false);
			}
		}
		this.miniGame_Canvas.gameObject.SetActive(false);
		this.miniGame_Canvas.gameObject.SetActive(true);
	}

	// Token: 0x060003DF RID: 991 RVA: 0x00024391 File Offset: 0x00022591
	public void HideMiniGameCanvas()
	{
		if (this.miniGame_Canvas.gameObject.activeSelf)
		{
			this.miniGame_Canvas.gameObject.SetActive(false);
		}
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x000243B6 File Offset: 0x000225B6
	public void MiniGame_Inputs(bool _holdingButton, int _buttonIndex)
	{
		if (this.miniGame_Index != 0 && this.miniGame_Index != 1 && this.miniGame_Index != 2)
		{
			int num = this.miniGame_Index;
		}
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x000243DB File Offset: 0x000225DB
	public void MiniGame_AimInputs(Vector2 _aim, int _player_index)
	{
		if (Input_Manager.instance.GetScheme(_player_index) == "Joystick")
		{
			this.aimRaw = _aim * 15f;
			return;
		}
		this.aimRaw = _aim;
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x00024410 File Offset: 0x00022610
	public float Get_Sensitivity()
	{
		float result = 1f;
		if (Input_Manager.instance.GetScheme(this.player_index) == "Keyboard&Mouse")
		{
			result = Settings_Manager.instance.GetMouseSensitivity(1f);
		}
		else if (Input_Manager.instance.GetScheme(this.player_index) == "Joystick")
		{
			result = Settings_Manager.instance.GetGamepadSensitivity(3f);
		}
		return result;
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x00024480 File Offset: 0x00022680
	private void CreateReferences()
	{
		this.miniGame_Updates.Add(new MiniGame_Manager.MiniGame_Update(this.MiniGame_Update_Right));
		this.miniGame_Updates.Add(new MiniGame_Manager.MiniGame_Update(this.MiniGame_Update_Wrong));
		this.miniGame_Updates.Add(new MiniGame_Manager.MiniGame_Update(this.MiniGame_Update_CleanDirt));
		this.miniGame_Updates.Add(new MiniGame_Manager.MiniGame_Update(this.MiniGame_Update_WaterPlant));
		this.miniGame_Updates.Add(new MiniGame_Manager.MiniGame_Update(this.MiniGame_Update_FixMach));
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x00024500 File Offset: 0x00022700
	private void MiniGame_Update_Right()
	{
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x00024502 File Offset: 0x00022702
	private void MiniGame_Update_Wrong()
	{
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x00024504 File Offset: 0x00022704
	private void MiniGame_Update_CleanDirt()
	{
		if (!this.currentInteractive)
		{
			this.FinishMiniGame(false);
		}
		if (this.miniGame_Stage == 0)
		{
			this.mG_CleanDirt_TimeToFail_Timer = 0f;
			this.mG_CleanDirt_Hits_Current = 0;
			this.mG_CleanDirt_MayScoreAgain = false;
			this.SetMiniGameStage(10);
			return;
		}
		if (this.miniGame_Stage == 10)
		{
			if (Quaternion.Angle(this.mG_CleanDirt_Panel_Broom.localRotation, this.mG_CleanDirt_Panel_BroomDesired.localRotation) <= this.mG_CleanDirt_AngleLimit_ToScore)
			{
				if (this.mG_CleanDirt_MayScoreAgain)
				{
					this.mG_CleanDirt_MayScoreAgain = false;
					this.mG_CleanDirt_Hits_Current++;
				}
			}
			else
			{
				this.mG_CleanDirt_MayScoreAgain = true;
			}
			this.mG_CleanDirt_Panel_Broom_Rotation_Desired = Mathf.Clamp(this.mG_CleanDirt_Panel_Broom_Rotation_Desired, 0f, this.mG_CleanDirt_Panel_RotarionMax);
			this.mG_CleanDirt_Panel_Broom_Rotation_Desired += this.aimRaw.x * this.mG_CleanDirt_Panel_Broom_Rotation_IncreaseRate * this.Get_Sensitivity() * Time.deltaTime;
			this.mG_CleanDirt_Panel_Broom.localRotation = Quaternion.Lerp(this.mG_CleanDirt_Panel_Broom.localRotation, Quaternion.Euler(0f, 0f, this.mG_CleanDirt_Panel_Broom_Rotation_Desired), Time.deltaTime * this.mG_CleanDirt_Panel_Broom_Rotation_Speed);
			this.mG_CleanDirt_TimeToFail_Timer += Time.deltaTime;
			if (this.mG_CleanDirt_TimeToFail_Timer >= this.mG_CleanDirt_TimeToFail)
			{
				this.FinishMiniGame(false);
				return;
			}
			if (this.mG_CleanDirt_Hits_Current >= this.mG_CleanDirt_Hits_Needed)
			{
				this.SetMiniGameStage(99);
				return;
			}
		}
		else if (this.miniGame_Stage == 99)
		{
			Inv_Manager.instance.DeleteDirtController(this.currentInteractive);
			this.FinishMiniGame(true);
			Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.clean_Dirt, -1);
		}
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x00024698 File Offset: 0x00022898
	private void MiniGame_Update_WaterPlant()
	{
		if (!this.currentInteractive)
		{
			this.FinishMiniGame(false);
		}
		if (this.miniGame_Stage == 0)
		{
			this.mG_WaterPlant_TimeToFail_Timer = 0f;
			this.mG_WaterPlant_Score_Current = 0f;
			this.mG_WaterPlant_Panel_Plant_Rotation_TimeToRandom_Timer = 99f;
			this.mG_WaterPlant_Panel_WateringCan.localRotation = Quaternion.Euler(0f, 0f, 0f);
			this.mG_WaterPlant_Panel_Watering_Rotation_Desired = UnityEngine.Random.Range(this.mG_WaterPlant_Panel_RotarionMax, 0f);
			this.mG_WaterPlant_Panel_Plant.localRotation = Quaternion.Euler(0f, 0f, this.mG_WaterPlant_Panel_RotarionStart);
			this.SetMiniGameStage(10);
			return;
		}
		if (this.miniGame_Stage == 10)
		{
			if (Quaternion.Angle(this.mG_WaterPlant_Panel_WateringCan.localRotation, this.mG_WaterPlant_Panel_Plant.localRotation) <= this.mG_WaterPlant_AngleLimit_ToScore)
			{
				this.mG_WaterPlant_Score_Current += Time.deltaTime * this.mG_WaterPlant_Score_IncreaseRate;
				this.mG_WaterPlant_Image_Watering.color = this.mG_WaterPlant_Colors[1];
			}
			else
			{
				this.mG_WaterPlant_TimeToFail_Timer += Time.deltaTime;
				this.mG_WaterPlant_Image_Watering.color = this.mG_WaterPlant_Colors[0];
			}
			this.mG_WaterPlant_Panel_Watering_Rotation_Desired = Mathf.Clamp(this.mG_WaterPlant_Panel_Watering_Rotation_Desired, this.mG_WaterPlant_Panel_RotarionMax, 0f);
			this.mG_WaterPlant_Panel_Plant_Rotation_TimeToRandom_Timer += Time.deltaTime;
			if (this.mG_WaterPlant_Panel_Plant_Rotation_TimeToRandom_Timer >= this.mG_WaterPlant_Panel_Plant_Rotation_TimeToRandom)
			{
				this.mG_WaterPlant_Panel_Plant_Rotation_TimeToRandom_Timer = 0f;
				float num = UnityEngine.Random.Range(this.mG_WaterPlant_Panel_RotarionMax, 0f);
				this.mG_WaterPlant_Panel_Plant_Rotation_Desired = num;
			}
			this.mG_WaterPlant_Panel_Watering_Rotation_Desired += this.aimRaw.y * this.mG_WaterPlant_Panel_Watering_Rotation_IncreaseRate * this.Get_Sensitivity() * 0.5f * Time.deltaTime;
			this.mG_WaterPlant_Panel_WateringCan.localRotation = Quaternion.Lerp(this.mG_WaterPlant_Panel_WateringCan.localRotation, Quaternion.Euler(0f, 0f, this.mG_WaterPlant_Panel_Watering_Rotation_Desired), Time.deltaTime * this.mG_WaterPlant_Panel_Watering_Rotation_Speed);
			this.mG_WaterPlant_Panel_Plant.localRotation = Quaternion.Lerp(this.mG_WaterPlant_Panel_Plant.localRotation, Quaternion.Euler(0f, 0f, this.mG_WaterPlant_Panel_Plant_Rotation_Desired), Time.deltaTime * this.mG_WaterPlant_Panel_Plant_Rotation_Speed);
			if (this.mG_WaterPlant_TimeToFail_Timer >= this.mG_WaterPlant_TimeToFail)
			{
				this.SetMiniGameStage(98);
				return;
			}
			if (this.mG_WaterPlant_Score_Current >= this.mG_WaterPlant_Score_Needed)
			{
				this.SetMiniGameStage(99);
				return;
			}
		}
		else
		{
			if (this.miniGame_Stage == 98)
			{
				Inv_Manager.instance.CreateDirtNextToObject(this.currentInteractive.gameObject, 1, 3);
				this.SetMiniGameStage(99);
				return;
			}
			if (this.miniGame_Stage == 99)
			{
				this.currentInteractive.GetComponent<Plant_Controller>().ResetLifeSpan();
				this.FinishMiniGame(true);
			}
		}
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x00024950 File Offset: 0x00022B50
	private void MiniGame_Update_FixMach()
	{
		if (!this.currentInteractive)
		{
			this.FinishMiniGame(false);
		}
		if (this.miniGame_Stage == 0)
		{
			this.mG_FixMach_Driver_Position_Desired = Vector3.zero;
			this.mG_FixMach_TimeToFail_Timer = 0f;
			for (int i = 0; i < this.mG_FixMach_Panel_ScrewList.Length; i++)
			{
				float z = UnityEngine.Random.Range(this.mG_FixMach_Screw_RotationStart[0], this.mG_FixMach_Screw_RotationStart[1]);
				this.mG_FixMach_Panel_ScrewList[i].localRotation = Quaternion.Euler(0f, 0f, z);
			}
			this.mG_FixMach_Screws_Current = 0;
			this.<MiniGame_Update_FixMach>g__FixMach_RefreshScrews|76_0(null);
			this.SetMiniGameStage(10);
			return;
		}
		if (this.miniGame_Stage == 10)
		{
			RectTransform rectTransform = this.mG_FixMach_Panel_ScrewDriver;
			RectTransform exclude = null;
			for (int j = 0; j < this.mG_FixMach_Panel_ScrewList.Length; j++)
			{
				RectTransform rectTransform2 = this.mG_FixMach_Panel_ScrewList[j];
				if (Vector3.Distance(rectTransform.position, rectTransform2.position) < this.mG_FixMach_PositionDifference_ToScore && rectTransform2.localRotation.eulerAngles.z < this.mG_FixMach_Screw_RotationFinish)
				{
					rectTransform2.localRotation = Quaternion.Euler(0f, 0f, rectTransform2.localRotation.eulerAngles.z + this.mG_FixMach_Screw_RotationSpeed * Time.deltaTime);
					exclude = rectTransform2;
					rectTransform2.GetComponent<Image>().color = this.mG_FixMach_Screw_Colors[2];
				}
			}
			this.mG_FixMach_Driver_Position_Desired.x = this.mG_FixMach_Driver_Position_Desired.x + this.aimRaw.x * this.mG_FixMach_Driver_MoveSpeed * this.Get_Sensitivity() * 0.5f * Time.deltaTime;
			this.mG_FixMach_Driver_Position_Desired.x = Mathf.Clamp(this.mG_FixMach_Driver_Position_Desired.x, -this.mG_FixMach_Driver_Position_Limit, this.mG_FixMach_Driver_Position_Limit);
			this.mG_FixMach_Driver_Position_Desired.y = this.mG_FixMach_Driver_Position_Desired.y + this.aimRaw.y * this.mG_FixMach_Driver_MoveSpeed * this.Get_Sensitivity() * 0.5f * Time.deltaTime;
			this.mG_FixMach_Driver_Position_Desired.y = Mathf.Clamp(this.mG_FixMach_Driver_Position_Desired.y, -this.mG_FixMach_Driver_Position_Limit, this.mG_FixMach_Driver_Position_Limit);
			rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, this.mG_FixMach_Driver_Position_Desired, this.mG_FixMach_Driver_MoveSpeed * Time.deltaTime);
			this.<MiniGame_Update_FixMach>g__FixMach_RefreshScrews|76_0(exclude);
			this.mG_FixMach_TimeToFail_Timer += Time.deltaTime;
			if (this.mG_FixMach_TimeToFail_Timer >= this.mG_FixMach_TimeToFail)
			{
				this.FinishMiniGame(false);
				return;
			}
			bool flag = true;
			for (int k = 0; k < this.mG_FixMach_Panel_ScrewList.Length; k++)
			{
				if (this.mG_FixMach_Panel_ScrewList[k].localRotation.eulerAngles.z < this.mG_FixMach_Screw_RotationFinish)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.SetMiniGameStage(99);
				return;
			}
		}
		else if (this.miniGame_Stage == 99)
		{
			this.currentInteractive.SetBrokenState(false, true);
			this.FinishMiniGame(true);
			Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.fixed_machine, -1);
		}
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x00024DF0 File Offset: 0x00022FF0
	[CompilerGenerated]
	private void <MiniGame_Update_FixMach>g__FixMach_RefreshScrews|76_0(RectTransform _exclude)
	{
		for (int i = 0; i < this.mG_FixMach_Panel_ScrewList.Length; i++)
		{
			RectTransform rectTransform = this.mG_FixMach_Panel_ScrewList[i];
			float num = this.mG_FixMach_Screw_RotationFinish;
			if (!_exclude || !(_exclude == rectTransform))
			{
				if (rectTransform.localRotation.eulerAngles.z >= num)
				{
					rectTransform.GetComponent<Image>().color = this.mG_FixMach_Screw_Colors[1];
				}
				else
				{
					rectTransform.GetComponent<Image>().color = this.mG_FixMach_Screw_Colors[0];
				}
				rectTransform.localScale = Vector3.one * this.mG_FixMach_Screw_Sizes[1];
			}
		}
	}

	// Token: 0x04000445 RID: 1093
	public int player_index;

	// Token: 0x04000446 RID: 1094
	public Camera cam_Game;

	// Token: 0x04000447 RID: 1095
	public Vector3 lastInteractivePos = Vector3.one * -10000f;

	// Token: 0x04000448 RID: 1096
	private static bool startingUp = true;

	// Token: 0x04000449 RID: 1097
	private float gamepadSensitivityMultiplier = 5f;

	// Token: 0x0400044A RID: 1098
	[Header("Mini Games New")]
	[SerializeField]
	private RectTransform miniGame_Canvas;

	// Token: 0x0400044B RID: 1099
	[SerializeField]
	private List<GameObject> miniGame_Panels = new List<GameObject>();

	// Token: 0x0400044C RID: 1100
	public int miniGame_Index = -1;

	// Token: 0x0400044D RID: 1101
	public int miniGame_Stage;

	// Token: 0x0400044E RID: 1102
	public Interaction_Controller currentInteractive;

	// Token: 0x0400044F RID: 1103
	private readonly int lastButtonIndex = -1;

	// Token: 0x04000450 RID: 1104
	private Vector2 aimRaw;

	// Token: 0x04000451 RID: 1105
	private readonly List<MiniGame_Manager.MiniGame_Update> miniGame_Updates = new List<MiniGame_Manager.MiniGame_Update>();

	// Token: 0x04000452 RID: 1106
	[Header("Clean Dirt")]
	[SerializeField]
	private RectTransform mG_CleanDirt_Panel_Broom;

	// Token: 0x04000453 RID: 1107
	[SerializeField]
	private RectTransform mG_CleanDirt_Panel_BroomDesired;

	// Token: 0x04000454 RID: 1108
	public float mG_CleanDirt_TimeToFail_Timer;

	// Token: 0x04000455 RID: 1109
	public int mG_CleanDirt_Hits_Current;

	// Token: 0x04000456 RID: 1110
	private bool mG_CleanDirt_MayScoreAgain;

	// Token: 0x04000457 RID: 1111
	private float mG_CleanDirt_Panel_Broom_Rotation_Desired;

	// Token: 0x04000458 RID: 1112
	private readonly int mG_CleanDirt_Hits_Needed = 5;

	// Token: 0x04000459 RID: 1113
	private readonly float mG_CleanDirt_TimeToFail = 5f;

	// Token: 0x0400045A RID: 1114
	private readonly float mG_CleanDirt_AngleLimit_ToScore = 8f;

	// Token: 0x0400045B RID: 1115
	private readonly float mG_CleanDirt_Panel_RotarionMax = 90f;

	// Token: 0x0400045C RID: 1116
	private readonly float mG_CleanDirt_Panel_RotarionStart = -40f;

	// Token: 0x0400045D RID: 1117
	private readonly float mG_CleanDirt_Panel_Broom_Rotation_Speed = 20f;

	// Token: 0x0400045E RID: 1118
	private readonly float mG_CleanDirt_Panel_Broom_Rotation_IncreaseRate = 5f;

	// Token: 0x0400045F RID: 1119
	[SerializeField]
	private Image mG_WaterPlant_Image_Watering;

	// Token: 0x04000460 RID: 1120
	[SerializeField]
	private RectTransform mG_WaterPlant_Panel_WateringCan;

	// Token: 0x04000461 RID: 1121
	[SerializeField]
	private RectTransform mG_WaterPlant_Panel_Plant;

	// Token: 0x04000462 RID: 1122
	[SerializeField]
	private Color[] mG_WaterPlant_Colors = new Color[]
	{
		Color.black,
		Color.black
	};

	// Token: 0x04000463 RID: 1123
	private float mG_WaterPlant_Score_Current;

	// Token: 0x04000464 RID: 1124
	private float mG_WaterPlant_TimeToFail_Timer;

	// Token: 0x04000465 RID: 1125
	private float mG_WaterPlant_Panel_Plant_Rotation_TimeToRandom_Timer;

	// Token: 0x04000466 RID: 1126
	private float mG_WaterPlant_Panel_Plant_Rotation_Desired;

	// Token: 0x04000467 RID: 1127
	private float mG_WaterPlant_Panel_Watering_Rotation_Desired;

	// Token: 0x04000468 RID: 1128
	private readonly float mG_WaterPlant_Score_Needed = 100f;

	// Token: 0x04000469 RID: 1129
	private readonly float mG_WaterPlant_Panel_RotarionMax = -90f;

	// Token: 0x0400046A RID: 1130
	private readonly float mG_WaterPlant_Panel_RotarionStart = -40f;

	// Token: 0x0400046B RID: 1131
	private readonly float mG_WaterPlant_Panel_Watering_Rotation_Speed = 20f;

	// Token: 0x0400046C RID: 1132
	private readonly float mG_WaterPlant_Panel_Watering_Rotation_IncreaseRate = 5f;

	// Token: 0x0400046D RID: 1133
	private readonly float mG_WaterPlant_Panel_Plant_Rotation_Speed = 0.5f;

	// Token: 0x0400046E RID: 1134
	private readonly float mG_WaterPlant_Panel_Plant_Rotation_TimeToRandom = 0.75f;

	// Token: 0x0400046F RID: 1135
	private readonly float mG_WaterPlant_Score_IncreaseRate = 25f;

	// Token: 0x04000470 RID: 1136
	private readonly float mG_WaterPlant_AngleLimit_ToScore = 10f;

	// Token: 0x04000471 RID: 1137
	private readonly float mG_WaterPlant_TimeToFail = 5f;

	// Token: 0x04000472 RID: 1138
	[Header("Fix Machinery")]
	[SerializeField]
	private RectTransform mG_FixMach_Panel_ScrewDriver;

	// Token: 0x04000473 RID: 1139
	[SerializeField]
	private RectTransform[] mG_FixMach_Panel_ScrewList;

	// Token: 0x04000474 RID: 1140
	[SerializeField]
	private Color[] mG_FixMach_Screw_Colors;

	// Token: 0x04000475 RID: 1141
	public int mG_FixMach_Screws_Current;

	// Token: 0x04000476 RID: 1142
	public float mG_FixMach_TimeToFail_Timer;

	// Token: 0x04000477 RID: 1143
	private Vector3 mG_FixMach_Driver_Position_Desired;

	// Token: 0x04000478 RID: 1144
	private float mG_FixMach_Driver_MoveSpeed = 10f;

	// Token: 0x04000479 RID: 1145
	private float mG_FixMach_Driver_Position_Limit = 170f;

	// Token: 0x0400047A RID: 1146
	private float[] mG_FixMach_Screw_Sizes = new float[]
	{
		0.5f,
		0.8f
	};

	// Token: 0x0400047B RID: 1147
	private float[] mG_FixMach_Screw_RotationStart = new float[]
	{
		0f,
		100f
	};

	// Token: 0x0400047C RID: 1148
	private float mG_FixMach_Screw_RotationFinish = 350f;

	// Token: 0x0400047D RID: 1149
	private float mG_FixMach_Screw_RotationSpeed = 150f;

	// Token: 0x0400047E RID: 1150
	private readonly int mG_FixMach_Screws_Needed = 5;

	// Token: 0x0400047F RID: 1151
	private readonly float mG_FixMach_TimeToFail = 20f;

	// Token: 0x04000480 RID: 1152
	private readonly float mG_FixMach_PositionDifference_ToScore = 25f;

	// Token: 0x02000084 RID: 132
	// (Invoke) Token: 0x060005C2 RID: 1474
	private delegate void MiniGame_Update();
}
