﻿using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 配置数据类
/// </summary>

public class MonsterData : BaseData<MonsterData>
{
    public int mWave;   //批次
    public int mIndex;  //序号
    public MonsterCfg mCfg;
    public Vector3 mBornPos;
    public Vector3 mBornRote;
    public int mLevel;
}

public class MonsterCfg : BaseData<MonsterCfg>
{
    public string mName;
    public string resPath;
    public BattleProps bps;
}

public class SkillMoveCfg : BaseData<SkillMoveCfg>
{
    public int delayTime;
    public int moveTime;
    public float moveDis;
}

public class SkillActionCfg : BaseData<SkillActionCfg>
{
    public int delayTime;
    public float radius;    //伤害计算的范围
    public int angle;     //伤害有效角度
}

public class SkillCfg : BaseData<SkillCfg>
{
    public string skillName;
    public int cdTime;
    public int skillTime;
    public int aniAction;
    public string fx;
    public DamageType dmgType;
    public List<int> skillMoveLst;
    public List<int> skillActionLst;
    public List<int> skillDamageLst;
}

public class StrongCfg : BaseData<StrongCfg>
{
    public int pos;
    public int starlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;
}

public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
    public int npcID;   //触发任务目标NPC索引号
    public string dilogArr;
    public int actID;
    public int coin;
    public int exp;
}

public class MapCfg : BaseData<MapCfg>
{
    public string mapName;
    public string sceneName;
    public int power;
    public Vector3 mainCamPos;
    public Vector3 mainCamRote;
    public Vector3 playerBornPos;
    public Vector3 playerBornRote;
    public List<MonsterData> monsterLst;
}

public class TaskRewardCfg : BaseData<TaskRewardCfg>
{
    public string taskName;
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData : BaseData<TaskRewardData>
{
    public int prgs;
    public bool taked;
}

public class BaseData<T>
{
    public int ID;
}

public class BattleProps
{
    public int hp;
    public int ad;
    public int ap;
    public int addef;
    public int apdef;
    public int dodge;
    public int pierce;
    public int critical;
}
