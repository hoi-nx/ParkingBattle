using UnityEngine;
using System.Collections;

public class TiltTuto : MonoBehaviour {

	// Use this for initialization

	public GameObject left_SwipeIndicator,right_SwipeIndicator,up_SwipeIndicator,down_SwipeIndicator,bg;
	bool rightSwipe,leftSwipe,upSwipe,downSwipe;
	public Vector3 originalScale;
	public TextMesh swipeInfo_Text;
	RaycastHit hit;
	void Start()
	{

		rightSwipe = true;
		swipeInfo_Text.text = " Swipe right to move car right lane";
		//	if (Application.isWebPlayer)
		//	gameObject.SetActive (false);
		//to show tutorial just three time in game lifetime.
		if (PlayerPrefs.GetInt ("tiltPlayTimes", 0) < 5) {
						//Update ();
						PlayerPrefs.SetInt ("tiltPlayTimes", PlayerPrefs.GetInt ("tiltPlayTimes", 0) + 1);

					NitrousIndicator.NitrousCount = 20;
			}
			else{

			Destroy(gameObject);
				}
			
		 
	}
	void Update()
	{
	
		if ((Input.GetKeyDown(KeyCode.RightArrow) || InputController.Static.isRight )&& rightSwipe) {
			Destroy(right_SwipeIndicator);
			Invoke("lestSwipeAnim",0.3f);
		}
		if ((Input.GetKeyDown(KeyCode.LeftArrow) || InputController.Static.isLeft) && leftSwipe) {
			Destroy(left_SwipeIndicator);
			Invoke("UpSwipeAnim",0.3f);
		}
		if ((Input.GetKeyDown(KeyCode.UpArrow )|| InputController.Static.isJump) && upSwipe) {
			Destroy(up_SwipeIndicator);
			Invoke("DownSwipeAnim",0.3f);
		}
		if ((Input.GetKeyDown(KeyCode.DownArrow) || InputController.Static.isDown) && downSwipe ) {
			Destroy(down_SwipeIndicator);
			bg.SetActive(false);
			swipeInfo_Text.text = "";
			downSwipe=false;
			StartGamePlayController ();
		}
	
	}


	void StartGamePlayController()
	{
		GamePlayController.Static.OnGameStart();
		Destroy(gameObject);
	}
	void lestSwipeAnim()
	{
		left_SwipeIndicator.SetActive(true);
		swipeInfo_Text.text = " Swipe left to move car left lane";
		rightSwipe=false;
		leftSwipe = true;
	}
	void UpSwipeAnim()
	{
		swipeInfo_Text.text = "Swipe up and HOLD to fire nitrous ";
		up_SwipeIndicator.SetActive(true);
		leftSwipe=false;
		upSwipe= true;
	}
	void DownSwipeAnim()
	{
		swipeInfo_Text.text = "         Swipe down to slowdown";
		down_SwipeIndicator.SetActive(true);
		upSwipe=false;
		downSwipe = true;
	}

	void OnDisable()
	{
		StartGamePlayController ();
	}
	 
}
