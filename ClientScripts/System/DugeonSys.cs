using PEProtocol;
/// <summary>
/// 副本业务系统
/// </summary>
public class DugeonSys : SystemRoot
{
    public static DugeonSys Instance = null;

    public DugeonWnd dugeonWnd;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init DugeonSys...");
    }

    public void EnterDugeon()
    {
        OpenDugeonWnd();
    }

    #region Dugeon Wnd
    public void OpenDugeonWnd()
    {
        dugeonWnd.SetWndState();
    }
    #endregion

    public void RspDugeonFight(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerDataByDugeonStart(msg.rspDugeonFight);
        MainCitySys.Instance.mainCityWnd.SetWndState(false);
        dugeonWnd.SetWndState(false);
        BattleSys.Instance.StartBattle(msg.rspDugeonFight.dugeon_id);
    }
}