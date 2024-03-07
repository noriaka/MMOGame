using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 主城业务系统
/// </summary>

public class MainCitySys : SystemRoot
{
    public static MainCitySys Instance = null;

    public MainCityWnd mainCityWnd;
    public InfoWnd infoWnd;
    public GuideWnd guideWnd;
    public StrongWnd strongWnd;
    public ChatWnd chatWnd;
    public BuyWnd buyWnd;
    public TaskWnd taskWnd;

    private PlayerController playerCtrl;
    private Transform charCamTrans;
    private AutoGuideCfg curTaskData;
    private Transform[] npcPosTrans;
    private NavMeshAgent nav;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init MainCitySys...");
    }

    public void EnterMainCity()
    {
        MapCfg mapData = resSvc.GetMapCfgData(Constants.MainCityMapID);
        resSvc.AsyncLoadScene(mapData.sceneName, () =>
        {
            PECommon.Log("Enter MainCity...");

            //加载游戏主角
            LoadPlayer(mapData);
            //打开主城场景UI
            mainCityWnd.SetWndState();

            //播放主城背景音乐
            audioSvc.PlayBGMusic(Constants.BGMainCity);

            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            MainCityMap mcm = map.GetComponent<MainCityMap>();
            npcPosTrans = mcm.NpcPosTrans;

            //设置人物展示相机
            if (charCamTrans != null)
            {
                charCamTrans.gameObject.SetActive(false);
            }
        });
    }

    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.CityPlayerPrefab, true);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        //相机初始化
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        nav = player.GetComponent<NavMeshAgent>();
    }

    public void SetMoveDir(Vector2 dir)
    {
        StopNavTask();

        if (dir == Vector2.zero)
        {
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
        else
        {
            playerCtrl.SetBlend(Constants.BlendMove);
        }
        playerCtrl.Dir = dir;
    }

    #region Enter DugeonSys
    public void EnterDugeon()
    {
        StopNavTask();
        DugeonSys.Instance.EnterDugeon();
    }
    #endregion

    #region TaksWnd
    public void OpenTaskRewardWnd()
    {
        StopNavTask();
        taskWnd.SetWndState();
    }
    public void RspTakeTaskReward(GameMsg msg)
    {
        RspTakeTaskReward data = msg.rspTakeTaskReward;
        GameRoot.Instance.SetPlayerDataByTask(data);
        taskWnd.RefreshUI();
        mainCityWnd.RefreshUI();
    }
    public void PushTaskPrgs(GameMsg msg)
    {
        PushTaskPrgs data = msg.pushTaskPrgs;
        GameRoot.Instance.SetPlayerDataByTaskPush(data);

        if (taskWnd.GetWndState())
        {
            taskWnd.RefreshUI();
        }
    }
    #endregion

    #region BuyWnd
    public void OpenBuyWnd(int type)
    {
        StopNavTask();
        buyWnd.SetBuyType(type);
        buyWnd.SetWndState();
    }
    public void RspBuy(GameMsg msg)
    {
        RspBuy rspBuy = msg.rspBuy;
        GameRoot.Instance.SetPlayerDataByBuy(rspBuy);
        GameRoot.AddTips("购买成功！");
        mainCityWnd.RefreshUI();
        buyWnd.SetWndState(false);

        if (msg.pushTaskPrgs != null)
        {
            GameRoot.Instance.SetPlayerDataByTaskPush(msg.pushTaskPrgs);
            if (taskWnd.GetWndState())
            {
                taskWnd.RefreshUI();
            }
        }
    }
    public void PushPower(GameMsg msg)
    {
        PushPower data = msg.pushPower;
        GameRoot.Instance.SetPlayerDataByPower(data);
        if (mainCityWnd.GetWndState())
        {
            mainCityWnd.RefreshUI();
        }
    }
    #endregion

    #region ChatWnd
    public void OpenChatWnd()
    {
        StopNavTask();
        chatWnd.SetWndState();
    }
    public void PushChat(GameMsg msg)
    {
        chatWnd.AddChatMsg(msg.pushChat.name, msg.pushChat.chat);
    }
    #endregion

    #region StrongWnd
    public void OpenStrongWnd()
    {
        StopNavTask();
        strongWnd.SetWndState();
    }

    public void RspStrong(GameMsg msg)
    {
        int fightPre = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
        GameRoot.Instance.SetPlayerDataByStrong(msg.rspStrong);
        int fightNow = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
        GameRoot.AddTips(Constants.Color("战力提升了" + (fightNow - fightPre), TxtColor.Red));
        strongWnd.UpdateUI();
        mainCityWnd.RefreshUI();
    }
    #endregion

    #region InfoWnd
    public void OpenInfoWnd()
    {
        StopNavTask();

        if (charCamTrans == null)
        {
            charCamTrans = GameObject.FindGameObjectWithTag("CharShowCam").transform;
        }

        //设置人物展示相机相对位置
        charCamTrans.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 2.3f + new Vector3(0, 2f, 0);
        charCamTrans.localEulerAngles = new Vector3(13, 180 + playerCtrl.transform.localEulerAngles.y, 0);
        charCamTrans.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        charCamTrans.gameObject.SetActive(true);

        infoWnd.SetWndState();
    }

    public void CloseInfoWnd()
    {
        if (charCamTrans != null)
        {
            charCamTrans.gameObject.SetActive(false);
            infoWnd.SetWndState(false);
        }
    }

    private float startRotate = 0;
    public void SetStartRotate()
    {
        startRotate = playerCtrl.transform.localEulerAngles.y;
    }

    public void SetPlayerRotate(float rotate)
    {
        playerCtrl.transform.localEulerAngles = new Vector3(0, startRotate + rotate, 0);
    }
    #endregion

    #region GuideWnd
    private bool isNavGuide = false;
    public void RunTask(AutoGuideCfg agc)
    {
        if (agc != null)
        {
            curTaskData = agc;
        }

        //解析任务数据
        nav.enabled = true;
        if (curTaskData.npcID != -1)
        {
            float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[agc.npcID].position);
            if (dis < 0.5f)
            {
                isNavGuide = false;
                nav.isStopped = true;
                playerCtrl.SetBlend(Constants.BlendIdle);
                nav.enabled = false;

                OpenGuideWnd();
            }
            else
            {
                isNavGuide = true;
                nav.enabled = true;
                nav.speed = Constants.PlayerMoveSpeed;
                nav.SetDestination(npcPosTrans[agc.npcID].position);
                playerCtrl.SetBlend(Constants.BlendMove);
            }
        }
        else
        {
            OpenGuideWnd();
        }
    }

    private void Update()
    {
        if (isNavGuide)
        {
            isArriveNavPos();
            playerCtrl.SetCam();
        }
    }

    private void isArriveNavPos()
    {
        float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[curTaskData.npcID].position);
        if (dis < 0.5f)
        {
            isNavGuide = false;
            nav.isStopped = true;
            playerCtrl.SetBlend(Constants.BlendIdle);
            nav.enabled = false;

            OpenGuideWnd();
        }
    }

    private void StopNavTask()
    {
        if (isNavGuide)
        {
            isNavGuide = false;

            nav.isStopped = true;
            nav.enabled = false;
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
    }

    private void OpenGuideWnd()
    {
        guideWnd.SetWndState();
    } 

    public AutoGuideCfg GetCurtTaskData()
    {
        return curTaskData;
    }

    public void RspGuide(GameMsg msg)
    {
        RspGuide data = msg.rspGuide;

        GameRoot.AddTips(Constants.Color("任务奖励 金币+" + curTaskData.coin + " 经验+" + curTaskData.exp, TxtColor.Blue));

        switch (curTaskData.actID)
        {
            case 0:
                //与智者对话
                break;
            case 1:
                //进入副本
                EnterDugeon();
                break;
            case 2:
                //进入强化界面
                OpenStrongWnd();
                break;
            case 3:
                //进行体力购买
                OpenBuyWnd(0);
                break;
            case 4:
                //进行金币铸造
                OpenBuyWnd(1);
                break;
            case 5:
                //进入世界聊天
                OpenChatWnd();
                break;
        }
        GameRoot.Instance.SetPlayerDataByGuide(data);
        mainCityWnd.RefreshUI();
    }
    #endregion
}
