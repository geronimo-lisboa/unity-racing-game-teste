using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

namespace BumpAndScrape
{

    public class BSBase : MonoBehaviour 
    {
        [Tooltip("Material type (Soft/Hard) which will create this sound")]
        public BSMaterialType MaterialType = BSMaterialType.All;

        protected SoundMaster Master = null;

        [Tooltip("Additional settings for audio source")]
        public bool AudioSourceSettings = false;

        public enum RollOffEnum { Logarithmic, Linear };

        [ConditionalHide("AudioSourceSettings", true)]
        [Tooltip("The target group to which the AudioSource should route its signal.")]
        public AudioMixerGroup Output = null;

        [ConditionalHide("AudioSourceSettings", true)]
        [Tooltip("Sets the priority of the AudioSource. [0..128]")]
        public int Priority = 128;

        [ConditionalHide("AudioSourceSettings", true)]
        [Tooltip("Pans a playing sound in a stereo way (left or right). This only applies to sounds that are Mono or Stereo..[-1, 1]")]
        public float StereoPan = 0f;

        [ConditionalHide("AudioSourceSettings", true)]
        [Tooltip("Sets how much this AudioSource is affected by 3D spatialisation calculations (attenuation, doppler etc). 0.0 makes the sound full 2D, 1.0 makes it full 3D.[0, 1]")]
        public float SpatialBlend = 0f;

        [ConditionalHide("AudioSourceSettings", true)]
        [Tooltip("The amount by which the signal from the AudioSource will be mixed into the global reverb associated with the Reverb Zones.[0, 1.1]")]
        public float ReverbZoneMix = 1f;

        [ConditionalHide("AudioSourceSettings", true)]
        [Tooltip("How the AudioSource attenuates over distance.")]
        public RollOffEnum RolloffMode = RollOffEnum.Logarithmic;

        [ConditionalHide("AudioSourceSettings", true)]
        [Tooltip("Within the Min distance the AudioSource will cease to grow louder in volume")]
        public float MinDistance = 1f;

        [ConditionalHide("AudioSourceSettings", true)]
        [Tooltip("(Logarithmic rolloff) MaxDistance is the distance a sound stops attenuating at. / (Linear rolloff) MaxDistance is the distance where the sound is completely inaudible")]
        public float MaxDistance = 500f;


        protected bool CheckMat(PhysicMaterial mat)
        {
            if (Master == null)
                return true;

            switch (MaterialType)
            {
                case BSMaterialType.All:
                    return true;
                case BSMaterialType.Soft:
                    return Master.SoftMaterials.Contains(mat);
                case BSMaterialType.Hard:
                    return Master.HardMaterials.Contains(mat);
                case BSMaterialType.NonSoft:
                    return !Master.SoftMaterials.Contains(mat);
                case BSMaterialType.NonHard:
                    return !Master.HardMaterials.Contains(mat);
                case BSMaterialType.Other:
                    return !Master.SoftMaterials.Contains(mat) && !Master.HardMaterials.Contains(mat);
            }
            return true;
        }

        protected void SetAudioSettings(AudioSource Source)
        {
            if (AudioSourceSettings)
            {
                Source.outputAudioMixerGroup = Output;
                Source.priority = Priority;
                Source.reverbZoneMix = ReverbZoneMix;
                Source.panStereo = StereoPan;
                Source.spatialBlend = SpatialBlend;
                switch (RolloffMode)
                {
                    case RollOffEnum.Linear:
                        Source.rolloffMode = AudioRolloffMode.Linear;
                        break;
                    case RollOffEnum.Logarithmic:
                        Source.rolloffMode = AudioRolloffMode.Logarithmic;
                        break;
                }
                Source.minDistance = MinDistance;
                Source.maxDistance = MaxDistance;
            }
        }

    }
}