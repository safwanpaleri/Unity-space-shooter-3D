//unity headers.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //creating variables.
    [SerializeField] Transform Target;
    NavMeshAgent navMeshAgent;

    [SerializeField] float Range = 10f;
    float distanceToTarget = Mathf.Infinity;

    [SerializeField] int Damage = 10;
    [SerializeField] float turningSpeed = 5f;

    public bool isProvoked = false;
    bool dead;

    void Start()
    {
        //referencing NavMeshAgent which is a unity feature which helps in movement of objects.
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //constantly checking whether the enemy is alive.
        dead = GetComponent<EnemyHealth>().IsDead();
        //and if not alive, do nothing and return.
        if (dead) { return; }
        //finding distance between the player and this enemy.
        distanceToTarget = Vector3.Distance(Target.position, transform.position);
        //if player reaches within the range of this enemy
        if (isProvoked)
        {
            //engaging the target
            EngageTarget();
        }
        //checking whether the player is within the range.
        else if (distanceToTarget <= Range)
        {
            isProvoked = true;
        }
    }
    
    //function for engaging the target
    private void EngageTarget()
    {
        //set direction and rotation towards the enemies position.
        lookTarget();
        
        //the enemy follows the player within a certain distance between them.
        //if the distance between them is larger than the stopping distance.
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            // follow the targert.
            navMeshAgent.SetDestination(Target.position);
            GetComponent<Animator>().SetBool("Attack", false);
            GetComponent<Animator>().SetTrigger("Move");
            
        }
        //if the enemy is within the shooting range(stopping distance).
        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            //attack the player
            GetComponent<Animator>().SetBool("Attack", true);
        }
    }
    
    //function responsible for adjusting rotation and direction of the enemy ship.
    private void lookTarget()
    {
        Vector3 direction = (Target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turningSpeed);
    }

    //function for decreasing enemy's health.
    public void DecreasePlayerHealth()
    {
        Target.GetComponent<PlayerCollision>().SetisDead();
    }

    //Debug purpose to see the activation range of enemy.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
