using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RaftTest
{
    public class Water : MonoBehaviour
    {
        public float Level { get { return gameObject.transform.position.y; } }
        public void RiseLevel()
        {
            gameObject.transform.position += Vector3.up;
        }
        public void LowerLevel()
        {
            gameObject.transform.position += Vector3.down;
        }
    }
}
