using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class ItemCreation_SceneManager : MonoBehaviour
{
	// Token: 0x0600036D RID: 877 RVA: 0x0001F5A5 File Offset: 0x0001D7A5
	private void Awake()
	{
		this.GetProdPrefabs();
	}

	// Token: 0x0600036E RID: 878 RVA: 0x0001F5AD File Offset: 0x0001D7AD
	private void Start()
	{
		this.AddShelves();
	}

	// Token: 0x0600036F RID: 879 RVA: 0x0001F5B8 File Offset: 0x0001D7B8
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			this.GetProdPrefabs();
			this.AddShelves();
		}
		float d = this.moveSpeed;
		if (Input.GetKey(KeyCode.LeftShift))
		{
			d = this.runSpeed;
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			this.cameraParent.transform.position = this.cameraParent.transform.position + Vector3.right * d * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			this.cameraParent.transform.position = this.cameraParent.transform.position + Vector3.left * d * Time.deltaTime;
		}
	}

	// Token: 0x06000370 RID: 880 RVA: 0x0001F680 File Offset: 0x0001D880
	private void GetProdPrefabs()
	{
		this.prod_Prefabs.Clear();
		Prod_Controller[] array = Resources.LoadAll<Prod_Controller>("Sellables/Prods/Prefabs");
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < array.Length; j++)
			{
				if (int.Parse(array[j].name.Split(new char[]
				{
					char.Parse("_")
				})[0]) == i)
				{
					this.prod_Prefabs.Add(array[j]);
					break;
				}
			}
		}
	}

	// Token: 0x06000371 RID: 881 RVA: 0x0001F6FC File Offset: 0x0001D8FC
	private void AddShelves()
	{
		for (int i = 0; i < this.shelf_Controllers.Count; i++)
		{
			UnityEngine.Object.Destroy(this.shelf_Controllers[i].gameObject);
			UnityEngine.Object.Destroy(this.shelf_Controllers2[i].gameObject);
		}
		this.shelf_Controllers.Clear();
		this.shelf_Controllers2.Clear();
		int num = 0;
		for (int j = 0; j < this.prod_Prefabs.Count; j++)
		{
			if (this.prod_Prefabs[j])
			{
				Shelf_Controller shelf_Controller = UnityEngine.Object.Instantiate<Shelf_Controller>(this.shelf_Prefab);
				this.shelf_Controllers.Add(shelf_Controller);
				shelf_Controller.transform.SetParent(base.transform.transform);
				shelf_Controller.gameObject.transform.position = new Vector3((float)(-(float)(j + num)), 0f, 0f);
				shelf_Controller.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
				Shelf_Controller shelf_Controller2 = UnityEngine.Object.Instantiate<Shelf_Controller>(this.shelf_Prefab2);
				this.shelf_Controllers2.Add(shelf_Controller2);
				shelf_Controller2.transform.SetParent(base.transform.transform);
				shelf_Controller2.gameObject.transform.position = new Vector3((float)(-(float)(j + num)), 0f, -6f);
				shelf_Controller2.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
				num++;
			}
		}
		base.Invoke("AddProdsToShelves", 0.5f);
	}

	// Token: 0x06000372 RID: 882 RVA: 0x0001F888 File Offset: 0x0001DA88
	private void AddProdsToShelves()
	{
		for (int i = 0; i < this.shelf_Controllers.Count; i++)
		{
			this.shelf_Controllers[i].StoreProdForTestingOnly(0, this.CreateProd(i, 2));
			this.shelf_Controllers[i].StoreProdForTestingOnly(1, this.CreateProd(i, 2));
			this.shelf_Controllers2[i].StoreProdForTestingOnly(0, this.CreateProd(i, 4));
		}
	}

	// Token: 0x06000373 RID: 883 RVA: 0x0001F8FC File Offset: 0x0001DAFC
	public Prod_Controller[] CreateProd(int _prodIndex, int _prodQnt)
	{
		Prod_Controller[] array = new Prod_Controller[_prodQnt];
		for (int i = 0; i < _prodQnt; i++)
		{
			array[i] = UnityEngine.Object.Instantiate<Prod_Controller>(this.prod_Prefabs[_prodIndex]);
			array[i].SetIndex(_prodIndex);
			if (!this.prod_Prefabs.Contains(array[i]))
			{
				this.prod_Prefabs.Add(array[i]);
			}
		}
		return array;
	}

	// Token: 0x040003B7 RID: 951
	[SerializeField]
	private Shelf_Controller shelf_Prefab;

	// Token: 0x040003B8 RID: 952
	[SerializeField]
	private List<Shelf_Controller> shelf_Controllers = new List<Shelf_Controller>();

	// Token: 0x040003B9 RID: 953
	[SerializeField]
	private Shelf_Controller shelf_Prefab2;

	// Token: 0x040003BA RID: 954
	[SerializeField]
	private List<Shelf_Controller> shelf_Controllers2 = new List<Shelf_Controller>();

	// Token: 0x040003BB RID: 955
	[SerializeField]
	private List<Prod_Controller> prod_Prefabs = new List<Prod_Controller>();

	// Token: 0x040003BC RID: 956
	[SerializeField]
	private GameObject cameraParent;

	// Token: 0x040003BD RID: 957
	[SerializeField]
	private float moveSpeed;

	// Token: 0x040003BE RID: 958
	[SerializeField]
	private float runSpeed;
}
