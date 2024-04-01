using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource whistle;
    public AudioSource Net;
    public AudioSource Miss;
    public AudioSource Blocked;
    public AudioSource buzz;
    // Start is called before the first frame update
   
    public void PlayeWhistle()
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
    // Update is called once per frame
    public void PlayBuzz()
    {
        buzz.Play();
    }
}
