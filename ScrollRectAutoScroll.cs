using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200005E RID: 94
[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAutoScroll : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600052C RID: 1324 RVA: 0x00032083 File Offset: 0x00030283
	private void OnEnable()
	{
		if (this.m_ScrollRect)
		{
			this.m_ScrollRect.content.GetComponentsInChildren<Button>(this.m_Selectables);
		}
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x000320A8 File Offset: 0x000302A8
	private void Awake()
	{
		this.m_ScrollRect = base.GetComponent<ScrollRect>();
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x000320B6 File Offset: 0x000302B6
	private void Start()
	{
		this._Start();
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x000320C0 File Offset: 0x000302C0
	public void _Start()
	{
		if (this.m_ScrollRect)
		{
			this.m_ScrollRect.content.GetComponentsInChildren<Button>(this.m_Selectables);
			foreach (Button button in this.m_Selectables)
			{
				if (!button.gameObject.activeSelf)
				{
					this.m_Selectables.Remove(button);
				}
			}
		}
		this.ScrollToSelected(true);
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x00032150 File Offset: 0x00030350
	private void Update()
	{
		if (Input_Manager.instance.GetScheme(-1) != "Joystick")
		{
			return;
		}
		this.InputScroll();
		this.m_ScrollRect.normalizedPosition = Vector2.Lerp(this.m_ScrollRect.normalizedPosition, this.m_NextScrollPosition, this.scrollSpeed * Time.unscaledDeltaTime);
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x000321A8 File Offset: 0x000303A8
	private void InputScroll()
	{
		if (this.m_Selectables.Count > 0)
		{
			this.ScrollToSelected(false);
		}
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x000321C0 File Offset: 0x000303C0
	private void ScrollToSelected(bool quickScroll)
	{
		this._selectedElement = Menu_Manager.instance.GetButtonSelected();
		if (this._selectedElement)
		{
			this.selectedIndex = (float)this.m_Selectables.IndexOf(this._selectedElement);
		}
		if (this.selectedIndex > -1f)
		{
			if (this.selectedIndex >= (float)this.m_Selectables.Count)
			{
				this.selectedIndex = (float)Mathf.CeilToInt(this.selectedIndex / (float)(this.multiplier - 1));
			}
			else
			{
				this.selectedIndex = (float)Mathf.FloorToInt(this.selectedIndex / (float)this.multiplier);
			}
			if (quickScroll)
			{
				this.m_ScrollRect.normalizedPosition = new Vector2(0f, 1f - this.selectedIndex / (((float)this.m_Selectables.Count - (float)this.multiplier) / (float)this.multiplier));
				this.m_NextScrollPosition = this.m_ScrollRect.normalizedPosition;
				return;
			}
			this.m_NextScrollPosition = new Vector2(0f, 1f - this.selectedIndex / (((float)this.m_Selectables.Count - (float)this.multiplier) / (float)this.multiplier));
		}
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x000322EA File Offset: 0x000304EA
	public void OnPointerEnter(PointerEventData eventData)
	{
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x000322EC File Offset: 0x000304EC
	public void OnPointerExit(PointerEventData eventData)
	{
	}

	// Token: 0x04000642 RID: 1602
	public int multiplier = 1;

	// Token: 0x04000643 RID: 1603
	public float scrollSpeed = 10f;

	// Token: 0x04000644 RID: 1604
	public List<Button> m_Selectables = new List<Button>();

	// Token: 0x04000645 RID: 1605
	public ScrollRect m_ScrollRect;

	// Token: 0x04000646 RID: 1606
	private Vector2 m_NextScrollPosition = Vector2.up;

	// Token: 0x04000647 RID: 1607
	public float selectedIndex = -1f;

	// Token: 0x04000648 RID: 1608
	public Button _selectedElement;
}
