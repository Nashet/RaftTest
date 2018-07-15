using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaftTest
{
    /// <summary>
    /// On Start teleports this object in good position for spawning
    /// If can't find good spot after amountOfAttemptsToFindProperSpawn attempts teleports to (0,0,0)
    /// </summary>
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private int amountOfAttemptsToFindProperSpawn;
        // Use this for initialization
        void Start()
        {
            TryToSpawn();
        }
        public void TryToSpawn()
        {
            gameObject.transform.position = World.Get.GetRandomSpawnPoint(amountOfAttemptsToFindProperSpawn) + Vector3Int.up * 4;
        }

    }
}