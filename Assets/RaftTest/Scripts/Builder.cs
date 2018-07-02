using System.Collections;
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

    // Update is called once per frame
    void Update()
    {
        if (holds != null)
        {
            // updates holding block position
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                var placingPosition = hit.point;

                placingPosition.x = (int)placingPosition.x * World.Get.blockSize;
                placingPosition.y = (int)placingPosition.y * World.Get.blockSize;
                placingPosition.z = (int)placingPosition.z * World.Get.blockSize;
                placingPosition.y += World.Get.blockSize / 2f; // adding half of standard height
                holds.gameObject.transform.position = placingPosition;
                //Debug.Log("Looking at (x,z,y)" + coordinats.x + " " + coordinats.z + " " + coordinats.y);
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
