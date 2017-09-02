using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerJumping: MonoBehaviour {

	private Rigidbody2D rb;
	private Collider2D col;

	[SerializeField]
	private float speed;
	[SerializeField]
	private float jumpVel;
	private bool grounded;

	public float stdFallMulti;
	public float jumpingFallMulti;

    [Range(0.01f, 5.0f)]
    public float jumpTravel = 2.2f;

    [Range(0.01f, 10.0f)]
    public float jumpSpeed = 2.5f;

    [Range(0.01f, 5.0f)]
    public float curveCutoff = 3.5f;

	public LayerMask groundMask;

	void Start () {
		grounded = true;
		rb = GetComponent<Rigidbody2D> ();
		col = GetComponent<BoxCollider2D> ();
	}


	void Update () {
		// Help to visualize the raycast checking the ground
		Debug.DrawRay (transform.position, Vector3.down, Color.red);

		Move ();

		if (Input.GetKeyDown (KeyCode.Space) && Grounded()) {
            //Jump ();
            StartCoroutine("JumpCurve");
		}

		// Gravity adjustment to improve platforming
		if (rb.velocity.y < 0) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * (stdFallMulti - 1) * Time.deltaTime;
		} else if (rb.velocity.y > 0 && !Input.GetKey (KeyCode.Space)) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpingFallMulti - 1) * Time.deltaTime;
		}
	}


	private void Move() {
		rb.velocity = new Vector2 (Input.GetAxis ("Horizontal") * speed, rb.velocity.y);
	}

    private IEnumerator JumpCurve()
    {
        float time = (10.0f - jumpSpeed) / Mathf.Pow(10.0f, jumpTravel);
        float curveVel = jumpTravel / time;

        while (Input.GetKey(KeyCode.Space) && curveVel > 3.5f)
        {
            Debug.Log(curveVel);
            rb.velocity = new Vector2(rb.velocity.x, curveVel);
            time += Time.deltaTime;
            curveVel = jumpTravel / time;
            yield return null;
        }

        rb.velocity = new Vector2(rb.velocity.x, Vector2.down.y * 0.01f);
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
