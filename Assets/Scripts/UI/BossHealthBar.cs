using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour {
	float currentHealthAmount;
	int totalHealthAmount;
	Text currentHealth;
	Text maxHealth;
	Image bossBar;

	bool enabled = false;

	void Start() {
		maxHealth = GameObject.Find ("BossMaxHealthText").GetComponent<Text>();
		currentHealth = GameObject.Find ("BossCurrentHealthText").GetComponent<Text>();
		bossBar = GameObject.Find ("BossBar").GetComponent<Image> ();
	}

	public void SetCurrentHealth(float health) {
		currentHealth.text = "" + health;
		currentHealthAmount = health;

		bossBar.fillAmount = (currentHealthAmount / totalHealthAmount);
	}

	public void Enable(int health) {
		enabled = true;
		currentHealthAmount = health;
		totalHealthAmount = health;

		currentHealth.text = "" + health;
		maxHealth.text = "" + health;
		bossBar.fillAmount = 1;
		transform.localScale = new Vector3 (1f, 1f, 1f);
	}

	public void Disable() {
		enabled = false;
		transform.localScale = new Vector3 (0f, 0f, 0f);
	}
}
