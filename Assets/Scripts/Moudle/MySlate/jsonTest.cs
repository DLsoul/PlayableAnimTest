using Slate;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jsonTest : MonoBehaviour
{
	AnimationDataCollection cut;
	// Start is called before the first frame update
	void Start()
	{
		//var cutscene = GameObject.Find("Cutscene").GetComponent<Cutscene>();
		//foreach (var group in cutscene.groups)
		//{
		//	foreach (var track in group.tracks)
		//	{
		//		foreach (var clip in track.clips)
		//		{
		//			if (clip.animationData != null) { cut = clip.animationData; break; }
		//		}
		//	}
		//}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
		}

		if (Input.GetKeyDown(KeyCode.L))
		{

		}
	}

	public void Save() { }
}
