using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestEnemy : MonoBehaviour
{
    public Transform player;

    public float detectRange = 10f;
    public float senseRange = 6f;
    public float chaseRange = 3f;
    public float AttackRange = 1f;

    public float moveSpeed = 3f;

    private Animator anim;
    
    enum State
    {
        Closed = 0,
        Idle = 1,
        Sense = 2,
        Chase = 3,
        Attack = 4
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        /*float distance = Vector3.Distance(transform.position, player.position);
        State currentState;
        if(distance > detectRange)
            currentState = State.Closed;
        else if(distance > senseRange)
            currentState = State.Idle;
        else if(distance > chaseRange)
            currentState = State.Sense;
        else if(distance > AttackRange)
            Debug.Log("In range");
        else
            currentState = State.Chase;
        
        anim.SetInteger("State", (int)currentState);
        if(currentState == State.Chase)
        {
            ChasePlayer();
        }*/
    }

    /*void ChasePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        transform. position += dir * moveSpeed * Time.deltaTime;

        transform.LookAt(player);
    }*/
    
}
