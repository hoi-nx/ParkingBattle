using UnityEngine;
using System.Collections;
using System;

public class coinControl : MonoBehaviour
{

		// Use this for initialization
		float coinSpeed = 1.0f;
		Transform coinTrans ;
		public static int  turnCount;
		public BoxCollider box ;
		Vector3 originalScale;
		public static bool isONMagetPower ;
		public bool moveToPlayer = false ;
		public bool moveToCoinTarget =false;
		Transform thisTrans;

		void OnEnable ()
		{
				thisTrans = transform;
				box = GetComponent<BoxCollider> () as BoxCollider;
				originalScale = box.size;
		playerCarControl.switchOnMagnetPower += onMagenetPower;
		playerCarControl.switchOFFMagnetPower += offMagenetPower;
		playerCarControl.gameEnded += onGameEnd;
				if (isONMagetPower)
						onMagenetPower (null, null);
		}

		void OnDisable ()
		{
		playerCarControl.switchOnMagnetPower -= onMagenetPower;
		playerCarControl.switchOFFMagnetPower -= offMagenetPower;
		playerCarControl.gameEnded -= onGameEnd;
		//Destroy(gameObject);
		}

		void onGameEnd (System.Object obj, EventArgs args)
		{
				Destroy (gameObject);
		}

		void onMagenetPower (System.Object obj, EventArgs args)
		{
				isONMagetPower = true;

				if(box != null) box.size = new Vector3 (60, 60,60);
		}

		public void resetSize ()
		{
			if(box !=null)	box.size = originalScale;
		}

		void offMagenetPower (System.Object obj, EventArgs args)
		{
				isONMagetPower = false;
				resetSize ();
		}

		void Start ()
		{

				coinTrans = transform;
			//	coinTrans.Rotate (0, 0, turnCount);
				turnCount += 4;
		}
	
		// Update is called once per frame
	Vector3 newCoinPostionTarget;
	bool isAwaredNitrous = false;
	bool reduceTozero=false;
		void Update ()
		{

		if (moveToPlayer) {
			thisTrans.position = Vector3.MoveTowards (thisTrans.position, playerCarControl.thisPosition  , 2.0f );
			if(thisTrans.position.z <= playerCarControl.thisPosition.z ) {
				moveToPlayer=false;
				newCoinPostionTarget= playerCarControl.thisPosition+ new Vector3(-50,100,100);
				moveToCoinTarget= true;
				if(NitrousIndicator.NitrousCount < 100)  NitrousIndicator.NitrousCount+=0.5f;
				NitrousIndicator.Static.UpdateNitrousDisplay();

				//renderer.enabled = false ;
				//thisTrans.GetChild(0).renderer.enabled = false;
				//SoundController.Static.playCoinHit();
				//Destroy(gameObject);
			}
		}
		else if ( moveToCoinTarget ) {
			//Debug.Log("moveto coin Target");
			//Vector3 coinRelativetoPlayer = new Vector3(-10,20,playerCarControl.thisPosition.z+40);
			thisTrans.position = Vector3.MoveTowards (thisTrans.position,newCoinPostionTarget,Time.deltaTime* 200f);

		}
				
	
		}

}
