using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionMovementController : MonoBehaviour
{

	public float normalSpeed;
	public float fastSpeed;
	public float maxDistance;
	public float minDistance;
	public float xPivotDistance;
	public float yPivotDistance;

	private Transform target;
	private Animator animator;
	private float moveX;
	private float moveY;
	private Vector2 flyingSpot;
	
	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator>();
		target = GameObject.FindGameObjectWithTag("MainCharacter").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Vector2.Distance(transform.position, target.position) > maxDistance)
		{
			flyingSpot = new Vector2(target.position.x - xPivotDistance, target.position.y + yPivotDistance);
			transform.position = Vector2.MoveTowards(
				transform.position,
				flyingSpot,
				fastSpeed * Time.deltaTime);
		}
		else if (Vector2.Distance(transform.position, target.position) < maxDistance &&
		         Vector2.Distance(transform.position, target.position) > minDistance)
		{
			if (target.position.x < transform.position.x)
			{
				flyingSpot = new Vector2(target.position.x + xPivotDistance, target.position.y + yPivotDistance);
			}
			else
			{
				flyingSpot = new Vector2(target.position.x - xPivotDistance, target.position.y + yPivotDistance);
			}
			transform.position = Vector2.MoveTowards(
				transform.position,
				flyingSpot, 
				normalSpeed * Time.deltaTime);
		}

		if (target.position.x < transform.position.x)
		{
			moveX = -1;
		}
		else if (target.position.x > transform.position.x)
		{
			moveX = +1;
		}

		if (target.position.y > transform.position.y)
		{
			moveY = +1;
		}

		animator.SetFloat("MoveX", moveX);
		animator.SetFloat("MoveY", moveY);
	}
}
