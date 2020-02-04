using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

   

    [System.Serializable]
    public class unitySound
    {

        public AudioClip AudioClip;
        [HideInInspector]
        public AudioSource AudioSource;

        public string name;

    }


    public unitySound[] unitySounds;


    public bool soundON;

	void Start () 
    {
        soundON = true;

        foreach (var item in unitySounds)
        {
            item.AudioSource = gameObject.AddComponent<AudioSource>();
            item.AudioSource.clip = item.AudioClip;
        }

    }


	


    public void PlayUnity(string name,float volume=1)
    {

        if (soundON)
        {
            unitySound s = Array.Find(unitySounds, sound => sound.name == name);
            s.AudioSource.volume = volume;
            s.AudioSource.Play();
        }

    }


  
    }
	

