using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBestTime : MonoBehaviour {
    public CarController car;
    private Text text;
    private CarLapTimeCalculator timeCalculator;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        timeCalculator = car.GetComponent<CarLapTimeCalculator>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = $"Best:{timeCalculator.BestTime}";
	}
}
