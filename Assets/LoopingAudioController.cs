using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingAudioController : MonoBehaviour
{
    [SerializeField] private LoopingSound loopingSound;
    [SerializeField] private SoundClipController soundClipController;

    public LoopingSound GetLoopingSound()
    {
        return loopingSound;
    }

    public SoundClipController GetSoundClipController()
    {
        return soundClipController;
    }

    public void SetSoundClipController(SoundClipController soundClip)
    {
        soundClipController = soundClip;
    }

    public void SetLoopingSound(LoopingSound looping)
    {
        loopingSound = looping;
    }

    public void OnDisable()
    {
        loopingSound = null;
        soundClipController = null;
    }
}
