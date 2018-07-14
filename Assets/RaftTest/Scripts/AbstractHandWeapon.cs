using RaftTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RaftTest
{
    [Serializable]    
    public abstract class AbstractHandWeapon : Hideable, IHandWeapon
    {
        private Animation _animation;
        public static event EventHandler<EventArgs> Used;

        protected void Start()
        {
            _animation = GetComponent<Animation>();
        }

        public virtual void UpdateBlock()
        {

        }

        public virtual void Act()
        {
            EventHandler<EventArgs> handler = Used;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
            if (_animation != null)
                _animation.Play();
        }

        public override string ToString()
        {
            return name;
        }
    }
}
