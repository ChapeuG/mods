using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class Face_Controller : MonoBehaviour
{
	// Token: 0x060000B0 RID: 176 RVA: 0x00008494 File Offset: 0x00006694
	private void Awake()
	{
		this.mesh_Eye = base.transform.Find("GameObject").transform.Find("Mesh_Eyes").GetComponent<MeshRenderer>();
		Customer_Controller component = base.transform.root.GetComponent<Customer_Controller>();
		if (component)
		{
			component.face_Controller = this;
		}
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x000084EC File Offset: 0x000066EC
	public void SetMaterial_Eye(Material _mat)
	{
		Material[] materials = new Material[]
		{
			_mat
		};
		this.mesh_Eye.materials = materials;
	}

	// Token: 0x040000EF RID: 239
	public MeshRenderer mesh_Eye;
}
