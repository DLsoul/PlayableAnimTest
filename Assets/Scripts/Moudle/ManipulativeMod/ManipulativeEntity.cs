using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIndexItem
{
	int InnerIndex { get; set; }
}
public interface IManipulative : IIndexItem
{
	bool ControlVisible { get; set; }
	void OnMove(Vector2 dir);
	void OnAttackX();
	void OnAttackXHold(float time);
	void OnAttackXUp(float time);


	void OnAttackY();
	void OnAttackYHold(float time);
	void OnAttackYUp(float time);


	void OnKeyADown();
	void OnKeyAHold(float time);
	void OnKeyAUp(float time);

	void OnKeyBDown();
	void OnKeyBHold(float time);
	void OnKeyBUp(float time);

}


public class ManipulativeMonoEntity : MonoBehaviour, IManipulative, IBaseBattleEntity
{
	//可控状态
	public bool ControlVisible { get; set; } = true;
	public int InnerIndex { get; set; }

	public BattleData BData { get; private set; } = new BattleData();

	public virtual void OnMove(Vector2 dir) { }

	public virtual void OnAttackX() { }
	public virtual void OnAttackXHold(float time) { }
	public virtual void OnAttackXUp(float time) { }


	public virtual void OnAttackY() { }
	public virtual void OnAttackYHold(float time) { }
	public virtual void OnAttackYUp(float time) { }


	public virtual void OnKeyADown() { }
	public virtual void OnKeyAHold(float time) { }
	public virtual void OnKeyAUp(float time) { }

	public virtual void OnKeyBDown() { }
	public virtual void OnKeyBHold(float time) { }
	public virtual void OnKeyBUp(float time) { }


	//=====================baseEntity相关==================
	public virtual void BeAttacked(IBaseBattleEntity source) { }

	public virtual void AttackTarget(IBaseBattleEntity target) { }

	public virtual void AttackOver() { }
}

/// <summary>
/// 默认注册了控制
/// </summary>
public abstract class ManipulativeRegisterMonoEntity : ManipulativeMonoEntity, IUpdate, IFixUpdate
{
	//控制器数据
	public ManipulativeData mData = new ManipulativeData();
	public ManipulativeComponentCtrlData manipCtrlData = new ManipulativeComponentCtrlData();

	public virtual void InitOnAwake()
	{
		manipCtrlData.mono = this;
		manipCtrlData.transform = transform;
		manipCtrlData.rig = GetComponent<Rigidbody>();
		manipCtrlData.col = GetComponent<Collider>();

		BData.transform = transform;
	}
	private void Awake()
	{
		GlobalGameManager.Instance.OnUpdateDo += OnUpdate;
		GlobalGameManager.Instance.OnFixUpdateDo += OnFixUpdate;
		InitOnAwake();
	}

	private void OnDestroy()
	{
		GlobalGameManager.Instance.OnUpdateDo -= OnUpdate;
		GlobalGameManager.Instance.OnFixUpdateDo -= OnFixUpdate;
	}

	private void OnEnable()
	{
		RegisterInputListener();
	}

	private void OnDisable()
	{
		UnRegisterInputListener();
	}

	public void RegisterInputListener()
	{
		Facade.ManipulativeFacade.GotManipulativeMod.Invoke()?.AddManipulative(this);
	}

	public void UnRegisterInputListener()
	{
		Facade.ManipulativeFacade.GotManipulativeMod.Invoke()?.RemoveManipulative(this);
	}

	public virtual void OnUpdate(float deltaTime) { }

	public virtual void OnFixUpdate(float deltaTime)
	{

	}
}
