using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class Tween_Controller : MonoBehaviour
{
	// Token: 0x060001A7 RID: 423 RVA: 0x00010577 File Offset: 0x0000E777
	public void Start_Coroutine()
	{
		base.StartCoroutine(this.coroutine);
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00010586 File Offset: 0x0000E786
	public void Stop_Coroutine()
	{
		base.StopCoroutine(this.coroutine);
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00010594 File Offset: 0x0000E794
	public IEnumerator Tween_Canvas_Alpha(GameObject _obj, float _start, float _final, float _time)
	{
		Debug.Log("STarting");
		CanvasGroup _canvas_group = null;
		Component component;
		if (!_obj.TryGetComponent(typeof(CanvasGroup), out component))
		{
			_obj.AddComponent<CanvasGroup>();
		}
		_canvas_group = _obj.GetComponent<CanvasGroup>();
		if (_final == 0f)
		{
			_canvas_group.interactable = false;
			_canvas_group.blocksRaycasts = false;
		}
		else
		{
			_canvas_group.interactable = true;
			_canvas_group.blocksRaycasts = true;
		}
		_canvas_group.alpha = _start;
		this.elapsed_time = 0f;
		while (_canvas_group.alpha != _final)
		{
			_canvas_group.alpha = Mathf.Lerp(_canvas_group.alpha, _final, this.elapsed_time / _time);
			this.elapsed_time += Time.deltaTime;
			yield return null;
		}
		_canvas_group.alpha = _final;
		if (_final == 0f)
		{
			_obj.SetActive(false);
			_canvas_group.interactable = true;
			_canvas_group.blocksRaycasts = true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x04000233 RID: 563
	public IEnumerator coroutine;

	// Token: 0x04000234 RID: 564
	public float elapsed_time;
}
