using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClipController : MonoBehaviour
{
    [SerializeField] private bool isRunning;
    [SerializeField] private float refreshRate = 0.1f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    public void PlayAudioClip()
    {
        if (!isRunning)
        {
            if (audioSource.clip != null)
            {
                audioSource.Play();
                //audioSource.PlayOneShot(audioSource.clip);
                StartCoroutine("AudioIsPlayingRoutine");
            }
        }
    }

    IEnumerator AudioIsPlayingRoutine()
    {
        isRunning = true;

        while (audioSource.isPlaying)
        {
            yield return new WaitForSeconds(refreshRate);
        }

        isRunning = false;

        PlayAudioOver();
    }

    public void StopPlayAudioClipRoutine()
    {
        isRunning = false;
        StopCoroutine("PlayAudioClip");
    }

    public void PlayAudioOver()
    {
        audioSource.Stop();
        audioSource.clip = null;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        audioSource.loop = false;
    }

}
