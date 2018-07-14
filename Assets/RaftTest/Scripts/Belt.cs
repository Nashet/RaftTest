using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

namespace RaftTest
{
    /// <summary>
    /// Represent player belt where player can select object can select object to use (player.Holdable)
    ///Can have any amount of slots as child. Slot should have Button and EventTrigger components
    /// </summary>
    public class Belt : MonoBehaviour
    {
        [SerializeField] protected Builder player;

        // Use this for initialization
        void Start()
        {
            FiilFromGameManager();
        }

        protected void FiilFromGameManager()
        {
            int count = 0;
            foreach (Transform child in transform)
            {
                var slot = child.GetComponent<Button>();

                if (slot != null)
                {
                    var holdable = GManager.Get.AllHoldable().ElementAtOrDefault(count);
                    PutInSlot(slot, holdable);
                    count++;
                }
            }
        }

        protected void PutInSlot(Button slot, IHoldable holdable)
        {
            if (slot != null)
            {
                var trigger = slot.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerUp;
                entry.callback.AddListener((data) =>
                {
                    player.TakeInHand(holdable);
                });
                trigger.triggers.Add(entry);

                // set name to slot
                var text = slot.GetComponentInChildren<Text>();
                if (text != null)
                    text.text = holdable == null ? "Empty" : holdable.ToString();
            }
        }
    }
}