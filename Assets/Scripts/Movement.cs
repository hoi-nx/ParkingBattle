using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public Transform sphereObject;
	  float speed;


	bool isParking1;
	bool isParking2;
	float _time1;
	public float _time2;
	// Use this for initialization
	void Start () {
	
		speed = Random.Range (20, 30);
	}
	
	// Update is called once per frame
	void Update () {
 
		if(!isParking1)
			transform.RotateAround(sphereObject.position, Vector3.up*600, speed * Time.deltaTime);


		/*if (isParking2 && _time2<1.0f) {
			transform.Translate (Vector3.back * Time.deltaTime * .5f * speed, Space.Self);//(sphereObject.position, Vector3.up*600, speed * Time.deltaTime);
			_time2 = _time1- Time.time;
			_time2 = Mathf.Abs(_time2);

		}*/


		if (Input.GetKeyDown (KeyCode.Space))
				stopCar ();



	}

	void stopCar()
	{
		transform.GetChild(0).parent = null;
 		parkCar ();
	}

	void parkCar()
	{
		 /*transform.Rotate (0,90,0);
		_time1 = Time.time  ;
		isParking2 = true;*/
 		iTween.RotateAdd(gameObject , iTween.Hash("y",90,"time",.2,"oncompletetarget", gameObject , "oncomplete", "afterRot",  "easeType",iTween.EaseType.easeInOutBounce  ));
 
	}

	void afterRot()
	{
		isParking1 = true;

		iTween.MoveAdd(gameObject , iTween.Hash("z",-8,"time",.3,"oncompletetarget", gameObject , "oncomplete", "afterParking",  "easeType",iTween.EaseType.easeOutQuad  ));
	}


	void afterParking()
	{
		GameObject.Find ("Game Manager").SendMessage ("goodParking",SendMessageOptions.DontRequireReceiver);
		print ("eeee");
	}










}
