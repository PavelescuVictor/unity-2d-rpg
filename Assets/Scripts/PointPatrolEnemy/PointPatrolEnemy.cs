using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PointPatrolEnemy : MonoBehaviour
{

    public float speed;
    public float startWaitTime;
    public GameObject enemyPatrolArea;

    private int randomSpot;
    private Vector2 minWalkPoint;
    private Vector2 maxWalkPoint;
    private Vector2 centerPoint;

    private float waitTime;
    private Vector2 nextPoint;

    private Animator animator;

    [SerializeField]
    private bool enemyMoving;
    [SerializeField] 
    private float moveX;
    [SerializeField]
    private float moveY;
    [SerializeField]
    private float lastMoveX;
    [SerializeField]
    private float lastMoveY;

    private Collider2D moveCollider;

    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        moveCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();   
        waitTime = startWaitTime;
        if (enemyPatrolArea != null)
        {
            minWalkPoint = enemyPatrolArea.GetComponent<BoxCollider2D>().bounds.min;
            maxWalkPoint = enemyPatrolArea.GetComponent<BoxCollider2D>().bounds.max;
            centerPoint = (Vector2)enemyPatrolArea.GetComponent<BoxCollider2D>().bounds.center;
        }

        nextPoint = ChooseDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckCollisions(nextPoint, distance) == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, nextPoint, speed * Time.deltaTime);
            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);

            if (Vector2.Distance(transform.position, nextPoint) < 0.2f)
            {
                if (waitTime <= 0)
                {
                    lastMoveX = moveX;
                    lastMoveY = moveY;
                    nextPoint = ChooseDestination();
                    waitTime = startWaitTime;
                    enemyMoving = true;
                }
                else
                {
                    enemyMoving = false;
                    waitTime -= Time.deltaTime;
                }
            }
        }
        else
        {
            lastMoveX = moveX;
            lastMoveY = moveY;
            nextPoint = ChooseDestination();
            waitTime = startWaitTime;
            enemyMoving = true;
        }
        animator.SetFloat("LastMoveX", lastMoveX);
        animator.SetFloat("LastMoveY", lastMoveY);
        animator.SetBool("EnemyMoving", enemyMoving);
    }

    private Vector2 ChooseDestination()
    {

        if (nextPoint.x < centerPoint.x)
        {
            nextPoint.x = Random.Range(centerPoint.x, maxWalkPoint.x);
            moveX = 1.0f;
        }
        else
        {
            nextPoint.x = Random.Range(minWalkPoint.x, centerPoint.x);
            moveX = -1.0f;
        }

        if (nextPoint.y < centerPoint.y)
        {
            nextPoint.y = Random.Range(centerPoint.y, maxWalkPoint.y);
            moveY = 1.0f;
        }
        else
        {
            nextPoint.y = Random.Range(minWalkPoint.y, centerPoint.y);
            moveY = -1.0f;
        }

        if (moveX < 0 || moveY < 0 || moveX < 0 && moveY < 0)
        {
            distance = distance * (-1);
        }

        return nextPoint;
    }

    private bool CheckCollisions(Vector2 direction, float distance)
    {
        if (moveCollider != null)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            ContactFilter2D filter = new ContactFilter2D();
            {
                
            };
            int numHits = moveCollider.Cast(direction, filter, hits, distance);
            for (int i = 0; i < numHits; i++)
            {
                if (!hits[i].collider.isTrigger)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
