using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaftTest
{
    /// <summary>
    /// Common interface for objects which can change visibility
    /// </summary>
    public interface IHideable
    {
        event EventHandler<EventArgs> Hidden;

        event EventHandler<EventArgs> Shown;

        void Hide();

        void Show();
    }

    /// <summary>
    /// Implements visibility change for MonoBehaviour objects
    /// Also may rise event "Hidden" or "Shown"
    /// </summary>
    public abstract class Hideable : MonoBehaviour, IHideable
    {
        public event EventHandler<EventArgs> Hidden;
        public event EventHandler<EventArgs> Shown;

        public virtual void Hide()
        {
            gameObject.SetActive(false);

            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<EventArgs> handler = Hidden;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            EventHandler<EventArgs> handler = Shown;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }

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

    
    /// <summary>
    /// Represents objects which can be hold in players hands
    /// </summary>    
    public interface IHoldable : IHideable
    {
        /// <summary>
        /// updates block held by player 
        /// </summary>
        void UpdateBlock();
    }

    /// <summary>
    /// Represents objects with basic humanoid behavior
    /// </summary>
    public interface ICharacter
    {
        void TakeInHand(IHoldable placeable);
        IHoldable Holds { get; }
        void Act();
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
            foreach (var validCoords in array.GetCoordsWithRadius( x, z, radius))
            {
                yield return array[validCoords.x, y, validCoords.y];
            }

        }
    }
}