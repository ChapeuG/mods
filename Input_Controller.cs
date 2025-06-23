using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;

// Token: 0x02000015 RID: 21
public class Input_Controller : MonoBehaviour
{
	// Token: 0x060000C0 RID: 192 RVA: 0x00008A68 File Offset: 0x00006C68
	public void _Start(int _index)
	{
		this.playerIndex = _index;
		this.playerInput = base.GetComponent<PlayerInput>();
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00008A7D File Offset: 0x00006C7D
	public void Set_Cooldown(float _f)
	{
		if (_f < this.coolDownTime_Timer)
		{
			return;
		}
		this.coolDownTime_Timer = _f;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00008A90 File Offset: 0x00006C90
	public void Set_Device(ReadOnlyArray<InputDevice> _devices)
	{
		if (_devices.Count <= 0)
		{
			return;
		}
		this.playerInput.user.UnpairDevices();
		if (_devices[0].name == "Keyboard" || _devices[0].name == "Mouse")
		{
			this.playerInput.SwitchCurrentControlScheme(new InputDevice[]
			{
				Keyboard.current,
				Mouse.current
			});
		}
		else
		{
			this.playerInput.SwitchCurrentControlScheme(new InputDevice[]
			{
				_devices[0]
			});
		}
		foreach (InputDevice device in _devices)
		{
			InputUser.PerformPairingWithDevice(device, this.playerInput.user, InputUserPairingOptions.None);
		}
		this.inputDevice = _devices[0];
		this.Refresh_Scheme();
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00008B90 File Offset: 0x00006D90
	public void Refresh_Scheme()
	{
		if (this.playerInput.devices[0].name == "Keyboard" || this.playerInput.devices[0].name == "Mouse")
		{
			this.playerInput.SwitchCurrentControlScheme(new InputDevice[]
			{
				Keyboard.current,
				Mouse.current
			});
			Input_Manager.instance.currentScheme[this.playerIndex] = "Keyboard&Mouse";
			base.gameObject.GetComponent<EventSystem>().enabled = true;
			base.gameObject.GetComponent<InputSystemUIInputModule>().enabled = true;
			return;
		}
		this.playerInput.SwitchCurrentControlScheme(new InputDevice[]
		{
			this.playerInput.devices[0]
		});
		Input_Manager.instance.currentScheme[this.playerIndex] = "Joystick";
		base.gameObject.GetComponent<EventSystem>().enabled = false;
		base.gameObject.GetComponent<InputSystemUIInputModule>().enabled = false;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00008CA3 File Offset: 0x00006EA3
	public void DeviceDisconnected()
	{
		if (this.playerIndex > 0)
		{
			Input_Manager.instance.Delete_InputControllers(this.playerIndex);
			return;
		}
		Game_Manager.instance.PauseGame(true);
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00008CCC File Offset: 0x00006ECC
	public void UpdateInput()
	{
		if (Keyboard.current.fKey.wasPressedThisFrame)
		{
			Cheat_Manager.instance.DebugMenu_Set("f");
		}
		if (Keyboard.current.dKey.wasPressedThisFrame)
		{
			Cheat_Manager.instance.DebugMenu_Set("d");
		}
		if (Keyboard.current.eKey.wasPressedThisFrame)
		{
			Cheat_Manager.instance.DebugMenu_Set("e");
		}
		if (Keyboard.current.bKey.wasPressedThisFrame)
		{
			Cheat_Manager.instance.DebugMenu_Set("b");
		}
		if (Keyboard.current.uKey.wasPressedThisFrame)
		{
			Cheat_Manager.instance.DebugMenu_Set("u");
		}
		if (Keyboard.current.gKey.wasPressedThisFrame)
		{
			Cheat_Manager.instance.DebugMenu_Set("g");
		}
		if (Keyboard.current.enterKey.wasPressedThisFrame)
		{
			Cheat_Manager.instance.DebugMenu_Set("!");
		}
		if (this.playerInput.user.lostDevices.Count >= 0 && this.inputDevice != null)
		{
			using (ReadOnlyArray<InputDevice>.Enumerator enumerator = this.playerInput.user.lostDevices.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == this.inputDevice)
					{
						this.DeviceDisconnected();
						this.inputDevice = null;
						return;
					}
				}
			}
		}
		this.coolDownTime_Timer -= Time.unscaledDeltaTime;
		string menuName = Menu_Manager.instance.GetMenuName();
		if ((menuName == "PC" || menuName == "Locker" || menuName == "Warning") && !Menu_Manager.instance.Get_Same_Specific_Player_Index(this.playerIndex))
		{
			return;
		}
		this.moveVec2 = this.playerInput.actions["Move"].ReadValue<Vector2>();
		if (menuName != "MainMenu" && menuName != "EE_Game" && this.Is_Allowed_For_UI() && Menu_Manager.instance.Get_Same_Specific_Player_Index(this.playerIndex))
		{
			if (this.playerInput.actions["ArrowUp"].triggered || (this.moveVec2.y > 0.1f && this.analogNavAvailable))
			{
				Menu_Manager.instance.MenuNav(0, 1);
			}
			else if (this.playerInput.actions["ArrowDown"].triggered || (this.moveVec2.y < -0.1f && this.analogNavAvailable))
			{
				Menu_Manager.instance.MenuNav(0, -1);
			}
			else if (this.playerInput.actions["ArrowLeft"].triggered || (this.moveVec2.x < -0.1f && this.analogNavAvailable))
			{
				Menu_Manager.instance.MenuNav(-1, 0);
			}
			else if (this.playerInput.actions["ArrowRight"].triggered || (this.moveVec2.x > 0.1f && this.analogNavAvailable))
			{
				Menu_Manager.instance.MenuNav(1, 0);
			}
			this.analogNavAvailable = false;
		}
		else if (menuName == "MainMenu" || menuName == "EE_Game")
		{
			Player_Manager.instance.GetPlayerController(this.playerIndex).Move(this.moveVec2);
			if (Cheat_Manager.instance.GetFreeCamera() == 1 && !this.playerInput.actions["ArrowUp"].triggered && !this.playerInput.actions["ArrowDown"].triggered && !this.playerInput.actions["ArrowLeft"].triggered)
			{
				bool triggered = this.playerInput.actions["ArrowRight"].triggered;
			}
		}
		if (Mathf.Abs(this.moveVec2.magnitude) < 0.1f)
		{
			this.analogNavAvailable = true;
		}
		this.aimVec2 = this.playerInput.actions["Look"].ReadValue<Vector2>();
		this.aimMagnitude = this.aimVec2.magnitude;
		if (menuName == "MainMenu" && !Player_Manager.instance.GetPlayerController(this.playerIndex).isMiniGaming)
		{
			if (this.coolDownTime_Timer > 0f)
			{
				return;
			}
			Camera_Controller.instance.Rotate(this.aimVec2, this.playerIndex);
		}
		else
		{
			Camera_Controller.instance.Rotate(Vector2.zero, this.playerIndex);
		}
		this.aimPresets = this.playerInput.actions["AimPresets"].triggered;
		if (this.aimPresets && menuName == "MainMenu")
		{
			Camera_Controller.instance.SetAnglePreset();
		}
		this.run = this.playerInput.actions["Run"].inProgress;
		if (menuName == "MainMenu")
		{
			Player_Manager.instance.GetPlayerController(this.playerIndex).Run(this.run);
		}
		if (menuName == "MainMenu" && this.playerInput.actions["Tasks"].triggered)
		{
			Menu_Manager.instance.Tasks_Cycle_State();
		}
		if (menuName == "EE_Game" && this.playerInput.actions["Dash"].triggered)
		{
			Player_Manager.instance.GetPlayerController(this.playerIndex).Dash();
		}
		if (this.playerInput.actions["Start"].triggered && this.playerIndex == 0)
		{
			if (menuName == "MainMenu" || menuName == "None" || menuName == "EE_Game")
			{
				Menu_Manager.instance.SetMenuName("Pause");
			}
			else if (menuName == "Pause" || menuName == "PC")
			{
				Menu_Manager.instance.SetMenuName("MainMenu");
			}
		}
		if (this.playerInput.actions["Back"].triggered && menuName != "MainMenu" && menuName != "PC" && menuName != "EE_Game" && this.Is_Allowed_For_UI() && Menu_Manager.instance.Get_Same_Specific_Player_Index(this.playerIndex))
		{
			Menu_Manager.instance.BackMenu();
		}
		if (this.playerInput.actions["InteractButton"].triggered && menuName != "MainMenu" && menuName != "PC" && menuName != "EE_Game" && this.Is_Allowed_For_UI() && Menu_Manager.instance.Get_Same_Specific_Player_Index(this.playerIndex))
		{
			Menu_Manager.instance.PressButton();
		}
		if (this.playerInput.actions["RB"].triggered && menuName == "PC" && Menu_Manager.instance.Get_Same_Specific_Player_Index(this.playerIndex))
		{
			PC_Manager.instance.NavTab(1);
		}
		if (this.playerInput.actions["LB"].triggered && menuName == "PC" && Menu_Manager.instance.Get_Same_Specific_Player_Index(this.playerIndex))
		{
			PC_Manager.instance.NavTab(-1);
		}
		if (this.playerInput.actions["RT"].triggered && menuName == "PC" && Menu_Manager.instance.Get_Same_Specific_Player_Index(this.playerIndex))
		{
			if (PC_Manager.instance.GetTab() == 1)
			{
				PC_Manager.instance.NavShopCat(1);
			}
			if (PC_Manager.instance.GetTab() == 8)
			{
				PC_Manager.instance.Mail_NavCat(1);
			}
		}
		if (this.playerInput.actions["LT"].triggered && menuName == "PC" && Menu_Manager.instance.Get_Same_Specific_Player_Index(this.playerIndex))
		{
			if (PC_Manager.instance.GetTab() == 1)
			{
				PC_Manager.instance.NavShopCat(-1);
			}
			if (PC_Manager.instance.GetTab() == 8)
			{
				PC_Manager.instance.Mail_NavCat(-1);
			}
		}
		if (this.playerInput.actions["RB"].triggered && menuName == "Locker" && Menu_Manager.instance.Get_Same_Specific_Player_Index(this.playerIndex))
		{
			Menu_Manager.instance.NavLockerTab(1);
		}
		if (this.playerInput.actions["LB"].triggered && menuName == "Locker" && Menu_Manager.instance.Get_Same_Specific_Player_Index(this.playerIndex))
		{
			Menu_Manager.instance.NavLockerTab(-1);
		}
		if (menuName == "MainMenu" || menuName == "MiniGame" || menuName == "EE_Game")
		{
			if (this.coolDownTime_Timer > 0f)
			{
				return;
			}
			if (Player_Manager.instance.GetPlayerController(this.playerIndex).isMiniGaming)
			{
				Interactor_Manager.instance.ui_ctrls[this.playerIndex].miniGame_manager.MiniGame_AimInputs(this.aimVec2, this.playerIndex);
			}
			this.holdButtonTimer += Time.deltaTime;
			if (!this.mayButtonUpAgain)
			{
				this.mayButtonUpAgain = true;
				return;
			}
			if (this.playerInput.actions["Int0"].triggered && this.mayButtonUpAgain)
			{
				this.mayButtonUpAgain = false;
				Player_Manager.instance.GetPlayerController(this.playerIndex).UseInteractive(false, true, false, false, false);
			}
			if (this.playerInput.actions["Int1"].triggered && this.mayButtonUpAgain)
			{
				this.mayButtonUpAgain = false;
				Player_Manager.instance.GetPlayerController(this.playerIndex).UseInteractive(false, false, true, false, false);
			}
			if (this.playerInput.actions["Int2"].triggered && this.mayButtonUpAgain)
			{
				this.mayButtonUpAgain = false;
				Player_Manager.instance.GetPlayerController(this.playerIndex).UseInteractive(false, false, false, true, false);
			}
			if (this.playerInput.actions["Int3"].triggered && this.mayButtonUpAgain)
			{
				this.mayButtonUpAgain = false;
				Player_Manager.instance.GetPlayerController(this.playerIndex).UseInteractive(false, false, false, false, true);
			}
			if (this.playerInput.actions["CycleItems"].triggered && this.mayButtonUpAgain)
			{
				this.mayButtonUpAgain = false;
				Player_Manager.instance.GetPlayerController(this.playerIndex).ChangeCurrentControllerIndex(1);
			}
			else if (this.playerInput.actions["RB"].triggered && this.mayButtonUpAgain)
			{
				this.mayButtonUpAgain = false;
				Player_Manager.instance.GetPlayerController(this.playerIndex).ChangeCurrentControllerIndex(1);
			}
			else if (this.playerInput.actions["LB"].triggered && this.mayButtonUpAgain)
			{
				this.mayButtonUpAgain = false;
				Player_Manager.instance.GetPlayerController(this.playerIndex).ChangeCurrentControllerIndex(-1);
			}
		}
		else if (menuName == "PC" && Input_Manager.instance.GetScheme(-1) == "Joystick" && Menu_Manager.instance.Get_Same_Specific_Player_Index(this.playerIndex))
		{
			if (this.playerInput.actions["Int0"].triggered)
			{
				PC_Manager.instance.UseButton(0);
			}
			if (this.playerInput.actions["Int1"].triggered)
			{
				PC_Manager.instance.UseButton(1);
			}
			if (this.playerInput.actions["Int2"].triggered)
			{
				PC_Manager.instance.UseButton(2);
			}
			if (this.playerInput.actions["Int3"].triggered)
			{
				PC_Manager.instance.UseButton(3);
			}
		}
		if (menuName == "SaveLoad" && Input_Manager.instance.GetScheme(-1) == "Joystick" && this.playerIndex == 0)
		{
			if (this.playerInput.actions["Int0"].triggered)
			{
				Save_Manager.instance.LoadGame(Save_Manager.instance.joystickIndexing);
			}
			if (this.playerInput.actions["Int1"].triggered)
			{
				Save_Manager.instance.DeleteGame(Save_Manager.instance.joystickIndexing);
			}
		}
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x000099A8 File Offset: 0x00007BA8
	private bool Is_Allowed_For_UI()
	{
		string menuName = Menu_Manager.instance.GetMenuName();
		return this.playerIndex == 0 || (!(menuName == "Start") && !(menuName == "Pause") && !(menuName == "Settings") && !(menuName == "Multiplayer"));
	}

	// Token: 0x04000106 RID: 262
	public int playerIndex;

	// Token: 0x04000107 RID: 263
	public PlayerInput playerInput;

	// Token: 0x04000108 RID: 264
	public InputDevice inputDevice;

	// Token: 0x04000109 RID: 265
	public float holdButtonTimer;

	// Token: 0x0400010A RID: 266
	public float buttonHeldCoolDown = 0.1f;

	// Token: 0x0400010B RID: 267
	public bool mayButtonUpAgain = true;

	// Token: 0x0400010C RID: 268
	private bool analogNavAvailable;

	// Token: 0x0400010D RID: 269
	private Vector2 moveVec2;

	// Token: 0x0400010E RID: 270
	private Vector2 aimVec2;

	// Token: 0x0400010F RID: 271
	private float aimMagnitude;

	// Token: 0x04000110 RID: 272
	private bool aimPresets;

	// Token: 0x04000111 RID: 273
	private bool run;

	// Token: 0x04000112 RID: 274
	private bool start;

	// Token: 0x04000113 RID: 275
	public float coolDownTime_Timer;

	// Token: 0x04000114 RID: 276
	public float coolDownTime = 0.1f;

	// Token: 0x04000115 RID: 277
	public string current_scheme = "";

	// Token: 0x04000116 RID: 278
	public string old_scheme = "";
}
