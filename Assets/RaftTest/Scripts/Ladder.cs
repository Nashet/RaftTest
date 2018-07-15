using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaftTest
{
    [RequireComponent(typeof(Collider))]
    /// <summary>
    /// Object with that script works as ladder. This object should have collider with IsTrigger on    
    /// </summary>    
    public class Ladder : MonoBehaviour
    {
        [SerializeField] private float climbSpeed;
        protected void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                other.gameObject.transform.Translate(Vector3.up * climbSpeed);
            }
        }
    }
}