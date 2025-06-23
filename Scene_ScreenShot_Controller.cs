using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class Scene_ScreenShot_Controller : MonoBehaviour
{
	// Token: 0x0600014C RID: 332 RVA: 0x0000D7F8 File Offset: 0x0000B9F8
	private void Awake()
	{
		if (this.parent == null)
		{
			this.parent = base.gameObject;
		}
		foreach (object obj in this.parent.transform)
		{
			Transform transform = (Transform)obj;
			this.children.Add(transform.gameObject);
		}
	}

	// Token: 0x0600014D RID: 333 RVA: 0x0000D87C File Offset: 0x0000BA7C
	private void Start()
	{
		this.ChangeActiveObject(0);
	}

	// Token: 0x0600014E RID: 334 RVA: 0x0000D888 File Offset: 0x0000BA88
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (this.index < this.children.Count)
			{
				this.index++;
			}
			else
			{
				this.index = 0;
			}
			this.ChangeActiveObject(this.index);
			return;
		}
		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			if (this.index >= 1)
			{
				this.index--;
			}
			else
			{
				this.index = this.children.Count - 1;
			}
			this.ChangeActiveObject(this.index);
		}
	}

	// Token: 0x0600014F RID: 335 RVA: 0x0000D918 File Offset: 0x0000BB18
	private void ChangeActiveObject(int _index)
	{
		for (int i = 0; i < this.children.Count; i++)
		{
			if (i == _index)
			{
				this.children[i].SetActive(true);
			}
			else
			{
				this.children[i].SetActive(false);
			}
		}
	}

	// Token: 0x040001E5 RID: 485
	[SerializeField]
	private GameObject parent;

	// Token: 0x040001E6 RID: 486
	[SerializeField]
	private List<GameObject> children = new List<GameObject>();

	// Token: 0x040001E7 RID: 487
	private int index;
}
