using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
 

public class playerCarControl : MonoBehaviour
{
		public float turnSpeed;
		public float carSpeed;
		public float tilt;
		public float[] limits;
		public Transform[] wheelObjs;
		public float wheelSpeed;

		public static event EventHandler gameEnded;
		public static event EventHandler switchOnMagnetPower;
		public static event EventHandler switchOFFMagnetPower;

		public float magnetPowerTime = 3.0f;
		private float nextFire;
		public static float isDoubleSpeed = 1;
		public Transform thisTrans ;
		public GameObject particleParent;
		public static Vector3 thisPosition ;
		float brakeSpeed = 0;
		carCamera camScript ;
		Camera mainCamera;
		float carJump_Distance;
		public GameObject carbrake_Redlight1, carbrake_Redlight2;
		public scrollUv scrollUvScript;
		public GameObject CarBody;
		public drawSkidmarks leftSkid, rightSkid, stopSkidMarks;
		public drawSkidmarks skidMarksscrip;
		public Animator animator;
	#region enums Declaration
		public enum RoadLanes
		{
				one=0,
				two=1,
				three=2,
				four=4
	  }
		;

		public enum carStates
		{
				OnRoad ,
				OnAir,
				OnAirWait

	}
		;
		public   carStates currentCarState ;
		public static  RoadLanes  currentCarLane ;
		GameObject mainCam;
	#endregion

		Vector3 rotation;

		void OnEnable ()
		{

				mainCam = GameObject.FindGameObjectWithTag ("MainCamera");
				skidMarksscrip = GameObject.FindGameObjectWithTag ("Player").GetComponent<drawSkidmarks> ();
				GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
				rotation = gameObject.transform.eulerAngles;
				#if UNITY_WEBPLAYER || UNITY_EDITOR
				tilt = tilt*2; //car is shaking toomuch in device ,due to non-Smooth acceleration value
				//no problem in webplayer
				#endif
				thisTrans = transform;
				mainCamera = Camera.main;
				foreach (Transform t in gameObject.GetComponentsInChildren<Transform>()) {
						if (t.name.Contains ("Effect")) {
								particleParent = t.gameObject;
						}
				}

				isDoubleSpeed = 1;

				camScript = Camera.main.GetComponent<carCamera> ();
				camScript.targetTrans = thisTrans;
				currentCarState = carStates.OnRoad;
				currentCarLane = RoadLanes.two;
	 
		}

		void Update ()
		{
	
				if (isDoubleSpeed == 1)
						hideNitrousParticle ();
				else
						showNitrousParticle ();
		 
		}

	#region Player Car Collision & trigger

		void OnTriggerEnter (Collider c)
		{
        		  
			if (c.GetComponent<Collider> ().tag == null) return;
				if (c.tag.Contains ("Coin")) {
						coinControl coinScript = c.gameObject.GetComponent<coinControl> () as coinControl;
						SoundController.Static.playCoinHit ();
						coinScript.moveToPlayer = true;
						Destroy (c);
						GamePlayController.collectedCoinsCounts++;

				} else if (c.GetComponent<Collider>().name.Contains ("Magnet")) {
						SoundController.Static.PlayPowerPickUp ();
						
						if (switchOnMagnetPower != null)
								switchOnMagnetPower (null, null);
						coinControl.isONMagetPower = true;
						Destroy (c.gameObject);
						Invoke ("EndMagnetPower", magnetPowerTime + PlayerPrefs.GetInt ("MagnetCount", 0));

				} else if (c.GetComponent<Collider>().name.Contains ("InstantNitrous")) {
						SoundController.Static.PlayPowerPickUp ();

						Destroy (c.gameObject);
						NitrousIndicator.NitrousCount = 100 + PlayerPrefs.GetInt ("IngameNitrousCount", 0); // set to nitrous   
						//Debug.Log("Nitrous Count"+NitrousIndicator.NitrousCount);
						NitrousIndicator.Static.UpdateNitrousDisplay ();
				} else if (c.GetComponent<Collider>().name.Contains ("Trigger")) {
						CarFlying ();
			c.GetComponent<Collider>().enabled = false;
						if (isDoubleSpeed == 1) {
								carJump_Distance = 7;
						} else {
								carJump_Distance = 10;
						}
						//currentCarState = carStates.OnAir;
				} else if (c.GetComponent<Collider>().name.Contains ("side")) {
						carSpeed = 0;
						wheelSpeed = 0;
						isDoubleSpeed = 0;
						turnSpeed = 0;
						GetComponent<Rigidbody>().velocity = Vector3.zero;
						isDoubleSpeed = 1;
						GamePlayController.isGameEnded = true;
						if (gameEnded != null)
								gameEnded (null, null);
					 
					 
						GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
						GameObject trafficCar = c.GetComponent<Collider>().gameObject; 
						trafficCar.SendMessage ("StopCar", SendMessageOptions.DontRequireReceiver);//to stop the car
						 
				}
		}

		void EndMagnetPower ()
		{
				if (switchOFFMagnetPower != null)
						switchOFFMagnetPower (null, null);
		}

		void OnCollisionEnter (Collision incomingCollision)
		{
		  
		string incTag = incomingCollision.collider.GetComponent<Transform> ().tag ;
		string incName = incomingCollision.collider.GetComponent<Transform> ().name;
				if (incTag.Contains ("trafficCar") || incTag.Contains ("policeCar")) {
						if (incTag.Contains ("policeCar")) {
								//incomingCollision.collider.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
						}
		
			 
						skidMarksscrip.stopSkid ();
						carSpeed = 0;
						wheelSpeed = 0;
						isDoubleSpeed = 0;
						turnSpeed = 0;
						GetComponent<Rigidbody>().velocity = Vector3.zero;
						isDoubleSpeed = 1;
						GamePlayController.isGameEnded = true;
						if (gameEnded != null)
								gameEnded (null, null);
					 
						GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
						GameObject trafficCar = incomingCollision.collider.gameObject; 
						trafficCar.SendMessage ("StopCar", SendMessageOptions.DontRequireReceiver);//to stop the car
				}
		}
	
	#endregion
	
		float moveHorizontal ;
		float lanePosition, targetLanePosition ;
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
				}
		}


#region Player Car On Road
		bool stopSkidMark = false;
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
				if (UIControl.isBrakesOn) {
						brakeSpeed = 2;
						if (!stopSkidMark) {
								leftSkid.drawSkid ();
								rightSkid.drawSkid ();
						}
						SoundController.Static.car_brake.enabled = true;
						carbrake_Redlight1.GetComponent<Renderer>().enabled = true;
						carbrake_Redlight2.GetComponent<Renderer>().enabled = true;
						
				} else {
						brakeSpeed = 1;
						SoundController.Static.car_brake.enabled = false;
						carbrake_Redlight1.GetComponent<Renderer>().enabled = false;
						carbrake_Redlight2.GetComponent<Renderer>().enabled = false;
				}
				foreach (Transform t in wheelObjs) {

						t.Rotate ((wheelSpeed * Time.deltaTime) / brakeSpeed, 0, 0);
				}
				#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
		 
				float smoothX = Input.acceleration.x ;
				if(smoothX < -0.5f  && smoothX < 0) smoothX = Mathf.Lerp(smoothX,-1,Time.deltaTime/6);  //smoothX = -1;
				else if(smoothX > 0.5f  ) smoothX = Mathf.Lerp(smoothX,1,Time.deltaTime/6); ;
				//moveHorizontal = smoothX * 2 ;
				#endif
		
				#if UNITY_EDITOR || UNITY_WEBPLAYER
				//moveHorizontal =   0;// Input.GetAxis ("Horizontal");
				#endif
				Vector3 movement = new Vector3 (moveHorizontal, 0.0f, (carSpeed * isDoubleSpeed) / brakeSpeed);
				GetComponent<Rigidbody>().velocity = movement * turnSpeed;
				GetComponent<Rigidbody>().position = new Vector3
		        (
				Mathf.Clamp (lanePosition, limits [0], limits [1]),
				0.0f, GetComponent<Rigidbody>().position.z
				);
				//carCamera.position = rigidbody.position - new Vector3(0,-10, 20 );
				GetComponent<Rigidbody>().rotation = Quaternion.Euler (0, GetComponent<Rigidbody>().velocity.x * tilt, 0.0f);
				//carBody.rotation = Quaternion.Euler (0, rigidbody.velocity.x * tilt, -rigidbody.velocity.x * tilt / 2);
		}

#endregion


#region Player Car On Air State

		public float carJump_Height;

		void CarFlying ()
		{
				animator.SetTrigger ("Jump1");

		}

		void afterReachingTop ()
		{
		 
		}

		void ChangeHeight (float newHeight)
		{
				carJump_Height = newHeight;
		}
	
		void CarSuspention ()
		{
				currentCarState = carStates.OnRoad;
				 
	 

		}

		 

#endregion

#region Power Ups

		public void switchoffmagnet ()
		{
				if (switchOFFMagnetPower != null) {
						switchOFFMagnetPower (null, null);
				}
		}

		void showNitrousParticle ()
		{
				if (particleParent != null)
						particleParent.SetActive (true);
				SoundController.Static.boostAudioControl.enabled = true;
		}
	
		void hideNitrousParticle ()
		{
				if (particleParent != null)
						particleParent.SetActive (false);
				SoundController.Static.boostAudioControl.enabled = false;
		}
#endregion

#region Car Lane Shift Rotation
		public void CarRotation_LangeChange_Right ()
		{
				if (!stopSkidMark) {
						leftSkid.drawSkid ();
						rightSkid.drawSkid ();
				}
			 
		}

		public void CarRotation_LangeChange_Left ()
		{
				if (!stopSkidMark) {
						leftSkid.drawSkid ();
						rightSkid.drawSkid ();
				}

				 
		}

		void OriginalRotation ()
		{
			 
		}
#endregion


	 
}

	
			