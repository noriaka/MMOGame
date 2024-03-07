using PEProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������洰��
/// </summary>

public class ChatWnd : WindowRoot
{
    public InputField iptChat;
    public Text txtChat;
    public Image imgWorld;
    public Image imgGuild;
    public Image imgFriend;

    private int chatType;
    private List<string> chatList = new List<string>();

    protected override void InitWnd()
    {
        base.InitWnd();
        chatType = 0;
        RefreshUI();

    }

    public void AddChatMsg(string name, string chat)
    {
        chatList.Add(Constants.Color(name + "��", TxtColor.Blue) + chat);
        if (chatList.Count > 8)
        {
            chatList.RemoveAt(0);
        }
        if (GetWndState())
        {
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (chatType == 0)
        {
            string chatMsg = "";
            for (int i = 0; i < chatList.Count; i++)
            {
                chatMsg += chatList[i] + "\n";
            }
            SetText(txtChat, chatMsg);

            SetSprite(imgWorld, "ResImages/btntype1");
            SetSprite(imgGuild, "ResImages/btntype2");
            SetSprite(imgFriend, "ResImages/btntype2");
        }
        else if (chatType == 1)
        {
            SetText(txtChat, "��δ���빫��");
            SetSprite(imgWorld, "ResImages/btntype2");
            SetSprite(imgGuild, "ResImages/btntype1");
            SetSprite(imgFriend, "ResImages/btntype2");
        }
        else if (chatType == 2) 
        {
            SetText(txtChat, "���޺�����Ϣ");
            SetSprite(imgWorld, "ResImages/btntype2");
            SetSprite(imgGuild, "ResImages/btntype2");
            SetSprite(imgFriend, "ResImages/btntype1");
        }
    }

    private bool canSend = true;
    public void ClickSendBtn()
    {
        if (!canSend)
        {
            GameRoot.AddTips("������Ϣÿ5����ܷ���һ��");
            return;
        }

        if (iptChat.text != null && iptChat.text != "" && iptChat.text != " ")
        {
            if (iptChat.text.Length > 12) 
            {
                GameRoot.AddTips("������Ϣ���ܳ���12���ַ�");
            }
            else
            {
                //����������Ϣ��������
                GameMsg msg = new GameMsg
                {
                    cmd = (int)CMD.SendChat,
                    sendChat = new SendChat
                    {
                        chat = iptChat.text
                    }
                };
                iptChat.text = "";
                netSvc.SendMsg(msg);
                canSend = false;

                timerSvc.AddTimeTask((int tid) =>
                {
                    canSend = true;
                }, 5, PETimeUnit.Second);
            }
        }
        else
        {
            GameRoot.AddTips("��δ����������Ϣ");
        }
    }
    public void ClickWorldBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        RefreshUI();
    }
    public void ClickGuildBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 1;
        RefreshUI();
    }
    public void ClickFriendBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 2;
        RefreshUI();
    }
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType= 0;
        SetWndState(false);
    }
}
