using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [SerializeField]
    public UnitInfo UnitInfo;

    public int maxHP;

    public Transform Leader;
    /// <summary>
    /// 리더 따라 다닐 위치의 오프셋
    /// </summary>
    public Vector3 formOffset;
    /// <summary>
    /// 목표 위치
    /// </summary>
    public Vector3 desiredVelocity;

    public float attTimger;

    public Transform currentTarget;

    public List<Unit> tempTeam = new List<Unit>();

    public NavMeshAgent navMeshAgent;

    public GameObject[] effectPrefab;

    public float[] skillCoolDown = new float[] { 0, 0, 0, 0, 0 };

    public Debuff Debuff = Debuff.none;
    /// <summary>
    /// 디버프 딜레이
    /// </summary>
    public float stunTimer = 2.0f;
    /// <summary>
    /// 디버프 딜레이 계산 변수
    /// </summary>
    public float stunTiming = 2.0f;

    PotionInfo PotionInfo = new PotionInfo();

    GameManager gameManager;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        UnitInfo = GetComponent<UnitInfo>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    private void FixedUpdate()
    {
        if(Leader == null)
        {
            //Debug.Log("test" + gameObject.name);
            return;
        }

        if(navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            return;
        }

        if(desiredVelocity.sqrMagnitude > 0.01f)
        {
            //Debug.Log("이동중");
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(desiredVelocity);
        } else
        {
            navMeshAgent.isStopped = true;
        }
    }

    private void Update()
    {
        if(Debuff == Debuff.stun)
        {
            stunTimer -= Time.deltaTime;
            if(stunTimer > 0)
            {
                return ;
            } else
            {
                stunTiming = stunTimer;
                Debuff = Debuff.none;
            }
        }

        if(UnitInfo.Status == Status.Dead)
        {
            return;
        }

        if(currentTarget == null || isVaildTarget(currentTarget))
        {
           
            currentTarget = minDistanceTarget();
        }

        if(currentTarget != null)
        {
            float dis = Vector3.Distance(transform.position, currentTarget.position);
            if(dis <= UnitInfo.attackRange)
            {
                UnitInfo.Status = Status.Attack;
            } else
            {
                UnitInfo.Status = Status.Chase;
            }
        }else
        {
            if(Leader != null)
            {
                UnitInfo.Status = Status.Follow;
            } else
            {
                UnitInfo.Status = Status.Idle;
            }
        }

        switch(UnitInfo.Status)
        {
            case Status.Attack:
                desiredVelocity = Vector3.zero;
                    Attack(currentTarget);

                    break;
            case Status.Chase:
                Vector3 dir = currentTarget.position - transform.position;
                dir.y = 0;
                if(dir.sqrMagnitude < 0.1f)
                {
                    desiredVelocity = Vector3.zero;
                }
                else
                {
                    desiredVelocity = currentTarget.position;
                }
                break;
            case Status.Follow:
                Vector3 target = Leader.position + formOffset;
                desiredVelocity = target;
                break;

        }
        
    }

    bool isVaildTarget(Transform tr)
    {
        if(tr == null)
        {
            return false;
        }
        Unit unit = tr.GetComponent<Unit>();
        if(unit == null)
        {
            return false;
        }
        if(unit.UnitInfo.curHp <= 0f)
        {
            return false;
        }
        if(unit.UnitInfo.team == this.UnitInfo.team)
        {
            return false;
        }

        return true;
    }

    Transform minDistanceTarget()
    {
        // 근접 탐지
        Collider[] col = Physics.OverlapSphere(transform.position, UnitInfo.detecRadius, LayerMask.GetMask("Unit"));

        Transform choice = null;

        float bestSqr = float.MaxValue;
        for(int i = 0; i < col.Length; i++)
        {
            Unit unit = col[i].GetComponent<Unit>();
            if (unit == null || unit.UnitInfo.team == this.UnitInfo.team || unit.UnitInfo.curHp <= 0)
            {
                continue;
            }

            float sq = (unit.transform.position - transform.position).sqrMagnitude;
            // distance도 됨
            float distance = Vector3.Distance(unit.transform.position, transform.position);
            if(sq < bestSqr)
            {
                bestSqr = sq;
                choice = unit.transform;
            }
        }
        if (choice != null)
        {
            //Debug.Log(choice.name);
        }
        return choice;
    }
    void Attack(Transform target)
    {
        attTimger -= Time.deltaTime;

        if (attTimger > 0)
        {
            return;
        }

        if (target == null)
        {
            return;
        }

        if (!isVaildTarget(target))
        {
            Debug.Log(!isVaildTarget(target));
            return;
        }
        Unit targetUnit = target.GetComponent<Unit>();
        if (UnitInfo.team == Team.Player)
        {
            // 공격 이펙트
            //GameObject effect = Instantiate(effectpr)
            if (targetUnit.UnitInfo.curHp - UnitInfo.attackDamage > 0)
            {
                UnitInfo.exp += targetUnit.UnitInfo.GetExp(UnitInfo.attackDamage);
            }
            else
            {
                UnitInfo.exp += targetUnit.UnitInfo.GetExp(UnitInfo.attackDamage);
                UnitInfo.exp += targetUnit.UnitInfo.GetBonusExp();
            }

            LvUp();
        }
        targetUnit.Damage(UnitInfo.attackDamage);
        attTimger = UnitInfo.attackRate;
    }

    public void LvUp()
    {
        if(UnitInfo.exp > UnitInfo.RequireExp())
        {
            UnitInfo.exp -= UnitInfo.RequireExp();
            UnitInfo.curLv++;
            UnitInfo.SetInitPlayerStatus(UnitInfo.playerType);

            gameManager.SetUI((int)UnitInfo.playerType, UnitInfo);
            if(UnitInfo.curLv % 2 == 1)
            {
                Time.timeScale = 0;
                gameManager.SetSkillUI((int)UnitInfo.playerType, UnitInfo);
            }
        }
    }

    public void Damage(float damage)
    {
        if(UnitInfo.curHp <= 0f)
        {
            return ;    
        }
        UnitInfo.curHp -= damage;

        if(UnitInfo.curHp <= 0f)
        {
            UnitInfo.curHp = 0f;
            GameObject manager = GameObject.Find("GameManager");
            GameManager gameManager = manager.GetComponent<GameManager>();
            LeaderManager leaderManager = gameManager.leaderManager;
            if (leaderManager.currentLeader == this)
            {
                leaderManager.units.Remove(this);
                leaderManager.currentLeaderIndex = 0;
                leaderManager.ChangePlayerLeader(0);
                //gameManager.SetEnemyLeader();
            } else
            {
                leaderManager.units.Remove(this);
                gameManager.enemys.Remove(this);
            }
            if(UnitInfo.team == Team.Enemy)
            {
                Destroy(gameObject);
            } else
            {
                gameObject.SetActive(false);
            }
        }

    }


}

