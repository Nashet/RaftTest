using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RaftTest
{
    /// <summary>
    /// Represents block which can be placed in world and can be hold in hands    
    /// </summary>
    [Serializable]
    public class BlockType : Nameable
    {
        [Tooltip("Turn on if you want to turn of physics for that block and/or include manual collision detection")]
        [SerializeField] private bool isTrigger;
        public bool IsTrigger { get { return IsTrigger; } }

        [SerializeField] public int maxLengthWithoutSupport;

        [SerializeField] public bool canBePlacedAtZeroLevelWithoutFoundation;

        [SerializeField] private bool isFullBlock;
        /// <summary> Full block means that it fills entire cell, like 1x1, not a wall like 0.5x1     
        public bool IsFullBlock { get { return isFullBlock; } }

        [SerializeField] public bool requiresSomeFoundation;

        [SerializeField] public bool allowsXZSnapping;
        [SerializeField] public bool allowsYSnapping;
        public bool OnlyCenterPlacing { get { return !allowsXZSnapping && !allowsYSnapping; } }


        [Tooltip("Should be about same as gameObject thickness")]
        [SerializeField] public float blockThickness;

        

        //protected GameObject block;

        [SerializeField] public GameObject prefab;

        

       

        public event EventHandler<EventArgs> Hidden;
        public event EventHandler<EventArgs> Shown;
        public static event EventHandler<EventArgs> Placed;

        public enum Side { Center, West, East, North, South, Top, Bottom }
        /// <summary>
        /// Constructor. Instead, you can set values in inspector
        /// </summary>   
        public BlockType(string name, bool allowsXZSnapping, bool allowsYSnapping, GameObject prefab, float blockThickness, bool isTrigger, bool requiresSomeFoundation, bool canBePlacedAtZeroLevelWithoutFoundation, bool isFullBlock, Material material, int maxLengthWithoutSupport)
        {
            this.name = name;
            this.isTrigger = isTrigger;
            this.requiresSomeFoundation = requiresSomeFoundation;
            this.canBePlacedAtZeroLevelWithoutFoundation = canBePlacedAtZeroLevelWithoutFoundation;
            this.isFullBlock = isFullBlock;
            //this.originalMaterial = material;
            this.name = name;

            //this.block = prefab;
            this.allowsXZSnapping = allowsXZSnapping;
            this.allowsYSnapping = allowsYSnapping;
            //if (block != null)
            //    renderer = block.GetComponent<MeshRenderer>();
            this.blockThickness = blockThickness;
        }

        

       

        

        
        
       
       
        

        

        

    }
}