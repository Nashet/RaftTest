using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingProjection : MonoBehaviour
{    
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject objectInHands;
    // Use this for initialization
    void Start()
    {
       
    }
    
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
                 objectInHands.transform.position = hit.point;
        //if (EventSystem.current.IsPointerOverGameObject())
        //    return null;// -3; //hovering over UI
        if (Input.GetMouseButtonUp(0))
            //new GameObject(objectInHands);
            Object.Instantiate(objectInHands);
    }
    
}
