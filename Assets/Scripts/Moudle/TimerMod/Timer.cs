using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ITimer : IIndexItem
{
	bool IsGlobal { get; }
	void Start();
	void Kill();
	void OnUpdate(float deltaTimer);
}

public class Timer : ITimer
{
	public int InnerIndex { get; set; }
	public bool IsGlobal { get; set; }
	private float tick;
	private float duration;
	private bool run;
	private bool once = true;
	private Action callBack;

	public Timer(float duration, Action callBack, bool isGlobal = false)
	{
		this.duration = duration;
		this.callBack = callBack;
		this.IsGlobal = isGlobal;
	}

	public void Kill()
	{
		Facade.TimerFacade.RemoveTimer(this);
	}

	public void OnUpdate(float deltaTime)
	{
		if (!run) { return; }
		tick += deltaTime;
		if (tick >= duration)
		{
			callBack?.Invoke();
			if (once) { Kill(); }
		}
	}

	public void Start()
	{
		run = true;
	}

}

