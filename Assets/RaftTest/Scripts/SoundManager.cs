using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaftTest
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {   
        [SerializeField] private AudioClip blockPlacedSound;
        private AudioSource audioSource;
        [SerializeField] public Placeable dffl;

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                Debug.Log("Missing AudioSource component");
            Placeable.Placed += OnBlockPlaced;
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
    }
}