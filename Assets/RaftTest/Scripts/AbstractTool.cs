using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RaftTest
{
    /// <summary>
    /// Basic tool class
    /// Selects block in a world by changing it's material
    /// Can't be instantiated
    /// </summary>
    [Serializable]
    abstract public class AbstractTool : MonoBehaviour, ITool
    {
        public event EventHandler<EventArgs> Hidden;
        public event EventHandler<EventArgs> Shown;
        public static event EventHandler<EventArgs> Used;

        protected PlacedBlock selectedObject;

        /// <summary>
        /// Plays animation on act, if presents
        /// </summary>
        private Animation availableAnimation;

        // Use this for initialization
        protected void Start()
        {
            //gameObject.SetActive(false);
            availableAnimation = GetComponent<Animation>();
        }
        public virtual void Hide()
        {
            gameObject.SetActive(false);
            if (selectedObject != null)
                RemoveSelection(selectedObject.gameObject);
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
        public virtual void UpdateBlock()
        {
#if MOBILE_INPUT // ignore touches over UI on mobile devices
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {

                    var lookingAt = hit.collider.gameObject;
                    var placed = lookingAt.GetComponent<PlacedBlock>();
                    if (placed == null)
                    {
                        if (selectedObject != null)
                        {
                            RemoveSelection(selectedObject.gameObject);
                        }
                        selectedObject = null;

                    }
                    else
                    {
                        //removing added material from previously selected object
                        if (selectedObject != null)
                        {
                            RemoveSelection(selectedObject.gameObject);
                        }
                        selectedObject = placed;

                        AddSelection(selectedObject.gameObject);
                    }
                }
            }
        }


        /// <summary>
        /// makes main action of a tool
        /// </summary>
        public virtual void Act()
        {
            if (selectedObject != null)
            {
                EventHandler<EventArgs> handler = Used;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
            if (availableAnimation != null)
                availableAnimation.Play();
        }
        public override string ToString()
        {
            return name;
        }
    }
}