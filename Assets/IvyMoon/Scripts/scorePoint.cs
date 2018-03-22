using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IvyMoon;

public class scorePoint : MonoBehaviour {
    Score score;
    // Use this for initialization
    void Start () {
        score = (Score)GameObject.FindObjectOfType(typeof(Score));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnDestroy()
    {
        score.score ++;
     // score.score = score.score + 11;
    }
}
