using UnityEngine;
/// <summary>
/// 玩家逻辑实体
/// </summary>
public class EntityPlayer : EntityBase
{
    public override Vector2 GetDirInput()
    {
        return battleMgr.GetDirInput();
    }
}
