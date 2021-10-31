using UnityEngine;

public class PlayerMovementController : Movement {
	private void FixedUpdate() {
		MoveWithInput(InputVectorMovement());
	}
private Vector2 InputVectorMovement () {
		float moveHorizontal = 0;
		float moveVertical = 0;
		moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
		//Use the two store floats to make movement vector
		return new Vector2(moveHorizontal, moveVertical);
	}
}
