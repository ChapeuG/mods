using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class Ball_Controller : MonoBehaviour
{
	// Token: 0x06000009 RID: 9 RVA: 0x0000214B File Offset: 0x0000034B
	public void DestroyBall()
	{
		this.particles.Play();
		this.particles.transform.SetParent(null);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002174 File Offset: 0x00000374
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Goal")
		{
			int index = 1;
			if (other.gameObject.name == "Goal (1)")
			{
				index = 0;
			}
			EasterEgg_Manager.instance.Soccer_Set_Goal(index);
		}
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000021BE File Offset: 0x000003BE
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Audio_Manager.instance.PlaySound(Audio_Manager.instance.event_soccer_kick);
		}
	}

	// Token: 0x04000011 RID: 17
	[SerializeField]
	private ParticleSystem particles;
}
