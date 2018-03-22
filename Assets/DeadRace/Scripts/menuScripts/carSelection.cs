using UnityEngine;
using System.Collections;

public class carSelection : MonoBehaviour
{

		// Use this for initialization

		public Camera uiCamera;
		public Renderer[] buttonRenders;
		public Texture[] buttonTexture;
		public RaycastHit hit;
		public GameObject buyButton, playButton;
		public GameObject buyPopUp, InAPPMenu;
		public GameObject loadingLevelObj;
		public Camera mainCam ;

		void Start ()
		{
				Debug.Log ("carSelection.cs is Attached to " + gameObject.name); 
				mainCam = Camera.main;
				#if UNITY_EDITOR || UNITY_WEBPLAYER
					//TotalCoins.staticInstance.AddCoins(999999); //allot some coins to test it 
				#endif
		}

		float lastX;

		void ChangeFieldOfView_Value (float value)
		{
		 
				mainCam.fieldOfView = value;
		}

		public GameObject menuObj;

		void Update ()
		{
				//Debug.Log("Car Index : "+carIndex);
				//Debug.Log ("no of Cars" + carMeshObjs.Length);

				if (Input.GetKeyDown (KeyCode.Mouse0)) {
			
						MouseDown (Input.mousePosition);
						lastX = Input.mousePosition.x;
				}
				if (Input.GetKeyUp (KeyCode.Mouse0)) {
			
						MouseUp (Input.mousePosition);
						if (Input.mousePosition.x - lastX > 20) {
					
								showPreviouscar ();

						} else if (Input.mousePosition.x - lastX < -20) {
				
								showNextcar ();
						}

					lastX = Input.mousePosition.x;
				}

				if (Input.GetKeyUp (KeyCode.Escape)) {
						menuObj.SetActive (true);
						gameObject.SetActive (false);
				}

				if (Input.GetKeyUp (KeyCode.P)) {
						TotalCoins.staticInstance.AddCoins (999999);
				}
				if (Input.GetKeyUp (KeyCode.Q)) {
						TotalCoins.staticInstance.ClearCoins ();
				}





		}

		void MouseUp (Vector3 a)
		{
				foreach (Renderer r in buttonRenders) {
						r.material.mainTexture = buttonTexture [0];
				}
				Ray ray = uiCamera.ScreenPointToRay (a);
		
				if (Physics.Raycast (ray, out hit, 500)) {

						switch (hit.collider.name) {
						case "next":

								
								showPreviouscar ();
				
								break;
						case "previous":
								showNextcar ();
								

								break;
				
						case "play":
				 
								loadingLevelObj.SetActive (true);
								gameObject.SetActive (false);

				 
								break;
						case "buycar":

								purchasecars ();
								break;
				
						}

				}
		
		}

		void MouseDown (Vector3 a)
		{
		
				Ray ray = uiCamera.ScreenPointToRay (a);
		
				if (Physics.Raycast (ray, out hit, 500)) {
						//SoundController.Static.PlayClickSound ();
						Debug.Log ("mouse hit on " + hit.collider.name);
						switch (hit.collider.name) {
						case "next":
								buttonRenders [0].material.mainTexture = buttonTexture [1];
								break;
						case "previous":
								buttonRenders [1].material.mainTexture = buttonTexture [1];
								break;
				
						case "play":
								buttonRenders [2].material.mainTexture = buttonTexture [1];
								break;
						case "buycar":
								buttonRenders [3].material.mainTexture = buttonTexture [1];
								break;
						case "wwq":
								buttonRenders [4].material.mainTexture = buttonTexture [1];
								break;
				
						}
			
			
				}
		
		}
    
		public static int carIndex = 0;
		public GameObject[] carMeshObjs;
		public Vector3[] CameraPositions;
		public Transform carCamera;
		public TextMesh carSpeedDisplayText, carPriceDisplayText, headingText;
		int activate_BtnCount = 1;

		void showNextcar ()
		{

				carIndex++;		
				if (carIndex > carMeshObjs.Length - 1) {	
					
						carIndex = 0;
					
						return;
				} else {

						iTween.MoveTo (carCamera.gameObject, iTween.Hash ("islocal", true, "position", CameraPositions [carIndex], "time", 0.5f));
						showcarINFO ();
				}
		}
		
		bool isCount_Pre = true;

		void showPreviouscar ()
		{
		 
				carIndex--;

				if (carIndex < 0) {	
						carIndex = carMeshObjs.Length - 1;
				}
					
				iTween.MoveTo (carCamera.gameObject, iTween.Hash ("islocal", true, "position", CameraPositions [carIndex], "time", 0.5f));
				showcarINFO ();
				
		}

		void OnEnable ()
		{
				if (carIndex == 0)
						return;
				if (PlayerPrefs.GetInt ("iscar" + carIndex + "Purchased", 0) == 1) {
						playButton.SetActive (true);
						buyButton.SetActive (false);
				} else {
						buyButton.SetActive (true);
						playButton.SetActive (false);
				}
		 
		 
		}

		void showcarINFO ()
		{

				switch (carIndex) {
				case 0:
						headingText.text = "MODEL : Hunter ";
						carSpeedDisplayText.text = "Top speed : 180 kmh";
						carPriceDisplayText.text = "PRice  :    FREE ";
						playButton.SetActive (true);
						buyButton.SetActive (false);
						break;
				case 1:
						headingText.text = "MODEL : Eagle";
						carSpeedDisplayText.text = "Top speed : 220 kmh";
						carPriceDisplayText.text = "PRice  :     1000 ";
						if (PlayerPrefs.GetInt ("iscar1Purchased", 0) == 1) {
								playButton.SetActive (true);
								buyButton.SetActive (false);
						} else {
								buyButton.SetActive (true);
								playButton.SetActive (false);
						}
						break;
				case 2:
						headingText.text = "MODEL : Racer ";
						carSpeedDisplayText.text = "Top speed : 230 kmh";
						carPriceDisplayText.text = "PRice  :     3000 ";
			
						if (PlayerPrefs.GetInt ("iscar2Purchased", 0) == 1) {
								playButton.SetActive (true);
								buyButton.SetActive (false);
						} else {
								buyButton.SetActive (true);
								playButton.SetActive (false);
						}
						break;
				case 3:
						headingText.text = "MODEL : Racer-X";
						carSpeedDisplayText.text = "Top speed : 240 kmh";
						carPriceDisplayText.text = "PRice  :     4000 ";
			
						if (PlayerPrefs.GetInt ("iscar3Purchased", 0) == 1) {
								playButton.SetActive (true);
								buyButton.SetActive (false);
						} else {
								buyButton.SetActive (true);
								playButton.SetActive (false);
						}
						break;
				case 4:
						headingText.text = "MODEL : F1-Car";
						carSpeedDisplayText.text = "Top speed : 260 kmh";
						carPriceDisplayText.text = "PRice  :     5000 ";
			
						if (PlayerPrefs.GetInt ("iscar4Purchased", 0) == 1) {
								playButton.SetActive (true);
								buyButton.SetActive (false);
						} else {
								buyButton.SetActive (true);
								playButton.SetActive (false);
						}
						break;
				case 5:
						headingText.text = "MODEL : Truck";
						carSpeedDisplayText.text = "Top speed : 280 kmh";
						carPriceDisplayText.text = "PRice  :     7000 ";
			
						if (PlayerPrefs.GetInt ("iscar5Purchased", 0) == 1) {
								playButton.SetActive (true);
								buyButton.SetActive (false);
						} else {
								buyButton.SetActive (true);
								playButton.SetActive (false);
						}
						break;
				}

		}

		void purchasecars ()
		{

				switch (carIndex) {
				case 1:

						if (TotalCoins.staticInstance.totalCoins >= 1000) {
								buyPopUP.carCost = 1000;//to set the cost in buyPopUpScript
								buyPopUp.SetActive (true);
								gameObject.SetActive (false);
						} else {
								InAPPMenu.SetActive (true);
								gameObject.SetActive (false);
						}
			 
						break;
				case 2:
						if (TotalCoins.staticInstance.totalCoins >= 3000) {
								buyPopUP.carCost = 3000;
								buyPopUp.SetActive (true);
								gameObject.SetActive (false);
						} else {
								InAPPMenu.SetActive (true);
								gameObject.SetActive (false);
						}
			
						break;
				case 3:
						if (TotalCoins.staticInstance.totalCoins >= 4000) {
								buyPopUP.carCost = 4000;
								buyPopUp.SetActive (true);
								gameObject.SetActive (false);
						} else {
								InAPPMenu.SetActive (true);
								gameObject.SetActive (false);
						}
			
						break;
				case 4:
						if (TotalCoins.staticInstance.totalCoins >= 5000) {
								buyPopUP.carCost = 5000;
								buyPopUp.SetActive (true);
								gameObject.SetActive (false);
						} else {
								InAPPMenu.SetActive (true);
								gameObject.SetActive (false);
						}
			
						break;
				case 5:
						if (TotalCoins.staticInstance.totalCoins >= 6000) {
								buyPopUP.carCost = 6000;
								buyPopUp.SetActive (true);
								gameObject.SetActive (false);
						} else {
								InAPPMenu.SetActive (true);
								gameObject.SetActive (false);
						}
			
						break;
				}

		}
}
