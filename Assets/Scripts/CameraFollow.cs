using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloatExtensions;
public class CameraFollow : MonoBehaviour
{
	public Transform target;
	public Vector3 biaPos;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		Vector3 tarPos = target.position;
		transform.position = GetSimiler(transform.position, tarPos) + biaPos;
	}

	Vector3 GetSimiler(Vector3 basep, Vector3 tarp, float margin = 0)
	{
		var ans = tarp;
		if (tarp.x.IsEqual(basep.x, margin)) { ans.x = basep.x; }
		if (tarp.y.IsEqual(basep.y, margin)) { ans.y = basep.y; }
		if (tarp.z.IsEqual(basep.z, margin)) { ans.z = basep.z; }
		return ans;
	}
}
