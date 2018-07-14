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

        [SerializeField] private Material selectedByToolMaterial;
        public Material SelectedByToolMaterial { get { return selectedByToolMaterial; } }

        [SerializeField] private GameObject playersHands;
        public GameObject PlayersHands { get { return playersHands; } }

        [SerializeField] private Placeable[] allBlocks;
        [SerializeField] private AbstractTool[] allTools;

        public IEnumerable<IPlaceable> AllPlaceable()
        {
            foreach (var item in allBlocks) yield return item;
        }

        public IEnumerable<IHoldable> AllHoldable()
        {
            foreach (var item in AllPlaceable()) yield return item;
            foreach(var item in allTools) yield return item;
        }


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