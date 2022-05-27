using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//冲击力数据
public class ForceData
{
	public Vector3 forceDir;
	public float forcePower;
}

//伤害数据
public class DamageData
{
	public float damage;
}

//动作单元数据
public class ActData
{
	public ForceData forceData;
	public DamageData damageData;
}
public interface IAct
{
	ActData Data { get; }

}


//实体
public interface IEntity
{
	public PlayableAnimCtrl AnimCtrl { get; }
}


public class Entity : IEntity
{
	public PlayableAnimCtrl AnimCtrl { get; private set; }

}

