using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

/// <summary>
/// Allows FPS camera to build blocks
/// </summary>
public class Builder : MonoBehaviour
{
    /// <summary>
    /// Whatever players holds in hands
    /// </summary>
    private Placeable holds;

    [SerializeField] private GameObject debugCube;

    void TakeInHand(Placeable placeable)//todo refactor with prefabs?
    {
        if (holds != null) // hides previous object in hands
        {
            holds.Hide();
        }
        holds = placeable;
        if (holds != null) // shows new object in habds
        {
            holds.Show();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (holds != null)
            holds.UpdateHoldingBlock();

        ManageControls();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            debugCube.transform.position = hit.point;
        }
    }

    void ManageControls()
    {
        // selects block 
        if (Input.GetKeyUp(KeyCode.F1))
            TakeInHand(null);
        else if (Input.GetKeyUp(KeyCode.F2))
            TakeInHand(GManager.Get.allBlocks[0]);
        else if (Input.GetKeyUp(KeyCode.F3))
            TakeInHand(GManager.Get.allBlocks[1]);
        else if (Input.GetKeyUp(KeyCode.F4))
            TakeInHand(GManager.Get.allBlocks[2]);
        else if (Input.GetKeyUp(KeyCode.F5))
            TakeInHand(GManager.Get.allBlocks[3]);
        else if (Input.GetKeyUp(KeyCode.F6))
            TakeInHand(GManager.Get.allBlocks[4]);

        if (Input.GetMouseButtonUp(0)) // place block in a world
        {
            holds.PlaceBlock(World.Get);
        }
    }
}
