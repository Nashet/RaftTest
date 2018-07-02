using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingController : MonoBehaviour
{
    private MeshRenderer renderer;
    [SerializeField] private Material buildingDenial;
    public bool canBePlaced = true;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }
    void OnTriggerEnter(Collider col)
    {
        renderer.material = buildingDenial;
        canBePlaced = false;
    }
    void OnTriggerExit(Collider col)
    {
        if (this.gameObject == Manager.Get.block1.gameObject)
            renderer.material = Manager.Get.originalMat1;//  originalColor;
        else
            renderer.material = Manager.Get.originalMat2;//  originalColor;
        canBePlaced = true;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
