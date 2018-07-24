using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RaftTest
{
    [RequireComponent(typeof(Rigidbody))]
    public class Floatable : MonoBehaviour
    {
        //[SerializeField]
        //protected float waterLevel;
        [SerializeField]
        protected Vector3 buoyancyCentreOffset;

        [SerializeField]
        protected float bouncing;

        protected Rigidbody rigidBody;
        // Use this for initialization
        void Start()
        {
            rigidBody = GManager.CheckComponentAvailability<Rigidbody>(this);
        }

        void FixedUpdate()
        {
            Vector3 actionPoint = transform.position + transform.TransformDirection(buoyancyCentreOffset);
            float forceFactor = GManager.Get.Water.Level- actionPoint.y;// 1f - ((actionPoint.y - waterLevel) / floatHeight);

            if (forceFactor > 0f)
            {
                Vector3 uplift = -Physics.gravity * (forceFactor * bouncing);// - rigidBody.velocity.y * bounceDamp
                rigidBody.AddForceAtPosition(uplift, actionPoint);
            }
        }
    }

}