using System;
using UnityEngine;

// Token: 0x02000029 RID: 41
[Serializable]
public class Staff_Data
{
	// Token: 0x06000184 RID: 388 RVA: 0x0000F14C File Offset: 0x0000D34C
	public void Randomize()
	{
		Player_Manager instance = Player_Manager.instance;
		Char_Manager instance2 = Char_Manager.instance;
		this.skinMat = UnityEngine.Random.Range(0, instance.material_SkinColors.Count);
		this.clothesMat = UnityEngine.Random.Range(0, instance.material_Clothes.Count);
		this.hairMat = UnityEngine.Random.Range(0, instance.material_HairColors.Count);
		this.hairMesh = UnityEngine.Random.Range(0, instance.mesh_Hairs.Count);
		this.hatMat = UnityEngine.Random.Range(0, instance.material_Clothes.Count);
		this.eyeMat = UnityEngine.Random.Range(0, instance.material_Eyes.Count);
		float num = 0f;
		this.name = instance2.staff_names[UnityEngine.Random.Range(0, instance2.staff_names.Length)];
		for (int i = 0; i < this.skills.Length; i++)
		{
			this.skills[i] = UnityEngine.Random.Range(0f, 1f);
			num += this.skills[i];
		}
		int[] array = new int[]
		{
			50,
			250
		};
		this.price = Mathf.FloorToInt((float)(array[1] - array[0]) * num / (float)this.skills.Length) + array[0];
	}

	// Token: 0x0400020B RID: 523
	[SerializeField]
	public string name = "Employee";

	// Token: 0x0400020C RID: 524
	[SerializeField]
	public int skinMat;

	// Token: 0x0400020D RID: 525
	[SerializeField]
	public int clothesMat;

	// Token: 0x0400020E RID: 526
	[SerializeField]
	public int hairMat;

	// Token: 0x0400020F RID: 527
	[SerializeField]
	public int hairMesh;

	// Token: 0x04000210 RID: 528
	[SerializeField]
	public int hatGo;

	// Token: 0x04000211 RID: 529
	[SerializeField]
	public int hatMat;

	// Token: 0x04000212 RID: 530
	[SerializeField]
	public int eyeMat;

	// Token: 0x04000213 RID: 531
	[SerializeField]
	public bool[] workingDays = new bool[]
	{
		true,
		true,
		true,
		true,
		true,
		true,
		true
	};

	// Token: 0x04000214 RID: 532
	[SerializeField]
	public bool[] tasks = new bool[]
	{
		true,
		true,
		true
	};

	// Token: 0x04000215 RID: 533
	[SerializeField]
	public float[] skills = new float[]
	{
		1f,
		1f,
		1f
	};

	// Token: 0x04000216 RID: 534
	[SerializeField]
	public float energy = 1f;

	// Token: 0x04000217 RID: 535
	[SerializeField]
	public int price;

	// Token: 0x04000218 RID: 536
	[SerializeField]
	public int daysOff;
}
