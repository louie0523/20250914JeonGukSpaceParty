using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderManager : MonoBehaviour
{
    public List<Unit> units = new List<Unit>();
    public int currentLeaderIndex = 0;
    public Unit currentLeader;
    GameManager gameManager;
    //Cam cam;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        //cam = GameManager.Find("Main Camera").Getcomponent<Cam>();
    }

     public void SetPlayerLeader()
    {
        if(units.Count > 0)
        {
            ChangePlayerLeader(currentLeaderIndex);
        } else
        {
            Unit[] temp = GameObject.FindObjectsOfType<Unit>();
            foreach(Unit unit in temp)
            {
                units.Add(unit);
            }
            units.Sort((x, Y) => x.UnitInfo.playerType.CompareTo(Y.UnitInfo.playerType));
            ChangePlayerLeader(currentLeaderIndex);
            
        }
    }

    public void ChangePlayerLeader(int index)
    {
        if(units.Count < 0)
        {
            return;
        }

        if(currentLeader != null)
        {
            LeaderController oldLCtrl = currentLeader.GetComponent<LeaderController>();
            if (oldLCtrl != null)
            {
                Destroy(oldLCtrl);
            }
            CharacterController oldCCtrl = currentLeader.GetComponent<CharacterController>();
            if(oldCCtrl != null)
            {
                Destroy(oldCCtrl);
            }

            currentLeader.Leader = units[currentLeaderIndex].transform;
        }

        // 새 리더 지정
        currentLeaderIndex = index; 

        currentLeader = units[currentLeaderIndex];
        if(gameManager != null)
        {
            gameManager = GetComponent<GameManager>();
        }
        //gameManager.SetUILeader(currentLeaderIndex, units[currentLeaderIndex].UnitInfo);
        if (currentLeader.GetComponent<LeaderController>() == null)
        {
            currentLeader.gameObject.AddComponent<LeaderController>();
        }
        if (currentLeader.GetComponent<CharacterController>() == null)
        {
            currentLeader.gameObject.AddComponent<CharacterController>();
        }
        //cam.Player = currentLeader.transform;
        for(int i = 0; i < units.Count; i++)
        {
            if( i != currentLeaderIndex)
            {
                units[i].Leader = currentLeader.transform;
                units[i].formOffset = units[i].UnitInfo.formOffset[i];
                units[i].navMeshAgent.isStopped = false;
            } else
            {
                units[i].Leader = null;
                units[i].formOffset = Vector3.zero;
                units[i].navMeshAgent.isStopped = true;
            }
        }
        Debug.Log($"리더 변경 : " + currentLeader.gameObject.name); 






    }
}
