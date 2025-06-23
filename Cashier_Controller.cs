using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class Cashier_Controller : MonoBehaviour
{
	// Token: 0x06000039 RID: 57 RVA: 0x000043FC File Offset: 0x000025FC
	private void Awake()
	{
		Transform transform = base.transform.Find("ProdPlaces");
		for (int i = 0; i < 3; i++)
		{
			this.prodPlaces.Add(transform.Find("ProdPlace (" + i.ToString() + ")").gameObject);
		}
		Transform transform2 = base.transform.Find("ClientPlaces");
		for (int j = 0; j < this.clientPlaces_Qnt; j++)
		{
			this.clientPlaces.Add(transform2.Find("ClientPlace (" + j.ToString() + ")").gameObject);
		}
		this.operatorPlace = base.transform.Find("OperatorPlace").gameObject;
	}

	// Token: 0x0600003A RID: 58 RVA: 0x000044BB File Offset: 0x000026BB
	private void Start()
	{
		Interactor_Manager.instance.AddCashierController(this, 0);
		Interactor_Manager.instance.AddCashierController(this, 1);
		if (this.paperBag_Anim)
		{
			this.paperBag_Anim.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600003B RID: 59 RVA: 0x000044F3 File Offset: 0x000026F3
	private void Update()
	{
		this.UpdateCashier();
	}

	// Token: 0x0600003C RID: 60 RVA: 0x000044FC File Offset: 0x000026FC
	public void OperateCashier(Player_Controller _player)
	{
		if (this.staff_Controller)
		{
			this.staff_Controller.SetCashier(null);
		}
		this.player_Controller = _player;
		this.staff_Controller = null;
		if (this.player_Controller)
		{
			this.player_Controller.SetCashier(this);
		}
	}

	// Token: 0x0600003D RID: 61 RVA: 0x0000454C File Offset: 0x0000274C
	public void OperateCashier(Staff_Controller _staff)
	{
		if (this.staff_Controller)
		{
			this.staff_Controller.SetCashier(null);
		}
		if (this.player_Controller)
		{
			this.player_Controller.SetCashier(null);
		}
		this.staff_Controller = _staff;
		this.player_Controller = null;
		if (this.staff_Controller)
		{
			this.staff_Controller.SetCashier(this);
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x000045B4 File Offset: 0x000027B4
	public void RemoveOperator()
	{
		if (this.player_Controller)
		{
			this.player_Controller.SetCashier(null);
		}
		if (this.staff_Controller)
		{
			this.staff_Controller.SetCashier(null);
		}
		this.player_Controller = null;
		this.staff_Controller = null;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00004601 File Offset: 0x00002801
	public bool GetHasPlayer()
	{
		return this.player_Controller;
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00004613 File Offset: 0x00002813
	public bool GetHasOperator()
	{
		return this.player_Controller || this.staff_Controller;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00004634 File Offset: 0x00002834
	private void UpdateCashier()
	{
		for (int i = 0; i < this.prod_Controllers.Count; i++)
		{
			if (this.prod_Controllers[i])
			{
				if (i < this.prodPlaces.Count - 1)
				{
					if (!this.prod_Controllers[i].gameObject.activeInHierarchy)
					{
						this.prod_Controllers[i].gameObject.SetActive(true);
					}
					if (this.prod_Controllers[i].transform.position != this.prodPlaces[i].transform.position)
					{
						this.prod_Controllers[i].transform.position = Vector3.Lerp(this.prod_Controllers[i].transform.position, this.prodPlaces[i].transform.position, this.prodMoveSpeed * Time.deltaTime);
					}
				}
				else if (i == this.prodPlaces.Count - 1)
				{
					if (!this.prod_Controllers[i].gameObject.activeInHierarchy)
					{
						this.prod_Controllers[i].gameObject.SetActive(true);
					}
					if (this.prod_Controllers[i].transform.position != this.prodPlaces[i].transform.position)
					{
						this.prod_Controllers[i].transform.position = this.prodPlaces[i].transform.position;
					}
				}
				else if (this.prod_Controllers[i].gameObject.activeInHierarchy)
				{
					this.prod_Controllers[i].gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00004815 File Offset: 0x00002A15
	public bool GetHasClients()
	{
		return this.char_Controllers.Count > 0;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00004825 File Offset: 0x00002A25
	public int SetClient(Customer_Controller _char)
	{
		if (!this.char_Controllers.Contains(_char))
		{
			this.char_Controllers.Add(_char);
			this.SendNewClientPlaces();
			return this.char_Controllers.Count - 1;
		}
		return -1;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00004856 File Offset: 0x00002A56
	public GameObject GetClientPlace(int _index)
	{
		if (_index < 0)
		{
			return null;
		}
		if (_index < this.clientPlaces_Qnt)
		{
			return this.clientPlaces[_index];
		}
		return null;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00004875 File Offset: 0x00002A75
	public GameObject GetAvailableClientPlace()
	{
		if (this.char_Controllers.Count < this.prodPlaces.Count)
		{
			return this.prodPlaces[this.char_Controllers.Count];
		}
		return null;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x000048A7 File Offset: 0x00002AA7
	public void ClientDesist(Customer_Controller _char)
	{
		if (this.char_Controllers.IndexOf(_char) == 0)
		{
			this.ClearProdList();
			this.RemoveClient(0);
			return;
		}
		this.RemoveClient(_char);
		this.SendNewClientPlaces();
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000048D4 File Offset: 0x00002AD4
	public void RemoveClient(int _index)
	{
		if (this.char_Controllers.Count > _index)
		{
			this.char_Controllers.RemoveAt(_index);
		}
		if (this.char_Controllers.Count == 0)
		{
			this.paperBag_Anim.PlayInFixedTime("Off", -1, 0f);
		}
		this.SendNewClientPlaces();
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00004924 File Offset: 0x00002B24
	public void RemoveClient(Customer_Controller _char)
	{
		if (this.char_Controllers.Contains(_char))
		{
			this.char_Controllers.Remove(_char);
		}
		if (this.char_Controllers.Count == 0)
		{
			this.paperBag_Anim.PlayInFixedTime("Off", -1, 0f);
		}
		this.SendNewClientPlaces();
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00004978 File Offset: 0x00002B78
	public void SendNewClientPlaces()
	{
		for (int i = 0; i < this.char_Controllers.Count; i++)
		{
			if (this.char_Controllers[i])
			{
				if (i < this.prodPlaces.Count)
				{
					this.char_Controllers[i].ReceiveNewClientPlaceIndex(i, this);
				}
				else
				{
					this.char_Controllers[i].ReceiveNewClientPlaceIndex(-1, this);
				}
			}
		}
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000049E4 File Offset: 0x00002BE4
	public void FinishClient(Customer_Controller _char)
	{
		this.RemoveClient(_char);
		_char.FinishedCashier();
		this.paperBag_Anim.PlayInFixedTime("Off", -1, 0f);
		Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.work_Cashier, -1);
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00004A18 File Offset: 0x00002C18
	public void SetProdList(List<int> _prodIndexes)
	{
		this.prod_ButtonIndex.Clear();
		if (_prodIndexes == null)
		{
			this.prod_Controllers.Clear();
			return;
		}
		this.prod_Controllers = new List<Prod_Controller>();
		for (int i = 0; i < _prodIndexes.Count; i++)
		{
			this.prod_ButtonIndex.Add(UnityEngine.Random.Range(0, 3));
			this.prod_Controllers.Add(Inv_Manager.instance.CreateProd(_prodIndexes[i], false, 7));
			if (i < this.prodPlaces.Count)
			{
				this.prod_Controllers[i].HoldProd(true, this, this.prodPlaces[i].transform.position, this.prodPlaces[i].transform.rotation);
			}
			else
			{
				this.prod_Controllers[i].HoldProd(false, this, base.transform.position, this.prodPlaces[this.prodPlaces.Count - 1].transform.rotation);
			}
		}
		this.got_right_buttons = true;
		this.paperBag_Anim.gameObject.SetActive(true);
		this.paperBag_Anim.PlayInFixedTime("On", -1, 0f);
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00004B4E File Offset: 0x00002D4E
	public List<Prod_Controller> GetProdList()
	{
		return new List<Prod_Controller>(this.prod_Controllers);
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00004B5C File Offset: 0x00002D5C
	public void ClearProdList()
	{
		for (int i = 0; i < this.prod_Controllers.Count; i++)
		{
			if (this.prod_Controllers[i])
			{
				this.prod_Controllers[i].DeleteProd();
			}
		}
		this.prod_Controllers.Clear();
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00004BAE File Offset: 0x00002DAE
	public List<int> GetButtonList()
	{
		return new List<int>(this.prod_ButtonIndex);
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00004BBC File Offset: 0x00002DBC
	public bool PassProdAction(bool _right, int _player_index)
	{
		if (this.prod_Controllers.Count == 0)
		{
			return false;
		}
		Prod_Controller prod_Controller = this.prod_Controllers[0];
		float num = (float)Inv_Manager.instance.GetProdSellPriceDiscounted(prod_Controller.prodIndex);
		if (_right)
		{
			Finances_Manager.instance.AddMoney(num);
			Finances_Manager.instance.AddTo_InSales(num);
			Interactor_Manager.instance.SetCashierAnimator("+" + num.ToString(), "Right", this, _player_index);
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_Cashier_RightButton);
		}
		else
		{
			this.got_right_buttons = false;
			Finances_Manager.instance.AddMoney(num);
			Finances_Manager.instance.AddTo_InSales(num);
			Interactor_Manager.instance.SetCashierAnimator(num.ToString(), "Wrong", this, _player_index);
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_Cashier_WrongButton);
		}
		this.paperBag_Anim.PlayInFixedTime("AddProd", -1, 0f);
		this.prod_Controllers.RemoveAt(0);
		this.prod_ButtonIndex.RemoveAt(0);
		prod_Controller.DeleteProd();
		if (this.char_Controllers.Count > 0)
		{
			Customer_Controller @char = this.char_Controllers[0];
			if (this.prod_Controllers.Count == 0)
			{
				this.FinishClient(@char);
			}
		}
		return _right;
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00004CF8 File Offset: 0x00002EF8
	public void PassProd(int _buttonIndex, int _player_index)
	{
		if (this.prod_Controllers.Count == 0)
		{
			return;
		}
		int itemIndex = Inv_Manager.instance.GetItemIndex(this.prod_Controllers[0].gameObject);
		Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.sell_Prod, itemIndex);
		this.PassProdAction(this.prod_ButtonIndex[0] == _buttonIndex, _player_index);
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00004D54 File Offset: 0x00002F54
	public void PassProdStaff(bool _right, int _player_index)
	{
		if (this.prod_Controllers.Count == 0)
		{
			return;
		}
		int itemIndex = Inv_Manager.instance.GetItemIndex(this.prod_Controllers[0].gameObject);
		Missions_Manager.instance.Set_DidAction(Missions_Manager.ActionType.sell_Prod, itemIndex);
		this.PassProdAction(_right, _player_index);
	}

	// Token: 0x04000053 RID: 83
	[HideInInspector]
	public GameObject operatorPlace;

	// Token: 0x04000054 RID: 84
	[SerializeField]
	private Animator paperBag_Anim;

	// Token: 0x04000055 RID: 85
	[SerializeField]
	private int clientPlaces_Qnt;

	// Token: 0x04000056 RID: 86
	[SerializeField]
	private List<Customer_Controller> char_Controllers = new List<Customer_Controller>();

	// Token: 0x04000057 RID: 87
	[SerializeField]
	private List<Prod_Controller> prod_Controllers = new List<Prod_Controller>();

	// Token: 0x04000058 RID: 88
	private List<GameObject> prodPlaces = new List<GameObject>();

	// Token: 0x04000059 RID: 89
	private List<GameObject> clientPlaces = new List<GameObject>();

	// Token: 0x0400005A RID: 90
	public Player_Controller player_Controller;

	// Token: 0x0400005B RID: 91
	private Staff_Controller staff_Controller;

	// Token: 0x0400005C RID: 92
	private float prodMoveSpeed = 5f;

	// Token: 0x0400005D RID: 93
	private List<int> prod_ButtonIndex = new List<int>();

	// Token: 0x0400005E RID: 94
	public bool got_right_buttons = true;
}
