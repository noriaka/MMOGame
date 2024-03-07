/****************************************************
    文件：DynamicWnd.cs
	作者：SIKI学院——Plane
    邮箱: 1785275942@qq.com
    日期：2018/12/4 6:33:31
	功能：动态UI元素界面
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWnd : WindowRoot {
    public Animation tipsAni;
    public Text txtTips;
    public Transform hpItemRoot;

    private bool isTipsShow = false;
    private Queue<string> tipsQue = new Queue<string>();
    public Dictionary<string, ItemEntityHp> itemDic = new Dictionary<string, ItemEntityHp>();

    protected override void InitWnd() {
        base.InitWnd();

        SetActive(txtTips, false);
    }

    #region Tips相关
    public void AddTips(string tips) {
        lock (tipsQue) {
            tipsQue.Enqueue(tips);
        }
    }

    private void Update() {
        if (tipsQue.Count > 0 && isTipsShow == false) {
            lock (tipsQue) {
                string tips = tipsQue.Dequeue();
                isTipsShow = true;
                SetTips(tips);
            }
        }
    }

    private void SetTips(string tips) {
        SetActive(txtTips);
        SetText(txtTips, tips);

        AnimationClip clip = tipsAni.GetClip("TipsShowAni");
        tipsAni.Play();
        //延时关闭激活状态

        StartCoroutine(AniPlayDone(clip.length, () => {
            SetActive(txtTips, false);
            isTipsShow = false;
        }));
    }

    private IEnumerator AniPlayDone(float sec, Action cb) {
        yield return new WaitForSeconds(sec);
        if (cb != null) {
            cb();
        }
    }
    #endregion

    public void AddHpItemInfo(string mName, Transform trans, int hp)
    {
        ItemEntityHp item = null;
        if (itemDic.TryGetValue(mName, out item))
        {
            return;
        }
        else
        {
            GameObject go = resSvc.LoadPrefab(PathDefine.HPItemPrefab, true);
            go.transform.SetParent(hpItemRoot, false);
            go.transform.localPosition = new Vector3(-1000, 0, 0);
            ItemEntityHp ieh = go.GetComponent<ItemEntityHp>();
            ieh.InitItemInfo(trans, hp);
            itemDic.Add(mName, ieh);
        }
    }

    public void RmvHpItemInfo(string mName)
    {
        ItemEntityHp item = null;
        if (itemDic.TryGetValue(mName, out item))
        {
            Destroy(item.gameObject);
            itemDic.Remove(mName);
        }
    }

    public void SetDodge(string key)
    {
        ItemEntityHp item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetDodge();
        }
    }
    public void SetCritical(string key, int critical)
    {
        ItemEntityHp item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetCritical(critical);
        }
    }
    public void SetHurt(string key, int hurt)
    {
        ItemEntityHp item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetHurt(hurt);
        }
    }
    public void SetHPVal(string key, int oldVal, int newVal)
    {
        ItemEntityHp item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetHPVal(oldVal, newVal);
        }
    }
}