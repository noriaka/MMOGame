/****************************************************
// 缓存层
*****************************************************/

using System.Collections.Generic;
using PEProtocol;

public class CacheSvc {
    private static CacheSvc instance = null;
    public static CacheSvc Instance {
        get {
            if (instance == null) {
                instance = new CacheSvc();
            }
            return instance;
        }
    }
    private DBMgr dbMgr;

    private Dictionary<string, ServerSession> onLineAcctDic = new Dictionary<string, ServerSession>();
    private Dictionary<ServerSession, PlayerData> onLineSessionDic = new Dictionary<ServerSession, PlayerData>();

    public void Init() {
        dbMgr = DBMgr.Instance;
        PECommon.Log("CacheSvc Init Done.");
    }

    public bool IsAcctOnLine(string acct) {
        return onLineAcctDic.ContainsKey(acct);
    }

    /// <summary>
    /// 根据账号密码返回对应账号数据，密码错误返回null，账号不存在则默认创建新账号
    /// </summary>
    public PlayerData GetPlayerData(string acct, string pass) {
        return dbMgr.QueryPlayerData(acct, pass);
    }

    /// <summary>
    /// 账号上线，缓存数据
    /// </summary>
    public void AcctOnline(string acct, ServerSession session, PlayerData playerData) {
        onLineAcctDic.Add(acct, session);
        onLineSessionDic.Add(session, playerData);
    }

    public bool IsNameExist(string name) {
        return dbMgr.QueryNameData(name);
    }

    public List<ServerSession> GetOnlineServerSessions()
    {
        List<ServerSession> list_sessions = new List<ServerSession>();
        foreach (var item in onLineSessionDic)
        {
            list_sessions.Add(item.Key);
        }
        return list_sessions; 
    }

    public PlayerData GetPlayerDataBySession(ServerSession session) {
        if (onLineSessionDic.TryGetValue(session, out PlayerData playerData)) {
            return playerData;
        }
        else {
            return null;
        }
    }

    public Dictionary<ServerSession, PlayerData> GetOnlineCache()
    {
        return onLineSessionDic;
    }

    public ServerSession GetOnlineServerSession(int ID)
    {
        ServerSession session = null;
        foreach (var item in onLineSessionDic)
        {
            if (item.Value.id == ID)
            {
                session = item.Key;
                break;
            }
        }
        return session;
    }

    public bool UpdatePlayerData(int id, PlayerData playerData, ServerSession session) {
        //onLineSessionDic[session] = playerData;
        return dbMgr.UpdatePlayerData(id, playerData);
    }

    public void AcctOffLine(ServerSession session)
    {
        foreach (var item in onLineAcctDic)
        {
            if (item.Value == session)
            {
                onLineAcctDic.Remove(item.Key);
                break;
            }
        }

        bool succ = onLineSessionDic.Remove(session);
        PECommon.Log("Offline Result: SessionID:" + session.sessionID + " " + succ);
    }
}
