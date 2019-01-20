using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

	public Sprite replacement;
	private Sprite original;
	private Image cursorImg;

	PlayerShooting plrShooting;

	public void Start() {
		cursorImg = GameObject.Find ("Reticle").GetComponent<Image>();
		original = cursorImg.sprite;

		plrShooting = GameObject.FindWithTag ("Player").GetComponent<PlayerShooting>();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		cursorImg.sprite = replacement;

		if (plrShooting != null) {
			plrShooting.enabled = false;
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		cursorImg.sprite = original;

		if (plrShooting != null) {
			plrShooting.enabled = true;
		}
	}

	public void OnPointerClick(PointerEventData eventData) {
		cursorImg.sprite = original;

		if (plrShooting != null) {
			plrShooting.enabled = true;
		}
	}


}
