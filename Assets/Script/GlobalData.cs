using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Player,
    Enemy,
    Neutral
}

public enum PlayerType
{
    Mele,
    Ranged,
    Magic
}

public enum EnemyType
{
    Mele1,
    Mele2,
    Mele3,
    Range1,
    Range2,
    Range3,
    boss1,
    boss2,
    boss3
}

public enum Boss1Skill
{
    summonMele1,
    summonMele2
}

public enum Boss2Skill
{
    summonMele1,
    summonMele2,
    summonRange2,
    summonRange3
}

public enum Boos3Skill
{
    summonMele3,
    summonRange3,
    HideUI,
    ChaosScreen
}

public enum Status
{
    Idle,
    Follow,
    Chase,
    Attack,
    Dead
}

public enum MeleSkill
{
    none,
    mele,
    muti,
    piercing,
    stun
}

public enum RangeSkill
{
    none,
    combo,
    muti,
    /// <summary>
    /// 관통
    /// </summary>
    anglePiercing,
    /// <summary>
    /// 범위
    /// </summary>
    aoe
}

public enum MagicSkill
{
    none,
    combo,
    chain,
    heal,
    poison
}

public enum CommonSkill
{
    criticalRateUp,
    attackSpeedUp,
    HpUp,
    moveSpeedUp,
    attackDamageUP
}

public enum ItemName
{
    none,
    Hp1,
    Hp2,
    Hp3,
    Mp1,
    Mp2,
    Mp3,
    FlashBattle,
    MentraFocus,
    Revival

}

public enum Debuff
{
    none,
    stun,
    poison
}

[System.Serializable]
public class PotionInfo
{
    public int[] Amount = new int[] { 10, 30, 50 };
    public float Coll = 2;
}










public class GlobalData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
