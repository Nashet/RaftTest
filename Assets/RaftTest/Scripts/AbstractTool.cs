using RaftTest.Utils;
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
    abstract public class AbstractTool : Hideable, ITool, ISelector
    {        
        public static event EventHandler<EventArgs> Used;

        protected PlacedBlock selectedObject;

        protected ISelector selectorComponent;
        /// <summary>
        /// Plays animation on act, if presents
        /// </summary>
        private Animation availableAnimation;

        // Use this for initialization
        protected void Start()
        {
            //gameObject.SetActive(false);
            availableAnimation = GetComponent<Animation>();
            selectorComponent = GManager.CheckComponentAvailability<ISelector>(this);
            Hide();
        }
        public override void Hide()
        {            
            base.Hide();
            if (selectedObject != null)
                Deselect(selectedObject.gameObject);
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
                    var placed = hit.collider.gameObject.GetComponent<PlacedBlock>();
                    if (placed == null)
                    {
                        if (selectedObject != null)
                        {
                            Deselect(selectedObject.gameObject);
                        }
                        selectedObject = null;

                    }
                    else
                    {
                        //removing added material from previously selected object
                        if (selectedObject != null)
                        {
                            Deselect(selectedObject.gameObject);
                        }
                        selectedObject = placed;

                        Select(selectedObject.gameObject);
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

        public void Select(GameObject someObject)
        {
            selectorComponent.Select(someObject);
        }

        public void Deselect(GameObject someObject)
        {
            selectorComponent.Deselect(someObject);
        }
    }
}