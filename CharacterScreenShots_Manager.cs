using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200003A RID: 58
public class CharacterScreenShots_Manager : MonoBehaviour
{
	// Token: 0x0600020D RID: 525 RVA: 0x0001312C File Offset: 0x0001132C
	private void Awake()
	{
		this.characters.Add(this.playerCharacter);
		GameObject[] array = Resources.LoadAll<GameObject>("Characters/Customers/Prefabs");
		for (int i = 0; i < array.Length; i++)
		{
			this.characters.Add(array[i]);
		}
	}

	// Token: 0x0600020E RID: 526 RVA: 0x00013171 File Offset: 0x00011371
	private void Start()
	{
		this.ChangeCharacter(0);
		Time.timeScale = 0.5f;
	}

	// Token: 0x0600020F RID: 527 RVA: 0x00013184 File Offset: 0x00011384
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Time.timeScale = 1f;
			SceneManager.LoadScene("MainGame");
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			this.CycleCharacter(1);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.CycleCharacter(-1);
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			this.CycleBasket();
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			this.RotateLight(1);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this.RotateLight(-1);
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			this.CycleBackgroundColors();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			this.ResetCamera();
		}
		this.RotateCamera();
	}

	// Token: 0x06000210 RID: 528 RVA: 0x00013230 File Offset: 0x00011430
	private void ChangeCharacter(int _index)
	{
		this.currentCharacter_Index = _index;
		if (this.currentCharacter)
		{
			UnityEngine.Object.Destroy(this.currentCharacter);
		}
		this.currentCharacter = UnityEngine.Object.Instantiate<GameObject>(this.characters[_index]);
		this.currentCharacter.transform.position = Vector3.zero;
		this.currentCharacter.transform.localScale = Vector3.one;
		this.currentCharacter.transform.rotation = Quaternion.Euler(Vector3.zero);
		base.Invoke("RefreshBasket", 0.5f);
	}

	// Token: 0x06000211 RID: 529 RVA: 0x000132C8 File Offset: 0x000114C8
	private void CycleCharacter(int _direction)
	{
		this.currentCharacter_Index += _direction;
		if (this.currentCharacter_Index >= this.characters.Count)
		{
			this.currentCharacter_Index = 0;
		}
		else if (this.currentCharacter_Index < 0)
		{
			this.currentCharacter_Index = this.characters.Count - 1;
		}
		this.ChangeCharacter(this.currentCharacter_Index);
	}

	// Token: 0x06000212 RID: 530 RVA: 0x00013327 File Offset: 0x00011527
	private void CycleBasket()
	{
		this.basketMode++;
		if (this.basketMode >= 3)
		{
			this.basketMode = 0;
		}
		this.RefreshBasket();
	}

	// Token: 0x06000213 RID: 531 RVA: 0x00013350 File Offset: 0x00011550
	private void RefreshBasket()
	{
		if (this.currentCharacter_Index != 0)
		{
			if (this.basketMode == 0)
			{
				this.currentCharacter.GetComponent<Customer_Controller>().SetBasket(false, false);
			}
			if (this.basketMode == 1)
			{
				this.currentCharacter.GetComponent<Customer_Controller>().SetBasket(true, false);
			}
			if (this.basketMode == 2)
			{
				this.currentCharacter.GetComponent<Customer_Controller>().SetBasket(false, true);
			}
		}
	}

	// Token: 0x06000214 RID: 532 RVA: 0x000133B8 File Offset: 0x000115B8
	private void RotateLight(int _direction)
	{
		Quaternion rotation = this.light.transform.rotation;
		Quaternion b = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y + (float)_direction, rotation.eulerAngles.z);
		Quaternion.Lerp(rotation, b, this.light_RotSpeed * Time.unscaledDeltaTime);
	}

	// Token: 0x06000215 RID: 533 RVA: 0x00013418 File Offset: 0x00011618
	private void CycleBackgroundColors()
	{
		this.backgroundColors_Index++;
		if (this.backgroundColors_Index >= this.backgroundColors.Length)
		{
			this.backgroundColors_Index = 0;
		}
		this.cam.backgroundColor = this.backgroundColors[this.backgroundColors_Index];
	}

	// Token: 0x06000216 RID: 534 RVA: 0x00013468 File Offset: 0x00011668
	private void RotateCamera()
	{
		if (!this.mainCamera)
		{
			this.mainCamera = this.cam.transform;
		}
		this.rotRaw = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
		float num = 5f;
		if (Input.GetMouseButton(1))
		{
			Quaternion b = Quaternion.Euler(this.cameraParent.rotation.eulerAngles.x + this.rotRaw.y * this.rotSpeed.y * num, this.cameraParent.rotation.eulerAngles.y + this.rotRaw.x * this.rotSpeed.x * num, 0f);
			this.cameraParent.rotation = Quaternion.Lerp(this.cameraParent.rotation, b, this.rotSpeed.x * Time.unscaledDeltaTime);
			return;
		}
		if (this.freeCameraZoomPos == Vector3.zero)
		{
			this.freeCameraZoomPos = this.mainCamera.localPosition;
		}
		if (Input.mouseScrollDelta.y > 0f)
		{
			this.freeCameraZoomPos.z = this.mainCamera.localPosition.z + this.cameraManualScrollSpeed;
		}
		else if (Input.mouseScrollDelta.y < 0f)
		{
			this.freeCameraZoomPos.z = this.mainCamera.localPosition.z - this.cameraManualScrollSpeed;
		}
		this.mainCamera.localPosition = Vector3.Lerp(this.mainCamera.localPosition, this.freeCameraZoomPos, this.cameraManualMoveSpeed * Time.unscaledDeltaTime);
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0001361C File Offset: 0x0001181C
	private void ResetCamera()
	{
		this.cameraParent.transform.rotation = Quaternion.Euler(Vector3.zero);
		this.rotRaw = Vector2.zero;
		this.mainCamera.localRotation = Quaternion.Euler(Vector3.zero);
		this.mainCamera.localPosition = Vector3.zero;
	}

	// Token: 0x040002EC RID: 748
	[Header("Characters")]
	[SerializeField]
	private GameObject playerCharacter;

	// Token: 0x040002ED RID: 749
	private List<GameObject> characters = new List<GameObject>();

	// Token: 0x040002EE RID: 750
	private GameObject currentCharacter;

	// Token: 0x040002EF RID: 751
	private int currentCharacter_Index;

	// Token: 0x040002F0 RID: 752
	private int basketMode = 1;

	// Token: 0x040002F1 RID: 753
	[Header("Light")]
	[SerializeField]
	private GameObject light;

	// Token: 0x040002F2 RID: 754
	[SerializeField]
	private float light_RotSpeed;

	// Token: 0x040002F3 RID: 755
	[Header("Background")]
	[SerializeField]
	private Color[] backgroundColors;

	// Token: 0x040002F4 RID: 756
	[SerializeField]
	private Camera cam;

	// Token: 0x040002F5 RID: 757
	private int backgroundColors_Index;

	// Token: 0x040002F6 RID: 758
	[SerializeField]
	private Transform cameraParent;

	// Token: 0x040002F7 RID: 759
	[SerializeField]
	private Transform cameraStartTransform;

	// Token: 0x040002F8 RID: 760
	private float cameraManualMoveSpeed = 5f;

	// Token: 0x040002F9 RID: 761
	private float cameraManualScrollSpeed = 5f;

	// Token: 0x040002FA RID: 762
	private Vector2 rotSpeed = new Vector2(10f, -5f);

	// Token: 0x040002FB RID: 763
	private Vector2 rotRaw;

	// Token: 0x040002FC RID: 764
	private Vector3 freeCameraZoomPos;

	// Token: 0x040002FD RID: 765
	private Transform mainCamera;
}
