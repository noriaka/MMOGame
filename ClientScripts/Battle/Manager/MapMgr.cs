using UnityEngine;
/// <summary>
/// 地图管理器
/// </summary>
public class MapMgr : MonoBehaviour
{
    private int waveIndex = 1;      //默认生成第一波怪物
    private BattleMgr battleMgr;
    public void Init(BattleMgr battle)
    {
        battleMgr = battle;

        //实例化第一批怪物
        battleMgr.LoadMonsterByWaveID(waveIndex);

        PECommon.Log("Init MapMgr Done.");
    }
}