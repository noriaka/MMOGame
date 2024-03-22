/****************************************************
// 网络通信协议（客户端服务端共用）
*****************************************************/

using System;
using PENet;

namespace PEProtocol {
    [Serializable]
    public class GameMsg : PEMsg {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;

        public ReqRename reqRename;
        public RspRename rspRename;

        public ReqGuide reqGuide;
        public RspGuide rspGuide;

        public ReqStrong reqStrong;
        public RspStrong rspStrong;

        public SendChat sendChat;
        public PushChat pushChat;

        public ReqBuy reqBuy;
        public RspBuy rspBuy;

        public PushPower pushPower;

        public ReqTakeTaskReward reqTakeTaskReward;
        public RspTakeTaskReward rspTakeTaskReward;

        public PushTaskPrgs pushTaskPrgs;

        public ReqDugeonFight reqDugeonFight;
        public RspDugeonFight rspDugeonFight;
    }

    #region 登录相关
    [Serializable]
    public class ReqLogin {
        public string acct;
        public string pass;
    }

    [Serializable]
    public class RspLogin {
        public PlayerData playerData;
    }

    [Serializable]
    public class PlayerData {
        public int id;
        public string name;
        public int lv;
        public int exp;
        public int power;
        public int coin;
        public int diamond;
        public int crystal;

        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int dodge;   //闪避概率
        public int pierce;   //穿透比率
        public int critical;  //暴击率  

        public int guideid;
        public int[] strongArr;

        public long time;
        public string[] taskArr;
        public int dugeon;
        //TOADD
    }

    [Serializable]
    public class ReqRename {
        public string name;
    }
    [Serializable]
    public class RspRename {
        public string name;
    }
    #endregion

    #region 引导相关
    [Serializable]
    public class ReqGuide
    {
        public int guideid;
    }

    [Serializable]
    public class RspGuide
    {
        public int guideid;
        public int coin;
        public int lv;
        public int exp;
    }

    #endregion

    #region 强化相关
    [Serializable]
    public class ReqStrong
    {
        public int pos;
    }
    [Serializable]
    public class RspStrong
    {
        public int coin;
        public int crystal;
        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int[] strongArr;
    }
    #endregion

    #region 聊天相关
    [Serializable]
    public class SendChat
    {
        public string chat;
    }
    [Serializable]
    public class PushChat
    {
        public string name;
        public string chat;
    }

    #endregion

    #region 资源交易相关
    [Serializable]
    public class ReqBuy
    {
        public int type;
        public int cost;
    }

    [Serializable]
    public class RspBuy
    {
        public int type;
        public int diamond;
        public int coin;
        public int power;
    }

    [Serializable]
    public class PushPower
    {
        public int power;
    }
    #endregion

    #region 任务奖励相关
    [Serializable]
    public class ReqTakeTaskReward
    {
        public int rid;
    }

    [Serializable]
    public class RspTakeTaskReward
    {
        public int coin;
        public int lv;
        public int exp;
        public string[] taskArr;
    }

    [Serializable]
    public class PushTaskPrgs
    {
        public string[] taskArr;
    }
    #endregion

    #region 副本战斗相关
    [Serializable]
    public class ReqDugeonFight
    {
        public int dugeon_id;
    }

    [Serializable]
    public class RspDugeonFight
    {
        public int dugeon_id;
        public int power;
    }
    #endregion

    public enum ErrorCode {
        None = 0,//没有错误
        ServerDataError,//服务器数据异常
        UpdateDBError,//更新数据库错误
        ClientDataError,//客户端数据异常

        AcctIsOnline,//账号已经上线
        WrongPass,//密码错误
        NameIsExist,//名字已经存在

        LackLevel,
        LackCoin,
        LackCrystal,
        LackDiamond,
        LackPower
    }

    public enum CMD {
        None = 0,
        //登录相关 100
        ReqLogin = 101,
        RspLogin = 102,

        ReqRename = 103,
        RspRename = 104,

        //主城相关200
        ReqGuide = 201,
        RspGuide = 202,

        ReqStrong = 203,
        RspStrong = 204,

        SendChat = 205,
        PushChat = 206,

        ReqBuy = 207,
        RspBuy = 208,

        PushPower = 209,

        ReqTakeTaskReward = 210,
        RspTakeTaskReward = 211,

        PushTaskPrgs = 212,

        ReqDugeonFight = 301,
        RspDugeonFight = 302,
    }

    public class SrvCfg {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}
