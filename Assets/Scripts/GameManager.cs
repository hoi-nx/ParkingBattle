using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {


	int score , bestScore;
	public Transform allCars;
	public Transform startPoint;
	public GameObject Cam;
	public Transform center;

	public GameObject effectWon;
	public GameObject resultScreen;
	public Text scoree;
	// Use this for initialization

	GameObject O;
	void Start () {
	
		O = Instantiate (allCars.GetChild (Random.Range (0, allCars.childCount)).gameObject, startPoint.position, startPoint.rotation) as GameObject;
		Cam.transform.parent = O.transform;
		Destroy( O.GetComponent<Rigidbody> ());
		Destroy( O.GetComponent<collissionDetec> ());
		O.AddComponent<Movement> ();
		O.GetComponent<Movement> ().sphereObject = center;
	}
	
 	void goodParking () {
	
		Instantiate (effectWon, O.transform.position+Vector3.up*5, O.transform.rotation);

		resultScreen.SetActive (true);
		score = PlayerPrefs.GetInt ("bestScore") + 1;
 		PlayerPrefs.SetInt ("bestScore",score);
 		scoree.text = PlayerPrefs.GetInt ("bestScore" ).ToString();
	}

	void badParking()
	{
		resultScreen.SetActive (true);
		scoree.text = PlayerPrefs.GetInt ("bestScore" ).ToString();
		

	}
	public void playAgain()
	{

		Application.LoadLevel (0);
	}

}
