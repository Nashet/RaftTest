using RaftTest.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RaftTest
{
    
    [Serializable]
    [RequireComponent(typeof(ISelector))]
    public abstract class AbstractHandWeapon : Hideable, IHandWeapon
    {
        private Animation _animation;

        protected ISelector selectorComponent;

        [SerializeField] protected int damage;
        public int Damage { get { return damage; } }

        [SerializeField] protected float damageDistance;
        public float DamageDistance { get { return damageDistance; } }

        public static event EventHandler<EventArgs> Used;

        protected void Start()
        {
            _animation = GetComponent<Animation>();
            selectorComponent = GManager.CheckComponentAvailability<ISelector>(this);
            Hide();
        }


        public virtual void Act()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, damageDistance))
            {

                var aimAt = hit.collider.gameObject.GetComponent<IBreakable>();
                if (aimAt != null)
                {
                    EventHandler<EventArgs> handler = Used;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                    aimAt.HitBy(this);

                    var isMonoBehavior = aimAt as MonoBehaviour;
                    if (isMonoBehavior != null)
                        selectorComponent.Select(isMonoBehavior.gameObject);

                }

            }
            if (_animation != null)
                _animation.Play();
        }

        public override string ToString()
        {
            return name;
        }

        public void UpdateBlock()
        {
            if (Input.touchCount > 0)
                Act();
        }
    }
}
