using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200001A RID: 26
public class Lock_Controller : MonoBehaviour
{
	// Token: 0x060000E6 RID: 230 RVA: 0x0000AA90 File Offset: 0x00008C90
	private void Awake()
	{
		this.anim = base.GetComponent<Animator>();
		this.keyHolder = base.transform.Find("Master").Find("KeyHolder");
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x0000AABE File Offset: 0x00008CBE
	public void ResetLockState()
	{
		this.Lock_This(!this.startsUnlocked);
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0000AAD0 File Offset: 0x00008CD0
	public void Lock(Player_Controller _player, bool _closeByPrompt)
	{
		bool flag = false;
		if (_player)
		{
			if (!_player.key_Controller && !this.isLocked)
			{
				flag = true;
			}
			else
			{
				if (!_player.key_Controller || !this.isLocked)
				{
					return;
				}
				flag = false;
			}
			if (Game_Manager.instance.GetMartOpen() && this.martOpener && !_closeByPrompt)
			{
				Menu_Manager.instance.SetWarningConfirmation("Do you want to close your store", this.player_index, "").onClick.AddListener(new UnityAction(this.Lock2));
				this.player_index = _player.playerIndex;
				_player.acceptedCloseMart = true;
				return;
			}
			_player.SetKeyActive(flag);
		}
		if (this.martOpener && _player)
		{
			Game_Manager.instance.SetMartOpenByPlayer();
			if (!Game_Manager.instance.GetMartOpen())
			{
				_player.SetKeyActive(false);
			}
		}
		this.Lock_This(flag);
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x0000ABB3 File Offset: 0x00008DB3
	private void Lock2()
	{
		this.Lock(Player_Manager.instance.GetPlayerController(this.player_index), true);
	}

	// Token: 0x060000EA RID: 234 RVA: 0x0000ABCC File Offset: 0x00008DCC
	public void Lock_This(bool _bool)
	{
		this.anim.SetBool("Bool", _bool);
		this.isLocked = _bool;
		this.keyHolder.gameObject.SetActive(!_bool);
	}

	// Token: 0x04000160 RID: 352
	[SerializeField]
	public bool isLocked;

	// Token: 0x04000161 RID: 353
	[SerializeField]
	private bool martOpener;

	// Token: 0x04000162 RID: 354
	[SerializeField]
	private bool startsUnlocked;

	// Token: 0x04000163 RID: 355
	private Animator anim;

	// Token: 0x04000164 RID: 356
	private Transform keyHolder;

	// Token: 0x04000165 RID: 357
	private int player_index;
}
