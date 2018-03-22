using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllMapCity : MonoBehaviour
{

    public GameObject img;

    public bool isShowing;

    private void Start()
    {
        img.SetActive(false);
    }
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(isShowing);
            isShowing = !isShowing;
            img.SetActive(isShowing);
        }
    }
}
