using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����״���
/// </summary>

public class BuyWnd : WindowRoot
{
    public Text txtInfo;
    public Button btnSure;
    private int buyType;    //0��������1�����

    public void SetBuyType(int type)
    {
        buyType = type;
    }
    protected override void InitWnd()
    {
        base.InitWnd();
        btnSure.interactable = true;
        RefreshUI();
    }

    private void RefreshUI()
    {
        switch (buyType)
        {
            case 0:
                //����
                txtInfo.text = "�Ƿ񻨷�" + Constants.Color("10��ʯ", TxtColor.Red)
                    + "����" + Constants.Color("100����", TxtColor.Green) + "��";
                break;
            case 1:
                //���
                txtInfo.text = "�Ƿ񻨷�" + Constants.Color("10��ʯ", TxtColor.Red)
                    + "����" + Constants.Color("1000���", TxtColor.Green) + "��";
                break;

        }
    }

    public void ClickSureBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        //����������Ϣ
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ReqBuy,
            reqBuy = new ReqBuy
            {
                type = buyType,
                cost = 10
            }
        };

        netSvc.SendMsg(msg);
        btnSure.interactable = false;
    }
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}
