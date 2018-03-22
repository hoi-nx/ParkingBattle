using UnityEngine;
using System.Collections;
using System;
public class RoadGenerator : MonoBehaviour {

	// Use this for initialization
 
	bool justOnce =false;
	static  int roadBlockCount=1;
	public GameObject otherRoadBlock;
	public float blockDistance = 1478.4f;
	void OnEnable()
	{
		playerCarControl.gameEnded += cancelDestroy;
	}
	void OnDisable()
	{
		playerCarControl.gameEnded -= cancelDestroy;
	}
	void cancelDestroy(System.Object obj,EventArgs args)
	{
    		CancelInvoke ("SwitchRoadBlocks");
	}
	              

	void  Start()
	{
		PlayerPrefs.SetInt ("valname", 3);

	 
	//	  if (OtherStaticBlock == null)OtherStaticBlock = otherRoadBlock;
	}
	void OnTriggerEnter(Collider inc )
	{
		 
		if( inc.tag.Contains("Player") && justOnce==false)
		{

			GameObject newBlock =   otherRoadBlock ;//GameObject.Instantiate(this.gameObject) as GameObject;
			newBlock.name="road" + roadBlockCount;
			roadBlockCount++;
			//229
			otherRoadBlock.transform.Translate( 0,0, blockDistance*2); //road tile width
			justOnce=true;
			//Destroy(this.gameObject,20);
			Invoke("SwitchRoadBlocks",4 );
		}

	}

	void SwitchRoadBlocks()
	{
		justOnce = false;
	}
}
