using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundController : MonoBehaviour {
    private float motorTorque;
    private float steeringAngle;
    private AudioSource carRootAudioSource;
    public AudioSource steeringAudioSource;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float gambiStart, gambiEnd;
    public float CurrentSpeed { get; set; }

    public bool PlayingHandbreak { get; set; }

    private void SetSoundByTorque(float motorTorque)
    {
        carRootAudioSource.pitch = motorTorque * 3.0f / maxMotorTorque;
        if (carRootAudioSource.pitch < 0.12f)
        {
            carRootAudioSource.pitch = 0.12f;
        }
    }

    private void SetSoundBySteering(float steeringAngle)
    {
        //de acordo com o steering angle, controla o volume
        float factor = Mathf.Abs(steeringAngle) / maxSteeringAngle;
        //leva em conta a velocidade.
        float speedFactor = Mathf.Abs ( CurrentSpeed ) / 50.0f;
        if (speedFactor > 1.0f)
        {
            speedFactor = 1.0f;
        }

        steeringAudioSource.volume = factor * speedFactor;
        if(steeringAudioSource.time < gambiStart)
        {
            steeringAudioSource.time = gambiStart;
        }
        if(steeringAudioSource.time > gambiEnd)
        {
            steeringAudioSource.time = gambiStart;
        }

        //lembrar que só quero um trecho do audio
    }

    public void SetTorque(float motorTorque)
    {
        this.motorTorque = motorTorque;
    }

    public void SetSteeringAngle(float angle)
    {
        this.steeringAngle = angle;
    }

    void Start () {
        SetTorque(0);
        carRootAudioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        //Engine sound (in car_root audiosource)
        SetSoundByTorque(motorTorque);
        //Steering sound - use the same sound of screeching tires. This sound will be controlled either
        //by the steering or by the handbreak
        if(PlayingHandbreak==false)
        {
            steeringAudioSource.volume = .0f;
            SetSoundBySteering(steeringAngle);
        }
        else
        {
            //Handbreak sound
            SetSoundBySteering(maxSteeringAngle);
        }

    }

}
