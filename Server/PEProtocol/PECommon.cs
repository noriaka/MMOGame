﻿/****************************************************
// 客户端服务端共用工具类
*****************************************************/

using PENet;
using PEProtocol;

public enum LogTypeNew {
    Log = 0,
    Warn = 1,
    Error = 2,
    Info = 3
}

public class PECommon {


    public static void Log(string msg = "", LogTypeNew tp = LogTypeNew.Log) {
        LogLevel lv = (LogLevel)tp;
        PETool.LogMsg(msg, lv);
    }

    public static int GetFightByProps(PlayerData pd)
    {
        return pd.lv * 100 + pd.ad + pd.ap + pd.addef + pd.apdef;
    }

    public static int GetPowerLimit(int lv)
    {
        return ((lv - 1) / 10) * 150 + 150;
    }

    public static int GetExpUpValBylv(int lv)
    {
        return 100 * lv * lv;
    }

    public static void CalcExp(PlayerData pd, int addExp)
    {
        int curtLv = pd.lv;
        int curtExp = pd.exp;
        int addRestExp = addExp;
        while (true)
        {
            int upNeedExp = PECommon.GetExpUpValBylv(curtLv) - curtExp;
            if (addRestExp >= upNeedExp)
            {
                curtLv += 1;
                curtExp = 0;
                addRestExp -= upNeedExp;
            }
            else
            {
                pd.lv = curtLv;
                pd.exp = addRestExp + curtExp;
                break;
            }
        }
    }

    public const int PowerAddSpace = 5; //分钟
    public const int PowerAddCount = 2;
}
