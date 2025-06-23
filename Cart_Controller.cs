using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class Cart_Controller : MonoBehaviour
{
	// Token: 0x06000030 RID: 48 RVA: 0x00003F48 File Offset: 0x00002148
	private void Awake()
	{
		foreach (GameObject gameObject in this.box_holders)
		{
			this.box_blanks.Add(gameObject.transform.Find("Box_Blank").gameObject);
			this.box_controllers.Add(null);
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003FC0 File Offset: 0x000021C0
	private void Start()
	{
		this.RefreshBoxes();
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00003FC8 File Offset: 0x000021C8
	public void ChangeCartIndex(int _dir = 1)
	{
		if (((_dir <= 0 && this.index <= this.boxQnt) || (_dir >= 0 && this.index < this.boxQnt)) && this.boxQnt > 0 && this.index < this.box_holders.Count - 1)
		{
			this.index += _dir;
			if (this.index < 0)
			{
				this.index = this.boxQnt;
			}
		}
		else
		{
			this.index = 0;
		}
		this.RefreshCartIndex();
		this._player.boxController = this.box_controllers[this.index];
		Menu_Manager.instance.RefreshMainHints();
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00004070 File Offset: 0x00002270
	public void RefreshCartIndex()
	{
		this.RefreshBoxes();
		this.index = Mathf.Clamp(this.index, 0, this.boxQnt);
		this.index = Mathf.Clamp(this.index, 0, this.box_holders.Count - 1);
		this.RefreshBoxes();
	}

	// Token: 0x06000034 RID: 52 RVA: 0x000040C0 File Offset: 0x000022C0
	public void RefreshBoxes()
	{
		List<Box_Controller> list = new List<Box_Controller>();
		foreach (Box_Controller box_Controller in this.box_controllers)
		{
			if (box_Controller != null)
			{
				list.Add(box_Controller);
			}
		}
		this.boxQnt = list.Count;
		for (int i = 0; i < this.box_controllers.Count; i++)
		{
			if (i < list.Count)
			{
				this.box_controllers[i] = list[i];
			}
			else
			{
				this.box_controllers[i] = null;
			}
		}
		for (int j = 0; j < this.box_controllers.Count; j++)
		{
			if (this.box_controllers[j] != null)
			{
				this.box_controllers[j].transform.SetParent(this.box_holders[j].transform);
				this.box_controllers[j].transform.localPosition = Vector3.zero;
				this.box_controllers[j].transform.localRotation = Quaternion.Euler(Vector3.zero);
				this.box_controllers[j].transform.localScale = Vector3.one;
				this.box_blanks[j].gameObject.SetActive(false);
			}
			else if (j == list.Count && j == this.index)
			{
				this.box_blanks[j].gameObject.SetActive(true);
			}
			else
			{
				this.box_blanks[j].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000035 RID: 53 RVA: 0x0000428C File Offset: 0x0000248C
	public GameObject Get_FreeSpace()
	{
		for (int i = 0; i < this.box_controllers.Count; i++)
		{
			if (this.box_controllers[i] == null)
			{
				return this.box_holders[i];
			}
		}
		return null;
	}

	// Token: 0x06000036 RID: 54 RVA: 0x000042D4 File Offset: 0x000024D4
	public void IncludeBox(Box_Controller _box)
	{
		for (int i = 0; i < this.box_controllers.Count; i++)
		{
			if (this.box_controllers[i] == null)
			{
				this.box_controllers[i] = _box;
				_box.cart_controller = this;
				break;
			}
		}
		this.RefreshBoxes();
		if (this.index == this.box_holders.Count - 1)
		{
			this.ChangeCartIndex(0);
			return;
		}
		this.ChangeCartIndex(1);
	}

	// Token: 0x06000037 RID: 55 RVA: 0x0000434C File Offset: 0x0000254C
	public void RemoveBox(Box_Controller _box)
	{
		for (int i = 0; i < this.box_controllers.Count; i++)
		{
			if (this.box_controllers[i] == _box)
			{
				this.box_controllers[i].cart_controller = null;
				this.box_controllers[i] = null;
				this.ChangeCartIndex(0);
				break;
			}
		}
		this.RefreshBoxes();
		if (this.box_controllers[this.index] == null)
		{
			this.ChangeCartIndex(-1);
		}
	}

	// Token: 0x0400004D RID: 77
	[SerializeField]
	private Player_Controller _player;

	// Token: 0x0400004E RID: 78
	public List<GameObject> box_holders = new List<GameObject>();

	// Token: 0x0400004F RID: 79
	[SerializeField]
	private List<GameObject> box_blanks = new List<GameObject>();

	// Token: 0x04000050 RID: 80
	[SerializeField]
	public List<Box_Controller> box_controllers = new List<Box_Controller>();

	// Token: 0x04000051 RID: 81
	public int index;

	// Token: 0x04000052 RID: 82
	private int boxQnt;
}
