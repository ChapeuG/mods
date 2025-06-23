using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class VoidPortal_Controller : MonoBehaviour
{
	// Token: 0x060001B6 RID: 438 RVA: 0x000109A4 File Offset: 0x0000EBA4
	private void Awake()
	{
		if (!VoidPortal_Controller.instance)
		{
			VoidPortal_Controller.instance = this;
		}
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x000109B8 File Offset: 0x0000EBB8
	public void TransportObject(GameObject _GO, Rigidbody _rigidbody)
	{
		this.transportObject.Add(_GO);
		this.portal.SetActive(false);
		base.transform.position = Player_Manager.instance.GetPlayerController(0).transform.position;
		this.portal.SetActive(true);
		_GO.SetActive(false);
		_GO.transform.position = this.transportPos.transform.position;
		if (_rigidbody)
		{
			_rigidbody.velocity = new Vector3(0f, this.portalDropVel, 0f);
		}
		Invoker.InvokeDelayed(new Invokable(this.DropObject), 2f);
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x00010A64 File Offset: 0x0000EC64
	private void DropObject()
	{
		this.transportObject.TrimExcess();
		if (this.transportObject.Count > 0)
		{
			if (!this.transportObject[0])
			{
				return;
			}
			this.transportObject[0].SetActive(true);
			this.transportObject.RemoveAt(0);
		}
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x00010ABC File Offset: 0x0000ECBC
	public void TransportPlayer(Player_Controller _player)
	{
		this.transportObject.Add(_player.gameObject);
		this.portal.SetActive(false);
		base.transform.position = _player.gameObject.transform.parent.transform.position;
		this.portal.SetActive(true);
		_player.gameObject.SetActive(false);
		_player.gameObject.transform.localPosition = Vector3.zero;
		_player.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		_player.gameObject.transform.localScale = Vector3.one;
		_player.gameObject.transform.position = this.transportPos.transform.position;
		Invoker.InvokeDelayed(new Invokable(this.DropObject), 2f);
	}

	// Token: 0x04000244 RID: 580
	public static VoidPortal_Controller instance;

	// Token: 0x04000245 RID: 581
	[SerializeField]
	private GameObject portal;

	// Token: 0x04000246 RID: 582
	[SerializeField]
	private GameObject transportPos;

	// Token: 0x04000247 RID: 583
	[SerializeField]
	private float portalDropVel;

	// Token: 0x04000248 RID: 584
	private List<GameObject> transportObject = new List<GameObject>();
}
