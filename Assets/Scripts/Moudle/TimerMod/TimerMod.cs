using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerMod : BaseMoudle
{
	private ItemContainer<ITimer> globalTimers = new ItemContainer<ITimer>();
	private ItemContainer<ITimer> sceneTimers = new ItemContainer<ITimer>();
	public override void Init(object _parames = null)
	{
		base.Init(_parames);
		GlobalGameManager.Instance.OnUpdateDo += OnUpdateDo;
		SceneManager.sceneUnloaded += OnSceneUnLoaded;
		Facade.TimerFacade.StartOnceTimer += StartOnceTimer;
		Facade.TimerFacade.RemoveTimer += RemoveTimer;
	}

	private void OnSceneUnLoaded(Scene scene)
	{
		foreach (var item in sceneTimers)
		{
			item.Kill();
		}
	}

	private void OnUpdateDo(float deltaTime)
	{
		foreach (var item in globalTimers)
		{
			item.OnUpdate(deltaTime);
		}

		foreach (var item in sceneTimers)
		{
			item.OnUpdate(deltaTime);
		}
	}

	public ITimer StartOnceTimer(float duration, Action callBack, bool isGlobal = false)
	{
		ITimer timer = new Timer(duration, callBack, isGlobal);
		if (isGlobal)
		{
			globalTimers.Add(timer);
		}
		else
		{
			sceneTimers.Add(timer);
		}
		timer.Start();
		return timer;
	}

	public void RemoveTimer(ITimer timer)
	{
		if (timer.IsGlobal)
		{
			globalTimers.Remove(timer);
		}
		else
		{
			sceneTimers.Remove(timer);
		}
	}
}

