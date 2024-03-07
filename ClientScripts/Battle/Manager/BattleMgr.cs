using PEProtocol;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 战场管理器
/// </summary>
public class BattleMgr : MonoBehaviour
{
    private ResSvc resSvc;
    private AudioSvc audioSvc;

    private StateMgr stateMgr;
    private SkillMgr skillMgr;
    private MapMgr mapMgr;

    private EntityPlayer entitySelfPlayer;
    private MapCfg mapCfg;
    
    private Dictionary<string, EntityMonster> monsterDic = new Dictionary<string, EntityMonster>();

    public void Init(int mapid)
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;

        //初始化各管理器
        stateMgr = gameObject.AddComponent<StateMgr>();
        stateMgr.Init();
        skillMgr= gameObject.AddComponent<SkillMgr>();
        skillMgr.Init();

        //加载战场地图
        mapCfg = resSvc.GetMapCfgData(mapid);
        resSvc.AsyncLoadScene(mapCfg.sceneName, () =>
        {
            //初始化地图数据
            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            mapMgr = map.GetComponent<MapMgr>();
            mapMgr.Init(this);

            map.transform.localPosition = Vector3.zero;
            map.transform.localScale = Vector3.one;

            Camera.main.transform.position = mapCfg.mainCamPos;
            Camera.main.transform.localEulerAngles = mapCfg.mainCamRote;

            LoadPlayer(mapCfg);
            entitySelfPlayer.Idle();

            //激活第一批次怪物
            ActiveCurrentBatchMonsters();

            audioSvc.PlayBGMusic(Constants.BGHuangYe);
        });


        PECommon.Log("Init BattleMgr Done.");
    }

    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.BattlePlayerPrefab, true);

        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = Vector3.one;

        PlayerData pd = GameRoot.Instance.PlayerData;
        BattleProps props = new BattleProps
        {
            hp = pd.hp,
            ad = pd.ad,
            ap = pd.ap,
            addef = pd.addef,
            apdef = pd.apdef,
            dodge = pd.dodge,
            pierce = pd.pierce,
            critical = pd.critical
        };

        entitySelfPlayer = new EntityPlayer
        {
            battleMgr = this,
            stateMgr = stateMgr,
            skillMgr = skillMgr,
        };
        entitySelfPlayer.Name = "AssassinBattle";
        entitySelfPlayer.SetBattleProps(props);

        PlayerControllerBattle playerCtrl = player.GetComponent<PlayerControllerBattle>();
        playerCtrl.Init();
        entitySelfPlayer.SetCtrl(playerCtrl);
    }

    public void LoadMonsterByWaveID(int wave)
    {
        for (int i = 0; i < mapCfg.monsterLst.Count; i++)
        {
            MonsterData md = mapCfg.monsterLst[i];
            if (md.mWave == wave)
            {
                GameObject m = resSvc.LoadPrefab(md.mCfg.resPath, true);
                m.transform.localPosition = md.mBornPos;
                m.transform.localEulerAngles = md.mBornRote;
                m.transform.localScale = Vector3.one;

                m.name = "m" + md.mWave + "_" + md.mIndex;

                EntityMonster em = new EntityMonster
                {
                    battleMgr = this,
                    stateMgr = stateMgr,
                    skillMgr = skillMgr,
                };
                //设置初始属性
                em.md = md;
                em.SetBattleProps(md.mCfg.bps);
                em.Name = m.name;

                MonsterController mc = m.GetComponent<MonsterController>();
                mc.Init();
                em.SetCtrl(mc);

                m.SetActive(false);
                monsterDic.Add(m.name, em);
                GameRoot.Instance.dynamicWnd.AddHpItemInfo(m.name, mc.hpRoot, em.HP);
            }
        }
    }

    public void ActiveCurrentBatchMonsters()
    {
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            foreach (var item in monsterDic)
            {
                item.Value.SetActive(true);
                item.Value.Born();
                TimerSvc.Instance.AddTimeTask((int id) =>
                {
                    //出生1秒钟后进入Idle
                    item.Value.Idle();
                }, 1000);
            }
        }, 500);
    }

    public List<EntityMonster> GetEntityMonsters()
    {
        List<EntityMonster> monsterLst = new List<EntityMonster>();
        foreach (var item in monsterDic)
        {
            monsterLst.Add(item.Value);
        }
        return monsterLst;
    }

    public void RmvMonster(string key)
    {
        EntityMonster entityMonster;
        if (monsterDic.TryGetValue(key, out entityMonster))
        {
            monsterDic.Remove(key);
            GameRoot.Instance.dynamicWnd.RmvHpItemInfo(key);
        }
    }

    #region 技能相关与角色控制
    public void SetSelfPlayerMoveDir(Vector2 dir)
    {
        //设置玩家移动
        //PECommon.Log(dir.ToString());
        if (entitySelfPlayer.canControl == false)
        {
            return;
        }

        if ( dir == Vector2.zero)
        {
            entitySelfPlayer.Idle();         
        }
        else
        {
            entitySelfPlayer.Move();
            entitySelfPlayer.SetDir(-dir);
        }    
    }
    public void ReqReleaseSkill(int index)
    {
        switch(index)
        {
            case 0:
                ReleaseNormalAtk();
                break;
            case 1:
                ReleaseSkill1();
                break;
            case 2: 
                ReleaseSkill2(); 
                break;
            case 3: 
                ReleaseSkill3();
                break;
        }
    }
    private void ReleaseNormalAtk()
    {
        PECommon.Log("Click Normal Atk");
    }
    private void ReleaseSkill1()
    {
        //PECommon.Log("Click Skill1");
        entitySelfPlayer.Attack(101);
    }
    private void ReleaseSkill2()
    {
        //PECommon.Log("Click Skill2");
        entitySelfPlayer.Attack(102);
    }
    private void ReleaseSkill3()
    {
        //PECommon.Log("Click Skill3");
        entitySelfPlayer.Attack(103);
    }
    public Vector2 GetDirInput()
    {
        return BattleSys.Instance.GetDirInput();
    }
    #endregion
}