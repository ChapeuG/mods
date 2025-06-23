using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002B RID: 43
public class Staff_Controller : MonoBehaviour
{
	// Token: 0x06000186 RID: 390 RVA: 0x0000F2F4 File Offset: 0x0000D4F4
	public Staff_SaveData GetSaveData()
	{
		Staff_SaveData result;
		result.id = Char_Manager.instance.staff_Controllers.IndexOf(base.gameObject.GetComponent<Char_Controller>());
		result.position = base.transform.position;
		return result;
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000F33A File Offset: 0x0000D53A
	private void Awake()
	{
		this.char_Controller = base.GetComponent<Char_Controller>();
		this.char_Controller.moveSpeedRun = this.char_Controller.moveSpeedWalk * 2f;
		this.skin_Controller = base.GetComponent<Skin_Controller>();
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000F370 File Offset: 0x0000D570
	private void Start()
	{
		base.GetComponent<Skin_Controller>().SetCompleteCustomization(this.staffData.skinMat, Player_Manager.instance.GetPlayerController(0).skin_.clothesMat_Index, this.staffData.hairMat, this.staffData.hairMesh, Player_Manager.instance.GetPlayerController(0).skin_.hatMat_Index, this.staffData.eyeMat, Player_Manager.instance.GetPlayerController(0).skin_.hatGo_Index);
		this.SetCooldown(1f, Staff_Controller.SkillType.NONE);
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0000F3FF File Offset: 0x0000D5FF
	private void Update()
	{
		if (!Game_Manager.instance)
		{
			return;
		}
		this.UpdateAI();
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000F414 File Offset: 0x0000D614
	public bool GetIsEmpty()
	{
		return this.boxController == null;
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000F424 File Offset: 0x0000D624
	public bool HoldOrChangeBox(Box_Controller _box)
	{
		if (_box == null)
		{
			return false;
		}
		if (!(this.boxController != null))
		{
			this.boxController = _box;
			this.boxController.HoldBox(this.boxHolder, true, null, null);
			return true;
		}
		if (this.boxController.GetBoxType() != _box.GetBoxType())
		{
			return false;
		}
		if (this.boxController.itemIndex != _box.itemIndex)
		{
			return false;
		}
		if (this.boxController.prodQnt >= Inv_Manager.instance.boxSize)
		{
			return false;
		}
		this.boxController.ChangeQnt(_box.prodQnt, _box.lifeSpanIndex, 0);
		_box.DeleteBox(0);
		return true;
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0000F4CA File Offset: 0x0000D6CA
	public void StoreBox(GameObject _boxHolder, Shelf_Controller _shelf)
	{
		if (this.boxController == null)
		{
			return;
		}
		this.boxController.HoldBox(_boxHolder, false, _shelf, null);
		this.boxController = null;
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000F4F4 File Offset: 0x0000D6F4
	public void SetCooldown(float _time, Staff_Controller.SkillType _skill)
	{
		if (_skill != Staff_Controller.SkillType.NONE && _skill != Staff_Controller.SkillType.ALL)
		{
			_skill--;
			float num = 2f;
			float num2 = this.staffData.skills[(int)_skill];
			if (this.staffData.energy < num2 / 2f)
			{
				num2 = this.staffData.energy + this.staffData.skills[(int)_skill] / 2f;
			}
			_time *= num - num2 * num + 1f;
		}
		this.cooldown_Timer = _time;
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000F56B File Offset: 0x0000D76B
	public void SetCashier(Cashier_Controller _cashier)
	{
		this.cashier_Controller = _cashier;
		if (!_cashier)
		{
			this.SetCooldown(0f, Staff_Controller.SkillType.NONE);
		}
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000F588 File Offset: 0x0000D788
	private void UpdateAI()
	{
		if (!Game_Manager.instance.MayRun() && Game_Manager.instance.GetGameMode() != 0)
		{
			return;
		}
		if (this.task != null)
		{
			this.task.Update();
		}
		this.cooldown_Timer -= Time.deltaTime;
		if (this.cooldown_Timer > 0f)
		{
			return;
		}
		if (this.task == null || !this.task.UpdateAI())
		{
			if (this.navPath == null || this.navPath.Count == 0)
			{
				this.FindNewPath();
				return;
			}
			if (this.navPathIndex < this.navPath.Count)
			{
				Vector3 position = base.transform.position;
				position.y = 0f;
				if (Vector3.Distance(position, this.navPath[this.navPathIndex].transform.position) > this.navPathDistance)
				{
					Vector3 normalized = (this.navPath[this.navPathIndex].transform.position - base.transform.position).normalized;
					this.char_Controller.Move(normalized);
					this.char_Controller.copyMove = normalized;
					this.char_Controller.animator_.SetBool("Thinking", false);
					this.char_Controller.findNewPathTime_Timer += Time.deltaTime;
					if (this.char_Controller.findNewPathTime_Timer > this.char_Controller.findNewPathTime * 2f)
					{
						this.FindNewPath();
						return;
					}
				}
				else if (this.NextNavSphere())
				{
					this.char_Controller.Move(Vector3.zero);
					this.char_Controller.copyMove = Vector3.zero;
					this.char_Controller.rigidbody_.velocity = Vector3.zero;
					this.navPath = null;
					if (this.task != null)
					{
						this.task.OnTargetReached();
					}
				}
			}
		}
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000F764 File Offset: 0x0000D964
	public void ClearTask()
	{
		this.task = null;
		this.navPath = null;
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000F774 File Offset: 0x0000D974
	private void FindNewPath()
	{
		this.navPathIndex = 0;
		this.navPath = null;
		this.task = null;
		if (this.boxController != null)
		{
			if (this.boxController.isGarbage)
			{
				this.task = Staff_Controller.Staff_Task_Trash.Create(this);
				if (this.task != null)
				{
					return;
				}
			}
			if (this.boxController.isProd && this.boxController.lifeSpanIndex <= 0 && Inv_Manager.instance.GetProdPrefab(this.boxController.itemIndex).lifeSpan)
			{
				this.task = Staff_Controller.Staff_Task_Trash.Create(this);
				if (this.task != null)
				{
					return;
				}
			}
			this.task = Staff_Controller.Staff_Task_StoreProd.Create(this);
			if (this.task != null)
			{
				return;
			}
			this.task = Staff_Controller.Staff_Task_StoreInv.Create(this);
			if (this.task != null)
			{
				return;
			}
			this.task = Staff_Controller.Staff_Task_ThrowBoxOnInv.Create(this);
			if (this.task != null)
			{
				return;
			}
		}
		if (UnityEngine.Random.value < 0.5f)
		{
			if (this.staffData.tasks[0])
			{
				this.task = Staff_Controller.Staff_Task_Cashier.Create(this);
			}
			if (this.task != null)
			{
				return;
			}
		}
		if (this.staffData.tasks[1] || this.staffData.tasks[2])
		{
			this.task = Staff_Controller.Staff_Task_GetExpiredProd.Create(this);
		}
		if (this.task != null)
		{
			return;
		}
		if (this.staffData.tasks[2])
		{
			this.task = Staff_Controller.Staff_Task_TakeObject.CreateTakeGarbage(this);
		}
		if (this.task != null)
		{
			return;
		}
		if (this.staffData.tasks[2])
		{
			this.task = Staff_Controller.Staff_Task_TakeObject.CreateCleanDirt(this);
		}
		if (this.task != null)
		{
			return;
		}
		if (this.staffData.tasks[1])
		{
			this.task = Staff_Controller.Staff_Task_TakeObject.CreatePickUpBox(this);
		}
		if (this.task != null)
		{
			return;
		}
		if (this.staffData.tasks[1])
		{
			this.task = Staff_Controller.Staff_Task_GetInvBox.Create(this);
		}
		if (this.task != null)
		{
			return;
		}
		this.task = Staff_Controller.Staff_Task_Loiter.Create(this);
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000F954 File Offset: 0x0000DB54
	private bool NextNavSphere()
	{
		if (this.navPath == null || this.navPath.Count == 0)
		{
			return false;
		}
		this.char_Controller.findNewPathTime_Timer = 0f;
		if (this.navPathIndex < this.navPath.Count - 1)
		{
			this.navPathIndex++;
		}
		return this.navPathIndex >= this.navPath.Count - 1;
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000F9C3 File Offset: 0x0000DBC3
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Door")
		{
			other.GetComponent<Door_Controller>().OpenDoor(base.transform.position, false);
		}
	}

	// Token: 0x0400021B RID: 539
	[SerializeField]
	public Staff_Data staffData;

	// Token: 0x0400021C RID: 540
	[HideInInspector]
	public Skin_Controller skin_Controller;

	// Token: 0x0400021D RID: 541
	private Char_Controller char_Controller;

	// Token: 0x0400021E RID: 542
	[SerializeField]
	private List<GameObject> navPath;

	// Token: 0x0400021F RID: 543
	[SerializeField]
	private int navPathIndex;

	// Token: 0x04000220 RID: 544
	private float navPathDistance = 0.6f;

	// Token: 0x04000221 RID: 545
	private float cooldown_Timer;

	// Token: 0x04000222 RID: 546
	public Cashier_Controller cashier_Controller;

	// Token: 0x04000223 RID: 547
	public Staff_Controller.Staff_Task task;

	// Token: 0x04000224 RID: 548
	[Header("BOX")]
	[SerializeField]
	public GameObject boxHolder;

	// Token: 0x04000225 RID: 549
	[SerializeField]
	public Box_Controller boxController;

	// Token: 0x02000068 RID: 104
	public enum SkillType
	{
		// Token: 0x04000672 RID: 1650
		NONE,
		// Token: 0x04000673 RID: 1651
		CASHIER,
		// Token: 0x04000674 RID: 1652
		CLEAN,
		// Token: 0x04000675 RID: 1653
		RESTOCK,
		// Token: 0x04000676 RID: 1654
		ALL
	}

	// Token: 0x02000069 RID: 105
	public class Staff_Task
	{
		// Token: 0x06000559 RID: 1369 RVA: 0x00032815 File Offset: 0x00030A15
		public Staff_Task(Staff_Controller c)
		{
			this.ctrl = c;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00032824 File Offset: 0x00030A24
		public virtual void Update()
		{
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00032826 File Offset: 0x00030A26
		public virtual bool UpdateAI()
		{
			return false;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00032829 File Offset: 0x00030A29
		public virtual void OnTargetReached()
		{
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0003282B File Offset: 0x00030A2B
		public virtual Cashier_Controller GetTargetCashier()
		{
			return null;
		}

		// Token: 0x04000677 RID: 1655
		protected Staff_Controller ctrl;
	}

	// Token: 0x0200006A RID: 106
	public class Staff_Task_Cashier : Staff_Controller.Staff_Task
	{
		// Token: 0x0600055E RID: 1374 RVA: 0x0003282E File Offset: 0x00030A2E
		private Staff_Task_Cashier(Staff_Controller _ctrl, Cashier_Controller _cashier) : base(_ctrl)
		{
			this.target_Cashier = _cashier;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00032840 File Offset: 0x00030A40
		public static Staff_Controller.Staff_Task_Cashier Create(Staff_Controller _ctrl)
		{
			Cashier_Controller cashier = Interactor_Manager.instance.GetCashier(0);
			if (cashier != null && !cashier.GetHasOperator() && Char_Manager.instance.WhoIsGoingToCashier(cashier) == null)
			{
				_ctrl.navPath = Nav_Manager.instance.GetNavPathCashier(cashier, _ctrl.transform.position, true);
				if (_ctrl.navPath != null)
				{
					return new Staff_Controller.Staff_Task_Cashier(_ctrl, cashier);
				}
			}
			return null;
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x000328AB File Offset: 0x00030AAB
		public override Cashier_Controller GetTargetCashier()
		{
			return this.target_Cashier;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x000328B4 File Offset: 0x00030AB4
		public override void Update()
		{
			if (!this.ctrl.cashier_Controller)
			{
				return;
			}
			Transform transform = this.ctrl.cashier_Controller.operatorPlace.transform;
			if (Vector3.Distance(this.ctrl.transform.position, transform.position) > 0.2f)
			{
				Vector3 position = transform.position;
				position.y = this.ctrl.transform.position.y;
				this.ctrl.char_Controller.rigidbody_.MovePosition(Vector3.Lerp(this.ctrl.transform.position, position, this.ctrl.char_Controller.moveSpeedWalk * Time.deltaTime));
			}
			if (Quaternion.Angle(this.ctrl.transform.rotation, transform.rotation) > 5f)
			{
				this.ctrl.transform.rotation = Quaternion.Lerp(this.ctrl.transform.rotation, transform.rotation, this.ctrl.char_Controller.rotSpeedWalk * Time.deltaTime);
			}
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x000329D4 File Offset: 0x00030BD4
		public override bool UpdateAI()
		{
			if (!this.ctrl.cashier_Controller)
			{
				return false;
			}
			if (this.ctrl.cashier_Controller.GetHasClients())
			{
				this.ctrl.cashier_Controller.PassProdStaff(UnityEngine.Random.value < this.ctrl.staffData.skills[0] * 2f, 0);
				this.ctrl.SetCooldown(UnityEngine.Random.Range(0.7f, 1f), Staff_Controller.SkillType.CASHIER);
			}
			else if (UnityEngine.Random.value < 0.2f)
			{
				this.ctrl.cashier_Controller.RemoveOperator();
				this.ctrl.FindNewPath();
			}
			else
			{
				this.ctrl.SetCooldown(1f, Staff_Controller.SkillType.NONE);
			}
			return true;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00032A90 File Offset: 0x00030C90
		public override void OnTargetReached()
		{
			if (this.target_Cashier.GetHasOperator())
			{
				this.ctrl.ClearTask();
			}
			else
			{
				this.target_Cashier.OperateCashier(this.ctrl);
			}
			this.ctrl.SetCooldown(1f, Staff_Controller.SkillType.NONE);
			this.ctrl.char_Controller.animator_.SetBool("Thinking", true);
		}

		// Token: 0x04000678 RID: 1656
		public Cashier_Controller target_Cashier;
	}

	// Token: 0x0200006B RID: 107
	public class Staff_Task_TakeObject : Staff_Controller.Staff_Task
	{
		// Token: 0x06000564 RID: 1380 RVA: 0x00032AF4 File Offset: 0x00030CF4
		private Staff_Task_TakeObject(Staff_Controller _ctrl, Interaction_Controller _target) : base(_ctrl)
		{
			this.target_Object = _target;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00032B04 File Offset: 0x00030D04
		public static Staff_Controller.Staff_Task_TakeObject Create(Staff_Controller _ctrl, Interaction_Controller _target)
		{
			if (_ctrl == null || _target == null || _target.gameObject == null)
			{
				return null;
			}
			_ctrl.navPath = Nav_Manager.instance.GetNavPathTo(_ctrl.transform.position, _target.gameObject, true);
			if (_ctrl.navPath == null)
			{
				return null;
			}
			return new Staff_Controller.Staff_Task_TakeObject(_ctrl, _target);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00032B68 File Offset: 0x00030D68
		public static Staff_Controller.Staff_Task_TakeObject CreateTakeGarbage(Staff_Controller _ctrl)
		{
			List<Interaction_Controller> garbage = Nav_Manager.instance.GetGarbage();
			if (garbage.Count <= 0)
			{
				return null;
			}
			return Staff_Controller.Staff_Task_TakeObject.Create(_ctrl, garbage[UnityEngine.Random.Range(0, garbage.Count)]);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00032BA4 File Offset: 0x00030DA4
		public static Staff_Controller.Staff_Task_TakeObject CreatePickUpBox(Staff_Controller _ctrl)
		{
			List<Box_Controller> list = Inv_Manager.instance.box_Controllers.FindAll((Box_Controller x) => !x.isHeld && !x.needsRefrigerator);
			List<Box_Controller> list2 = Inv_Manager.instance.box_Controllers.FindAll((Box_Controller x) => !x.isHeld && x.needsRefrigerator);
			bool ifInvIsFull = Inv_Manager.instance.GetIfInvIsFull(false);
			bool ifInvIsFull2 = Inv_Manager.instance.GetIfInvIsFull(true);
			if (ifInvIsFull)
			{
				list = list.FindAll((Box_Controller x) => Vector3.Distance(x.gameObject.transform.position, Inv_Manager.instance.deliveryPlaces[0].transform.position) > 6f);
			}
			if (ifInvIsFull2)
			{
				list2 = list2.FindAll((Box_Controller x) => Vector3.Distance(x.gameObject.transform.position, Inv_Manager.instance.deliveryPlaces[0].transform.position) > 6f);
			}
			if (list.Count <= 0 && list2.Count <= 0)
			{
				return null;
			}
			List<Box_Controller> list3 = new List<Box_Controller>(list);
			foreach (Box_Controller item in list2)
			{
				list3.Add(item);
			}
			return Staff_Controller.Staff_Task_TakeObject.Create(_ctrl, list3[UnityEngine.Random.Range(0, list3.Count)].GetComponent<Interaction_Controller>());
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00032CF4 File Offset: 0x00030EF4
		public static Staff_Controller.Staff_Task_TakeObject CreateCleanDirt(Staff_Controller _ctrl)
		{
			List<Interaction_Controller> dirt = Nav_Manager.instance.GetDirt();
			if (dirt.Count <= 0)
			{
				return null;
			}
			return Staff_Controller.Staff_Task_TakeObject.Create(_ctrl, dirt[UnityEngine.Random.Range(0, dirt.Count)]);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00032D2F File Offset: 0x00030F2F
		public override void OnTargetReached()
		{
			this.ctrl.SetCooldown(1f, Staff_Controller.SkillType.CLEAN);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00032D44 File Offset: 0x00030F44
		public override void Update()
		{
			if (this.ctrl.navPath != null)
			{
				return;
			}
			if (this.target_Object == null)
			{
				this.ctrl.ClearTask();
				return;
			}
			Vector3 normalized = (this.target_Object.transform.position - this.ctrl.transform.position).normalized;
			this.ctrl.char_Controller.Rotate(normalized);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00032DB8 File Offset: 0x00030FB8
		public override bool UpdateAI()
		{
			if (this.ctrl.navPath != null)
			{
				return false;
			}
			if (this.target_Object == null)
			{
				this.ctrl.ClearTask();
				return false;
			}
			this.target_Object.StaffInteraction(this.ctrl);
			this.ctrl.ClearTask();
			return true;
		}

		// Token: 0x04000679 RID: 1657
		public Interaction_Controller target_Object;
	}

	// Token: 0x0200006C RID: 108
	public class Staff_Task_ThrowBox : Staff_Controller.Staff_Task
	{
		// Token: 0x0600056C RID: 1388 RVA: 0x00032E0D File Offset: 0x0003100D
		private Staff_Task_ThrowBox(Staff_Controller _ctrl) : base(_ctrl)
		{
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00032E16 File Offset: 0x00031016
		public static Staff_Controller.Staff_Task_ThrowBox Create(Staff_Controller _ctrl)
		{
			if (_ctrl.boxController == null)
			{
				return null;
			}
			_ctrl.SetCooldown(1f, Staff_Controller.SkillType.RESTOCK);
			return new Staff_Controller.Staff_Task_ThrowBox(_ctrl);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00032E3A File Offset: 0x0003103A
		public override bool UpdateAI()
		{
			if (this.ctrl.boxController != null)
			{
				this.ctrl.boxController.ThrowBox();
				this.ctrl.boxController = null;
			}
			this.ctrl.ClearTask();
			return true;
		}
	}

	// Token: 0x0200006D RID: 109
	public class Staff_Task_ThrowBoxOnInv : Staff_Controller.Staff_Task
	{
		// Token: 0x0600056F RID: 1391 RVA: 0x00032E77 File Offset: 0x00031077
		private Staff_Task_ThrowBoxOnInv(Staff_Controller _ctrl, GameObject _target) : base(_ctrl)
		{
			this.target = _target;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00032E88 File Offset: 0x00031088
		public static Staff_Controller.Staff_Task_ThrowBoxOnInv Create(Staff_Controller _ctrl)
		{
			if (_ctrl.boxController == null)
			{
				return null;
			}
			GameObject x = Nav_Manager.instance.GetTrashCan(_ctrl.transform.position);
			if (Inv_Manager.instance.deliveryPlaces.Count >= 0 && Inv_Manager.instance.deliveryPlaces[0])
			{
				x = Inv_Manager.instance.deliveryPlaces[0];
			}
			if (x == null)
			{
				return null;
			}
			_ctrl.navPath = Nav_Manager.instance.GetNavPathTo(_ctrl.transform.position, x, true);
			if (_ctrl.navPath == null)
			{
				return null;
			}
			return new Staff_Controller.Staff_Task_ThrowBoxOnInv(_ctrl, x);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00032F2E File Offset: 0x0003112E
		public override void OnTargetReached()
		{
			this.ctrl.SetCooldown(1f, Staff_Controller.SkillType.RESTOCK);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00032F44 File Offset: 0x00031144
		public override void Update()
		{
			if (this.ctrl.navPath != null)
			{
				return;
			}
			Vector3 normalized = (this.target.transform.position - this.ctrl.transform.position).normalized;
			this.ctrl.char_Controller.Rotate(normalized);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00032FA0 File Offset: 0x000311A0
		public override bool UpdateAI()
		{
			if (this.ctrl.navPath != null)
			{
				return false;
			}
			if (this.target == null)
			{
				this.ctrl.ClearTask();
				return false;
			}
			if (this.ctrl.boxController != null)
			{
				this.ctrl.boxController.ThrowBox();
				this.ctrl.boxController = null;
			}
			this.ctrl.ClearTask();
			return true;
		}

		// Token: 0x0400067A RID: 1658
		public GameObject target;
	}

	// Token: 0x0200006E RID: 110
	public class Staff_Task_Trash : Staff_Controller.Staff_Task
	{
		// Token: 0x06000574 RID: 1396 RVA: 0x00033012 File Offset: 0x00031212
		private Staff_Task_Trash(Staff_Controller _ctrl, GameObject _target) : base(_ctrl)
		{
			this.target = _target;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00033024 File Offset: 0x00031224
		public static Staff_Controller.Staff_Task_Trash Create(Staff_Controller _ctrl)
		{
			if (_ctrl.boxController == null)
			{
				return null;
			}
			GameObject trashCan = Nav_Manager.instance.GetTrashCan(_ctrl.transform.position);
			if (trashCan == null)
			{
				return null;
			}
			_ctrl.navPath = Nav_Manager.instance.GetNavPathTo(_ctrl.transform.position, trashCan, true);
			if (_ctrl.navPath == null)
			{
				return null;
			}
			return new Staff_Controller.Staff_Task_Trash(_ctrl, trashCan);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00033090 File Offset: 0x00031290
		public override void OnTargetReached()
		{
			this.ctrl.SetCooldown(1f, Staff_Controller.SkillType.RESTOCK);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x000330A4 File Offset: 0x000312A4
		public override void Update()
		{
			if (this.ctrl.navPath != null)
			{
				return;
			}
			Vector3 normalized = (this.target.transform.position - this.ctrl.transform.position).normalized;
			this.ctrl.char_Controller.Rotate(normalized);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00033100 File Offset: 0x00031300
		public override bool UpdateAI()
		{
			if (this.ctrl.navPath != null)
			{
				return false;
			}
			if (this.target == null)
			{
				this.ctrl.ClearTask();
				return false;
			}
			if (this.ctrl.boxController != null)
			{
				this.target.GetComponent<Interaction_Controller>().AnimateGeneric();
				this.ctrl.boxController.DeleteBox(0);
				this.ctrl.boxController = null;
			}
			this.ctrl.ClearTask();
			return true;
		}

		// Token: 0x0400067B RID: 1659
		public GameObject target;
	}

	// Token: 0x0200006F RID: 111
	public class Staff_Task_GetInvBox : Staff_Controller.Staff_Task
	{
		// Token: 0x06000579 RID: 1401 RVA: 0x00033183 File Offset: 0x00031383
		private Staff_Task_GetInvBox(Staff_Controller _ctrl, Shelf_Controller _shelf, int _place, int _prodIndex) : base(_ctrl)
		{
			this.shelf = _shelf;
			this.place = _place;
			this.prodIndex = _prodIndex;
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x000331A4 File Offset: 0x000313A4
		public static Staff_Controller.Staff_Task_GetInvBox Create(Staff_Controller _ctrl)
		{
			if (_ctrl.boxController != null)
			{
				return null;
			}
			int num = -1;
			foreach (Shelf_Controller shelf_Controller in Inv_Manager.instance.shelfProd_Controllers)
			{
				if (shelf_Controller.isShelfProd)
				{
					for (int i = 0; i < shelf_Controller.height; i++)
					{
						if (shelf_Controller.prodControllers[i, 0] && !shelf_Controller.prodControllers[i, shelf_Controller.width - 1] && (!shelf_Controller.prodControllers[i, 0].lifeSpan || shelf_Controller.prodControllers[i, 0].lifeSpanIndex > 0))
						{
							num = shelf_Controller.prodControllers[i, 0].prodIndex;
							Debug.Log("Specific Prod Index is " + num.ToString());
							break;
						}
					}
					if (num != -1)
					{
						break;
					}
				}
			}
			List<Staff_Controller.Staff_Task_GetInvBox.ShelfPlace> list = new List<Staff_Controller.Staff_Task_GetInvBox.ShelfPlace>();
			bool ifStoreIsFull = Inv_Manager.instance.GetIfStoreIsFull(false);
			bool ifStoreIsFull2 = Inv_Manager.instance.GetIfStoreIsFull(true);
			if (ifStoreIsFull && ifStoreIsFull2)
			{
				return null;
			}
			foreach (Shelf_Controller shelf_Controller2 in Inv_Manager.instance.shelfInv_Controllers)
			{
				if (shelf_Controller2.isShelfInv)
				{
					for (int j = 0; j < shelf_Controller2.height; j++)
					{
						if (shelf_Controller2.boxControllers[j] && !shelf_Controller2.boxControllers[j].frozen)
						{
							if (shelf_Controller2.boxControllers[j].itemIndex == num)
							{
								Staff_Controller.Staff_Task_GetInvBox.ShelfPlace item;
								item.shelf = shelf_Controller2;
								item.place = j;
								list.Add(item);
								break;
							}
							if ((shelf_Controller2.boxControllers[j].needsRefrigerator || !ifStoreIsFull) && (!shelf_Controller2.boxControllers[j].needsRefrigerator || !ifStoreIsFull2))
							{
								int _prod = shelf_Controller2.boxControllers[j].itemIndex;
								if (Inv_Manager.instance.shelfInv_Controllers.Exists((Shelf_Controller x) => x.FindProdBox(_prod) >= 0))
								{
									Staff_Controller.Staff_Task_GetInvBox.ShelfPlace item2;
									item2.shelf = shelf_Controller2;
									item2.place = j;
									list.Add(item2);
								}
							}
						}
					}
				}
			}
			Debug.Log("0");
			if (list.Count <= 0)
			{
				Debug.Log("1A");
				return null;
			}
			Debug.Log("1B");
			Staff_Controller.Staff_Task_GetInvBox.ShelfPlace shelfPlace = list[UnityEngine.Random.Range(0, list.Count)];
			int _prodIndex = shelfPlace.shelf.boxControllers[shelfPlace.place].itemIndex;
			Debug.Log("2B");
			List<Shelf_Controller> list2 = Inv_Manager.instance.shelfInv_Controllers;
			list2 = list2.FindAll((Shelf_Controller x) => x.FindProdBox(_prodIndex) >= 0);
			Debug.Log("3B");
			if (list2.Count <= 0)
			{
				return null;
			}
			Shelf_Controller shelf_Controller3 = list2[UnityEngine.Random.Range(0, list2.Count)];
			Debug.Log("4B");
			int num2 = shelf_Controller3.FindProdBox(_prodIndex);
			Debug.Log("5B");
			Debug.Log("2");
			Vector3 endPos = shelf_Controller3.transform.position + shelf_Controller3.transform.forward * 1f;
			_ctrl.navPath = Nav_Manager.instance.GetNavPath(_ctrl.transform.position, endPos, true);
			if (_ctrl.navPath == null)
			{
				return null;
			}
			Debug.Log("3");
			return new Staff_Controller.Staff_Task_GetInvBox(_ctrl, shelf_Controller3, num2, _prodIndex);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x000335A4 File Offset: 0x000317A4
		public override void OnTargetReached()
		{
			this.ctrl.SetCooldown(1f, Staff_Controller.SkillType.RESTOCK);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x000335B8 File Offset: 0x000317B8
		public override void Update()
		{
			if (this.ctrl.navPath != null)
			{
				return;
			}
			Vector3 vector = this.shelf.transform.position + this.shelf.transform.forward * 1f - this.ctrl.transform.position;
			if (vector.magnitude > 0.1f)
			{
				this.ctrl.char_Controller.Move(vector.normalized);
			}
			Vector3 normalized = (this.shelf.transform.position - this.ctrl.transform.position).normalized;
			this.ctrl.char_Controller.Rotate(normalized);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0003367C File Offset: 0x0003187C
		public override bool UpdateAI()
		{
			if (this.ctrl.navPath != null)
			{
				return false;
			}
			if (this.shelf == null || this.ctrl.boxController != null)
			{
				this.ctrl.ClearTask();
				return false;
			}
			if (this.shelf.boxControllers[this.place] == null || !this.shelf.boxControllers[this.place].isProd || this.shelf.boxControllers[this.place].itemIndex != this.prodIndex)
			{
				this.place = this.shelf.FindProdBox(this.prodIndex);
				if (this.place < 0)
				{
					this.ctrl.ClearTask();
					return false;
				}
			}
			this.shelf.GiveBox(this.place, this.ctrl);
			this.ctrl.ClearTask();
			return true;
		}

		// Token: 0x0400067C RID: 1660
		public Shelf_Controller shelf;

		// Token: 0x0400067D RID: 1661
		public int place;

		// Token: 0x0400067E RID: 1662
		public int prodIndex;

		// Token: 0x020000A5 RID: 165
		private struct ShelfPlace
		{
			// Token: 0x0400072D RID: 1837
			public Shelf_Controller shelf;

			// Token: 0x0400072E RID: 1838
			public int place;
		}
	}

	// Token: 0x02000070 RID: 112
	public class Staff_Task_GetExpiredProd : Staff_Controller.Staff_Task
	{
		// Token: 0x0600057E RID: 1406 RVA: 0x00033768 File Offset: 0x00031968
		private Staff_Task_GetExpiredProd(Staff_Controller _ctrl, Shelf_Controller _shelf, int _placeIndex, int _prodIndex) : base(_ctrl)
		{
			this.shelf = _shelf;
			this.placeIndex = _placeIndex;
			this.prodIndex = _prodIndex;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00033788 File Offset: 0x00031988
		public static Staff_Controller.Staff_Task_GetExpiredProd Create(Staff_Controller _ctrl)
		{
			if (_ctrl.boxController != null)
			{
				return null;
			}
			List<Staff_Controller.Staff_Task_GetExpiredProd.ShelfPlace> list = new List<Staff_Controller.Staff_Task_GetExpiredProd.ShelfPlace>();
			foreach (Shelf_Controller shelf_Controller in Inv_Manager.instance.shelfProd_Controllers)
			{
				if (shelf_Controller.isShelfProd)
				{
					for (int i = 0; i < shelf_Controller.height; i++)
					{
						if (shelf_Controller.prodControllers[i, 0] && shelf_Controller.prodControllers[i, 0].lifeSpan && shelf_Controller.prodControllers[i, 0].lifeSpanIndex < 1)
						{
							Staff_Controller.Staff_Task_GetExpiredProd.ShelfPlace item;
							item.shelf = shelf_Controller;
							item.placeIndex = i;
							list.Add(item);
						}
					}
				}
			}
			int num = 0;
			if (list.Count <= 0)
			{
				return null;
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			Shelf_Controller shelf_Controller2 = list[index].shelf;
			int num2 = list[index].placeIndex;
			Vector3 endPos = shelf_Controller2.transform.position + shelf_Controller2.transform.forward * 1f;
			_ctrl.navPath = Nav_Manager.instance.GetNavPath(_ctrl.transform.position, endPos, true);
			if (_ctrl.navPath == null)
			{
				return null;
			}
			return new Staff_Controller.Staff_Task_GetExpiredProd(_ctrl, shelf_Controller2, num2, num);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00033908 File Offset: 0x00031B08
		public override void OnTargetReached()
		{
			this.ctrl.SetCooldown(1f, Staff_Controller.SkillType.RESTOCK);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0003391C File Offset: 0x00031B1C
		public override void Update()
		{
			if (this.ctrl.navPath != null)
			{
				return;
			}
			Vector3 vector = this.shelf.transform.position + this.shelf.transform.forward * 1f - this.ctrl.transform.position;
			if (vector.magnitude > 0.1f)
			{
				this.ctrl.char_Controller.Move(vector.normalized);
			}
			Vector3 normalized = (this.shelf.transform.position - this.ctrl.transform.position).normalized;
			this.ctrl.char_Controller.Rotate(normalized);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x000339E0 File Offset: 0x00031BE0
		public override bool UpdateAI()
		{
			if (this.ctrl.navPath != null)
			{
				return false;
			}
			if (this.shelf == null || this.ctrl.boxController != null || !this.shelf.prodControllers[this.placeIndex, 0])
			{
				this.ctrl.ClearTask();
				return false;
			}
			this.shelf.GiveProd(this.placeIndex, this.ctrl);
			this.ctrl.ClearTask();
			return true;
		}

		// Token: 0x0400067F RID: 1663
		public Shelf_Controller shelf;

		// Token: 0x04000680 RID: 1664
		public int placeIndex;

		// Token: 0x04000681 RID: 1665
		public int prodIndex;

		// Token: 0x020000A8 RID: 168
		private struct ShelfPlace
		{
			// Token: 0x04000731 RID: 1841
			public Shelf_Controller shelf;

			// Token: 0x04000732 RID: 1842
			public int placeIndex;
		}
	}

	// Token: 0x02000071 RID: 113
	public class Staff_Task_StoreProd : Staff_Controller.Staff_Task
	{
		// Token: 0x06000583 RID: 1411 RVA: 0x00033A6B File Offset: 0x00031C6B
		private Staff_Task_StoreProd(Staff_Controller _ctrl, Shelf_Controller _shelf, int _place) : base(_ctrl)
		{
			this.shelf = _shelf;
			this.place = _place;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00033A84 File Offset: 0x00031C84
		public static Staff_Controller.Staff_Task_StoreProd Create(Staff_Controller _ctrl)
		{
			if (_ctrl.boxController == null || !_ctrl.boxController.isProd)
			{
				return null;
			}
			int _prodIndex = _ctrl.boxController.itemIndex;
			Prod_Controller _prod = Inv_Manager.instance.GetProdPrefab(_prodIndex);
			List<Shelf_Controller> list = Inv_Manager.instance.shelfProd_Controllers;
			list = list.FindAll((Shelf_Controller s) => s.isShelfProd && s.isRefrigerated == _prod.needsRefrigerator);
			List<Shelf_Controller> list2 = list.FindAll((Shelf_Controller s) => s.FindProdPlace(_prodIndex) >= 0);
			if (list2.Count <= 0)
			{
				list2 = list.FindAll((Shelf_Controller s) => s.FindEmptyPlace() >= 0);
				if (list2.Count <= 0)
				{
					return null;
				}
			}
			Shelf_Controller shelf_Controller = list2[UnityEngine.Random.Range(0, list2.Count)];
			int num = shelf_Controller.FindProdPlace(_prodIndex);
			if (num < 0)
			{
				num = shelf_Controller.FindEmptyPlace();
			}
			Vector3 endPos = shelf_Controller.transform.position + shelf_Controller.transform.forward * 1f;
			_ctrl.navPath = Nav_Manager.instance.GetNavPath(_ctrl.transform.position, endPos, true);
			if (_ctrl.navPath == null)
			{
				return null;
			}
			return new Staff_Controller.Staff_Task_StoreProd(_ctrl, shelf_Controller, num);
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00033BCC File Offset: 0x00031DCC
		public override void OnTargetReached()
		{
			this.ctrl.SetCooldown(1f, Staff_Controller.SkillType.RESTOCK);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00033BE0 File Offset: 0x00031DE0
		public override void Update()
		{
			if (this.ctrl.navPath != null)
			{
				return;
			}
			Vector3 vector = this.shelf.transform.position + this.shelf.transform.forward * 1f - this.ctrl.transform.position;
			if (vector.magnitude > 0.1f)
			{
				this.ctrl.char_Controller.Move(vector.normalized);
			}
			Vector3 normalized = (this.shelf.transform.position - this.ctrl.transform.position).normalized;
			this.ctrl.char_Controller.Rotate(normalized);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00033CA4 File Offset: 0x00031EA4
		public override bool UpdateAI()
		{
			if (this.ctrl.navPath != null)
			{
				return false;
			}
			if (this.shelf == null || this.ctrl.boxController == null || !this.ctrl.boxController.isProd)
			{
				this.ctrl.ClearTask();
				return false;
			}
			if (this.shelf.prodControllers[this.place, 0] != null && this.shelf.prodControllers[this.place, 0].prodIndex != this.ctrl.boxController.itemIndex)
			{
				this.place = this.shelf.FindProdPlace(this.ctrl.boxController.itemIndex);
				if (this.place < 0)
				{
					this.place = this.shelf.FindEmptyPlace();
				}
				if (this.place < 0)
				{
					this.ctrl.ClearTask();
					return false;
				}
			}
			this.shelf.StoreProd(this.place, this.ctrl.boxController, 0);
			this.ctrl.ClearTask();
			return true;
		}

		// Token: 0x04000682 RID: 1666
		public Shelf_Controller shelf;

		// Token: 0x04000683 RID: 1667
		public int place;
	}

	// Token: 0x02000072 RID: 114
	public class Staff_Task_StoreInv : Staff_Controller.Staff_Task
	{
		// Token: 0x06000588 RID: 1416 RVA: 0x00033DC7 File Offset: 0x00031FC7
		private Staff_Task_StoreInv(Staff_Controller _ctrl, Shelf_Controller _shelf, int _place) : base(_ctrl)
		{
			this.shelf = _shelf;
			this.place = _place;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00033DE0 File Offset: 0x00031FE0
		public static Staff_Controller.Staff_Task_StoreInv Create(Staff_Controller _ctrl)
		{
			if (_ctrl.boxController == null)
			{
				return null;
			}
			List<Shelf_Controller> list = Inv_Manager.instance.shelfInv_Controllers.FindAll(delegate(Shelf_Controller x)
			{
				if (x.isShelfInv)
				{
					return Array.Exists<Box_Controller>(x.boxControllers, (Box_Controller b) => b == null);
				}
				return false;
			});
			bool _refrigerated = Inv_Manager.instance.GetProdPrefab(_ctrl.boxController.itemIndex).needsRefrigerator;
			List<Shelf_Controller> list2 = list.FindAll((Shelf_Controller x) => x.isRefrigerated == _refrigerated);
			int count = list2.Count;
			if (list2.Count <= 0)
			{
				return null;
			}
			Shelf_Controller shelf_Controller = list2[UnityEngine.Random.Range(0, list2.Count)];
			Vector3 endPos = shelf_Controller.transform.position + shelf_Controller.transform.forward * 1f;
			_ctrl.navPath = Nav_Manager.instance.GetNavPath(_ctrl.transform.position, endPos, true);
			if (_ctrl.navPath == null)
			{
				return null;
			}
			return new Staff_Controller.Staff_Task_StoreInv(_ctrl, shelf_Controller, Array.IndexOf<Box_Controller>(shelf_Controller.boxControllers, null));
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00033EEA File Offset: 0x000320EA
		public override void OnTargetReached()
		{
			this.ctrl.SetCooldown(1f, Staff_Controller.SkillType.RESTOCK);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00033F00 File Offset: 0x00032100
		public override void Update()
		{
			if (this.ctrl.navPath != null)
			{
				return;
			}
			Vector3 vector = this.shelf.transform.position + this.shelf.transform.forward * 1f - this.ctrl.transform.position;
			if (vector.magnitude > 0.1f)
			{
				this.ctrl.char_Controller.Move(vector.normalized);
			}
			Vector3 normalized = (this.shelf.transform.position - this.ctrl.transform.position).normalized;
			this.ctrl.char_Controller.Rotate(normalized);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00033FC4 File Offset: 0x000321C4
		public override bool UpdateAI()
		{
			if (this.ctrl.navPath != null)
			{
				return false;
			}
			if (this.shelf == null)
			{
				this.ctrl.ClearTask();
				return false;
			}
			if (this.shelf.boxControllers[this.place] != null)
			{
				this.place = Array.IndexOf<Box_Controller>(this.shelf.boxControllers, null);
				if (this.place < 0)
				{
					this.ctrl.ClearTask();
					return false;
				}
			}
			if (this.ctrl.boxController != null)
			{
				this.shelf.StoreBox(this.place, this.ctrl);
			}
			this.ctrl.ClearTask();
			return true;
		}

		// Token: 0x04000684 RID: 1668
		public Shelf_Controller shelf;

		// Token: 0x04000685 RID: 1669
		public int place;
	}

	// Token: 0x02000073 RID: 115
	public class Staff_Task_Loiter : Staff_Controller.Staff_Task
	{
		// Token: 0x0600058D RID: 1421 RVA: 0x00034078 File Offset: 0x00032278
		private Staff_Task_Loiter(Staff_Controller _ctrl) : base(_ctrl)
		{
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00034081 File Offset: 0x00032281
		public static Staff_Controller.Staff_Task_Loiter Create(Staff_Controller _ctrl)
		{
			_ctrl.navPath = Nav_Manager.instance.GetNavPathLoiter(_ctrl.transform.position, true);
			return new Staff_Controller.Staff_Task_Loiter(_ctrl);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x000340A5 File Offset: 0x000322A5
		public override void OnTargetReached()
		{
			this.ctrl.SetCooldown(2f, Staff_Controller.SkillType.ALL);
			this.ctrl.char_Controller.animator_.SetBool("Thinking", true);
			this.ctrl.ClearTask();
		}
	}
}
