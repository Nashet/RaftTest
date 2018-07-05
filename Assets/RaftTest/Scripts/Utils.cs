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

        //declare event of type delegate
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
    interface ICharacter
    {
        void TakeInHand(IHoldable placeable);
        IHoldable Holds { get; }
    }
}