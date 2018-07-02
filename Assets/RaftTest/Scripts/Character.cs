using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Character : FirstPersonController
{
    [SerializeField] Placeable holds;
    [SerializeField] private Camera camera;
   
    void TakeInHand(Placeable placeable)
    {
        if (holds != null)
            holds.gameObject.SetActive(false);
        holds = placeable;
        if (holds != null)
            holds.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (holds != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
                holds.gameObject.transform.position = hit.point;
            //if (EventSystem.current.IsPointerOverGameObject())
            //    return null;// -3; //hovering over UI
            if (Input.GetMouseButtonUp(0))
                //new GameObject(objectInHands);
                Object.Instantiate(holds.gameObject);
        }
        if (Input.GetKeyUp(KeyCode.F1))
            TakeInHand(Manager.Get.material1);
        else
        if (Input.GetKeyUp(KeyCode.F2))
            TakeInHand(Manager.Get.material2);
        else
        if (Input.GetKeyUp(KeyCode.F3))
            TakeInHand(null);
    }

}
