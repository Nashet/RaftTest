using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    [SerializeField] public Placeable block1, block2;
    [SerializeField] private GameObject prefab1, prefab2;
    [SerializeField] public Material originalMat1, originalMat2;

    static Manager thisObject;
    public static Manager Get
    {
        get { return thisObject; }
    }
	// Use this for initialization
	void Start () {
        thisObject = this;
        // could be read from a config file
        block1 = new Placeable(false, prefab1);
        block2 = new Placeable(true, prefab2);
    }	
}
