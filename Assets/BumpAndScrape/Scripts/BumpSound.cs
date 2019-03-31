using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BumpAndScrape
{

    public class BumpSound : BSBase
    {
        [Space]

        [Tooltip("AudioClip to be used")]
        public AudioClip Clip;
        [Tooltip("Minimum collision to make sound")]
        public float BumpMinSize = 0f;
        [Tooltip("Minimum sound volume")]
        public float BumpMinVolume = 1f;
        [Tooltip("Collision to make the full volume sound")]
        public float BumpMaxSize = 0f;
        [Tooltip("Maximum sound volume")]
        public float BumpMaxVolume = 1f;
        [Tooltip("false won't make sound for bumps above BumpMaxSize")]
        public bool MakeSoundAboveMaxSize = true;
        [Tooltip("Random pitch multiplier variation around 1")]
        [Range(0f, 1f)]
        public float PitchSpread = 0f;

        private float Volume = 0f;

        [Tooltip("The last collision size - to be used for configuration of above values")]
        public float LastBumpSize = 0f;

        private AudioSource Source;


        void Awake()
        {
            Master = FindObjectOfType<SoundMaster>();
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (Clip != null)
            {
                if(CheckMat(collision.collider.sharedMaterial))
                {
                    // we need a mass to make sense of the collision impulse size

                    // let's try our rigidbody first
                    Rigidbody RB = null;
                    RB = GetComponent<Rigidbody>();
                    if (RB != null)
                    {
                        // scale the collision impulse to mass 
                        LastBumpSize = collision.impulse.magnitude / RB.mass;
                    }
                    else
                    {
                        // we have no rigid body, let's try the coliding object
                        RB = collision.rigidbody;
                        if (RB != null)
                        {
                            LastBumpSize = collision.impulse.magnitude / RB.mass;
                        }
                        else
                        {
                            // a default value will have to do
                            LastBumpSize = collision.impulse.magnitude / 10f;
                        }
                    }

                    if (LastBumpSize > BumpMaxSize)
                    {
                        // zero or full above the range
                        Volume = MakeSoundAboveMaxSize ? BumpMaxVolume : 0f;
                    }
                    else if (LastBumpSize > BumpMinSize)
                    {
                        // interpolation within the range
                        Volume = Mathf.Lerp(BumpMinVolume, BumpMaxVolume, Mathf.InverseLerp(BumpMinSize, BumpMaxSize, LastBumpSize));
                    }
                    else
                    {
                        // too low bump, no sound
                        Volume = 0f;
                    }

                    // add the sound mster volume if available
                    if (Master != null)
                    {
                        Volume *= Master.Volume;
                    }

                    // zero volume, no fun, let's go away
                    if (Volume < 0.01f) return;

                    // random pitch
                    float Pitch = UnityEngine.Random.Range(1.0f - PitchSpread, 1.0f + PitchSpread);


                    // create and set up the source (if it hasn't been created yet)
                    if (Source == null)
                    {
                        Source = gameObject.AddComponent<AudioSource>();
                        Source.clip = Clip;
                        Source.loop = false;
                        SetAudioSettings(Source);
                    }
                    
                    // set volume and play
                    Source.volume = Volume;
                    Source.pitch = Pitch;
                    Source.Play();


                }


            }
        }

    }
}