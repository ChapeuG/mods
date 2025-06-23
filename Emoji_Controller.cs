using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000010 RID: 16
public class Emoji_Controller : MonoBehaviour
{
	// Token: 0x060000A7 RID: 167 RVA: 0x00008218 File Offset: 0x00006418
	private void Awake()
	{
		this.anim = base.gameObject.GetComponent<Animator>();
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x0000822B File Offset: 0x0000642B
	private void Start()
	{
		if (this.customer.char_Controller.isGenCustomer)
		{
			this.genericCharOffset = new Vector3(0f, -0.5f, 0f);
		}
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00008259 File Offset: 0x00006459
	private void FixedUpdate()
	{
		if (this.followObj)
		{
			base.transform.position = this.followObj.transform.position + this.followOffset + this.genericCharOffset;
		}
	}

	// Token: 0x060000AA RID: 170 RVA: 0x0000829C File Offset: 0x0000649C
	private void Update()
	{
		this.lookAt_Image.transform.LookAt(Camera.main.transform);
		this.active_Timer += Time.deltaTime;
		if (this.active_Timer > 3f && this.followObj == null && !this.markToDestroy)
		{
			this.markToDestroy = true;
			this.DestroyCtrl();
		}
		if (this.isEmoji && this.customer.prodWanted_ctrl)
		{
			this.followOffset = new Vector3(0f, 1f, 0f);
			return;
		}
		this.followOffset = Vector3.zero;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00008348 File Offset: 0x00006548
	public void SetEmojiSprite(Sprite _newSprite, bool _permanent = false)
	{
		if (this.markToDestroy)
		{
			return;
		}
		if (this.isEmoji || !this.customer.doneShopping)
		{
			if (!_permanent)
			{
				this.anim.PlayInFixedTime("Emoji_Temp", -1, 0f);
			}
			else
			{
				this.anim.PlayInFixedTime("Emoji_On", -1, 0f);
			}
			this.emoji_Image.sprite = _newSprite;
		}
	}

	// Token: 0x060000AC RID: 172 RVA: 0x000083B0 File Offset: 0x000065B0
	public void SetHide(bool _b)
	{
		if (this.markToDestroy)
		{
			return;
		}
		if (!_b)
		{
			this.anim.PlayInFixedTime("Emoji_Off", -1, 0f);
			return;
		}
		if (this.isEmoji)
		{
			this.anim.PlayInFixedTime("Emoji_Temp", -1, 0f);
			return;
		}
		if (!this.customer.doneShopping)
		{
			this.anim.PlayInFixedTime("Emoji_On", -1, 0f);
		}
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00008422 File Offset: 0x00006622
	public void SetObjToFollow(GameObject _followObj)
	{
		this.customer = _followObj.GetComponent<Customer_Controller>();
		this.anim.PlayInFixedTime("Emoji_Off", -1, 0f);
		this.followObj = _followObj;
	}

	// Token: 0x060000AE RID: 174 RVA: 0x0000844D File Offset: 0x0000664D
	public void DestroyCtrl()
	{
		this.anim.PlayInFixedTime("Emoji_Off", -1, 0f);
		UnityEngine.Object.Destroy(base.gameObject, 5f);
	}

	// Token: 0x040000E5 RID: 229
	public bool isEmoji;

	// Token: 0x040000E6 RID: 230
	[SerializeField]
	public Image emoji_Image;

	// Token: 0x040000E7 RID: 231
	[SerializeField]
	public Image lookAt_Image;

	// Token: 0x040000E8 RID: 232
	private GameObject followObj;

	// Token: 0x040000E9 RID: 233
	private Customer_Controller customer;

	// Token: 0x040000EA RID: 234
	private Vector3 followOffset = Vector3.zero;

	// Token: 0x040000EB RID: 235
	private Vector3 genericCharOffset = Vector3.zero;

	// Token: 0x040000EC RID: 236
	private Animator anim;

	// Token: 0x040000ED RID: 237
	private float active_Timer;

	// Token: 0x040000EE RID: 238
	private bool markToDestroy;
}
