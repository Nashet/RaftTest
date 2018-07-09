using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
namespace RaftTest
{
    /// <summary>
    /// Allows FPS camera to build blocks
    /// </summary>
    public class Builder : MonoBehaviour, ICharacter
    {
        /// <summary>
        /// Whatever players holds in hands
        /// </summary>
        public IHoldable Holds { get; private set; }

        [SerializeField] private GameObject debugCube;

        public void TakeInHand(IHoldable holdable)//todo refactor with prefabs?
        {
            if (Holds != null) // hides previous object in hands
            {
                Holds.Hide();
            }
            Holds = holdable;
            if (Holds != null) // shows new object in hands
            {
                Holds.Show();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Holds != null)
                Holds.UpdateBlock();

            // places small cube at looking position, for debugging
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                debugCube.transform.position = hit.point;
            }
        }
    }
}