using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonTimerCanvasController : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private ButtonTimerController buttonTimer;

    private void Awake()
    {
        buttonTimer.OnTimerStart += TimerStart;
        buttonTimer.OnTimerUpdate += TimerUpdate;
        buttonTimer.OnTimerEnd += TimerEnd;
    }

    private void Start()
    {
        timeText.gameObject.SetActive(false);
    }

    public void TimerStart(LoopingSound loopingSound, ButtonController buttonController, Vector3 pos)
    {
        timeText.gameObject.SetActive(true);
    }

    public void TimerUpdate(ButtonController buttonController, Vector3 pos)
    {
        float time = (int)(buttonTimer.GetReleaseDelay() - buttonTimer.GetReleaseElapsed());
        timeText.text = time.ToString();
    }

    public void TimerEnd(LoopingSound loopingSound, ButtonController buttonController, Vector3 pos)
    {
        timeText.gameObject.SetActive(false);
    }
}
