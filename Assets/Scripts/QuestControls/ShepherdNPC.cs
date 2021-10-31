using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShepherdNPC : MonoBehaviour
{
	[Header("Movement Controls")]
    //Player Movement Variables
	[Range(0, 20)] [SerializeField] private float playerSpeed = 3f;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

	//[SerializeField] private Joystick movementJoystick;

	private Rigidbody2D m_Rigidbody2D;
	private Vector3 m_Velocity = Vector3.zero;
	private bool m_FacingRight = true;
	private Animator playerAnim;
	private bool collidingTerrain = false;

    public Vector3 targetPos;

	private bool contactedByPlayer = false;

	void Awake()
    {
		//Need intances of player components on game start.
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		playerAnim = GetComponent<Animator>();
    }
	private void Start() {
		GameEvents.current.onDialogueStart += DialogueStart;
		GameEvents.current.onDialogueEnd += DialogueEnd;
	}
	void FixedUpdate()
    {
		// Instantiate(gameObject, )
		// targetPos = GameObject.FindWithTag("Meatworks").transform.position;
		if(!contactedByPlayer){
			MoveWithInput((targetPos- transform.position).normalized);
		}else{
			MoveWithInput(new Vector2(0, 0));
		}
    }
	private void MoveWithInput(Vector2 movement)
	{
		if (!collidingTerrain) //Animate only if not hitting terrain
			playerAnim.SetFloat("speedMovement", movement.magnitude);
		else
			playerAnim.SetFloat("speedMovement", 0f);

		if(Mathf.Abs(movement.magnitude) > 0) {
			Vector3 targetVelocity = new Vector2(movement.x * playerSpeed, m_Rigidbody2D.velocity.y);

			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		} else {
			m_Rigidbody2D.velocity = Vector3.zero;
		}

		if (movement.x != 0)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(movement.x * playerSpeed, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
		}
		//If moving on y
		if (movement.y != 0)
		{
			// movement the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(m_Rigidbody2D.velocity.x, movement.y * playerSpeed);
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
	public void DialogueStart () {
		contactedByPlayer = true;
	}
	public void DialogueEnd (string nameOfNPC) {
		if (nameOfNPC != gameObject.name)
            return;
		FindObjectOfType<QuestManager>().QuestComplete();
	}
	// If the player should jump...
	private void Flip()
	{
		m_FacingRight = !m_FacingRight;

		transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
		Tag tagScript;
		if (TryGetComponent<Tag>(out tagScript)) { //Because the player is flipping we need to ensure the text is still right way round
			tagScript.tagText.transform.localScale = new Vector3(tagScript.tagText.transform.localScale.x * -1f, tagScript.tagText.transform.localScale.y, tagScript.tagText.transform.localScale.z);
		}
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
