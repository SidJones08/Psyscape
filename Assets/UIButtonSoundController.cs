using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonSoundController : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    private AudioListenerManager audioListenerManager;

    private void Start()
    {
        audioListenerManager = AudioListenerManager.instance;
        audioListenerManager.OnVolumeChange += UpdateFill;
    }

    private void UpdateFill(float value)
    {
        fillImage.fillAmount = value;
    }
}
