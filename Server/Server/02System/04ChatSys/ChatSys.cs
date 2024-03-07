using PEProtocol;
using System.Collections.Generic;
/// <summary>
/// 聊天业务系统
/// </summary>
public class ChatSys
{
    private static ChatSys instance = null;
    public static ChatSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ChatSys();
            }
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("ChatSys Init Done.");
    }

    public void SendChat(MsgPack pack)
    {
        SendChat data = pack.msg.sendChat;
        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

        //任务进度数据更新
        TaskSys.Instance.CalcTaskPrgs(pd, 6);

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.PushChat,
            pushChat = new PushChat
            {
                name = pd.name,
                chat = data.chat
            }
        };

        //广播所有在线客户端
        List<ServerSession> list_sessions = cacheSvc.GetOnlineServerSessions();
        byte[] bytes = PENet.PETool.PackNetMsg(msg);
        for (int i = 0; i < list_sessions.Count; i++)
        {
            list_sessions[i].SendMsg(bytes);
        }
    }
}

