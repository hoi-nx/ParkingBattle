using UnityEngine;
using System.Collections;

public class drawSkidmarks : MonoBehaviour {

	// Use this for initialization
	public Transform skidTrailPrefab;
	public float SkidFactor { get; private set; }
	public static Transform skidTrailsDetachedParent;
	private Transform skidTrail;
	public bool leavingSkidTrail;

	private float skidFactorTarget;

	public Transform wheelPosition ;
	public Vector3 offset ;
	public int distroyskidMarks=8;
	void Start () {
	
	}
 

	public void 	drawSkid()
	{

		if (skidTrailPrefab != null)
		{
			 	if (!leavingSkidTrail)
				{
					skidTrail = Instantiate(skidTrailPrefab) as Transform;
					if (skidTrail != null)
					{
						skidTrail.parent = transform;
					skidTrail.position = wheelPosition.position - offset ;
					}
					leavingSkidTrail = true;
					Invoke("stopSkid",3.0f);
				}
				
			 
				
			}
		}

	public	void stopSkid ()
	{
		if (leavingSkidTrail)
		{

			skidTrail.parent = skidTrailsDetachedParent;
			Destroy(skidTrail.gameObject, distroyskidMarks);
			leavingSkidTrail = false;
		}
	}


	}
 
