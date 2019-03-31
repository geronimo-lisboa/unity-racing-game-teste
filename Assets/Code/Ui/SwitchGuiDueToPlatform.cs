using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class SwitchGuiDueToPlatform : MonoBehaviour {
#if UNITY_WEBGL && !UNITY_EDITOR
    //[DllImport("__Internal")]
    //private static extern void Hello();

    //[DllImport("__Internal")]
    //private static extern void HelloString(string str);

    //[DllImport("__Internal")]
    //private static extern void PrintFloatArray(float[] array, int size);

    //[DllImport("__Internal")]
    //private static extern int AddNumbers(int x, int y);

    //[DllImport("__Internal")]
    //private static extern string StringReturnValueFunction();

    //[DllImport("__Internal")]
    //private static extern void BindWebGLTexture(int texture);

    [DllImport("__Internal")]
    private static extern bool IsItMobile();
#else
    private static bool IsItMobile()
    {
        return false;
    }
#endif
    public CarController car;
    public bool IsRunningInMobile;
    // Use this for initialization
    void Start () {
        IsRunningInMobile = IsItMobile();
        car.IsRunningOnMobile = IsRunningInMobile;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
