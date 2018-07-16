using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RaftTest
{
    /// <summary>
    /// As component it gives ability to select & deselect some GameObject with additional material
    /// </summary>
    public class TimedSelectorWithMaterial : MonoBehaviour, ISelector
    {        
        protected Queue<GameObject> recentlySelected = new Queue<GameObject>();

        [Header("Gives ability to select & deselect some GameObject with additional material")]
        [SerializeField] protected Material selectionMaterial;

        [Tooltip("In seconds, zero mean no time limit")]
        [SerializeField] protected float selectionTime;

        /// <summary>
        /// Is forbidden since it's MonoBehaviour
        /// </summary>        
        private TimedSelectorWithMaterial()
        {            
            
        }

        /// <summary>
        /// Use this instead
        /// </summary>        
        public static TimedSelectorWithMaterial AddTo(GameObject toWhom, Material selectionMaterial, float selectionTime)
        {
            var added = toWhom.AddComponent<TimedSelectorWithMaterial>();
            added.selectionMaterial = selectionMaterial;
            added.selectionTime = selectionTime;
            return added;
        }

        protected void Start()
        {
            if (selectionTime != 0f)
                StartCoroutine(CheckSelectionQueue());
        }

        public void Deselect(GameObject someObject)
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

        public void Select(GameObject someObject)
        {
            if (selectionTime != 0f)
                recentlySelected.Enqueue(someObject);
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
        protected void RemoveMaterial(MeshRenderer renderer)
        {
            Material[] newArray = new Material[1];
            newArray[0] = renderer.material;
            renderer.materials = newArray;
        }
        protected void AddMaterial(MeshRenderer renderer)
        {
            Material[] rt = new Material[2];
            rt[0] = renderer.material;
            rt[1] = selectionMaterial;
            renderer.materials = rt;
        }
        protected IEnumerator CheckSelectionQueue()
        {
            while (true)
            {
                if (recentlySelected.Count > 0)
                {
                    var _object = recentlySelected.Dequeue();
                    if (_object != null)
                        Deselect(_object);
                }
                yield return new WaitForSeconds(selectionTime);
            }
        }
    }
}
