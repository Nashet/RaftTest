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
        private Placeable[,] container;
        private Placeable centralBlock;
        /// <summary>
        /// Necessary to call
        /// </summary>
        public void Init(Placeable block)
        {
            container = new Placeable[2, 2];
            container[0, 0] = block;
            container[0, 1] = block;
            container[1, 0] = block;
            container[1, 1] = block;
            centralBlock = block;
        }

        public Placeable Get(Vector2Int side) // Convert Vector2Int into specialized Struct?
        {
            if (side == Vector2Int.zero)
                return centralBlock;
            Vector2Int convertedCoords = ConvertCoords(side);
            return container[convertedCoords.x, convertedCoords.y];
        }

        public void Place(Placeable block, Vector2Int side)
        {
            if (side == Vector2Int.zero)
            {
                centralBlock = block;
                Debug.Log("Placed block in (x,y,z)" + block.GetIntegerCoords() + " with snapping " + side);
            }
            else
            {
                Vector2Int convertedCoords = ConvertCoords(side);
                container[convertedCoords.x, convertedCoords.y] = block;
                Debug.Log("Placed block in (x,y,z)" + block.GetIntegerCoords() + " with snapping " + convertedCoords);
            }
        }
        /// <summary>
        /// Converts coords from (-1..1,-1..1) format to normal array format (0..1, 0..1)
        /// </summary>    
        private Vector2Int ConvertCoords(Vector2Int side)
        {
            // if side is center (0,0) it stays (0,0)

            if (side.x < 0)
            {
                side.x = 1;
                side.y = 1;
            }
            if (side.y < 0)
            {
                side.y = 0;
                side.y = 0;
            }
            return side;
        }

        internal void Remove(Vector2Int side)
        {
            if (side == Vector2Int.zero)
            {
                centralBlock = World.AirBlock;                 
            }
            else
            {
                Vector2Int convertedCoords = ConvertCoords(side);
                container[convertedCoords.x, convertedCoords.y] = World.AirBlock;                
            }
        }
    }
}