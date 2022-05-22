using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloatExtensions
{
	public static class FloatExtension
	{
		public static bool IsEqual(this float baseF, float tarF, float margin)
		{
			if (margin == 0) { return IsEqual(baseF, tarF); }
			return Math.Abs(baseF - tarF) < margin;
		}
		public static bool IsEqual(this float baseF, float tarF)
		{
			return Math.Abs(baseF - tarF) < float.Epsilon;
		}
	}
}
