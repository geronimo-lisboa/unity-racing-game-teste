using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnNewLapExitRegionVerifier : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other">This is actually the boxCollider that, in my hierarchy, belongs to the car mesh, not to the parent 
    /// object that has the CarColider.</param>
    private void OnTriggerEnter(Collider other)
    {
        //Is it a car?
        CarController car = other.GetComponentInParent<CarController>();
        //Flags it as going the correct direction
        if (car)
        {
            car.LeaveLapFinishRegion();
        }
    }

}
