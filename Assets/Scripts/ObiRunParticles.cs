using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObiRunParticles : MonoBehaviour
{

    public float xPivotDistance;
    public float yPivotDistance;
    
    private Transform target;
    private Animator animator;
    private bool showParticles;
    private bool moveDiagonal;
    private float moveX;
    private float moveY;
    private Vector2 appearenceSpot;
	
    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("MainCharacter").GetComponent<Transform>();
    }
	
    // Update is called once per frame
    void Update()
    {
        showParticles = false;
        moveDiagonal = false;

        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            showParticles = true;
            appearenceSpot = new Vector2(target.position.x - xPivotDistance, target.position.y + yPivotDistance);
            transform.position = appearenceSpot;
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            showParticles = true;
            appearenceSpot = new Vector2(target.position.x + xPivotDistance, target.position.y + yPivotDistance);
            transform.position = appearenceSpot;
        }
        
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            moveDiagonal = true;
        }
        else if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            moveDiagonal = true;
        }

        animator.SetBool("MoveDiagonal", moveDiagonal);
        animator.SetBool("ShowParticles", showParticles);
        animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
    }
}