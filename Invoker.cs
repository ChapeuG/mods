using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class Invoker : MonoBehaviour
{
	// Token: 0x1700000D RID: 13
	// (get) Token: 0x0600051C RID: 1308 RVA: 0x00031C7C File Offset: 0x0002FE7C
	public static Invoker Instance
	{
		get
		{
			if (Invoker._instance == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.AddComponent<Invoker>();
				gameObject.name = "_FunoniumInvoker";
				Invoker._instance = gameObject.GetComponent<Invoker>();
			}
			return Invoker._instance;
		}
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00031CB1 File Offset: 0x0002FEB1
	public float RealDeltaTime()
	{
		return this.fRealDeltaTime;
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x00031CB9 File Offset: 0x0002FEB9
	public static void InvokeDelayed(Invokable func, float delaySeconds)
	{
		Invoker.Instance.invokeListPendingAddition.Add(new Invoker.InvokableItem(func, delaySeconds));
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x00031CD4 File Offset: 0x0002FED4
	public void Update()
	{
		this.fRealDeltaTime = Time.realtimeSinceStartup - this.fRealTimeLastFrame;
		this.fRealTimeLastFrame = Time.realtimeSinceStartup;
		foreach (Invoker.InvokableItem item in this.invokeListPendingAddition)
		{
			this.invokeList.Add(item);
		}
		this.invokeListPendingAddition.Clear();
		foreach (Invoker.InvokableItem invokableItem in this.invokeList)
		{
			if (invokableItem.executeAtTime <= Time.realtimeSinceStartup)
			{
				if (invokableItem.func != null)
				{
					invokableItem.func();
				}
				this.invokeListExecuted.Add(invokableItem);
			}
		}
		foreach (Invoker.InvokableItem item2 in this.invokeListExecuted)
		{
			this.invokeList.Remove(item2);
		}
		this.invokeListExecuted.Clear();
	}

	// Token: 0x04000638 RID: 1592
	private static Invoker _instance;

	// Token: 0x04000639 RID: 1593
	private float fRealTimeLastFrame;

	// Token: 0x0400063A RID: 1594
	private float fRealDeltaTime;

	// Token: 0x0400063B RID: 1595
	private List<Invoker.InvokableItem> invokeList = new List<Invoker.InvokableItem>();

	// Token: 0x0400063C RID: 1596
	private List<Invoker.InvokableItem> invokeListPendingAddition = new List<Invoker.InvokableItem>();

	// Token: 0x0400063D RID: 1597
	private List<Invoker.InvokableItem> invokeListExecuted = new List<Invoker.InvokableItem>();

	// Token: 0x02000099 RID: 153
	private struct InvokableItem
	{
		// Token: 0x060005EF RID: 1519 RVA: 0x000353AD File Offset: 0x000335AD
		public InvokableItem(Invokable func, float delaySeconds)
		{
			this.func = func;
			if (Time.time == 0f)
			{
				this.executeAtTime = delaySeconds;
				return;
			}
			this.executeAtTime = Time.realtimeSinceStartup + delaySeconds;
		}

		// Token: 0x04000725 RID: 1829
		public Invokable func;

		// Token: 0x04000726 RID: 1830
		public float executeAtTime;

		// Token: 0x04000727 RID: 1831
		public static Invoker _instance;
	}
}
