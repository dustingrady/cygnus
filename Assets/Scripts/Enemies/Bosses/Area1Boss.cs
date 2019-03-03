using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Area1Boss : MonoBehaviour {
	[SerializeField]
	private GameObject baseAttackPrefab;
	[SerializeField]
	private GameObject specialAttackPrefab;
	[SerializeField]
	private GameObject nade;
	public GameObject exit; 
	public GameObject shipPiece;

	public CinemachineVirtualCamera cam;
	public bool expandingCamera = false;
	private float originalCameraSize;
	public Sprite phaseThreeSprite;

	private Transform enemyTransform;
	private Vector3 startingpos;
	private int baseAttackCount = 0;
	private int specialAttackCount = 0;
	private int cooldown = 50;
	private int specialOffset = 0;
	private int moveCount = 0; //What move are we on again?
	public GameObject player;
	private float maxHealth;
	public float rotationSpeed = 5f;
	bool moving = false;
	Vector3 currentVel = Vector3.zero;
	Vector3 offset;

	private AudioController ac;
	enum attackState{
		healthy,
		halfhealth,
		neardeath
	}

	attackState atkState;
	void Awake(){
		enemyTransform = this.transform; //Reference to current enemy
		startingpos = transform.position;
		maxHealth = gameObject.GetComponent<BossEnemy> ().getHealth ();
		StartCoroutine(changeTar());
		GameObject camera = GameObject.Find("Main Camera");
		ac = camera.GetComponent<AudioController>();

		cam = GameObject.Find ("FollowCM").gameObject.GetComponent<CinemachineVirtualCamera> ();
		expandingCamera = true;
		originalCameraSize = cam.m_Lens.OrthographicSize;
	}

	void moveaninch(){
		//if (!moving) {
		transform.position = Vector3.SmoothDamp (transform.position, offset, ref currentVel, 50/rotationSpeed, 50/rotationSpeed *5,Time.deltaTime);
			//moving = true;
		//}
	}

	void Update () {
		
		determineState ();

		if (atkState == attackState.neardeath) {
			moveaninch ();
			//transform.RotateAround (startingpos, Vector3.forward, rotationSpeed * Time.deltaTime);
		}

		if (gameObject.GetComponent<BossEnemy> ().getHealth () <= 0) {
			exit.SetActive (true);
			ac.source.Stop ();
			ac.source.clip = ac.audio [0]; 
			ac.source.Play ();
		}

		if (expandingCamera) {
			cam.m_Lens.OrthographicSize += 1.5f * Time.deltaTime;

			if (cam.m_Lens.OrthographicSize > 12) {
				expandingCamera = false;
				//this.gameObject.SetActive (false);
			}
		}
	}

	void determineState(){
		if ((gameObject.GetComponent<BossEnemy> ().getHealth () / maxHealth) > 0.8f) {
			atkState = attackState.healthy;
		}
		if ((gameObject.GetComponent<BossEnemy> ().getHealth () / maxHealth) <= 0.8f && (gameObject.GetComponent<BossEnemy> ().getHealth () / maxHealth) > 0.3f) {
			atkState = attackState.halfhealth;
		}
		if ((gameObject.GetComponent<BossEnemy> ().getHealth () / maxHealth) <= 0.3f) {
			atkState = attackState.neardeath;
		}
	}

	/*Determine which attack to use*/
	public void Determine_Attack(){
		moveCount++;
		if (moveCount % 5 == 0) {
			Special_Attack ();
		} 
		Base_Attack ();
	}

	/*Base attack (in progress)*/
	private void Base_Attack(){
		if(baseAttackCount >= cooldown){
			if (atkState == attackState.healthy) {
				cooldown = 75;
				specialOffset = 150;
				GameObject bullet = (GameObject)Instantiate (baseAttackPrefab, enemyTransform.position, enemyTransform.rotation);
				bullet.GetComponent<BulletBehaviour> ().setBullet ("single");
				Destroy (bullet, 10.0f);
				baseAttackCount = 0;
			}
			if (atkState == attackState.halfhealth) {
				cooldown = 75;
				specialOffset = 100;
				for (float i = -2; i <= 2; i+=0.5f) {
					GameObject bullet = (GameObject)Instantiate (baseAttackPrefab, enemyTransform.position, enemyTransform.rotation);
					bullet.GetComponent<BulletBehaviour> ().setBullet ("spread");
					bullet.transform.LookAt (new Vector3(player.transform.position.x + i*3f, player.transform.position.y + i*3f, 0));
				}

				baseAttackCount = 0;
			}
			if (atkState == attackState.neardeath) {
				cooldown = 75;
				specialOffset = 50;
				StartCoroutine(changeTar());
				GameObject grenade = Instantiate(nade, enemyTransform.position, enemyTransform.rotation) as GameObject;
				Vector2 dir = (player.transform.position - transform.position);
				dir = new Vector2 (dir.x, dir.y + 2).normalized;
				grenade.GetComponent<Rigidbody2D> ().AddForce (dir * 5f);
				baseAttackCount = 0;
			}
		}
		baseAttackCount++;
	}

	/*Special attack (in progress)*/
	private void Special_Attack(){
		if(specialAttackCount >= (cooldown + specialOffset)){
			GameObject bullet = (GameObject)Instantiate (specialAttackPrefab, enemyTransform.position, enemyTransform.rotation);
			Destroy (bullet, 10.0f);
			specialAttackCount = 0;
		}
		specialAttackCount++;
	}

	IEnumerator changeTar(){
		offset = new Vector3(Random.Range(-3,3), Random.Range(-5,5), 0);
		offset = startingpos + offset;
		yield return new WaitForSeconds (3f);
	}
}
