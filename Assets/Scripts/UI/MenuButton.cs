using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

	public Text textField;
	public Color mainColor;
	public AudioClip hoverSound;
	public AudioClip clickSound;

	public void Start() {
		mainColor = textField.color;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		textField.color = Color.white;

		if (hoverSound != null)
			Camera.main.gameObject.GetComponent<AudioSource> ().PlayOneShot (hoverSound, 1.2f);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		textField.color = mainColor;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (hoverSound != null)
			Camera.main.gameObject.GetComponent<AudioSource> ().PlayOneShot (clickSound, 1.2f);
		
		textField.color = mainColor;
	}
}
