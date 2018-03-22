using UnityEngine;
using System.Collections;

public class Store: MonoBehaviour
{

		public Camera Cam;
		public string hitObjName;
		public int nitrousCost = 2000;
		public int MagnetCost = 1000;
		public GameObject unsufficentCoins, mainMenu, magnetBuy_Btn, nitrousBuy_Btn;
		public GameObject[] magnetPower;
		public GameObject[] nitrousPower;
		public Renderer nitrousBuy_BtnRender, magnetBuy_BtnRender, okay_BtnRender, back_BtnRender;
		public Texture[] nitrousBuy_BtnTexture, magnetBuy_BtnTexture, okay_BtnTexture, back_BtnTexture;
		public TextMesh nitrousCountText,magnetCountText;
		int nitrousIncreaseCount=5,magnetIncreaseCount=5;
	
		void Start ()
		{
//				#if UNITY_EDITOR
//   				 // PlayerPrefs.DeleteAll ();
//				//TotalCoins.staticInstance.AddCoins (200000);
//				#endif
				nitrousCountText.text = "+ "+nitrousIncreaseCount+" seconds";
				magnetCountText.text = "+ "+magnetIncreaseCount+" seconds";
				for (int i=0; i<PlayerPrefs.GetInt ("NitrousCount", 0); i++) {
						nitrousPower [i].SetActive (true);
				}
				for (int i=0; i<PlayerPrefs.GetInt ("MagnetCount", 0); i++) {
						magnetPower [i].SetActive (true);
				}
		}
	
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.D)) {
						PlayerPrefs.DeleteAll ();
				}
				if (Input.GetKeyUp (KeyCode.Mouse0)) {
						Upstate (Input.mousePosition);
				}
				if (Input.GetKeyDown (KeyCode.Mouse0)) {
						DownState (Input.mousePosition);
				}
		}
	
		void Upstate (Vector3 up)
		{
				back_BtnRender.material.mainTexture = back_BtnTexture [0];
				okay_BtnRender.material.mainTexture = okay_BtnTexture [0];
				nitrousBuy_BtnRender.material.mainTexture = nitrousBuy_BtnTexture [0];
				magnetBuy_BtnRender.material.mainTexture = magnetBuy_BtnTexture [0];
				Ray rayObj = Cam.ScreenPointToRay (up);
				RaycastHit hitObject;
		
				if (Physics.Raycast (rayObj, out hitObject)) {
						hitObjName = hitObject.collider.name;
			
						switch (hitObjName) {
						case "NitrousBuyBtn":
								if (TotalCoins.staticInstance.getCoins () >= nitrousCost && PlayerPrefs.GetInt ("NitrousCount", 0) <= 3) {
										for (int i=0; i<nitrousPower.Length; i++) {
												if (i == PlayerPrefs.GetInt ("NitrousCount", 0)) {
														nitrousPower [i].SetActive (true);
												}
										}
										TotalCoins.staticInstance.deductCoins (MagnetCost);
										PlayerPrefs.SetInt("IngameNitrousCount",PlayerPrefs.GetInt("IngameNitrousCount")+10);
										PlayerPrefs.SetInt ("NitrousCount", PlayerPrefs.GetInt ("NitrousCount") + 1);
										nitrousCountText.text = "+ "+nitrousIncreaseCount*(PlayerPrefs.GetInt ("NitrousCount", 0))+" seconds";
								} else {
										//gameObject.SetActive(false);
										if (PlayerPrefs.GetInt ("NitrousCount", 0) > 3) {
												nitrousBuy_Btn.GetComponent<Collider>().enabled = false;
										} else {
												unsufficentCoins.SetActive (true);
										}
								}
				
								break;
						case "MagnetBuyBtn":
								if (TotalCoins.staticInstance.getCoins () >= MagnetCost && PlayerPrefs.GetInt ("MagnetCount", 0) <= 3) {
										
										for (int i=0; i<magnetPower.Length; i++) {
												if (i == PlayerPrefs.GetInt ("MagnetCount", 0)) {
														magnetPower [i].SetActive (true);
												}
										}
										TotalCoins.staticInstance.deductCoins (MagnetCost);
										PlayerPrefs.SetInt ("MagnetCount", PlayerPrefs.GetInt ("MagnetCount") + 1);
										magnetCountText.text = "+ "+magnetIncreaseCount*(PlayerPrefs.GetInt ("MagnetCount",0))+" seconds";
								} else {
										//gameObject.SetActive(false);
										if (PlayerPrefs.GetInt ("MagnetCount", 0) > 3) {
												magnetBuy_Btn.GetComponent<Collider>().enabled = false;
										} else {
												unsufficentCoins.SetActive (true);
										}
								}
								break;
						case "OkayBtn":
								unsufficentCoins.SetActive (false);
								break;
						case "BackBtn":
								gameObject.SetActive (false);
								mainMenu.SetActive (true);
								break;
						}
				}
		}

		void DownState (Vector3 down)
		{
				Ray rayObj = Cam.ScreenPointToRay (down);
				RaycastHit hitObject;
		
				if (Physics.Raycast (rayObj, out hitObject)) {
						hitObjName = hitObject.collider.name;
						switch (hitObjName) {
						case "NitrousBuyBtn":
								//SoundController.Static.PlayClickSound ();
								nitrousBuy_BtnRender.material.mainTexture = nitrousBuy_BtnTexture [1];
								break;
						case "MagnetBuyBtn":
								//SoundController.Static.PlayClickSound ();
								magnetBuy_BtnRender.material.mainTexture = magnetBuy_BtnTexture [1];
								break;
						case "OkayBtn":
								//SoundController.Static.PlayClickSound ();
								okay_BtnRender.material.mainTexture = okay_BtnTexture [1];
								break;
						case "BackBtn":
								back_BtnRender.material.mainTexture = back_BtnTexture [1];
								break;
						}

				}
		}
		

}
