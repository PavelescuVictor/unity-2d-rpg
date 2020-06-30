using System.Collections;
using System.Collections.Generic;
//using System.Runtime.Remoting.Channels;
using System.Security;
using UnityEngine;

public class FrogMovementController : MonoBehaviour
{

    public float moveSpeed;
    public bool isMoving;
    public float walkTime;
    public float waitTime;
    public BoxCollider2D walkzone;

    private Vector2 minWalkPoint;
    private Vector2 maxWalkPoint;
    private bool hasWalkZone;

    private float walkCounter;
    private float waitCounter;
    private Rigidbody2D myRigidbody;
    private Animator animator;
    private float moveX;
    private float moveY;
    private float lastMoveX;
    private float lastMoveY;

    private int WalkDirection;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        waitCounter = waitTime;
        walkCounter = walkTime;

        ChooseDiraction();

        if (walkzone != null)
        {
            minWalkPoint = walkzone.bounds.min;
            maxWalkPoint = walkzone.bounds.max;
            hasWalkZone = true;
        }

    }
	
    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            walkCounter -= Time.deltaTime;
            lastMoveX = myRigidbody.velocity.x;
            lastMoveY = myRigidbody.velocity.y;
            
            switch (WalkDirection)
            {
                case 0:
                    myRigidbody.velocity = new Vector2(0,moveSpeed);
                    if (hasWalkZone && transform.position.y > maxWalkPoint.y)
                    {
                        isMoving = false;
                        waitCounter = waitTime;
                    }
                    break;
                case 1:
                    myRigidbody.velocity = new Vector2(moveSpeed, 0);
                    if (hasWalkZone && transform.position.x > maxWalkPoint.x)
                    {
                        isMoving = false;
                        waitCounter = waitTime;
                    }
                    break;
                case 2:
                    myRigidbody.velocity = new Vector2(0,-moveSpeed);
                    if (hasWalkZone && transform.position.y < minWalkPoint.y)
                    {
                        isMoving = false;
                        waitCounter = waitTime;
                    }
                    break;
                case 3:
                    myRigidbody.velocity = new Vector2(-moveSpeed, 0);
                    if (hasWalkZone && transform.position.x < minWalkPoint.x)
                    {
                        isMoving = false;
                        waitCounter = waitTime;
                    }
                    break;
            }
            
            if (walkCounter < 0)
            {
                isMoving = false;
                waitCounter = waitTime;
            }

            
        }
        else
        {
            waitCounter -= Time.deltaTime;
            myRigidbody.velocity = Vector2.zero;
            if (waitCounter < 0)
            {
                ChooseDiraction();
            }
        }
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("MoveX", myRigidbody.velocity.x);
        animator.SetFloat("MoveY", myRigidbody.velocity.y);
        animator.SetFloat("LastMoveX", lastMoveX);
        animator.SetFloat("LastMoveY", lastMoveY);
    }

    public void ChooseDiraction()
    {
        WalkDirection = Random.Range(0, 4);
        isMoving = true;
        walkCounter = walkTime;
    }
}