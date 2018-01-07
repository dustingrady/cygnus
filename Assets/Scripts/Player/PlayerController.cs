using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour {

	private Rigidbody2D rb;
	private Collider2D col;

	[SerializeField]
	private float sprintMulti = 1.4f;
	[SerializeField]
	private float moveForce = 100f;
	[SerializeField]
	private float maxSpeed = 5f;
	[SerializeField]
	private float baseSpeed = 5f;
	[SerializeField]
	private float grappleSpeed = 1f;

	[SerializeField]
	private float gravity = 20;

	private bool grounded;

	public bool grapplingLeft = false;
	public bool grapplingRight = false;

    [Range(0.01f, 5.0f)]
    public float jumpTravel = 1.8f;

    [Range(0.01f, 10.0f)]
    public float jumpSpeed = 3.0f;

    [Range(0.01f, 5.0f)]
    public float curveCutoff = 3.0f;

	public LayerMask groundMask;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		col = GetComponent<BoxCollider2D> ();
	}


	void Update () {
		// ------------- Visualize groundcheck rays -----------------------------
		Vector3[] castPos = new Vector3[] { transform.position, 
			new Vector3 (transform.position.x - col.bounds.extents.x + 0.1f, transform.position.y, transform.position.z),
			new Vector3 (transform.position.x + col.bounds.extents.x - 0.1f, transform.position.y, transform.position.z)
		};
		foreach (Vector3 pos in castPos) {
			Debug.DrawRay (pos, Vector3.down, Color.red);
		}
		// ------------- Visualize groundcheck rays -----------------------------

		// Jumping while grappled left
		if (Input.GetButtonDown("Jump") && grapplingLeft) {
			rb.AddForce(Vector3.left * 100);
			StartCoroutine("JumpCurve");
		}

		// Jumping while grappled right
		else if (Input.GetButtonDown("Jump") && grapplingRight) {
			rb.AddForce(Vector3.right * 100);
			StartCoroutine("JumpCurve");
		}

		// Normal jump
		else if (Input.GetButtonDown("Jump") && JumpCheck()) {
            StartCoroutine("JumpCurve");
		}

		// Sprinting
		if (Input.GetKey (KeyCode.LeftShift) && JumpCheck()) {
			maxSpeed = baseSpeed * sprintMulti;
		} else if (JumpCheck()) {
			maxSpeed = baseSpeed;
		}

	}


	void FixedUpdate() {
		if (grapplingLeft || grapplingRight) {
			GrappleMove ();
		} else {
			rb.AddForce (Vector3.down * gravity * rb.mass); // Add more weight to the player
			Move ();
		}
	}
		

	private void Move() {
		float h = Input.GetAxis("Horizontal");

		if (Mathf.Abs (rb.velocity.x) < maxSpeed || Mathf.Sign(h) != Mathf.Sign(rb.velocity.x))
			rb.AddForce(Vector2.right * h * moveForce);
	}
		

	private void GrappleMove() {
		if (Input.GetKey (KeyCode.W)) {
			rb.velocity = Vector3.up * grappleSpeed;
		} else if (Input.GetKey (KeyCode.S)) {
			rb.velocity = Vector3.down * grappleSpeed;
		} else {
			rb.velocity = Vector3.zero;
		}
	}


    private IEnumerator JumpCurve()
    {
        float time = (10.0f - jumpSpeed) / Mathf.Pow(10.0f, jumpTravel);
        float curveVel = jumpTravel / time;

		while (Input.GetButton("Jump") && curveVel > curveCutoff)
        {
			if (Mathf.Abs(rb.velocity.y) < Mathf.Abs(curveVel)) {
				rb.velocity = new Vector2(rb.velocity.x, curveVel);
			}
            time += Time.fixedDeltaTime;
            curveVel = jumpTravel / time;
			yield return new WaitForFixedUpdate ();
        }

		if (Mathf.Abs(rb.velocity.y) < Mathf.Abs(curveVel)) {
			rb.velocity = new Vector2(rb.velocity.x, Vector2.down.y * 0.01f);
		}

    }


	private bool JumpCheck() {
		Vector3[] castPos = new Vector3[] { transform.position, 
			new Vector3 (transform.position.x - col.bounds.extents.x + 0.005f, transform.position.y, transform.position.z),
			new Vector3 (transform.position.x + col.bounds.extents.x - 0.005f, transform.position.y, transform.position.z)
		};

		bool validJump = false;
		foreach (Vector3 pos in castPos) {
			if (Physics2D.Raycast (pos, Vector2.down, col.bounds.extents.y + 0.2f, groundMask).collider != null) {
				validJump = true;
			}
		}

		return validJump;
	}


	public void StartGrapple(string side) {
		if (side == "left") {
			grapplingLeft = true;
		} else {
			grapplingRight = true;
		}

		rb.gravityScale = 0;
		rb.velocity = Vector3.zero;
	}


	public void StopGrapple() {
		grapplingRight = false;
		grapplingLeft = false;
		rb.gravityScale = 1;
	}

	void OnCollisionExit2D(Collision2D col) {
		StopGrapple ();
	}
}
