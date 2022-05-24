using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IBaseBattleEntity
{
	BattleData BData { get; }
	void AttackOver();
	void BeAttacked(IBaseBattleEntity source);
	void AttackTarget(IBaseBattleEntity target);
}

public class BattleData
{
	public Transform transform;
}


