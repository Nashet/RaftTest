﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

/// <summary>
/// Allows FPS to build blocks
/// </summary>
public class Builder : MonoBehaviour
{
    /// <summary>
    /// Whatever players holds in hands
    /// </summary>
    [SerializeField] Placeable holds;

    [SerializeField] private Material buildingDenialMaterial;

    void TakeInHand(Placeable placeable)
    {
        if (holds != null)
        {
            holds.gameObject.SetActive(false);
        }
        holds = placeable;
        if (holds != null)
        {
            holds.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// updates holding block position
    /// </summary>
    void UpdateHoldingBlock()
    {
        if (holds != null)
        {
            
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Vector3 lookingPosition = World.AdjustCoords(hit.point);
                Vector3 blockPlacingPosition;
                //Debug.Log("Looking at (x,z,y)" + lookingPosition.x + " " + lookingPosition.z + " " + lookingPosition.y);
                Debug.Log("Looking at (x,z,y)" + hit.point.x + " " + hit.point.z + " " + hit.point.y);


                // removing fractional part placing blocks in grid 
                blockPlacingPosition.x = Mathf.FloorToInt(lookingPosition.x);
                blockPlacingPosition.y = Mathf.FloorToInt(lookingPosition.y);
                blockPlacingPosition.z = Mathf.FloorToInt(lookingPosition.z);

                if (holds.AllowsMultipleObjectsInCell)
                {
                    Vector2Int lookingAtSide = Placeable.GetClosestSide(lookingPosition, blockPlacingPosition);

                    blockPlacingPosition.x += lookingAtSide.x * (0.5f - holds.BlockThickness / 2f);
                    blockPlacingPosition.z += lookingAtSide.y * (0.5f - holds.BlockThickness / 2f);
                    if (lookingAtSide.y == 0) // rotate block if it's closer to y side
                        holds.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    else
                        holds.gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                }

                //Debug.Log("Looking at (x,z,y)" + coordinates.x + " " + coordinates.z + " " + coordinates.y);
                holds.gameObject.transform.position = blockPlacingPosition;
            }

            //if (EventSystem.current.IsPointerOverGameObject())
            //    return null;// -3; //hovering over UI
            // updates holding block color 
            if (World.Get.CanBePlaced(holds))
            {
                if (Input.GetMouseButtonUp(0))
                    World.Get.PlaceBlock(holds);
                if (holds.gameObject == GManager.Get.block1.gameObject)
                    holds.renderer.material = GManager.Get.originalMat1;//  originalColor;
                else
                    holds.renderer.material = GManager.Get.originalMat2;//  originalColor;
            }
            else
            {
                holds.renderer.material = buildingDenialMaterial;

            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdateHoldingBlock();
        // selects block 
        if (Input.GetKeyUp(KeyCode.F1))
            TakeInHand(GManager.Get.block1);
        else
        if (Input.GetKeyUp(KeyCode.F2))
            TakeInHand(GManager.Get.block2);
        else
        if (Input.GetKeyUp(KeyCode.F3))
            TakeInHand(null);
    }

}
