using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facade
{
	public class TimerFacade
	{
		/// <summary>
		/// duration,callback,isGlobal,   return ITimer
		/// </summary>
		public static Func<float, Action, bool, ITimer> StartOnceTimer;

		public static Action<ITimer> RemoveTimer;
	}
}


