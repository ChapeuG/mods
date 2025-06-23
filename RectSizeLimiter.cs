using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200005D RID: 93
[ExecuteInEditMode]
public class RectSizeLimiter : UIBehaviour, ILayoutSelfController, ILayoutController
{
	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000522 RID: 1314 RVA: 0x00031E3B File Offset: 0x0003003B
	// (set) Token: 0x06000523 RID: 1315 RVA: 0x00031E43 File Offset: 0x00030043
	public Vector2 maxSize
	{
		get
		{
			return this.m_maxSize;
		}
		set
		{
			if (this.m_maxSize != value)
			{
				this.m_maxSize = value;
				this.SetDirty();
			}
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000524 RID: 1316 RVA: 0x00031E60 File Offset: 0x00030060
	// (set) Token: 0x06000525 RID: 1317 RVA: 0x00031E68 File Offset: 0x00030068
	public Vector2 minSize
	{
		get
		{
			return this.m_minSize;
		}
		set
		{
			if (this.m_minSize != value)
			{
				this.m_minSize = value;
				this.SetDirty();
			}
		}
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00031E85 File Offset: 0x00030085
	protected override void OnEnable()
	{
		base.OnEnable();
		this.SetDirty();
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00031E93 File Offset: 0x00030093
	protected override void OnDisable()
	{
		this.m_Tracker.Clear();
		LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
		base.OnDisable();
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x00031EB1 File Offset: 0x000300B1
	protected void SetDirty()
	{
		if (!this.IsActive())
		{
			return;
		}
		LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00031EC8 File Offset: 0x000300C8
	public void SetLayoutHorizontal()
	{
		if (this.m_maxSize.x > 0f && this.rectTransform.rect.width > this.m_maxSize.x)
		{
			this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.maxSize.x);
			this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.SizeDeltaX);
		}
		if (this.m_minSize.x > 0f && this.rectTransform.rect.width < this.m_minSize.x)
		{
			this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.minSize.x);
			this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.SizeDeltaX);
		}
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x00031F98 File Offset: 0x00030198
	public void SetLayoutVertical()
	{
		if (this.m_maxSize.y > 0f && this.rectTransform.rect.height > this.m_maxSize.y)
		{
			this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.maxSize.y);
			this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.SizeDeltaY);
		}
		if (this.m_minSize.y > 0f && this.rectTransform.rect.height < this.m_minSize.y)
		{
			this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.minSize.y);
			this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.SizeDeltaY);
		}
	}

	// Token: 0x0400063E RID: 1598
	public RectTransform rectTransform;

	// Token: 0x0400063F RID: 1599
	[SerializeField]
	protected Vector2 m_maxSize = Vector2.zero;

	// Token: 0x04000640 RID: 1600
	[SerializeField]
	protected Vector2 m_minSize = Vector2.zero;

	// Token: 0x04000641 RID: 1601
	private DrivenRectTransformTracker m_Tracker;
}
