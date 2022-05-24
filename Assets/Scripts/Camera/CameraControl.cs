using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloatExtensions;
public class CameraControl : MonoBehaviour
{
	public Transform target;
	public Vector3 offsetPos;
	public Vector2 boardMinPos;
	public Vector2 boardMaxPos;
	public float speed;

	public Vector2 extraOff;
	private void FixedUpdate()
	{
		Vector3 off = offsetPos;
		off.y += extraOff.y;
		off.z += extraOff.x;
		if (target.localEulerAngles.y.IsEqual(180, 0.01f)) { off.z = -off.z; }

		Vector3 pos = target.position + off;
		pos.z = Mathf.Max(boardMinPos.x, pos.z);
		pos.z = Mathf.Min(boardMaxPos.x, pos.z);
		pos.y = Mathf.Max(boardMinPos.y, pos.y);
		pos.y = Mathf.Min(boardMaxPos.y, pos.y);
		Vector3 dir = (pos - transform.position);
		if (dir.sqrMagnitude > 0.01f)
		{
			transform.Translate(dir * speed * Time.fixedDeltaTime, Space.World);
		}
		//transform.position = pos;
	}
}
