using UnityEngine;
using UnityEditorInternal.VR;
using JetBrains.Annotations;
/// <summary>
/// 逻辑实体基类
/// </summary>
public class EntityBase
{
    public AniState currentAniState = AniState.None;

    public BattleMgr battleMgr = null;
    public StateMgr stateMgr = null;
    public SkillMgr skillMgr = null;
    protected Controller controller = null;
    private string name;
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public bool canControl = true;

    private BattleProps props;
    public BattleProps Props
    {
        get
        {
            return props;
        }

        protected set
        {
            props = value;
        }
    }

    private int hp;
    public int HP
    {
        get
        {
            return hp;
        }

        set
        {
            //通知UI层 TODO
            PECommon.Log("hp change:" + hp + "to" + value);
            SetHPVal(hp, value);
            hp = value;
        }
    }

    public void Born()
    {
        stateMgr.ChangeState(this, AniState.Born, null);
    }
    public void Move()
    {
        stateMgr.ChangeState(this, AniState.Move, null);
    }
    public void Idle()
    {
        stateMgr.ChangeState(this, AniState.Idle, null);
    }
    public void Attack(int skillID)
    {
        stateMgr.ChangeState(this, AniState.Attack, skillID);
    }
    public void Hit()
    {
        stateMgr.ChangeState(this, AniState.Hit, null);
    } 
    public void Die()
    {
        stateMgr.ChangeState(this, AniState.Die, null);
    }

    public void SetCtrl(Controller ctrl)
    {
        controller = ctrl;
    }

    public void SetActive(bool active = true)
    {
        if (controller != null)
        {
            controller.gameObject.SetActive(active);
        }
    }

    public virtual void SetBattleProps(BattleProps props)
    {
        HP = props.hp;
        Props = props;
    }

    public virtual void SetBlend(float blend)
    {
        if (controller != null)
        {
            controller.SetBlend(blend);
        }
    }

    public virtual void SetDir(Vector2 dir)
    {
        if (controller != null)
        {
            controller.Dir = dir;
        }
    }

    public virtual void SetAction(int act)
    {
        if (controller != null)
        {
            controller.SetAction(act);
        }
    }

    public virtual void SetSkillMoveState(bool move, float speed = 0f)
    {
        if (controller != null)
        {
            controller.SetSkillMoveState(move, speed);
        }
    }

    public virtual void SetDodge()
    {
        if (controller != null)
        {
            GameRoot.Instance.dynamicWnd.SetDodge(Name);
        }
    }
    public virtual void SetCritical(int critical)
    {
        if (controller != null)
        {
            GameRoot.Instance.dynamicWnd.SetCritical(Name, critical);
        }
    }
    public virtual void SetHurt(int hurt)
    {
        if (controller != null)
        {
            GameRoot.Instance.dynamicWnd.SetHurt(Name, hurt);
        }
    }
    public virtual void SetHPVal(int oldVal, int newVal)
    {
        if (controller != null)
        {
            GameRoot.Instance.dynamicWnd.SetHPVal(Name, oldVal, newVal);
        }
    }
    public virtual void SetFX(string name, float destroy)
    {
        if (controller != null)
        {
            controller.SetFX(name, destroy);
        }
    }

    public virtual void SkillAttack(int skillID)
    {
        skillMgr.SkillAttack(this, skillID);
    }

    public virtual Vector2 GetDirInput()
    {
        return Vector2.zero;
    }

    public virtual Vector3 GetPos()
    {
        return controller.transform.position;
    }

    public virtual Transform GetTrans()
    {
        return controller.transform;
    }

    public AnimationClip[] GetAniClips()
    {
        if (controller != null)
        {
            return controller.ani.runtimeAnimatorController.animationClips;
        }
        return null;
    }
}
