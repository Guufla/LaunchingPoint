using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;
    public float waitTimer;

    public override void Enter()
    {
        waypointIndex = FindClosestWaypoint();
        enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
    }

    public override void Perform()
    {
        PatrolCycle();
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    public override void Exit()
    {

    }

    public void PatrolCycle() 
    {
        // implement patrol logic
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > 3)
            {
                if (waypointIndex < enemy.path.waypoints.Count - 1)
                    waypointIndex++;
                else
                    waypointIndex = 0;
                
                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
                Debug.Log(enemy.path.waypoints[waypointIndex].position);
                waitTimer = 0;
            }
        }
    }

    public int FindClosestWaypoint()
    {   
        //assumes waypoints are not empty
        int closest = 0;
        for (int i = 1; i < enemy.path.waypoints.Count; i++)
        {
            if (Vector3.Distance(enemy.path.waypoints[i].position, enemy.Agent.transform.position) < 
                Vector3.Distance(enemy.path.waypoints[closest].position, enemy.Agent.transform.position))
            {
                closest = i;
            }
        }
        return closest;
    }   
}
