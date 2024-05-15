using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioSource whistle;
    public AudioSource Net;
    public AudioSource Miss;
    public AudioSource Blocked;
    public AudioSource buzz;
    public AudioMixer audioMixer; // Reference to the Audio Mixer

    public Slider volumeSlider; // Reference to the volume slider UI

    // Start is called before the first frame update
    void Start()
    {
        audioMixer.SetFloat("sound",  20f);
    }

    public void PlayWhistle()
    {
        whistle.Play();
    }

    public void PlayNet()
    {
        Net.Play();
    }

    public void PlayMiss()
    {
        Miss.Play();
    }

    public void PlayBlocked()
    {
        Blocked.Play();
    }

    public void PlayBuzz()
    {
        buzz.Play();
    }

    // Function to set master volume
    public void SetMasterVolume()
    {
        float volume = volumeSlider.value;
        Debug.Log(" volume:" + volume);
        Debug.Log("sound volume:"+ Mathf.Log10(volume) * 20);
        audioMixer.SetFloat("sound", Mathf.Log10(volume)*20);
    }

    // Function to mute or unmute master volume
    public void SetMasterMute(bool isMuted)
    {
        float muteValue = isMuted ? -80 : 0; // -80 dB means muted
        audioMixer.SetFloat("Master", muteValue);
    }

    // Function to get current master volume
    public float GetMasterVolume()
    {
        float volume;
        audioMixer.GetFloat("Master", out volume);
        return Mathf.Pow(10, volume / 20); // Convert logarithmic value to linear
    }
}
