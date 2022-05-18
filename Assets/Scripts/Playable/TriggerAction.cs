using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ITriggerAlwaysDo { }
public abstract class TriggerAction
{
	public TriggerAction()
	{
		if (this is ITriggerAlwaysDo) { AlwaysDo = true; }
	}
	public bool AlwaysDo { get; private set; }
	public void Init(string str)
	{
		OnInit(str);
	}
	public virtual void OnInit(string str) { }
	public virtual void Invoke(IEntity source) { }
}

public class TriggerDamageStart : TriggerAction
{
	public override void Invoke(IEntity source)
	{
		base.Invoke(source);
		Debug.Log(this.GetType().Name);
	}
}
public class TriggerDamageEnd : TriggerAction, ITriggerAlwaysDo
{
	public override void Invoke(IEntity source)
	{
		base.Invoke(source);
		Debug.Log(this.GetType().Name);
	}
}
public class TriggerCanChangeState : TriggerAction
{
	public override void Invoke(IEntity source)
	{
		base.Invoke(source);
		Debug.Log(this.GetType().Name);

	}
}
