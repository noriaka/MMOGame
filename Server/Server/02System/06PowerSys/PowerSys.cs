using PEProtocol;
using System.Collections.Generic;
/// <summary>
/// 体力恢复系统
/// </summary>
public class PowerSys
{
    private static PowerSys instance = null;
    public static PowerSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PowerSys();
            }
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;
    private TimerSvc timerSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        timerSvc = TimerSvc.Instance;

        TimerSvc.Instance.AddTimeTask(CalcPowerAdd, PECommon.PowerAddSpace, PETimeUnit.Second, 0);
        PECommon.Log("PowerSys Init Done.");
    }

    private void CalcPowerAdd(int tid)
    {
        //计算体力增长
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.PushPower
        };
        msg.pushPower = new PushPower();

        //所有在线玩家获得实时体力增长推送数据
        Dictionary<ServerSession, PlayerData> onlineDic = cacheSvc.GetOnlineCache();
        foreach (var item in onlineDic)
        {
            PlayerData pd = item.Value;
            ServerSession session = item.Key;

            int powerMax = PECommon.GetPowerLimit(pd.lv);
            if (pd.power >= powerMax)
            {
                continue;
            }
            else
            {
                pd.power += PECommon.PowerAddCount;
                pd.time = timerSvc.GetNowTime();
                if (pd.power > powerMax)
                {
                    pd.power = powerMax;
                }
            }
            if (!cacheSvc.UpdatePlayerData(pd.id, pd, session))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.pushPower.power = pd.power;
                session.SendMsg(msg);
            }    
        }
    }
}
