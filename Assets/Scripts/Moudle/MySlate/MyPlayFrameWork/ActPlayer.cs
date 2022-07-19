using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Slate
{
	[DisallowMultipleComponent]
	public class ActPlayer : MonoBehaviour
	{
		[SerializeField]
		public Dictionary<string, ActSequence> skillDic = new Dictionary<string, ActSequence>();

	}
}
