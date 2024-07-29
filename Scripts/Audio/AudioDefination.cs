using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    public PlayAudioEventSO playAudioEvent;
    public AudioClip audioClip;
    public bool playOnEnable;
    private void OnEnable()
    {
        if (playOnEnable)
        {
            playAudioClip();
        }
    }

    private void playAudioClip()
    {
        playAudioEvent.OnEventRaised(audioClip);
    }
}
