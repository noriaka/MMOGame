﻿using UnityEditor.Animations;
using UnityEngine;
/// <summary>
/// 战斗业务系统
/// </summary>
public class BattleSys : SystemRoot
{
    public static BattleSys Instance = null;
    public PlayerCtrlWnd playerCtrlWnd;
    public BattleMgr battleMgr;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init BattleSys...");
    }

    public void StartBattle(int mapid)
    {
        GameObject go = new GameObject
        {
            name = "BattleRoot"
        };

        go.transform.SetParent(GameRoot.Instance.transform);
        battleMgr = go.AddComponent<BattleMgr>();

        battleMgr.Init(mapid);
        SetPlayerCtrlWndState();
    }

    public void SetPlayerCtrlWndState(bool isActive = true)
    {
        playerCtrlWnd.SetWndState(isActive);
    }

    public void SetMoveDir(Vector2 dir)
    {
        battleMgr.SetSelfPlayerMoveDir(dir);
    }

    public void ReqReleaseSkill(int index)
    {
        battleMgr.ReqReleaseSkill(index);
    }

    public Vector2 GetDirInput()
    {
        return playerCtrlWnd.currentDir;
    }
}