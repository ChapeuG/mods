using System;
using UnityEngine;

// Token: 0x0200005F RID: 95
[Serializable]
public struct SerializableVector3
{
	// Token: 0x06000536 RID: 1334 RVA: 0x00032329 File Offset: 0x00030529
	public SerializableVector3(float rX, float rY, float rZ)
	{
		this.x = rX;
		this.y = rY;
		this.z = rZ;
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x00032340 File Offset: 0x00030540
	public override string ToString()
	{
		return string.Format("[{0}, {1}, {2}]", this.x, this.y, this.z);
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x0003236D File Offset: 0x0003056D
	public static implicit operator Vector3(SerializableVector3 rValue)
	{
		return new Vector3(rValue.x, rValue.y, rValue.z);
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x00032386 File Offset: 0x00030586
	public static implicit operator SerializableVector3(Vector3 rValue)
	{
		return new SerializableVector3(rValue.x, rValue.y, rValue.z);
	}

	// Token: 0x04000649 RID: 1609
	public float x;

	// Token: 0x0400064A RID: 1610
	public float y;

	// Token: 0x0400064B RID: 1611
	public float z;
}
