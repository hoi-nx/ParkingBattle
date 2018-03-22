using UnityEngine;
using System.Collections;

public class Traffic_PoliceCarLights : MonoBehaviour {

	// Use this for initialization
	void Start () {
		BlueLight_Enable ();
	}
	public GameObject blueLight, redLight;
	void BlueLight_Enable ()
	{
		blueLight.GetComponent<Renderer>().enabled = true;
		redLight.GetComponent<Renderer>().enabled = false;
		Invoke ("RedLight_Enable", 0.3f);
	}
	void RedLight_Enable ()
	{
		blueLight.GetComponent<Renderer>().enabled = false;
		redLight.GetComponent<Renderer>().enabled = true;
		Invoke ("BlueLight_Enable", 0.3f);
	}
}
