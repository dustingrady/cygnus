using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    private Rigidbody2D rb;
	[SerializeField]
	private Animator playerAnim;
    private float baseSpeed = 100;

    private bool grounded;
	private bool facingLeft = false;

    string levelName = "";
    bool onTop = false;

	void Awake() {
		string prevLoc = GameManager.instance.previousLocation;
		GameObject loc = GameObject.Find (prevLoc);

		if (loc != null) {
			transform.position = loc.transform.position;
		}
		playerAnim = GetComponent<Animator> ();
	}

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (onTop)
        {
            if (Input.GetKeyDown("e"))
            {
				GameManager.instance.previousLocation = SceneManager.GetActiveScene ().name;
                SceneManager.LoadScene(levelName);
            }
        }

		// Change the player's facing direction based on directional movement
		if (Input.GetAxis ("Horizontal") > 0) {
			transform.localScale = new Vector3 (0.5f, transform.localScale.y, transform.localScale.z);
			facingLeft = false;
		} else if (Input.GetAxis ("Horizontal") < 0) {
			transform.localScale = new Vector3 (-0.5f, transform.localScale.y, transform.localScale.z);
			facingLeft = true;
		}


		// Determine if the player is standing or running
		if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.001 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.001f) {
				playerAnim.SetInteger ("State", 1);
		}
		else if (Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.001 && Mathf.Abs(Input.GetAxis("Vertical")) <= 0.001f) {
				playerAnim.SetInteger ("State", 0);
		}
    }

    void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * baseSpeed * Time.deltaTime, Input.GetAxis("Vertical") * baseSpeed * Time.deltaTime);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        levelName = col.gameObject.name;
        onTop = !onTop;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        levelName = "";
        onTop = !onTop;
    }
}
