using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    private Rigidbody2D rb;

    private float baseSpeed = 250;

    private bool grounded;

    string levelName = "";
    bool onTop = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }

        if (onTop)
        {
            if (Input.GetKeyDown("e"))
            {
                Debug.Log("Entering level " + levelName);
                SceneManager.LoadScene(levelName);
            }
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
        Debug.Log("Entering collision with " + col.gameObject.name);
        onTop = !onTop;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        levelName = "";
        Debug.Log("Exiting collision with " + col.gameObject.name);
        onTop = !onTop;
    }
}
