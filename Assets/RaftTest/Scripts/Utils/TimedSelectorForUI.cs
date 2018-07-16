using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RaftTest.Utils
{
    /// <summary>
    /// As component it gives ability to select & deselect some UI element with a selectionMaterial
    /// </summary>
    public class TimedSelectorForUI : MonoBehaviour, ISelector
    {
        protected Queue<GameObject> recentlySelected = new Queue<GameObject>();

        [Header("Gives ability to select & deselect some GameObject with additional material")]
        [SerializeField] protected Material selectionMaterial;

        [SerializeField] protected Material defaultMaterial;

        [Tooltip("In seconds, zero mean no time limit")]
        [SerializeField] protected float selectionTime;

        /// <summary>
        /// Is forbidden since it's MonoBehaviour
        /// </summary>        
        protected TimedSelectorForUI()
        {

        }

        /// <summary>
        /// Use this instead
        /// </summary>        
        public static TimedSelectorForUI AddTo(GameObject toWhom, Material selectionMaterial, Material defaultMaterial, float selectionTime)
        {
            var added = toWhom.AddComponent<TimedSelectorForUI>();
            added.selectionMaterial = selectionMaterial;
            added.selectionTime = selectionTime;
            added.defaultMaterial = defaultMaterial;
            return added;
        }

        protected void Start()
        {
            if (selectionTime != 0f)
                StartCoroutine(CheckSelectionQueue());
        }

        public void Deselect(GameObject someObject)
        {
            var image = someObject.GetComponent<Image>();
            if (image == null)
            //if there is no render in selected object, find one in childes
            {
                var children = someObject.GetComponentsInChildren<Image>();
                foreach (var item in children)
                {
                    item.material = defaultMaterial;
                }
            }
            else
            {
                image.material = defaultMaterial;
            }
        }

        public void Select(GameObject someObject)
        {
            if (selectionTime != 0f)
                recentlySelected.Enqueue(someObject);
            var image = someObject.GetComponent<Image>();
            if (image == null)
            //if there is no render in selected object, find one in childes
            {
                var children = someObject.GetComponentsInChildren<Image>();
                foreach (var item in children)
                {
                    item.material = selectionMaterial;
                }
            }
            else
            {
                image.material = selectionMaterial;
            }
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
