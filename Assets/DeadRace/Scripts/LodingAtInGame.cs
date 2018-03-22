using UnityEngine;
using System.Collections;

public class LodingAtInGame : MonoBehaviour {

	// Use this for initialization
	public Vector3 rotationDirection ;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (rotationDirection * Time.deltaTime);
	}
}
