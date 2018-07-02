using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    [SerializeField] public Placeable material1, material2;
    [SerializeField] GameObject prefab1, prefab2;

    static Manager thisObject;
    public static Manager Get
    {
        get { return thisObject; }
    }
	// Use this for initialization
	void Start () {
        thisObject = this;
        // could be read from a config file
        material1 = new Placeable(true, prefab1);
        material2 = new Placeable(false, prefab2);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
