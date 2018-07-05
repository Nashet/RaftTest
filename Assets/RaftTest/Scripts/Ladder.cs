using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaftTest
{
    /// <summary>
    /// Object with that script works as ladder. Object should have collider with IsTrigger on
    /// Climbing object should have RigidBody component
    /// </summary>
    public class Ladder : MonoBehaviour
    {
        [SerializeField] private float climbSpeed;
        void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                other.gameObject.transform.Translate(Vector3.up * climbSpeed);
            }
        }
    }
}