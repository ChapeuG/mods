using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000016 RID: 22
public class Interaction_Controller : MonoBehaviour
{
	// Token: 0x060000C8 RID: 200 RVA: 0x00009A44 File Offset: 0x00007C44
	private void Awake()
	{
		if (this.inter_position != null)
		{
			this.inter_position.transform.Find("GameObject").gameObject.SetActive(false);
		}
		if (this.isPc)
		{
			this.RefreshPCTexture();
			PC_Manager.instance.pc_controller = this;
			return;
		}
		if (this.isLocker)
		{
			this.locker_Controller = base.gameObject.GetComponent<Locker_Controller>();
			return;
		}
		if (this.isBox)
		{
			this.anim = base.gameObject.GetComponent<Animator>();
			this.box_Controller = base.gameObject.GetComponent<Box_Controller>();
			return;
		}
		if (this.isShelf)
		{
			this.shelf_Controller = base.gameObject.GetComponent<Shelf_Controller>();
			this.event_Material = this.shelf_Controller.event_Material;
			return;
		}
		if (this.isDecor)
		{
			this.decor_Controller = base.gameObject.GetComponent<Decor_Controller>();
			this.event_Material = this.decor_Controller.event_Material;
			return;
		}
		if (this.isWallPaint)
		{
			this.wallPaint_Controller = base.gameObject.GetComponent<WallPaint_Controller>();
			return;
		}
		if (this.isFloor)
		{
			this.floor_Controller = base.gameObject.GetComponent<Floor_Controller>();
			return;
		}
		if (this.isUtil)
		{
			this.util_Controller = base.gameObject.GetComponent<Util_Controller>();
			this.event_Material = this.util_Controller.event_Material;
			return;
		}
		if (this.isTrash)
		{
			this.anim = base.gameObject.GetComponent<Animator>();
			return;
		}
		if (this.isChar)
		{
			this.char_Controller = base.gameObject.GetComponent<Char_Controller>();
			return;
		}
		if (this.isCashier)
		{
			this.cashier_Controller = base.gameObject.GetComponent<Cashier_Controller>();
			return;
		}
		if (this.isOpenSign)
		{
			this.openSign_Controller = base.gameObject.GetComponent<OpenSign_Controller>();
			return;
		}
		if (this.isDirt)
		{
			Inv_Manager.instance.AddDirtControllers(this);
			return;
		}
		if (this.isLock)
		{
			this.lock_Controller = base.gameObject.GetComponent<Lock_Controller>();
			return;
		}
		if (!this.isLeaveMart && this.isBall)
		{
			this.anim = base.gameObject.GetComponent<Animator>();
		}
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00009C4C File Offset: 0x00007E4C
	private void Start()
	{
		if (this.isMusic)
		{
			if (Audio_Manager.instance.music_isPlaying)
			{
				this.Play_ParticleSound(true);
			}
			else
			{
				this.Play_ParticleSound(false);
			}
			Audio_Manager.instance.playMusic_Event.AddListener(new UnityAction(this.Play_ParticleSound_True));
			Audio_Manager.instance.stopMusic_Event.AddListener(new UnityAction(this.Play_ParticleSound_False));
		}
		if (this.isMachinery)
		{
			this.broken_Particles[0] = base.transform.Find("brokenParticles_Ray").GetComponent<ParticleSystem>();
			this.broken_Particles[1] = base.transform.Find("brokenParticles_Smoke").GetComponent<ParticleSystem>();
		}
		this.RefreshBrokenParticles();
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00009CFB File Offset: 0x00007EFB
	public void ReadyToInteract(bool _ready)
	{
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00009D00 File Offset: 0x00007F00
	public bool StaffInteraction(Staff_Controller _staff)
	{
		if (this.isDecor)
		{
			if (!_staff.GetIsEmpty())
			{
				return false;
			}
			Plant_Controller plant = null;
			if (this.isDecorPlant)
			{
				plant = base.GetComponent<Plant_Controller>();
			}
			this.decor_Controller.GetThisDecorBox(_staff, this.isGarbage, plant);
			return true;
		}
		else
		{
			if (this.isBox)
			{
				return !this.box_Controller.isHeld && _staff.HoldOrChangeBox(base.GetComponent<Box_Controller>());
			}
			if (this.isDirt)
			{
				Inv_Manager.instance.DeleteDirtController(this);
				return true;
			}
			return false;
		}
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00009D80 File Offset: 0x00007F80
	public void Interact(bool _holdingButton, int _buttonIndex, Player_Controller _player)
	{
		if (this.isPc && _buttonIndex == 0)
		{
			Menu_Manager.instance.SetMenuName("PC", _player.playerIndex);
			return;
		}
		if (this.isLocker && _buttonIndex == 0)
		{
			Menu_Manager.instance.SetMenuName("Locker", _player.playerIndex);
			Menu_Manager.instance.locker_player_index = _player.playerIndex;
			_player.locker_Controller = this.locker_Controller;
			return;
		}
		if (this.isShelf)
		{
			if (this.isMachinery && this.isBroken && _player.tools_Controller)
			{
				Interactor_Manager.instance.ui_ctrls[_player.playerIndex].miniGame_manager.gameObject.SetActive(true);
				Interactor_Manager.instance.ui_ctrls[_player.playerIndex].miniGame_manager.StartMiniGame(4, this, _player.playerIndex);
			}
			if (!this.shelf_Controller.GetIsEmpty() || !this.shelf_Controller.isShelfProd || !_player.GetIsEmptyOrHasSameShelfBox(this.shelf_Controller.shelfIndex))
			{
				this.shelf_Controller.Interact(_holdingButton, _buttonIndex, _player);
				return;
			}
			if (_buttonIndex == 0)
			{
				this.shelf_Controller.GetThisShelfBox(_player);
				return;
			}
			if (_buttonIndex == 2)
			{
				this.RotateObj(1);
				return;
			}
		}
		else if (this.isDecor)
		{
			if (this.isDecorPlant && _player.tools_Controller)
			{
				Interactor_Manager.instance.ui_ctrls[_player.playerIndex].miniGame_manager.gameObject.SetActive(true);
				Interactor_Manager.instance.ui_ctrls[_player.playerIndex].miniGame_manager.StartMiniGame(3, this, _player.playerIndex);
			}
			if (_player.GetIsEmptyOrHasSameDecorBox(this.decor_Controller.decorIndex))
			{
				if (_buttonIndex == 0)
				{
					Plant_Controller plant = null;
					if (this.isDecorPlant)
					{
						plant = base.GetComponent<Plant_Controller>();
					}
					this.decor_Controller.GetThisDecorBox(_player, this.isGarbage, plant);
					return;
				}
				if (_buttonIndex == 2)
				{
					this.RotateObj(1);
					return;
				}
			}
		}
		else if (this.isUtil)
		{
			if (this.isMachinery && this.isBroken && _player.tools_Controller)
			{
				Interactor_Manager.instance.ui_ctrls[_player.playerIndex].miniGame_manager.gameObject.SetActive(true);
				Interactor_Manager.instance.ui_ctrls[_player.playerIndex].miniGame_manager.StartMiniGame(4, this, _player.playerIndex);
				return;
			}
			if (!this.util_Controller.GetIsEmpty() || !_player.GetIsEmptyOrHasSameUtilBox(this.util_Controller.utilIndex))
			{
				this.util_Controller.Interact(_holdingButton, _buttonIndex, _player);
				return;
			}
			if (_buttonIndex == 0)
			{
				this.util_Controller.GetThisUtilBox(_player, this.isGarbage);
				return;
			}
			if (_buttonIndex == 2)
			{
				this.RotateObj(1);
				return;
			}
		}
		else
		{
			if (this.isNews && _buttonIndex == 0)
			{
				Menu_Manager.instance.SetMenuName("News");
				return;
			}
			if (this.isBox && _buttonIndex == 0)
			{
				if (this.box_Controller.isHeld)
				{
					return;
				}
				Player_Manager.instance.GetPlayerController(_player.playerIndex).HoldOrChangeBox(base.GetComponent<Box_Controller>());
				return;
			}
			else if (this.isTrash && _buttonIndex == 0)
			{
				if (_player.boxController)
				{
					Debug.Log("Trashing confirmation");
					Button button = Menu_Manager.instance.SetWarningConfirmation("Do you want to trash this box?", _player.playerIndex, "");
					Player_Controller player = _player;
					button.onClick.AddListener(delegate()
					{
						this.TrashBox(_player);
					});
					_player.acceptedCloseMart = true;
					return;
				}
			}
			else if (this.isChar && _buttonIndex == 0)
			{
				if (!_player.boxController)
				{
					this.char_Controller.talk_Controller.Talk(0, _player.playerIndex);
					return;
				}
				if (_player.boxController.isProd)
				{
					int itemIndex = _player.boxController.itemIndex;
					int lifeSpanIndex = _player.boxController.lifeSpanIndex;
					if (this.char_Controller.customer_Controller.ReceiveItem(Inv_Manager.instance.prod_Prefabs[itemIndex], lifeSpanIndex, _player.playerIndex))
					{
						_player.GiveProdFromBox();
						return;
					}
				}
			}
			else if (!this.isStoryChar)
			{
				if (this.isCashier && _buttonIndex == 0)
				{
					this.cashier_Controller.OperateCashier(_player);
					return;
				}
				if (this.isDiscountTable && _buttonIndex == 0)
				{
					_player.CreateDiscountBlock(0);
					return;
				}
				if (this.isToolsTable)
				{
					_player.SetToolsActive(true);
					return;
				}
				if (this.isCartTable)
				{
					_player.SetCartActive(true);
					return;
				}
				if (this.isOpenSign)
				{
					Game_Manager.instance.SetMartOpenByPlayer();
					return;
				}
				if (this.isLock)
				{
					this.lock_Controller.Lock(_player, false);
					return;
				}
				if (this.isDirt && _buttonIndex == 0)
				{
					Interactor_Manager.instance.ui_ctrls[_player.playerIndex].miniGame_manager.gameObject.SetActive(true);
					Interactor_Manager.instance.ui_ctrls[_player.playerIndex].miniGame_manager.StartMiniGame(2, this, _player.playerIndex);
				}
			}
		}
	}

	// Token: 0x060000CD RID: 205 RVA: 0x0000A343 File Offset: 0x00008543
	public void TrashBox(Player_Controller _player)
	{
		Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.trash_items, -1);
		_player.TrashBox();
		this.AnimateGeneric();
	}

	// Token: 0x060000CE RID: 206 RVA: 0x0000A35E File Offset: 0x0000855E
	public void Shake()
	{
		this.anim.SetTrigger("Shake");
	}

	// Token: 0x060000CF RID: 207 RVA: 0x0000A370 File Offset: 0x00008570
	public void AnimateGeneric()
	{
		this.anim.PlayInFixedTime("Main", -1, 0f);
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x0000A388 File Offset: 0x00008588
	public void RotateObj(int _direction)
	{
		Build_Manager.instance.SetRotateObj(base.gameObject, _direction);
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_Rotate, base.transform, this.event_Material);
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x0000A3BC File Offset: 0x000085BC
	public void Play_Particles()
	{
		if (this.isMusic)
		{
			return;
		}
		for (int i = 0; i < this.int_Particles.Length; i++)
		{
			this.int_Particles[i].Play();
		}
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x0000A3F2 File Offset: 0x000085F2
	private void Play_ParticleSound_True()
	{
		this.Play_ParticleSound(true);
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x0000A3FB File Offset: 0x000085FB
	private void Play_ParticleSound_False()
	{
		this.Play_ParticleSound(false);
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x0000A404 File Offset: 0x00008604
	public void Play_ParticleSound(bool _b)
	{
		if (!this.isMusic)
		{
			return;
		}
		for (int i = 0; i < this.int_Particles.Length; i++)
		{
			if (_b)
			{
				this.int_Particles[i].Play();
			}
			else
			{
				this.int_Particles[i].Stop();
			}
		}
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x0000A44C File Offset: 0x0000864C
	public void SetBrokenState(bool _b, bool _fixing_it = false)
	{
		if (!this.isMachinery)
		{
			return;
		}
		this.isBroken = _b;
		if (_fixing_it)
		{
			this.broken_odds = -1;
		}
		this.RefreshBrokenParticles();
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x0000A470 File Offset: 0x00008670
	public void RefreshBrokenState()
	{
		if (!this.isMachinery)
		{
			return;
		}
		if (this.isBroken)
		{
			this.RefreshBrokenParticles();
			return;
		}
		int num = 5;
		int num2 = 30;
		if (this.broken_odds == -1)
		{
			this.broken_odds = UnityEngine.Random.Range(0, num);
		}
		if (UnityEngine.Random.Range(0, 100) < this.broken_odds)
		{
			this.isBroken = true;
		}
		else if (UnityEngine.Random.Range(0, 100) < num2 && this.broken_odds < num)
		{
			this.broken_odds++;
		}
		this.RefreshBrokenParticles();
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x0000A4F4 File Offset: 0x000086F4
	public void RefreshBrokenParticles()
	{
		if (this.isBroken && this.isMachinery)
		{
			for (int i = 0; i < this.broken_Particles.Length; i++)
			{
				if (this.broken_Particles[i])
				{
					this.broken_Particles[i].Play();
				}
			}
			this.Play_ParticleSound(false);
			return;
		}
		if (this.isMachinery)
		{
			for (int j = 0; j < this.broken_Particles.Length; j++)
			{
				if (this.broken_Particles[j])
				{
					this.broken_Particles[j].Stop();
				}
			}
			this.Play_ParticleSound(true);
		}
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x0000A588 File Offset: 0x00008788
	public void RefreshPCTexture()
	{
		Material[] array = new Material[1];
		if (Mail_Manager.instance.new_mails)
		{
			array[0] = this.pc_materials[1];
		}
		else
		{
			array[0] = this.pc_materials[0];
		}
		base.GetComponent<MeshRenderer>().materials = array;
	}

	// Token: 0x04000117 RID: 279
	public bool act_as_navSphere;

	// Token: 0x04000118 RID: 280
	public GameObject navSphereDeactivated;

	// Token: 0x04000119 RID: 281
	public GameObject inter_position;

	// Token: 0x0400011A RID: 282
	public bool isEditable;

	// Token: 0x0400011B RID: 283
	public bool isPc;

	// Token: 0x0400011C RID: 284
	public bool isLocker;

	// Token: 0x0400011D RID: 285
	public Locker_Controller locker_Controller;

	// Token: 0x0400011E RID: 286
	public bool isShelf;

	// Token: 0x0400011F RID: 287
	public Shelf_Controller shelf_Controller;

	// Token: 0x04000120 RID: 288
	public bool isDecor;

	// Token: 0x04000121 RID: 289
	public bool isDecorPlant;

	// Token: 0x04000122 RID: 290
	public Decor_Controller decor_Controller;

	// Token: 0x04000123 RID: 291
	public bool isWallPaint;

	// Token: 0x04000124 RID: 292
	public WallPaint_Controller wallPaint_Controller;

	// Token: 0x04000125 RID: 293
	public bool isFloor;

	// Token: 0x04000126 RID: 294
	public Floor_Controller floor_Controller;

	// Token: 0x04000127 RID: 295
	public bool isUtil;

	// Token: 0x04000128 RID: 296
	public Util_Controller util_Controller;

	// Token: 0x04000129 RID: 297
	public bool isNews;

	// Token: 0x0400012A RID: 298
	public bool isBox;

	// Token: 0x0400012B RID: 299
	public Box_Controller box_Controller;

	// Token: 0x0400012C RID: 300
	public bool isProd;

	// Token: 0x0400012D RID: 301
	public bool isTrash;

	// Token: 0x0400012E RID: 302
	public bool isGarbage;

	// Token: 0x0400012F RID: 303
	public bool isChar;

	// Token: 0x04000130 RID: 304
	public bool isStoryChar;

	// Token: 0x04000131 RID: 305
	public bool isCashier;

	// Token: 0x04000132 RID: 306
	public bool isDiscountTable;

	// Token: 0x04000133 RID: 307
	public bool isToolsTable;

	// Token: 0x04000134 RID: 308
	public bool isCartTable;

	// Token: 0x04000135 RID: 309
	public bool isLock;

	// Token: 0x04000136 RID: 310
	public Lock_Controller lock_Controller;

	// Token: 0x04000137 RID: 311
	public bool isOpenSign;

	// Token: 0x04000138 RID: 312
	public OpenSign_Controller openSign_Controller;

	// Token: 0x04000139 RID: 313
	public bool isDirt;

	// Token: 0x0400013A RID: 314
	public bool isLeaveMart;

	// Token: 0x0400013B RID: 315
	public int dirtTypeIndex;

	// Token: 0x0400013C RID: 316
	public bool isMusic;

	// Token: 0x0400013D RID: 317
	public bool isMachinery;

	// Token: 0x0400013E RID: 318
	public bool isBroken;

	// Token: 0x0400013F RID: 319
	public bool isBall;

	// Token: 0x04000140 RID: 320
	private int broken_odds = -1;

	// Token: 0x04000141 RID: 321
	[HideInInspector]
	public Char_Controller char_Controller;

	// Token: 0x04000142 RID: 322
	private Animator anim;

	// Token: 0x04000143 RID: 323
	[HideInInspector]
	public Cashier_Controller cashier_Controller;

	// Token: 0x04000144 RID: 324
	[SerializeField]
	public List<GameObject> ui_Pos = new List<GameObject>();

	// Token: 0x04000145 RID: 325
	[SerializeField]
	public ParticleSystem[] int_Particles;

	// Token: 0x04000146 RID: 326
	[SerializeField]
	private Animator[] int_Animators;

	// Token: 0x04000147 RID: 327
	[SerializeField]
	public GameObject[] int_NavPoint;

	// Token: 0x04000148 RID: 328
	[HideInInspector]
	public EventReference event_Material;

	// Token: 0x04000149 RID: 329
	[SerializeField]
	public ParticleSystem[] broken_Particles = new ParticleSystem[2];

	// Token: 0x0400014A RID: 330
	[SerializeField]
	public Material[] pc_materials = new Material[2];
}
