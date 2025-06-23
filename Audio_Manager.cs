using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000036 RID: 54
public class Audio_Manager : MonoBehaviour
{
	// Token: 0x060001C2 RID: 450 RVA: 0x000115DA File Offset: 0x0000F7DA
	private void Awake()
	{
		if (!Audio_Manager.instance)
		{
			Audio_Manager.instance = this;
		}
		this.music_EI = base.transform.Find("Music").GetComponent<StudioEventEmitter>();
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x0001160C File Offset: 0x0000F80C
	public void PlaySound(EventReference _event, Transform _trans, EventReference _materialMade)
	{
		if (_event.IsNull)
		{
			return;
		}
		EventInstance eventInstance = RuntimeManager.CreateInstance(_event);
		RuntimeManager.AttachInstanceToGameObject(eventInstance, _trans, base.GetComponent<Rigidbody>());
		eventInstance.start();
		eventInstance.release();
		if (_materialMade.IsNull)
		{
			return;
		}
		EventInstance eventInstance2 = RuntimeManager.CreateInstance(_materialMade);
		RuntimeManager.AttachInstanceToGameObject(eventInstance2, _trans, base.GetComponent<Rigidbody>());
		eventInstance2.start();
		eventInstance2.release();
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x00011675 File Offset: 0x0000F875
	public void PlaySound(EventReference _event)
	{
		this.PlaySound(_event, base.transform, this.event_null);
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0001168C File Offset: 0x0000F88C
	public void ChangeMusic_ByIndex(int _index)
	{
		this.music_index = _index;
		if (_index >= this.music_songsAvailable.Length)
		{
			return;
		}
		this.music_EI.Stop();
		this.music_EI.EventReference = this.music_songsAvailable[_index];
		this.music_EI.Play();
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x000116DC File Offset: 0x0000F8DC
	public void ChangeMusic_Dir(int _dir)
	{
		this.music_index += _dir;
		if (this.music_index >= this.music_songsAvailable.Length)
		{
			this.music_index = 0;
		}
		else if (this.music_index <= 0)
		{
			this.music_index = this.music_songsAvailable.Length - 1;
		}
		this.ChangeMusic_ByIndex(this.music_index);
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x00011738 File Offset: 0x0000F938
	public void ChangeMusic_RandomList_Next()
	{
		Debug.LogWarning("Trying... Next Random Music!");
		if (this.music_songsRandomList.Count == 0)
		{
			this.music_songsRandomList = this.Music_CreateRandomList();
		}
		if (this.music_songsRandomList.Count == 0)
		{
			return;
		}
		this.music_EI.Stop();
		this.music_EI.EventReference = this.music_songsRandomList[0];
		this.music_EI.Play();
		this.music_songsRandomList.RemoveAt(0);
		Debug.LogWarning("DONE - Next Random Music!");
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x000117BC File Offset: 0x0000F9BC
	private List<EventReference> Music_CreateRandomList()
	{
		List<EventReference> list = new List<EventReference>();
		List<EventReference> list2 = new List<EventReference>(this.music_songsAvailable);
		while (list2.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, list2.Count);
			list.Add(list2[index]);
			list2.RemoveAt(index);
			list2.TrimExcess();
		}
		return list;
	}

	// Token: 0x04000295 RID: 661
	public static Audio_Manager instance;

	// Token: 0x04000296 RID: 662
	[Header("General")]
	public EventReference event_Rotate;

	// Token: 0x04000297 RID: 663
	public EventReference event_GrabBox;

	// Token: 0x04000298 RID: 664
	public EventReference event_GrabLight;

	// Token: 0x04000299 RID: 665
	public EventReference event_GrabMedium;

	// Token: 0x0400029A RID: 666
	public EventReference event_GrabHeavy;

	// Token: 0x0400029B RID: 667
	public EventReference event_PlaceBox;

	// Token: 0x0400029C RID: 668
	public EventReference event_PlaceItem;

	// Token: 0x0400029D RID: 669
	public EventReference event_PlaceLight;

	// Token: 0x0400029E RID: 670
	public EventReference event_PlaceMedium;

	// Token: 0x0400029F RID: 671
	public EventReference event_PlaceHeavy;

	// Token: 0x040002A0 RID: 672
	public EventReference event_DropLight;

	// Token: 0x040002A1 RID: 673
	public EventReference event_DropMedium;

	// Token: 0x040002A2 RID: 674
	public EventReference event_DropHeavy;

	// Token: 0x040002A3 RID: 675
	public EventReference event_UseFloor;

	// Token: 0x040002A4 RID: 676
	public EventReference event_UsePaint;

	// Token: 0x040002A5 RID: 677
	[Header("Player")]
	public EventReference event_PlayerWalk;

	// Token: 0x040002A6 RID: 678
	public EventReference event_PlayerRun;

	// Token: 0x040002A7 RID: 679
	[Header("Other Characters")]
	public EventReference event_CharWalk;

	// Token: 0x040002A8 RID: 680
	[Header("Door")]
	public EventReference event_DoorSliding_Open;

	// Token: 0x040002A9 RID: 681
	public EventReference event_DoorNormal_Open;

	// Token: 0x040002AA RID: 682
	[Header("UI")]
	public EventReference event_AddItem;

	// Token: 0x040002AB RID: 683
	public EventReference event_ClickButton;

	// Token: 0x040002AC RID: 684
	public EventReference event_CloseWindow;

	// Token: 0x040002AD RID: 685
	public EventReference event_HoverButton;

	// Token: 0x040002AE RID: 686
	public EventReference event_OpenWindow;

	// Token: 0x040002AF RID: 687
	public EventReference event_RemoveItem;

	// Token: 0x040002B0 RID: 688
	public EventReference event_StartGame;

	// Token: 0x040002B1 RID: 689
	[Header("Soccer")]
	public EventReference event_soccer_shot;

	// Token: 0x040002B2 RID: 690
	public EventReference event_soccer_kick;

	// Token: 0x040002B3 RID: 691
	public EventReference event_soccer_goal;

	// Token: 0x040002B4 RID: 692
	[Header("Cashier")]
	public EventReference event_Cashier_RightButton;

	// Token: 0x040002B5 RID: 693
	public EventReference event_Cashier_WrongButton;

	// Token: 0x040002B6 RID: 694
	[HideInInspector]
	public EventReference event_null;

	// Token: 0x040002B7 RID: 695
	public StudioEventEmitter music_EI;

	// Token: 0x040002B8 RID: 696
	public bool music_isPlaying = true;

	// Token: 0x040002B9 RID: 697
	public UnityEvent playMusic_Event = new UnityEvent();

	// Token: 0x040002BA RID: 698
	public UnityEvent stopMusic_Event = new UnityEvent();

	// Token: 0x040002BB RID: 699
	public int music_index;

	// Token: 0x040002BC RID: 700
	public EventReference[] music_songsAvailable;

	// Token: 0x040002BD RID: 701
	public List<EventReference> music_songsRandomList = new List<EventReference>();
}
