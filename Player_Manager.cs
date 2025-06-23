using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class Player_Manager : MonoBehaviour
{
	// Token: 0x06000461 RID: 1121 RVA: 0x0002C520 File Offset: 0x0002A720
	private void Awake()
	{
		if (!Player_Manager.instance)
		{
			Player_Manager.instance = this;
		}
		Player_Controller component = UnityEngine.Object.Instantiate<Player_Controller>(this.player_Normal, this.player_Normal.transform.parent).GetComponent<Player_Controller>();
		this.playerControllerList.Add(component);
		component.gameObject.SetActive(true);
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x0002C578 File Offset: 0x0002A778
	private void Start()
	{
		this.player_Normal.gameObject.SetActive(false);
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0002C58B File Offset: 0x0002A78B
	public Player_Controller GetPlayerController(int _index)
	{
		return this.playerControllerList[_index];
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0002C59C File Offset: 0x0002A79C
	public void CreateSecondPlayer(bool _b)
	{
		if (_b && this.playerControllerList.Count <= 4)
		{
			Player_Controller component = UnityEngine.Object.Instantiate<Player_Controller>(this.player_Normal, this.player_Normal.transform.parent).GetComponent<Player_Controller>();
			component.gameObject.SetActive(true);
			component.playerIndex = this.playerControllerList.Count;
			this.playerControllerList.Add(component);
			component.BackToMartRandomPos();
			component.skin_.SetCompleteCustomization(-1, -1, -1, -1, -1, -1, -1);
		}
		else
		{
			for (int i = this.playerControllerList.Count - 1; i > 0; i--)
			{
				UnityEngine.Object.Destroy(this.playerControllerList[i].gameObject);
				this.playerControllerList.RemoveAt(i);
			}
		}
		Menu_Manager.instance.Refresh_Multiplayer_Menu();
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0002C662 File Offset: 0x0002A862
	public void DeletePlayer(int index)
	{
		UnityEngine.Object.Destroy(this.playerControllerList[index].gameObject);
		this.playerControllerList.RemoveAt(index);
		Menu_Manager.instance.Refresh_Multiplayer_Menu();
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0002C690 File Offset: 0x0002A890
	public void Set_All_Players_Anim_Update_Mode(AnimatorUpdateMode _mode = AnimatorUpdateMode.Normal)
	{
		foreach (Player_Controller player_Controller in this.playerControllerList)
		{
			player_Controller.animator_.updateMode = _mode;
			player_Controller.animator_eyes.updateMode = _mode;
		}
		if (_mode == AnimatorUpdateMode.Normal)
		{
			this.Set_All_Players_Multiplayer_Pos(false);
			return;
		}
		this.Set_All_Players_Multiplayer_Pos(true);
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x0002C704 File Offset: 0x0002A904
	public void Set_All_Players_Multiplayer_Pos(bool _b)
	{
		for (int i = 0; i < this.playerControllerList.Count; i++)
		{
			Player_Controller player_Controller = this.playerControllerList[i];
			if (!(player_Controller == null))
			{
				if (_b)
				{
					if (player_Controller.transform.position.y < 100f)
					{
						this.normal_positions[i] = player_Controller.transform.position;
					}
					player_Controller.transform.position = this.multiplayer_positions[i].transform.position;
				}
				else if (player_Controller.transform.position.y > 100f)
				{
					player_Controller.transform.position = this.normal_positions[i];
				}
			}
		}
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x0002C7C1 File Offset: 0x0002A9C1
	public void SetPlayerSkinColor(int _index, int _player_index)
	{
		this.GetPlayerController(_player_index).skin_.SetMaterial_Skin(_index);
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x0002C7D8 File Offset: 0x0002A9D8
	public void SetPlayerOutfit(int _index, int _player_index)
	{
		this.GetPlayerController(_player_index).skin_.SetMaterial_Clothes(_index);
		this.GetPlayerController(_player_index).skin_.SetMaterial_Hat(_index);
		for (int i = 0; i < Char_Manager.instance.staff_Controllers.Count; i++)
		{
			if (Char_Manager.instance.staff_Controllers[i])
			{
				Char_Manager.instance.staff_Controllers[i].staff_Controller.skin_Controller.SetMaterial_Clothes(this.GetPlayerController(_player_index).skin_.clothesMat_Index);
				Char_Manager.instance.staff_Controllers[i].staff_Controller.skin_Controller.SetMaterial_Hat(this.GetPlayerController(_player_index).skin_.hatMat_Index);
			}
		}
		for (int j = 0; j < Char_Manager.instance.staff_Data.Count; j++)
		{
			if (Char_Manager.instance.staff_Controllers[j])
			{
				Char_Manager.instance.staff_Data[j].clothesMat = this.GetPlayerController(_player_index).skin_.clothesMat_Index;
				Char_Manager.instance.staff_Data[j].hatMat = this.GetPlayerController(_player_index).skin_.hatMat_Index;
			}
		}
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x0002C91A File Offset: 0x0002AB1A
	public void SetPlayerHairColor(int _index, int _player_index)
	{
		this.GetPlayerController(_player_index).skin_.SetMaterial_Hair(_index);
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x0002C92E File Offset: 0x0002AB2E
	public void SetPlayerHair(int _index, int _player_index)
	{
		this.GetPlayerController(_player_index).skin_.SetMesh_Hair(_index);
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x0002C942 File Offset: 0x0002AB42
	public void SetPlayerEyes(int _index, int _player_index)
	{
		this.GetPlayerController(_player_index).skin_.SetMaterial_Eye(_index);
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x0002C958 File Offset: 0x0002AB58
	public void SetPlayerHat(int _index, int _player_index)
	{
		this.GetPlayerController(_player_index).skin_.SetGO_Hat(_index);
		for (int i = 0; i < Char_Manager.instance.staff_Controllers.Count; i++)
		{
			if (Char_Manager.instance.staff_Controllers[i])
			{
				Char_Manager.instance.staff_Controllers[i].staff_Controller.skin_Controller.SetGO_Hat(this.GetPlayerController(_player_index).skin_.hatGo_Index);
			}
		}
		for (int j = 0; j < Char_Manager.instance.staff_Data.Count; j++)
		{
			if (Char_Manager.instance.staff_Controllers[j])
			{
				Char_Manager.instance.staff_Data[j].hatGo = this.GetPlayerController(_player_index).skin_.hatGo_Index;
			}
		}
	}

	// Token: 0x04000536 RID: 1334
	public static Player_Manager instance;

	// Token: 0x04000537 RID: 1335
	[SerializeField]
	private Player_Controller player_Normal;

	// Token: 0x04000538 RID: 1336
	public List<Player_Controller> playerControllerList = new List<Player_Controller>();

	// Token: 0x04000539 RID: 1337
	[Header("Positions")]
	public GameObject[] multiplayer_positions = new GameObject[4];

	// Token: 0x0400053A RID: 1338
	private Vector3[] normal_positions = new Vector3[4];

	// Token: 0x0400053B RID: 1339
	[Header("Customization")]
	[SerializeField]
	public List<Material> material_Clothes = new List<Material>();

	// Token: 0x0400053C RID: 1340
	[SerializeField]
	public List<Material> material_SkinColors = new List<Material>();

	// Token: 0x0400053D RID: 1341
	[SerializeField]
	public List<Mesh> mesh_Hairs = new List<Mesh>();

	// Token: 0x0400053E RID: 1342
	[SerializeField]
	public List<Material> material_HairColors = new List<Material>();

	// Token: 0x0400053F RID: 1343
	[SerializeField]
	public List<Material> material_Eyes = new List<Material>();

	// Token: 0x04000540 RID: 1344
	[SerializeField]
	public List<GameObject> hat_prefabs = new List<GameObject>();
}
