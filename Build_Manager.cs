using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class Build_Manager : MonoBehaviour
{
	// Token: 0x060001CA RID: 458 RVA: 0x0001183E File Offset: 0x0000FA3E
	private void Awake()
	{
		if (!Build_Manager.instance)
		{
			Build_Manager.instance = this;
		}
	}

	// Token: 0x060001CB RID: 459 RVA: 0x00011852 File Offset: 0x0000FA52
	private void Update()
	{
		this.Update_Rotation();
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0001185C File Offset: 0x0000FA5C
	private void Update_Rotation()
	{
		for (int i = 0; i < this.objToRotate.Count; i++)
		{
			if (this.objToRotate[i])
			{
				Quaternion b = Quaternion.Euler(this.objToRotate[i].transform.rotation.eulerAngles.x, this.eulerAngleToRotate[i], this.objToRotate[i].transform.rotation.eulerAngles.z);
				if (Quaternion.Angle(this.objToRotate[i].transform.rotation, b) > 5f)
				{
					this.objToRotate[i].transform.rotation = Quaternion.Lerp(this.objToRotate[i].transform.rotation, b, this.rotationSpeed * Time.unscaledDeltaTime);
				}
				else
				{
					this.objToRotate[i].transform.rotation = Quaternion.Euler(0f, this.eulerAngleToRotate[i], 0f);
					this.RemoveRotateObj(i);
				}
			}
		}
	}

	// Token: 0x060001CD RID: 461 RVA: 0x00011990 File Offset: 0x0000FB90
	public void SetRotateObj(GameObject _obj, int _direction)
	{
		if (this.objToRotate.Contains(_obj))
		{
			return;
		}
		this.objToRotate.Add(_obj);
		this.eulerAngleToRotate.Add(_obj.transform.rotation.eulerAngles.y + (float)(90 * _direction));
	}

	// Token: 0x060001CE RID: 462 RVA: 0x000119E1 File Offset: 0x0000FBE1
	public void RemoveRotateObj(int _index)
	{
		this.objToRotate.RemoveAt(_index);
		this.eulerAngleToRotate.RemoveAt(_index);
	}

	// Token: 0x040002BE RID: 702
	public static Build_Manager instance;

	// Token: 0x040002BF RID: 703
	[SerializeField]
	private float rotationSpeed = 10f;

	// Token: 0x040002C0 RID: 704
	[SerializeField]
	private List<GameObject> objToRotate = new List<GameObject>();

	// Token: 0x040002C1 RID: 705
	[SerializeField]
	private List<float> eulerAngleToRotate = new List<float>();
}
