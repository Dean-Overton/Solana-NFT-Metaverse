using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
	[Header("Movement Controls")]
    //Player Movement Variables
	[Range(0, 20)] [SerializeField] private float movementSpeed = 3f;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
	[SerializeField] private bool m_FacingRight = true;

	//[SerializeField] private Joystick movementJoystick;

	private Rigidbody2D m_Rigidbody2D;
	private Vector3 m_Velocity = Vector3.zero;
	private Animator playerAnim;
	private bool collidingTerrain = false;

	void Awake()
    {
		//Need intances of player components on game start.
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		playerAnim = GetComponent<Animator>();
    }
	public void MoveWithInput(Vector2 movement)
	{
		if (!collidingTerrain) //Animate only if not hitting terrain
			playerAnim.SetFloat("speedMovement", movement.magnitude);
		else
			playerAnim.SetFloat("speedMovement", 0f);

		if(Mathf.Abs(movement.magnitude) > 0) {
			Vector3 targetVelocity = new Vector2(movement.x * movementSpeed, m_Rigidbody2D.velocity.y);

			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		} else {
			m_Rigidbody2D.velocity = Vector3.zero;
		}

		if (movement.x != 0)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(movement.x * movementSpeed, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
		}
		//If moving on y
		if (movement.y != 0)
		{
			// movement the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(m_Rigidbody2D.velocity.x, movement.y * movementSpeed);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		}

		//Ensures players movement matches with direction players facing.
		if (movement.x > 0 && !m_FacingRight)
			{
				Flip();
			}
			else if (movement.x < 0 && m_FacingRight)
			{
				Flip();
			}
		}
	private void Flip()
	{
		m_FacingRight = !m_FacingRight;

		transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
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
