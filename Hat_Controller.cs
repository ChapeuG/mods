using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class Hat_Controller : MonoBehaviour
{
	// Token: 0x060000B6 RID: 182 RVA: 0x0000855C File Offset: 0x0000675C
	public void RefreshHat(int _index)
	{
		base.transform.localPosition = this.hatPositions[_index];
		base.transform.localRotation = Quaternion.Euler(this.hatRotations[_index]);
		this.meshRenderer.enabled = false;
		if (this.hatBools[_index] && this.hatGO)
		{
			this.hatGO.transform.Find("Mesh").GetComponent<MeshRenderer>().enabled = true;
			return;
		}
		if (this.hatGO)
		{
			this.hatGO.transform.Find("Mesh").GetComponent<MeshRenderer>().enabled = false;
		}
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00008614 File Offset: 0x00006814
	public void ChangeHat(int _index, int _hatMat_Index)
	{
		this.hat_index = _index;
		if (this.hatGO != null)
		{
			UnityEngine.Object.Destroy(this.hatGO);
		}
		this.hatGO = UnityEngine.Object.Instantiate<GameObject>(Player_Manager.instance.hat_prefabs[this.hat_index]);
		this.hatGO.transform.SetParent(base.gameObject.transform);
		this.hatGO.transform.localPosition = Vector3.zero;
		this.hatGO.transform.localRotation = Quaternion.Euler(Vector3.zero);
		this.hatGO.transform.localScale = Vector3.one;
		if (_index == 1)
		{
			Material[] materials = new Material[]
			{
				Player_Manager.instance.material_Clothes[_hatMat_Index]
			};
			this.hatGO.transform.Find("Mesh").GetComponent<MeshRenderer>().materials = materials;
		}
	}

	// Token: 0x040000F8 RID: 248
	[SerializeField]
	public List<bool> hatBools = new List<bool>();

	// Token: 0x040000F9 RID: 249
	[SerializeField]
	public List<Vector3> hatPositions = new List<Vector3>();

	// Token: 0x040000FA RID: 250
	[SerializeField]
	public List<Vector3> hatRotations = new List<Vector3>();

	// Token: 0x040000FB RID: 251
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x040000FC RID: 252
	public int hat_index;

	// Token: 0x040000FD RID: 253
	[SerializeField]
	private GameObject hatGO;
}
