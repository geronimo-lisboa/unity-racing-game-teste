using UnityEngine;
using System.Collections;

public class CameraTurn : MonoBehaviour
{
	public Transform Box;

	void LateUpdate ()
	{
		Vector3 GroundPoint = Box.position;
		Ray DownRay = new Ray(Box.position, Vector3.down);
		RaycastHit Hit;
		if(Physics.Raycast(DownRay, out Hit))
		{
			GroundPoint = Hit.point;
		}
		transform.LookAt(GroundPoint);
	}
}
