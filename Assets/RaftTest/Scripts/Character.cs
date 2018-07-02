using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

/// <summary>
/// Extends standard Unity class
/// </summary>
public class Character : FirstPersonController
{
    /// <summary>Minimal block size, default is 1, in Unity units</summary>
    [SerializeField] private int blockSize;
    [SerializeField] Placeable holds;
    [SerializeField] private Camera camera;
    private Material building;
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
    void PlaceBlock()
    {
        var newBlock = Object.Instantiate(holds.gameObject);
        Destroy(newBlock.GetComponent<PlacingController>());// 
        newBlock.layer = 0;
    }
    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (holds != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                var placingPosition = hit.point;
                placingPosition.y += blockSize / 2f; // adding half of standard height
                placingPosition.x = (int)placingPosition.x / blockSize;
                placingPosition.y = (int)placingPosition.y / blockSize;
                placingPosition.z = (int)placingPosition.z / blockSize;
                holds.gameObject.transform.position = placingPosition;
            }
            //if (EventSystem.current.IsPointerOverGameObject())
            //    return null;// -3; //hovering over UI
            if (Input.GetMouseButtonUp(0))
                if (holds.gameObject.GetComponent<PlacingController>().canBePlaced)
                    PlaceBlock();

        }
        if (Input.GetKeyUp(KeyCode.F1))
            TakeInHand(Manager.Get.block1);
        else
        if (Input.GetKeyUp(KeyCode.F2))
            TakeInHand(Manager.Get.block2);
        else
        if (Input.GetKeyUp(KeyCode.F3))
            TakeInHand(null);
    }

}
