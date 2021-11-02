using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flames : MonoBehaviour
{
    [Header("Movement Controls")]
    //Player Movement Variables
	[Range(0, 20)] [SerializeField] private float playerSpeed = 3f;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

	//[SerializeField] private Joystick movementJoystick;

	private Rigidbody2D m_Rigidbody2D;
	private Vector3 m_Velocity = Vector3.zero;

    public Vector3 targetPos;

    private void Start(){
        GameObject[] environments = GameObject.FindGameObjectsWithTag("Environment");     
        
        foreach (GameObject environment in environments){
            Debug.Log("Ignoring");
            if(environment.GetComponent<Collider2D>() != null){
                Physics2D.IgnoreCollision(environment.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
        }
    }

    void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // targetPos = GameObject.FindWithTag("EndGoal").transform.position;
		MoveWithInput((targetPos- transform.position).normalized);
    }

    private void MoveWithInput(Vector2 movement){
        // playerAnim.SetFloat("speedMovement", movement.magnitude);

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
    }

    private void Die () {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Rigidbody2D rB = other.gameObject.GetComponent<Rigidbody2D>();
        if (rB) { //kill only rigidbodies
            if (other.gameObject.GetComponent<Player>())
                other.gameObject.GetComponent<Player>().Die();
        }
    }
}
