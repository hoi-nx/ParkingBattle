using UnityEngine;
using System.Collections;

public class slopeController : MonoBehaviour {


	void Start () {
	
	}
	bool justOnce2 = false;
	
	void OnBecameInvisible() {
		//when destroying traffic car,make sure it is behind the player car
		if (!justOnce2) {
						//Debug.Log ("Slope value changed ");
						GamePlayController.Static.SlopeLanePositionIndex = -1;
						justOnce2 = true;		
				} 
						Destroy (gameObject, 1.0f);
			
		//print ("Slope is destroyed");
	}
}
