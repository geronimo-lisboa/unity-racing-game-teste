using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/*
 * Holds the Axle's properties. A vehicle can have up to 20 axles.
 */
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor? Important for 4x4 for example.
    public bool steering; // does this wheel apply steer angle? 
}

public class CarController : MonoBehaviour {
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxMotorRearTorque;
    public float maxSteeringAngle;
    private CarLapTimeCalculator LapTimeCalculator;
    private CarSoundController soundController;
    public bool canFinishLap;
    public SpeedBarChangeController speedBar;

    public bool IsRunningOnMobile;


    internal bool PassedThruLapFinishedEntriRegion()
    {
        return canFinishLap;
    }



    internal void LeaveLapFinishRegion()
    {
        canFinishLap = false;
    }

    internal void FinishLap()
    {
        LapTimeCalculator.FinishLap();
    }

    internal void PassThruLapFinishEntryRegion()
    {
        canFinishLap = true;
    }

    public float currentMotorTorque = 0;

    private void Start()
    {
        canFinishLap = false;
        soundController = GetComponent<CarSoundController>();
        soundController.maxMotorTorque = maxMotorTorque;
        soundController.maxSteeringAngle = maxSteeringAngle;
        soundController.PlayingHandbreak = false;
        soundController.CurrentSpeed = 0;
        LapTimeCalculator = GetComponent<CarLapTimeCalculator>();
    }
    /// <summary>
    /// This method assumes that the wheel has a child object and that this child object is where the geometry of the wheel is.
    /// </summary>
    /// <param name="wheelCollider"></param>
    private void ApplySteeringWheelTurnToWheelMesh(WheelCollider wheelCollider)
    {
        float steeringAngle = wheelCollider.steerAngle; //Always degrees, always around Y.
        Transform child = wheelCollider.GetComponentInChildren<Transform>();
        child.rotation = new Quaternion(); //resets rotation bc i dont want to accumulate it.
        child.Rotate(Vector3.up, steeringAngle);//Now, the rotation.
    }

    private void ApplyRollToWheelMesh(WheelCollider wheelCollider)
    {
        //RPM is how many rotations in a minute, so 1 RPM means 60 s <-> 1 R
        //So...
        //60 s           =   wheelCollider.rpm (rotations)
        //FixedDeltaTime =   x Rotations
        //then the angle is x R * 360.
        var RPM = wheelCollider.rpm;
        var x = RPM * Time.fixedDeltaTime / 60.0f;
        var angleOfRotation = x * 360.0f;

        Transform child = wheelCollider.GetComponentInChildren<Transform>();
        child.Rotate(new Vector3(1,0,0), angleOfRotation);//Now, the rotation.
    }

    public float MeasureSpeed()
    {
        return GetComponent<Rigidbody>().velocity.magnitude;
    }

    private float mobileMotorTorque=0;
    

    public void Accelerate()
    {
        mobileMotorTorque = 10;
    }

    public void ConstantSpeed()
    {
        mobileMotorTorque = 0;
    }

    public void Deacellerate()
    {
        mobileMotorTorque = -10;
    }

    private bool mobileHandbreak = false;

    internal void Handbreak()
    {
        mobileHandbreak = true;
    }

    internal void EndHandbreak()
    {
        mobileHandbreak = false;
    }

    private void FixedUpdate()
    {
        //TODO: O aumento do torque do motor devia ser logarítmico e não linear
        //TODO: Different input methods for different form factors, like mobile and Web.
        //Pega o motor e a direção de acordo com o input
        float steering = 0;
        float breakTorque = 0;
        float dMotor = 0;

        if (!IsRunningOnMobile)
        {
            float motorTorqueControlAxis = Input.GetAxis("Vertical");
            float dTorque = 10.0f; //TODO: Should be a property because it controls acceleration.
            if (Mathf.Abs(motorTorqueControlAxis) >= Mathf.Abs(0.01f))
            {
                dMotor = dTorque * motorTorqueControlAxis < 0 ? -1 : 1;
            }
            else
            {
                dMotor = 0;
            }
            steering = maxSteeringAngle * Input.GetAxis("Horizontal");
            //Pra ver se tem que freiar
            breakTorque = 0;
            if (Input.GetKey(KeyCode.Space))
            {
                breakTorque = 300000;
                dMotor = -currentMotorTorque / 10;
                soundController.PlayingHandbreak = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                breakTorque = 0;
                soundController.PlayingHandbreak = false;
            }
        }
        else
        {
            dMotor = mobileMotorTorque;
            breakTorque = 0;
            if (mobileHandbreak)
            {
                breakTorque = 300000;
                dMotor = -currentMotorTorque / 10;
                soundController.PlayingHandbreak = true;
            }
            else
            {
                breakTorque = 0;
                soundController.PlayingHandbreak = false;
            }
        }

        
        float speedKmh = MeasureSpeed();
        speedBar.setSpeed(speedKmh);
        soundController.CurrentSpeed = speedKmh;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
                axleInfo.leftWheel.brakeTorque = breakTorque;
                axleInfo.rightWheel.brakeTorque = breakTorque;
                this.ApplySteeringWheelTurnToWheelMesh(axleInfo.leftWheel);
                this.ApplySteeringWheelTurnToWheelMesh(axleInfo.rightWheel);
                soundController.SetSteeringAngle(steering);
            }
            if (axleInfo.motor)
            {       
                axleInfo.leftWheel.motorTorque += dMotor;
                axleInfo.rightWheel.motorTorque += dMotor;
                //Impede que, em caso de torque positivo, ultrapasse o torque máximo.
                if (axleInfo.leftWheel.motorTorque >= maxMotorTorque)
                {
                    axleInfo.leftWheel.motorTorque = maxMotorTorque;
                }
                if (axleInfo.rightWheel.motorTorque >= maxMotorTorque)
                {
                    axleInfo.rightWheel.motorTorque = maxMotorTorque;
                }
                //Impede que em caso de torque negavito, ultrapasse o torque negativo máximo.
                if (axleInfo.leftWheel.motorTorque <= -maxMotorRearTorque)
                {
                    axleInfo.leftWheel.motorTorque = -maxMotorRearTorque;
                }
                if (axleInfo.rightWheel.motorTorque <= -maxMotorRearTorque)
                {
                    axleInfo.rightWheel.motorTorque = -maxMotorRearTorque;
                }

                axleInfo.leftWheel.brakeTorque = breakTorque;
                axleInfo.rightWheel.brakeTorque = breakTorque;
                soundController.SetTorque(axleInfo.leftWheel.motorTorque);
                currentMotorTorque = axleInfo.leftWheel.motorTorque;
               
            }
            ApplyRollToWheelMesh(axleInfo.leftWheel);
            ApplyRollToWheelMesh(axleInfo.rightWheel);
        }
    }


}
