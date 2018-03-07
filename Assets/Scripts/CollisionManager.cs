using UnityEngine;
using System.Collections;

public class CollisionManager : MonoBehaviour {
	public GameObject effect;
	// Use this for initialization
	void Start () {
	
	}
	

	void CollisionHappened (Collision collision  ) {
	
		foreach (Transform T in transform) {
			T.GetComponent<Rigidbody>().freezeRotation = true;
			T.GetComponent<Rigidbody>().isKinematic = true;
		}
		iTween.Stop (collision.gameObject);


		foreach (ContactPoint contact in collision.contacts) {
  			//Instantiate(effect , contact.thisCollider.transform.position +contact.otherCollider.transform.forward*4 , effect.transform.rotation);
			Instantiate(effect , contact.thisCollider.transform.position +contact.thisCollider.transform.up   , effect.transform.rotation);

		}

		GameObject.Find ("Game Manager").SendMessage ("badParking",SendMessageOptions.DontRequireReceiver);

	}
}
	