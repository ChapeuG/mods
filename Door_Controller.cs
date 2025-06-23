using System;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class Door_Controller : MonoBehaviour
{
	// Token: 0x060000A2 RID: 162 RVA: 0x00007D54 File Offset: 0x00005F54
	private void Start()
	{
		for (int i = 0; i < this.doors.Length; i++)
		{
			this.startAperture[i] = this.doors[i].transform.localPosition;
			this.startRotation[i] = this.doors[i].transform.localRotation.eulerAngles;
		}
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00007DB8 File Offset: 0x00005FB8
	private void Update()
	{
		this.timeToClose_Timer += Time.deltaTime;
		if (this.timeToClose_Timer > this.timeToClose)
		{
			this.openSide = 0;
		}
		if (this.openSide != 0)
		{
			Vector3[] array = new Vector3[]
			{
				new Vector3(0f, 0f, this.startAperture[0].z - this.maxAperture),
				new Vector3(0f, 0f, this.startAperture[1].z + this.maxAperture)
			};
			Quaternion[] array2 = new Quaternion[]
			{
				Quaternion.Euler(0f, this.startRotation[0].y - this.maxAngle * (float)this.openSide, 0f),
				Quaternion.Euler(0f, this.startRotation[1].y + this.maxAngle * (float)this.openSide, 0f)
			};
			for (int i = 0; i < this.doors.Length; i++)
			{
				if (this.inLine)
				{
					this.doors[i].transform.localPosition = Vector3.Lerp(this.doors[i].transform.localPosition, array[i], this.speedAperture * Time.deltaTime);
				}
				if (this.rotate)
				{
					this.doors[i].transform.localRotation = Quaternion.Lerp(this.doors[i].transform.localRotation, array2[i], this.speedAngle * Time.deltaTime);
				}
			}
			return;
		}
		if (this.timeToClose_Timer > this.timeToClose || Game_Manager.instance.GetCinematicMode())
		{
			if (!this.mayOpenAgain)
			{
				this.mayOpenAgain = true;
			}
			for (int j = 0; j < this.doors.Length; j++)
			{
				this.doors[j].transform.localPosition = Vector3.Lerp(this.doors[j].transform.localPosition, this.startAperture[j], this.speedApertureClose * Time.deltaTime);
				this.doors[j].transform.localRotation = Quaternion.Lerp(this.doors[j].transform.localRotation, Quaternion.Euler(this.startRotation[j]), this.speedAngleClose * Time.deltaTime);
			}
			this.timeToClose_Timer += Time.deltaTime;
			if (this.timeToClose_Timer > this.timeToStop)
			{
				this.timeToClose_Timer = 0f;
			}
		}
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00008068 File Offset: 0x00006268
	public void OpenDoor(int _openSide, bool _is_player = false)
	{
		if (this.lock_Controller && this.lock_Controller.isLocked && _openSide == this.opensEvenClosed && this.isEntrance && _is_player)
		{
			this.openSide = 0;
			return;
		}
		if (Game_Manager.instance.GetCinematicMode())
		{
			this.openSide = 0;
			return;
		}
		if (this.openSide == 0)
		{
			this.openSide = _openSide;
		}
		this.timeToClose_Timer = 0f;
		if (this.oneDirection > 0)
		{
			if (_openSide < 0)
			{
				_openSide = Mathf.Abs(_openSide);
			}
		}
		else if (this.oneDirection < 0 && _openSide > 0)
		{
			_openSide = -_openSide;
		}
		if (this.mayOpenAgain)
		{
			if (this.inLine)
			{
				Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_DoorSliding_Open, base.transform, Audio_Manager.instance.event_null);
			}
			if (this.rotate)
			{
				Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_DoorNormal_Open, base.transform, Audio_Manager.instance.event_null);
			}
			this.mayOpenAgain = false;
		}
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x0000816C File Offset: 0x0000636C
	public void OpenDoor(Vector3 _position, bool _is_player = false)
	{
		if (this.doorSider.Length == 0)
		{
			return;
		}
		int num = 1;
		float num2 = Vector3.Distance(_position, this.doorSider[0].transform.position);
		float num3 = Vector3.Distance(_position, this.doorSider[1].transform.position);
		if (num2 > num3)
		{
			num = -1;
		}
		this.OpenDoor(num, _is_player);
	}

	// Token: 0x040000CF RID: 207
	[SerializeField]
	public GameObject[] doorSider;

	// Token: 0x040000D0 RID: 208
	[SerializeField]
	private Lock_Controller lock_Controller;

	// Token: 0x040000D1 RID: 209
	[SerializeField]
	private GameObject[] doors = new GameObject[2];

	// Token: 0x040000D2 RID: 210
	[SerializeField]
	private bool rotate;

	// Token: 0x040000D3 RID: 211
	[SerializeField]
	private int oneDirection;

	// Token: 0x040000D4 RID: 212
	[SerializeField]
	private float maxAngle;

	// Token: 0x040000D5 RID: 213
	[SerializeField]
	private float speedAngle;

	// Token: 0x040000D6 RID: 214
	[SerializeField]
	private float speedAngleClose;

	// Token: 0x040000D7 RID: 215
	[SerializeField]
	private bool inLine;

	// Token: 0x040000D8 RID: 216
	[SerializeField]
	private float maxAperture;

	// Token: 0x040000D9 RID: 217
	[SerializeField]
	private float speedAperture;

	// Token: 0x040000DA RID: 218
	[SerializeField]
	private float speedApertureClose;

	// Token: 0x040000DB RID: 219
	[SerializeField]
	private float timeToClose = 3f;

	// Token: 0x040000DC RID: 220
	[SerializeField]
	private float timeToStop = 3f;

	// Token: 0x040000DD RID: 221
	[SerializeField]
	private bool OnlyWhenClosed;

	// Token: 0x040000DE RID: 222
	[SerializeField]
	private int opensEvenClosed;

	// Token: 0x040000DF RID: 223
	[SerializeField]
	private bool isEntrance;

	// Token: 0x040000E0 RID: 224
	[SerializeField]
	private int openSide;

	// Token: 0x040000E1 RID: 225
	private float timeToClose_Timer;

	// Token: 0x040000E2 RID: 226
	private Vector3[] startAperture = new Vector3[2];

	// Token: 0x040000E3 RID: 227
	private Vector3[] startRotation = new Vector3[2];

	// Token: 0x040000E4 RID: 228
	private bool mayOpenAgain = true;
}
