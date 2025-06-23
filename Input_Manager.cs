using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

// Token: 0x02000040 RID: 64
public class Input_Manager : MonoBehaviour
{
	// Token: 0x060002B4 RID: 692 RVA: 0x0001640E File Offset: 0x0001460E
	private void Awake()
	{
		if (!Input_Manager.instance)
		{
			Input_Manager.instance = this;
		}
		InputSystem.onAnyButtonPress.Call(delegate(InputControl control)
		{
			this.Set_LastActiveDevice(control);
		});
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00016439 File Offset: 0x00014639
	private void Start()
	{
		this.Delete_InputControllers(-1);
		this.Create_InputController();
		this.ResetScheme();
		this.UpdateScheme(false);
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00016458 File Offset: 0x00014658
	private void Update()
	{
		this.UpdateScheme(false);
		foreach (Input_Controller input_Controller in this.inputControllers)
		{
			input_Controller.UpdateInput();
		}
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x000164B0 File Offset: 0x000146B0
	private void Set_LastActiveDevice(InputControl _control)
	{
		if (_control.device.name == "Keyboard" || _control.device.name == "Mouse")
		{
			this.last_active_device = new ReadOnlyArray<InputDevice>(new InputDevice[]
			{
				Keyboard.current,
				Mouse.current
			});
		}
		else
		{
			this.last_active_device = new ReadOnlyArray<InputDevice>(new InputDevice[]
			{
				_control.device
			});
		}
		int num = this.Get_IfDeviceAlreadyInUse(this.last_active_device);
		if (Menu_Manager.instance.GetMenuName() == "Multiplayer" && num == -1)
		{
			this.StartMultiplayer(this.last_active_device);
			return;
		}
		if (num == -1)
		{
			this.inputControllers[0].Set_Device(this.last_active_device);
		}
		this.lastInputPlayerIndex = num;
		if (this.beforeInputPlayerIndex != this.lastInputPlayerIndex && Menu_Manager.instance.GetMenuName() != "MainMenu")
		{
			this.beforeInputPlayerIndex = this.lastInputPlayerIndex;
			string menuName = Menu_Manager.instance.GetMenuName();
			if ((menuName == "PC" || menuName == "Locker" || menuName == "Warning") && Menu_Manager.instance.Get_Same_Specific_Player_Index(this.lastInputPlayerIndex))
			{
				Menu_Manager.instance.UpdateMenu();
			}
		}
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x000165FC File Offset: 0x000147FC
	private int Get_IfDeviceAlreadyInUse(ReadOnlyArray<InputDevice> _devices)
	{
		if (_devices.Count <= 0)
		{
			return 0;
		}
		foreach (Input_Controller input_Controller in this.inputControllers)
		{
			if (input_Controller.playerInput.devices.Count > 0 && input_Controller.playerInput.devices[0] == _devices[0])
			{
				return input_Controller.playerIndex;
			}
		}
		return -1;
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00016694 File Offset: 0x00014894
	private int Debug_ListOfDevices()
	{
		int result = 0;
		for (int i = this.debug_devices.Length; i < this.debug_devices.Length; i++)
		{
			this.debug_devices[i] = "";
		}
		for (int j = 0; j < InputSystem.devices.Count; j++)
		{
			if (j < this.debug_devices.Length)
			{
				this.debug_devices[j] = InputSystem.devices[j].name;
			}
		}
		return result;
	}

	// Token: 0x060002BA RID: 698 RVA: 0x00016709 File Offset: 0x00014909
	public int Get_PlayerQnt()
	{
		return this.inputControllers.Count;
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00016718 File Offset: 0x00014918
	public string[] UpdateScheme(bool _forced = false)
	{
		this.Debug_ListOfDevices();
		if (Menu_Manager.instance.GetMenuName() != "Multiplayer" && this.Get_PlayerQnt() <= 1)
		{
			Input_Controller input_Controller = this.inputControllers[0];
			if (input_Controller.playerInput.devices.Count > 0 && this.last_active_device.Count > 0 && input_Controller.playerInput.devices[0] != this.last_active_device[0])
			{
				input_Controller.Set_Device(this.last_active_device);
				Menu_Manager.instance.UpdateMenu();
				this.RefreshInputHints();
				this.RefreshInputHintsActive();
			}
		}
		return this.currentScheme;
	}

	// Token: 0x060002BC RID: 700 RVA: 0x000167C8 File Offset: 0x000149C8
	public void ResetScheme()
	{
		foreach (Input_Controller input_Controller in this.inputControllers)
		{
			input_Controller.Refresh_Scheme();
		}
		this.UpdateScheme(false);
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00016820 File Offset: 0x00014A20
	public string GetScheme(int _player_index = -1)
	{
		string menuName = Menu_Manager.instance.GetMenuName();
		if ((menuName == "PC" || menuName == "Locker" || menuName == "Warning") && !Menu_Manager.instance.Get_Same_Specific_Player_Index(_player_index))
		{
			_player_index = Menu_Manager.instance.specific_player_index;
		}
		if (_player_index == -1)
		{
			_player_index = this.lastInputPlayerIndex;
		}
		if (_player_index == -1)
		{
			_player_index = 0;
		}
		return this.currentScheme[_player_index];
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00016894 File Offset: 0x00014A94
	public Input_Controller Create_InputController()
	{
		if (this.inputControllers.Count >= 4)
		{
			return null;
		}
		Input_Controller input_Controller = UnityEngine.Object.Instantiate<Input_Controller>(this.inputControllerRes.gameObject.GetComponent<Input_Controller>());
		this.inputControllers.Add(input_Controller);
		input_Controller.gameObject.SetActive(true);
		input_Controller.transform.SetParent(base.transform);
		input_Controller._Start(this.inputControllers.Count - 1);
		input_Controller.Refresh_Scheme();
		return input_Controller;
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0001690C File Offset: 0x00014B0C
	public void Refresh_InputControllers_Indexes()
	{
		for (int i = 0; i < this.inputControllers.Count; i++)
		{
			this.inputControllers[i].playerIndex = i;
		}
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x00016944 File Offset: 0x00014B44
	public void Delete_InputControllers(int _index = -1)
	{
		if (_index == -1)
		{
			foreach (Input_Controller input_Controller in this.inputControllers)
			{
				UnityEngine.Object.Destroy(input_Controller.gameObject);
			}
			this.inputControllers.Clear();
		}
		else
		{
			UnityEngine.Object.Destroy(this.inputControllers[_index].gameObject);
			this.inputControllers.RemoveAt(_index);
			Player_Manager.instance.DeletePlayer(_index);
			this.inputControllers.TrimExcess();
		}
		this.Refresh_InputControllers_Indexes();
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x000169E8 File Offset: 0x00014BE8
	public void StartMultiplayer(ReadOnlyArray<InputDevice> _devices)
	{
		this.Create_InputController().Set_Device(_devices);
		Player_Manager.instance.CreateSecondPlayer(true);
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x00016A01 File Offset: 0x00014C01
	public void EndMultiplayer()
	{
		Menu_Manager.instance.BackMenu();
		Player_Manager.instance.CreateSecondPlayer(false);
		this.Delete_InputControllers(-1);
		this.Create_InputController();
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00016A26 File Offset: 0x00014C26
	public void RemoveControllers()
	{
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x00016A28 File Offset: 0x00014C28
	public void SetCooldown(int _player_index)
	{
		this.inputControllers[_player_index].Set_Cooldown(this.inputControllers[_player_index].coolDownTime);
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x00016A4C File Offset: 0x00014C4C
	public void SetCooldown(int _player_index, float _t)
	{
		this.inputControllers[_player_index].Set_Cooldown(_t);
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x00016A60 File Offset: 0x00014C60
	public void SetCooldown(float _t)
	{
		foreach (Input_Controller input_Controller in this.inputControllers)
		{
			input_Controller.Set_Cooldown(_t);
		}
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x00016AB4 File Offset: 0x00014CB4
	public void SetCooldown()
	{
		foreach (Input_Controller input_Controller in this.inputControllers)
		{
			input_Controller.Set_Cooldown(input_Controller.coolDownTime);
		}
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x00016B0C File Offset: 0x00014D0C
	public void AddInputHint(InputHint_Controller _inputHint)
	{
		if (!this.inputHints.Contains(_inputHint))
		{
			this.inputHints.Add(_inputHint);
		}
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x00016B28 File Offset: 0x00014D28
	public void RefreshInputHints()
	{
		IEnumerator routine = this.RefreshInputHints_WaitingFrames(1);
		base.StartCoroutine(routine);
	}

	// Token: 0x060002CA RID: 714 RVA: 0x00016B48 File Offset: 0x00014D48
	public void RefreshInputHintsActive()
	{
		for (int i = 0; i < this.inputHints.Count; i++)
		{
			if (this.inputHints[i].gameObject.activeInHierarchy)
			{
				this.inputHints[i].RefreshInputHint();
			}
		}
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00016B95 File Offset: 0x00014D95
	private IEnumerator RefreshInputHints_WaitingFrames(int _count)
	{
		int num;
		for (int i = 0; i < _count; i = num + 1)
		{
			yield return null;
			num = i;
		}
		for (int j = 0; j < this.inputHints.Count; j++)
		{
			this.inputHints[j].RefreshInputHint();
		}
		yield break;
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00016BAB File Offset: 0x00014DAB
	public void SetVibration()
	{
	}

	// Token: 0x060002CD RID: 717 RVA: 0x00016BAD File Offset: 0x00014DAD
	public void SetVibration(float _level, float _duration)
	{
	}

	// Token: 0x0400035E RID: 862
	public static Input_Manager instance;

	// Token: 0x0400035F RID: 863
	public int beforeInputPlayerIndex;

	// Token: 0x04000360 RID: 864
	public int lastInputPlayerIndex;

	// Token: 0x04000361 RID: 865
	public string[] currentScheme = new string[4];

	// Token: 0x04000362 RID: 866
	public Input_Controller inputControllerRes;

	// Token: 0x04000363 RID: 867
	public List<Input_Controller> inputControllers = new List<Input_Controller>();

	// Token: 0x04000364 RID: 868
	public int connectedJoysticksQnt;

	// Token: 0x04000365 RID: 869
	private List<InputHint_Controller> inputHints = new List<InputHint_Controller>();

	// Token: 0x04000366 RID: 870
	public string[] debug_devices = new string[10];

	// Token: 0x04000367 RID: 871
	public ReadOnlyArray<InputDevice> last_active_device;
}
