using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour {
    public Transform target;
    public float yDist;
    public float zDist;

    private void LateUpdate()
    {
        Vector3 targetPos = target.position;
        Vector3 targetForward = target.forward;
        Vector3 camPos = targetPos + targetForward * (-zDist) + new Vector3(0, yDist, 0);
        this.transform.position = camPos;
        this.transform.LookAt(target);
    }
}
