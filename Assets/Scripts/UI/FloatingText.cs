/* Abstract: Control lifecycle/ behavior of floating damage text
 */

using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {
	public Animator animator;

	void Start(){
		AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo (0); //Get animation info
		Destroy(gameObject, clipInfo[0].clip.length); //Destroy object after the length of its animation
	}

	public void SetText(string text){
		animator.GetComponent<Text>().text = string.Format("{0:N0}", text);
	}

	public void SetColor(Color clr) {
		animator.GetComponent<Text> ().color = clr;
	}

	public void SetSize(int size) {
		animator.GetComponent<Text> ().fontSize = size;
	}
}
