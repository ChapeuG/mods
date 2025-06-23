using System;
using Steamworks;
using UnityEngine;

// Token: 0x02000061 RID: 97
public class SteamScript : MonoBehaviour
{
	// Token: 0x06000544 RID: 1348 RVA: 0x0003255C File Offset: 0x0003075C
	private void OnEnable()
	{
		if (SteamManager.Initialized)
		{
			this.m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(this.OnGameOverlayActivated));
		}
		if (SteamManager.Initialized)
		{
			this.m_NumberOfCurrentPlayers = CallResult<NumberOfCurrentPlayers_t>.Create(new CallResult<NumberOfCurrentPlayers_t>.APIDispatchDelegate(this.OnNumberOfCurrentPlayers));
		}
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x0003259C File Offset: 0x0003079C
	private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
	{
		if (pCallback.m_bActive != 0)
		{
			Debug.Log("Steam Overlay has been activated");
			if (Menu_Manager.instance.GetMenuName() == "MainMenu")
			{
				Menu_Manager.instance.SetMenuName("Pause");
				return;
			}
		}
		else
		{
			Debug.Log("Steam Overlay has been closed");
		}
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x000325EB File Offset: 0x000307EB
	private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIOFailure)
	{
		if (pCallback.m_bSuccess != 1 || bIOFailure)
		{
			Debug.Log("There was an error retrieving the NumberOfCurrentPlayers.");
			return;
		}
		Debug.Log("The number of players playing your game: " + pCallback.m_cPlayers.ToString());
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x00032623 File Offset: 0x00030823
	private void Start()
	{
		if (SteamManager.Initialized)
		{
			Debug.Log(SteamFriends.GetPersonaName());
		}
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x00032636 File Offset: 0x00030836
	private void Update()
	{
	}

	// Token: 0x04000650 RID: 1616
	protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

	// Token: 0x04000651 RID: 1617
	private CallResult<NumberOfCurrentPlayers_t> m_NumberOfCurrentPlayers;

	// Token: 0x04000652 RID: 1618
	[SerializeField]
	private bool runSteam;
}
