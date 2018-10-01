using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RaftTest
{
    [RequireComponent(typeof(Collider))]
    class Breakable : MonoBehaviour, IBreakable
    {
        [SerializeField] protected int initialStrength;
        [SerializeField] protected int currentStrength;

        static public event EventHandler<EventArgs> Damaged;
        static public event EventHandler<EventArgs> Destroyed;

        void Start()
        {
            currentStrength = initialStrength;
        }
        public void HitBy(IDamageSource damageSource)
        {
            if (UnityEngine.Random.Range(0, 3) == 0)
            {
                currentStrength -= damageSource.Damage;

                if (currentStrength <= 0)
                {
                    EventHandler<EventArgs> handler = Destroyed;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                    Destroy(gameObject);
                }
                else
                {
                    EventHandler<EventArgs> handler = Damaged;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}
