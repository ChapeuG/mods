using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class Movement_Controller : MonoBehaviour
{
	// Token: 0x04000171 RID: 369
	[Header("General")]
	[Header("Movement")]
	[SerializeField]
	private Vector3 copyMove;

	// Token: 0x04000172 RID: 370
	[SerializeField]
	private float moveSpeedWalk = 2f;

	// Token: 0x04000173 RID: 371
	[SerializeField]
	private float moveSpeedRun;

	// Token: 0x04000174 RID: 372
	[SerializeField]
	private float moveSpeedAnimationMultiplier = 2f;

	// Token: 0x04000175 RID: 373
	[SerializeField]
	private float rotSpeedWalk = 3f;

	// Token: 0x04000176 RID: 374
	[SerializeField]
	private float rotSpeedOnlyRotation = 5f;

	// Token: 0x04000177 RID: 375
	[SerializeField]
	private float leanLimit;

	// Token: 0x04000178 RID: 376
	[SerializeField]
	private float gravitySpeed = -4f;

	// Token: 0x04000179 RID: 377
	[SerializeField]
	private float chairAnim;

	// Token: 0x0400017A RID: 378
	private bool isGrounded;

	// Token: 0x0400017B RID: 379
	private Vector3 charVelocity;
}
