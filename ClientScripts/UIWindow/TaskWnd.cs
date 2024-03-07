using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 任务奖励界面
/// </summary>

public class TaskWnd : WindowRoot
{
    public Transform scrollTrans;

    private PlayerData pd = null;
    private List<TaskRewardData> trdList = new List<TaskRewardData>();
    protected override void InitWnd()
    {
        base.InitWnd();

        pd = GameRoot.Instance.PlayerData;
        RefreshUI();
    }

    public void RefreshUI()
    {
        trdList.Clear();

        List<TaskRewardData> todoList = new List<TaskRewardData>();
        List<TaskRewardData> doneList = new List<TaskRewardData>();

        //1|0|0
        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            TaskRewardData trd = new TaskRewardData
            {
                ID = int.Parse(taskInfo[0]),
                prgs = int.Parse(taskInfo[1]),
                taked = taskInfo[2].Equals("1")
            };

            if (trd.taked)
            {
                doneList.Add(trd);
            }
            else
            {
                todoList.Add(trd);
            }    
        }

        trdList.AddRange(todoList);
        trdList.AddRange(doneList);

        for (int i = 0; i < scrollTrans.childCount; i++)
        {
            Destroy(scrollTrans.GetChild(i).gameObject);
        }

        for (int i = 0; i < trdList.Count; i++)
        {
            GameObject go = resSvc.LoadPrefab(PathDefine.TaskItemPrefab);
            go.transform.SetParent(scrollTrans); //此处需要设置为false，否则子物体会被放大
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.name = "taskItem_" + i;

            TaskRewardData trd = trdList[i];
            TaskRewardCfg trc = resSvc.GetTaskRewardCfg(trd.ID);

            SetText(GetTrans(go.transform, "txtName"), trc.taskName);
            SetText(GetTrans(go.transform, "txtPrg"), trd.prgs + "/" + trc.count);
            SetText(GetTrans(go.transform, "txtExp"), "奖励：     经验" + trc.exp);
            SetText(GetTrans(go.transform, "txtCoin"), "     金币" + trc.coin);
            Image imgPrg = GetTrans(go.transform, "prgBar/prgVal").GetComponent<Image>();
            float prgVal = trd.prgs * 1.0f / trc.count;
            imgPrg.fillAmount = prgVal;

            Button btnTake = GetTrans(go.transform, "btnTake").GetComponent<Button>();
            btnTake.onClick.AddListener(() =>
            {
                ClickTakeBtn(go.name);
            });

            Transform transComp = GetTrans(go.transform, "imgComp");
            if (trd.taked)
            {
                btnTake.interactable = false;
                SetActive(transComp);
            }
            else
            {
                SetActive(transComp, false);
                if (trd.prgs == trc.count)
                {
                    btnTake.interactable = true;
                }
                else
                {
                    btnTake.interactable = false;
                }
            }
        }
    }

    private void ClickTakeBtn(string name)
    {
        PECommon.Log("Name:" + name);

        string[] nameArr = name.Split('_');
        int index = int.Parse(nameArr[1]);
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ReqTakeTaskReward,
            reqTakeTaskReward = new ReqTakeTaskReward
            {
                rid = trdList[index].ID
            }
        };

        netSvc.SendMsg(msg);
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}
