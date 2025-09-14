using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderController : MonoBehaviour
{
    private Unit unit;

    private CharacterController characterController;

    public Camera cam;

    public LayerMask enemyMask;

    public Team team = Team.Player;

    public void Awake()
    {
        unit = GetComponent<Unit>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
        Rotation();
    }

    void Move()
    {
        switch(team)    
        {
            case Team.Player:
                if(characterController == null)
                {
                    characterController = GetComponent<CharacterController>();
                    return;
                }
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");

                Vector3 move = new Vector3(h, 0, v).normalized;

                characterController.Move(move * unit.UnitInfo.moveSpeed * Time.deltaTime);
                break;
            
        }
    }

    void Rotation()
    {
        if(Time.timeScale < 1)
        {
            return;
        }
        switch(team)
        {
            case Team.Player:
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane plan = new Plane(Vector3.up, Vector3.zero);
                float rayLength;
                if(plan.Raycast(ray, out rayLength))
                {
                    Vector3 mousePoint = ray.GetPoint(rayLength);

                    this.transform.LookAt(new Vector3(mousePoint.x, transform.transform.position.y, mousePoint.z));
                }
                break;
        }
    }
}
