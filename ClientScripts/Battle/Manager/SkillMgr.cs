using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 技能管理器
/// </summary>
public class SkillMgr : MonoBehaviour
{
    private ResSvc resSvc;
    private TimerSvc timerSvc;
    public void Init()
    {
        resSvc = ResSvc.Instance;
        timerSvc = TimerSvc.Instance;
        PECommon.Log("Init SkillMgr Done.");
    }

    public void SkillAttack(EntityBase entity, int skillID)
    {
        AttackDamage(entity, skillID);
        AttackEffect(entity, skillID);
    }

    public void AttackDamage(EntityBase entity, int skillID)
    {
        SkillCfg skillData = resSvc.GetSkillCfg(skillID);
        List<int> actionLst = skillData.skillActionLst;
        int sum = 0;
        for (int i = 0; i < actionLst.Count; i++)
        {
            SkillActionCfg skillAction = resSvc.GetSkillActionCfg(actionLst[i]);
            sum += skillAction.delayTime;
            int index = i;
            if (sum > 0)
            {
                timerSvc.AddTimeTask((int tid) =>
                {
                    SkillAction(entity, skillData, index);
                }, sum);
            }
            else
            {
                //瞬时技能
                SkillAction(entity, skillData, index);
            }
        }
    }

    public void SkillAction(EntityBase caster, SkillCfg skillCfg, int index)
    {
        SkillActionCfg skillActionCfg = resSvc.GetSkillActionCfg(skillCfg.skillActionLst[index]);

        int damage = skillCfg.skillDamageLst[index];
        //获取场景里所有怪物的实体，遍历运算
        List<EntityMonster> monsterLst = caster.battleMgr.GetEntityMonsters();
        for (int i = 0; i < monsterLst.Count; i++)
        {
            EntityMonster target = monsterLst[i];
            //判断距离，判断角度
            if (InRange(caster.GetPos(), target.GetPos(), skillActionCfg.radius)
                && InAngle(caster.GetTrans(), target.GetPos(), skillActionCfg.angle))
            {
                //计算伤害
                CalcDamage(caster, target, skillCfg, damage);
            }
        }
    }

    System.Random rd = new System.Random();
    private void CalcDamage(EntityBase caster, EntityBase target, SkillCfg skillCfg, int damage)
    {
        int dmgSum = damage;
        if (skillCfg.dmgType == DamageType.AD)
        {
            //计算闪避
            int dodgeNum = PETools.RDInt(1, 100, rd);
            if (dodgeNum <= target.Props.dodge)
            {
                //UI显示闪避 TODO
                PECommon.Log("闪避Rate:" + dodgeNum + "/" + target.Props.dodge);
                target.SetDodge();
                return;
            }
            //计算属性加成
            dmgSum += caster.Props.ad;

            //计算暴击
            int criticalNum = PETools.RDInt(1, 100, rd);
            if (criticalNum <= caster.Props.critical)
            {
                float criticalRate = 1 + (PETools.RDInt(1, 100, rd) / 100.0f);
                dmgSum = (int)criticalRate * dmgSum;
                PECommon.Log("暴击Rate:" + criticalNum + "/" + target.Props.critical);
                target.SetCritical(dmgSum);
            }

            //计算穿甲
            int addef = (int)((1 - caster.Props.pierce / 100.0f) * target.Props.addef);
            dmgSum -= addef;
        }
        else if (skillCfg.dmgType == DamageType.AP)
        {
            //计算属性加成
            dmgSum += caster.Props.ap;

            //计算魔法抗性
            dmgSum -= target.Props.apdef;
        }
        else
        {
            //TOADD
        }

        //最终伤害
        if (dmgSum < 0)
        {
            dmgSum = 0;
            return;
        }
        target.SetHurt(dmgSum);

        if (target.HP < dmgSum)
        {
            target.HP = 0;
            //目标死亡
            target.Die();
            target.battleMgr.RmvMonster(target.Name);
        }
        else
        {
            target.HP -= dmgSum;
            target.Hit();
        }
    }

    private bool InRange(Vector3 from, Vector3 to, float range)
    {
        float dis = Vector3.Distance(from, to);
        if (dis <= range)
        {
            return true;
        }
        return false;
    }

    private bool InAngle(Transform trans, Vector3 to, float angle)
    {
        if (angle == 360)
        {
            return true;
        }
        else
        {
            Vector3 start = trans.forward;
            Vector3 dir = (to - trans.position).normalized;

            float ang = Vector3.Angle(start, dir);

            if (ang <= angle / 2) 
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 技能效果表现
    /// </summary>
    public void AttackEffect(EntityBase entity, int skillID)
    {
        SkillCfg skillData = resSvc.GetSkillCfg(skillID);

        entity.SetAction(skillData.aniAction);
        entity.SetFX(skillData.fx, skillData.skillTime);

        CalcSkillMove(entity, skillData);

        entity.canControl = false;
        entity.SetDir(Vector2.zero);
             
        timerSvc.AddTimeTask((int tid) =>
        {
            entity.Idle();
        }, skillData.skillTime);
    }

    public void CalcSkillMove(EntityBase entity, SkillCfg skillData)
    {
        List<int> skillMoveList = skillData.skillMoveLst;
        int sum = 0;
        for (int i = 0; i < skillMoveList.Count; i++)
        {
            SkillMoveCfg skillMoveCfg = resSvc.GetSkillMoveCfg(skillData.skillMoveLst[i]);
            float speed = skillMoveCfg.moveDis / (skillMoveCfg.moveTime / 1000f);
            sum += skillMoveCfg.delayTime;
            if (sum > 0)
            {
                timerSvc.AddTimeTask((int tid) => {
                    entity.SetSkillMoveState(true, speed);
                }, sum);
            }
            else
            {
                entity.SetSkillMoveState(true, speed);
            }

            sum += skillMoveCfg.moveTime;
            timerSvc.AddTimeTask((int tid) =>
            {
                entity.SetSkillMoveState(false);
            }, sum);
        }
    }
}