using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{

    [SerializeField] private float climbSpeed;
    
    void OnTriggerStay(Collider other)
    {     
        //if (collision.gameObject.GetComponent<Builder>()!= )
        {
            var position = other.gameObject.transform.position;
            position.y += climbSpeed;            
            other.transform.position = position;            
        }
    }
}
