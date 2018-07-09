using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RaftTest
{
    /// <summary>
    /// Represents block placed in a world
    /// Placeable - type of that block
    /// </summary>
    public class PlacedBlock : MonoBehaviour
    {
        [SerializeField] public Placeable Placeable { get; private set; }
        [SerializeField] public Vector2Int sideSnapping { get; private set; }
        public static PlacedBlock Add(GameObject go, Placeable placeable, Vector2Int side)
        {
            var placed = go.AddComponent<PlacedBlock>();
            placed.Placeable = placeable;
            placed.sideSnapping = side;
            return placed;
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}