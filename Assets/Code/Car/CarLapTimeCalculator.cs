using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CarLapTimeCalculator : MonoBehaviour {
    public List<float> lastTimes = new List<float>();
    public float CurrentTime { get; set; }
    public float AvarageTime { get {
            if(lastTimes.Count == 0)//Evitar divisão por zero.
            {
                return float.NaN;
            }
            float acc = 0;
            foreach(var t in lastTimes)
            {
                acc += t;
            }
            return acc / lastTimes.Count;            
        }  }
    public float BestTime { get {
            if (lastTimes.Count == 0)
                return 0;
            float min = lastTimes.Min();
            return min;
        }}
    // Use this for initialization
    void Start () {
        CurrentTime = 0;	       
	}

    private void FixedUpdate()
    {
        CurrentTime += Time.fixedDeltaTime;
    }

    public void FinishLap()
    {
        lastTimes.Add(CurrentTime);
        CurrentTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
