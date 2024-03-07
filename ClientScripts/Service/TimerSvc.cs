using System;
/// <summary>
/// 计时服务
/// </summary>
public class TimerSvc : SystemRoot
{
    public static TimerSvc Instance = null;

    private PETimer pt;
    public void InitSvc()
    {
        Instance = this;
        pt = new PETimer();
        //设置日志输出
        pt.SetLog((string info) =>
        {
            PECommon.Log(info);
        });
        PECommon.Log("Init TimerSvc...");
    }
    public void Update()
    {
        pt.Update();
    }
    public int AddTimeTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        return pt.AddTimeTask(callback, delay, timeUnit, count);
    }
}

