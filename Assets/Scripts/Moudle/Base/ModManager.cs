using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModManager : ILifeCycle
{
	List<IMoudles> AllMods { get; }
}
public class ModManager : IModManager
{
	public List<IMoudles> AllMods { get; private set; } = new List<IMoudles>()
	{
		new ManipulativeMod(),
		new TimerMod(),
	};

	public void Init(object _parames = null)
	{
		foreach (var mou in AllMods)
		{
			mou.Init(_parames);
		}
	}

	public void OnDestroy(object _params = null)
	{
		foreach (var mou in AllMods)
		{
			mou.OnDestroy(_params);
		}
	}

	public void OnHide(object _params = null)
	{
		foreach (var mou in AllMods)
		{
			mou.OnHide(_params);
		}
	}

	public void OnShow(object _params = null)
	{
		foreach (var mou in AllMods)
		{
			mou.OnShow(_params);
		}
	}
}
