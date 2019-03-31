using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBarChangeController : MonoBehaviour {
    const float MAX_SPEED = 22;
    private float currentSpeed = 0;
    public void setSpeed(float speed)
    {
        currentSpeed = speed;
    }
	
	void Start () {
		
	}
	
	void Update () {
        float ratio = currentSpeed / MAX_SPEED;
        this.transform.localScale = new Vector3(ratio, 1, 1);
        float blue = (1 - ratio*2);
        float green = 0.75f * ratio;
        float red = 1.5f * ratio;
        this.GetComponent<UnityEngine.UI.Image>().color = new Color(red, green, blue);
	}
}
