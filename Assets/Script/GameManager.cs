using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject prefabUnit;
    public LeaderManager leaderManager;
    public List<Unit> enemys = new List<Unit>();
    public int enemyLv;
    public Transform[] Board;
    public UnitInfo lvUnitinfo;

    enum eBoard
    {
        Leader,
        Job,
        Lv,
        Exp,
        Hp,
        Mp,
        Atk,
        Skill,
        Inv
    }

    public GameObject BackLvBox;


    private void Start()
    {
        SetPlayerTeam();    
        leaderManager.SetPlayerLeader();
        enemyLv = 0;
        Invoke("SetEnemeyTeam", 5.0f);
    }

    public void SetPlayerTeam()
    {
        for(int i = 0; i < 3; i ++)
        {
            Unit unit = Instantiate(prefabUnit).GetComponent<Unit>();
            unit.UnitInfo.team = Team.Player;
            unit.UnitInfo.playerType = (PlayerType)i;
            unit.UnitInfo.SetInitPlayerStatus(unit.UnitInfo.playerType);
            unit.gameObject.name = ((PlayerType)i).ToString();
            SetUI(i, unit.UnitInfo);
        }
    }

    public void SetUI(int num, UnitInfo unitinfo)
    {
        Board[(int)eBoard.Job].GetChild(num).GetComponent<Text>().text = unitinfo.playerType.ToString();
        Board[(int)eBoard.Lv].GetChild(num).GetComponent<Text>().text = unitinfo.curLv + "/" + unitinfo.MaxLv;
        Board[(int)eBoard.Exp].GetChild(num).GetComponent<Text>().text = unitinfo.exp + "/" + unitinfo.RequireExp();
        Board[(int)eBoard.Hp].GetChild(num).GetComponent<Text>().text = unitinfo.curHp + "/" + unitinfo.maxHp;
        Board[(int)eBoard.Mp].GetChild(num).GetComponent<Text>().text = unitinfo.curMp + "/" + unitinfo.maxMp;
        Board[(int)eBoard.Atk].GetChild(num).GetComponent<Text>().text = unitinfo.attackDamage.ToString();
    }

    public void SetSkillUI(int num, UnitInfo unitInfo)
    {
        lvUnitinfo = unitInfo;
        BackLvBox.SetActive(true);
        Text CurrentPlayerJob = BackLvBox.transform.GetChild(0).GetComponent<Text>();
        CurrentPlayerJob.text = unitInfo.playerType.ToString();
        GameObject PrivateSkillParent = BackLvBox.transform.GetChild(1).gameObject;
        for(int i = 0; i < PrivateSkillParent.transform.childCount; i++)
        {
            Text SkillNameText = PrivateSkillParent.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
            string skillName = "";
            switch(num)
            {
                case 0:
                    skillName = ((MeleSkill)(i + 1)).ToString();
                    break;
                case 1:
                    skillName = ((RangeSkill)(i + 1)).ToString();
                    break;
                case 2:
                    skillName = ((MagicSkill)(i + 1)).ToString();
                    break;
            }
            SkillNameText.text = skillName;
        }
    }

    public void UnitSkillUI(int num, UnitInfo unitInfo)
    {
        GameObject PrivateSkillObj = Board[(int)eBoard.Skill].transform.GetChild(num).gameObject;

        for(int i = 0; i < PrivateSkillObj.transform.childCount; i++)
        {
            Text skillText = PrivateSkillObj.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
            string skillName = "";
            switch(num)
            {
                case 0:
                    skillName = unitInfo.meleSkill[i].ToString();
                    break;
                case 1:
                    skillName = unitInfo.rangeSkills[i].ToString();
                    break;
                case 2:
                    skillName = unitInfo.magicSkill[i].ToString();
                    break;
            }
            if(skillName.Equals("none"))
                continue;
            skillName += $" Lv.{unitInfo.SkillLev[i]}";
            skillText.text = skillName;

        }
    }

    public void BtnSKill(int num)
    {
        switch(num)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                switch(lvUnitinfo.playerType)
                {
                    case PlayerType.Mele:
                        {
                            int tempNum = -1;
                            for (int i = 0; i < lvUnitinfo.meleSkill.Length; i++)
                            {
                                if (lvUnitinfo.meleSkill[i] == (MeleSkill)(num + 1))
                                {
                                    tempNum = i;
                                    break;
                                }
                            }
                            if (tempNum == -1)
                            {
                                for (int i = 0; i < lvUnitinfo.SkillLev.Length; i++)
                                {
                                    if (lvUnitinfo.SkillLev[i] == 0)
                                    {
                                        tempNum = i;
                                        break;
                                    }
                                }
                                lvUnitinfo.meleSkill[tempNum] = (MeleSkill)(num + 1);
                                Time.timeScale = 1f;
                                BackLvBox.SetActive(false);
                                lvUnitinfo.SkillLev[tempNum]++;
                                UnitSkillUI(0, lvUnitinfo);
                                return;

                            }
                            // 스킬 UI 코드( 안함 )
                            if (lvUnitinfo.SkillLev[tempNum] >= 5)
                            {
                                return;
                            }
                            lvUnitinfo.SkillLev[tempNum]++;
                            UnitSkillUI(0, lvUnitinfo);
                        }
                        break;
                    case PlayerType.Ranged:
                        {
                            int tempNum = -1;
                            for (int i = 0; i < lvUnitinfo.rangeSkills.Length; i++)
                            {
                                if (lvUnitinfo.rangeSkills[i] == (RangeSkill)(num + 1))
                                {
                                    tempNum = i;
                                    break;
                                }
                            }
                            if (tempNum == -1)
                            {
                                for (int i = 0; i < lvUnitinfo.SkillLev.Length; i++)
                                {
                                    if (lvUnitinfo.SkillLev[i] == 0)
                                    {
                                        tempNum = i;
                                        break;
                                    }
                                }
                                lvUnitinfo.rangeSkills[tempNum] = (RangeSkill)(num + 1);
                                Time.timeScale = 1f;
                                BackLvBox.SetActive(false);
                                lvUnitinfo.SkillLev[tempNum]++;
                                UnitSkillUI(1, lvUnitinfo);
                                return;

                            }
                            // 스킬 UI 코드( 안함 )
                            if (lvUnitinfo.SkillLev[tempNum] >= 5)
                            {
                                return;
                            }
                            lvUnitinfo.SkillLev[tempNum]++;
                            UnitSkillUI(1, lvUnitinfo);
                        }
                        break;
                    case PlayerType.Magic:
                        {
                            int tempNum = -1;
                            for (int i = 0; i < lvUnitinfo.magicSkill.Length; i++)
                            {
                                if (lvUnitinfo.magicSkill[i] == (MagicSkill)(num + 1))
                                {
                                    tempNum = i;
                                    Debug.Log("Magic : " + tempNum);
                                    break;
                                }
                            }
                            if (tempNum == -1)
                            {
                                for (int i = 0; i < lvUnitinfo.SkillLev.Length; i++)
                                {
                                    if (lvUnitinfo.SkillLev[i] == 0)
                                    {
                                        tempNum = i;
                                        Debug.Log("Magic2 : " + tempNum);
                                        break;
                                    }
                                }

                                lvUnitinfo.magicSkill[tempNum] = (MagicSkill)(num + 1);
                                Time.timeScale = 1f;
                                BackLvBox.SetActive(false);
                                lvUnitinfo.SkillLev[tempNum]++;
                                UnitSkillUI(2, lvUnitinfo);
                                return;
                            }
                            // 스킬 UI 코드( 안함 )
                            if (lvUnitinfo.SkillLev[tempNum] >= 5)
                            {
                                return;
                            }
                            lvUnitinfo.SkillLev[tempNum]++;
                            UnitSkillUI(2, lvUnitinfo);
                        }

                        break;


                }
                break;

            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
                {
                    foreach(Unit u in leaderManager.units)
                    {
                        u.UnitInfo.commonSkillLev[(num - 4)]++;
                    }
                }
                break;
        }
        Time.timeScale = 1f;
        BackLvBox.SetActive(false);
    }

    public void SetUILeader(int num, UnitInfo unitInfo)
    {
        for(int i = 0; i < Board.Length; i++)
        {
            if(i  == num)
            {
                Board[(int)eBoard.Leader].GetChild(i).GetComponent<Text>().text = "Leader";
            } else
            {
                Board[(int)eBoard.Leader].GetChild(i).GetComponent<Text>().text = "";
            }
        }
    }

    public void SetEnemeyTeam()
    {
        switch (enemyLv)
        {
            case 0:
                int num = UnityEngine.Random.Range(2, 6);
                for(int i = 0; i < num; i++)
                {
                    Unit unit = Instantiate(prefabUnit, new Vector3(5.0f, 0, 0), prefabUnit.transform.rotation).GetComponent<Unit>();
                    unit.UnitInfo.team = Team.Enemy;
                    unit.UnitInfo.enemyType = EnemyType.Mele1;
                    //unit.currentTarget = leaderManager.currentLeader.transform;
                    unit.Leader = leaderManager.currentLeader.transform;
                    unit.gameObject.name = EnemyType.Mele1.ToString() + i.ToString();
                    unit.UnitInfo.SetInitEnemyStatus(unit.UnitInfo.enemyType);
                    enemys.Add(unit);
                }
                break;
        }
    }





}
