using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WallopData
{
	public Vector2 wallopDir;
	public float wallopForce;
}

public class BonusData
{
	public float addValue;
	public float MulValue = 1;
}

public class AttackData
{
	public float atkValue;
	public float defValue;
	public BonusData atkBonus = new BonusData();
	public BonusData defBonus = new BonusData();
}

public class BaseActData
{
	public IBaseBattleEntity belongBattleEntity;
	public WallopData wallop;
	public AttackData atkData;

}

public class BaseAct
{
	public BaseActData actData { get; private set; } = new BaseActData();
}

