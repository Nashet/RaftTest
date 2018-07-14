using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

        public virtual void TakeInHand(IHoldable holdable)
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
        public virtual void Act()
        {
            if (Holds != null)
            {
                var isPlaceable = Holds as IPlaceable;
                if (isPlaceable != null)
                    isPlaceable.Place(World.Get);
                else
                {
                    var isTool = Holds as AbstractTool;
                    if (isTool != null)
                        isTool.Act();
                }
            }
        }
        // Update is called once per frame
        protected void Update()
        {
            if (Holds != null)
                Holds.UpdateBlock();
#if !MOBILE_INPUT
            if (Input.GetMouseButtonUp(0)
            && !EventSystem.current.IsPointerOverGameObject())//hovering over UI) 
            {
                Act();
            }
#endif
            if (debugCube != null)            // places small cube at looking position, for debugging
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {

                    debugCube.transform.position = hit.point;
                }
            }
        }
    }
}