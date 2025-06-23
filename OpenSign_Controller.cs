using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class OpenSign_Controller : MonoBehaviour
{
	// Token: 0x060000F9 RID: 249 RVA: 0x0000B0F0 File Offset: 0x000092F0
	private void Start()
	{
		this.RefreshSign(0);
	}

	// Token: 0x060000FA RID: 250 RVA: 0x0000B0F9 File Offset: 0x000092F9
	private void Update()
	{
		this.isOpen = Game_Manager.instance.GetMartOpen();
		if (this.isOpen)
		{
			this.RefreshSign(0);
			return;
		}
		if (Game_Manager.instance.GetWaitingCustomersToLeave())
		{
			this.RefreshSign(1);
			return;
		}
		this.RefreshSign(1);
	}

	// Token: 0x060000FB RID: 251 RVA: 0x0000B136 File Offset: 0x00009336
	private void RefreshSign(int _index)
	{
		if (this.state == _index)
		{
			return;
		}
		this.state = _index;
		this.meshRenderer.material = this.materials[_index];
	}

	// Token: 0x04000180 RID: 384
	[SerializeField]
	private Material[] materials;

	// Token: 0x04000181 RID: 385
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x04000182 RID: 386
	private bool isOpen;

	// Token: 0x04000183 RID: 387
	private int state = -1;
}
