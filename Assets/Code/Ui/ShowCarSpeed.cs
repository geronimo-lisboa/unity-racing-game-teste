using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCarSpeed : MonoBehaviour {
    public CarController car;
    private Rigidbody _rigidbody;
    private Text text;
	// Use this for initialization
	void Start () {
        _rigidbody = car.GetComponent<Rigidbody>();
        text = this.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void LateUpdate () {
        var speed = _rigidbody.velocity.magnitude;
        text.text = $"Speed: {speed}";
	}
}
