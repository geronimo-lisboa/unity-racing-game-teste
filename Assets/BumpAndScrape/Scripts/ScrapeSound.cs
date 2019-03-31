using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BumpAndScrape
{

    public class ScrapeSound : BSBase
    {
        [Tooltip("AudioClip to be used")]
        public AudioClip Clip;
        [Tooltip("Clip volume")]
        public float ClipVolume = 1f;
        [Tooltip("Minimum side speed to be considered scraping")]
        public float MinSideSpeed = 0.1f;

        private float Volume = 0f;

        private AudioSource Source;

        void Awake()
        {
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

                        Volume = ClipVolume;

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

//                        // set up the source (if it hasn't been yet)
//                        if (Source != null)
//                        {
//
//                            if (Clip != null)
//                            {
//                                Source.clip = Clip;
//                            }
//                        }

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
                        if (Clip != null && Source != null)
                        {
                            if (Source.isPlaying)
                            {
                                Source.Stop();
                            }
                            //Source.clip = null;
                        }
                    }

                }
            }

        }

        void OnCollisionExit(Collision collision)
        {
            if (Clip != null && Source != null)
            {
                if (Source.isPlaying)
                {
                    Source.Stop();
                }
                //Source.clip = null;
            }
        }
    }
}