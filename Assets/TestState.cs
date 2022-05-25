using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TestState : ManipulativeRegisterMonoEntity, IEntity
{
	public Animator animator;
	private Rigidbody rb;
	public PlayableAnimCtrl AnimCtrl { get; private set; }
	public float runSpeed;
	public float doadgeSpeed;
	public float jumpSpeed;
	public Transform checkLandNode;
	private CapsuleCollider col;
	private PlayableDirector director;

	public bool IsGround;


	Vector2 cacheInputDir;
	bool m_isRuning { get { return AnimCtrl.GetCurClipName().Equals("Run_ver_A"); } }
	bool m_animIsJumping { get { return AnimCtrl.GetCurClipName().Equals("Jump_Loop"); } }

	// Start is called before the first frame update
	void Start()
	{
		director = GetComponent<PlayableDirector>();
		col = GetComponent<CapsuleCollider>();
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
		AnimCtrl = new PlayableAnimCtrl();
		AnimCtrl.Init(animator, this);
	}

	private void OnDestroy()
	{
		AnimCtrl.Destroy();
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
			tarFace = h < 0 ? 180 : 0;
			if (transform.localEulerAngles.y != tarFace) { inRotate = true; }
			if (m_isRuning || !IsGround)
			{
				hSpeed = h > 0 ? runSpeed : -runSpeed;
			}

		}
		else
		{
			if (m_isRuning) { AnimCtrl.Play("Idle"); hSpeed = 0; }
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

		var dir = cacheInputDir;
		float zForce;
		if (dir.x == 0) { zForce = DirRight(transform.localEulerAngles.y) ? doadgeSpeed : -doadgeSpeed; }
		else { zForce = dir.x > 0 ? doadgeSpeed : -doadgeSpeed; }
		SetVelocity(zForce, 0);
		//rb.AddForce(0, 0, zForce);

		//Debug.Log($"Zforce:{zForce}");
	}

	PlayQueue[] jumpQue = new PlayQueue[]
	{
		new PlayQueue(){name ="Jump_Start" },
		new PlayQueue(){name ="Jump_Loop" },
	};
	void Jump()
	{
		AnimCtrl.PlayQueue(jumpQue);
		SetVelocity(rb.velocity.z, jumpSpeed);
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


	public override void OnFixUpdate(float deltaTime)
	{
		CheckGround();
		CheckFaceTo(deltaTime);
	}

	float rotateSpeed = 1500;
	float tarFace;
	bool inRotate;
	void CheckFaceTo(float deltaTime)
	{
		if (inRotate)
		{
			var curEu = transform.localEulerAngles;
			bool addORsub = tarFace > curEu.y;
			curEu.y += deltaTime * rotateSpeed * (addORsub ? 1 : -1);
			if ((curEu.y >= tarFace && addORsub) || (!addORsub && curEu.y <= tarFace))
			{
				curEu.y = tarFace;
				inRotate = false;
			}
			transform.localEulerAngles = curEu;
		}
	}

	void CheckGround()
	{
		if (Physics.CheckCapsule(checkLandNode.position, checkLandNode.position + Vector3.down * 0.01f, col.radius, LayerMask.GetMask("Plane")))
		{
			IsGround = true;
			if (CheckAnim("Jump_Loop")) { AnimCtrl.Play("Idle"); }
		}
		else
		{
			IsGround = false;
			if (CheckAnim("Idle")) { AnimCtrl.Play("Jump_Loop"); }
		}
	}

	#region ²Ù×÷½Ó¿Ú

	public override void OnMove(Vector2 dir)
	{
		cacheInputDir = dir;
		Move(dir.x, dir.y);
	}
	public override void OnAttackX()
	{
		AttackX();
	}
	public override void OnAttackY()
	{
		AttackY();
	}
	public override void OnKeyADown()
	{
		Jump();
	}

	public override void OnKeyBDown()
	{
		Doadge();
	}
	#endregion

	//private void OnDrawGizmos()
	//{
	//	Gizmos.DrawSphere(checkLandNode.position + Vector3.down * 0.01f, col.radius);
	//}
}

