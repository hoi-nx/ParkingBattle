using UnityEngine;
using System.Collections;
using System;

public class EndScoreDisplayer : MonoBehaviour
{

		// Use this for initialization


		public float startCoinsCount, TargetCoisCount, startDistanceCount, startBestDistanceCount, TargetDistanceCount, TargetBestDistanceCount;
		private float toreachCoins, toreachDistance, toreachBestDistance ;
		public float  valueForCoins, valueForDistance, ValueForBestDistance ;
		public TextMesh coinsText, distancetext, bestDistanceText;
		private float coins, distance;
		public GameObject playAgainButton, mainMenuButton, newBestDistance, menu;
		public Vector3[] originalPositions;
		public Vector3 originalScale;
	   
		public static event EventHandler showFullScreenAd;

		void OnEnable ()
		{
				
			
		       
				CoinsCount ();
				startDistanceCountNow ();
		PlayerPrefs.SetInt ("TotalCoins", PlayerPrefs.GetInt ("TotalCoins", 0) + GamePlayController.collectedCoinsCounts);
				//to stop bgsounds on GameoVer
				//SoundController.Static.BgSoundsObj.SetActive (false);
				//SoundController.Static.PlayCarCrashSound ();
				Invoke ("LateShow", 2.5f);
				Invoke ("showAd", 3.5f);
		}

		bool canshowButtons = false;

		void Update ()
		{
				valueForCoins = Mathf.Lerp (valueForCoins, toreachCoins, 0.1f);
		       
				coinsText.text = "" + Mathf.RoundToInt (valueForCoins);
		


				if (Mathf.RoundToInt (valueForCoins) == toreachCoins) {
						
						valueForDistance = Mathf.Lerp (valueForDistance, toreachDistance, 0.1f);

						distancetext.text = "" + Mathf.RoundToInt (valueForDistance);
				}
				if (Mathf.RoundToInt (valueForDistance) == toreachDistance) {
						
						ValueForBestDistance = Mathf.Lerp (ValueForBestDistance, toreachBestDistance, 0.1f);
						
						bestDistanceText.text = "" + Mathf.RoundToInt (ValueForBestDistance);
		
				}
				if (Mathf.RoundToInt (ValueForBestDistance) == toreachBestDistance) {
						 
                    
						canshowButtons = true;
				}

				if (canshowButtons) {

						buttonsMoving = Mathf.Lerp (buttonsMoving, 0, 0.1f);
						menu.transform.localPosition = new Vector3 (0, buttonsMoving, 0);
				}

		       
		}

		void CoinsCount ()
		{
				TargetCoisCount = GamePlayController.collectedCoinsCounts;
				toreachCoins = TargetCoisCount;
				SoundController.Static.PlayClickSound1 ();
		}

		void changeCoinText (float newValue)
		{
		       

				coinsText.text = "" + Mathf.RoundToInt (newValue);
				
		}

		void startDistanceCountNow ()
		{

				TargetDistanceCount = GamePlayController.distanceTravelled;
				toreachDistance = TargetDistanceCount;
				SoundController.Static.PlayClickSound1 ();

		}

		void changeDistanceText (float newValue)
		{
				
				
				distancetext.text = "" + Mathf.RoundToInt (newValue);
		}
	 
		float buttonsMoving = -9.0f;

		void showAd ()
		{
				 
	
				if (showFullScreenAd != null)
						showFullScreenAd (null, null); 
				
				
		}

		void BestDistance ()
		{


				toreachBestDistance = Mathf.RoundToInt (PlayerPrefs.GetFloat ("BestDistance", 0));
				if (PlayerPrefs.GetFloat ("BestDistance", 0) < GamePlayController.distanceTravelled) {
						newBestDistance.SetActive (true);
						TargetBestDistanceCount = GamePlayController.distanceTravelled;
						toreachBestDistance = TargetBestDistanceCount;
						SoundController.Static.playNewBestScoreSound ();
						PlayerPrefs.SetFloat ("BestDistance", GamePlayController.distanceTravelled);
				}
				 

				// Invoke ("showButtons", 0.5f);


		}

	 

		void LateShow ()
		{
				BestDistance ();
		}
}
