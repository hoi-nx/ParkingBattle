using UnityEngine;
using System.Collections;

public class PoliceCar_Controller : MonoBehaviour {





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
	public Animator animator;

	public enum PoliceCarLane
	{
		one,two,three,four
	};
	public PoliceCarLane currentLane;

	public enum PoliceCarState
	{
		OnRoad,OnFly,OnAirWait,idle
	};
	public PoliceCarState currentState;

	public enum PoliceCarMode
	{
		Avoid,Aggressive,None
	};
	public PoliceCarMode currentMode;



	void OnEnable ()
	{
		PoliceCar_Back ();
		thisTrans = transform; // coping the game object transform values in to a local value 

		playercarScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<playerCarControl> (); 

		currentState = PoliceCarState.OnRoad; // to start police car state on road

		currentMode = PoliceCarMode.Avoid; // to start police car with avid Mode

		PoliceCar_Back (); // 

		InvokeRepeating ("PoliceCarMode_Change", 10f, 20f);// police car mode changing here

		policeCarSound.volume = 0.3f;
	}

	void Start () {
	
	}

	float difference;

	void Update () {

		// to calculate difference distance b/w player car position and police car position

		difference = thisTrans.position.z - playercarScript.thisTrans.position.z;


		if (difference >= 80) {
						carSpeed = 3;//stop increasing speed if police is infront of the player 
				} else if (difference <= 0) {
						PoliceCarInBack();//carspeed set to original speed 
				}

		//......................................



		// police car mode ..........................

		switch (currentMode) {
		case PoliceCarMode.Avoid:
			policeCarAvoidMode();
			break;
		case PoliceCarMode.Aggressive:
			policeCarAggressiveMode ();
			break;
		case PoliceCarMode.None:
			break;
		}

		//.............................


		//Police car sound......................
		if (difference < 0) {
			policeCarSound.volume = 0.3f;
		} else if(difference>0 && difference<=30) {
			policeCarSound.volume = 1f;
		}else{
			policeCarSound.volume = 0.3f;
		}
		//............................
	
	}

	#region Police Mode Changes to Avoid mode to Aggressive mode

	void PoliceCarMode_Change()
	{
		if (difference >= 10 && difference <= 30) {
			Debug.Log ("Mode Changing");
		currentMode = PoliceCarMode.Aggressive;
			Invoke ("PoliceCarMode_PreviousMode", 2.0f);
		}
	}
	void PoliceCarMode_PreviousMode()
	{
		currentMode = PoliceCarMode.Avoid;
	}


	#endregion




	#region if Police Car Trigger with 
	
	void OnTriggerEnter (Collider incoming)
	{
		if (incoming.GetComponent<Collider>().name.Contains ("Trigger")) {

			CarFlying ();
		}
	}
	#endregion

	#region if Police car Collision with

	void OnCollisionEnter (Collision incoming)
	{
		string incTag = incoming.collider.tag;
		if (incTag.Contains ("trafficCar") || incTag.Contains("policeCar")|| incTag.Contains("Player")) {
			carSpeed = 0;
			wheelSpeed = 0;
			isDoubleSpeed = 1;
			turnSpeed = 0;
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			GameObject trafficCar = incoming.collider.gameObject; 
			trafficCar.SendMessage ("StopCar", SendMessageOptions.DontRequireReceiver);//to stop the car
			policeCarSound.volume = 0;
			Destroy(gameObject,3f);

		
		}
	}


	#endregion



	void FixedUpdate ()
	{
		if (GamePlayController.isGameEnded)
			return;
		switch (currentState) {
		case PoliceCarState.OnRoad:
			CarOnRoad();
			break;
		case PoliceCarState.OnFly:
		
			currentState = PoliceCarState.OnAirWait;
			break;
		case PoliceCarState.OnAirWait:
			GetComponent<Rigidbody>().position = new Vector3 (lanePosition, carJump_Height, GetComponent<Rigidbody>().position.z);
			break;
		case PoliceCarState.idle:
			break;
			
		}
	}

	#region if car on road
	
	float moveHorizontal;
	public float[] carLanePositions;
	public float targetLanePosition,lanePosition,laneShiftSpeed;

	void CarOnRoad()
	{
		float carYPosition;
			
		switch (currentLane) {
				
			case PoliceCarLane.one:
			targetLanePosition = carLanePositions [0];
				break;
			case PoliceCarLane.two:
			targetLanePosition = carLanePositions [1];
				break;
			case PoliceCarLane.three:
			targetLanePosition = carLanePositions [2];
				break;
			case PoliceCarLane.four:
			targetLanePosition = carLanePositions [3];
				break;
			}

			lanePosition = Mathf.Lerp (lanePosition, targetLanePosition, laneShiftSpeed);
			
			foreach (Transform t in wheelObjs) {

				t.Rotate (wheelSpeed * Time.deltaTime, 0, 0);
			}
	
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



	#region Car On Fly State
	
	public float carJump_Height;
	
	void CarFlying ()
	{
		animator.SetTrigger ("Jump1");
	}
	

	#endregion

	#region Police car in Avoid mode

	float lastTime;
	RaycastHit hitObj;

	void policeCarAvoidMode ()
	{
		Vector3 fwd = thisTrans.TransformDirection (Vector3.forward);

		if (Physics.Raycast (transform.position, fwd, out hitObj, 50) && Time.timeSinceLevelLoad - lastTime > 2.0f) {
			string hitObjTagName= hitObj.collider.tag;
			if ( hitObjTagName.Contains ("Player") || hitObjTagName.Contains ("trafficCar") || hitObjTagName.Contains ("PoliceCar") ) {
				lastTime = Time.timeSinceLevelLoad;
				int random = Random.Range (-1, 1);

				switch (currentLane) {
				case PoliceCarLane.one:

					currentLane = PoliceCarLane.two;
					break;
				case PoliceCarLane.two:
					if (random > 0) {
						currentLane = PoliceCarLane.three;
					} else {
						currentLane = PoliceCarLane.one;
					}

					break;
				case PoliceCarLane.three:
					if (random > 0) {
						currentLane = PoliceCarLane.four;
					} else {
						currentLane = PoliceCarLane.two;
					}

					break;
				case PoliceCarLane.four:
					currentLane = PoliceCarLane.three;

					break;
				}
			}
		}
		PoliceCarAvoid_Back ();
	}


	#endregion
	 


	void PoliceCarAvoid_Back()
	{
		Vector3 back = thisTrans.TransformDirection (-Vector3.forward);
			
			if (Physics.Raycast (transform.position, back, out hitObj, 50) && Time.timeSinceLevelLoad - lastTime > 2.0f) {
				string hitObjTagName= hitObj.collider.tag;
				if ( hitObjTagName.Contains ("Player") || hitObjTagName.Contains ("trafficCar") || hitObjTagName.Contains ("PoliceCar") ) {
					lastTime = Time.timeSinceLevelLoad;
					int random = Random.Range (-1, 1);
					
					switch (currentLane) {
					case PoliceCarLane.one:
						currentLane = PoliceCarLane.two;

						break;
					case PoliceCarLane.two:
						if (random > 0) {
							currentLane = PoliceCarLane.three;
						} else {
							currentLane = PoliceCarLane.one;
						}

						break;
					case PoliceCarLane.three:
						if (random > 0) {
							currentLane = PoliceCarLane.four;
						} else {
							currentLane = PoliceCarLane.two;
						}
				
						break;
					case PoliceCarLane.four:
						currentLane = PoliceCarLane.three;

						break;
					}
				}
			}
		}


	#region Police Aggressive Mode

	void policeCarAggressiveMode ()
	{
		switch (playerCarControl.currentCarLane) {
		case playerCarControl.RoadLanes.one:
			if (currentLane == PoliceCarLane.four) {
				currentLane = PoliceCarLane.three;
			} else if (currentLane == PoliceCarLane.three) {
				currentLane = PoliceCarLane.two;	
			} else {
				currentLane = PoliceCarLane.one;	
			}
			
			break;
		case playerCarControl.RoadLanes.two:
			if (currentLane == PoliceCarLane.four) {
				currentLane = PoliceCarLane.three;	
			} else {
				currentLane = PoliceCarLane.two;
			}
			
			break;
		case playerCarControl.RoadLanes.three:
			if (currentLane == PoliceCarLane.one) {
				currentLane = PoliceCarLane.two;
			} else {
				currentLane = PoliceCarLane.three;	
			}
			
			break;
		case playerCarControl.RoadLanes.four:
			if (currentLane == PoliceCarLane.one) {
				currentLane = PoliceCarLane.two;
				
			} else if (currentLane == PoliceCarLane.two) {
				currentLane = PoliceCarLane.three;
				
			} else {
				currentLane = PoliceCarLane.four;	
			}
			break;
		}
	}

	#endregion
	
	#region If police car is back to the player car and if is two cars in same lane position  decrease speed of police car
	
	void PoliceCarInBack()
	{
		
		switch (playerCarControl.currentCarLane) {
		case playerCarControl.RoadLanes.one:
			if(currentLane == PoliceCarLane.one){
			carSpeed = 3.0f;
			}else{
			
				carSpeed = 7.0f;
			}
			break;
		case playerCarControl.RoadLanes.two:
			if(currentLane == PoliceCarLane.two){
				carSpeed = 3.0f;
			}else{
				carSpeed = 7.0f;
			}
		
			break;
		case playerCarControl.RoadLanes.three:
			if(currentLane == PoliceCarLane.three){
				carSpeed = 3.0f;
			}else{
				carSpeed = 7.0f;
			}
			
			break;
		case playerCarControl.RoadLanes.four:
			if(currentLane == PoliceCarLane.four){
				carSpeed = 3.0f;
			}else{
				carSpeed = 7.0f;
			}
			break;
		}
	}
	

	#endregion

	#region select the lane for Police Car
	
	void PoliceCar_Back()
	{
		
		switch (playerCarControl.currentCarLane) {
		case playerCarControl.RoadLanes.one:
		
			currentLane = PoliceCarLane.two;	
			break;
		case playerCarControl.RoadLanes.two:
		
			currentLane = PoliceCarLane.three;	
			break;
		case playerCarControl.RoadLanes.three:

			currentLane = PoliceCarLane.four;
			
			break;
		case playerCarControl.RoadLanes.four:
		
			currentLane = PoliceCarLane.three;
			break;
		}
	}
	#endregion




	#region onDisable police car destroy here

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
	if(playerCarControl.isDoubleSpeed==1.5)
		{
			Destroy (gameObject, 1);
		}else
		{
			Destroy (gameObject, 2);
		}
	}

	#endregion




}
