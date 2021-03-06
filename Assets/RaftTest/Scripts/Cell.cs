﻿using System;
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
        private IPlaceable[] container;

        /// <summary>
        /// Necessary to call
        /// </summary>
        public void Init(IPlaceable block)
        {
            container = new IPlaceable[subBlocksCount];
            for (int i = 0; i < subBlocksCount; i++)
            {
                container[i] = block;
            }
        }

        public IPlaceable Get(Placeable.Side side)
        {
            return container[(int)side];
        }

        public void Place(IPlaceable block, Placeable.Side side)
        {
            container[(int)side] = block;
        }

        public void Remove(Placeable.Side side)
        {
            container[(int)side] = World.AirBlock;
        }              

        public bool HasAnyNonAirBlock()
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