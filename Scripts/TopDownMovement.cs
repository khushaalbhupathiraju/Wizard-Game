using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [Header("Movement:")]
    private InputHandler _input;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;
    private bool rotateTowardsMouse;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundLayer;


    [Header("Refernce:")]
    [SerializeField]
    private Camera camera;

    [Header("Background:")]
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private bool canMove;
    private bool isGrounded;
    [SerializeField]
    bool slash;
    public float movementTime= 2f;
    public float swordTime = 2f;
    public GameObject swordObject;

    [Header("Weapon:")]
    public float attackRadius = 5f;
    public float damageAmount = 20f;
    public LayerMask enemyLayer;
    private Rigidbody rb;
    private bool wasGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _input = GetComponent<InputHandler>();
        anim = GetComponent<Animator>();
        canMove = true;
        slash= false;
    }

    private void Update()
    {
        isGrounded = IsGrounded();

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded && canMove )
        {
            Jump();
            anim.SetBool("Jump", true);
        }
        if(!wasGrounded && isGrounded)
        {
            anim.SetBool("Jump", false);
        }
        wasGrounded = isGrounded;
        /*if(rotateTowardsMouse)
            RotateTowardsMovementTarget(movementVector);
        else
            RotateTowardsMouseVector();*/
        if(slash == true)
        {
            SpellPower();
            GameObject swordSlash = Instantiate(swordObject,transform);

            Destroy(swordSlash, swordTime);
        }

        if(Input.GetKey(KeyCode.W) && canMove == true)
        {
            anim.SetFloat("VelocityY", 1, 0.05f, Time.deltaTime);
            if(Input.GetKey(KeyCode.F) && canMove == true)
            {
                canMove = false;
                StartCoroutine(LimitMovement());
                slash = true;
            }
        }
        else if(Input.GetKey(KeyCode.S) && canMove == true)
        {
            anim.SetFloat("VelocityY", 1f, 0.05f, Time.deltaTime);
            if(Input.GetKey(KeyCode.F) && canMove == true)
            {
                canMove = false;
                StartCoroutine(LimitMovement());
                slash = true;

            }
        }
        else if(Input.GetKey(KeyCode.A) && canMove == true)
        {
            anim.SetFloat("VelocityY", 1f, 0.05f, Time.deltaTime);
            if(Input.GetKey(KeyCode.F) && canMove == true)
            {
                canMove = false;
                StartCoroutine(LimitMovement());
                slash = true;
            }
        }
        else if(Input.GetKey(KeyCode.D) && canMove == true)
        {
            anim.SetFloat("VelocityY", 1f, 0.05f, Time.deltaTime);
            if(Input.GetKey(KeyCode.F) && canMove == true)
            {
                canMove = false;
                StartCoroutine(LimitMovement());
                slash = true;
            }
        }
        else if(Input.GetKey(KeyCode.F) && canMove == true)
        {
            canMove = false;
            StartCoroutine(LimitMovement());
            slash = true;
        }
        else
        {
            anim.SetFloat("VelocityX", 0);
            anim.SetFloat("VelocityY", 0);
            anim.SetBool("Attack", false);
            slash = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        if(canMove == true)
        {

            var movementVector = MoveTowardTarget(targetVector);
            RotateTowardsMovementTarget(movementVector);
        }
        else
        {
            rb.velocity = new Vector3(0,rb.velocity.y,0);
            return;
        }
    }

    void Jump()
    {
        
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    IEnumerator LimitMovement()
    {
        anim.SetFloat("VelocityX", 0);
        anim.SetFloat("VelocityY", 0);
        anim.SetBool("Attack", true);

        yield return new WaitForSeconds(movementTime);

        canMove = true;
    }

    public void SpellPower()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);
        foreach(var enemyCollider in hitColliders)
        {
            Vector3 directionToEnemy = (enemyCollider.transform.position - transform.position).normalized;

            float dotProduct = Vector3.Dot(transform.forward, directionToEnemy);

            if(dotProduct > 0.707f)
            {
                if(enemyCollider.TryGetComponent(out EnemyHealth health))
                {
                    health.TakeDamage(damageAmount);
                }
            }
        }
    }

    /*void RotateTowardsMouseVector()
    {
        Ray ray = camera.ScreenPointToRay(_input.MousePosition);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }*/

    private void RotateTowardsMovementTarget(Vector3 movementVector)
    {
        if(movementVector.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation,rotateSpeed);
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        //var speed = moveSpeed * Time.deltaTime;

        /*targetVector = Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;*/
        Vector3 movementDirection = Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0) * targetVector;

        movementDirection = movementDirection.normalized;
        float currentYVelocity = rb.velocity.y;

        if(!IsGrounded())
        {
            currentYVelocity += Physics.gravity.y * Time.fixedDeltaTime;
        }


        rb.velocity = new Vector3(movementDirection.x * moveSpeed,currentYVelocity,movementDirection.z * moveSpeed);

        return movementDirection;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.3f, groundLayer);
    }
}
