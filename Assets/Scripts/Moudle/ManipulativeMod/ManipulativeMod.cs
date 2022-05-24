using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManipulativeMod
{
	bool ControlVisible { get; set; }
	List<IManipulative> AllManiEntitys { get; }
	void AddManipulative(IManipulative _mani);
	void AddManipulative(List<IManipulative> _manis);
	void RemoveManipulative(IManipulative _mani);
	void RemoveManipulative(List<IManipulative> _manis);
}

public class ManipulativeMod : BaseMoudle, IManipulativeMod
{
	private InputComponent inputComponent = new InputComponent();

	//¿É¿Ø×´Ì¬
	public bool ControlVisible { get; set; } = true;

	//²Ù¿ØÁÐ±í
	public List<IManipulative> AllManiEntitys { get; private set; } = new List<IManipulative>();
	private Queue<IManipulative> needRemoveEntitys = new Queue<IManipulative>();
	private Queue<IManipulative> needAddEntitys = new Queue<IManipulative>();
	private bool inDispatching;
	public override void Init(object _parames = null)
	{
		base.Init(_parames);
		Facade.ManipulativeFacade.GotManipulativeMod += GetManipulativeMod;
		GlobalGameManager.Instance.OnUpdateDo += inputComponent.OnUpdate;
		GlobalGameManager.Instance.OnFixUpdateDo += inputComponent.OnFixUpdate;
		RegisterInputListener();
	}

	public void AddManipulative(IManipulative _mani)
	{
		if (inDispatching) { needAddEntitys.Enqueue(_mani); }
		else { AllManiEntitys.Add(_mani); }
	}

	public void RemoveManipulative(IManipulative _mani)
	{
		if (inDispatching) { needRemoveEntitys.Enqueue(_mani); }
		else { AllManiEntitys.Remove(_mani); }
	}

	public void AddManipulative(List<IManipulative> _manis)
	{
		foreach (var item in _manis)
		{
			AddManipulative(item);
		}
	}

	public void RemoveManipulative(List<IManipulative> _manis)
	{
		foreach (var item in _manis)
		{
			RemoveManipulative(item);
		}
	}
	public IManipulativeMod GetManipulativeMod()
	{
		return this;
	}

	public void RegisterInputListener()
	{
		inputComponent.Move += Move;

		inputComponent.AttackXDown += AttackX;
		inputComponent.AttackXHold += AttackXHold;
		inputComponent.AttackXUp += AttackXUp;

		inputComponent.AttackYDown += AttackY;
		inputComponent.AttackYHold += AttackYHold;
		inputComponent.AttackYUp += AttackYUp;

		inputComponent.KeyADown += JumpDown;
		inputComponent.KeyAUp += JumpUp;
		inputComponent.KeyAHold += JumpHold;

		inputComponent.KeyBDown += KeyBDown;
		inputComponent.KeyBUp += KeyBUp;
		inputComponent.KeyBHold += KeyBHold;
	}

	public void UnRegisterInputListener()
	{
		inputComponent.Move -= Move;
		inputComponent.AttackXDown -= AttackX;
		inputComponent.AttackXHold -= AttackXHold;
		inputComponent.AttackXUp -= AttackXUp;

		inputComponent.AttackYDown -= AttackY;
		inputComponent.AttackYHold -= AttackYHold;
		inputComponent.AttackYUp -= AttackYUp;

		inputComponent.KeyADown -= JumpDown;
		inputComponent.KeyAUp -= JumpUp;
		inputComponent.KeyAHold -= JumpHold;

		inputComponent.KeyBDown -= KeyBDown;
		inputComponent.KeyBUp -= KeyBUp;
		inputComponent.KeyBHold -= KeyBHold;
	}

	private void DoAc(Action<IManipulative> ac)
	{
		if (!ControlVisible) { return; }
		inDispatching = true;
		foreach (var en in AllManiEntitys)
		{
			if (!en.ControlVisible) { continue; }
			ac?.Invoke(en);
		}
		while (needRemoveEntitys.Count > 0)
		{
			var remove = needRemoveEntitys.Dequeue();
			AllManiEntitys.Remove(remove);
		}
		while (needAddEntitys.Count > 0)
		{
			var add = needAddEntitys.Dequeue();
			if (add == null) { continue; }
			AllManiEntitys.Add(add);
		}
		inDispatching = false;
	}



	private void Move(Vector2 dir)
	{
		DoAc(manti => { manti?.OnMove(dir); });
	}

	//========================  X  =======================
	private void AttackX()
	{
		DoAc(manti => { manti?.OnAttackX(); });
	}
	private void AttackXHold(float time)
	{
		DoAc(manti => { manti?.OnAttackXHold(time); });
	}

	private void AttackXUp(float time)
	{
		DoAc(manti => { manti?.OnAttackXUp(time); });
	}

	//========================  Y  =======================
	private void AttackY()
	{
		DoAc(manti => { manti?.OnAttackY(); });
	}

	private void AttackYHold(float time)
	{
		DoAc(manti => { manti?.OnAttackYHold(time); });
	}

	private void AttackYUp(float time)
	{
		DoAc(manti => { manti?.OnAttackYUp(time); });

	}


	//========================  JUMP  =======================
	private void JumpDown()
	{
		DoAc(manti => { manti?.OnKeyADown(); });
	}
	private void JumpHold(float time)
	{
		DoAc(manti => { manti?.OnKeyAHold(time); });
	}
	private void JumpUp(float time)
	{
		DoAc(manti => { manti?.OnKeyAUp(time); });
	}

	//========================  B  =======================
	private void KeyBDown()
	{
		DoAc(manti => { manti?.OnKeyBDown(); });
	}
	private void KeyBHold(float time)
	{
		DoAc(manti => { manti?.OnKeyBHold(time); });
	}
	private void KeyBUp(float time)
	{
		DoAc(manti => { manti?.OnKeyBUp(time); });
	}
}
