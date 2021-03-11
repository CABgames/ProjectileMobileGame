/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///AudioManager.cs
///Developed by Charlie Bullock
///This class simply plays audioclips given into it's PlayClip parameter
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //Variables
    public bool soundMuted = false;
    public bool vibrationEnabled = true;
    private AudioSource aS;
    [SerializeField]
    private AudioClip[] audioTracks;

    //Start function gets the audio source on the main camera
    void Start()
    {
        aS = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }

    //This function simply plays the audioclip given into the parameter and plays it once, it also vibrates the device if it's handheld
    public void PlayClip(AudioClip sound)
    {        
        if (soundMuted == false && aS)
        {
            if (vibrationEnabled && SystemInfo.deviceType == DeviceType.Handheld)
            {
                Handheld.Vibrate();
            }
            aS.PlayOneShot(sound);
        }
    }
}
