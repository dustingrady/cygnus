using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour {

	private Rigidbody2D rb;
	private Collider2D col;
	private Player playerScript; // Reference to player script where elemental abilities live
	[SerializeField]
	private Animator playerAnim;

	[SerializeField]
	private float baseSpeed = 7;
	[SerializeField]
	private float sprintMulti = 1.4f;
	private float speed;

	[SerializeField]
	private float gravity = 30;

	private bool grounded;

    [Range(0.01f, 5.0f)]
    public float jumpTravel = 1.8f;

    [Range(0.01f, 10.0f)]
    public float jumpSpeed = 6.0f;

    [Range(0.01f, 5.0f)]
    public float curveCutoff = 3.5f;

	public LayerMask groundMask;

	void Start () {
		grounded = true;
		rb = GetComponent<Rigidbody2D> ();
		col = GetComponent<BoxCollider2D> ();
		playerScript = GetComponent<Player> ();
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

		// Jumping
		if (Input.GetKeyDown (KeyCode.Space) && Grounded()) {
            StartCoroutine("JumpCurve");
		}

		// Sprinting
		if (Input.GetKey (KeyCode.LeftShift) && Grounded ()) {
			speed = baseSpeed * sprintMulti;
		} else if (Grounded()) {
			speed = baseSpeed;
		}
	}


	void FixedUpdate() {
		rb.AddForce(Vector3.down * gravity * rb.mass); // Add more weight to the player
		Move ();
	}
		

	private void Move() {
		rb.velocity = new Vector2 (Input.GetAxis ("Horizontal") * speed, rb.velocity.y);

		if (playerAnim != null) {
			//Debug.Log (Input.GetAxis ("Horizontal"));

			if (Input.GetAxis ("Horizontal") > 0) {
				transform.localScale = new Vector3 (1, transform.localScale.y, transform.localScale.z);
			} else if (Input.GetAxis ("Horizontal") < 0) {
				transform.localScale = new Vector3 (-1, transform.localScale.y, transform.localScale.z);
			}

			if (Grounded() && Input.GetAxis("Horizontal") > 0.001 || Input.GetAxis("Horizontal") < -0.001) {
				playerAnim.SetInteger ("State", 1);
			}
			else if (Grounded() && Input.GetAxis("Horizontal") <= 0.001 && Input.GetAxis("Horizontal") >= -0.001) {
				Debug.Log ("Going into idle state");
				playerAnim.SetInteger ("State", 0);
			}
		}
	}
		

    private IEnumerator JumpCurve()
    {
        float time = (10.0f - jumpSpeed) / Mathf.Pow(10.0f, jumpTravel);
        float curveVel = jumpTravel / time;

        while (Input.GetKey(KeyCode.Space) && curveVel > curveCutoff)
        {
            rb.velocity = new Vector2(rb.velocity.x, curveVel);
            time += Time.deltaTime;
            curveVel = jumpTravel / time;
			yield return new WaitForFixedUpdate ();
        }

        rb.velocity = new Vector2(rb.velocity.x, Vector2.down.y * 0.01f);
    }


	private bool Grounded() {
		if (JumpCasts()) {
			// On the ground
			return true;
		} else {
			// In the air
			return false;
		};
	}


	private bool JumpCasts() {
		Vector3[] castPos = new Vector3[] { transform.position, 
			new Vector3 (transform.position.x - col.bounds.extents.x + 0.01f, transform.position.y, transform.position.z),
			new Vector3 (transform.position.x + col.bounds.extents.x - 0.01f, transform.position.y, transform.position.z)
		};

		bool validJump = false;
		foreach (Vector3 pos in castPos) {
			if (Physics2D.Raycast (pos, Vector2.down, col.bounds.extents.y + 0.1f, groundMask).collider != null) {
				validJump = true;
			}
		}

		return validJump;
	}
}
