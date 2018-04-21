/* Abstract: Controlls instantiation of floating damage text
 */

using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour {
	private static FloatingText popupText;
	private static GameObject canvas;

	public static void Initialize(){
		canvas = GameObject.Find("World Canvas");
		//canvas = Resources.Load<GameObject>("Prefabs/UI/FloatingDamageCanvas");
		if (!popupText) {
			popupText = Resources.Load<FloatingText> ("Prefabs/UI/FloatingDamage");
		}
	}

	public static void CreateFloatingText(string dmg, Transform location, float height, Color clr = default(Color), int size = 20){
		FloatingText instance = Instantiate (popupText);
		instance.transform.SetParent (canvas.transform, false);
		//instance.transform.position = (location.position + Vector3.up);
		instance.transform.position = new Vector3 (location.position.x, location.position.y + height, location.position.z);

		instance.SetText (dmg);
		if (clr != default(Color))
			instance.SetColor (clr);
		if (size != null)
			instance.SetSize (size);
	}
}