using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaftTest.Utils
{
    /// <summary>
    /// Each descendant will have a name
    /// </summary>
    public abstract class Nameable
    {
        [SerializeField] protected string name;

        /// <summary>
        /// for better debug
        // </summary>    
        override public string ToString()
        {
            return name;
        }
    }
    
    public static class ArrayExtensions
    {
        /// <summary>
        /// Gives valid coordinates of array elements within specified radius of some point. 
        /// </summary>        
        public static IEnumerable<Vector2Int> GetCoordsWithRadius(int xLength, int zLength, int x, int z, int radius)
        {
            // scan neighbor cells for support            
            int xStart = x - radius;
            int xEnd = x + radius;
            int zStart = z - radius;
            int zEnd = z + radius;

            if (xStart < 0) xStart = 0;
            if (zStart < 0) zStart = 0;
            if (xEnd > xLength - 1) xEnd = xLength - 1;
            if (zEnd > zLength - 1) zEnd = zLength - 1;
            for (int i = xStart; i <= xEnd; i++)
                for (int j = zStart; j <= zEnd; j++)
                    yield return new Vector2Int(i, j);
        }
        /// <summary>
        /// Gives valid coordinates of array elements within specified radius of some point. 
        /// </summary>        
        public static IEnumerable<Vector2Int> GetCoordsWithRadius<T>(this T[,,] array, int x, int z, int radius)
        {
            return GetCoordsWithRadius(array.GetLength(0), array.GetLength(2), x, z, radius);
        }
        /// <summary>
        /// Gives elements of array within specified radius of some point
        /// </summary>        
        public static IEnumerable<T> GetElementsWithRadius<T>(this T[,,] array, int x, int y, int z, int radius)
        {
            foreach (var validCoords in array.GetCoordsWithRadius(x, z, radius))
            {
                yield return array[validCoords.x, y, validCoords.y];
            }

        }
    }
}