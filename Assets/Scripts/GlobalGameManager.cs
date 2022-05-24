using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
	public static GlobalGameManager Instance { get; private set; }
	public event Action<float> OnUpdateDo;
	public event Action<float> OnFixUpdateDo;
	public IModManager ModCtrl { get; set; } = new ModManager();
	private void Awake()
	{
		Instance = this;
		DontDestroyOnLoad(this);
		ModCtrl.Init();
	}

	private void OnEnable()
	{
		ModCtrl.OnShow();
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		OnUpdateDo?.Invoke(deltaTime);
	}
	private void FixedUpdate()
	{
		float deltaTime = Time.fixedDeltaTime;
		OnFixUpdateDo?.Invoke(deltaTime);
	}

	private void OnDisable()
	{
		ModCtrl.OnHide();
	}

	private void OnDestroy()
	{
		ModCtrl.OnDestroy();
	}
}
