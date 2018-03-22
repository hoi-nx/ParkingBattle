using UnityEngine;
using System.Collections;

public class curveSet : MonoBehaviour {


	
	 
	public Vector4 curveOffset ;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Shader.SetGlobalVector("_QOffset", curveOffset);
	}
}
