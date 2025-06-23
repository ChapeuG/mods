using System;
using System.Text;
using AOT;
using Steamworks;
using UnityEngine;

// Token: 0x02000060 RID: 96
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x17000010 RID: 16
	// (get) Token: 0x0600053A RID: 1338 RVA: 0x0003239F File Offset: 0x0003059F
	protected static SteamManager Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x0600053B RID: 1339 RVA: 0x000323C3 File Offset: 0x000305C3
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x000323CF File Offset: 0x000305CF
	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x000323D7 File Offset: 0x000305D7
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void InitOnPlayMode()
	{
		SteamManager.s_EverInitialized = false;
		SteamManager.s_instance = null;
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x000323E8 File Offset: 0x000305E8
	protected virtual void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
			{
				Debug.Log("[Steamworks.NET] Shutting down because RestartAppIfNecessary returned true. Steam will restart the application.");
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			string str = "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n";
			DllNotFoundException ex2 = ex;
			Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null), this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInitialized = true;
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x000324D0 File Offset: 0x000306D0
	protected virtual void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x0003251E File Offset: 0x0003071E
	protected virtual void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x00032542 File Offset: 0x00030742
	protected virtual void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x0400064C RID: 1612
	protected static bool s_EverInitialized;

	// Token: 0x0400064D RID: 1613
	protected static SteamManager s_instance;

	// Token: 0x0400064E RID: 1614
	protected bool m_bInitialized;

	// Token: 0x0400064F RID: 1615
	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
