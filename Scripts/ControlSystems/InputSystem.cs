using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
	public static InputSystem Instance { get; private set; }
	public event Action<float, float> OnAxis;
	public event Action OnXKeyDown;
	public event Action OnYKeyDown;
	public event Action OnAKeyDown;
	public event Action OnBKeyDown;
	private void Awake()
	{
		Instance = this;
		DontDestroyOnLoad(this);
	}

	private void Update()
	{
		var hori = Input.GetAxis("Horizontal");
		var verti = Input.GetAxis("Vertical");
		OnAxis?.Invoke(hori, verti);

		if (Input.GetKeyDown(KeyCode.J)) { OnXKeyDown?.Invoke(); }
		if (Input.GetKeyDown(KeyCode.K)) { OnYKeyDown?.Invoke(); }
		if (Input.GetKeyDown(KeyCode.U)) { OnAKeyDown?.Invoke(); }
		if (Input.GetKeyDown(KeyCode.I)) { OnBKeyDown?.Invoke(); }
	}
}

