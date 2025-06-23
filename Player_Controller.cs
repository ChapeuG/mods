using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class Player_Controller : MonoBehaviour
{
	// Token: 0x0600010B RID: 267 RVA: 0x0000B438 File Offset: 0x00009638
	public void Set_Multiplayer_Color(Color _color)
	{
		if (_color == Color.white)
		{
			this.multiplayerColorMesh.gameObject.SetActive(false);
			return;
		}
		this.multiplayerColorMesh.gameObject.SetActive(true);
		Material[] array = new Material[]
		{
			UnityEngine.Object.Instantiate<Material>(this.multiplayerColorMesh.materials[0])
		};
		array[0].color = _color;
		this.multiplayerColorMesh.materials = array;
	}

	// Token: 0x0600010C RID: 268 RVA: 0x0000B4A6 File Offset: 0x000096A6
	private void Awake()
	{
		this.rigidbody_ = base.GetComponent<Rigidbody>();
		this.skin_ = base.GetComponent<Skin_Controller>();
	}

	// Token: 0x0600010D RID: 269 RVA: 0x0000B4C0 File Offset: 0x000096C0
	private void Start()
	{
		this.camera_Controller = Camera_Controller.instance;
		PhysicMaterial material = this.capsuleCollider.material;
		material.staticFriction = 0f;
		material.dynamicFriction = 0f;
		this.capsuleCollider.material = material;
		this.Set_Multiplayer_Color(Color.white);
	}

	// Token: 0x0600010E RID: 270 RVA: 0x0000B514 File Offset: 0x00009714
	private void Update()
	{
		if (!this.mayUpdate)
		{
			return;
		}
		this.SortInteractives();
		this.UpdateAutoMove();
		this.EE_Start_Update();
		if (base.transform.position.y <= -20f)
		{
			VoidPortal_Controller.instance.TransportPlayer(this);
		}
		if (Menu_Manager.instance.GetMenuName() != "Locker")
		{
			this.locker_Controller = null;
		}
	}

	// Token: 0x0600010F RID: 271 RVA: 0x0000B57B File Offset: 0x0000977B
	private void FixedUpdate()
	{
		this.MovementUpdate();
	}

	// Token: 0x06000110 RID: 272 RVA: 0x0000B584 File Offset: 0x00009784
	private void MovementUpdate()
	{
		if (!Game_Manager.instance.MayRun())
		{
			return;
		}
		if (Menu_Manager.instance.GetMenuName() != "MainMenu" && Menu_Manager.instance.GetMenuName() != "EE_Game")
		{
			this.moveVec3 = Vector3.zero;
		}
		float num = this.rotSpeedWalk;
		if (this.GetIsRunning())
		{
			num = this.rotSpeedRun;
		}
		Vector3 normalized = (this.camera_Controller.mainCamera.transform.forward * this.moveVec3.z + this.camera_Controller.mainCamera.transform.right * this.moveVec3.x).normalized;
		if (this.moveVec3 != Vector3.zero)
		{
			float num2 = this.leanLimitWalk;
			if (this.GetIsRunning())
			{
				num2 = this.leanLimitRun;
			}
			Quaternion b = Quaternion.LookRotation(normalized);
			b = Quaternion.Euler(this.moveVec3.normalized.magnitude * num2, b.eulerAngles.y, 0f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, num * Time.deltaTime);
		}
		else
		{
			Quaternion b2 = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b2, num * Time.deltaTime);
		}
		float num3 = this.moveSpeedWalk;
		if (this.GetIsRunning() && Mathf.Abs(this.moveVec3.magnitude) >= 0.05f && !this.is_dashing)
		{
			num3 = this.moveSpeedRun;
			this.runParticles.enableEmission = true;
			Input_Manager.instance.SetVibration(0.2f, 0.1f);
		}
		else
		{
			this.runParticles.enableEmission = false;
		}
		Vector3 velocity = this.rigidbody_.velocity;
		velocity.x = 0f;
		velocity.z = 0f;
		Vector3 velocity2 = new Vector3(normalized.x, 0f, normalized.z) * num3 * Mathf.Clamp01(new Vector2(this.moveVec3.x, this.moveVec3.z).magnitude);
		if (this.is_dashing)
		{
			velocity2.x = this.rigidbody_.velocity.x;
			velocity2.z = this.rigidbody_.velocity.z;
		}
		if (EasterEgg_Manager.instance.Get_Is_MiniGaming() && EasterEgg_Manager.instance.soccer_waiting_ball)
		{
			velocity2.x = 0f;
			velocity2.z = 0f;
		}
		velocity2.y = velocity.y + this.gravitySpeed * Time.deltaTime;
		this.rigidbody_.velocity = velocity2;
		this.animator_.SetBool("Dashing", this.is_dashing);
		this.animator_.SetFloat("MoveSpeed", Mathf.Clamp01(new Vector2(this.moveVec3.x, this.moveVec3.z).magnitude) * num3);
		this.debug_world_y_pos = base.transform.position.y;
	}

	// Token: 0x06000111 RID: 273 RVA: 0x0000B8E0 File Offset: 0x00009AE0
	private void MovementFixedUpdate()
	{
	}

	// Token: 0x06000112 RID: 274 RVA: 0x0000B8E4 File Offset: 0x00009AE4
	public void Dash()
	{
		if (this.is_dashing || Menu_Manager.instance.GetMenuName() != "EE_Game")
		{
			return;
		}
		this.is_dashing = true;
		float d = this.shoot_impulse_forward_force;
		float y = this.shoot_impulse_updward_force;
		Vector3 force = base.transform.forward * d;
		force.y = y;
		this.rigidbody_.AddForce(force, ForceMode.VelocityChange);
		Invoker.InvokeDelayed(new Invokable(this.Set_Not_Dashing), this.dash_recover_time);
	}

	// Token: 0x06000113 RID: 275 RVA: 0x0000B963 File Offset: 0x00009B63
	public void Set_Not_Dashing()
	{
		this.is_dashing = false;
	}

	// Token: 0x06000114 RID: 276 RVA: 0x0000B96C File Offset: 0x00009B6C
	public void Move(Vector2 vec2)
	{
		bool flag = false;
		if (this.GetPlayerWalking() && Mathf.Abs(vec2.magnitude) <= 0.7f)
		{
			flag = true;
		}
		else if (!this.GetPlayerWalking() && Mathf.Abs(vec2.magnitude) > 0.7f)
		{
			flag = true;
		}
		if (this.cashier_Controller && vec2.magnitude <= 0.9f)
		{
			return;
		}
		if (this.cashier_Controller && vec2.magnitude > 0.9f)
		{
			this.cashier_Controller.RemoveOperator();
		}
		if (this.locker_Controller)
		{
			return;
		}
		this.moveVec3 = new Vector3(vec2.x, 0f, vec2.y);
		if (flag)
		{
			Interactor_Manager.instance.RefreshInputHints(this.playerIndex);
		}
		if (vec2.magnitude == 0f)
		{
			if (Game_Manager.instance.MayRun() && Game_Manager.instance.GetGameMode() == 0 && !this.boxController)
			{
				if (this.sleepTimer >= this.dreamTime)
				{
					this.Dream();
					return;
				}
				if (this.sleepTimer >= this.sleepTime)
				{
					this.Sleep();
					return;
				}
			}
		}
		else
		{
			if (Game_Manager.instance.GetGameMode() == 1)
			{
				this.animator_.SetBool("Sleeping", false);
				return;
			}
			this.WakeUp();
		}
	}

	// Token: 0x06000115 RID: 277 RVA: 0x0000BAB8 File Offset: 0x00009CB8
	public void Run(bool b)
	{
		this.run = b;
	}

	// Token: 0x06000116 RID: 278 RVA: 0x0000BAC1 File Offset: 0x00009CC1
	public bool GetIsRunning()
	{
		if (EasterEgg_Manager.instance.Get_Is_MiniGaming())
		{
			this.run = true;
		}
		return this.run;
	}

	// Token: 0x06000117 RID: 279 RVA: 0x0000BADC File Offset: 0x00009CDC
	public bool GetPlayerWalking()
	{
		bool result = false;
		if (Mathf.Abs(this.moveVec3.magnitude) > 0.7f)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06000118 RID: 280 RVA: 0x0000BB05 File Offset: 0x00009D05
	public bool HoldBox(Box_Controller _box)
	{
		if (this.boxController != null)
		{
			return false;
		}
		this.boxController = _box;
		this.boxController.HoldBox(this.boxHolder, true, null, this.cart_Controller);
		Menu_Manager.instance.RefreshMainHints();
		return true;
	}

	// Token: 0x06000119 RID: 281 RVA: 0x0000BB44 File Offset: 0x00009D44
	public bool HoldOrChangeBox(Box_Controller _box)
	{
		if (this.boxController != null)
		{
			this.boxController.ChangeQnt(_box.prodQnt, _box.lifeSpanIndex, this.playerIndex);
			_box.DeleteBox(this.playerIndex);
			return true;
		}
		this.boxController = _box;
		this.boxController.HoldBox(this.boxHolder, true, null, this.cart_Controller);
		Menu_Manager.instance.RefreshMainHints();
		return true;
	}

	// Token: 0x0600011A RID: 282 RVA: 0x0000BBB8 File Offset: 0x00009DB8
	public void ThrowBox()
	{
		this.Set_May_Shoot_False();
		if (this.boxController == null)
		{
			return;
		}
		Box_Controller box_Controller = this.boxController;
		if (this.cart_Controller)
		{
			this.cart_Controller.RemoveBox(box_Controller);
		}
		box_Controller.ThrowBox();
		if (box_Controller == this.boxController)
		{
			this.boxController = null;
		}
		this.interactionTimeOut_Timer = 0f;
		Menu_Manager.instance.RefreshMainHints();
	}

	// Token: 0x0600011B RID: 283 RVA: 0x0000BC2C File Offset: 0x00009E2C
	public void TrashBox()
	{
		if (this.boxController == null)
		{
			return;
		}
		Box_Controller box_Controller = this.boxController;
		if (this.cart_Controller)
		{
			this.cart_Controller.RemoveBox(box_Controller);
		}
		box_Controller.DeleteBox(this.playerIndex);
		if (box_Controller == this.boxController)
		{
			this.boxController = null;
		}
		Menu_Manager.instance.RefreshMainHints();
	}

	// Token: 0x0600011C RID: 284 RVA: 0x0000BC94 File Offset: 0x00009E94
	public void StoreBox(GameObject _boxHolder, Shelf_Controller _shelf)
	{
		if (this.boxController == null)
		{
			return;
		}
		Box_Controller box_Controller = this.boxController;
		this.RemoveInteractive(box_Controller.gameObject.GetComponent<Interaction_Controller>());
		if (this.cart_Controller)
		{
			this.cart_Controller.RemoveBox(box_Controller);
		}
		Debug.Log(box_Controller);
		box_Controller.HoldBox(_boxHolder, false, _shelf, null);
		if (box_Controller == this.boxController)
		{
			this.boxController = null;
		}
		Menu_Manager.instance.RefreshMainHints();
	}

	// Token: 0x0600011D RID: 285 RVA: 0x0000BD10 File Offset: 0x00009F10
	public void RemoveBox()
	{
		if (this.boxController == null)
		{
			return;
		}
		Box_Controller box_Controller = this.boxController;
		if (this.cart_Controller)
		{
			this.cart_Controller.RemoveBox(box_Controller);
		}
		if (box_Controller == this.boxController)
		{
			this.boxController = null;
		}
		Menu_Manager.instance.RefreshMainHints();
	}

	// Token: 0x0600011E RID: 286 RVA: 0x0000BD6B File Offset: 0x00009F6B
	public void GiveProdFromBox()
	{
		if (this.boxController == null)
		{
			return;
		}
		this.boxController.ChangeQnt(-1, this.boxController.lifeSpanIndex, this.playerIndex);
		Menu_Manager.instance.RefreshMainHints();
	}

	// Token: 0x0600011F RID: 287 RVA: 0x0000BDA4 File Offset: 0x00009FA4
	public void CreateDiscountBlock(int _discountLevel)
	{
		if (this.boxController)
		{
			return;
		}
		this.DestroyDiscountPaper();
		this.discountPaper_Controller = Inv_Manager.instance.CreateDiscountPaper(true);
		this.discountPaper_Controller.Change_DiscountPaper(_discountLevel);
		this.discountPaper_Controller.HoldPaper(this.boxHolder);
		Menu_Manager.instance.RefreshMainHints();
	}

	// Token: 0x06000120 RID: 288 RVA: 0x0000BDFD File Offset: 0x00009FFD
	public void DestroyDiscountPaper()
	{
		if (this.discountPaper_Controller)
		{
			this.discountPaper_Controller.DestroyPaper();
			this.discountPaper_Controller = null;
		}
		Menu_Manager.instance.RefreshMainHints();
	}

	// Token: 0x06000121 RID: 289 RVA: 0x0000BE28 File Offset: 0x0000A028
	public void SetToolsActive(bool _active)
	{
		this.tools_Animator.gameObject.SetActive(_active);
		if (_active)
		{
			this.tools_Controller = this.tools_Animator.gameObject;
		}
		else
		{
			this.tools_Controller = null;
		}
		Menu_Manager.instance.RefreshMainHints();
	}

	// Token: 0x06000122 RID: 290 RVA: 0x0000BE64 File Offset: 0x0000A064
	public void SetCartActive(bool _active)
	{
		this.cart_Animator.gameObject.SetActive(_active);
		if (_active)
		{
			this.cart_Controller = this.cart_Animator.gameObject.GetComponent<Cart_Controller>();
			this.cart_Controller.RefreshBoxes();
		}
		else
		{
			this.cart_Controller = null;
		}
		Menu_Manager.instance.RefreshMainHints();
	}

	// Token: 0x06000123 RID: 291 RVA: 0x0000BEB9 File Offset: 0x0000A0B9
	public void ChangeCurrentControllerIndex(int _dir = 1)
	{
		if (this.cart_Controller != null)
		{
			this.cart_Controller.ChangeCartIndex(_dir);
			return;
		}
		if (this.discountPaper_Controller != null)
		{
			this.discountPaper_Controller.Change_DiscountPaper(_dir);
		}
	}

	// Token: 0x06000124 RID: 292 RVA: 0x0000BEF0 File Offset: 0x0000A0F0
	public void SetKeyActive(bool _active)
	{
		this.key_Animator.gameObject.SetActive(_active);
		if (_active)
		{
			this.key_Controller = this.key_Animator.gameObject;
		}
		else
		{
			this.key_Controller = null;
		}
		Menu_Manager.instance.RefreshMainHints();
	}

	// Token: 0x06000125 RID: 293 RVA: 0x0000BF2C File Offset: 0x0000A12C
	public bool GetIsEmpty()
	{
		bool result = true;
		if (this.boxController)
		{
			result = false;
		}
		else if (this.discountPaper_Controller)
		{
			result = false;
		}
		else if (this.tools_Controller)
		{
			result = false;
		}
		else if (this.key_Controller)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x0000BF80 File Offset: 0x0000A180
	public bool GetIsEmptyOrHasSameShelfBox(int _index)
	{
		bool result = true;
		if (this.boxController)
		{
			if (this.boxController.prodQnt >= 8)
			{
				result = false;
			}
			else if (!this.boxController.isShelf)
			{
				result = false;
			}
			else if (_index != this.boxController.itemIndex)
			{
				result = false;
			}
		}
		else if (this.discountPaper_Controller)
		{
			result = false;
		}
		else if (this.tools_Controller)
		{
			result = false;
		}
		else if (this.key_Controller)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06000127 RID: 295 RVA: 0x0000C004 File Offset: 0x0000A204
	public bool GetIsEmptyOrHasSameDecorBox(int _index)
	{
		bool result = true;
		if (this.boxController)
		{
			if (this.boxController.prodQnt >= 8)
			{
				result = false;
			}
			else if (!this.boxController.isDecor)
			{
				result = false;
			}
			else if (_index != this.boxController.itemIndex)
			{
				result = false;
			}
		}
		else if (this.discountPaper_Controller)
		{
			result = false;
		}
		else if (this.tools_Controller)
		{
			result = false;
		}
		else if (this.key_Controller)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06000128 RID: 296 RVA: 0x0000C088 File Offset: 0x0000A288
	public bool GetIsEmptyOrHasSameUtilBox(int _index)
	{
		bool result = true;
		if (this.boxController)
		{
			if (this.boxController.prodQnt >= 8)
			{
				result = false;
			}
			else if (!this.boxController.isUtil)
			{
				result = false;
			}
			else if (_index != this.boxController.itemIndex)
			{
				result = false;
			}
		}
		else if (this.discountPaper_Controller)
		{
			result = false;
		}
		else if (this.tools_Controller)
		{
			result = false;
		}
		else if (this.key_Controller)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06000129 RID: 297 RVA: 0x0000C10C File Offset: 0x0000A30C
	public void SetCashier(Cashier_Controller _cashier)
	{
		if (this.cashier_Controller == _cashier)
		{
			return;
		}
		this.cashier_Controller = _cashier;
		Menu_Manager.instance.RefreshMainHints();
		if (_cashier)
		{
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_Cashier_RightButton);
			return;
		}
		Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_Cashier_WrongButton);
	}

	// Token: 0x0600012A RID: 298 RVA: 0x0000C16C File Offset: 0x0000A36C
	private void UpdateAutoMove()
	{
		if (this.cashier_Controller || this.locker_Controller)
		{
			Transform transform = null;
			if (this.cashier_Controller)
			{
				transform = this.cashier_Controller.operatorPlace.transform;
			}
			else if (this.locker_Controller)
			{
				transform = this.locker_Controller.operatorPlace.transform;
			}
			if (!transform)
			{
				return;
			}
			if (Vector3.Distance(base.transform.position, transform.position) > 0.2f)
			{
				Vector3 position = transform.position;
				position.y = base.transform.position.y;
				base.transform.position = Vector3.Lerp(base.transform.position, position, this.moveSpeedWalk * Time.unscaledDeltaTime);
			}
			else
			{
				this.moveVec3 = Vector3.zero;
			}
			if (Quaternion.Angle(base.transform.rotation, transform.rotation) > 5f)
			{
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, transform.rotation, this.rotSpeedWalk * Time.unscaledDeltaTime);
			}
		}
	}

	// Token: 0x0600012B RID: 299 RVA: 0x0000C299 File Offset: 0x0000A499
	public void SetNewPos(Vector3 _newPos)
	{
		if (_newPos.y >= 1.3f)
		{
			this.ResetPos();
			return;
		}
		base.transform.position = _newPos;
		this.SetToolsActive(false);
		this.SetCartActive(false);
		this.SetKeyActive(false);
	}

	// Token: 0x0600012C RID: 300 RVA: 0x0000C2D0 File Offset: 0x0000A4D0
	public void ResetPos()
	{
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.Euler(Vector3.zero);
		base.transform.localScale = Vector3.one;
		this.SetToolsActive(false);
		this.SetCartActive(false);
		this.SetKeyActive(false);
	}

	// Token: 0x0600012D RID: 301 RVA: 0x0000C328 File Offset: 0x0000A528
	public void BackToMartRandomPos()
	{
		List<GameObject> list = new List<GameObject>(Nav_Manager.instance.GetActiveNavSpheres());
		Vector3 position = list[UnityEngine.Random.Range(0, list.Count)].transform.position;
		position.y = 1f;
		base.transform.position = position;
		base.transform.localRotation = Quaternion.Euler(Vector3.zero);
		base.transform.localScale = Vector3.one;
		this.SetToolsActive(false);
		this.SetCartActive(false);
		this.SetKeyActive(false);
		this.Set_Multiplayer_Color(Color.white);
	}

	// Token: 0x0600012E RID: 302 RVA: 0x0000C3BF File Offset: 0x0000A5BF
	public void WakeUp()
	{
		this.sleepStage = 0;
		this.sleepTimer = 0f;
		this.animator_.SetBool("Sleeping", false);
	}

	// Token: 0x0600012F RID: 303 RVA: 0x0000C3E4 File Offset: 0x0000A5E4
	public void Sleep()
	{
		if (this.sleepStage == 0)
		{
			this.sleepStage = 1;
			Save_Manager.instance.SaveGame();
			this.animator_.SetBool("Sleeping", true);
		}
	}

	// Token: 0x06000130 RID: 304 RVA: 0x0000C410 File Offset: 0x0000A610
	public void Dream()
	{
		if (this.sleepStage == 1)
		{
			this.sleepStage = 2;
			Game_Manager.instance.SetGameMode(1);
		}
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0000C42D File Offset: 0x0000A62D
	private void EE_Start_Update()
	{
		if (this.ee_start_shoot_timer_current <= 0f)
		{
			this.ee_start_shoot_qnt_current = 0;
			return;
		}
		if (this.ee_start_shoot_timer_current >= 0f)
		{
			this.ee_start_shoot_timer_current -= Time.unscaledDeltaTime;
		}
	}

	// Token: 0x06000132 RID: 306 RVA: 0x0000C464 File Offset: 0x0000A664
	private void EE_Increase_Start_Times(bool _increase = true)
	{
		if (!_increase)
		{
			this.ee_start_shoot_timer_current = 0f;
			return;
		}
		this.ee_start_shoot_timer_current = this.ee_start_shoot_timer_time;
		this.ee_start_shoot_qnt_current++;
		if (this.ee_start_shoot_qnt_current >= this.ee_start_shoot_qnt_needed)
		{
			this.ee_start_shoot_qnt_current = 0;
			this.interactive_Controllers.Clear();
			EasterEgg_Manager.instance.Start_Soccer(true);
		}
	}

	// Token: 0x06000133 RID: 307 RVA: 0x0000C4C8 File Offset: 0x0000A6C8
	public void Shoot(GameObject _gameObject)
	{
		if (this.may_shoot && this.GetIsRunning() && this.is_dashing)
		{
			Vector3 position = base.transform.position;
			Vector3 position2 = _gameObject.transform.position;
			position.y = 0f;
			position2.y = 0f;
			if (Vector3.Distance(position, position2) > this.shoot_distance)
			{
				return;
			}
			this.may_shoot = false;
			Vector3 vector = position2 - position;
			float d = this.shoot_forward_force / Mathf.Clamp(_gameObject.transform.position.y, this.shoot_upward_minHight, this.shoot_upward_maxHight);
			float y = this.shoot_upward_force * Mathf.Clamp(_gameObject.transform.position.y, this.shoot_upward_minHight, this.shoot_upward_maxHight);
			Vector3 force = vector.normalized * d;
			force.y = y;
			Rigidbody component = _gameObject.GetComponent<Rigidbody>();
			Vector3 velocity = component.velocity;
			velocity.x = 0f;
			velocity.y = 0f;
			component.velocity = velocity;
			component.AddForce(force, ForceMode.Impulse);
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_soccer_shot);
			Invoker.InvokeDelayed(new Invokable(this.Set_May_Shoot), this.shoot_may_shoot_time);
		}
	}

	// Token: 0x06000134 RID: 308 RVA: 0x0000C611 File Offset: 0x0000A811
	public void Set_May_Shoot()
	{
		this.may_shoot = true;
	}

	// Token: 0x06000135 RID: 309 RVA: 0x0000C61A File Offset: 0x0000A81A
	public void Set_May_Shoot_False()
	{
		this.may_shoot = false;
		Invoker.InvokeDelayed(new Invokable(this.Set_May_Shoot), this.shoot_may_shoot_time);
	}

	// Token: 0x06000136 RID: 310 RVA: 0x0000C63C File Offset: 0x0000A83C
	public void UseInteractive(bool _holdingButton, bool _b0, bool _b1, bool _b2, bool _b3)
	{
		if (Cheat_Manager.instance.GetFreezeTimeScale() == 1f)
		{
			return;
		}
		if (Interactor_Manager.instance.ui_ctrls[this.playerIndex].miniGame_manager.GetIsMiniGaming())
		{
			if (_b0)
			{
				Interactor_Manager.instance.ui_ctrls[this.playerIndex].miniGame_manager.MiniGame_Inputs(_holdingButton, 0);
				return;
			}
			if (_b1)
			{
				Interactor_Manager.instance.ui_ctrls[this.playerIndex].miniGame_manager.MiniGame_Inputs(_holdingButton, 1);
				return;
			}
			if (_b2)
			{
				Interactor_Manager.instance.ui_ctrls[this.playerIndex].miniGame_manager.MiniGame_Inputs(_holdingButton, 2);
				return;
			}
			if (_b3)
			{
				Interactor_Manager.instance.ui_ctrls[this.playerIndex].miniGame_manager.MiniGame_Inputs(_holdingButton, 3);
			}
			return;
		}
		else
		{
			if (!this.GetPlayerWalking())
			{
				if (this.boxController && _b3)
				{
					this.ThrowBox();
				}
				else if (this.discountPaper_Controller && _b3)
				{
					this.DestroyDiscountPaper();
				}
				else if (this.tools_Controller && _b3)
				{
					this.SetToolsActive(false);
				}
				else if (this.cart_Controller && this.cart_Controller.box_controllers[0] == null && _b3)
				{
					this.SetCartActive(false);
				}
			}
			if (this.GetPlayerWalking())
			{
				return;
			}
			Interactor_Manager.instance.RefreshInputHints(this.playerIndex);
			GameObject placePoint = Interactor_Manager.instance.GetPlacePoint(this.playerIndex);
			if (this.boxController && _b0 && placePoint && !_holdingButton)
			{
				if (this.boxController.isShelf)
				{
					this.boxController.PlaceItem(placePoint, this.playerIndex);
					return;
				}
				if (this.boxController.isDecor)
				{
					this.boxController.PlaceItem(placePoint, this.playerIndex);
					return;
				}
				if (this.boxController.isWall)
				{
					this.boxController.ChangeItemWallPaint(placePoint.GetComponent<WallPaint_Controller>(), this.playerIndex);
					return;
				}
				if (this.boxController.isFloor)
				{
					this.boxController.ChangeItemFloor(placePoint.GetComponent<Floor_Controller>(), this.playerIndex);
					return;
				}
				if (this.boxController.isUtil)
				{
					this.boxController.PlaceItem(placePoint, this.playerIndex);
					return;
				}
			}
			if (!_holdingButton)
			{
				if (this.cashier_Controller && _b3)
				{
					this.cashier_Controller.RemoveOperator();
				}
				else if (this.cashier_Controller)
				{
					if (_b0)
					{
						this.cashier_Controller.PassProd(0, this.playerIndex);
						return;
					}
					if (_b1)
					{
						this.cashier_Controller.PassProd(1, this.playerIndex);
						return;
					}
					if (_b2)
					{
						this.cashier_Controller.PassProd(2, this.playerIndex);
					}
					return;
				}
			}
			if (!this.currentInterative)
			{
				return;
			}
			if (this.currentInterative.isChar && Cheat_Manager.instance.GetFreezeTimeScale() == 1f)
			{
				return;
			}
			if (this.currentInterative.isShelf && this.boxController && this.currentInterative.GetComponent<Shelf_Controller>().isShelfProd && (this.boxController.isWall || this.boxController.isFloor))
			{
				return;
			}
			if (Interactor_Manager.instance.GetPlacePoint(this.playerIndex) != null && this.boxController)
			{
				return;
			}
			if (_b0)
			{
				this.currentInterative.Interact(_holdingButton, 0, this);
				return;
			}
			if (_b1)
			{
				this.currentInterative.Interact(_holdingButton, 1, this);
				return;
			}
			if (_b2)
			{
				this.currentInterative.Interact(_holdingButton, 2, this);
			}
			return;
		}
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0000C9D0 File Offset: 0x0000ABD0
	private void AddInteractive(Interaction_Controller _interactive)
	{
		if (!this.interactive_Controllers.Contains(_interactive))
		{
			this.interactive_Controllers.Add(_interactive);
		}
	}

	// Token: 0x06000138 RID: 312 RVA: 0x0000C9EC File Offset: 0x0000ABEC
	private void RemoveInteractive(Interaction_Controller _interactive)
	{
		if (this.interactive_Controllers.Contains(_interactive))
		{
			this.interactive_Controllers.Remove(_interactive);
		}
	}

	// Token: 0x06000139 RID: 313 RVA: 0x0000CA0C File Offset: 0x0000AC0C
	public Vector3 Get_Interaction_Position()
	{
		Vector3 vector = base.transform.position + base.transform.forward * this.forward_interaction_position;
		if (this.interaction_pos_debug_cube != null)
		{
			this.interaction_pos_debug_cube.transform.position = vector;
		}
		return vector;
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000CA60 File Offset: 0x0000AC60
	private void SortInteractives()
	{
		if (Interactor_Manager.instance.ui_ctrls[this.playerIndex].miniGame_manager.GetIsMiniGaming())
		{
			this.currentInterative = null;
		}
		else if (this.cashier_Controller)
		{
			this.currentInterative = null;
		}
		else if (this.interactive_Controllers.Count > 0)
		{
			Vector3 position = base.transform.position;
			position.y = 0f;
			if (this.interactive_Controllers.Count == 1 && this.boxController)
			{
				if (this.interactive_Controllers[0])
				{
					if (this.interactive_Controllers[0].gameObject == this.boxController.gameObject)
					{
						this.currentInterative = null;
					}
				}
				else
				{
					this.interactive_Controllers.RemoveAt(0);
				}
			}
			float num = 1000f;
			for (int i = 0; i < this.interactive_Controllers.Count; i++)
			{
				Interaction_Controller interaction_Controller = this.interactive_Controllers[i];
				if (this.interactive_Controllers[i])
				{
					if (this.interactive_Controllers[i].isBox && this.interactive_Controllers[i].box_Controller.cart_controller != null)
					{
						this.interactive_Controllers[i] = null;
					}
					else
					{
						if (interaction_Controller.isBox && interaction_Controller.box_Controller.isBall && this.GetIsRunning() && this.may_shoot)
						{
							this.EE_Increase_Start_Times(true);
							this.Set_May_Shoot_False();
						}
						if (interaction_Controller.isBall)
						{
							this.Shoot(interaction_Controller.gameObject);
						}
						Vector3 from = this.interactive_Controllers[i].transform.position - base.transform.position;
						from.y = 0f;
						Vector3 forward = base.transform.forward;
						forward.y = 0f;
						float num2 = math.abs(Vector3.Angle(from, forward));
						float num3 = Vector3.Distance(this.Get_Interaction_Position(), this.interactive_Controllers[i].transform.position);
						if (this.interactive_Controllers[i].inter_position != null && Vector3.Distance(position, this.interactive_Controllers[i].inter_position.transform.position) <= this.interaction_max_distance)
						{
							num3 = 0f;
						}
						if (num3 < num && num2 <= this.interactionAngle)
						{
							this.angleDiference = num2;
							if (this.boxController)
							{
								if (this.boxController.gameObject != this.interactive_Controllers[i].gameObject)
								{
									num = num3;
									this.currentInterative = this.interactive_Controllers[i];
								}
							}
							else
							{
								num = num3;
								this.currentInterative = this.interactive_Controllers[i];
							}
						}
						if (this.currentInterative == this.interactive_Controllers[i])
						{
						}
					}
				}
				else
				{
					this.interactive_Controllers.RemoveAt(i);
				}
			}
			if (this.currentInterative)
			{
				if (this.currentInterative.inter_position != null && Vector3.Distance(position, this.currentInterative.inter_position.transform.position) > this.interaction_max_distance)
				{
					this.currentInterative = null;
				}
				else if (Game_Manager.instance.tutorialHappening && (!this.currentInterative.isChar || (this.currentInterative.isChar && this.currentInterative.char_Controller.moving)))
				{
					this.currentInterative = null;
				}
				else if (this.cashier_Controller)
				{
					this.currentInterative = null;
				}
				else if (!this.boxController && !this.discountPaper_Controller && !this.tools_Controller && !this.key_Controller && !this.currentInterative.isChar && !this.cart_Controller)
				{
					if (this.currentInterative.isTrash)
					{
						this.currentInterative = null;
					}
					else if (this.currentInterative.isBroken && !this.tools_Controller)
					{
						this.currentInterative = null;
					}
					else if (this.currentInterative.isLock && this.currentInterative.lock_Controller.isLocked)
					{
						this.currentInterative = null;
					}
				}
				else if (this.boxController)
				{
					if (!this.currentInterative.isShelf && !this.currentInterative.isTrash && !this.currentInterative.isDecor && !this.currentInterative.isChar && !this.currentInterative.isUtil && !this.currentInterative.isBox)
					{
						this.currentInterative = null;
					}
					else if (this.currentInterative.isBox && this.currentInterative.box_Controller.GetBoxType() != this.boxController.GetBoxType())
					{
						this.currentInterative = null;
					}
					else if (this.currentInterative.isBox && this.currentInterative.box_Controller.itemIndex != this.boxController.itemIndex)
					{
						this.currentInterative = null;
					}
					else if (this.currentInterative.isBox && this.currentInterative.box_Controller.prodQnt > 8 - this.boxController.prodQnt)
					{
						this.currentInterative = null;
					}
					else if (this.currentInterative.isChar && !this.GetPlayerWalking())
					{
						Char_Controller char_Controller = this.currentInterative.char_Controller;
						Customer_Controller customer_Controller = char_Controller.customer_Controller;
						if (char_Controller.isGenCustomer)
						{
							if (this.boxController.isProd && customer_Controller.prodWantedNow_Index == this.boxController.itemIndex && !customer_Controller.doneShopping)
							{
								char_Controller.SetPlayerController(this);
							}
							else
							{
								this.currentInterative = null;
							}
						}
						else if (char_Controller.isCustomer)
						{
							char_Controller.SetPlayerController(this);
						}
					}
					else if (this.currentInterative.isShelf && this.currentInterative.shelf_Controller.isShelfProd && this.boxController.frozen)
					{
						this.currentInterative = null;
					}
					if (this.currentInterative && this.currentInterative.isUtil)
					{
						Util_Controller util_Controller = this.currentInterative.util_Controller;
						if (this.boxController.isProd && !util_Controller.CheckItemAcceptance(this.boxController))
						{
							this.currentInterative = null;
						}
					}
				}
				else if (this.cart_Controller)
				{
					if (!this.currentInterative.isBox && !this.currentInterative.isShelf && !this.currentInterative.isTrash && !this.currentInterative.isDecor && !this.currentInterative.isChar && !this.currentInterative.isUtil)
					{
						this.currentInterative = null;
					}
				}
				else if (this.discountPaper_Controller)
				{
					if (!this.currentInterative.isShelf)
					{
						this.currentInterative = null;
					}
				}
				else if (this.tools_Controller)
				{
					if (!this.currentInterative.isDecorPlant && !this.currentInterative.isBroken)
					{
						this.currentInterative = null;
					}
				}
				else if (this.currentInterative.isChar && this.currentInterative.char_Controller.isCustomer && !this.GetPlayerWalking())
				{
					this.currentInterative.char_Controller.SetPlayerController(this);
				}
				else if (this.currentInterative.isChar && this.currentInterative.char_Controller.isGenCustomer)
				{
					this.currentInterative = null;
				}
				else if (this.key_Controller && !this.currentInterative.isLock)
				{
					this.currentInterative = null;
				}
			}
		}
		else
		{
			this.currentInterative = null;
		}
		this.interactionTimeOut_Timer += Time.deltaTime;
		if (this.interactionTimeOut_Timer < this.interactionTimeOut)
		{
			this.currentInterative = null;
		}
		Interactor_Manager.instance.SetInteractive(this.currentInterative, this.playerIndex);
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0000D2B0 File Offset: 0x0000B4B0
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Interactive")
		{
			this.AddInteractive(other.gameObject.GetComponent<Interaction_Controller>());
		}
	}

	// Token: 0x0600013C RID: 316 RVA: 0x0000D2DA File Offset: 0x0000B4DA
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Interactive")
		{
			this.RemoveInteractive(other.gameObject.GetComponent<Interaction_Controller>());
		}
	}

	// Token: 0x0600013D RID: 317 RVA: 0x0000D304 File Offset: 0x0000B504
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Door")
		{
			other.GetComponent<Door_Controller>().OpenDoor(base.transform.position, true);
		}
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0000D334 File Offset: 0x0000B534
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Killer")
		{
			Debug.Log("Killer");
			Game_Manager.instance.SetGameMode(0);
		}
	}

	// Token: 0x0600013F RID: 319 RVA: 0x0000D362 File Offset: 0x0000B562
	public void SetPlayerMeshVisibility(bool _bool)
	{
		this.playerMesh.SetActive(_bool);
	}

	// Token: 0x06000140 RID: 320 RVA: 0x0000D370 File Offset: 0x0000B570
	public bool Get_Is_EE_Game()
	{
		return EasterEgg_Manager.instance != null && EasterEgg_Manager.instance.Get_Is_MiniGaming();
	}

	// Token: 0x0400018D RID: 397
	public float debug_world_y_pos;

	// Token: 0x0400018E RID: 398
	public int playerIndex;

	// Token: 0x0400018F RID: 399
	private Rigidbody rigidbody_;

	// Token: 0x04000190 RID: 400
	[SerializeField]
	public Animator animator_;

	// Token: 0x04000191 RID: 401
	[SerializeField]
	public Animator animator_eyes;

	// Token: 0x04000192 RID: 402
	private Camera_Controller camera_Controller;

	// Token: 0x04000193 RID: 403
	[SerializeField]
	public GameObject cameraHolder;

	// Token: 0x04000194 RID: 404
	[SerializeField]
	public CapsuleCollider capsuleCollider;

	// Token: 0x04000195 RID: 405
	[SerializeField]
	public Camera camera_player_char;

	// Token: 0x04000196 RID: 406
	[SerializeField]
	private MeshRenderer multiplayerColorMesh;

	// Token: 0x04000197 RID: 407
	public Skin_Controller skin_;

	// Token: 0x04000198 RID: 408
	public Locker_Controller locker_Controller;

	// Token: 0x04000199 RID: 409
	public bool isMiniGaming;

	// Token: 0x0400019A RID: 410
	public bool mayUpdate;

	// Token: 0x0400019B RID: 411
	[Header("Movement")]
	[SerializeField]
	private float moveSpeedWalk;

	// Token: 0x0400019C RID: 412
	[SerializeField]
	private float moveSpeedRun;

	// Token: 0x0400019D RID: 413
	[SerializeField]
	private float rotSpeedWalk;

	// Token: 0x0400019E RID: 414
	[SerializeField]
	private float rotSpeedRun;

	// Token: 0x0400019F RID: 415
	[SerializeField]
	private float leanLimitWalk;

	// Token: 0x040001A0 RID: 416
	[SerializeField]
	private float leanLimitRun;

	// Token: 0x040001A1 RID: 417
	[SerializeField]
	private float gravitySpeed;

	// Token: 0x040001A2 RID: 418
	[SerializeField]
	private float jumpSpeed;

	// Token: 0x040001A3 RID: 419
	private bool isGrounded;

	// Token: 0x040001A4 RID: 420
	private Vector3 playerVelocity;

	// Token: 0x040001A5 RID: 421
	[SerializeField]
	private ParticleSystem runParticles;

	// Token: 0x040001A6 RID: 422
	[Header("Dash")]
	[SerializeField]
	private float moveSpeedDash;

	// Token: 0x040001A7 RID: 423
	[SerializeField]
	private float dash_recover_time = 1f;

	// Token: 0x040001A8 RID: 424
	public bool is_dashing;

	// Token: 0x040001A9 RID: 425
	private Vector3 moveVec3;

	// Token: 0x040001AA RID: 426
	private bool run;

	// Token: 0x040001AB RID: 427
	[Header("BOX")]
	[SerializeField]
	public GameObject boxHolder;

	// Token: 0x040001AC RID: 428
	[SerializeField]
	public Box_Controller boxController;

	// Token: 0x040001AD RID: 429
	[Header("Discount System")]
	[SerializeField]
	public DiscountPaper_Controller discountPaper_Controller;

	// Token: 0x040001AE RID: 430
	[Header("Tools")]
	[SerializeField]
	public Animator tools_Animator;

	// Token: 0x040001AF RID: 431
	[HideInInspector]
	public GameObject tools_Controller;

	// Token: 0x040001B0 RID: 432
	[Header("Cart")]
	[SerializeField]
	public Animator cart_Animator;

	// Token: 0x040001B1 RID: 433
	[HideInInspector]
	public Cart_Controller cart_Controller;

	// Token: 0x040001B2 RID: 434
	[Header("Key")]
	[SerializeField]
	public Animator key_Animator;

	// Token: 0x040001B3 RID: 435
	[SerializeField]
	public GameObject key_Controller;

	// Token: 0x040001B4 RID: 436
	[Header("Cashier System")]
	[SerializeField]
	public Cashier_Controller cashier_Controller;

	// Token: 0x040001B5 RID: 437
	public int sleepStage;

	// Token: 0x040001B6 RID: 438
	public float sleepTimer;

	// Token: 0x040001B7 RID: 439
	private float sleepTime = 120f;

	// Token: 0x040001B8 RID: 440
	private float dreamTime = 120f;

	// Token: 0x040001B9 RID: 441
	public bool acceptedCloseMart;

	// Token: 0x040001BA RID: 442
	private int ee_start_shoot_qnt_current;

	// Token: 0x040001BB RID: 443
	private int ee_start_shoot_qnt_needed = 5;

	// Token: 0x040001BC RID: 444
	private float ee_start_shoot_timer_current;

	// Token: 0x040001BD RID: 445
	private float ee_start_shoot_timer_time = 5f;

	// Token: 0x040001BE RID: 446
	[Header("Shoot")]
	[SerializeField]
	private float shoot_forward_force = 15f;

	// Token: 0x040001BF RID: 447
	[SerializeField]
	private float shoot_upward_force = 10f;

	// Token: 0x040001C0 RID: 448
	[SerializeField]
	private float shoot_upward_minHight = 0.3f;

	// Token: 0x040001C1 RID: 449
	[SerializeField]
	private float shoot_upward_maxHight = 1.5f;

	// Token: 0x040001C2 RID: 450
	[SerializeField]
	private float shoot_distance = 1f;

	// Token: 0x040001C3 RID: 451
	[SerializeField]
	private float shoot_may_shoot_time = 0.25f;

	// Token: 0x040001C4 RID: 452
	[SerializeField]
	private float shoot_impulse_forward_force = 5f;

	// Token: 0x040001C5 RID: 453
	[SerializeField]
	private float shoot_impulse_updward_force;

	// Token: 0x040001C6 RID: 454
	private bool may_shoot = true;

	// Token: 0x040001C7 RID: 455
	[Header("Interactives")]
	[SerializeField]
	private List<Interaction_Controller> interactive_Controllers = new List<Interaction_Controller>();

	// Token: 0x040001C8 RID: 456
	[SerializeField]
	public Interaction_Controller currentInterative;

	// Token: 0x040001C9 RID: 457
	[SerializeField]
	private GameObject currentIntPlacePoint;

	// Token: 0x040001CA RID: 458
	private float interactionTimeOut_Timer;

	// Token: 0x040001CB RID: 459
	private float interactionTimeOut = 0.05f;

	// Token: 0x040001CC RID: 460
	public float interactionAngle = 90f;

	// Token: 0x040001CD RID: 461
	private float interactionAngleMoving = 40f;

	// Token: 0x040001CE RID: 462
	public float angleDiference;

	// Token: 0x040001CF RID: 463
	public float forward_interaction_position = 1f;

	// Token: 0x040001D0 RID: 464
	public float interaction_max_angle = 40f;

	// Token: 0x040001D1 RID: 465
	public float interaction_max_distance = 1f;

	// Token: 0x040001D2 RID: 466
	public GameObject interaction_pos_debug_cube;

	// Token: 0x040001D3 RID: 467
	[SerializeField]
	private GameObject playerMesh;
}
