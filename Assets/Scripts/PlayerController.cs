using UnityEngine;
using System.Collections;
using Chronos;


public class PlayerController : BaseBehaviour 
{
	// Character movement speed values
	private float walkSpeed = 0.06f;
	private float runSpeed = 0.1f;
	private Vector3 pos;
	private Quaternion rot;

	protected bool canMove = true;

	protected float speedModifier = 0.0f;

	// Use this for initialization
	void Start () 
	{
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
		speedModifier = Timekeeper.instance.Clock("Player").localTimeScale;
		transform.rotation = rot;
		pos = transform.position;

		// Left, Right
		if (Input.GetKey(KeyCode.D))
		{
			pos.x += walkSpeed * speedModifier;
			transform.position = pos;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			pos.x -= walkSpeed * speedModifier;
			transform.position = pos;
		}

		// Running
		if (Input.GetKey(KeyCode.LeftShift))
		{
			// Left, Right
			if (Input.GetKey(KeyCode.D))
			{
				pos.x += runSpeed * speedModifier;
				transform.position = pos;
			}
			else if (Input.GetKey(KeyCode.A))
			{
				pos.x -= runSpeed * speedModifier;
				transform.position = pos;
			}
		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		pos = transform.position;
		if (other.gameObject.tag == "Chest")
		{
			other.transform.position = Vector2.Lerp(other.transform.position, new Vector2(other.transform.position.x - pos.x, other.transform.position.y), 1.0f);
		}
	}
}
