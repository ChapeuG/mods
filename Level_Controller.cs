using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class Level_Controller : MonoBehaviour
{
	// Token: 0x060000E1 RID: 225 RVA: 0x0000A754 File Offset: 0x00008954
	public void CreateReferences()
	{
		Nav_Manager.instance.RefreshReferences();
		Inv_Manager.instance.RefreshReferences();
		Inv_Manager.instance.GetDeliveryPlaces();
		GameObject[] array = GameObject.FindGameObjectsWithTag("Interactive");
		for (int i = 0; i < array.Length; i++)
		{
			Interaction_Controller component = array[i].GetComponent<Interaction_Controller>();
			if (component != null && component.isLock)
			{
				component.lock_Controller.ResetLockState();
			}
		}
		this.tree_Controllers.Clear();
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("Tree");
		for (int j = 0; j < array2.Length; j++)
		{
			if (array2[j])
			{
				this.tree_Controllers.Add(array2[j].GetComponent<Tree_Controller>());
			}
		}
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x0000A804 File Offset: 0x00008A04
	public void SetNewSeason(int _index, Texture2D _texture, Color _grassColor)
	{
		this.terrain.terrainData.terrainLayers[0].diffuseTexture = _texture;
		this.terrain.terrainData.terrainLayers[1].diffuseTexture = _texture;
		DetailPrototype[] detailPrototypes = this.terrain.terrainData.detailPrototypes;
		detailPrototypes[0].dryColor = _grassColor;
		detailPrototypes[0].healthyColor = _grassColor;
		this.terrain.terrainData.detailPrototypes = detailPrototypes;
		for (int i = 0; i < this.tree_Controllers.Count; i++)
		{
			if (this.tree_Controllers[i])
			{
				this.tree_Controllers[i].SetTexturesByIndex(_index);
			}
		}
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x0000A8B0 File Offset: 0x00008AB0
	public void RefreshExpansions()
	{
		for (int i = 0; i < this.expansions_GOs.Count; i++)
		{
			if (World_Manager.instance.currentExpansionIndex > i)
			{
				this.expansions_GOs[i].SetActive(true);
			}
			else
			{
				this.expansions_GOs[i].SetActive(false);
			}
			if (World_Manager.instance.currentExpansionIndex == i)
			{
				this.expansionWalls_GOs[i].SetActive(true);
				this.expansionNavs_GOs[i].SetActive(false);
			}
			else if (World_Manager.instance.currentExpansionIndex - 1 == i)
			{
				if (World_Manager.instance.expansion_RemainingDays <= 0)
				{
					this.expansionWalls_GOs[i].SetActive(false);
					this.expansionNavs_GOs[i].SetActive(true);
				}
				else
				{
					this.expansionWalls_GOs[i].SetActive(true);
					this.expansionNavs_GOs[i].SetActive(false);
				}
			}
			else
			{
				this.expansionWalls_GOs[i].SetActive(false);
				this.expansionNavs_GOs[i].SetActive(true);
			}
		}
		if (World_Manager.instance.expansion_RemainingDays <= 0)
		{
			this.expansionWorkers_GO.SetActive(false);
			return;
		}
		this.expansionWorkers_GO.SetActive(true);
		this.expansionWorkers_GO.transform.position = this.expansions_GOs[World_Manager.instance.currentExpansionIndex - 1].transform.position;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x0000AA25 File Offset: 0x00008C25
	public List<GameObject> GetUndirtableNavSpheres()
	{
		return this.navSpheres_Undirtable;
	}

	// Token: 0x04000152 RID: 338
	[SerializeField]
	public Sprite levelSprite;

	// Token: 0x04000153 RID: 339
	[SerializeField]
	public int level_price;

	// Token: 0x04000154 RID: 340
	[SerializeField]
	public int expansion_price;

	// Token: 0x04000155 RID: 341
	[SerializeField]
	public int operational_cost;

	// Token: 0x04000156 RID: 342
	[SerializeField]
	public List<int> maxCharQnt = new List<int>();

	// Token: 0x04000157 RID: 343
	[SerializeField]
	public Terrain terrain;

	// Token: 0x04000158 RID: 344
	[SerializeField]
	private List<Tree_Controller> tree_Controllers = new List<Tree_Controller>();

	// Token: 0x04000159 RID: 345
	[SerializeField]
	public List<GameObject> expansions_GOs = new List<GameObject>();

	// Token: 0x0400015A RID: 346
	[SerializeField]
	private List<GameObject> expansionWalls_GOs = new List<GameObject>();

	// Token: 0x0400015B RID: 347
	[SerializeField]
	private List<GameObject> expansionNavs_GOs = new List<GameObject>();

	// Token: 0x0400015C RID: 348
	[SerializeField]
	private GameObject expansionWorkers_GO;

	// Token: 0x0400015D RID: 349
	[SerializeField]
	private List<GameObject> navSpheres_Undirtable = new List<GameObject>();

	// Token: 0x0400015E RID: 350
	[SerializeField]
	public List<GameObject> navSpheres_Tutorial = new List<GameObject>();

	// Token: 0x0400015F RID: 351
	[SerializeField]
	public GameObject referenceFloor;
}
