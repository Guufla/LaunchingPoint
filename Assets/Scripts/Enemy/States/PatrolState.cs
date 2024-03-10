using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;
    public float waitTimer;
    private Vector3 destination;

    public override void Enter()
    {
        while (enemy.target == null)
        {
            enemy.target = enemy.players[Random.Range(0, enemy.players.Count-1)];
        }
        destination = enemy.GetDestination(enemy.target);
        enemy.agent.SetDestination(destination);
        //waypointIndex = FindClosestWaypoint();
        //enemy.agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
    }

    public override void Perform()
    {
        //PatrolCycle();

        // If a player is in sight, target the player
        foreach (GameObject player in enemy.players)
        {
            if (player == null) continue;

            if (enemy.CanSee(player))
            {
                if (enemy.target != player)
                {
                    enemy.agent.SetDestination(enemy.transform.position); // stop moving
                }
                enemy.target = player;
                stateMachine.ChangeState(new AttackState());
            }
        }

        // If the target would not be seen by the new destination, make a new destination
        if (!enemy.WouldSee(enemy.target, destination))
        {
            destination = enemy.GetDestination(enemy.target);
            enemy.agent.SetDestination(destination);
        }
    }

    public override void Exit()
    {

    }


    /*
    public void PatrolCycle() 
    {
        // implement patrol logic
        if (enemy.agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > 3)
            {
                if (waypointIndex < enemy.path.waypoints.Count - 1)
                    waypointIndex++;
                else
                    waypointIndex = 0;
                
                enemy.agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
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
            if (Vector3.Distance(enemy.path.waypoints[i].position, enemy.agent.transform.position) < 
                Vector3.Distance(enemy.path.waypoints[closest].position, enemy.agent.transform.position))
            {
                closest = i;
            }
        }
        return closest;
    }   
    */
}
