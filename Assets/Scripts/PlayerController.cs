using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour {

	private Rigidbody2D rb;
	private Collider2D col;

	[SerializeField]
	private float speed;
	[SerializeField]
	private float jumpVel;
	private bool grounded;

	public float stdFallMulti;
	public float jumpingFallMulti;

	public LayerMask groundMask;

	void Start () {
		grounded = true;
		rb = GetComponent<Rigidbody2D> ();
		col = GetComponent<BoxCollider2D> ();
	}


	void Update () {
		// Help to visualize the raycast checking the ground
		Debug.DrawRay (transform.position, Vector3.down, Color.red);

		

		if (Input.GetKeyDown (KeyCode.Space) && Grounded()) {
			Jump ();
		}
	}
  
  //Removed old FixedUpdate()

    void FixedUpdate()
    {
        Move();
        // Gravity adjustment to improve platforming
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (stdFallMulti - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpingFallMulti - 1) * Time.deltaTime;
        }
    }

	private void Move() {
		rb.velocity = new Vector2 (Input.GetAxis ("Horizontal") * speed, rb.velocity.y);
	}


	private bool Grounded() {
		if (Physics2D.Raycast (transform.position, Vector2.down, col.bounds.extents.y + 0.1f, groundMask).collider != null) {
			// On the ground
			return true;
		} else {
			// In the air
			return false;
		};
	}


	private void Jump() {
		rb.velocity = new Vector2 (rb.velocity.x, jumpVel);
	}
}
