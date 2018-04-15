using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoom : MonoBehaviour {

	[System.Serializable]
	public class ListWrapper
	{
		public List<GameObject> eList = new List<GameObject>();
	}

	//public List<GameObject> l = new List<GameObject>();
	public List<ListWrapper> theList = new List<ListWrapper>();
	public bool summon = false;
	public float timer = 0;
	public float spawnRotation = 10f;
	int listIndex = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (summon) {
			if (listIndex < theList.Count && timer <= 0.5f) {
				for(int i = 0; i < theList[listIndex].eList.Count; i++) {
					if (!theList[listIndex].eList[i].activeInHierarchy) {
						theList[listIndex].eList[i].SetActive (true);
					}
				}
			}

			timer += Time.deltaTime;

			if (timer >= spawnRotation) {
				listIndex++;
				timer = 0;
			}
			/*
			foreach (GameObject g in l) {
				if (!g.activeInHierarchy) {
					g.SetActive (true);
				}
			}
			//Destroy (this.gameObject, 2f);
			*/
		}
	}

	void OnTriggerEnter2D(Collider2D col)	{
		if (col.gameObject.name == "Player") {
			summon = true;
		}
	}
}
