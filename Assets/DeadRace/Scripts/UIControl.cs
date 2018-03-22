using UnityEngine;
using System.Collections;
using System ;
public class UIControl : MonoBehaviour {
	public Camera UICamera;

	public enum UiState
	{
		pause,
		resume,
		gameOver,
		empty 
	}
	public static float     ShieldTime = 10;
	public static float     MagnetTime = 15;
	public GameObject pauseMenu,gameOverMenu,coinIngameCointainer,distanceInGameContainer,pauseButton,NitrousUiParent;
	public RaycastHit hit;
	public Texture[] buttonTex,pauseButtonTex,brakeButtonTex,nitrousButton,camChangeBtnTextures;
	public Renderer pauseButtonRenderer,camChangeBtnRenders;
	public Texture[] fbLikeBtn_Textures;
	public Renderer fbLikeBtn_Render;
	public static bool isBrakesOn = false ;
	public Renderer[] buttonRenders;
	public Transform nitrousTransform,brakeTransform ;
	public GameObject loadingInGame;

	void OnEnable()
	{
	 
		playerCarControl.gameEnded += onGameEnd;
	}
	void OnDisable()
	{
		playerCarControl.gameEnded -= onGameEnd;
	}
	
	void onGameEnd(System.Object obj, System.EventArgs args)
	{
		pauseMenu.SetActive(false);
		coinIngameCointainer.SetActive(false);
		distanceInGameContainer.SetActive(false);
		NitrousUiParent.SetActive(false);
		pauseButton.SetActive (false);
	}
	void downState(Vector3 a )
	{
		
		Ray ray = UICamera.ScreenPointToRay(a);
		
		if (Physics.Raycast(ray, out hit, 500))
		{
			string objName  = hit.collider.name;
			SoundController.Static.PlayClickSound();
			switch(objName)
			{
			case "PlayAgain":
				SoundController.Static.PlayClickSound();
				buttonRenders[0].material.mainTexture=buttonTex[1];
				break;
			case "mainmenu":
				buttonRenders[1].material.mainTexture=buttonTex[1]; //we have two mainmenu button in pausemenu and gameover menu,
				buttonRenders[4].material.mainTexture=buttonTex[1];
				break;
			case "FblikeButton":
				fbLikeBtn_Render.material.mainTexture= fbLikeBtn_Textures[1];
				break;
			case "resume":
				buttonRenders[3].material.mainTexture=buttonTex[1];
				break;
			case "pauseIngame":
				pauseButtonRenderer.material.mainTexture = pauseButtonTex[1];
				break;

			case "NitrousButton":
				if(NitrousIndicator.NitrousCount > 1)
				{
					NitrousIndicator.Static.isNitrousOn = true;
				}
				break;
			case "BrakeButton" :
				isBrakesOn = true ;
				///brakeRenderer.material.mainTexture = brakeButtonTex [1];
				break;
			case "ChangeCamIngame":
				camChangeBtnRenders.material.mainTexture= camChangeBtnTextures[1];
				break;
			}
			
		}
		
	}

	GameObject[] policecar;
	GameObject[] traficCar;

	void DestroyGameObj_PlayGain()
	{
		 policecar= GameObject.FindGameObjectsWithTag("policeCar");
		traficCar= GameObject.FindGameObjectsWithTag("trafficCar");
		for (int i=0; i<policecar.Length; i++) {
						Destroy (policecar [i]);
				}
			for (int j=0; j<traficCar.Length; j++) {
				Destroy (traficCar[j]);


				}


		}


	void upState(Vector3 a )
	{
		fbLikeBtn_Render.material.mainTexture= fbLikeBtn_Textures[0];
		camChangeBtnRenders.material.mainTexture= camChangeBtnTextures[0];
		pauseButtonRenderer.material.mainTexture = pauseButtonTex[0];
		//brakeRenderer.material.mainTexture = brakeButtonTex [0];
		isBrakesOn = false;
		Ray ray = UICamera.ScreenPointToRay(a);
		
		if (Physics.Raycast(ray, out hit, 500))
		{
			
			string objName  = hit.collider.name;
			
			switch(objName)
			{
			case "PlayAgain":
					DestroyGameObj_PlayGain();
				GamePlayController.isGameEnded = false;
				gameOverMenu.SetActive(false);
				loadingInGame.SetActive(true);
				GameObject.FindGameObjectWithTag("Player").SetActive(false);
				Invoke("LevelLoading",2.0f);
				break;
			case "mainmenu":
				Application.LoadLevel("mainMenu");
				break;
			case "FblikeButton":
				string fbUrl="https://www.facebook.com/AceGamesHyderabad";
				Application.OpenURL(fbUrl);
				break;
			case "resume":
				Time.timeScale=1;
				pauseButton.SetActive(true);
				pauseMenu.SetActive(false);
				coinIngameCointainer.SetActive(true);
				distanceInGameContainer.SetActive(true);
				NitrousUiParent.SetActive(true);
				break;
			case "pauseIngame":
				Time.timeScale=0;
				pauseMenu.SetActive(true);
				coinIngameCointainer.SetActive(false);
				distanceInGameContainer.SetActive(false);
				NitrousUiParent.SetActive(false);
				pauseButton.SetActive(false);
				break;
			case "NitrousButton":
				NitrousIndicator.Static.isNitrousOn = false;
				playerCarControl.isDoubleSpeed=1.0f; 
				break;
			case "ChangeCamIngame":
				carCamera.Static.ChangeCamera();
				break;

			}
		}
		foreach(Renderer r in buttonRenders)
		{
			r.material.mainTexture=buttonTex[0];
		}
		
	}


	void LevelLoading () {
		loadingInGame.SetActive (false);
		Application.LoadLevel(Application.loadedLevelName);
	
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKeyDown(KeyCode.Mouse0) )
		{
			
			downState(Input.mousePosition );
		}
		if( Input.GetKeyUp(KeyCode.Mouse0) )
		{
			
			upState(Input.mousePosition );
		}
		if( Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.RightControl)  )
		{
			
			NitrousIndicator.Static.isNitrousOn = false;
			playerCarControl.isDoubleSpeed=1.0f;
		}

		if( Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.RightControl)  )
		{
			if(NitrousIndicator.NitrousCount > 1)
			{
				NitrousIndicator.Static.isNitrousOn = true;

			}
		}

		if( Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Space)  )
		{
			isBrakesOn = true ;
			//brakeRenderer.material.mainTexture = brakeButtonTex [1];
		}
		if( Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.Space)  )
		{
			isBrakesOn = false ;
			//brakeRenderer.material.mainTexture = brakeButtonTex [0];
		}


		#if ( UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 ) && !UNITY_EDITOR
//		//orientation change
//		if ((Screen.orientation == ScreenOrientation.Portrait) || (Screen.orientation == ScreenOrientation.PortraitUpsideDown) ) 
//		{
//			nitrousTransform.localPosition = new Vector3(-7,-16.17969f,0);
//			brakeTransform.localPosition = new Vector3(7,-16.17969f,0);
//		}
//		else if ((Screen.orientation == ScreenOrientation.LandscapeLeft) || (Screen.orientation == ScreenOrientation.LandscapeRight) ) 
//		{
//			nitrousTransform.localPosition =  new Vector3(0,-16.17969f,0);
//			brakeTransform.localPosition = new Vector3(0,-16.17969f,0);
//		}
		#endif
	}
}
