using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000007 RID: 7
public class Camera_Controller : MonoBehaviour
{
	// Token: 0x06000024 RID: 36 RVA: 0x00002D18 File Offset: 0x00000F18
	private void Awake()
	{
		if (!Camera_Controller.instance)
		{
			Camera_Controller.instance = this;
		}
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002D2C File Offset: 0x00000F2C
	private void Start()
	{
		this.playerController = Player_Manager.instance.GetPlayerController(0);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002D3F File Offset: 0x00000F3F
	private void Update()
	{
		if (Game_Manager.instance.GetGameMode() != 0 || Game_Manager.instance.GetGameStage() < 99)
		{
			return;
		}
		this.Follow_Update();
		this.Rotate_Update();
		this.Zoom_Update();
		this.AnglePreset_Update();
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002D74 File Offset: 0x00000F74
	private void Follow_Update()
	{
		this.playerController = Player_Manager.instance.GetPlayerController(0);
		float num = this.followSpeed;
		int freeCamera = Cheat_Manager.instance.GetFreeCamera();
		int firstPerson = Cheat_Manager.instance.GetFirstPerson();
		int thirdPerson = Cheat_Manager.instance.GetThirdPerson();
		if (this.playerController && freeCamera == 0)
		{
			if (firstPerson == 1)
			{
				num = this.followSpeed * 10f;
			}
			if (thirdPerson == 1)
			{
				num = this.followSpeed * 10f;
			}
			Vector3 vector = Player_Manager.instance.GetPlayerController(0).transform.position;
			if (vector.y > 10f)
			{
				return;
			}
			int count = Player_Manager.instance.playerControllerList.Count;
			if (count >= 2)
			{
				for (int i = 1; i < count; i++)
				{
					Vector3 vector2 = vector;
					Vector3 position = Player_Manager.instance.GetPlayerController(i).transform.position;
					vector = vector2 + 0.5f * (position - vector2);
				}
			}
			base.transform.position = Vector3.Lerp(base.transform.position, vector, num * Time.unscaledDeltaTime);
		}
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002E9C File Offset: 0x0000109C
	private void Rotate_Update()
	{
		int freeCamera = Cheat_Manager.instance.GetFreeCamera();
		int firstPerson = Cheat_Manager.instance.GetFirstPerson();
		int thirdPerson = Cheat_Manager.instance.GetThirdPerson();
		if (Menu_Manager.instance.GetMenuName() == "Locker")
		{
			this.freeCameraZoomPos = Vector3.zero;
			Player_Controller player_Controller = Player_Manager.instance.GetPlayerController(Menu_Manager.instance.locker_player_index);
			if (player_Controller.locker_Controller && this.mainCamera.transform.parent != player_Controller.locker_Controller.cameraPlace.transform)
			{
				this.mainCamera.transform.SetParent(player_Controller.locker_Controller.cameraPlace.transform);
				this.mainCamera.GetComponent<Camera>().fieldOfView = 30f;
			}
			this.mainCamera.transform.localRotation = Quaternion.Lerp(this.mainCamera.transform.localRotation, Quaternion.Euler(Vector3.zero), this.autoRotSpeed * Time.unscaledDeltaTime);
			this.mainCamera.transform.localPosition = Vector3.Lerp(this.mainCamera.transform.localPosition, Vector3.zero, this.autoMoveSpeed * Time.unscaledDeltaTime);
			return;
		}
		if (Menu_Manager.instance.GetMenuName() == "EE_Game")
		{
			this.freeCameraZoomPos = Vector3.zero;
			if (EasterEgg_Manager.instance.Get_Is_MiniGaming() && this.mainCamera.transform.position != EasterEgg_Manager.instance.camera_holder.transform.position)
			{
				this.mainCamera.transform.position = EasterEgg_Manager.instance.camera_holder.transform.position;
				this.mainCamera.transform.rotation = EasterEgg_Manager.instance.camera_holder.transform.rotation;
				this.mainCamera.GetComponent<Camera>().fieldOfView = 30f;
			}
			return;
		}
		if (this.playerController && freeCamera == 0 && firstPerson == 0 && thirdPerson == 0)
		{
			this.freeCameraZoomPos = Vector3.zero;
			if (this.mainCamera.transform.parent != this.cameraHolder.transform)
			{
				this.mainCamera.transform.SetParent(this.cameraHolder.transform);
				this.mainCamera.GetComponent<Camera>().fieldOfView = 30f;
			}
			this.mainCamera.transform.localRotation = Quaternion.Lerp(this.mainCamera.transform.localRotation, Quaternion.Euler(Vector3.zero), this.autoRotSpeed * Time.unscaledDeltaTime);
			this.mainCamera.transform.localPosition = Vector3.Lerp(this.mainCamera.transform.localPosition, Vector3.zero, this.autoMoveSpeed * Time.unscaledDeltaTime);
		}
		int count = Player_Manager.instance.playerControllerList.Count;
		if (this.playerController && (this.rotRaw.magnitude != 0f || count > 1) && freeCamera == 0 && firstPerson == 0 && thirdPerson == 0)
		{
			if (Player_Manager.instance.GetPlayerController(0).transform.position.y > 10f)
			{
				return;
			}
			Vector2 vector = this.rotXLimit;
			if (count > 1)
			{
				float num = 0f;
				foreach (Player_Controller player_Controller2 in Player_Manager.instance.playerControllerList)
				{
					foreach (Player_Controller player_Controller3 in Player_Manager.instance.playerControllerList)
					{
						if (!(player_Controller2 == player_Controller3))
						{
							float num2 = Vector3.Distance(player_Controller2.transform.position, player_Controller3.transform.position);
							if (num2 > num)
							{
								num = num2;
							}
						}
					}
				}
				this.multi_bigger_distance = num;
				vector.x = Mathf.Clamp(this.rotXLimit.x * this.multi_bigger_distance * this.multi_angle_dist_multiplier, this.rotXLimit.x, this.rotXLimit.y);
				this.rotRaw.y = this.multi_rot_raw_vertical;
			}
			Quaternion b = Quaternion.Euler(Mathf.Clamp(base.transform.rotation.eulerAngles.x + this.rotRaw.y * this.rotSpeed.y * this.Get_Sensitivity(), vector.x, this.rotXLimit.y), base.transform.rotation.eulerAngles.y + this.rotRaw.x * this.rotSpeed.x * this.Get_Sensitivity(), 0f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, this.rotSpeed.x * Time.unscaledDeltaTime);
			return;
		}
		else
		{
			if (this.rotRaw.magnitude != 0f && thirdPerson == 0 && firstPerson == 1)
			{
				if (this.mainCamera.transform.parent != this.cameraFirstPersonHolder.transform)
				{
					this.mainCamera.transform.SetParent(this.cameraFirstPersonHolder.transform);
					this.mainCamera.transform.localRotation = Quaternion.Euler(Vector3.zero);
					this.mainCamera.transform.localPosition = Vector3.zero;
					base.transform.rotation = Quaternion.Euler(Vector3.zero);
					this.mainCamera.GetComponent<Camera>().fieldOfView = 70f;
				}
				Quaternion b2 = Quaternion.Euler(this.mainCamera.rotation.eulerAngles.x + this.rotRaw.y * this.rotSpeed.y * this.Get_Sensitivity(), this.mainCamera.rotation.eulerAngles.y + this.rotRaw.x * this.rotSpeed.x * this.Get_Sensitivity(), 0f);
				this.mainCamera.rotation = Quaternion.Lerp(this.mainCamera.rotation, b2, this.rotSpeed.x * Time.unscaledDeltaTime);
				return;
			}
			if (this.rotRaw.magnitude != 0f && firstPerson == 0 && thirdPerson == 1)
			{
				if (this.mainCamera.transform.parent != this.cameraThirdPersonHolder.transform)
				{
					this.mainCamera.transform.SetParent(this.cameraThirdPersonHolder.transform);
					this.mainCamera.transform.localRotation = Quaternion.Euler(Vector3.zero);
					this.mainCamera.transform.localPosition = Vector3.zero;
					base.transform.rotation = Quaternion.Euler(Vector3.zero);
					this.mainCamera.GetComponent<Camera>().fieldOfView = 30f;
					this.cameraThirdPersonRotator.transform.localRotation = Quaternion.Euler(Vector3.zero);
				}
				float num3 = 1f;
				if (Input_Manager.instance.GetScheme(this.lastPlayerToRotate_index) == "Keyboard&Mouse")
				{
					num3 = Settings_Manager.instance.GetMouseSensitivity(0.5f);
				}
				else if (Input_Manager.instance.GetScheme(this.lastPlayerToRotate_index) == "Joystick")
				{
					num3 = Settings_Manager.instance.GetGamepadSensitivity(1f);
				}
				Quaternion b3 = Quaternion.Euler(this.cameraThirdPersonRotator.rotation.eulerAngles.x + this.rotRaw.y * this.rotSpeed.y * num3, this.cameraThirdPersonRotator.rotation.eulerAngles.y + this.rotRaw.x * this.rotSpeed.x * num3, 0f);
				this.cameraThirdPersonRotator.rotation = Quaternion.Lerp(this.mainCamera.rotation, b3, this.rotSpeed.x * Time.unscaledDeltaTime);
				return;
			}
			if (freeCamera == 1)
			{
				if (this.mainCamera.transform.parent != null)
				{
					this.mainCamera.transform.SetParent(null);
					this.freeCameraZoomPos = this.mainCamera.position;
				}
				if ((Input_Manager.instance.GetScheme(this.lastPlayerToRotate_index) == "Keyboard&Mouse" && Mouse.current.leftButton.isPressed) || (Mathf.Abs(this.freeCamAimVec2.magnitude) > 0.1f && Input_Manager.instance.GetScheme(-1) == "Joystick"))
				{
					this.freeCameraZoomPos = this.mainCamera.localPosition + (this.mainCamera.forward * this.rotRaw.y + this.mainCamera.right * this.rotRaw.x * this.cameraManualMoveSpeed * this.Get_Sensitivity());
					this.freeCameraZoomPos.y = this.mainCamera.localPosition.y;
					this.mainCamera.localPosition = Vector3.Lerp(this.mainCamera.localPosition, this.freeCameraZoomPos, this.cameraManualLerp * Time.unscaledDeltaTime);
					this.rotRaw = Vector2.zero;
					return;
				}
				if (Input_Manager.instance.GetScheme(this.lastPlayerToRotate_index) == "Keyboard&Mouse" && Mouse.current.middleButton.isPressed)
				{
					this.freeCameraZoomPos = new Vector3(this.mainCamera.localPosition.x + (this.mainCamera.right * this.freeCamAimVec2.x * this.cameraManualMoveSpeed * this.Get_Sensitivity()).x, this.mainCamera.localPosition.y + this.rotRaw.y * this.cameraManualMoveSpeed * this.Get_Sensitivity(), this.mainCamera.localPosition.z + (this.mainCamera.right * this.freeCamAimVec2.x * this.cameraManualMoveSpeed * this.Get_Sensitivity()).z);
					this.mainCamera.localPosition = Vector3.Lerp(this.mainCamera.localPosition, this.freeCameraZoomPos, this.cameraManualLerp * Time.unscaledDeltaTime);
					this.rotRaw = Vector2.zero;
					return;
				}
				if (Input_Manager.instance.GetScheme(this.lastPlayerToRotate_index) == "Keyboard&Mouse" && Mouse.current.rightButton.isPressed)
				{
					this.freeCameraZoomPos = this.mainCamera.localPosition;
					Quaternion b4 = Quaternion.Euler(this.mainCamera.rotation.eulerAngles.x + this.rotRaw.y * this.rotSpeed.y * this.Get_Sensitivity() * 0.25f, this.mainCamera.rotation.eulerAngles.y + this.rotRaw.x * this.rotSpeed.x * this.Get_Sensitivity() * 0.25f, 0f);
					this.mainCamera.rotation = Quaternion.Lerp(this.mainCamera.rotation, b4, this.rotSpeed.x * Time.unscaledDeltaTime);
					return;
				}
				if (this.freeCameraZoomPos == Vector3.zero)
				{
					this.freeCameraZoomPos = this.mainCamera.localPosition;
				}
				if (Input.mouseScrollDelta.y > 0f)
				{
					this.freeCameraZoomPos += this.mainCamera.forward * this.cameraManualScrollSpeed;
				}
				else if (Input.mouseScrollDelta.y < 0f)
				{
					this.freeCameraZoomPos += this.mainCamera.forward * -this.cameraManualScrollSpeed;
				}
				this.mainCamera.localPosition = Vector3.Lerp(this.mainCamera.localPosition, this.freeCameraZoomPos, this.cameraManualMoveSpeed * Time.unscaledDeltaTime);
			}
			return;
		}
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00003B6C File Offset: 0x00001D6C
	public void Rotate(Vector2 vec2, int _player_index)
	{
		if (vec2 == Vector2.zero && this.lastPlayerToRotate_index != _player_index)
		{
			return;
		}
		if (Mathf.Abs(vec2.magnitude) <= 0.01f && this.updatePreset)
		{
			return;
		}
		this.updatePreset = false;
		this.rotRaw = vec2;
		this.lastPlayerToRotate_index = _player_index;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00003BC4 File Offset: 0x00001DC4
	private void Zoom_Update()
	{
		string menuName = Menu_Manager.instance.GetMenuName();
		if (menuName == "Locker" || menuName == "EE_Game")
		{
			return;
		}
		int freeCamera = Cheat_Manager.instance.GetFreeCamera();
		int firstPerson = Cheat_Manager.instance.GetFirstPerson();
		int thirdPerson = Cheat_Manager.instance.GetThirdPerson();
		if (freeCamera == 1 || firstPerson == 1 || thirdPerson == 1)
		{
			return;
		}
		Vector2 vector = this.zoomLimit;
		if (Player_Manager.instance.playerControllerList.Count > 1)
		{
			vector = this.zoomLimit * this.multi_bigger_distance * this.multi_zoom_max_multiplier;
		}
		float z = base.transform.rotation.eulerAngles.x * vector.y / this.rotXLimit.y;
		Vector3 b = new Vector3(this.mainCamera.transform.localPosition.x, this.mainCamera.transform.localPosition.y, z);
		this.mainCamera.transform.localPosition = Vector3.Lerp(this.mainCamera.transform.localPosition, b, this.zoomSpeed * Time.unscaledDeltaTime);
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00003CF0 File Offset: 0x00001EF0
	public float Get_Sensitivity()
	{
		float result = 1f;
		if (Input_Manager.instance.GetScheme(this.lastPlayerToRotate_index) == "Keyboard&Mouse")
		{
			result = Settings_Manager.instance.GetMouseSensitivity(0.02f);
		}
		else if (Input_Manager.instance.GetScheme(this.lastPlayerToRotate_index) == "Joystick")
		{
			result = Settings_Manager.instance.GetGamepadSensitivity(1f);
		}
		return result;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00003D5E File Offset: 0x00001F5E
	public void SetAnglePreset()
	{
		this.presetIndex++;
		if (this.presetIndex >= this.anglePresets.Length)
		{
			this.presetIndex = 0;
		}
		this.updatePreset = true;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00003D8C File Offset: 0x00001F8C
	public void AnglePreset_Update()
	{
		if (this.updatePreset)
		{
			Quaternion b = Quaternion.Euler(Mathf.Clamp(this.anglePresets[this.presetIndex], this.rotXLimit.x, this.rotXLimit.y), base.transform.rotation.eulerAngles.y, 0f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, this.rotSpeed.x * 1f * Time.unscaledDeltaTime);
			if (base.transform.rotation.eulerAngles.x == this.anglePresets[this.presetIndex])
			{
				this.updatePreset = false;
			}
		}
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00003E50 File Offset: 0x00002050
	public void ResetPosToPlayer()
	{
		int freeCamera = Cheat_Manager.instance.GetFreeCamera();
		if (this.playerController && freeCamera == 0)
		{
			base.transform.position = this.playerController.transform.position;
			base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, this.playerController.transform.rotation.eulerAngles.y, base.transform.rotation.eulerAngles.z);
		}
	}

	// Token: 0x0400002F RID: 47
	public static Camera_Controller instance;

	// Token: 0x04000030 RID: 48
	[SerializeField]
	private GameObject cameraHolder;

	// Token: 0x04000031 RID: 49
	[SerializeField]
	private GameObject cameraFirstPersonHolder;

	// Token: 0x04000032 RID: 50
	[SerializeField]
	private GameObject cameraThirdPersonHolder;

	// Token: 0x04000033 RID: 51
	[SerializeField]
	private Transform cameraThirdPersonRotator;

	// Token: 0x04000034 RID: 52
	private Player_Controller playerController;

	// Token: 0x04000035 RID: 53
	[Header("Variables")]
	[SerializeField]
	private float followSpeed;

	// Token: 0x04000036 RID: 54
	[Header("Auto Movement")]
	[SerializeField]
	private float autoMoveSpeed;

	// Token: 0x04000037 RID: 55
	[SerializeField]
	private float autoRotSpeed;

	// Token: 0x04000038 RID: 56
	[Header("Multiplayer")]
	[SerializeField]
	private float multi_rot_raw_vertical;

	// Token: 0x04000039 RID: 57
	[SerializeField]
	private float multi_angle_dist_multiplier = 0.5f;

	// Token: 0x0400003A RID: 58
	[SerializeField]
	private float multi_zoom_max_multiplier = 2f;

	// Token: 0x0400003B RID: 59
	private float multi_bigger_distance = 1f;

	// Token: 0x0400003C RID: 60
	[Header("Manual Movement")]
	[SerializeField]
	private Vector2 rotSpeed;

	// Token: 0x0400003D RID: 61
	[SerializeField]
	private Vector2 rotXLimit;

	// Token: 0x0400003E RID: 62
	[SerializeField]
	private Vector3 freeCameraPos;

	// Token: 0x0400003F RID: 63
	[SerializeField]
	private Vector3 freeCameraZoomPos;

	// Token: 0x04000040 RID: 64
	[SerializeField]
	private Vector2 freeCamAimVec2;

	// Token: 0x04000041 RID: 65
	private Vector2 rotRaw;

	// Token: 0x04000042 RID: 66
	private int lastPlayerToRotate_index;

	// Token: 0x04000043 RID: 67
	[Header("Zoom")]
	[SerializeField]
	public Transform mainCamera;

	// Token: 0x04000044 RID: 68
	[SerializeField]
	private Vector2 zoomLimit;

	// Token: 0x04000045 RID: 69
	[SerializeField]
	private float zoomSpeed;

	// Token: 0x04000046 RID: 70
	[Header("Angle Presets")]
	[SerializeField]
	private float[] anglePresets;

	// Token: 0x04000047 RID: 71
	[SerializeField]
	private float presetSpeed;

	// Token: 0x04000048 RID: 72
	private int presetIndex;

	// Token: 0x04000049 RID: 73
	private bool updatePreset;

	// Token: 0x0400004A RID: 74
	[Header("MoveCameraManual")]
	[SerializeField]
	private float cameraManualMoveSpeed = 1f;

	// Token: 0x0400004B RID: 75
	[SerializeField]
	private float cameraManualLerp = 1f;

	// Token: 0x0400004C RID: 76
	[SerializeField]
	private float cameraManualScrollSpeed = 1f;
}
