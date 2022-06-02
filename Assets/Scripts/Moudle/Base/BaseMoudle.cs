using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILifeCycle
{
	void Init(object _parames = null);
	void OnShow(object _params = null);
	void OnHide(object _params = null);
	void OnDestroy(object _params = null);
}

public interface IMoudles : ILifeCycle
{

}
public class BaseMoudle : IMoudles
{
	public BaseMoudle()
	{
		if (this is IUpdate update) { GlobalGameManager.Instance.OnUpdateDo += update.OnUpdate; }
		if (this is IFixUpdate fixupdate) { GlobalGameManager.Instance.OnFixUpdateDo += fixupdate.OnFixUpdate; }
	}

	public virtual void Init(object _parames = null)
	{

	}

	public virtual void OnDestroy(object _params = null)
	{

	}

	public virtual void OnHide(object _params = null)
	{

	}

	public virtual void OnShow(object _params = null)
	{

	}
}
