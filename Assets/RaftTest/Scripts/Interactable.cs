using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RaftTest
{
    /// <summary>
    /// Shows some GUI if player is close
    /// </summary>
    public class Interactable : MonoBehaviour, IInteractable
    {
        protected GameObject gui;

        [SerializeField] protected float interactionDistance;

        [SerializeField] protected GameObject guiPrefab;        

        protected void Start()
        {
            //if (GetComponent<Placeable>() == null)
            gui = Instantiate(guiPrefab, GManager.Get.InteractableCanvas.transform);
        }

        protected void Update()
        {
            if (GetComponent<PlacedBlock>() != null)
            {
                if (Vector3.Distance(GManager.Get.Player.transform.position, this.transform.position) <= interactionDistance)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance))
                    {
                        if (hit.collider.gameObject == this.gameObject)
                        {
                            gui.SetActive(true);                            
                            return;
                        }
                    }
                }
                gui.SetActive(false);
            }
        }
    }
}
