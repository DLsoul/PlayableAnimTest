using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestState : MonoBehaviour, IEntity
{
	public Animator animator;
	private Rigidbody rb;
	public PlayableAnimCtrl AnimCtrl { get; private set; }

	public float runSpeed;
	public float doadgeForce;
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
		AnimCtrl = new PlayableAnimCtrl();
		AnimCtrl.Init(animator, this);
	}

	private void OnEnable()
	{
		InputSystem.Instance.OnAxis += Move;
		InputSystem.Instance.OnXKeyDown += AttackX;
		InputSystem.Instance.OnYKeyDown += AttackY;
		InputSystem.Instance.OnAKeyDown += Doadge;
	}

	bool m_isRuning { get { return AnimCtrl.GetCurClipName().Equals("Run_ver_A"); } }
	private void OnDisable()
	{
		InputSystem.Instance.OnAxis -= Move;
		InputSystem.Instance.OnXKeyDown -= AttackX;
		InputSystem.Instance.OnYKeyDown -= AttackY;
		InputSystem.Instance.OnAKeyDown -= Doadge;
	}

	bool CheckAnim(string name)
	{
		return AnimCtrl.GetCurClipName().Equals(name);
	}


	void Move(float h, float v)
	{
		float lastHspeed = rb.velocity.z;
		float hSpeed = lastHspeed;
		if (h != 0)
		{
			if (CheckAnim("Idle"))
			{
				AnimCtrl.Play("Run_ver_A");
			}
			transform.localEulerAngles = new Vector3(0, h < 0 ? 180 : 0, 0);
			if (m_isRuning)
			{
				hSpeed = h > 0 ? runSpeed : -runSpeed;
			}

		}
		else
		{
			if (m_isRuning) { AnimCtrl.Play("Idle"); hSpeed = 0; Debug.Log("hspeed: 0"); }
		}

		if (v != 0)
		{

		}

		if (hSpeed != lastHspeed) { SetVelocity(hSpeed, rb.velocity.y); }

	}



	int xtick;
	int ytick;


	void AttackX()
	{
		AnimCtrl.Play($"Attack_3Combo_{xtick + 1}");
		++xtick;
		xtick %= 3;
	}

	void AttackY()
	{

		AnimCtrl.Play($"Attack_7Combo_{ytick + 1}");
		++ytick;
		ytick %= 7;
	}

	void Doadge()
	{
		AnimCtrl.Play($"Dodge_Front");

		var dir = InputSystem.Instance.inputDir;
		float zForce;
		if (dir.x == 0) { zForce = DirRight(transform.localEulerAngles.y) ? doadgeForce : -doadgeForce; }
		else { zForce = dir.x > 0 ? doadgeForce : -doadgeForce; }
		rb.AddForce(0, 0, zForce);

		//Debug.Log($"Zforce:{zForce}");
	}


	bool DirRight(float yAngle)
	{
		bool isRight = true;
		float abs = Mathf.Abs(yAngle) % 360;
		if (90 <= abs && abs < 270) { isRight = false; }
		return isRight;
	}

	void SetVelocity(float x, float y)
	{
		rb.velocity = new Vector3(0, y, x);
	}

	MoveDataComponent movedata;
	bool inForceDash;
	void ForceDash()
	{
		if (!inForceDash) { return; }

	}
}

struct MoveDataComponent
{
	public Vector2 forceDir;
	public float forcePower;
	public float forceAttenuation;
	public float duration;
}