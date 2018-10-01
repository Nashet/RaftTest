using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RaftTest.Utils
{
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
}
