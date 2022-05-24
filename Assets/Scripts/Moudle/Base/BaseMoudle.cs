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
