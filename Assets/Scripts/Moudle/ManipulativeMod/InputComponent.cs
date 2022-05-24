using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdate
{
	void OnUpdate(float deltaTime);
}
public interface IFixUpdate
{
	void OnFixUpdate(float deltaTime);
}


public class InputComponent : IUpdate, IFixUpdate
{
	public event Action<Vector2> Move;

	public event Action KeyADown;
	public event Action<float> KeyAHold;
	public event Action<float> KeyAUp;

	public event Action KeyBDown;
	public event Action<float> KeyBHold;
	public event Action<float> KeyBUp;

	public event Action AttackXDown;
	public event Action<float> AttackXHold;
	public event Action<float> AttackXUp;

	public event Action AttackYDown;
	public event Action<float> AttackYHold;
	public event Action<float> AttackYUp;

	public string keyX = "j";
	public string keyY = "u";
	public string keyA = "k";
	public string keyB = "i";
	public string keyLB = "q";
	public string keyRB = "e";
	public string keyLT = "l";
	public string keyRT = "o";

	public bool InputEnable { get; set; } = true;

	private Vector2 vec_inputDir;
	private float time_keyADown;
	private float time_keyBDown;
	private float time_atkXDown;
	private float time_atkYDown;

	private void SetHoldInputInfo(string key, Action acDown, Action<float> acUp, Action<float> acHold, ref float time, float deltaTime)
	{
		if (Input.GetKeyDown(key))
		{
			acDown?.Invoke();
			time = 0;
		}
		else if (Input.GetKeyUp(key))
		{
			acUp?.Invoke(time);
		}
		else if (Input.GetKey(key))
		{
			float addTime = deltaTime;
			time += addTime;
			acHold?.Invoke(time);
		}
	}

	public void OnUpdate(float deltaTime)
	{
		if (!InputEnable) { return; }
		SetHoldInputInfo(keyX, AttackXDown, AttackXUp, AttackXHold, ref time_atkXDown, deltaTime);
		SetHoldInputInfo(keyY, AttackYDown, AttackYUp, AttackYHold, ref time_atkYDown, deltaTime);
		SetHoldInputInfo(keyA, KeyADown, KeyAUp, KeyAHold, ref time_keyADown, deltaTime);
		SetHoldInputInfo(keyB, KeyBDown, KeyBUp, KeyBHold, ref time_keyBDown, deltaTime);
	}

	public void OnFixUpdate(float deltaTime)
	{
		if (!InputEnable) { return; }
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		vec_inputDir = new Vector2(horizontal, vertical);
		Move?.Invoke(vec_inputDir);
	}
}
