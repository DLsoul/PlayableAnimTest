using UnityEngine;

public class ManipulativeData
{
	//================地面检测相关===============
	public float offsetDistance = 0.1f;
	public float rayGroundXScale = 0.8f;
	public int manipulativeLayer = 6;
	public bool isGround;


	//================跳跃相关==================
	public int jumpCountTally = 0;
	public float jumpFirstSpeed = 13;
	public float jumpHoldForce = 2;
	public float jumpUpMinSpeed = 3;
	public float jumpTime = 0.35f;
	public int jumpCount = 2;

	//================连击相关==================
	public bool m_inAttack;
	public float m_timeSinceAttack = 0;
	public int m_currentAttack = 0;

	//================移动相关==================
	public float moveSpeed = 10;

}
public class ManipulativeComponentCtrlData
{
	public MonoBehaviour mono;
	public Transform transform;
	public Rigidbody rig;
	public Collider col;
}