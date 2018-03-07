using UnityEngine;
using System.Collections;

public class collissionDetec : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnCollisionEnter(Collision collision) {
		print (collision.gameObject.name);
		gameObject.SendMessageUpwards ("CollisionHappened",collision , SendMessageOptions.DontRequireReceiver);
	}
}
