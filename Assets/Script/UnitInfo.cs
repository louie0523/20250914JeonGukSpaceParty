using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfo : MonoBehaviour
{
    
    public int[] LvUpStatusHP = new int[] { 20, 15, 10 };

    public int[] LvUpStatusDamage = new int[] { 10, 8, 10 };

    public int[] SkillLev = new int[] { 0, 0, 0, 0 };

    public int[] commonSkillLev = new int[] { 0, 0, 0, 0, 0 };
    /// <summary>
    /// 크확 증가
    /// </summary>
    public int[] commonSkill1 = new int[] { 0, 10, 20, 30, 40, 50 };
    /// <summary>
    /// 공속 증가
    /// </summary>
    public float[] commonSkill2 = new float[] { 0f, 0.5f, 1.0f, 1.5f, 2.0f, 3.0f };
    /// <summary>
    /// 체력 증가
    /// </summary>
    public int[] commonSkill3 = new int[] { 0, 200, 400, 600, 800, 1000 };
    /// <summary>
    /// 이속 증가
    /// </summary>
    public float[] commonSkill4 = new float[] { 0f, 0.5f, 1.0f, 1.2f, 1.5f, 2.0f };
    /// <summary>
    /// 공격력 증가
    /// </summary>
    public int[] commonSkill5 = new int[] { 0, 20, 40, 60, 80, 100 };

    public MeleSkill[] meleSkill = new MeleSkill[]
    {
        MeleSkill.none, MeleSkill.none,
        MeleSkill.none, MeleSkill.none
    };

    public RangeSkill[] rangeSkills = new RangeSkill[]
    {
        RangeSkill.none, RangeSkill.none, RangeSkill.none, RangeSkill.none
    };

    public MagicSkill[] magicSkill = new MagicSkill[]
    {
        MagicSkill.none, MagicSkill.none,
        MagicSkill.none, MagicSkill.none
    };

    public Status Status;

    public Team team;

    public PlayerType playerType;

    public EnemyType enemyType;


    public int MaxLv = 20;

    public int curLv = 0;

    public float baseHp = 100f;

    public float maxHp = 100f;

    public float curHp = 100f;

    public float maxMp = 50f;

    public float curMp = 50f;

    public float moveSpeed = 4.5f;

    public float criticalRate = 10f;

    public float turnSpeed = 720f;

    public float baseAttackDamage = 1f;

    public float attackDamage = 1f;
    /// <summary>
    /// 공격 간격
    /// </summary>
    public float attackRate = 1.0f;

    public float attackRange = 1.4f;
    /// <summary>
    /// 적 감지 반경
    /// </summary>
    public float detecRadius = 8f;

    public float arrowSpeed = 16f;

    public float healDamage = 10f;

    public float arriveRadius = 0.6f;

    public float friendRadius = 1.5f;
    /// <summary>
    /// Sepaaration 힘 가증추
    /// </summary>
    public float separationMeight = 4f;
    /// <summary>
    /// 방향 Aiigment 가중치
    /// </summary>
    public float cohesionWeight = 1.0f;
    
    public float exp = 0;

    public Vector3[] formOffset = new Vector3[]
    {
        new Vector3(2.0f, 0, 2.0f),
        new Vector3(2.0f, 0, 2.0f),
        new Vector3(2.0f, 0, -2.0f)
    };

    public float GetExp(float damage)
    {
        if (damage > maxHp)
        {
            return exp * curHp / maxHp;
        }
        return exp * damage / maxHp;
    }

    public float GetBonusExp()
    {
        return exp / 10;
    }

    public float RequireExp()
    {
        return 100f * curLv;
    }

    public void SetInitPlayerStatus(PlayerType _type)
    {
        playerType = _type;

        switch(playerType)
        {
            case PlayerType.Mele:
                baseHp = 100;
                maxHp = baseHp;
                curHp = maxHp;
                maxMp = 50;
                curMp = maxMp;
                baseAttackDamage = 20;
                attackDamage = baseAttackDamage;
                attackRange = 1.0f;
                criticalRate = 0.1f;
                break;
            case PlayerType.Ranged:
                baseHp = 80;
                maxHp = baseHp;
                curHp = maxHp;
                maxMp = 70;
                curMp = maxMp;
                baseAttackDamage = 15;
                attackDamage = baseAttackDamage;
                attackRange = 3.0f;
                criticalRate = 0.2f;
                break;
            case PlayerType.Magic:
                baseHp = 70;
                maxHp = baseHp;
                curHp = maxHp;
                maxMp = 150;
                curMp = maxMp;
                baseAttackDamage = 10;
                attackDamage = baseAttackDamage;
                attackRange = 3.0f;
                criticalRate = 0f;
                break;

        }
        maxHp = baseHp + (curLv * LvUpStatusHP[(int)_type]);
        curHp = maxHp;
        curMp = maxMp;
        attackDamage = baseAttackDamage + (curLv * LvUpStatusDamage[(int)_type]);
    }


    public void SetInitEnemyStatus(EnemyType _type)
    {
        enemyType = _type;

        switch (enemyType)
        {
            case EnemyType.Mele1:
                baseHp = 50;
                maxHp = baseHp;
                curHp = maxHp;
                maxMp = 50;
                curMp = maxMp;
                baseAttackDamage = 10;
                attackDamage = baseAttackDamage;
                attackRange = 1.0f;
                criticalRate = 0.1f;
                exp = 100;
                break;
            case EnemyType.Mele2:
                baseHp = 50;
                maxHp = baseHp;
                curHp = maxHp;
                maxMp = 50;
                curMp = maxMp;
                baseAttackDamage = 10;
                attackDamage = baseAttackDamage;
                attackRange = 1.0f;
                criticalRate = 0.1f;
                exp = 150;
                break;
            case EnemyType.Mele3:
                baseHp = 50;
                maxHp = baseHp;
                curHp = maxHp;
                maxMp = 50;
                curMp = maxMp;
                baseAttackDamage = 10;
                attackDamage = baseAttackDamage;
                attackRange = 1.0f;
                criticalRate = 0.1f;
                exp = 200;
                break;
            case EnemyType.Range1:
                baseHp = 30;
                maxHp = baseHp;
                curHp = maxHp;
                maxMp = 50;
                curMp = maxMp;
                baseAttackDamage = 10;
                attackDamage = baseAttackDamage;
                attackRange = 3.0f;
                criticalRate = 0.1f;
                exp = 100;
                break;
            case EnemyType.Range2:
                baseHp = 30;
                maxHp = baseHp;
                curHp = maxHp;
                maxMp = 50;
                curMp = maxMp;
                baseAttackDamage = 10;
                attackDamage = baseAttackDamage;
                attackRange = 3.0f;
                criticalRate = 0.1f;
                exp = 150;
                break;
            case EnemyType.Range3:
                baseHp = 30;
                maxHp = baseHp;
                curHp = maxHp;
                maxMp = 50;
                curMp = maxMp;
                baseAttackDamage = 10;
                attackDamage = baseAttackDamage;
                attackRange = 3.0f;
                criticalRate = 0.1f;
                exp = 200;
                break;
            case EnemyType.boss1:
                baseHp = 70;
                maxHp = baseHp;
                curHp = maxHp;
                maxMp = 150;
                curMp = maxMp;
                baseAttackDamage = 10;
                attackDamage = baseAttackDamage;
                attackRange = 3.0f;
                criticalRate = 0f;
                break;
            case EnemyType.boss2:
                baseHp = 70;
                maxHp = baseHp;
                curHp = maxHp;
                maxMp = 150;
                curMp = maxMp;
                baseAttackDamage = 10;
                attackDamage = baseAttackDamage;
                attackRange = 3.0f;
                criticalRate = 0f;
                break;
            case EnemyType.boss3:
                baseHp = 70;
                maxHp = baseHp;
                curHp = maxHp;
                maxMp = 150;
                curMp = maxMp;
                baseAttackDamage = 10;
                attackDamage = baseAttackDamage;
                attackRange = 3.0f;
                criticalRate = 0f;
                break;

        }
        maxHp = baseHp + (curLv * LvUpStatusHP[(int)_type]);
        curHp = maxHp;
        curMp = maxMp;
        attackDamage = baseAttackDamage + (curLv * LvUpStatusDamage[(int)_type]);
    }

}
