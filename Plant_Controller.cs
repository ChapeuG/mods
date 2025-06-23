using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class Plant_Controller : MonoBehaviour
{
	// Token: 0x06000104 RID: 260 RVA: 0x0000B318 File Offset: 0x00009518
	private void Start()
	{
		this.RefreshMaterials();
	}

	// Token: 0x06000105 RID: 261 RVA: 0x0000B320 File Offset: 0x00009520
	public void SetLifeSpan(int _lifeSpanIndex)
	{
		this.lifeSpanIndex = _lifeSpanIndex;
		this.RefreshMaterials();
	}

	// Token: 0x06000106 RID: 262 RVA: 0x0000B32F File Offset: 0x0000952F
	public int GetLifeSpanIndex()
	{
		return this.lifeSpanIndex;
	}

	// Token: 0x06000107 RID: 263 RVA: 0x0000B337 File Offset: 0x00009537
	public void DecreaseLifeSpan()
	{
		this.lifeSpanIndex--;
		if (this.lifeSpanIndex <= 0)
		{
			this.lifeSpanIndex = 0;
		}
		this.RefreshMaterials();
	}

	// Token: 0x06000108 RID: 264 RVA: 0x0000B35D File Offset: 0x0000955D
	public void ResetLifeSpan()
	{
		this.lifeSpanIndex = Inv_Manager.instance.plantHealth_StartValue;
		this.RefreshMaterials();
	}

	// Token: 0x06000109 RID: 265 RVA: 0x0000B378 File Offset: 0x00009578
	public void RefreshMaterials()
	{
		if (this.meshRenderer.Count <= 0)
		{
			this.meshRenderer.Add(base.transform.Find("Master").GetComponentInChildren<MeshRenderer>());
		}
		if (this.meshRenderer.Count > 0 && this.hasLifeSpan)
		{
			for (int i = 0; i < this.meshRenderer.Count; i++)
			{
				if (this.meshRenderer[i])
				{
					this.meshRenderer[i].material.color = Inv_Manager.instance.plantHealth_Colors[this.lifeSpanIndex];
				}
			}
		}
	}

	// Token: 0x0400018A RID: 394
	[SerializeField]
	private List<MeshRenderer> meshRenderer = new List<MeshRenderer>();

	// Token: 0x0400018B RID: 395
	[SerializeField]
	private bool hasLifeSpan;

	// Token: 0x0400018C RID: 396
	[SerializeField]
	private int lifeSpanIndex = 3;
}
