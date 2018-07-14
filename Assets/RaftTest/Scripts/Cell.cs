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
        private static int subBlocksCount = Enum.GetValues(typeof(BlockType.Side)).Length;
        private BlockType[] container;

        /// <summary>
        /// Necessary to call
        /// </summary>
        public void Init(BlockType block)
        {
            container = new BlockType[subBlocksCount];
            for (int i = 0; i < subBlocksCount; i++)
            {
                container[i] = block;
            }
        }

        public BlockType Get(BlockType.Side side)
        {
            return container[(int)side];
        }

        public void Place(BlockType block, BlockType.Side side)
        {
            container[(int)side] = block;
        }

        public void Remove(BlockType.Side side)
        {
            container[(int)side] = World.AirBlock;
        }              

        public bool HasAnyNonAirBlock()
        {
            foreach (BlockType.Side eachSide in Enum.GetValues(typeof(BlockType.Side)))
            {
                if (Get(eachSide) != World.AirBlock)
                    return true;
            }
            return false;
        }
    }
}