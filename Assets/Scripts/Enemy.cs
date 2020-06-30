using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        PathFindingAI,
        PatrolAI,
        SleepAI,
        ScanAI,
        FollowAI
    };

    public EnemyType enemyType;

    public enum EnemyState
    {
        idle,
        walk,
        attack,
        stagger,
        sleeping,
        wakingUp,
        fallingAsleep
    }

    public EnemyState currentState;
    public float moveSpeed;
    public FloatValue maxHealth;
    [SerializeField] private float health;
    private Rigidbody2D myRigidBody2D;
    private GameObject target;
    public float chaseRadius;
    public float attackRadius;
    private bool enemyMoving;
    private bool enemyFallingAsleep;
    private bool enemyWakingUp;
    private bool enemySleeping;
    public float minTimeBeforeSleep;
    public float maxTimeBeforeSleep;
    [SerializeField]
    private float fallAsleepCounter;
    private bool waitingForSleep;

    private float moveX;
    private float moveY;
    private float lastMoveX;
    private float lastMoveY;
    private Vector2 lastPosition;
    private Vector2 currentPosition;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth.initialValue;
        enemyMoving = false;
        enemySleeping = false;
        enemyFallingAsleep = false;
        enemyWakingUp = false;
        waitingForSleep = false;
        moveX = 0f;
        moveY = 0f;
        lastMoveX = 0f;
        lastMoveY = 0f;
        lastPosition = transform.position;
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("MainCharacter");
        myRigidBody2D = GetComponent<Rigidbody2D>();
        currentState = EnemyState.idle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= chaseRadius)
        {
            if (currentState == EnemyState.sleeping)
            {
                float wakingUpTime = 0.70f;
                ChangeState(EnemyState.wakingUp);
                enemyWakingUp = true;
                enemySleeping = false;
                animator.SetBool("EnemyWakingUp", true);
                animator.SetBool("EnemySleeping", false);
                StartCoroutine(WakingUpCo(wakingUpTime));
            }
            else if (currentState == EnemyState.idle)
            {
                currentState = EnemyState.walk;
            }
            else if (currentState == EnemyState.walk  &&
                     Vector3.Distance(target.transform.position, transform.position) > attackRadius)
            {
                enemyMoving = true;
                animator.SetBool("EnemyMoving", true);
                CheckDirection(lastPosition);
                if (enemyType == EnemyType.FollowAI)
                {
                    FollowAI();
                }
            }
            float randomSleepTimer = UnityEngine.Random.Range(minTimeBeforeSleep, maxTimeBeforeSleep);
            fallAsleepCounter = randomSleepTimer;
        }
        else
        {
            if (currentState == EnemyState.walk)
            {
                currentState = EnemyState.idle;
                enemyMoving = false;
                animator.SetBool("EnemyMoving", false);
            }
            else if (currentState == EnemyState.idle)
            {
                if (fallAsleepCounter > 0)
                {
                    fallAsleepCounter -= Time.deltaTime;
                }
                else if (fallAsleepCounter <= 0)
                {
                    float fallAsleepTime = 0.70f;
                    ChangeState(EnemyState.fallingAsleep);
                    enemyFallingAsleep = true;
                    animator.SetBool("EnemyFallingAsleep", true);
                    StartCoroutine(FallingAsleepCo(fallAsleepTime));
                }
            }
        }
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);
        animator.SetFloat("LastMoveX", lastMoveX);
        animator.SetFloat("LastMoveY", lastMoveY);
        
        lastPosition = transform.position;
        lastMoveX = moveX;
        lastMoveY = moveY;
    }
    public void FollowAI()
    {
        Vector3 temp = Vector3.MoveTowards(transform.position, target.transform.position,
            moveSpeed * Time.deltaTime);
        myRigidBody2D.MovePosition(temp);  
    }

    public IEnumerator FallingAsleepCo(float fallingAsleepTime)
    {
        yield return new WaitForSeconds(fallingAsleepTime);
        currentState = EnemyState.sleeping;
        enemyFallingAsleep = false;
        enemySleeping = true;
        animator.SetBool("EnemySleeping", true);
        animator.SetBool("EnemyFallingAsleep", false);
    }
    
    public IEnumerator WakingUpCo(float wakingUpTime)
    {
        yield return new WaitForSeconds(wakingUpTime);
        currentState = EnemyState.walk;
        enemyWakingUp = false;
        enemyMoving = true;
        animator.SetBool("EnemyWakingUp", false);
        animator.SetBool("EnemyMoving", true);
    }
    public void CheckDirection(Vector2 lastPosition)
    {
        Vector2 direction = (Vector2)transform.position - lastPosition;
        moveX = (float)Math.Round(direction.x, 2);
        moveY = (float)Math.Round(direction.y, 2);
    }
    private void ChangeState(EnemyState newState)
    {
        if (this.currentState != newState)
        {
            this.currentState = newState;
        }
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void Knock(float knockTime, float damage)
    {
        StartCoroutine(KnockCo(knockTime));
        TakeDamage(damage);
    }
    
    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidBody2D != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidBody2D.velocity = Vector2.zero;
            currentState = EnemyState.idle;
        }
    }
}
