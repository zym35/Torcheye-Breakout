using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _source;
    
    public static AudioManager Instance { get; private set; }
    public AudioClip barBounce;
    
    private void Awake()
    {
        //Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlaySFX(String clip)
    {
        switch (clip)
        {
            case "barBounce":
                _source.PlayOneShot(barBounce);
                break;
        }
    }
}
