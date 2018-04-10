using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBossTest : MonoBehaviour {
	public List<Transform> bodyParts = new List<Transform> ();

	public float minDistance = 0.25f;
	public float speed = 1;
	public float rotationSpeed = 50;
	public int beginSize;

	public GameObject bodyPrefab;

	private float dis;
	private Transform curBodyPart;
	private Transform prevBodyPart;
	
	// Use this for initialization
	void Start () {
		for (int i = 0; i < beginSize - 1; i++) {
			addBodyPart ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	public void Move() {
		float curSpeed = speed;

		bodyParts [0].Translate (bodyParts [0].forward * curSpeed * Time.smoothDeltaTime, Space.World);

		if(Input.GetAxis("Horizontal") != 0) 
			bodyParts[0].Rotate(Vector3.up * rotationSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));

		for (int i = 1; i < bodyParts.Count; i++) {
			curBodyPart = bodyParts [i];
			prevBodyPart = bodyParts [i - 1];

			dis = Vector3.Distance (prevBodyPart.position, curBodyPart.position);

			Vector3 newPos = prevBodyPart.position;
			newPos.y = bodyParts [0].position.y;
			float T = Time.deltaTime * dis / minDistance * curSpeed;

			if (T > 0.5f) {
				T = 0.5f;
			}
			curBodyPart.position = Vector3.Slerp (curBodyPart.position, newPos, T);
			curBodyPart.rotation = Quaternion.Slerp (curBodyPart.rotation, prevBodyPart.rotation, T);
		}
	}

	public void addBodyPart() {
		Transform newPart = (Instantiate(bodyPrefab, bodyParts[bodyParts.Count - 1].position, bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;
		newPart.SetParent (transform);

		bodyParts.Add (newPart);
	}
}
