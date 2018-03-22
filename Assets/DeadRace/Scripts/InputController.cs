using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{

		 
		public static InputController Static; 
	    public playerCarControl playerCarScript;
		public bool isJump = false, isDown = false, isLeft = false, isRight = false;

		void Start ()
		{
				Static = this;
		 
		}

		Touch currentTouch ;
		float touch_began_x, touch_began_y, touch_moved_x, touch_moved_y, touching;

		void Update ()
		{
				if (playerCarScript == null)
						return;
 
				for (int i = 0; i < Input.touchCount; i++) {
						 
					
								currentTouch = Input.touches [i];
								if (currentTouch.phase == TouchPhase.Began) {
										touch_began_x = currentTouch.position.x;
										touch_began_y = currentTouch.position.y;
										touching = 1;
								}
								if (currentTouch.phase == TouchPhase.Moved) {

										touch_moved_x = currentTouch.position.x;
										touch_moved_y = currentTouch.position.y;
			                      	if (touching == 1) {
										if (touch_began_x + 30 < touch_moved_x) {
												isRight = true;
												RightSideMoving ();
												touching++;
											}
										if (touch_began_x - 30 > touch_moved_x) {
											 
												isLeft = true;
												LeftSideMoving ();
												touching++;
											}
										if (touch_began_y - 30 > touch_moved_y) {

												 
												isDown = true;
												UIControl.isBrakesOn = true;
												touching++;
											}
										if (touch_began_y + 30 < touch_moved_y) {

												 
												isJump = true;
												NitrousIndicator.Static.isNitrousOn = true;
												touching++;
											}
										}
							 
								}
						}
						if (currentTouch.phase == TouchPhase.Ended) {
								touching = 0;
								NitrousIndicator.Static.isNitrousOn = false;
								 
						}
			
				

					if (Input.touchCount == 0) {
						KeyBoardControl ();
						}



		

		}

		void LeftSideMoving ()
		{
				if (playerCarScript.currentCarState == playerCarControl.carStates.OnRoad) {
			
						switch (playerCarControl.currentCarLane) {
						case playerCarControl.RoadLanes.one:
				
								break;
						case playerCarControl.RoadLanes.two:
								playerCarScript.CarRotation_LangeChange_Left ();
								playerCarControl.currentCarLane = playerCarControl.RoadLanes.one;

				playerCarScript.animator.SetTrigger("ShakeLeft");
								break;
						case playerCarControl.RoadLanes.three:
								playerCarScript.CarRotation_LangeChange_Left ();
								playerCarControl.currentCarLane = playerCarControl.RoadLanes.two;
				playerCarScript.animator.SetTrigger("ShakeLeft");
								break;
						case playerCarControl.RoadLanes.four:
								playerCarScript.CarRotation_LangeChange_Left ();
								playerCarControl.currentCarLane = playerCarControl.RoadLanes.three;
         				playerCarScript.animator.SetTrigger("ShakeLeft");
								break;
						}
			
				}
		}

		void RightSideMoving ()
		{
				if (playerCarScript.currentCarState == playerCarControl.carStates.OnRoad) {
			
						switch (playerCarControl.currentCarLane) {
						case playerCarControl.RoadLanes.one:
				
								playerCarControl.currentCarLane = playerCarControl.RoadLanes.two;
								playerCarScript.CarRotation_LangeChange_Right ();
				playerCarScript.animator.SetTrigger("ShakeRight");
								break;
						case playerCarControl.RoadLanes.two:
								playerCarControl.currentCarLane = playerCarControl.RoadLanes.three;
								playerCarScript.CarRotation_LangeChange_Right ();
				playerCarScript.animator.SetTrigger("ShakeRight");
								break;
						case playerCarControl.RoadLanes.three:
								playerCarControl.currentCarLane = playerCarControl.RoadLanes.four;
								playerCarScript.CarRotation_LangeChange_Right ();
				playerCarScript.animator.SetTrigger("ShakeRight");
								break;
						case playerCarControl.RoadLanes.four:
				
								break;
						}
			
				}

		}

		void KeyBoardControl ()
		{
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				
						LeftSideMoving ();
				
				} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
						RightSideMoving ();
				}

		}

		 
}
