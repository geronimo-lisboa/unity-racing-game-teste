using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BumpAndScrape
{

    public class ScrapeOrRollSound : BSBase
	{
		[Tooltip("AudioClip to be used")]
		public AudioClip ScrapeClip;
		[Tooltip("Clip volume")]
		public float ScrapeClipVolume = 1f;
		[Tooltip("AudioClip to be used")]
		public AudioClip RollClip;
		[Tooltip("Clip volume")]
		public float RollClipVolume = 1f;
		[Tooltip("Minimum side speed to be considered scraping")]
		public float MinSideSpeed = 0.1f;

		private float Volume = 0f;

		private AudioSource ScrapeSource;
		private AudioSource RollSource;

		private Vector3 ContactSpeed = new Vector3 ();
		private Vector3 ContactPoint = new Vector3 ();
		private Rigidbody RB = null;

		private bool IsScraping = false;
		private bool IsRolling = false;


		void Awake()
		{
			RB = GetComponent<Rigidbody>();
			Master = FindObjectOfType<SoundMaster>();
		}

		private void OnCollisionStay(Collision collision)
		{
            if (RollClip != null && ScrapeClip != null)
            {
                if (CheckMat(collision.collider.sharedMaterial))
                {
                    
                    IsScraping = false;
                    IsRolling = false;

                    // split speed into part in direction of normal and a tangential one
                    float NormalPart = Vector3.Dot(collision.impulse.normalized, collision.relativeVelocity);
                    float TangentialPartSqr = collision.relativeVelocity.sqrMagnitude - NormalPart * NormalPart;

                    if (TangentialPartSqr > MinSideSpeed * MinSideSpeed)
                    {
                        // it is fast enough. we are going to play something
                        // let's play scraping unless we detect a rolling contact
                        IsScraping = true;
                        IsRolling = false;
                        if (RB != null)
                        {
                            ContactPoint = collision.contacts[0].point + Time.fixedDeltaTime * RB.velocity;
                            ContactSpeed = RB.GetPointVelocity(ContactPoint);
                            if (collision.rigidbody != null)
                            {
                                ContactSpeed -= collision.rigidbody.GetPointVelocity(ContactPoint);
                            }
                            if (ContactSpeed.sqrMagnitude < 1f)
                            {
                                // this is rolling motion after all
                                IsRolling = true;
                                IsScraping = false;
                            }
                        }
                    }

                    if (IsRolling)
                    {

                        // this is officially a rolling movement 

                        Volume = RollClipVolume;

                        // add the sound master volume if available
                        if (Master != null)
                        {
                            Volume *= Master.Volume;
                        }

                        // zero volume, no fun, let's go away
                        if (Volume < 0.01f)
                            return;

                        // create the source (if it hasn't been created yet)
                        if (RollSource == null)
                        {
                            RollSource = gameObject.AddComponent<AudioSource>();
                            RollSource.loop = true;
                            // set up the source (if it hasn't been yet)
                            if (RollSource != null)
                            {
                                if (RollClip != null)
                                {
                                    RollSource.clip = RollClip;
                                }
                            }
                            SetAudioSettings(RollSource);
                        }
                            
                        if (RollSource != null)
                        {
                            // set volume and play
                            RollSource.volume = Volume;
                            if (!RollSource.isPlaying)
                            {
                                RollSource.Play();
                            }
                        }
                    }
                    else
                    {
                        // not sliding
                        if (RollClip != null && RollSource != null)
                        {
                            if (RollSource.isPlaying)
                            {
                                RollSource.Stop();
                            }
                        }
                    }

                    if (IsScraping)
                    {

                        // this is officially a sliding movement 

                        Volume = ScrapeClipVolume;

                        // add the sound master volume if available
                        if (Master != null)
                        {
                            Volume *= Master.Volume;
                        }

                        // zero volume, no fun, let's go away
                        if (Volume < 0.01f)
                            return;

                        // create the source (if it hasn't been created yet)
                        if (ScrapeSource == null)
                        {
                            ScrapeSource = gameObject.AddComponent<AudioSource>();
                            ScrapeSource.loop = true;
                            // set up the source (if it hasn't been yet)
                            if (ScrapeSource != null)
                            {
                                if (ScrapeClip != null)
                                {
                                    ScrapeSource.clip = ScrapeClip;
                                }
                            }
                            SetAudioSettings(ScrapeSource);
                        }
                            
                        if (ScrapeSource != null)
                        {
                            // set volume and play
                            ScrapeSource.volume = Volume;
                            if (!ScrapeSource.isPlaying)
                            {
                                ScrapeSource.Play();
                            }

                        }
                    }
                    else
                    {
                        // not sliding
                        if (ScrapeClip != null && ScrapeSource != null)
                        {
                            if (ScrapeSource.isPlaying)
                            {
                                ScrapeSource.Stop();
                            }
                        }
                    }
                }
            }


		}

		void OnCollisionExit(Collision collision)
		{
			if (RollClip != null && RollSource != null)
			{
				if (RollSource.isPlaying)
				{
					RollSource.Stop();
				}
			}

			if (ScrapeClip != null && ScrapeSource != null)
			{
				if (ScrapeSource.isPlaying)
				{
					ScrapeSource.Stop();
				}
			}
		}
	}
}