using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioListenerManager : MonoBehaviour
{
    [SerializeField] private List<float> volumeSteps = new List<float>();
    [SerializeField] private int volumeIndex = 0;

    public event Action<float> OnVolumeChange;

    public static AudioListenerManager instance;

    private void Awake()
    {
        instance = this;
        AudioListener.volume = 0;
    }

    //Temporary
    private void Start()
    {
        Invoke("TemptEnableVolume", 0.1f);
    }

    //Temporary
    public void TemptEnableVolume()
    {
        AudioListener.volume = 1;
    }

    public void UpdateVolume()
    {
        if(volumeIndex < volumeSteps.Count - 1)
            volumeIndex++;
        else
            volumeIndex = 0;

        float newVol = AudioListener.volume;
        newVol = volumeSteps[volumeIndex];
        AudioListener.volume = newVol;

        if (OnVolumeChange != null)
            OnVolumeChange(volumeSteps[volumeIndex]);
    }

    public List<float> GetVolumeSteps()
    {
        return volumeSteps;
    }

    public int GetVolumeIndex()
    {
        return volumeIndex;
    }
}
