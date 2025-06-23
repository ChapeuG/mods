using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class OtherModes_Controller : MonoBehaviour
{
	// Token: 0x060000FD RID: 253 RVA: 0x0000B16B File Offset: 0x0000936B
	private void Awake()
	{
		this.CreateReferences();
	}

	// Token: 0x060000FE RID: 254 RVA: 0x0000B173 File Offset: 0x00009373
	private void Update()
	{
		this.modesUpdates[(int)this.modeIndex]();
	}

	// Token: 0x060000FF RID: 255 RVA: 0x0000B18B File Offset: 0x0000938B
	private void CreateReferences()
	{
		this.modesUpdates.Add(null);
		this.modesUpdates.Add(null);
		this.modesUpdates.Add(new OtherModes_Controller.OtherModes_Updates(this.Update_2));
	}

	// Token: 0x06000100 RID: 256 RVA: 0x0000B1BC File Offset: 0x000093BC
	private void SetModeStage(int _index)
	{
		Game_Manager.instance.SetModeStage(_index);
	}

	// Token: 0x06000101 RID: 257 RVA: 0x0000B1C9 File Offset: 0x000093C9
	private void SetCooldown(float _timer)
	{
		this.cooldown_Timer = _timer;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x0000B1D4 File Offset: 0x000093D4
	private void Update_2()
	{
		int modeStage = Game_Manager.instance.GetModeStage();
		this.cooldown_Timer -= Time.deltaTime;
		if (this.cooldown_Timer >= 0f)
		{
			return;
		}
		if (modeStage == 0)
		{
			Camera_Controller.instance.mainCamera.SetParent(this.m2_CameraHolder);
			Camera_Controller.instance.mainCamera.transform.localPosition = Vector3.zero;
			Camera_Controller.instance.mainCamera.transform.localRotation = Quaternion.Euler(Vector3.zero);
			Skin_Controller[] componentsInChildren = this.m2_SpectatorParent.GetComponentsInChildren<Skin_Controller>();
			Vector3 position = this.m2_PresenterGuy.transform.position;
			foreach (Skin_Controller skin_Controller in componentsInChildren)
			{
				position.y = skin_Controller.transform.position.y;
				skin_Controller.transform.LookAt(position);
			}
			this.SetModeStage(10);
			return;
		}
		if (modeStage != 1)
		{
			if (modeStage == 10)
			{
				if (!Game_Manager.instance.MayRun())
				{
					return;
				}
				this.SetCooldown(2f);
				this.SetModeStage(11);
				return;
			}
			else if (modeStage == 11)
			{
				this.m2_PresenterGuy.talk_Controller.Talk(0, -1);
				this.SetModeStage(12);
			}
		}
	}

	// Token: 0x04000184 RID: 388
	public OtherModes_Controller.ModeIndex modeIndex;

	// Token: 0x04000185 RID: 389
	private List<OtherModes_Controller.OtherModes_Updates> modesUpdates = new List<OtherModes_Controller.OtherModes_Updates>();

	// Token: 0x04000186 RID: 390
	private float cooldown_Timer;

	// Token: 0x04000187 RID: 391
	public Transform m2_CameraHolder;

	// Token: 0x04000188 RID: 392
	public Char_Controller m2_PresenterGuy;

	// Token: 0x04000189 RID: 393
	public GameObject m2_SpectatorParent;

	// Token: 0x02000066 RID: 102
	[Serializable]
	public enum ModeIndex
	{
		// Token: 0x0400066F RID: 1647
		Dream = 1,
		// Token: 0x04000670 RID: 1648
		TaigaAwards
	}

	// Token: 0x02000067 RID: 103
	// (Invoke) Token: 0x06000556 RID: 1366
	private delegate void OtherModes_Updates();
}
