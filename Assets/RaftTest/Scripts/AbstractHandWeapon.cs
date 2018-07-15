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
    public abstract class AbstractHandWeapon : Hideable, IHandWeapon
    {
        private Animation _animation;

        protected Queue<GameObject> recentHit = new Queue<GameObject>();

        [SerializeField] protected int damage;
        public int Damage { get { return damage; } }

        [SerializeField] protected float damageDistance;
        public float DamageDistance { get { return damageDistance; } }

        [Tooltip("In seconds")]
        [SerializeField]protected float selectionTime ;


        public static event EventHandler<EventArgs> Used;

        protected void Start()
        {
            _animation = GetComponent<Animation>();
            StartCoroutine(CheckSelectionQueue());
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
                        AddSelection(isMonoBehavior.gameObject);

                }

            }
            if (_animation != null)
                _animation.Play();
        }

        public override string ToString()
        {
            return name;
        }


        protected void RemoveMaterial(MeshRenderer renderer)
        {
            Material[] newArray = new Material[1];
            newArray[0] = renderer.material;
            renderer.materials = newArray;
        }

        protected void RemoveSelection(GameObject someObject)
        {
            var renderer = someObject.GetComponent<MeshRenderer>();
            if (renderer == null)
            //if there is no render in selected object, find one in childes
            {
                var children = someObject.GetComponentsInChildren<MeshRenderer>();
                foreach (var item in children)
                {
                    RemoveMaterial(item);
                }
            }
            else
            {
                RemoveMaterial(renderer);
            }

        }

        protected void AddMaterial(MeshRenderer renderer)
        {
            Material[] rt = new Material[2];
            rt[0] = renderer.material;
            rt[1] = GManager.Get.SelectedByToolMaterial;
            renderer.materials = rt;
        }

        protected void AddSelection(GameObject someObject)
        {
            recentHit.Enqueue(someObject);
            var renderer = someObject.GetComponent<MeshRenderer>();
            if (renderer == null)
            //if there is no render in selected object, find one in childes
            {
                var children = someObject.GetComponentsInChildren<MeshRenderer>();
                foreach (var item in children)
                {
                    AddMaterial(item);
                }
            }
            else
            {
                AddMaterial(renderer);
            }

        }

        public void UpdateBlock()
        {
            if (Input.touchCount > 0)
                Act();
        }
        protected IEnumerator CheckSelectionQueue()
        {
            while (true)
            {
                if (recentHit.Count > 0)
                {
                    var _object = recentHit.Dequeue();
                    if (_object != null)
                        RemoveSelection(_object);
                }
                yield return new WaitForSeconds(selectionTime);
            }
        }
    }
}
