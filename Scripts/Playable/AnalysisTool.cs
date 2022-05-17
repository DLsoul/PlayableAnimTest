using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AnalysisTool
{
	private static Dictionary<string, Type> animTriggerActionDic = new Dictionary<string, Type>() {
		{"DamageStart",typeof(TriggerDamageStart) },
		{"DamageEnd",typeof(TriggerDamageEnd) },
		{"CanChangeState",typeof(TriggerCanChangeState) },
	};

	public static TriggerAction GetTrigger(string action)
	{
		animTriggerActionDic.TryGetValue(action, out var type);
		if (type == null) { throw new ArgumentException($"error triggerAction name  :{action}"); }
		var ans = Activator.CreateInstance(type) as TriggerAction;
		ans.Init(action);
		return ans;
	}
}

