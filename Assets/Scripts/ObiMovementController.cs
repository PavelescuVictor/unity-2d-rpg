﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObiMovementController : MonoBehaviour
{

	public float moveSpeed;
	private float currentMoveSpeed;
	public float diagonalMoveModifier;
	private Animator animator;
	private Rigidbody2D myRigidbody2D;
	private bool playerMoving;
	private bool playerAtacking;
	private Vector2 lastMove;
	public float attackTime;
	private float attackTimeCounter;
	public PlayerState currentState;
	public enum PlayerState
	{
		walk,
		attack,
		stagger,
		idle,
	}

	void Start()
	{
		currentState = PlayerState.idle;
		animator = GetComponent<Animator>();
		myRigidbody2D = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		playerMoving = false;
		if (currentState != PlayerState.attack)
		{
			playerAtacking = false;
		}
		if (currentState != PlayerState.attack && currentState != PlayerState.stagger)
		{
			currentState = PlayerState.idle;
			if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
			{
				//transform.Translate(new  Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
				
				myRigidbody2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * currentMoveSpeed,
					myRigidbody2D.velocity.y);
				playerMoving = true;
				currentState = PlayerState.walk;
				lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
			}

			if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
			{
				//transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
				myRigidbody2D.velocity =
					new Vector2(myRigidbody2D.velocity.x, Input.GetAxisRaw("Vertical") * currentMoveSpeed);
				playerMoving = true;
				currentState = PlayerState.walk;
				lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
			}

			if (Input.GetAxisRaw("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") > -0.5f)
			{
				myRigidbody2D.velocity = new Vector2(0f, myRigidbody2D.velocity.y);
			}

			if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f)
			{
				myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, 0f);
			}

			if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f &&
			    Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.5f)
			{
				currentMoveSpeed = moveSpeed * diagonalMoveModifier;
				lastMove = new Vector2(Input.GetAxisRaw("Horizontal"),
					Input.GetAxisRaw("Vertical"));
			}
			else
			{
				currentMoveSpeed = moveSpeed;
			}

			if (Input.GetButtonDown("Attack"))
			{
				playerAtacking = true;
				myRigidbody2D.velocity = Vector2.zero;
				animator.SetBool("PlayerAttacking", true);
				currentState = PlayerState.attack;
			}
		}
		else if (currentState == PlayerState.attack)
		{
			if (attackTimeCounter > 0)
			{
				attackTimeCounter -= Time.deltaTime;
			}

			if (attackTimeCounter <= 0)
			{
				animator.SetBool("PlayerAttacking", false);
				currentState = PlayerState.idle;
				attackTimeCounter = attackTime;
			}
		}

		animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
		animator.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
		animator.SetBool("PlayerMoving", playerMoving);
		animator.SetFloat("LastMoveX", lastMove.x);
		animator.SetFloat("LastMoveY", lastMove.y);
		
		
	}

	public void Knock(float knockTime)
	{
		StartCoroutine(KnockCo(knockTime));
	}
	
	private IEnumerator KnockCo(float knockTime)
	{
		if (myRigidbody2D != null)
		{
			yield return new WaitForSeconds(knockTime);
			myRigidbody2D.velocity = Vector2.zero;
			currentState = PlayerState.idle;
		}
	}
}
