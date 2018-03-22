using UnityEngine;
using System.Collections;

public class levelSelection : MonoBehaviour {

	// Use this for initialization
	public RaycastHit hit;
	public Camera uiCamera;
	public static  string levelName;
	public GameObject carSelection;
	public GameObject LadingSpin;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		OnMouseOver ();
		if( Input.GetKeyDown(KeyCode.Mouse0) )
		{
			
			MouseDown(Input.mousePosition );
		}
		if( Input.GetKeyUp(KeyCode.Mouse0) )
		{
			
			MouseUp(Input.mousePosition );
		}
	}


	void MouseUp(Vector3 a )
	{
		 
		Ray ray = uiCamera.ScreenPointToRay(a);
		
		if (Physics.Raycast(ray, out hit, 500))
		{
			
			switch(hit.collider.name)
			{
			case "BACK":
				iTween.PunchScale(hit.collider.gameObject,new Vector3(1,1,0),0.3f);
                Debug.Log("back");
				break;
	
			case "Snow":
				levelName = "SnowGamePlay";
				LadingSpin.SetActive(true);
				gameObject.SetActive(false);
                Debug.Log("Snow");
                break;


				
			}
			
		}
		
	}
	void MouseDown(Vector3 a )
	{
		
		Ray ray = uiCamera.ScreenPointToRay(a);
		
		if (Physics.Raycast(ray, out hit, 500))
		{
			//SoundController.Static.PlayClickSound();
			 
			switch(hit.collider.name)
			{
			case "BACK":
				carSelection.SetActive(true);
				gameObject.SetActive(false);
				 
				break;
			case "dayLight":
				iTween.PunchScale(hit.collider.gameObject,new Vector3(1,1,0),0.3f);
				break;
			case "Sunny":
				iTween.PunchScale(hit.collider.gameObject,new Vector3(1,1,0),0.3f);
				break;
			 
				
			}
			
			
		}
		
	}
	void OnMouseOver()
	{
		Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast (ray, out hit, 500)) {
		
			switch(hit.collider.name)
			{
			
			case "Snow":
				iTween.PunchRotation (hit.collider.gameObject, iTween.Hash ("amount", new Vector3 (0, 0, 8f), "time", 2f));
				break;
				
			}
				}

	}


}
