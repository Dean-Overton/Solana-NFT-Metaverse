using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
	//Player Movement Variables
	[Range(0, 20)] [SerializeField] private float speed = 3f;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

	private Rigidbody2D m_Rigidbody2D;
	private Vector3 m_Velocity = Vector3.zero;
	private bool m_FacingRight = true;
	private Animator nPCAnim;
	private bool collidingTerrain = false;

	private Vector3 targetPos;
	private float timeToWait = 10f;
	private float timeWaited;

	private bool pause = false;

	[SerializeField] private SpriteRenderer sR;

	void Awake()
	{
		//Get and store a reference to the Rigidbody2D component so that we can access it.
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		nPCAnim = GetComponent<Animator>();
	}
    private void Start()
    {
		targetPos = new Vector3(GameManager.current.player.transform.position.x + Random.Range(-5, 5), GameManager.current.player.transform.position.x + Random.Range(-5, 5), 0);

		GameEvents.current.onDialogueStart += OnDialogueStart;
		GameEvents.current.onDialogueEnd += OnDialogueEnd;
	}
    void FixedUpdate()
	{
		if (!pause)
		{
			if (Vector3.Distance(transform.position, targetPos) <= 1f)
			{
				nPCAnim.SetFloat("speedMovement", 0f);

				//Wait some time before resetting the targer position
				if (timeWaited < timeToWait)
				{
					timeWaited += Time.deltaTime;

					if (timeWaited >= timeToWait)
					{
						targetPos = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y + Random.Range(-10, 10), 0);
						timeWaited = 0f;
					}
				}
			}
			else
			{
				//Use the two store floats to make movement vector
				Vector2 movementVector = (targetPos - transform.position).normalized;
				Move(movementVector, false);
			}
		}
	}
	public void Move(Vector2 move, bool crouch)
	{
		if (!collidingTerrain)
		{
			nPCAnim.SetFloat("speedMovement", move.magnitude);
		}
		else
		{
			nPCAnim.SetFloat("speedMovement", 0f);
		}
		//If moving on x
		if (move.x != 0)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move.x * speed, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		}
		//If moving on y
		if (move.y != 0)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(m_Rigidbody2D.velocity.x, move.y * speed);
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
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		sR.flipX = !GetComponentInChildren<SpriteRenderer>().flipX;
	}
	void OnDialogueStart()
	{
		pause = true;
		nPCAnim.SetFloat("speedMovement", 0f);
	}
	void OnDialogueEnd ()
    {
		timeWaited = timeToWait;
		targetPos = new Vector3(transform.position.x + Random.Range(-30, 30), transform.position.x + Random.Range(-30, 30), 0);
		speed = 6.5f;
		pause = false;
	}
}
