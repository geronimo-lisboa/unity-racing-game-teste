using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BumpAndScrape
{
    public enum BSMaterialType { All, Soft, Hard, NonSoft, NonHard, Other };  

    public class SoundMaster : MonoBehaviour
    {
        [Tooltip("KMSound Master Volume Multiplicator")]
        [Range(0f, 2f)]
        public float Volume = 1f;

        [Tooltip("List of SOFT sounding physics materials")]
        public List<PhysicMaterial> SoftMaterials;

        [Tooltip("List of HARD sounding physics materials")]
        public List<PhysicMaterial> HardMaterials;
    }
}