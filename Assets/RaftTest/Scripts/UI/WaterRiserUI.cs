using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RaftTest
{
    public class WaterRiserUI : MonoBehaviour
    {

        [SerializeField] protected Button rise, lower;
        // Use this for initialization
        void Start()
        {
            rise.onClick.AddListener(new UnityEngine.Events.UnityAction(GManager.Get.Water.RiseLevel));
            lower.onClick.AddListener(new UnityEngine.Events.UnityAction(GManager.Get.Water.LowerLevel));
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}