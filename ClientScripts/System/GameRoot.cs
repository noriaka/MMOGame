/****************************************************
    文件：GameRoot.cs
	作者：SIKI学院——Plane
    邮箱: 1785275942@qq.com
    日期：2018/12/3 5:30:21
	功能：游戏启动入口
*****************************************************/

using PEProtocol;
using UnityEngine;

public class GameRoot : MonoBehaviour {
    public static GameRoot Instance = null;

    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;

    private void Start() {
        Instance = this;
        DontDestroyOnLoad(this);
        PECommon.Log("Game Start...");

        ClearUIRoot();

        Init();
    }

    private void ClearUIRoot() {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++) {
            canvas.GetChild(i).gameObject.SetActive(false);
        }

    }

    private void Init() {
        //服务模块初始化
        NetSvc net = GetComponent<NetSvc>();
        net.InitSvc();
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();
        AudioSvc audio = GetComponent<AudioSvc>();
        audio.InitSvc();
        TimerSvc timer = GetComponent<TimerSvc>();
        timer.InitSvc();


        //业务系统初始化
        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();
        MainCitySys mainCitySys = GetComponent<MainCitySys>();
        mainCitySys.InitSys();
        DugeonSys dugeonSys = GetComponent<DugeonSys>();
        dugeonSys.InitSys();
        BattleSys battleSys = GetComponent<BattleSys>();
        battleSys.InitSys();

        dynamicWnd.SetWndState();
        //进入登录场景并加载相应UI
        login.EnterLogin();
    }

    public static void AddTips(string tips) {
        Instance.dynamicWnd.AddTips(tips);
    }

    private PlayerData playerData = null;
    public PlayerData PlayerData {
        get {
            return playerData;
        }
    }
    public void SetPlayerData(RspLogin data) {
        playerData = data.playerData;
    }

    public void SetPlayerName(string name) {
        PlayerData.name = name;
    }

    public void SetPlayerDataByGuide(RspGuide data)
    {
        PlayerData.coin = data.coin;
        PlayerData.lv = data.lv;
        PlayerData.exp = data.exp;
        PlayerData.guideid = data.guideid;
    }

    public void SetPlayerDataByStrong(RspStrong data)
    {
        PlayerData.coin = data.coin;
        PlayerData.crystal = data.crystal;
        PlayerData.hp = data.hp;
        PlayerData.ad = data.ad;
        PlayerData.ap = data.ap;
        PlayerData.addef = data.addef;
        PlayerData.apdef = data.apdef;

        PlayerData.strongArr = data.strongArr;
    }

    public void SetPlayerDataByBuy(RspBuy data)
    {
        PlayerData.coin = data.coin;
        PlayerData.power = data.power;
        PlayerData.diamond = data.diamond;
    }

    public void SetPlayerDataByPower(PushPower data)
    {
        PlayerData.power = data.power;
    }

    public void SetPlayerDataByTask(RspTakeTaskReward data)
    {
        PlayerData.coin = data.coin;
        PlayerData.lv = data.lv;
        PlayerData.exp = data.exp;
        PlayerData.taskArr = data.taskArr;
    }

    public void SetPlayerDataByTaskPush(PushTaskPrgs data)
    {
        PlayerData.taskArr = data.taskArr;
    }

    public void SetPlayerDataByDugeonStart(RspDugeonFight data)
    {
        PlayerData.power = data.power;
    }
}