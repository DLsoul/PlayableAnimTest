using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TimerTest : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		float duration = 1f;
		int count = 3000000;
		Stopwatch watch = new Stopwatch();
		watch.Start();
		for (int i = 0; i < count; ++i)
		{
			Facade.TimerFacade.StartOnceTimer(duration, () => { }, false);
		}
		watch.Stop();
		UnityEngine.Debug.Log($"timeDeltatime�ܹ���ʱ {watch.ElapsedMilliseconds}");

		//watch.Restart();
		//for (int i = 0; i < count; ++i)
		//{
		//	StartCoroutine(Timer(duration));
		//}
		//watch.Stop();
		//UnityEngine.Debug.Log($"IEnumerator�ܹ���ʱ {watch.ElapsedMilliseconds}");
	}

	// Update is called once per frame
	void Update()
	{

	}

	IEnumerator Timer(float duration)
	{
		yield return new WaitForSeconds(duration);
	}
}
