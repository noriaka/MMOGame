using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 副本选择界面
/// </summary>

public class DugeonWnd : WindowRoot
{
    public Button[] dgBtnArr;
    public Transform pointerTrans;
    private PlayerData pd;
    protected override void InitWnd()
    {
        base.InitWnd();
        pd = GameRoot.Instance.PlayerData;

        RefreshUI();
    }

    public void RefreshUI()
    {
        int dgid = pd.dugeon;
        for (int i = 0;i < dgBtnArr.Length;i++)
        {
            if (i < dgid % 10000)
            {
                SetActive(dgBtnArr[i].gameObject);
                if (i == dgid % 10000 - 1)
                {
                    pointerTrans.SetParent(dgBtnArr[i].transform);
                    pointerTrans.localPosition = new Vector3(5, 100, 0);
                    pointerTrans.localScale = Vector3.one;
                }
            }
            else
            {
                SetActive(dgBtnArr[i].gameObject, false);
            }
        }
    }

    public void ClickTaskBtn(int dugeon_id)
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        //检查体力是否足够
        int power = resSvc.GetMapCfgData(dugeon_id).power;
        if (power > pd.power)
        {
            GameRoot.AddTips("体力值不足");
        }
        else
        {
            netSvc.SendMsg(new GameMsg
            {
                cmd = (int)CMD.ReqDugeonFight,
                reqDugeonFight = new ReqDugeonFight
                {
                    dugeon_id = dugeon_id
                }
            });
        }
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}
