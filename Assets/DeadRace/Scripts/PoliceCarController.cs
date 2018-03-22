using UnityEngine;
using System.Collections;

public class PoliceCarController : MonoBehaviour
{

		public float turnSpeed;
		public float carSpeed;
		public float tilt;
		public float[] limits;
		public Transform carBody;
		public Transform[] wheelObjs;
		public float wheelSpeed;
		public float magnetPowerTime = 3.0f;
		private float nextFire;
		public static float isDoubleSpeed = 1;
		Transform thisTrans ;
		public static Vector3 thisPosition ;
		float brakeSpeed = 0;
		playerCarControl playercarScript;
		public AudioSource policeCarSound;

#region enum Declaration

		public  enum RoadLanes
		{
				one = 0,
				two,
				three,
				four
		}
		;
		public enum policeCarSpeed
		{
				originalSpeed,
				stopSpeed
		}
		;
		public policeCarSpeed currentSpeed;

		public enum carStates
		{
				OnRoad ,
				OnAir,
				OnAirWait,
				idle
		
		}
		;
		public enum policeCarMode
		{
				avoid,
				aggressive,
				idle
		}
		;
		public policeCarMode currentCarMode;
		public   carStates currentCarState ;
		public  RoadLanes  currentCarLane,lastCarLane ;

#endregion

		void OnEnable ()
		{
				PoliceCarinstantiation_Back();
				int randomValue_Mode = Random.Range (0, 8);
				thisTrans = transform;
				isDoubleSpeed = 1;
				playercarScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<playerCarControl> ();
				currentCarState = carStates.OnRoad;
				currentCarMode = policeCarMode.avoid;
				BlueLight_Enable ();
		      	InvokeRepeating ("PoliceCarMode_Change", 10f, 20f);

		policeCarSound.volume = 0.3f;
			

		}

#region Car LightsBlinking

		public GameObject blueLight, redLight;

		void BlueLight_Enable ()
		{
				blueLight.GetComponent<Renderer>().enabled = true;
				redLight.GetComponent<Renderer>().enabled = false;
				Invoke ("RedLight_Enable", 0.2f);
		}

		void RedLight_Enable ()
		{
				blueLight.GetComponent<Renderer>().enabled = false;
				redLight.GetComponent<Renderer>().enabled = true;
				Invoke ("BlueLight_Enable", 0.2f);
		}

#endregion

		RaycastHit hit;
		float lastTime,distance;

		void Update ()
		{
		  //the difference between policar and player 
				distance = thisTrans.position.z - playercarScript.thisTrans.position.z;
				if (distance >= 80) {
						currentSpeed = policeCarSpeed.stopSpeed;//stop increasing speed if police is infront of the player 
				} else if (distance < 0) {
					 
						currentSpeed = policeCarSpeed.originalSpeed;//if police car is back to player ,increase it's speed 
						//PoliceCarinstantiation_Back ();
				
				}  
				switch (currentSpeed) {
				case policeCarSpeed.originalSpeed:
						carSpeed = 6;
						break;
				case policeCarSpeed.stopSpeed:
						carSpeed = 3;
						break;
				}
				switch (currentCarMode) {
				case policeCarMode.avoid:
					policeCarAvoidMode_Front ();
						break;
				case policeCarMode.aggressive:
						policeCarAggressiveMode ();
						break;
				case policeCarMode.idle:
						break;
				}
				if (distance < 0) {
			policeCarSound.volume = 0.3f;
				} else if(distance>0 && distance<=30) {
			policeCarSound.volume = 1f;
				}else{
			policeCarSound.volume = 0.3f;
				}

		}

#region Police car Mode Change	

	void PoliceCarMode_Change()
	{
		if (distance >= 10 && distance <= 30) {
			Debug.Log ("Mode Changing");
			currentCarMode = policeCarMode.aggressive;
			Invoke ("PoliceCarMode_PreviousMode", 2.0f);
		}
	}
	void PoliceCarMode_PreviousMode()
	{
		currentCarMode = policeCarMode.avoid;
	}

#endregion

#region Police Car Collision & Trigger

		void OnTriggerEnter (Collider c)
		{
				if (c.GetComponent<Collider>().name.Contains ("Trigger")) {
						currentCarState = carStates.OnAir;
				}
		}
	void OnCollisionEnter (Collision incomingCollision)
	{
		string incTag = incomingCollision.collider.tag;
		if (incTag.Contains ("trafficCar") || incTag.Contains("policeCar")|| incTag.Contains("Player")) {
			carSpeed = 0;
		
			wheelSpeed = 0;
			isDoubleSpeed = 0;
			turnSpeed = 0;

			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			GameObject trafficCar = incomingCollision.collider.gameObject; 
			trafficCar.SendMessage ("StopCar", SendMessageOptions.DontRequireReceiver);//to stop the car
			//iTween.RotateTo (trafficCar,new Vector3(0,0,0), 1.0f);
			Destroy(gameObject,3f);
			policeCarSound.volume = 0;
			currentCarState = carStates.idle;
			currentCarMode = policeCarMode.idle;
		}
	}

#endregion
	
		float moveHorizontal ;
		public float lanePosition, targetLanePosition ;
		public float[] LanesPositions ;
		public float laneShiftSpeed ;
		bool canFly;

		void FixedUpdate ()
		{
				thisPosition = thisTrans.position;
				if (GamePlayController.isGameEnded)
						return;
				switch (currentCarState) {
				case carStates.OnRoad:
						carOnRoad ();
						break;
				case carStates.OnAir:
						CarFlying ();
						currentCarState = carStates.OnAirWait;
						break;
				case carStates.OnAirWait:
						GetComponent<Rigidbody>().position = new Vector3 (lanePosition, carJump_Height, GetComponent<Rigidbody>().position.z);
						break;
				case carStates.idle:
						break;
			
				}


		}
	
#region Car On Road State

		float carYPosition;

		void carOnRoad ()
		{
				switch (currentCarLane) {
			
				case RoadLanes.one:
						targetLanePosition = LanesPositions [0];
						break;
				case RoadLanes.two:
						targetLanePosition = LanesPositions [1];
						break;
				case RoadLanes.three:
						targetLanePosition = LanesPositions [2];
						break;
				case RoadLanes.four:
						targetLanePosition = LanesPositions [3];
						break;
				}
				lanePosition = Mathf.Lerp (lanePosition, targetLanePosition, laneShiftSpeed);

				foreach (Transform t in wheelObjs) {
			
						t.Rotate (wheelSpeed * Time.deltaTime, 0, 0);
				}
				#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
	 
				#endif
		
				#if UNITY_EDITOR || UNITY_WEBPLAYER
	  
				#endif
				Vector3 movement = new Vector3 (moveHorizontal, 0.0f, (carSpeed * isDoubleSpeed));
				GetComponent<Rigidbody>().velocity = movement * turnSpeed;
				GetComponent<Rigidbody>().position = new Vector3
					(
						Mathf.Clamp (lanePosition, limits [0], limits [1]),
						0.0f, GetComponent<Rigidbody>().position.z
				);
				GetComponent<Rigidbody>().rotation = Quaternion.Euler (0, GetComponent<Rigidbody>().velocity.x * tilt, 0.0f);
				carBody.rotation = Quaternion.Euler (0, GetComponent<Rigidbody>().velocity.x * tilt, -GetComponent<Rigidbody>().velocity.x * tilt / 2);
		}
#endregion

#region Car On Air State

		public float carJump_Height;

		void CarFlying ()
		{
				iTween.ValueTo (gameObject, iTween.Hash ("from", carJump_Height, "to", 7, "time", 0.8f, "onupdate", "ChangeHeight"));
				iTween.RotateTo (gameObject, iTween.Hash ("rotation", new Vector3 (-8, 0, 0), "time", 0.8f, "oncomplete", "afterReachingTop"));
		}

		void afterReachingTop ()
		{
				iTween.ValueTo (gameObject, iTween.Hash ("from", carJump_Height, "to", 0, "time", 0.5f, "onupdate", "ChangeHeight", "oncomplete", "changeCarState"));
				iTween.RotateTo (gameObject, new Vector3 (-0, 0, 0), 0.5f);
		}
	
		void ChangeHeight (float newHeight)
		{
				carJump_Height = newHeight;
		}
	
		public void changeCarState ()
		{
				currentCarState = carStates.OnRoad;
		}
#endregion

		bool justOnce ;
		bool isVisible;
		
		void OnBecameVisible ()
		{
				isVisible = true;
				CancelInvoke ("DestroyPoliceCar");
		}

		void OnBecameInvisible ()
		{
			if(!isVisible) Invoke ("DestroyPoliceCar", 0);
		}

		void OnDisable ()
		{
				GamePlayController.Static.policeCarPositionIndex = -1;
		}

		void DestroyPoliceCar ()
		{
			Destroy (gameObject, 3);
		}

#region Police Car Avoid Mode from front

		void policeCarAvoidMode_Front ()
		{
				Vector3 fwd = thisTrans.TransformDirection (Vector3.forward);
				if (Physics.Raycast (transform.position, fwd, out hit, 50) && Time.timeSinceLevelLoad - lastTime > 2.0f) {
						Debug.DrawRay (thisTrans.position, Vector3.forward, Color.red);
						GameObject hitObj = hit.collider.gameObject;
						string hitObjTagName= hit.collider.tag;
						if ( hitObjTagName.Contains ("Player") || hitObjTagName.Contains ("trafficCar") || hitObjTagName.Contains ("PoliceCar") ) {//
							//Debug.Log("police car"+ hit.collider.name);
								lastTime = Time.timeSinceLevelLoad;
								int random = Random.Range (-1, 1);
				
								switch (currentCarLane) {
					
								case RoadLanes.one:
										currentCarLane = RoadLanes.two;
										break;
								case RoadLanes.two:
										if (random > 0) {
												currentCarLane = RoadLanes.three;
										} else {
												currentCarLane = RoadLanes.one;
										}
										break;
								case RoadLanes.three:
										if (random > 0) {
												currentCarLane = RoadLanes.four;
										} else {
												currentCarLane = RoadLanes.two;
										}
										break;
								case RoadLanes.four:
										currentCarLane = RoadLanes.three;
										break;
								}
						}
				}
		policeCarAvoidMode_Back ();
		}
#endregion

#region Police Car Avoid Mode from Back

	void policeCarAvoidMode_Back ()
	{
		Vector3 backWard = thisTrans.TransformDirection (Vector3.back);
		if (Physics.Raycast (transform.position, backWard, out hit, 50) && Time.timeSinceLevelLoad - lastTime > 2.0f) {
			Debug.DrawRay (thisTrans.position, Vector3.forward, Color.red);
			GameObject hitObj = hit.collider.gameObject;
			string hitObjTagName= hit.collider.tag;
			if ( hitObjTagName.Contains ("Player")) {
				lastTime = Time.timeSinceLevelLoad;
				int random = Random.Range (-1, 1);
				
				switch (currentCarLane) {
					
				case RoadLanes.one:
					currentCarLane = RoadLanes.two;
					break;
				case RoadLanes.two:
					if (random > 0) {
						currentCarLane = RoadLanes.three;
					} else {
						currentCarLane = RoadLanes.one;
					}
					break;
				case RoadLanes.three:
					if (random > 0) {
						currentCarLane = RoadLanes.four;
					} else {
						currentCarLane = RoadLanes.two;
					}
					break;
				case RoadLanes.four:
					currentCarLane = RoadLanes.three;
					break;
				}
			}
		}
	}

#endregion
	
#region police Car Aggressive Mode
		
		int laneChangeCount;
		void policeCarAggressiveMode ()
		{
			switch (playerCarControl.currentCarLane) {
				case playerCarControl.RoadLanes.one:
						if (currentCarLane == RoadLanes.four) {
								currentCarLane = RoadLanes.three;
						} else if (currentCarLane == RoadLanes.three) {
								currentCarLane = RoadLanes.two;	
						} else {
								currentCarLane = RoadLanes.one;	
						}

						break;
				case playerCarControl.RoadLanes.two:
						if (currentCarLane == RoadLanes.four) {
								currentCarLane = RoadLanes.three;	
						} else {
								currentCarLane = RoadLanes.two;
						}
		
						break;
				case playerCarControl.RoadLanes.three:
						if (currentCarLane == RoadLanes.one) {
								currentCarLane = RoadLanes.two;
						} else {
								currentCarLane = RoadLanes.three;	
						}
		
						break;
				case playerCarControl.RoadLanes.four:
						if (currentCarLane == RoadLanes.one) {
								currentCarLane = RoadLanes.two;
				
						} else if (currentCarLane == RoadLanes.two) {
								currentCarLane = RoadLanes.three;
				
						} else {
								currentCarLane = RoadLanes.four;	
						}
						break;
				}
		lastCarLane = currentCarLane;
		}
#endregion

#region select the lane for Police Car

	void PoliceCarinstantiation_Back()
	{

		switch (playerCarControl.currentCarLane) {
		case playerCarControl.RoadLanes.one:
			carSpeed = 3.0f;
				currentCarLane = RoadLanes.two;	
			break;
		case playerCarControl.RoadLanes.two:
			carSpeed = 3.0f;
				currentCarLane = RoadLanes.three;	
			break;
		case playerCarControl.RoadLanes.three:
			carSpeed = 3.0f;
			currentCarLane = RoadLanes.four;
			
			break;
		case playerCarControl.RoadLanes.four:
			carSpeed = 3.0f;
				currentCarLane = RoadLanes.three;
			break;
		}
	}

#endregion
}
