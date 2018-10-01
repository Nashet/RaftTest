using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaftTest
{
    /// <summary>
    /// React to events playing sound
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    sealed public class SoundManager : MonoBehaviour
    {   
        [SerializeField] private AudioClip blockPlacedSound;
        [SerializeField] private AudioClip toolUsedSound;
        [SerializeField] private AudioClip entityDamaged;
        [SerializeField] private AudioClip entityDead;
        private AudioSource audioSource;
        

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                Debug.Log("Missing AudioSource component");
            Placeable.Placed += OnBlockPlaced;
            AbstractTool.Used += OnToolUsed;
            AbstractHandWeapon.Used += OnToolUsed;
            Breakable.Damaged += OnEntityDamaged;
            Breakable.Destroyed += OnEntityDestroyed;
        }

        private void OnEntityDamaged(object sender, EventArgs e)
        {
            audioSource.clip = entityDamaged;
            audioSource.Play();
        }
        private void OnEntityDestroyed(object sender, EventArgs e)
        {
            audioSource.clip = entityDead;
            audioSource.Play();
        }

        private void OnBlockPlaced(object sender, EventArgs e)
        {
            if (blockPlacedSound == null)
                Debug.Log("blockPlacedSound isn't set");
            else
            {
                audioSource.clip = blockPlacedSound;
                audioSource.Play();
            }
        }
        private void OnToolUsed(object sender, EventArgs e)
        {
            if (toolUsedSound == null)
                Debug.Log("toolUsedSound isn't set");
            else
            {
                audioSource.clip = toolUsedSound;
                audioSource.Play();
            }
        }
    }
}