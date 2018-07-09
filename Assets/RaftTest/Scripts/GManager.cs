using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RaftTest
{
    /// <summary>
    /// game manager, holds some common links
    /// </summary>
    public class GManager : MonoBehaviour
    {
        [SerializeField] private Material buildingDeniedMaterial;
        public Material BuildingDeniedMaterial { get { return buildingDeniedMaterial; } }

        [SerializeField] private Material buildingAlowedMaterial;
        public Material BuildingAlowedMaterial { get { return buildingAlowedMaterial; } }

        [SerializeField] private Placeable[] allBlocks;
        public IEnumerable<Placeable> AllBlocks()
        {
            foreach (var item in allBlocks) yield return item;
        }

        [SerializeField] private Hammer hammer;
        public Hammer Hammer { get { return hammer; } }

        static GManager thisObject;
        // allows static access
        public static GManager Get
        {
            get { return thisObject; }
        }



        // Use this for initialization
        void Awake()
        {
            thisObject = this;
            // could be read from a config file
            //allBlocks[] = new Placeable(false, null, blockThickness: 1f);
        }
        /// <summary>
        /// Allowing faster app reloading
        /// </summary>
        void Update()
        {
            if (thisObject == null)
                Awake();
        }
    }
}