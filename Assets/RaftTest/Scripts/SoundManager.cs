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
    public class SoundManager : MonoBehaviour
    {   
        [SerializeField] private AudioClip blockPlacedSound;
        [SerializeField] private AudioClip toolUsedSound;
        private AudioSource audioSource;
        

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                Debug.Log("Missing AudioSource component");
            Placeable.Placed += OnBlockPlaced;
            Tool.Used += OnToolUsed;
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