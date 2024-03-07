/****************************************************
	文件：PathDefine.cs
	作者：SIKI学院——Plane
	邮箱: 1785275942@qq.com
	日期：2018/12/05 3:46   	
	功能：路径常量宣言
*****************************************************/

using UnityEngine;
using System.Collections;

public class PathDefine {

    #region Configs
    public const string RDNameCfg = "ResCfgs/rdname";
	public const string MapCfg = "ResCfgs/map";
	public const string GuideCfg = "ResCfgs/guide";
	public const string StrongCfg = "ResCfgs/strong";
	public const string TaskRewardCfg = "ResCfgs/taskreward";
	public const string SkillCfg = "ResCfgs/skill";
	public const string SkillMoveCfg = "ResCfgs/skillmove";
	public const string SkillActionCfg = "ResCfgs/skillaction";
	public const string MonsterCfg = "ResCfgs/monster";
	#endregion

	#region Strong
	public const string ItemHead = "MyRes/Images/head";
	public const string ItemBody = "MyRes/Images/body";
	public const string ItemWaist = "MyRes/Images/waist";
	public const string ItemHand = "MyRes/Images/hand";
	public const string ItemLeg = "MyRes/Images/leg";
	public const string ItemFoot = "MyRes/Images/foot";

	public const string SpStar1 = "ResImages/star1";
	public const string SpStar2 = "ResImages/star2";
	#endregion

	#region TaskReward
	public const string TaskItemPrefab = "PrefabUI/ItemTask";
    #endregion

    #region AutoGuide
    public const string TaskHead = "MyRes/Images/task";
	public const string WiseManHead = "MyRes/Images/wiseman";
	public const string GeneralHead = "MyRes/Images/general";
	public const string ArtisanHead = "MyRes/Images/artisan";
	public const string TraderHead = "MyRes/Images/trader";

	public const string SelfIcon = "MyRes/Images/task";
	public const string GuideIcon = "MyRes/Images/task";
	public const string WiseManIcon = "MyRes/Images/wiseman";
	public const string GeneralIcon = "MyRes/Images/general";
	public const string ArtisanIcon = "MyRes/Images/artisan";
	public const string TraderIcon = "MyRes/Images/trader";
	#endregion

	#region Player
	public const string CityPlayerPrefab = "PrefabPlayer/Eula";
	public const string BattlePlayerPrefab = "PrefabPlayer/AssassinBattle";

	public const string HPItemPrefab = "PrefabUI/ItemEntityHp";
	#endregion
}
