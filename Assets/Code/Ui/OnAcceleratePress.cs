using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Quando começar a clicar no botão, acelerar o carro. Quando parar de clicar ou sair da região, parar de acelerar
/// </summary>
public class OnAcceleratePress : MonoBehaviour,  
    IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public bool IsBeingTouched;
    public CarController car;
    public OnSlowdownPress slowdownBtn;

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
            car.Accelerate();
        }
        else
        {
            if(!slowdownBtn.IsBeingTouched)
                car.ConstantSpeed();
        }
	}
}
