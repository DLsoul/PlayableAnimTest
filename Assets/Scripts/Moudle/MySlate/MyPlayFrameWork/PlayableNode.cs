using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slate
{
	public class PlayableNode
	{
		public string name;

		public float _startTime;

		//public AnimationDataCollection _animationData;
	}

	public class PlayableNode<T> : PlayableNode where T : PlayableNode
	{
		public List<T> _children = new List<T>();
	}

	public class Clip : PlayableNode
	{

	}

	public class Track : PlayableNode<Clip>
	{

	}

	public class Group : PlayableNode<Track>
	{

	}
}
