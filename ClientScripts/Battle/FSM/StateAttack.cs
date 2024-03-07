using System;
/// <summary>
/// 攻击状态
/// </summary>
public class StateAttack : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Attack;
        //PECommon.Log("Enter StateAttack.");
    }
    public void Exit(EntityBase entity, params object[] args)
    {
        entity.canControl = true;
        entity.SetAction(Constants.ActionDefault);
        //PECommon.Log("Exit StateAttack.");
    }

    public void Process(EntityBase entity, params object[] args)
    {
        //技能伤害计算
        //技能效果表现
        entity.SkillAttack((int)args[0]);
        //PECommon.Log("Process StateAttack.");
    }
}