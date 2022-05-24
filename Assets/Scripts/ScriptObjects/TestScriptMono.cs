using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptMono : MonoBehaviour
{
	public TestScriptObjects testScript;
	// Start is called before the first frame update
	void Start()
	{
		Debug.Log($"testScript.testNum:  {testScript.testNum}");
	}

}
