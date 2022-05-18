using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Test : MonoBehaviour
{
	public Animator animator;
	public AnimationClip[] clips;

	AnimationClipPlayable lastClip;
	AnimationClipPlayable curClip;
	AnimationMixerPlayable mixer;
	PlayableGraph graph;
	AnimationPlayableOutput output;
	// Start is called before the first frame update
	void Start()
	{
		graph = PlayableGraph.Create("TestGra");
		output = AnimationPlayableOutput.Create(graph, "testOut", animator);
		mixer = AnimationMixerPlayable.Create(graph);
		output.SetSourcePlayable(mixer);

		var clips = animator.runtimeAnimatorController.animationClips;
		mixer.SetInputCount(clips.Length);
		for (int i = 0; i < clips.Length; ++i)
		{
			var curClip = clips[i];
			var p = AnimationClipPlayable.Create(graph, curClip);
			graph.Connect(p, 0, mixer, i);
		}
		lastClip = (AnimationClipPlayable)mixer.GetInput(0);
		curClip = (AnimationClipPlayable)mixer.GetInput(1);

		mixer.SetInputWeight(lastClip, 1 - curClipWeight);
		mixer.SetInputWeight(curClip, curClipWeight);

		graph.Play();
	}

	float curClipWeight = 1;
	bool inFading;
	public float fadeSpeed = 10f;

	int curIndex = 0;
	// Update is called once per frame
	void Update()
	{
		if (inFading)
		{
			curClipWeight += Time.deltaTime * fadeSpeed;
			if (curClipWeight >= 1) { curClipWeight = 1; inFading = false; }
			mixer.SetInputWeight(lastClip, 1 - curClipWeight);
			mixer.SetInputWeight(curClip, curClipWeight);
		}

		if (Input.GetKeyDown(KeyCode.X))
		{
			int count = mixer.GetInputCount();
			curIndex = (curIndex + count - 1) % count;
			ChangeClip(curIndex);
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			curIndex = (curIndex + 1) % mixer.GetInputCount();
			ChangeClip(curIndex);
		}
	}
	void ChangeClip(int curIndex)
	{
		mixer.SetInputWeight(lastClip, 0);
		lastClip = curClip;
		curClip = (AnimationClipPlayable)mixer.GetInput(curIndex);
		curClipWeight = 1 - curClipWeight;
		curClip.SetTime(0);
		inFading = true;

		Debug.Log($"cur: {curClip.GetAnimationClip().name}");
	}
	public void TestFrameEvent()
	{
		//Debug.Log("TestFrameEvent");
	}

}
