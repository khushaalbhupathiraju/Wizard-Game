using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestEnemy : MonoBehaviour
{
    [Header("References:")]
    public Transform player;
    public Animator animator;
    public Transform[] spawnPoints;

    [Header("Detection:")]
    public float wakeRange = 10f;

    [Header("Movement: ")]
    public float moveSpeed = 4f;
    public float rotationSpeed = 8f;

    [Header("Attack Ranges:")]
    public float longAttackRange = 6f;
    public float closeAttackRange = 2f;

    [Header("Animation Duration:")]
    public float wakeDuratiob = 1.2f;
    public float alertDuration = 1.0f;
    public float longAttackDuration = 1.1f;
    public float closeAttackDuration= 0.9f;
    public float longAttackCoolDown = 2f;
    public float closeAttackCoolDown = 1.2f;

    [Header("Teleport:")]
    public float teleportDistace = 30f;
    public float teleportDelay = 3f;
    public float teleportRadius = 4f;

    private bool hasSpottedPlayer;
    private bool canChase;
    private bool isAttacking;

    private float nextAttackTime;
    private float farTimer;

    void Start()
    {
        animator.SetInteger("State", 0);
    }

    void Update()
    {
        if(player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if(!hasSpottedPlayer && distance <= wakeRange)
        {
            hasSpottedPlayer = true;
            StartCoroutine(WakeSequence());
            return;
        }
        if(hasSpottedPlayer)
            FacePlayer();

        if(!canChase || isAttacking)
        {
            return;
        }
        
        if(distance <= closeAttackRange)
        {
            if(Time.time >= nextAttackTime)
            {
                StartCoroutine(CloseAttack());
            }
            return;
        }

        if(distance <= longAttackRange)
        {
            if(Time.time >= nextAttackTime)
            {
                StartCoroutine(LongAttack());
            }
            return;
        }

        animator.SetInteger("State", 3);
        
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        transform.position += direction * moveSpeed *Time.deltaTime;

        HandleTeleport(distance);
    }

    IEnumerator WakeSequence()
    {
        canChase = false;
        
        animator.SetInteger("State", 1);
        yield return new WaitForSeconds(wakeDuratiob);

        animator.SetInteger("State", 2);
        yield return new WaitForSeconds(alertDuration);

        animator.SetInteger("State", 3);
        canChase = true;
    }

    IEnumerator LongAttack()
    {
        isAttacking = true;
        animator.SetInteger("State", 4);
        nextAttackTime = Time.time+longAttackCoolDown;

        yield return new WaitForSeconds(longAttackDuration);

        animator.SetInteger("State", 3);
        isAttacking = false;
    }

    IEnumerator CloseAttack()
    {
        isAttacking = true;
        animator.SetInteger("State", 5);
        nextAttackTime = Time.time + closeAttackCoolDown;

        yield return new WaitForSeconds(closeAttackDuration);
        animator.SetInteger("State", 3);
        isAttacking = false;
    }

    void FacePlayer()
    {
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0f;

        if(lookDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void HandleTeleport(float distance)
    {
        if(distance > teleportDistace)
        {
            farTimer += Time.deltaTime;

            if(farTimer >= teleportDelay)
            {
                TeleportNearPlayer();
                farTimer = 0f;
            }
        }
        else
        {
            farTimer = 0f;
        }
    }

    void TeleportNearPlayer()
    {
        if(spawnPoints.Length == 0)
            return;

        Transform closestPoint = spawnPoints[0];
        float closestDistance = Vector3.Distance(player.position, closestPoint.position);

        foreach(Transform point in spawnPoints)
        {
            float distance = Vector3.Distance(player.position, point.position);

            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        transform.position = closestPoint.position;
    }

}
