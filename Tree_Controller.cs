using System;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class Tree_Controller : MonoBehaviour
{
	// Token: 0x060001A5 RID: 421 RVA: 0x00010508 File Offset: 0x0000E708
	public void SetTexturesByIndex(int _index)
	{
		if (!this.leavesRenderer)
		{
			return;
		}
		this.leavesRenderer.material = this.leavesMaterials[_index];
		if (!this.trunkRenderer)
		{
			return;
		}
		this.trunkRenderer.material = this.trunkMaterials[_index];
	}

	// Token: 0x0400022F RID: 559
	[SerializeField]
	private MeshRenderer leavesRenderer;

	// Token: 0x04000230 RID: 560
	[SerializeField]
	private MeshRenderer trunkRenderer;

	// Token: 0x04000231 RID: 561
	[SerializeField]
	private Material[] leavesMaterials = new Material[4];

	// Token: 0x04000232 RID: 562
	[SerializeField]
	private Material[] trunkMaterials = new Material[4];
}
