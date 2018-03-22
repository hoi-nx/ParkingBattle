using UnityEngine;
using System.Collections;
using IvyMoon;

public class Pickup : MonoBehaviour {
	public int PickUpRange = 40; //distance the player will pickup the item from
	public float spinSpeed = 200;//how fast the pickup will spin
	public GameObject Player;//Set the player, if using Unity's FPSController set it to FirstPersonCharacter
    public AudioClip ActivateSound;//set a sound to play on activate
    public int Coins = 50;// set the amount of coins to receive from the pickup
	public AudioClip pickupSound;// set a sound to play on pickup

	currency currency; //reference the currency script and give it a name in this script

	// Use this for initialization
	void Start () {
		//find the currency script and set it to be referenced as currency in this script
		currency = (currency)GameObject.FindObjectOfType(typeof(currency));
        if (ActivateSound != null)
        {
            AudioSource.PlayClipAtPoint(ActivateSound, transform.position);//Play the audio at myself
        }
        else
        {
            Debug.LogError("Activate Sound is not assigned ");//type out an error message to the Console
        }

    }
	
	// Update is called once per frame
	void Update () {
		//Spin Object
		transform.Rotate(Vector3.back,spinSpeed*Time.deltaTime);//have the moneybag spin right

		//Check player is in range
		float PlayerDistance = (this.transform.position - Player.transform.position).sqrMagnitude;//determine the players distance from this
		if (PlayerDistance <= PickUpRange) {
			//play an audio clip
			if(pickupSound != null){
				AudioSource.PlayClipAtPoint(pickupSound, transform.position);//Play the audio at myself
			}else{ 
				Debug.LogError ("Pickup Sound is not assigned ");//type out an error message to the Console
			}

			//Kill myself
			Destroy(gameObject);
			//tell currency.cs script(referenced as currency now) to add the value of Coins to its public int coins
			currency.coins = currency.coins + Coins;
	}
}
}