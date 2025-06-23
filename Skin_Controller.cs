using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class Skin_Controller : MonoBehaviour
{
	// Token: 0x06000176 RID: 374 RVA: 0x0000EB2C File Offset: 0x0000CD2C
	private void Start()
	{
		if (this.isGenericCustomer)
		{
			Player_Manager instance = Player_Manager.instance;
			Char_Manager instance2 = Char_Manager.instance;
			this.SetCompleteCustomization(UnityEngine.Random.Range(0, instance.material_SkinColors.Count), UnityEngine.Random.Range(0, instance2.genCustomer_Material_Clothes.Count), UnityEngine.Random.Range(0, instance2.genCustomer_Material_HairColors.Count), UnityEngine.Random.Range(0, instance2.genCustomer_Mesh_Hairs.Count), UnityEngine.Random.Range(0, instance.material_Clothes.Count), UnityEngine.Random.Range(0, instance.material_Eyes.Count), -1);
		}
	}

	// Token: 0x06000177 RID: 375 RVA: 0x0000EBBC File Offset: 0x0000CDBC
	public int[] GetCompleteCustomization()
	{
		return new int[]
		{
			this.skinMat_Index,
			this.clothesMat_Index,
			this.hairMat_Index,
			this.hairMesh_Index,
			this.hatMat_Index,
			this.eyeMat_Index,
			this.hatGo_Index
		};
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0000EC0E File Offset: 0x0000CE0E
	public int[] GetCompleteCustomizationUI()
	{
		return new int[]
		{
			this.skinMat_Index,
			this.clothesMat_Index,
			this.hairMat_Index,
			this.hairMesh_Index,
			this.eyeMat_Index,
			this.hatGo_Index
		};
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0000EC4C File Offset: 0x0000CE4C
	public void SetCompleteCustomization(int _skinMat, int _clothesMat, int _hairMat, int _hairMesh, int _hatMat, int _eyeMat, int _hatGo)
	{
		this.SetMaterial_Skin(_skinMat);
		this.SetMesh_Hair(_hairMesh);
		this.SetMaterial_Hair(_hairMat);
		this.SetMaterial_Clothes(_clothesMat);
		this.SetMaterial_Eye(_eyeMat);
		this.SetGO_Hat(_hatGo);
		if (_hatGo != -1)
		{
			this.SetMaterial_Hat(_clothesMat);
		}
	}

	// Token: 0x0600017A RID: 378 RVA: 0x0000EC88 File Offset: 0x0000CE88
	private int Get_RandomIndex(int _category, int _count)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < _count; i++)
		{
			if (Unlock_Manager.instance.item_Unlocked[_category, i])
			{
				list.Add(i);
			}
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x0600017B RID: 379 RVA: 0x0000ECD4 File Offset: 0x0000CED4
	public void SetMaterial_Skin(int _index)
	{
		if (_index == -1)
		{
			_index = this.Get_RandomIndex(6, Player_Manager.instance.material_SkinColors.Count);
		}
		Material material;
		if (this.isGenericCustomer)
		{
			material = Char_Manager.instance.genCustomer_Material_Clothes[this.clothesMat_Index];
		}
		else
		{
			material = Player_Manager.instance.material_Clothes[this.clothesMat_Index];
		}
		this.skinMat_Index = _index;
		Material material2 = Player_Manager.instance.material_SkinColors[this.skinMat_Index];
		Material[] materials = new Material[]
		{
			material,
			material2
		};
		this.mesh_Body.materials = materials;
		Material[] materials2 = new Material[]
		{
			material2
		};
		this.mesh_Head.materials = materials2;
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0000ED84 File Offset: 0x0000CF84
	public void SetMaterial_Clothes(int _index)
	{
		if (_index == -1)
		{
			_index = this.Get_RandomIndex(7, Player_Manager.instance.material_Clothes.Count);
		}
		if (this.isGenericCustomer && _index >= Char_Manager.instance.genCustomer_Material_Clothes.Count)
		{
			_index = Char_Manager.instance.genCustomer_Material_Clothes.Count - 1;
		}
		this.clothesMat_Index = _index;
		Material material;
		if (this.isGenericCustomer)
		{
			material = Char_Manager.instance.genCustomer_Material_Clothes[this.clothesMat_Index];
		}
		else
		{
			material = Player_Manager.instance.material_Clothes[this.clothesMat_Index];
		}
		Material material2 = Player_Manager.instance.material_SkinColors[this.skinMat_Index];
		Material[] materials = new Material[]
		{
			material,
			material2
		};
		this.mesh_Body.materials = materials;
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0000EE4C File Offset: 0x0000D04C
	public void SetMaterial_Hair(int _index)
	{
		if (_index == -1)
		{
			_index = this.Get_RandomIndex(8, Player_Manager.instance.material_HairColors.Count);
		}
		if (this.isGenericCustomer && _index >= Char_Manager.instance.genCustomer_Material_HairColors.Count)
		{
			_index = Char_Manager.instance.genCustomer_Material_HairColors.Count - 1;
		}
		this.hairMat_Index = _index;
		Material material;
		if (this.isGenericCustomer)
		{
			material = Char_Manager.instance.genCustomer_Material_HairColors[this.hairMat_Index];
		}
		else
		{
			material = Player_Manager.instance.material_HairColors[this.hairMat_Index];
		}
		Material[] materials = new Material[]
		{
			material
		};
		this.meshR_Hair.materials = materials;
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0000EEF8 File Offset: 0x0000D0F8
	public void SetMesh_Hair(int _index)
	{
		if (_index == -1)
		{
			_index = this.Get_RandomIndex(9, Player_Manager.instance.mesh_Hairs.Count);
		}
		if (this.isGenericCustomer && _index >= Char_Manager.instance.genCustomer_Mesh_Hairs.Count)
		{
			_index = Char_Manager.instance.genCustomer_Mesh_Hairs.Count - 1;
		}
		this.hairMesh_Index = _index;
		Mesh mesh;
		if (this.isGenericCustomer)
		{
			mesh = Char_Manager.instance.genCustomer_Mesh_Hairs[this.hairMesh_Index];
		}
		else
		{
			mesh = Player_Manager.instance.mesh_Hairs[this.hairMesh_Index];
		}
		this.meshF_Hair.mesh = mesh;
		this.mesh_Hat.GetComponent<Hat_Controller>().RefreshHat(this.hairMesh_Index);
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000EFB0 File Offset: 0x0000D1B0
	public void SetMaterial_Hat(int _index)
	{
		if (_index == -1)
		{
			this.SetGO_Hat(-1);
		}
		if (this.isGenericCustomer)
		{
			return;
		}
		this.hatMat_Index = _index;
		this.mesh_Hat.GetComponent<Hat_Controller>().ChangeHat(this.hatGo_Index, this.hatMat_Index);
		this.mesh_Hat.GetComponent<Hat_Controller>().RefreshHat(this.hairMesh_Index);
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000F00C File Offset: 0x0000D20C
	public void SetMaterial_Eye(int _index)
	{
		if (_index == -1)
		{
			_index = this.Get_RandomIndex(10, Player_Manager.instance.material_Eyes.Count);
		}
		if (this.mesh_Eye == null)
		{
			this.mesh_Eye = this.face_Controller.mesh_Eye;
		}
		this.eyeMat_Index = _index;
		Material material = Player_Manager.instance.material_Eyes[this.eyeMat_Index];
		Material[] materials = new Material[]
		{
			material
		};
		this.mesh_Eye.materials = materials;
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000F08C File Offset: 0x0000D28C
	public void SetMaterial_Eye(Material _mat)
	{
		if (this.mesh_Eye == null)
		{
			this.mesh_Eye = this.face_Controller.mesh_Eye;
		}
		Material[] materials = new Material[]
		{
			_mat
		};
		this.mesh_Eye.materials = materials;
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000F0D0 File Offset: 0x0000D2D0
	public void SetGO_Hat(int _index)
	{
		if (_index == -1)
		{
			_index = this.Get_RandomIndex(11, Player_Manager.instance.hat_prefabs.Count);
		}
		if (this.isGenericCustomer)
		{
			return;
		}
		this.hatGo_Index = _index;
		this.mesh_Hat.GetComponent<Hat_Controller>().ChangeHat(this.hatGo_Index, this.hatMat_Index);
		this.mesh_Hat.GetComponent<Hat_Controller>().RefreshHat(this.hairMesh_Index);
	}

	// Token: 0x040001FC RID: 508
	[SerializeField]
	public int skinMat_Index;

	// Token: 0x040001FD RID: 509
	[SerializeField]
	public int clothesMat_Index;

	// Token: 0x040001FE RID: 510
	[SerializeField]
	public int hairMat_Index;

	// Token: 0x040001FF RID: 511
	[SerializeField]
	public int hairMesh_Index;

	// Token: 0x04000200 RID: 512
	[SerializeField]
	public int hatMat_Index;

	// Token: 0x04000201 RID: 513
	[SerializeField]
	public int hatGo_Index = 1;

	// Token: 0x04000202 RID: 514
	[SerializeField]
	public int eyeMat_Index;

	// Token: 0x04000203 RID: 515
	[SerializeField]
	private SkinnedMeshRenderer mesh_Body;

	// Token: 0x04000204 RID: 516
	[SerializeField]
	private MeshRenderer mesh_Head;

	// Token: 0x04000205 RID: 517
	[SerializeField]
	private MeshFilter meshF_Hair;

	// Token: 0x04000206 RID: 518
	[SerializeField]
	private MeshRenderer meshR_Hair;

	// Token: 0x04000207 RID: 519
	[SerializeField]
	private MeshRenderer mesh_Hat;

	// Token: 0x04000208 RID: 520
	[SerializeField]
	private Face_Controller face_Controller;

	// Token: 0x04000209 RID: 521
	private MeshRenderer mesh_Eye;

	// Token: 0x0400020A RID: 522
	[SerializeField]
	private bool isGenericCustomer;
}
