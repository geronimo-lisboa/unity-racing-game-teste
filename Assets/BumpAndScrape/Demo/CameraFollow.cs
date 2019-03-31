using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform Box;
    public float Distance;
	
	void LateUpdate ()
    {
        Vector3 GroundPoint = Box.position;
        Ray DownRay = new Ray(Box.position, Vector3.down);
        RaycastHit Hit;
        if(Physics.Raycast(DownRay, out Hit))
        {
            GroundPoint = Hit.point;
        }
        transform.position = GroundPoint + Vector3.back * Distance;
    }
}
