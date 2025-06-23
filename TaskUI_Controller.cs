using System;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class TaskUI_Controller : MonoBehaviour
{
	// Token: 0x0600019F RID: 415 RVA: 0x0001046A File Offset: 0x0000E66A
	public void MayRun()
	{
		this.mayRun = true;
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x00010474 File Offset: 0x0000E674
	private void Update()
	{
		if (!this.mayRun)
		{
			return;
		}
		this.time += Time.unscaledDeltaTime;
		if (this.time >= this.timer)
		{
			this.animator.SetBool("Bool", true);
			if (this.time >= this.timer + 1f)
			{
				this.DeleteThis();
			}
		}
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x000104D5 File Offset: 0x0000E6D5
	public void SetTimer(float _timer)
	{
		this.timer = _timer;
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x000104DE File Offset: 0x0000E6DE
	public void DeleteThis()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400022A RID: 554
	private float time;

	// Token: 0x0400022B RID: 555
	private float timer = 10f;

	// Token: 0x0400022C RID: 556
	[SerializeField]
	private Animator animator;

	// Token: 0x0400022D RID: 557
	private bool mayRun;
}
