using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000047 RID: 71
public class Math_Manager : MonoBehaviour
{
	// Token: 0x06000392 RID: 914 RVA: 0x000208CC File Offset: 0x0001EACC
	public static float GetAverageFromFloats(List<float> _list)
	{
		if (_list.Count == 0)
		{
			return 0f;
		}
		float num = 0f;
		for (int i = 0; i < _list.Count; i++)
		{
			num += _list[i];
		}
		return num / (float)_list.Count;
	}
}
