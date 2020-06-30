using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAccelerationMovement: MonoBehaviour
{

	public float moveSpeedMax;
	[SerializeField] private float moveSpeed = 0f;
	public float acceleration;
	public float deceleration;
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
	[SerializeField] private bool playerAccelerating;
	[SerializeField] private bool playerDecelerating;
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
		moveSpeed = 0;
		playerAccelerating = false;
		playerDecelerating = false;
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
				if ((Input.GetAxisRaw("Horizontal") > 0.5f) && (moveSpeed < moveSpeedMax))
				{
					
					moveSpeed = moveSpeed + (acceleration * Time.deltaTime);
					playerAccelerating = true;
				}
				else if ((Input.GetAxisRaw("Horizontal") < 0.5f) && (moveSpeed > -moveSpeedMax))
				{
					moveSpeed = moveSpeed - (acceleration * Time.deltaTime);
					playerAccelerating = true;
				}
				currentMoveSpeed = moveSpeed;
				myRigidbody2D.velocity = new Vector2(currentMoveSpeed, myRigidbody2D.velocity.y);
				playerMoving = true;
				currentState = PlayerState.walk;
				lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
			}

			if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
			{
				if ((Input.GetAxisRaw("Vertical") > 0.5f) && (moveSpeed < moveSpeedMax))
				{
					
					moveSpeed = moveSpeed + (acceleration * Time.deltaTime);
					playerAccelerating = true;
				}
				else if ((Input.GetAxisRaw("Vertical") < 0.5f) && (moveSpeed > -moveSpeedMax))
				{
					moveSpeed = moveSpeed - (acceleration * Time.deltaTime);
					playerAccelerating = true;
				}
				currentMoveSpeed = moveSpeed;
				//transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
				myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, currentMoveSpeed);
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

			if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f && Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.5f)
			{
				if (moveSpeed < moveSpeedMax)
				{
					moveSpeed = moveSpeed + (acceleration * Time.deltaTime);
					playerAccelerating = true;
				}
				else if (moveSpeed > -moveSpeedMax)
				{
					moveSpeed = moveSpeed - (acceleration * Time.deltaTime);
					playerAccelerating = true;
				}
				currentMoveSpeed = moveSpeed;
				currentMoveSpeed = currentMoveSpeed * diagonalMoveModifier;
				lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			}
			else
			{
				/*
				if ((moveSpeed > deceleration * Time.deltaTime) && (playerMoving == false))
				{
					moveSpeed = moveSpeed + (deceleration * Time.deltaTime);
					playerDecelerating = true;
					playerAccelerating = false;
					if (lastMove.x > 0.5f || lastMove.x < -0.5f)
					{
						myRigidbody2D.velocity = new Vector2(currentMoveSpeed, myRigidbody2D.velocity.y);
					}
					else if (lastMove.y > 0.5f || lastMove.y < -0.5f)
					{
						myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, currentMoveSpeed);
					}
				}
				else if ((moveSpeed < -deceleration) && (playerMoving == false))
				{
					moveSpeed = moveSpeed - (deceleration * Time.deltaTime);
					playerDecelerating = true;
					playerAccelerating = false;
				}
				*/
				if (playerMoving == false)
				{
					moveSpeed = 0;
					playerAccelerating = false;
					playerDecelerating = false;
				}
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
			myRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
			currentState = PlayerState.idle;
		}
	}
}
