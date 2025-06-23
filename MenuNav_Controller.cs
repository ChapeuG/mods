using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class MenuNav_Controller : MonoBehaviour
{
	// Token: 0x060000F0 RID: 240 RVA: 0x0000AC1C File Offset: 0x00008E1C
	private void Start()
	{
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x0000AC1E File Offset: 0x00008E1E
	public void ResetButtons()
	{
		this.nav_Up = null;
		this.nav_Down = null;
		this.nav_Left = null;
		this.nav_Right = null;
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x0000AC3C File Offset: 0x00008E3C
	public GameObject GetButton(int hor, int ver)
	{
		GameObject gameObject = null;
		if (hor > 0)
		{
			if (Menu_Manager.instance.GetMenuName() == "Settings")
			{
				Settings_Manager.instance.Button_HorizontalMovementHandler(base.gameObject, 1);
			}
			else
			{
				if (!(this.nav_Right != null))
				{
					return null;
				}
				if (this.nav_Right.activeSelf)
				{
					gameObject = this.nav_Right;
				}
				else if (this.nav_Right_fallback != null)
				{
					if (this.nav_Right_fallback.activeSelf)
					{
						gameObject = this.nav_Right_fallback;
					}
				}
				else
				{
					GameObject gameObject2 = this.nav_Right;
					for (int i = 0; i < 3; i++)
					{
						if (gameObject2.GetComponent<MenuNav_Controller>().nav_Right != null)
						{
							if (gameObject2.GetComponent<MenuNav_Controller>().nav_Right.activeSelf)
							{
								gameObject = gameObject2.GetComponent<MenuNav_Controller>().nav_Right;
								break;
							}
							gameObject2 = gameObject2.GetComponent<MenuNav_Controller>().nav_Right;
						}
					}
				}
			}
		}
		else if (hor < 0)
		{
			if (Menu_Manager.instance.GetMenuName() == "Settings")
			{
				Settings_Manager.instance.Button_HorizontalMovementHandler(base.gameObject, -1);
			}
			else
			{
				if (!(this.nav_Left != null))
				{
					return null;
				}
				if (this.nav_Left.activeSelf)
				{
					gameObject = this.nav_Left;
				}
				else if (this.nav_Left_fallback != null)
				{
					if (this.nav_Left_fallback.activeSelf)
					{
						gameObject = this.nav_Left_fallback;
					}
				}
				else
				{
					GameObject gameObject3 = this.nav_Left;
					for (int j = 0; j < 3; j++)
					{
						if (gameObject3.GetComponent<MenuNav_Controller>().nav_Left != null)
						{
							if (gameObject3.GetComponent<MenuNav_Controller>().nav_Left.activeSelf)
							{
								gameObject = gameObject3.GetComponent<MenuNav_Controller>().nav_Left;
								break;
							}
							gameObject3 = gameObject3.GetComponent<MenuNav_Controller>().nav_Left;
						}
					}
				}
			}
		}
		else if (ver > 0)
		{
			if (!(this.nav_Up != null))
			{
				return null;
			}
			if (this.nav_Up.activeSelf)
			{
				gameObject = this.nav_Up;
			}
			else if (this.nav_Up_fallback != null)
			{
				if (this.nav_Up_fallback.activeSelf)
				{
					gameObject = this.nav_Up_fallback;
				}
			}
			else
			{
				GameObject gameObject4 = this.nav_Up;
				for (int k = 0; k < 3; k++)
				{
					if (gameObject4.GetComponent<MenuNav_Controller>().nav_Up != null)
					{
						if (gameObject4.GetComponent<MenuNav_Controller>().nav_Up.activeSelf)
						{
							gameObject = gameObject4.GetComponent<MenuNav_Controller>().nav_Up;
							break;
						}
						gameObject4 = gameObject4.GetComponent<MenuNav_Controller>().nav_Up;
					}
				}
			}
		}
		else if (ver < 0)
		{
			if (!(this.nav_Down != null))
			{
				return null;
			}
			if (this.nav_Down.activeSelf)
			{
				gameObject = this.nav_Down;
			}
			else if (this.nav_Down_fallback != null)
			{
				if (this.nav_Down_fallback.activeSelf)
				{
					gameObject = this.nav_Down_fallback;
				}
			}
			else
			{
				GameObject gameObject5 = this.nav_Down;
				for (int l = 0; l < 3; l++)
				{
					if (gameObject5.GetComponent<MenuNav_Controller>().nav_Down != null)
					{
						if (gameObject5.GetComponent<MenuNav_Controller>().nav_Down.activeSelf)
						{
							Debug.Log("Found");
							gameObject = gameObject5.GetComponent<MenuNav_Controller>().nav_Down;
							break;
						}
						gameObject5 = gameObject5.GetComponent<MenuNav_Controller>().nav_Down;
					}
				}
			}
		}
		if (gameObject != null && !gameObject.activeInHierarchy && this.work_only_for_actives)
		{
			gameObject = null;
		}
		if (gameObject)
		{
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_HoverButton);
		}
		return gameObject;
	}

	// Token: 0x04000168 RID: 360
	[Header("Default")]
	public GameObject nav_Up;

	// Token: 0x04000169 RID: 361
	public GameObject nav_Down;

	// Token: 0x0400016A RID: 362
	public GameObject nav_Left;

	// Token: 0x0400016B RID: 363
	public GameObject nav_Right;

	// Token: 0x0400016C RID: 364
	[Header("FallBack")]
	public GameObject nav_Up_fallback;

	// Token: 0x0400016D RID: 365
	public GameObject nav_Down_fallback;

	// Token: 0x0400016E RID: 366
	public GameObject nav_Left_fallback;

	// Token: 0x0400016F RID: 367
	public GameObject nav_Right_fallback;

	// Token: 0x04000170 RID: 368
	public bool work_only_for_actives;
}
