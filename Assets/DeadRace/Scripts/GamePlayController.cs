using UnityEngine;
using System.Collections;
 
public class GamePlayController : MonoBehaviour {


	public GameObject[] trafficCars,roadBlocks,vlcCans,sideTres,coinsParent,powerPickups,playerCars,obsticals;
	public static int collectedCoinsCounts,distanceTravelled;
	public static bool isGameEnded = false;
	public GameObject playerObj,slopes_parent,policeCar,policeCar_Traffic;
	public GameObject gameEndMenu;
	public TextMesh coinsText,distanceText ,gameOverTxt;
	float startPlayerCarPositionZ;
	public carCamera camScript ;
	public static GamePlayController Static ;
	public InputController input;
	public Vector4 curve ;
	void OnEnable()
	{
		isGameEnded = false;
		playerCarControl.gameEnded += onGameEnd;
		coinControl.isONMagetPower = false;
		Static = this;
		Shader.SetGlobalVector ("_Offset",curve);
	
	}

	void OnDisable()
	{
		Shader.SetGlobalVector ("_Offset",Vector4.zero);
		playerCarControl.gameEnded -= onGameEnd;
	}
	
	void onGameEnd(System.Object obj, System.EventArgs args)
	{
		CancelInvoke();
		gameEndMenu.SetActive (true);
		this.enabled = false;
	}
	void Start () {
		//OnGameStart ();
		collectedCoinsCounts=0;
		if(camScript == null) camScript = Camera.main.GetComponent<carCamera> ();
		//playerObj = GameObject.FindGameObjectWithTag ("Player");
		if(playerObj == null) playerObj =	GameObject.Instantiate(playerCars[carSelection.carIndex]) as GameObject;
		input.playerCarScript = playerObj.GetComponent<playerCarControl>();
		startPlayerCarPositionZ = playerObj.transform.position.z;
		camScript.targetTrans=playerObj.transform;



	}

	public float[] LanePositions ;
	public void OnGameStart()
	{
		InvokeRepeating("generateTrafficCars",1,1.5f);//for traffic Car
		InvokeRepeating("generatePowerups",15,15);//for PowerUps 
		if( !Application.loadedLevelName.Contains("city"))  
		{
			InvokeRepeating("generateSideTress",0,2.0f);// for Tress
			InvokeRepeating ("generateObstacles", 0, 3.0f);// for Obstacles

		}

		InvokeRepeating("generateCoins",0,2);//for Coins
		//InvokeRepeating ("generateSlpoes", 1.0f, 4f);//for Slopes
		InvokeRepeating ("generatePoliceCars", 4.0f, 3f);
		InvokeRepeating ("generatePoliceCars", 3.0f, 1f);//for Police Car
		//InvokeRepeating ("GeneratePoliceCar_Traffic", 2, 10);
	}
	
	void Update( )
    {
	
	   coinsText.text=""+collectedCoinsCounts;
		distanceText.text=""+GamePlayController.distanceTravelled +" m";
		if (isGameEnded) {
			coinsText.text="" ;
			distanceText.text="" ;
			gameOverTxt.text = "GAME OVER :(";
		}
		else{
			distanceTravelled = Mathf.RoundToInt (( playerCarControl.thisPosition.z  + (1104.015f ))/10);
		}
    }
#region Powerups

	void generatePowerups( )
	{
		GameObject pickupObj = GameObject.Instantiate( powerPickups[ Random.Range(0,powerPickups.Length-1 ) ] ) as GameObject;
		pickupObj.transform.position = new Vector3( LanePositions[Random.Range(0,LanePositions.Length )],5 , playerObj.transform.position.z + 400+(Random.Range(1,10 )*10) ) ;
	}

#endregion



#region Tress

	void generateSideTress( )
	{
		//left side
		GameObject treeObj = GameObject.Instantiate( sideTres[ Random.Range(0,sideTres.Length-1 ) ] ) as GameObject;
		treeObj.transform.position = new Vector3( Random.Range(-31,-23 ) ,0 , playerObj.transform.position.z + 500  ) ;
		treeObj.transform.Rotate (0, Random.Range (0, 36) * 10, 0);
	   
       //rightside
		treeObj = GameObject.Instantiate( sideTres[ Random.Range(0,sideTres.Length-1 ) ] ) as GameObject;
		treeObj.transform.position = new Vector3( Random.Range(23 ,31 ) ,0 , playerObj.transform.position.z + 750  ) ;
		treeObj.transform.Rotate (0, Random.Range (0, 36) * 10, 0);
	}

#endregion

#region Obstacles

	void generateObstacles()
	{
		GameObject ObstaclesObj = GameObject.Instantiate( obsticals[ Random.Range(0,obsticals.Length-1 ) ] ) as GameObject;
		ObstaclesObj.transform.position = new Vector3( Random.Range(-80,-60 ) ,0 , playerObj.transform.position.z + 780  ) ;
		ObstaclesObj.transform.Rotate (0, Random.Range (0, 36) * 10, 0);
		
		ObstaclesObj = GameObject.Instantiate( obsticals[ Random.Range(0,obsticals.Length-1 ) ] ) as GameObject;
		ObstaclesObj.transform.position = new Vector3 (Random.Range (-90, -60), 0, playerObj.transform.position.z + 1200);
		ObstaclesObj.transform.Rotate (0, Random.Range (0, 36) * 10, 0);
		//rightside
		ObstaclesObj = GameObject.Instantiate( obsticals[ Random.Range(0,obsticals.Length-1 ) ] ) as GameObject;
		ObstaclesObj.transform.position = new Vector3( Random.Range(60,80 ) ,0 , playerObj.transform.position.z + 820  ) ;
		ObstaclesObj.transform.Rotate (0, Random.Range (0, 36) * 10, 0);
		
		ObstaclesObj = GameObject.Instantiate( obsticals[ Random.Range(0,obsticals.Length-1 ) ] ) as GameObject;
		ObstaclesObj.transform.position = new Vector3( Random.Range(60,90 ) ,0 , playerObj.transform.position.z + 1200  ) ;
		ObstaclesObj.transform.Rotate (0, Random.Range (0, 36) * 10, 0);
	}

#endregion

#region Coins

	public int[] coinPosition;
	void generateCoins( )
	{
		GameObject coin = GameObject.Instantiate( coinsParent[ Random.Range(0,coinsParent.Length-1 ) ] ) as GameObject;
		coin.transform.position = new Vector3( LanePositions[Random.Range(0,coinPosition.Length )] ,1.4f , playerObj.transform.position.z + 180  ) ;
	}

#endregion

#region Traffic Cars

	public   int[] laneCarCount ;
	int laneRandom;
	void generateTrafficCars( )
	{
		int randomNumber = 3 * Random.Range (1, 11);
		GameObject trafficObj = GameObject.Instantiate( trafficCars[ randomNumber] ) as GameObject;
		laneRandom = Random.Range (0, LanePositions.Length);

		if (laneRandom == SlopeLanePositionIndex)
						return;

         trafficObj.transform.position = new Vector3( LanePositions[laneRandom]  ,0 , playerObj.transform.position.z + 400+(Random.Range(1,10 )*10) ) ;
		trafficObj.GetComponent<trafficCar> ().lanePosition = laneRandom;
		laneCarCount [laneRandom]++;
		if (laneRandom > 1) {
			trafficObj.transform.Rotate (0, 180, 0);
		}
		else {
			trafficObj.transform.Rotate (0, 0, 0);
				}
	}

#endregion

#region for Police Traffic Car
	void GeneratePoliceCar_Traffic()
	{
		GameObject policeCar_TrafficObj = GameObject.Instantiate (policeCar_Traffic)as GameObject;
		policeCar_TrafficObj.transform.position = new Vector3 (LanePositions [laneRandom], 0, playerObj.transform.position.z + 400 + (Random.Range (1, 10) * 10));
		if (laneRandom > 1) {
			policeCar_TrafficObj.transform.Rotate (0, 0, 0);
		}
		else {
			policeCar_TrafficObj.transform.Rotate (0, 180, 0);
		}
	}
#endregion

#region Slopes

	public  float SlopeLanePositionIndex=-1,lastSlopeIndex;
	public int laneWeight;
	public int slopeLane;
	void generateSlpoes()
	{
		//print("Slope  index  "+SlopeLanePositionIndex); 
		if (SlopeLanePositionIndex != -1) {
			return;		
		}
				

		for  (laneWeight=0;laneWeight <= laneCarCount.Length-1;laneWeight++ ) {
			//Debug.Log ("LastSlopeIndex value : " + lastSlopeIndex + " laneWeight : " + laneWeight);
				
			if(laneCarCount[laneWeight] ==0 && lastSlopeIndex != laneWeight)
			{
				GameObject Slope = GameObject.Instantiate (slopes_parent) as GameObject;
				Slope.transform.position = new Vector3 (LanePositions [laneWeight], -1.16349f, playerObj.transform.position.z + 400);
				//print("Slope Lane Position : "+laneWeight);
				//slopeLane= laneWeight;
				lastSlopeIndex = laneWeight;
				SlopeLanePositionIndex =  laneWeight;
				return;
			}

				}
		}

#endregion

#region Police Car 

	public  float policeCarPositionIndex=-1,lastPoliceCarIndex;
	public void generatePoliceCars()
	{
		int random = Random.Range (-1, 1);
		if (policeCarPositionIndex != -1)
			return;
			for  (int laneWeight=0;laneWeight <= laneCarCount.Length-1;laneWeight++ ) {
			if(laneCarCount[laneWeight] ==0 && lastPoliceCarIndex != laneWeight)
			{
				GameObject PoliceCarObj = GameObject.Instantiate (policeCar) as GameObject;
//				if(random<0)
//				{
					//PoliceCarObj.transform.position = new Vector3 (LanePositions [laneWeight], 0, playerObj.transform.position.z-50);
//				}else{
					PoliceCarObj.transform.position = new Vector3 (LanePositions [laneWeight], 0, playerObj.transform.position.z+350);
				//}
				lastSlopeIndex = laneWeight;
				PoliceCar_Controller policeScript= PoliceCarObj.GetComponent<PoliceCar_Controller>() as PoliceCar_Controller;
				policeScript.currentLane = (PoliceCar_Controller.PoliceCarLane)laneWeight ;
				//Debug.Log("police created at " + policeScript.currentCarLane);
				policeCarPositionIndex =  lastPoliceCarIndex;	
				return;
			}
		}
	}

#endregion
}
