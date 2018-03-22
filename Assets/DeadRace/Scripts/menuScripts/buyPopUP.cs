using UnityEngine;
using System.Collections;

public class buyPopUP : MonoBehaviour {

	// Use this for initialization
	public TextMesh costText;
	public GameObject carSelectionMenu;
	public Camera uiCamera;
	public static int carCost;
	void OnEnable()
	{
		  costText.text=" "+carCost;
   

	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKeyUp(KeyCode.Mouse0) )
		{
			
			MouseUp(Input.mousePosition );
		}

	}

	void MouseUp(Vector3 a )
	{
		 Ray ray = uiCamera.ScreenPointToRay(a);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 500))
		{
			Debug.Log(gameObject.name + "    " + hit.collider.name);
			switch(hit.collider.name)
			{
			case "YES":
				 
				PlayerPrefs.SetInt("iscar"+carSelection.carIndex+"Purchased",1); // to save the car lock status
				Debug.Log(carSelection.carIndex);
				TotalCoins.staticInstance.deductCoins(carCost);
				 
				carSelectionMenu.SetActive(true);
				gameObject.SetActive(false);
				break;
			case "NO":

				carSelectionMenu.SetActive(true);
				gameObject.SetActive(false);
				break;
			 
				
			}
			
		}
		
	}
}
