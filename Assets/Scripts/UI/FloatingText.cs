/* Abstract: Control lifecycle/ behavior of floating damage text
 */

using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {
	public Animator animator;
	private Text damageText;

	void Start(){
		AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo (0); //Get animation info
		Destroy(gameObject, clipInfo[0].clip.length); //Destroy object after the length of its animation
		damageText = animator.GetComponent<Text>();
	}

	public void SetText(float text){
		//damageText.text = string.Format("{0:N0}", text);
		animator.GetComponent<Text>().text = string.Format("{0:N0}", text);
	}
}
