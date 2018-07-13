using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RaftTest
{
    /// <summary>
    /// Player's inventory.
    /// just a sketch, not a real inventory
    /// </summary>
    [RequireComponent(typeof(Builder))]
    public class Inventory : MonoBehaviour
    {
        private ICharacter player;        
        
        protected void Start()
        {
            player = GetComponent<Builder>();            
            if (player == null)
                Debug.Log("Missing Builder component");
            player.TakeInHand(GManager.Get.AllBlocks().ElementAt(1));
        }

        // Update is called once per frame
        protected void Update()
        {
            if (player == null)
                player = GetComponent<Builder>();// for fast reloading
            ManageControls();
        }
        protected void ManageControls()
        {
            // selects block 
            if (Input.GetKeyUp(KeyCode.F1))
                player.TakeInHand(null);
            else if (Input.GetKeyUp(KeyCode.F2))
                player.TakeInHand(GManager.Get.AllBlocks().ElementAt(0));
            else if (Input.GetKeyUp(KeyCode.F3))
                player.TakeInHand(GManager.Get.AllBlocks().ElementAt(1));
            else if (Input.GetKeyUp(KeyCode.F4))
                player.TakeInHand(GManager.Get.AllBlocks().ElementAt(2));
            else if (Input.GetKeyUp(KeyCode.F5))
                player.TakeInHand(GManager.Get.AllBlocks().ElementAt(3));
            else if (Input.GetKeyUp(KeyCode.F6))
                player.TakeInHand(GManager.Get.AllBlocks().ElementAt(4));
            else if (Input.GetKeyUp(KeyCode.F7))
                player.TakeInHand(GManager.Get.AllBlocks().ElementAt(5));
            else if (Input.GetKeyUp(KeyCode.F8))
                player.TakeInHand(GManager.Get.Hammer);

            
        }        
    }
}