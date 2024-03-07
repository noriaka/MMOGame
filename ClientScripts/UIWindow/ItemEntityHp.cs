using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ÑªÌõÎïÌå
/// </summary>

public class ItemEntityHp : MonoBehaviour
{
    #region UI Define
    public Image imgHPGray;
    public Image imgHPRed;

    public Animation criticalAni;
    public Text txtCritical;

    public Animation dodgeAni;
    public Text txtDodge;

    public Animation hpAni;
    public Text txtHp;
    #endregion

    private RectTransform rect;
    private Transform rootTrans;
    private int hpVal;
    private float scaleRate = 1.0f * Constants.ScreenStandardHeight / Screen.height;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SetCritical(111);
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    SetHurt(222);
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    SetDodge();
        //}
        Vector3 screenPos = Camera.main.WorldToScreenPoint(rootTrans.position);
        rect.anchoredPosition = screenPos * scaleRate;

        UpdateMixBlend();
        imgHPGray.fillAmount = currentPrg;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currentPrg - targetPrg) < Constants.AccelerHPSpeed * Time.deltaTime)
        {
            currentPrg = targetPrg;
        }
        else if (currentPrg > targetPrg)
        {
            currentPrg -= Constants.AccelerHPSpeed * Time.deltaTime;
        }
        else
        {
            currentPrg += Constants.AccelerHPSpeed * Time.deltaTime;
        }
    }
    public void InitItemInfo(Transform trans, int hp)
    {
        rect = transform.GetComponent<RectTransform>();
        rootTrans = trans;
        hpVal = hp;
        imgHPGray.fillAmount = 1;
        imgHPRed.fillAmount = 1;
    }
    public void SetCritical(int critical)
    {
        criticalAni.Stop();
        txtCritical.text = "±©»÷ " + critical;
        criticalAni.Play();
    }
    public void SetDodge()
    {
        dodgeAni.Stop();
        txtDodge.text = "ÉÁ±Ü";
        dodgeAni.Play();
    }
    public void SetHurt(int hurt)
    {
        hpAni.Stop();
        txtHp.text = "-" + hurt;
        hpAni.Play();
    }

    private float currentPrg;
    private float targetPrg;
    public void SetHPVal(int oldVal, int newVal)
    {
        currentPrg = oldVal * 1.0f / hpVal;
        targetPrg = newVal * 1.0f / hpVal;
        imgHPRed.fillAmount = targetPrg;
    }
}
