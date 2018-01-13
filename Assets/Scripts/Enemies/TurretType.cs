﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretType : Enemy {
	float hitpoints;
	string type;

	string[] listOfTypes = {"fire", "water", "earth", "metal"};

	private Transform playerTransform;
	private Transform enemyTransform;
	private float turretRadius = 10.0f; //How far our turret enemies can see
	private EnemyShooting es;
	LineRenderer line;


	void Awake(){
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		es = gameObject.GetComponent<EnemyShooting>();
	}

	public override void takeDamage(float amount)
	{
		StartCoroutine (damage (amount));
	}

	public string getEnemyType()
	{
		return type;
	}

	public float getEnemyHitPoints()
	{
		return hitpoints;
	}

	void Start()
	{
		//change this temp to randomize type. Random.range for int is exclusive for last interger.
		int temp = Random.Range (0, 4);
		type = listOfTypes [temp];

		hitpoints = Mathf.Floor(Random.Range (5f, 11f));


		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		line = this.gameObject.GetComponent<LineRenderer>();
		rb.constraints = RigidbodyConstraints2D.FreezeAll;

	}

	void Update()
	{
		if (hitpoints <= 0)
			Destroy (this.gameObject);

		guard_Area ();
	}

	void guard_Area(){
		if (Vector3.Distance (transform.position, playerTransform.position) < turretRadius) { //If player is in range of turret
			//line.positionCount (2);
			line.enabled = true;
			line.SetPosition (0, transform.position);
			line.SetPosition (1, playerTransform.position);
			es.shoot_At_Player (); //Shoot um up
		} else {
			line.enabled = false;
		}
	}

	//Return distance between player and enemy
	private float Distance(){
		return Vector3.Distance(enemyTransform.position, playerTransform.position);
	}

	IEnumerator damage(float amount)
	{
		hitpoints -= amount;
		yield return new WaitForSeconds (1);
	}
}
