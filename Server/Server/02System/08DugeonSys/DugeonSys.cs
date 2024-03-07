using PEProtocol;
/// <summary>
/// 副本战斗业务系统
/// </summary>
public class DugeonSys
{
    private static DugeonSys instance = null;
    public static DugeonSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DugeonSys();
            }
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;
    private CfgSvc cfgSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgSvc.Instance;
        PECommon.Log("DugeonSys Init Done.");
    }

    public void ReqDugeonFight(MsgPack pack)
    {
        ReqDugeonFight data = pack.msg.reqDugeonFight;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspDugeonFight
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        int power = cfgSvc.GetMapCfg(data.dugeon_id).power;

        if (pd.dugeon < data.dugeon_id)
        {
            msg.err = (int)ErrorCode.ClientDataError;
        }
        else if (pd.power < power)
        {
            msg.err = (int)ErrorCode.LackPower;
        }
        else
        {
            pd.power -= power;
            if (cacheSvc.UpdatePlayerData(pd.id, pd, pack.session))
            {
                RspDugeonFight rspDugeonFight = new RspDugeonFight
                {
                    dugeon_id = data.dugeon_id,
                    power = pd.power
                };
                msg.rspDugeonFight = rspDugeonFight;
            }
            else
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
        }
        pack.session.SendMsg(msg);
    }
}