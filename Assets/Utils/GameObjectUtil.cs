using UnityEngine;
using System.Collections;

public static class GameObjectUtil {

	public static void RemoveObjectFromAnimator(GameObject gameObject, Animator animator) {
		Transform parentTransform = gameObject.transform.parent;
		gameObject.transform.parent = null;
		float playbackTime = animator.playbackTime;
		animator.Rebind();
		animator.playbackTime = playbackTime;
		gameObject.transform.parent = parentTransform;
	}
}
