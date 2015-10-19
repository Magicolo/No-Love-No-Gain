using UnityEngine;
using System.Collections;

public static class AnimatorExtentions {

	public static bool isPlaying(this Animator animator){
		return animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0);
	}
}
