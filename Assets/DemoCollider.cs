using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoCollider : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("other = " + other.gameObject.tag); // player
        Debug.Log("This = " + this.gameObject.tag); // store
        if (other.gameObject.tag == "Player" && this.gameObject.tag == "Untagged")
        {
            SceneManager.LoadScene("MainBackground");
        } 
    }
}
