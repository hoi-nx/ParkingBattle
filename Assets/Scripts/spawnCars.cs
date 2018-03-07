using UnityEngine;
using System.Collections;

public class spawnCars : MonoBehaviour {

	public Transform prefab;
	public Transform carsParent;
	// Use this for initialization
	void Start () {
	
		Vector3 center = transform.position;
 		for (int i = 0; i < 72; i++)
		{
			int a = i * 5;
			Vector3 pos = RandomCircle(center, 60.0f ,a);
			GameObject p = Instantiate(prefab.GetChild(Random.Range(0,prefab.childCount)).gameObject, pos, Quaternion.identity) as GameObject;
			p.transform.LookAt(transform.position);
			p.transform.parent = carsParent;
		}
		Destroy (carsParent.GetChild(Random.Range(0,prefab.childCount)).gameObject);
	}
	
	Vector3 RandomCircle(Vector3 center, float radius,int a)
	{
 		float ang = a;
		Vector3 pos;
		pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		pos.y = center.y;
		pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		return pos;
	}
}
