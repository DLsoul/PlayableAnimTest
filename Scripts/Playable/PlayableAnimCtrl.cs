using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;


public class FrameTriggerData
{
	public float time;
	public string action;
	public bool enable;
	public TriggerAction CacheAction { get; set; }
	public void Init(IEntity source)
	{
		CacheAction = AnalysisTool.GetTrigger(action);
	}
}

public class AnimStateTransition
{
	public string target;
}

public class AnimState
{
	public IEntity baseEntity;
	public List<FrameTriggerData> triggers;
	public AnimStateTransition transition;
	public AnimationClipPlayable playableClip;
	private float curtick;

	private AnimationClip animClip;
	public AnimationClip AnimClip { get { if (animClip == null) { animClip = playableClip.GetAnimationClip(); } return animClip; } }

	void ResetTrigger()
	{
		if (triggers != null)
		{
			foreach (var item in triggers)
			{
				item.enable = true;
			}
		}
	}

	void TriggerAlwaysDo()
	{
		if (triggers != null)
		{
			foreach (var item in triggers)
			{
				if (item.enable && item.CacheAction.AlwaysDo) { item.CacheAction.Invoke(baseEntity); }
			}
		}
	}

	public void OnStateEnter()
	{
		ResetTrigger();

		curtick = 0;
	}

	public void OnStateExit()
	{
		TriggerAlwaysDo();
	}

	public void OnUpdate(Playable playable, FrameData info)
	{
		curtick += info.deltaTime;
		if (triggers != null)
		{
			foreach (var item in triggers)
			{
				if (!item.enable || curtick < item.time) { continue; }

				//to do triggerAction
				//UnityEngine.Debug.Log($"curtick:{curtick}   ac:{item.action}");
				item.CacheAction?.Invoke(baseEntity);

				item.enable = false;
			}
		}

		//Ä¬ÈÏÇÐ»»
		if (curtick > AnimClip.length)
		{
			if (!AnimClip.isLooping)
			{
				baseEntity.AnimCtrl.Play(transition?.target ?? null);
			}
			else { ResetTrigger(); }
		}

	}
}

public class AnimStateMachine
{
	public Dictionary<string, AnimState> clipDic = new Dictionary<string, AnimState>();
	public AnimState lastState;
	public AnimState currentState;
	private string defaultState;
	public void SetDefaultState(string name)
	{
		defaultState = name;
		ChangeState(name);
		lastState = currentState;
	}

	public void AddState(AnimState _state)
	{
		AnimationClip clip = _state.playableClip.GetAnimationClip();
		if (clipDic.ContainsKey(clip.name)) { return; }
		clipDic.Add(clip.name, _state);
	}

	public void ChangeState(string key)
	{
		if (string.IsNullOrEmpty(key)) { key = defaultState; }
		if (!clipDic.ContainsKey(key))
		{
			return;
		}
		lastState = currentState;
		lastState?.OnStateExit();
		currentState = clipDic[key];
		currentState.OnStateEnter();
	}

}

public class AnimBehaviour : PlayableBehaviour
{
	private Playable m_playable;
	public Playable playable { get { return m_playable; } }
	private AnimationMixerPlayable mixer;
	public IEntity baseEntity;
	public AnimStateMachine stateMachine = new AnimStateMachine();

	//==== anim blend params =====
	float curClipWeight = 1;
	bool inFading;
	float fadeSpeed = 10f;
	//======================
	public override void OnPlayableCreate(Playable playable)
	{
		base.OnPlayableCreate(playable);
		m_playable = playable;
		var graph = m_playable.GetGraph();
		mixer = AnimationMixerPlayable.Create(graph);
		graph.Connect(mixer, 0, m_playable, 0);

	}

	public override void PrepareFrame(Playable playable, FrameData info)
	{
		base.PrepareFrame(playable, info);
		UpdateBlendAnim();
		stateMachine.currentState?.OnUpdate(playable, info);
	}

	public void Init(IEntity _entity, AnimationClip[] clips)
	{
		baseEntity = _entity;
		for (int i = 0; i < clips.Length; i++)
		{
			AddClip(clips[i]);
		}
		stateMachine.SetDefaultState("Idle");
	}

	public void AddClip(AnimationClip clip)
	{
		var clipPlayable = AnimationClipPlayable.Create(playable.GetGraph(), clip);
		mixer.AddInput(clipPlayable, 0);
		AnimState state = new AnimState();
		state.baseEntity = baseEntity;

		//set playable
		state.playableClip = clipPlayable;

		//set triggers
		var triggers = new List<FrameTriggerData>();
		state.triggers = triggers;

		//link to stateMachine
		stateMachine.AddState(state);
	}

	public void Play(string _name = null, float startTime = 0)
	{
		mixer.SetInputWeight(stateMachine.lastState.playableClip, 0);
		stateMachine.ChangeState(_name);
		curClipWeight = 1 - curClipWeight;
		stateMachine.currentState.playableClip.SetTime(startTime);
		inFading = true;

		Debug.Log($"cur: {  stateMachine.currentState.AnimClip.name}");
	}

	private void UpdateBlendAnim()
	{
		if (inFading)
		{
			curClipWeight += Time.deltaTime * fadeSpeed;
			if (curClipWeight >= 1) { curClipWeight = 1; inFading = false; }
			mixer.SetInputWeight(stateMachine.lastState.playableClip, 1 - curClipWeight);
			mixer.SetInputWeight(stateMachine.currentState.playableClip, curClipWeight);
		}
	}
}

public class PlayableAnimCtrl
{
	public Animator animator;
	public PlayableGraph graph;
	private AnimBehaviour behaviour;
	public IEntity BaseEntity { get; set; }
	public void Init(Animator _animator, IEntity _entity)
	{
		BaseEntity = _entity;
		animator = _animator;
		graph = PlayableGraph.Create();
		var template = new AnimBehaviour();
		var behaviourPlayable = ScriptPlayable<AnimBehaviour>.Create(graph, template, 1);
		behaviour = behaviourPlayable.GetBehaviour();
		behaviour.Init(BaseEntity, animator.runtimeAnimatorController.animationClips);
		AnimationPlayableUtilities.Play(animator, behaviour.playable, graph);
	}

	public void Play(string _name = null, float _startTime = 0)
	{
		behaviour.Play(_name, _startTime);
	}

	public AnimState GetCurState()
	{
		return behaviour.stateMachine.currentState;
	}

	public string GetCurClipName()
	{
		return GetCurState().AnimClip.name;
	}
}
