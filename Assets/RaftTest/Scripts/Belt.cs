using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using RaftTest.Utils;

namespace RaftTest
{
    /// <summary>
    /// Represents player belt where player can select object to use (player.Holdable)
    /// Can have any amount of slots as children. Slots should have Button and EventTrigger components
    /// </summary>
    [RequireComponent(typeof(ICharacter))]
    public class Belt : MonoBehaviour
    {
        protected ICharacter player;
        
        protected ISelector slotSelector;

        /// <summary>
        /// Some child of a canvas
        /// </summary>
        [SerializeField] protected GameObject slotHolder;

        // Use this for initialization
        void Start()
        {
            player = GetComponent<ICharacter>();
            FiilFromGameManager();
            slotSelector = GManager.CheckComponentAvailability<ISelector>(this);
        }
        protected IEnumerable<Button> AllSlots
        {
            get
            {
                foreach (Transform child in slotHolder.transform)
                {
                    var slot = child.GetComponent<Button>();

                    if (slot != null)
                        yield return slot;
                }
            }
        }

        protected void FiilFromGameManager()
        {
            int count = 0;
            foreach (var item in AllSlots)
            {
                var holdable = GManager.Get.AllHoldable().ElementAtOrDefault(count);
                PutInSlot(item, holdable);
                count++;
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
                    foreach (var item in AllSlots)slotSelector.Deselect(item.gameObject);
                    
                    slotSelector.Select(data.selectedObject);
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