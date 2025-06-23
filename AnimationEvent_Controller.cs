using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class AnimationEvent_Controller : MonoBehaviour
{
	// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
	public void DeactivateGameObject()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000004 RID: 4 RVA: 0x0000206E File Offset: 0x0000026E
	public void FinishTalking()
	{
		Talk_Manager.instance.FinishTalk();
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0000207C File Offset: 0x0000027C
	public void Walk()
	{
		if (!this.player_Controller || this.player_Controller.is_dashing)
		{
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_PlayerWalk, base.transform, Audio_Manager.instance.event_null);
			return;
		}
		if (!this.player_Controller.GetIsRunning())
		{
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_PlayerWalk, base.transform, Audio_Manager.instance.event_null);
			return;
		}
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_PlayerRun, base.transform, Audio_Manager.instance.event_null);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x0000211E File Offset: 0x0000031E
	public void DeactivateSelf()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x0000212C File Offset: 0x0000032C
	public void ReceiptOff()
	{
		this.receiptAnim.CrossFade("EndDayReceipt_Off", 0.1f);
	}

	// Token: 0x0400000F RID: 15
	[SerializeField]
	private Player_Controller player_Controller;

	// Token: 0x04000010 RID: 16
	[SerializeField]
	private Animator receiptAnim;
}
