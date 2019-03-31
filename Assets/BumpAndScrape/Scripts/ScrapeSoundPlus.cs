using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BumpAndScrape
{

    public class ScrapeSoundPlus : BSBase
    {
        [Tooltip("AudioClip to be used")]
        public AudioClip Clip;
        [Tooltip("Minimum collision to make sound")]
        public float ScrapeMinSize = 0f;
        [Tooltip("Minimum sound volume")]
        public float ScrapeMinVolume = 1f;
        [Tooltip("Collision to make the full volume sound")]
        public float ScrapeMaxSize = 0f;
        [Tooltip("Maximum sound volume")]
        public float ScrapeMaxVolume = 1f;
        [Tooltip("Minimum side speed to be considered scraping")]
        public float MinSideSpeed = 0.1f;

        private float Volume = 0f;

        [Tooltip("The last collision size - to be used for configuration of above values")]
        public float LastScrapeSize = 0f;

        private AudioSource Source;
        private Rigidbody RB = null;

        void Awake()
        {
            RB = GetComponent<Rigidbody>();
            Master = FindObjectOfType<SoundMaster>();
        }

        private void OnCollisionStay(Collision collision)
        {
            if (Clip != null)
            {
                if (CheckMat(collision.collider.sharedMaterial))
                {
                    
                    // split speed into part in direction of normal and a tangential one
                    float NormalPart = Vector3.Dot(collision.impulse.normalized, collision.relativeVelocity);
                    float TangentialPartSqr = collision.relativeVelocity.sqrMagnitude - NormalPart * NormalPart;

                    if (TangentialPartSqr > MinSideSpeed * MinSideSpeed)
                    {
					
                        // this is officially sliding movement 

                        // we need a mass to make sense of the collision impulse size
                        if (RB == null)
                        {
                            RB = GetComponent<Rigidbody>();
                        }

                        if (RB != null)
                        {
                            // scale the collision impulse to mass 
                            LastScrapeSize = collision.impulse.magnitude / RB.mass;
                        }
                        else
                        {
                            // we have no rigid body, let's try the coliding object
                            RB = collision.rigidbody;
                            if (RB != null)
                            {
                                LastScrapeSize = collision.impulse.magnitude / RB.mass;
                            }
                            else
                            {
                                // a default value will have to do
                                LastScrapeSize = collision.impulse.magnitude / 10f;
                            }
                        }

                        if (LastScrapeSize > ScrapeMaxSize)
                        {
                            // zero or full above the range
                            Volume = ScrapeMaxVolume;
                        }
                        else if (LastScrapeSize > ScrapeMinSize)
                        {
                            // interpolation within the range
                            Volume = Mathf.Lerp(ScrapeMinVolume, ScrapeMaxVolume, Mathf.InverseLerp(ScrapeMinSize, ScrapeMaxSize, LastScrapeSize));
                        }
                        else
                        {
                            // too small contact, no sound
                            Volume = 0f;
                        }

                        // add the sound master volume if available
                        if (Master != null)
                        {
                            Volume *= Master.Volume;
                        }

                        // zero volume, no fun, let's go away
                        if (Volume < 0.01f)
                            return;


                        // create the source (if it hasn't been created yet)
                        if (Source == null)
                        {
                            Source = gameObject.AddComponent<AudioSource>();
                            Source.loop = true;
                            // set up the source (if it hasn't been yet)
                            if (Source != null)
                            {
                                if (Clip != null)
                                {
                                    Source.clip = Clip;
                                }
                            }
                            SetAudioSettings(Source);
                        }
                            
                        if (Source != null)
                        {
                            // set volume and play
                            Source.volume = Volume;
                            if (!Source.isPlaying)
                            {
                                Source.Play();
                            }
                        }
                    }
                    else
                    {
                        // not sliding
                        if (Source != null)
                        {
                            if (Source.isPlaying)
                            {
                                Source.Stop();
                            }
                        }
                    }

                }
            }

        }

        void OnCollisionExit(Collision collision)
        {
            if (Source != null)
            {
                if (Source.isPlaying)
                {
                    Source.Stop();
                }
            }
        }
    }
}