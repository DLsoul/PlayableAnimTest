using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySystem : MonoBehaviour
{
	public float gravity;
	public float gravityScale = 1;
	public bool useGravity = true;

	Rigidbody rb;
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (!useGravity) { return; }
		rb.AddForce(0, -gravity * gravityScale, 0);
	}
}
