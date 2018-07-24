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

        [SerializeField] private GameObject playersHands;
        public GameObject PlayersHands { get { return playersHands; } }

        [SerializeField] private Placeable[] allBlocks;
        [SerializeField] private AbstractTool[] allTools;
        [SerializeField] private AbstractHandWeapon[] allHoldableWeapons;

        public IEnumerable<IPlaceable> AllPlaceable()
        {
            foreach (var item in allBlocks) yield return item;
        }

        public IEnumerable<IHoldable> AllHoldable()
        {
            foreach (var item in AllPlaceable()) yield return item;
            foreach (var item in allTools) yield return item;
            foreach (var item in allHoldableWeapons) yield return item;
        }


        static GManager thisObject;
        // allows static access
        public static GManager Get
        {
            get { return thisObject; }
        }

        [SerializeField] protected GameObject interactableCanvas;
        public GameObject InteractableCanvas { get { return interactableCanvas; } }

        [SerializeField] protected GameObject player;
        public GameObject Player { get { return player; } }

        [SerializeField] protected Water water;
        public Water Water { get { return water; } }



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

        public static T CheckComponentAvailability<T>(MonoBehaviour that)// where T : Component
        {
            foreach (var item in that.GetComponents<T>())
            {
                if (item as Component != that) // avoid self returning
                    return item;
            }
            Debug.LogError("Missing " + typeof(T).Name + " component");
            return default(T);
        }
    }
}