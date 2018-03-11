using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

	public Sprite replacement;
	private Sprite original;
	private Image cursorImg;

	public void Start() {
		cursorImg = GameObject.Find ("Reticle").GetComponent<Image>();
		original = cursorImg.sprite;
	}

	public void OnPointerEnter(PointerEventData eventData) {
		cursorImg.sprite = replacement;
	}

	public void OnPointerExit(PointerEventData eventData) {
		cursorImg.sprite = original;
	}

	public void OnPointerClick(PointerEventData eventData) {
		cursorImg.sprite = original;
	}


}
