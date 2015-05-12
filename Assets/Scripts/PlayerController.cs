using UnityEngine;
using System.Collections;
using Chronos;


public class PlayerController : BaseBehaviour 
{
	// Character movement speed values
	private float walkSpeed = 0.12f;
	private float runSpeed = 0.18f;
	private float diagonalWalkSpeed = 0f;
	private float diagonalRunSpeed = 0f;
	private Vector3 pos;
	private Quaternion rot;

	protected bool canMove = true;

	public LocalClock playerClock;


	// Use this for initialization
	void Start () 
	{
		playerClock = GetComponent<LocalClock> ();
		rot = transform.rotation;
	}

	void FixedUpdate () 
	{
		if (canMove)
		{
			MovementInputs ();
		}
	}

	// How the player moves the avatar
	private void MovementInputs()
	{
		transform.rotation = rot;
		pos = transform.position;
		diagonalWalkSpeed = walkSpeed * 2;
		diagonalRunSpeed = runSpeed * 2;

		// Left, Right, Up, Down
		if (Input.GetKey(KeyCode.W))
		{
			pos.y += walkSpeed;
			transform.position = pos;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			pos.y -= walkSpeed;
			transform.position = pos;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			pos.x += walkSpeed;
			transform.position = pos;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			pos.x -= walkSpeed;
			transform.position = pos;
		}

		// Diagonals
		if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
		{
			walkSpeed = 0.06f;
			pos.y += walkSpeed;
			pos.x += diagonalWalkSpeed;
			transform.position = pos;
		}
		else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
		{
			walkSpeed = 0.06f;
			pos.y += walkSpeed;
			pos.x -= diagonalWalkSpeed;
			transform.position = pos;
		}
		else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
		{
			walkSpeed = 0.06f;
			pos.y -= walkSpeed;
			pos.x += diagonalWalkSpeed;
			transform.position = pos;
		}
		else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
		{
			walkSpeed = 0.06f;
			pos.y -= walkSpeed;
			pos.x -= diagonalWalkSpeed;
			transform.position = pos;
		}
		else
		{
			walkSpeed = 0.12f;
		}

		// Running
		if (Input.GetKey(KeyCode.LeftShift))
		{
			// Left, Right, Up, Down
			if (Input.GetKey(KeyCode.W))
			{
				pos.y += runSpeed;
				transform.position = pos;
			}
			else if (Input.GetKey(KeyCode.S))
			{
				pos.y -= runSpeed;
				transform.position = pos;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				pos.x += runSpeed;
				transform.position = pos;
			}
			else if (Input.GetKey(KeyCode.A))
			{
				pos.x -= runSpeed;
				transform.position = pos;
			}
			
			// Diagonals
			if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
			{
				runSpeed = 0.09f;
				pos.y += runSpeed;
				pos.x += diagonalRunSpeed;
				transform.position = pos;
			}
			else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
			{
				runSpeed = 0.09f;
				pos.y += runSpeed;
				pos.x -= diagonalRunSpeed;
				transform.position = pos;
			}
			else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
			{
				runSpeed = 0.09f;
				pos.y -= runSpeed;
				pos.x += diagonalRunSpeed;
				transform.position = pos;
			}
			else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
			{
				runSpeed = 0.09f;
				pos.y -= runSpeed;
				pos.x -= diagonalRunSpeed;
				transform.position = pos;
			}
			else
			{
				runSpeed = 0.18f;
			}
		}

		// Time Control Test Movement
		if (Input.GetKey(KeyCode.LeftControl))
		{
			playerClock.localTimeScale = 0.5f;
		}
		else
		{
			playerClock.localTimeScale = 1;
		}
	}

//	void OnCollisionEnter2D (Collision2D other)
//	{
//		if (other.gameObject.isStatic)
//		{
//			canMove = false;
//		}
//	}
}
