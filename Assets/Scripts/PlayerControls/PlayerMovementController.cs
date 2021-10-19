using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    //Player Movement Variables
	[Range(0, 20)] [SerializeField] private float playerSpeed = 3f;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

	//[SerializeField] private Joystick movementJoystick;

	private Rigidbody2D m_Rigidbody2D;
	private Vector3 m_Velocity = Vector3.zero;
	private bool m_FacingRight = true;
	private Animator playerAnim;
	private bool collidingTerrain = false;

	void Awake()
    {
		//Get and store a reference to the Rigidbody2D component so that we can access it.
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		playerAnim = GetComponent<Animator>();
    }
	private float moveHorizontal;
	private float moveVertical;
	void FixedUpdate()
    {
#if UNITY_ANDROID
		//moveHorizontal = movementJoystick.Horizontal;
		//moveVertical = movementJoystick.Vertical;
#elif UNITY_EDITOR
		//Store the current horizontal input in the float moveHorizontal.
		moveHorizontal = Input.GetAxis("Horizontal");

        //Store the current vertical input in the float moveVertical.
        moveVertical = Input.GetAxis("Vertical");
#endif

		//Use the two store floats to make movement vector
		Vector2 movementVector = new Vector2(moveHorizontal, moveVertical);
		MoveWithInputVector(movementVector);
    }
	public void MoveWithInputVector(Vector2 move)
	{
		if (!collidingTerrain)
		{
			playerAnim.SetFloat("speedMovement", move.magnitude);
		} else
		{
			playerAnim.SetFloat("speedMovement", 0f);
		}

		//If moving on x
		if (move.x != 0)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move.x * playerSpeed, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		}
		//If moving on y
		if (move.y != 0)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(m_Rigidbody2D.velocity.x, move.y * playerSpeed);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		}

		// If the input is moving the player right and the player is facing left...
		if (move.x > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move.x < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
	// If the player should jump...
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		GetComponentInChildren<SpriteRenderer>().flipX = !GetComponentInChildren<SpriteRenderer>().flipX;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag =="Terrain")
		{
			collidingTerrain = true;
		}
	}
	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Terrain")
		{
			collidingTerrain = true;
		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Terrain")
		{
			collidingTerrain = false;
		}
	}
}
