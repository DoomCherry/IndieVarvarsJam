using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource startClip;
    public AudioSource loopClip;

    private void Update()
    {
        if (!startClip.isPlaying)
        {
            loopClip.Play();
            Destroy(this);
        }
    }
}
