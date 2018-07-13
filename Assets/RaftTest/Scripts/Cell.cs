using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RaftTest
{
    /// <summary>
    /// Represents element of World's map
    /// </summary>
    public struct Cell
    {        
        private static int subBlocksCount = Enum.GetValues(typeof(Placeable.Side)).Length;
        private Placeable[] container;

        /// <summary>
        /// Necessary to call
        /// </summary>
        public void Init(Placeable block)
        {
            container = new Placeable[subBlocksCount];
            for (int i = 0; i < subBlocksCount; i++)
            {
                container[i] = block;
            }
        }

        public Placeable Get(Placeable.Side side)
        {
            return container[(int)side];
        }

        public void Place(Placeable block, Placeable.Side side)
        {
            container[(int)side] = block;
        }

        internal void Remove(Placeable.Side side)
        {
            container[(int)side] = World.AirBlock;
        }

        internal bool HasAnyNonAirBlock()
        {
            foreach (Placeable.Side eachSide in Enum.GetValues(typeof(Placeable.Side)))
            {
                if (Get(eachSide) != World.AirBlock)
                    return true;
            }
            return false;
        }
    }
}