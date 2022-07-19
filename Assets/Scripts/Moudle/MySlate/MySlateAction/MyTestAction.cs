using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slate;
using Slate.ActionClips;
using UnityEngine;

[Attachable(typeof(MyTestTrack), typeof(ActorActionTrack))]
public class MyTestAction : ActorActionClip<TestEntity>
{
	[AnimatableParameter]
	public int a;
	public float interval;//间隔
	private float tick;
	public override string info => "test my action";

	[SerializeField, HideInInspector]
	private float _length = 1f;
	public override float length { get => _length; set => _length = value; }

	protected override void OnCreate()
	{
		base.OnCreate();
		Debug.Log("my test create");
		
	}

	protected override void OnEditorValidate()
	{
		base.OnEditorValidate();
	}

	protected override bool OnInitialize()
	{
		return base.OnInitialize();
	}
	protected override void OnEnter()
	{
		base.OnEnter();
		Debug.Log("my test enter");
	}


	protected override void OnUpdate(float time, float previousTime)
	{
		base.OnUpdate(time, previousTime);
		float deltaTime = time - previousTime;
		tick += deltaTime;
		if (tick >= interval)
		{
			tick = 0;
			Debug.Log($"Test my action    update time:{time}    cur a:{a}");
		}
	}

	protected override void OnExit()
	{
		base.OnExit();
		Debug.Log("my test exit");
	}

	protected override void OnReverseEnter()
	{
		base.OnReverseEnter();
	}

	protected override void OnReverse()
	{
		base.OnReverse();
	}

}

