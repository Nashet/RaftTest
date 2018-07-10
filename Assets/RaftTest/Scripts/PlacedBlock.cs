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
        [SerializeField] public Placeable.Side SideSnapping { get; private set; }
        public static PlacedBlock Add(GameObject go, Placeable placeable, Placeable.Side side)
        {
            var placed = go.AddComponent<PlacedBlock>();
            placed.Placeable = placeable;
            placed.SideSnapping = side;
            return placed;
        }        
    }
}