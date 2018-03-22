using UnityEngine;
using System.Collections;

public class trafficCar : MonoBehaviour {

	// Use this for initialization

	Transform thisTrans;
	public float speed;
	public Transform[] wheelObjs;
	public float wheelTurnSpeed;
	public int  lanePosition ;
	void Start () {

		thisTrans = transform;
		
		#if UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8
//		foreach(Transform t in transform.GetComponentsInChildren<Transform>() )
//		{ if(t.name.Contains("Shadow") ) {
//				Debug.Log(t.name);
//				Destroy(t.gameObject);
//			}
//		}
		#endif
		thisTrans.localScale = new Vector3 (thisTrans.localScale.x, thisTrans.localScale.y, thisTrans.localScale.z * -1);
		//float scaleChangeFor_TCars;
		//scaleChangeFor_TCars =  thisTrans.localScale.z*-1;
		//thisTrans.localScale.z = scaleChangeFor_TCars;
		//print ("trafic car " + scaleChangeFor_TCars+" Scale "+thisTrans.localScale.z);
		gameObject.AddComponent<Rigidbody> ();
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
	}
	
	// Update is called once per frame
	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.GetComponent<Transform>().tag == null)  	return;
		if (collision.collider.tag.Contains ("trafficCar"))
		{
			Destroy(gameObject);
		}

	}
	void Update () {

		thisTrans.Translate( 0,0,-speed* Time.deltaTime );
		foreach (Transform  wheel in wheelObjs) {
			wheel.Rotate(wheelTurnSpeed*Time.deltaTime*2,0,0);
				}
	
	}

	bool justOnce2 = false;
	void OnBecameInvisible() {

	//	Debug.Log ("traffic became invisible");
		 if (!justOnce2) {
			 GamePlayController.Static.laneCarCount [lanePosition]--;

			if (GamePlayController.Static.laneCarCount [lanePosition] < 0)
				       GamePlayController.Static.laneCarCount [lanePosition] = 0;
			justOnce2 = true ;		

		//	Debug.Log ("reduced lane weight ");
		}
		 Destroy (gameObject, 2.0f);
		 
	}

	bool justOnce=false;
	 
	public void StopCar()
	{
		speed=0;
		wheelTurnSpeed=0;
	}
}
