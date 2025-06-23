using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class Char_Controller : MonoBehaviour
{
	// Token: 0x06000053 RID: 83 RVA: 0x00004DFC File Offset: 0x00002FFC
	private void Awake()
	{
		this.rigidbody_ = base.GetComponent<Rigidbody>();
		this.animator_ = base.transform.Find("Scaler").Find("Anim_").GetComponent<Animator>();
		this.animator_.gameObject.AddComponent<AnimationEvent_Controller>();
		if (this.isCustomer)
		{
			this.customer_Controller = base.GetComponent<Customer_Controller>();
		}
		if (this.isGenCustomer)
		{
			this.customer_Controller = base.GetComponent<Customer_Controller>();
		}
		if (this.isStaff)
		{
			this.staff_Controller = base.GetComponent<Staff_Controller>();
		}
		if (!this.isCustomer && !this.isGenCustomer && !this.isStaff)
		{
			this.storyChar_Controller = base.GetComponent<StoryChar_Controller>();
		}
		this.talk_Controller = base.GetComponent<Talk_Controller>();
		if (this.talk_Controller)
		{
			this.talk_Controller.char_Controller = this;
		}
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00004ED0 File Offset: 0x000030D0
	private void Start()
	{
		if (this.isTutorial)
		{
			Game_Manager.instance.tutorialHappening = true;
			this.navPoints = new List<GameObject>(World_Manager.instance.currentLevel.navSpheres_Tutorial);
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00004EFF File Offset: 0x000030FF
	private void Update()
	{
		if (!Game_Manager.instance)
		{
			return;
		}
		this.UpdateEnterExit();
		this.UpdateAI();
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00004F1C File Offset: 0x0000311C
	private async void UpdateEnterExit()
	{
		if (this.destroyChar)
		{
			if (base.transform.localScale.x > 0.05f)
			{
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.zero, this.changeScaleSpeed * Time.unscaledDeltaTime);
			}
			else
			{
				Char_Manager.instance.DestroyStoryChar(this);
				Char_Manager.instance.DestroyCustomer(this);
			}
		}
		else if (base.transform.localScale.x != 1f)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.one, this.changeScaleSpeed * Time.unscaledDeltaTime);
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00004F58 File Offset: 0x00003158
	public void DestroyChar()
	{
		if (this.isTutorial)
		{
			Game_Manager.instance.tutorialHappening = false;
			Game_Manager.instance.firsDay = false;
			Mail_Manager.instance.Send_Mail_OldMan_Welcome();
		}
		if (this.customer_Controller != null)
		{
			if (this.customer_Controller.emotion_ctrl != null)
			{
				this.customer_Controller.emotion_ctrl.DestroyCtrl();
			}
			if (this.customer_Controller.prodWanted_ctrl != null)
			{
				this.customer_Controller.prodWanted_ctrl.DestroyCtrl();
			}
		}
		Game_Manager.instance.SetCinematicMode(false);
		this.destroyChar = true;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00004FF3 File Offset: 0x000031F3
	public void StartLeaving()
	{
		this.ResetNavPath();
		this.navPath = Nav_Manager.instance.GetNavPathExit(base.transform.position, true);
		this.leaving = true;
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00005020 File Offset: 0x00003220
	public void SetPlayerController(Player_Controller _player)
	{
		if (this.player_Controller && this.player_Controller != _player)
		{
			return;
		}
		this.player_Controller = _player;
		if (this.player_Controller)
		{
			this.HideEmojis(true);
		}
		else
		{
			this.HideEmojis(false);
		}
		base.CancelInvoke("NullPlayerController");
		base.Invoke("NullPlayerController", this.nullPlayerControllerTime);
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00005089 File Offset: 0x00003289
	public void NullPlayerController()
	{
		this.player_Controller = null;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00005094 File Offset: 0x00003294
	public void HideEmojis(bool _b)
	{
		if (this.customer_Controller.emotion_ctrl)
		{
			this.customer_Controller.emotion_ctrl.SetHide(_b);
		}
		if (this.customer_Controller.prodWanted_ctrl)
		{
			this.customer_Controller.prodWanted_ctrl.SetHide(_b);
		}
	}

	// Token: 0x0600005C RID: 92 RVA: 0x000050E8 File Offset: 0x000032E8
	private void UpdateAI()
	{
		if (!this.destroyChar && this.leaving && (this.isTutorial || this.isEvilGuy || this.isSeller || this.isAuditor))
		{
			this.MoveByPath();
			if (this.navPathIndex >= this.navPath.Count - 1)
			{
				this.DestroyChar();
				this.leaving = false;
			}
		}
		if (this.isTutorial && (!this.destroyChar || this.leaving))
		{
			if (!this.moving)
			{
				this.player_Controller = Player_Manager.instance.GetPlayerController(0);
				if (this.player_Controller)
				{
					Vector3 normalized = (this.player_Controller.transform.position - base.transform.position).normalized;
					this.Rotate(normalized);
				}
			}
			if (this.talk_Controller.talkIndex <= 0)
			{
				return;
			}
			this.MoveByPath();
		}
	}

	// Token: 0x0600005D RID: 93 RVA: 0x000051D0 File Offset: 0x000033D0
	public void MoveByPath()
	{
		if (this.navPath.Count == 0)
		{
			return;
		}
		if (this.navPathIndex <= this.navPath.Count)
		{
			if (Vector3.Distance(base.transform.position, this.navPath[this.navPathIndex].transform.position) < 1.2f)
			{
				this.NextNavSphere();
				this.moving = false;
				return;
			}
			Vector3 normalized = (this.navPath[this.navPathIndex].transform.position - base.transform.position).normalized;
			this.Move(normalized);
		}
	}

	// Token: 0x0600005E RID: 94 RVA: 0x0000527C File Offset: 0x0000347C
	private bool NextNavSphere()
	{
		if (this.navPath.Count == 0)
		{
			return false;
		}
		bool result = false;
		if (this.navPathIndex < this.navPath.Count - 1)
		{
			this.navPathIndex++;
		}
		if (this.navPathIndex >= this.navPath.Count - 1)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x0600005F RID: 95 RVA: 0x000052D5 File Offset: 0x000034D5
	public void ResetNavPath()
	{
		this.navPath.Clear();
		this.navPathIndex = 0;
	}

	// Token: 0x06000060 RID: 96 RVA: 0x000052EC File Offset: 0x000034EC
	public void Move(Vector3 _moveVec3)
	{
		this.moving = true;
		_moveVec3.y = 0f;
		if (this.isGrounded && this.charVelocity.y < 0f)
		{
			this.charVelocity.y = 0f;
		}
		this.charVelocity.y = this.charVelocity.y + this.gravitySpeed * Time.deltaTime;
		float num = this.moveSpeedWalk;
		if (this.findNewPathTime_Timer > this.findNewPathTime)
		{
			num = this.moveSpeedRun;
			_moveVec3 += base.transform.right;
			this.findNewPathTime_Timer = this.findNewPathTime * 0.95f;
		}
		float num2 = this.rotSpeedWalk;
		if (_moveVec3 != Vector3.zero)
		{
			Quaternion b = Quaternion.LookRotation(_moveVec3);
			b = Quaternion.Euler(_moveVec3.normalized.magnitude * this.leanLimit, b.eulerAngles.y, 0f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, num2 * Time.deltaTime);
		}
		else
		{
			Quaternion b2 = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b2, num2 * Time.deltaTime);
		}
		Vector3 velocity = this.rigidbody_.velocity;
		velocity.x = 0f;
		velocity.z = 0f;
		Vector3 velocity2 = new Vector3(_moveVec3.x, 0f, _moveVec3.z) * num;
		velocity2.y = velocity.y + this.gravitySpeed * Time.deltaTime;
		this.rigidbody_.velocity = velocity2;
		if (this.animator_)
		{
			this.animator_.SetFloat("MoveSpeed", Mathf.Clamp01(new Vector2(_moveVec3.x, _moveVec3.z).magnitude) * (num * this.moveSpeedAnimationMultiplier));
		}
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00005500 File Offset: 0x00003700
	public void Rotate(Vector3 _moveVec3)
	{
		_moveVec3.y = 0f;
		float num = this.rotSpeedOnlyRotation;
		if (_moveVec3 != Vector3.zero)
		{
			Quaternion b = Quaternion.LookRotation(_moveVec3);
			b = Quaternion.Euler(_moveVec3.normalized.magnitude * this.leanLimit, b.eulerAngles.y, 0f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, num * Time.deltaTime);
			if (this.animator_)
			{
				if (Quaternion.Angle(base.transform.rotation, b) > 10f)
				{
					this.animator_.SetFloat("MoveSpeed", this.moveSpeedWalk * this.moveSpeedAnimationMultiplier);
					return;
				}
				this.animator_.SetFloat("MoveSpeed", 0f);
				return;
			}
		}
		else
		{
			Quaternion b2 = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b2, num * Time.deltaTime);
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x0000562F File Offset: 0x0000382F
	public void SetID(int _index)
	{
		this.charID = _index;
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00005638 File Offset: 0x00003838
	public int GetID()
	{
		return this.charID;
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00005640 File Offset: 0x00003840
	public string GetCharName()
	{
		return this.charName;
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00005648 File Offset: 0x00003848
	public Color GetCharColor()
	{
		return this.charColor;
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00005650 File Offset: 0x00003850
	public Sprite GetCharSprite()
	{
		return this.charSprite;
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00005658 File Offset: 0x00003858
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Door" && this.mayOpenDoors)
		{
			other.GetComponent<Door_Controller>().OpenDoor(base.transform.position, false);
		}
	}

	// Token: 0x0400005F RID: 95
	[SerializeField]
	public string charName;

	// Token: 0x04000060 RID: 96
	[SerializeField]
	public Sprite charSprite;

	// Token: 0x04000061 RID: 97
	[SerializeField]
	public Sprite charFriendshipPhoto;

	// Token: 0x04000062 RID: 98
	[SerializeField]
	public Color charColor;

	// Token: 0x04000063 RID: 99
	[SerializeField]
	public int charID;

	// Token: 0x04000064 RID: 100
	public bool inGame;

	// Token: 0x04000065 RID: 101
	public bool isCustomer;

	// Token: 0x04000066 RID: 102
	public bool isGenCustomer;

	// Token: 0x04000067 RID: 103
	public bool isStaff;

	// Token: 0x04000068 RID: 104
	public bool isTutorial;

	// Token: 0x04000069 RID: 105
	public bool isEvilGuy;

	// Token: 0x0400006A RID: 106
	public bool isSeller;

	// Token: 0x0400006B RID: 107
	public bool isAuditor;

	// Token: 0x0400006C RID: 108
	public bool isTaigaAward;

	// Token: 0x0400006D RID: 109
	public Customer_Controller customer_Controller;

	// Token: 0x0400006E RID: 110
	public Staff_Controller staff_Controller;

	// Token: 0x0400006F RID: 111
	public StoryChar_Controller storyChar_Controller;

	// Token: 0x04000070 RID: 112
	public Talk_Controller talk_Controller;

	// Token: 0x04000071 RID: 113
	public Rigidbody rigidbody_;

	// Token: 0x04000072 RID: 114
	public Animator animator_;

	// Token: 0x04000073 RID: 115
	public Player_Controller player_Controller;

	// Token: 0x04000074 RID: 116
	[HideInInspector]
	private bool destroyChar;

	// Token: 0x04000075 RID: 117
	private readonly float changeScaleSpeed = 2f;

	// Token: 0x04000076 RID: 118
	private readonly float nullPlayerControllerTime = 0.5f;

	// Token: 0x04000077 RID: 119
	[Header("Movement")]
	[SerializeField]
	public Vector3 copyMove;

	// Token: 0x04000078 RID: 120
	[SerializeField]
	public float moveSpeedWalk;

	// Token: 0x04000079 RID: 121
	[SerializeField]
	public float moveSpeedRun;

	// Token: 0x0400007A RID: 122
	[SerializeField]
	public float moveSpeedAnimationMultiplier;

	// Token: 0x0400007B RID: 123
	[SerializeField]
	public float rotSpeedWalk;

	// Token: 0x0400007C RID: 124
	[SerializeField]
	public float rotSpeedOnlyRotation;

	// Token: 0x0400007D RID: 125
	[SerializeField]
	public float leanLimit;

	// Token: 0x0400007E RID: 126
	private float gravitySpeed = -10f;

	// Token: 0x0400007F RID: 127
	[SerializeField]
	public float chairAnim;

	// Token: 0x04000080 RID: 128
	[SerializeField]
	public bool mayOpenDoors;

	// Token: 0x04000081 RID: 129
	public bool moving;

	// Token: 0x04000082 RID: 130
	public bool leaving;

	// Token: 0x04000083 RID: 131
	public float findNewPathTime = 3f;

	// Token: 0x04000084 RID: 132
	public float findNewPathTime_Timer;

	// Token: 0x04000085 RID: 133
	private Vector3 charVelocity;

	// Token: 0x04000086 RID: 134
	private readonly bool isGrounded;

	// Token: 0x04000087 RID: 135
	[Header("NavPath")]
	[SerializeField]
	public List<GameObject> navPath = new List<GameObject>();

	// Token: 0x04000088 RID: 136
	[SerializeField]
	private int navPathIndex;

	// Token: 0x04000089 RID: 137
	public List<GameObject> navPoints = new List<GameObject>();
}
