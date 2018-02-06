using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController2: MonoBehaviour {

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

    [Range(0.01f, 5.0f)]
    public float jumpTravel = 1.8f;

    [Range(0.01f, 10.0f)]
    public float jumpSpeed = 3.0f;

    [Range(0.01f, 5.0f)]
    public float curveCutoff = 3.0f;

	public LayerMask groundMask;

    public enum GrappleState
    {
        Left,
        Right,
        None
    }

    GrappleState grapple = GrappleState.None;

    void Start () {
		rb = GetComponent<Rigidbody2D> ();
		col = GetComponent<BoxCollider2D> ();

		string currentScene = SceneManager.GetActiveScene ().name;

		if (GameManager.instance.previousLocation != null) {
			string prevLoc = GameManager.instance.previousLocation;
			GameObject loc = GameObject.Find (prevLoc + "_spawn");
			if (loc != null) {
				transform.position = loc.transform.position;

				// Really dumb
				if (currentScene != "Ship") {
					Camera.main.transform.position = new Vector3 (transform.position.x, 
						transform.position.y, Camera.main.transform.position.z);
				}
			}
		}
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

        if (Input.GetButtonDown("Jump") && grapple != GrappleState.None)
        {
            rb.AddForce((grapple == GrappleState.Left ? Vector3.left : Vector3.right) * 100);
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
       if (grapple != GrappleState.None) {
            GrappleMove();
        } else {
            rb.AddForce(Vector3.down * gravity * rb.mass); // Add more weight to the player
            Move();
        }
	}
		

	private void Move() {
		float h = 0;

		if (GameManager.instance.controllerConnected) {
			h = Input.GetAxis ("Horizontal");
		} else {
			if (Input.GetKey (KeyCode.A)) {
				h = -1f;
			} else if (Input.GetKey (KeyCode.D)) {
				h = 1f;
			}
		}
        float speed = Mathf.Abs(rb.velocity.x);

		//float speedPercentage = speed / maxSpeed;

		if (speed < maxSpeed || Mathf.Sign (h) != Mathf.Sign (rb.velocity.x)) {
			rb.AddForce (Vector2.right * h * (moveForce - speed * 10.0f));
		}
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

		rb.velocity = new Vector2 (rb.velocity.x, Mathf.Lerp (rb.velocity.y, Vector3.down.y, 0.5f));
    }


	private bool JumpCheck() {
		Vector3[] castPos = new Vector3[] { transform.position, 
			new Vector3 (transform.position.x - col.bounds.extents.x + 0.005f, transform.position.y, transform.position.z),
			new Vector3 (transform.position.x + col.bounds.extents.x - 0.005f, transform.position.y, transform.position.z)
		};

		foreach (Vector3 pos in castPos) {
			if (Physics2D.Raycast (pos, Vector2.down, col.bounds.extents.y + 0.2f, groundMask).collider != null) {
                return true;
			}
		}

        return false;
	}


	public void StartGrapple(string side) {
		rb.gravityScale = 0;
		rb.velocity = Vector3.zero;
	}


	public void StopGrapple() {
		rb.gravityScale = 1;
	}

    public GrappleState Grapple{
        set
        {
            grapple = value;
            if (value == GrappleState.None)
            {
				gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            }
            else
            {
                rb.gravityScale = 0;
                rb.velocity = Vector3.zero;
            }
        }
    }

	void OnCollisionExit2D(Collision2D col) {
        //StopGrapple ();
        Grapple = GrappleState.None;
	}

	void OnCollisionEnter2D(Collision2D col) {
		StopCoroutine("JumpCurve");
	}
}
