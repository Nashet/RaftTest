using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RaftTest
{
    /// <summary>
    /// Implements specific behavior of a tool - block destroying
    /// </summary>
    public class Hammer : AbstractTool
    {
        public override void Act()
        {
            base.Act();
            if (selectedObject != null)
            {
                World.Get.Remove(selectedObject);
            }
        }
    }
}