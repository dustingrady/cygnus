/* Abstract: Controlls instantiation of floating damage text
 */

using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour {
	private static FloatingText popupText;
	private static GameObject canvas;

	public static void Initialize(){
		canvas = GameObject.Find("FloatingDamageCanvas");
		//canvas = Resources.Load<GameObject>("Prefabs/UI/FloatingDamageCanvas");
		if (!popupText) {
			popupText = Resources.Load<FloatingText> ("Prefabs/UI/FloatingDamage");
		}
	}

	public static void CreateFloatingText(float dmg, Transform location){
		//Vector2 topOfPlayer = new Vector2(location.position.x, location.position.y + 1);
		Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x, location.position.y + 1));
		FloatingText instance = Instantiate (popupText);
		instance.transform.SetParent (canvas.transform, false);
		instance.transform.position = screenPosition;
		instance.SetText (dmg);
	}
}