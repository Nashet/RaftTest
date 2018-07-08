using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RaftTest
{
    /// <summary>
    /// Basic tool class
    /// Select block in a world by changing it's material
    /// </summary>
    [Serializable]
    public class Tool : MonoBehaviour, IHoldable // Placeable
    {
        public event EventHandler<EventArgs> Hidden;
        public event EventHandler<EventArgs> Shown;
        public static event EventHandler<EventArgs> Used;
        protected PlacedBlock selectedObject;

        private Animation animation;

        public void Hide()
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

        public void Show()
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
            rt[1] = GManager.Get.BuildingDeniedMaterial;
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
        public void UpdateBlock()
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                var lookingAt = hit.collider.gameObject;
                var placed = lookingAt.GetComponent<PlacedBlock>();
                if (placed != null)
                {
                    //removing added material from previously selected object
                    if (selectedObject != null)
                    {
                        RemoveSelection(selectedObject.gameObject);
                    }
                    selectedObject = placed;

                    AddSelection(selectedObject.gameObject);
                }
                else if (selectedObject != null)
                {
                    RemoveSelection(selectedObject.gameObject);
                }
            }
        }



        // Use this for initialization
        void Start()
        {
            gameObject.SetActive(false);
            animation = GetComponent<Animation>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Act()
        {
            if (selectedObject != null)
            {
                World.Get.Remove(selectedObject);
                EventHandler<EventArgs> handler = Used;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
            if (animation != null)
                animation.Play();
        }
    }
}