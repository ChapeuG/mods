using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class Nav_Manager : MonoBehaviour
{
	// Token: 0x06000405 RID: 1029 RVA: 0x00025A48 File Offset: 0x00023C48
	private void BuildGraph()
	{
		this.nodes.Clear();
		foreach (GameObject gameObject in this.navSpheres_Customers)
		{
			if (gameObject.activeSelf)
			{
				this.nodes.Add(new Nav_Manager.Nav_Manager_Node(gameObject, false));
			}
		}
		foreach (GameObject gameObject2 in this.navSpheres_Staff)
		{
			if (gameObject2.activeSelf)
			{
				this.nodes.Add(new Nav_Manager.Nav_Manager_Node(gameObject2, true));
			}
		}
		foreach (Interaction_Controller interaction_Controller in this.interactables)
		{
			if (!(interaction_Controller == null) && interaction_Controller.act_as_navSphere)
			{
				this.nodes.Add(new Nav_Manager.Nav_Manager_Node(interaction_Controller.navSphereDeactivated, false));
			}
		}
		for (int i = 1; i < this.nodes.Count; i++)
		{
			for (int j = 0; j < i; j++)
			{
				float d = Vector3.Distance(this.nodes[i].sphere.transform.position, this.nodes[j].sphere.transform.position);
				this.nodes[i].AddNeighbor(this.nodes[j], d);
				this.nodes[j].AddNeighbor(this.nodes[i], d);
			}
		}
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x00025C30 File Offset: 0x00023E30
	private void Awake()
	{
		if (!Nav_Manager.instance)
		{
			Nav_Manager.instance = this;
		}
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x00025C44 File Offset: 0x00023E44
	private void Start()
	{
		this.DeactivateNavSpheresByInteractbles();
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x00025C4C File Offset: 0x00023E4C
	public void RefreshReferences()
	{
		for (int i = 0; i < this.navSpheres_Customers.Count; i++)
		{
			if (this.navSpheres_Customers[i])
			{
				this.navSpheres_Customers[i].SetActive(true);
			}
		}
		this.navSpheres_Customers.Clear();
		GameObject[] array = GameObject.FindGameObjectsWithTag("NavSphere_Customers");
		for (int j = 0; j < array.Length; j++)
		{
			this.navSpheres_Customers.Add(array[j]);
		}
		this.navSpheres_Staff.Clear();
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("NavSphere_Staff");
		for (int k = 0; k < array2.Length; k++)
		{
			this.navSpheres_Staff.Add(array2[k]);
		}
		this.navSpheres_Exit.Clear();
		GameObject[] array3 = GameObject.FindGameObjectsWithTag("NavSphere_Exit");
		for (int l = 0; l < array3.Length; l++)
		{
			this.navSpheres_Exit.Add(array3[l]);
		}
		this.interactables.Clear();
		GameObject[] array4 = GameObject.FindGameObjectsWithTag("Interactive");
		for (int m = 0; m < array4.Length; m++)
		{
			this.interactables.Add(array4[m].GetComponent<Interaction_Controller>());
		}
		this.nodes.Clear();
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x00025D88 File Offset: 0x00023F88
	public void DeactivateNavSpheresByInteractbles()
	{
		this.nodes.Clear();
		for (int i = 0; i < this.navSpheres_Customers.Count; i++)
		{
			bool active = true;
			for (int j = 0; j < this.interactables.Count; j++)
			{
				if (this.interactables[j] && !this.interactables[j].isChar && !this.interactables[j].isStoryChar && !this.interactables[j].isBox)
				{
					if (Vector3.Distance(this.navSpheres_Customers[i].transform.position, this.interactables[j].transform.position) <= 0.5f)
					{
						active = false;
						if (this.interactables[j].act_as_navSphere)
						{
							this.interactables[j].navSphereDeactivated = this.navSpheres_Customers[i];
						}
					}
					for (int k = 0; k < this.interactables[j].int_NavPoint.Length; k++)
					{
						if (this.interactables[j].int_NavPoint[k] && Vector3.Distance(this.navSpheres_Customers[i].transform.position, this.interactables[j].int_NavPoint[k].transform.position) <= 0.5f)
						{
							active = false;
						}
					}
				}
			}
			this.navSpheres_Customers[i].SetActive(active);
		}
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x00025F27 File Offset: 0x00024127
	public void AddInteractable(Interaction_Controller _interactable)
	{
		if (!this.interactables.Contains(_interactable))
		{
			this.interactables.Add(_interactable);
		}
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x00025F43 File Offset: 0x00024143
	public void RemoveInteractable(Interaction_Controller _interactable)
	{
		if (this.interactables.Contains(_interactable))
		{
			this.interactables.Remove(_interactable);
			this.interactables.TrimExcess();
		}
		this.RemoveMissingInteractables();
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x00025F74 File Offset: 0x00024174
	public void RemoveMissingInteractables()
	{
		for (int i = this.interactables.Count - 1; i >= 0; i--)
		{
			if (this.interactables[i] == null)
			{
				this.interactables.RemoveAt(i);
			}
		}
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x00025FB9 File Offset: 0x000241B9
	public List<Interaction_Controller> GetGarbage()
	{
		this.RemoveMissingInteractables();
		return this.interactables.FindAll((Interaction_Controller x) => x.isGarbage);
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x00025FEB File Offset: 0x000241EB
	public List<Interaction_Controller> GetDirt()
	{
		this.RemoveMissingInteractables();
		return this.interactables.FindAll((Interaction_Controller x) => x.isDirt);
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x00026020 File Offset: 0x00024220
	public GameObject GetTrashCan(Vector3 _startPos)
	{
		this.RemoveMissingInteractables();
		GameObject result = null;
		float num = 10000000f;
		foreach (Interaction_Controller interaction_Controller in this.interactables)
		{
			if (!(interaction_Controller == null) && interaction_Controller.isTrash)
			{
				float num2 = Vector3.Distance(interaction_Controller.transform.position, _startPos);
				if (num2 < num)
				{
					num = num2;
					result = interaction_Controller.gameObject;
				}
			}
		}
		return result;
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x000260B0 File Offset: 0x000242B0
	public GameObject GetNavExitPointRandom()
	{
		if (this.navSpheres_Exit.Count == 0)
		{
			return Player_Manager.instance.GetPlayerController(0).transform.parent.gameObject;
		}
		return this.navSpheres_Exit[UnityEngine.Random.Range(0, this.navSpheres_Exit.Count)];
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x00026104 File Offset: 0x00024304
	public List<GameObject> GetNavPathExit(Vector3 _startPos, bool _staff)
	{
		int index = UnityEngine.Random.Range(0, this.navSpheres_Exit.Count);
		List<GameObject> navPath = this.GetNavPath(_startPos, this.navSpheres_Exit[index].transform.position, _staff);
		navPath.Add(this.navSpheres_Exit[index]);
		return navPath;
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x00026154 File Offset: 0x00024354
	public List<GameObject> GetNavPathCashier(Cashier_Controller _cashier, Vector3 _startPos, bool _staff)
	{
		if (_cashier)
		{
			GameObject gameObject;
			if (_staff)
			{
				gameObject = _cashier.operatorPlace;
			}
			else
			{
				gameObject = _cashier.GetAvailableClientPlace();
			}
			if (gameObject)
			{
				return this.GetNavPath(_startPos, gameObject.transform.position, _staff);
			}
		}
		return null;
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x0002619C File Offset: 0x0002439C
	public List<GameObject> GetNavPathLoiter(Vector3 _startPos, bool _staff)
	{
		if (this.nodes.Count == 0)
		{
			this.BuildGraph();
		}
		if (this.nodes.Count == 0)
		{
			return null;
		}
		if (!_staff)
		{
			Debug.LogError("GetNavPathLoiter is not implemented for customers");
		}
		GameObject sphere = this.nodes[UnityEngine.Random.Range(0, this.nodes.Count)].sphere;
		return this.GetNavPath(_startPos, sphere.transform.position, _staff);
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x00026210 File Offset: 0x00024410
	public List<GameObject> GetNavPathTo(Vector3 _startPos, GameObject _target, bool _staff)
	{
		List<GameObject> navPath = this.GetNavPath(_startPos, _target.transform.position, _staff);
		if (navPath == null)
		{
			return navPath;
		}
		navPath.Add(_target);
		return navPath;
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00026240 File Offset: 0x00024440
	private List<GameObject> ReconstructPath(Nav_Manager.Nav_Manager_Node _node, Dictionary<Nav_Manager.Nav_Manager_Node, Nav_Manager.Path_Data> _pathData)
	{
		List<GameObject> list = new List<GameObject>();
		while (_node != null)
		{
			list.Add(_node.sphere);
			_node = _pathData[_node].from;
		}
		list.Reverse();
		return list;
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x0002627C File Offset: 0x0002447C
	public List<GameObject> GetNavPath(Vector3 _startPos, Vector3 _endPos, bool _staff)
	{
		if (this.nodes.Count == 0)
		{
			this.BuildGraph();
		}
		_startPos.y = 0f;
		_endPos.y = 0f;
		float num = 100000f;
		float num2 = 100000f;
		Nav_Manager.Nav_Manager_Node nav_Manager_Node = null;
		Nav_Manager.Nav_Manager_Node nav_Manager_Node2 = null;
		foreach (Nav_Manager.Nav_Manager_Node nav_Manager_Node3 in this.nodes)
		{
			Vector3 position = nav_Manager_Node3.sphere.transform.position;
			float num3 = Vector3.Distance(position, _startPos);
			if (num3 < num)
			{
				num = num3;
				nav_Manager_Node = nav_Manager_Node3;
			}
			num3 = Vector3.Distance(position, _endPos);
			if (num3 < num2)
			{
				num2 = num3;
				nav_Manager_Node2 = nav_Manager_Node3;
			}
		}
		if (nav_Manager_Node == null)
		{
			return new List<GameObject>();
		}
		Vector3 position2 = nav_Manager_Node2.sphere.transform.position;
		List<Nav_Manager.Nav_Manager_Node> list = new List<Nav_Manager.Nav_Manager_Node>();
		list.Add(nav_Manager_Node);
		Dictionary<Nav_Manager.Nav_Manager_Node, Nav_Manager.Path_Data> dictionary = new Dictionary<Nav_Manager.Nav_Manager_Node, Nav_Manager.Path_Data>();
		dictionary[nav_Manager_Node] = new Nav_Manager.Path_Data(null, 0f, Vector3.Distance(position2, nav_Manager_Node.sphere.transform.position));
		Nav_Manager.Nav_Manager_Node node = nav_Manager_Node;
		float num4 = dictionary[nav_Manager_Node].fScore;
		while (list.Count > 0)
		{
			Nav_Manager.Nav_Manager_Node nav_Manager_Node4 = list[0];
			float num5 = dictionary[nav_Manager_Node4].fScore;
			for (int i = 1; i < list.Count; i++)
			{
				float fScore = dictionary[list[i]].fScore;
				if (fScore < num5)
				{
					num5 = fScore;
					nav_Manager_Node4 = list[i];
				}
			}
			if (nav_Manager_Node4 == nav_Manager_Node2)
			{
				return this.ReconstructPath(nav_Manager_Node4, dictionary);
			}
			list.Remove(nav_Manager_Node4);
			for (int j = 0; j < nav_Manager_Node4.neighbors.Length; j++)
			{
				Nav_Manager.Nav_Manager_Node nav_Manager_Node5 = nav_Manager_Node4.neighbors[j];
				if (nav_Manager_Node5 != null && (_staff || nav_Manager_Node4.staffOnly || !nav_Manager_Node5.staffOnly))
				{
					float num6 = dictionary[nav_Manager_Node4].gScore + nav_Manager_Node4.dist[j];
					if (!dictionary.ContainsKey(nav_Manager_Node5) || num6 < dictionary[nav_Manager_Node5].gScore)
					{
						float num7 = Vector3.Distance(position2, nav_Manager_Node5.sphere.transform.position);
						dictionary[nav_Manager_Node5] = new Nav_Manager.Path_Data(nav_Manager_Node4, num6, num6 + num7);
						if (num7 < num4)
						{
							num4 = num7;
							node = nav_Manager_Node5;
						}
						if (!list.Contains(nav_Manager_Node5))
						{
							list.Add(nav_Manager_Node5);
						}
					}
				}
			}
		}
		Debug.Log(string.Concat(new string[]
		{
			"No path from '",
			nav_Manager_Node.sphere.name,
			"' to '",
			nav_Manager_Node2.sphere.name,
			"'"
		}));
		return this.ReconstructPath(node, dictionary);
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x00026554 File Offset: 0x00024754
	public List<GameObject> GetActiveNavSpheres()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.navSpheres_Customers.Count; i++)
		{
			if (this.navSpheres_Customers[i].activeInHierarchy)
			{
				list.Add(this.navSpheres_Customers[i]);
			}
		}
		return list;
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x000265A4 File Offset: 0x000247A4
	public GameObject FindNearPlacePoint(Box_Controller _boxController, Vector3 _interaction_pos)
	{
		GameObject result = null;
		List<GameObject> activeNavSpheres = this.GetActiveNavSpheres();
		float num = 10000f;
		for (int i = 0; i < activeNavSpheres.Count; i++)
		{
			float num2 = Vector3.Distance(_interaction_pos, activeNavSpheres[i].transform.position);
			if (num2 <= this.nearPlacePointDistance && num2 < num)
			{
				result = activeNavSpheres[i];
				num = num2;
			}
		}
		this.nearPlacePoint = result;
		return result;
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x0002660C File Offset: 0x0002480C
	public GameObject GetNearPlacePoint()
	{
		if (this.nearPlacePoint && !this.nearPlacePoint.gameObject.activeInHierarchy)
		{
			this.nearPlacePoint = null;
		}
		return this.nearPlacePoint;
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0002663A File Offset: 0x0002483A
	public void SetNearPlacePoint(GameObject _placePoint)
	{
		this.nearPlacePoint = _placePoint;
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x00026644 File Offset: 0x00024844
	public GameObject FindNearSphere(GameObject _position)
	{
		GameObject result = null;
		List<GameObject> activeNavSpheres = this.GetActiveNavSpheres();
		float num = 10000f;
		for (int i = 0; i < activeNavSpheres.Count; i++)
		{
			float num2 = Vector3.Distance(_position.transform.position, activeNavSpheres[i].transform.position);
			if (num2 < num)
			{
				result = activeNavSpheres[i];
				num = num2;
			}
		}
		return result;
	}

	// Token: 0x040004A9 RID: 1193
	public static Nav_Manager instance;

	// Token: 0x040004AA RID: 1194
	[SerializeField]
	private List<GameObject> navSpheres_Customers = new List<GameObject>();

	// Token: 0x040004AB RID: 1195
	[SerializeField]
	private List<GameObject> navSpheres_Staff = new List<GameObject>();

	// Token: 0x040004AC RID: 1196
	[SerializeField]
	private List<GameObject> navSpheres_Exit = new List<GameObject>();

	// Token: 0x040004AD RID: 1197
	[SerializeField]
	private List<Interaction_Controller> interactables = new List<Interaction_Controller>();

	// Token: 0x040004AE RID: 1198
	private List<Nav_Manager.Nav_Manager_Node> nodes = new List<Nav_Manager.Nav_Manager_Node>();

	// Token: 0x040004AF RID: 1199
	[Header("Place Point")]
	[SerializeField]
	private GameObject nearPlacePoint;

	// Token: 0x040004B0 RID: 1200
	private float nearPlacePointDistance = 2f;

	// Token: 0x02000086 RID: 134
	private class Nav_Manager_Node
	{
		// Token: 0x060005C5 RID: 1477 RVA: 0x00034A18 File Offset: 0x00032C18
		public Nav_Manager_Node(GameObject o, bool staff)
		{
			this.sphere = o;
			this.staffOnly = staff;
			this.neighbors = new Nav_Manager.Nav_Manager_Node[4];
			this.dist = new float[this.neighbors.Length];
			for (int i = 0; i < this.dist.Length; i++)
			{
				this.dist[i] = 3f;
			}
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00034A78 File Offset: 0x00032C78
		public void AddNeighbor(Nav_Manager.Nav_Manager_Node _node, float _d)
		{
			for (int i = 0; i < this.dist.Length; i++)
			{
				if (_d < this.dist[i])
				{
					for (int j = this.dist.Length - 1; j > i; j--)
					{
						this.dist[j] = this.dist[j - 1];
						this.neighbors[j] = this.neighbors[j - 1];
					}
					this.dist[i] = _d;
					this.neighbors[i] = _node;
					return;
				}
			}
		}

		// Token: 0x040006E7 RID: 1767
		public GameObject sphere;

		// Token: 0x040006E8 RID: 1768
		public Nav_Manager.Nav_Manager_Node[] neighbors;

		// Token: 0x040006E9 RID: 1769
		public float[] dist;

		// Token: 0x040006EA RID: 1770
		public bool staffOnly;
	}

	// Token: 0x02000087 RID: 135
	private struct Path_Data
	{
		// Token: 0x060005C7 RID: 1479 RVA: 0x00034AEF File Offset: 0x00032CEF
		public Path_Data(Nav_Manager.Nav_Manager_Node n, float gs, float fs)
		{
			this.from = n;
			this.gScore = gs;
			this.fScore = fs;
		}

		// Token: 0x040006EB RID: 1771
		public Nav_Manager.Nav_Manager_Node from;

		// Token: 0x040006EC RID: 1772
		public float gScore;

		// Token: 0x040006ED RID: 1773
		public float fScore;
	}
}
