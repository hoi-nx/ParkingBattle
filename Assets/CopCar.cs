using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopCar : MonoBehaviour
{

    public float velocity = 100f;

    //Các hàm cơ bàn
    void Awake()
    {
        Debug.Log("Test debug Awake");

    }
    // Use this for initialization
    void Start()
    {
        Debug.Log("Test debug Start");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Test debug Update");
        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    transform.Translate(new Vector3(-0.5f, 0, 0));
        //}
        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    transform.Translate(new Vector3(0.5f, 0,0));
        //}
        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    transform.Translate(new Vector3(0, 0, 0.5f));
        //}
        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    transform.Translate(new Vector3(0, 0, -0.5f));
        //}

        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.left * velocity * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0, -90 * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(0, 90 * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.right * velocity * Time.deltaTime);
        // transform.Translate(new Vetor3());
    }
    void LateUpdate()
    {
        Debug.Log("Test debug LateUpdate");
    }
    void FixedUpdate()
    {
        Debug.Log("Test debug FixedUpdate");
    }

}


