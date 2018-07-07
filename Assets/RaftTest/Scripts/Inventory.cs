using System;
using System.Collections;
using System.Collections.Generic;
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
        void Start()
        {
            player = GetComponent<Builder>();
            if (player == null)
                Debug.Log("Missing Builder component");
        }

        // Update is called once per frame
        void Update()
        {
            ManageControls();
        }
        void ManageControls()
        {
            // selects block 
            if (Input.GetKeyUp(KeyCode.F1))
                player.TakeInHand(null);
            else if (Input.GetKeyUp(KeyCode.F2))
                player.TakeInHand(GManager.Get.allBlocks[0]);
            else if (Input.GetKeyUp(KeyCode.F3))
                player.TakeInHand(GManager.Get.allBlocks[1]);
            else if (Input.GetKeyUp(KeyCode.F4))
                player.TakeInHand(GManager.Get.allBlocks[2]);
            else if (Input.GetKeyUp(KeyCode.F5))
                player.TakeInHand(GManager.Get.allBlocks[3]);
            else if (Input.GetKeyUp(KeyCode.F6))
                player.TakeInHand(GManager.Get.allBlocks[4]);
            else if (Input.GetKeyUp(KeyCode.F7))
                player.TakeInHand(GManager.Get.allBlocks[5]);

            if (player.Holds != null && Input.GetMouseButtonUp(0)) // place block in a world
            {
                var isPlaceable = player.Holds as Placeable;
                if (isPlaceable != null)
                    isPlaceable.PlaceBlock(World.Get);
            }
        }
    }
}