/// <summary>
/// 死亡状态
/// </summary>
public class StateDie : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Die;
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        
    }

    public void Process(EntityBase entity, params object[] args)
    {
        entity.SetAction(Constants.ActionDie);
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.SetActive(false);
        }, Constants.DieAniLength);
    }
}
