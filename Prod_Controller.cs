using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class Prod_Controller : MonoBehaviour
{
	// Token: 0x06000142 RID: 322 RVA: 0x0000D473 File Offset: 0x0000B673
	private void Awake()
	{
		this.meshRenderer.Add(base.transform.Find("Master").Find("Scaler").Find("Mesh").GetComponent<MeshRenderer>());
	}

	// Token: 0x06000143 RID: 323 RVA: 0x0000D4A9 File Offset: 0x0000B6A9
	private void Start()
	{
		if (!this.lifeSpan)
		{
			this.lifeSpanIndex = 7;
		}
	}

	// Token: 0x06000144 RID: 324 RVA: 0x0000D4BA File Offset: 0x0000B6BA
	public void SetIndex(int _prodIndex)
	{
		this.prodIndex = _prodIndex;
	}

	// Token: 0x06000145 RID: 325 RVA: 0x0000D4C3 File Offset: 0x0000B6C3
	public void DeleteProd()
	{
		Inv_Manager.instance.DeleteProd(this);
	}

	// Token: 0x06000146 RID: 326 RVA: 0x0000D4D0 File Offset: 0x0000B6D0
	public void HoldProd(GameObject _prodHolder, Shelf_Controller _shelf, int _lifeSpanToMerge)
	{
		if (_shelf)
		{
			this.shelf_Controller = _shelf;
		}
		else
		{
			this.shelf_Controller = null;
		}
		base.transform.SetParent(_prodHolder.transform);
		base.transform.localRotation = Quaternion.Euler(Vector3.zero);
		base.transform.localPosition = Vector3.zero;
		base.transform.localScale = Vector3.one;
		base.gameObject.GetComponent<Animator>().PlayInFixedTime("Box_On", -1, 0f);
		this.MergeLifeSpan(_lifeSpanToMerge);
	}

	// Token: 0x06000147 RID: 327 RVA: 0x0000D560 File Offset: 0x0000B760
	public void HoldProd(bool _active, Cashier_Controller _cashier, Vector3 _pos, Quaternion _rot)
	{
		this.cashier_Controller = _cashier;
		base.transform.SetParent(_cashier.transform);
		base.transform.localRotation = _rot;
		base.transform.localPosition = _pos;
		base.transform.localScale = Vector3.one;
		base.gameObject.GetComponent<Animator>().PlayInFixedTime("Box_On", -1, 0f);
		base.gameObject.SetActive(_active);
	}

	// Token: 0x06000148 RID: 328 RVA: 0x0000D5D8 File Offset: 0x0000B7D8
	public void DecreaseLifeSpan()
	{
		int num = Inv_Manager.instance.lifeSpan_DailyLoss_RightShelf;
		if (this.shelf_Controller && !this.shelf_Controller.isRefrigerated && this.needsRefrigerator && this.shelf_Controller.utilController == null)
		{
			num = Inv_Manager.instance.lifeSpan_DailyLoss_WrongShelf;
		}
		if (this.lifeSpanIndex > 0)
		{
			this.lifeSpanIndex -= num;
		}
		if (this.lifeSpanIndex < 0)
		{
			this.lifeSpanIndex = 0;
		}
		this.Expire();
	}

	// Token: 0x06000149 RID: 329 RVA: 0x0000D65D File Offset: 0x0000B85D
	public void MergeLifeSpan(int _lifeSpanToMerge)
	{
		if (_lifeSpanToMerge < this.lifeSpanIndex)
		{
			this.lifeSpanIndex = _lifeSpanToMerge;
			return;
		}
		this.Expire();
	}

	// Token: 0x0600014A RID: 330 RVA: 0x0000D678 File Offset: 0x0000B878
	public void Expire()
	{
		if (this.meshRenderer.Count <= 0)
		{
			this.meshRenderer.Add(base.transform.Find("Master").Find("Scaler").Find("Mesh").GetComponent<MeshRenderer>());
		}
		if (this.meshRenderer.Count > 0 && this.lifeSpanIndex <= 0 && this.lifeSpan)
		{
			for (int i = 0; i < this.meshRenderer.Count; i++)
			{
				if (this.meshRenderer[i])
				{
					this.meshRenderer[i].material.color = this.expiredColor;
				}
			}
			if (this.spoiledParticles == null)
			{
				this.spoiledParticles = UnityEngine.Object.Instantiate<GameObject>(Inv_Manager.instance.prod_SpoiledParticles.gameObject).GetComponent<ParticleSystem>();
				this.spoiledParticles.gameObject.transform.SetParent(base.transform.Find("Master"));
				this.spoiledParticles.gameObject.transform.localPosition = Vector3.zero;
				this.spoiledParticles.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
				this.spoiledParticles.gameObject.transform.localScale = Vector3.one;
			}
		}
	}

	// Token: 0x040001D4 RID: 468
	[Header("Prod Type")]
	[SerializeField]
	public Inv_Manager.ProdType prodType;

	// Token: 0x040001D5 RID: 469
	[Header("General")]
	[SerializeField]
	public bool buyable;

	// Token: 0x040001D6 RID: 470
	[SerializeField]
	public bool buyableByCustomers;

	// Token: 0x040001D7 RID: 471
	[SerializeField]
	public bool randonlyUnlockable;

	// Token: 0x040001D8 RID: 472
	[SerializeField]
	public bool needsRefrigerator;

	// Token: 0x040001D9 RID: 473
	[SerializeField]
	public bool lifeSpan;

	// Token: 0x040001DA RID: 474
	[SerializeField]
	public int lifeSpanIndex = 7;

	// Token: 0x040001DB RID: 475
	[SerializeField]
	public int prodPrice;

	// Token: 0x040001DC RID: 476
	[SerializeField]
	public bool isBall;

	// Token: 0x040001DD RID: 477
	[SerializeField]
	public Color[] prodColors;

	// Token: 0x040001DE RID: 478
	[HideInInspector]
	public int prodIndex;

	// Token: 0x040001DF RID: 479
	[SerializeField]
	public List<MeshRenderer> meshRenderer = new List<MeshRenderer>();

	// Token: 0x040001E0 RID: 480
	[SerializeField]
	public Color expiredColor;

	// Token: 0x040001E1 RID: 481
	[HideInInspector]
	public Shelf_Controller shelf_Controller;

	// Token: 0x040001E2 RID: 482
	[HideInInspector]
	public Cashier_Controller cashier_Controller;

	// Token: 0x040001E3 RID: 483
	public EventReference event_Material;

	// Token: 0x040001E4 RID: 484
	private ParticleSystem spoiledParticles;
}
