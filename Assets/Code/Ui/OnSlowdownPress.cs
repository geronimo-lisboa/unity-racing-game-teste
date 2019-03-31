using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnSlowdownPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public bool IsBeingTouched;
    public CarController car;
    public OnAcceleratePress accelerateBtn;

    public void OnPointerDown(PointerEventData eventData)
    {
        IsBeingTouched = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsBeingTouched = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsBeingTouched = false;
    }

    // Use this for initialization
    void Start () {
        IsBeingTouched = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (IsBeingTouched)
        {
            car.Deacellerate();
        }
        else
        {
            if(!accelerateBtn.IsBeingTouched)
                car.ConstantSpeed();
        }
    }
}
