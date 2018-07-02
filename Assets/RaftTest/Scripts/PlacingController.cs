using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingController : MonoBehaviour {

    private MeshRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name != "Plane")
        {
            renderer.material.color = Color.red;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name != "Plane")
        {
            renderer.material.color = Color.green;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
