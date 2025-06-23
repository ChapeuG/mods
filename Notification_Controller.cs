using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000020 RID: 32
public class Notification_Controller : MonoBehaviour
{
	// Token: 0x060000F5 RID: 245 RVA: 0x0000B014 File Offset: 0x00009214
	private void Update()
	{
		this.time += Time.unscaledDeltaTime;
		if (this.time >= this.timer)
		{
			this.animator.SetBool("Bool", true);
			if (this.time >= this.timer + 1f)
			{
				this.DeleteNotification();
			}
			if (this.timer_Image)
			{
				this.timer_Image.fillAmount = 0f;
				return;
			}
		}
		else if (this.timer_Image)
		{
			this.timer_Image.fillAmount = Mathf.Abs(this.time / this.timer - 1f);
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x0000B0BA File Offset: 0x000092BA
	public void SetTimer(float _timer)
	{
		this.timer = _timer;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x0000B0C3 File Offset: 0x000092C3
	public void DeleteNotification()
	{
		if (!Menu_Manager.instance.DeleteNotification(this))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400017C RID: 380
	[SerializeField]
	private Image timer_Image;

	// Token: 0x0400017D RID: 381
	[SerializeField]
	private Animator animator;

	// Token: 0x0400017E RID: 382
	private float time;

	// Token: 0x0400017F RID: 383
	private float timer = 3f;
}
