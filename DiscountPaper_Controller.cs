using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class DiscountPaper_Controller : MonoBehaviour
{
	// Token: 0x0600009C RID: 156 RVA: 0x00007C28 File Offset: 0x00005E28
	public void Change_DiscountPaper(int _dir = 1)
	{
		if (_dir == 0)
		{
			this.discountLevel = -1;
		}
		else
		{
			this.discountLevel += _dir;
			if (this.discountLevel > 2)
			{
				this.discountLevel = -1;
			}
			else if (this.discountLevel < -1)
			{
				this.discountLevel = 2;
			}
		}
		this.CreatePaper(this.discountLevel);
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00007C80 File Offset: 0x00005E80
	public void CreatePaper(int _discountLevel)
	{
		this.discountLevel = _discountLevel;
		Material material = Inv_Manager.instance.discount_PaperMaterials[3];
		if (_discountLevel >= 0)
		{
			material = Inv_Manager.instance.discount_PaperMaterials[_discountLevel];
		}
		this.meshRenderer.material = material;
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00007CBE File Offset: 0x00005EBE
	public void DestroyPaper()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00007CCC File Offset: 0x00005ECC
	public void HoldPaper(GameObject _paperHolder)
	{
		base.transform.SetParent(_paperHolder.transform);
		base.transform.localRotation = Quaternion.Euler(Vector3.zero);
		base.transform.localPosition = Vector3.zero;
		base.transform.localScale = Vector3.one;
		base.gameObject.GetComponent<Animator>().PlayInFixedTime("Box_On", -1, 0f);
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x00007D3A File Offset: 0x00005F3A
	public int GetDiscountLevel()
	{
		return this.discountLevel;
	}

	// Token: 0x040000CC RID: 204
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x040000CD RID: 205
	[SerializeField]
	private Animator anim;

	// Token: 0x040000CE RID: 206
	private int discountLevel = -1;
}
