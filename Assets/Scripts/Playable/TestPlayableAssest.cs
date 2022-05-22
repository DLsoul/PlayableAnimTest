using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TestPlayableAssest : PlayableAsset
{
	public int de = 400;
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		var playable = ScriptPlayable<TestLearnBehavior>.Create(graph);

		var behaviour = playable.GetBehaviour();
		behaviour.debugTick = de;
		return playable;
	}

}


public class TestLearnBehavior : PlayableBehaviour
{
	int frame = 0;
	public int debugTick = 500;
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		base.ProcessFrame(playable, info, playerData);
		frame++;
		if (frame >= debugTick) { Debug.Log($"tick: {DateTime.Now.Millisecond}"); frame = 0; }
	}
}
