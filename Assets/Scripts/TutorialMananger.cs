using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public class TutorialMananger : MonoBehaviour
{
    private UpgradeStatusController upgradeStatusController;
    [SerializeField] private GameObject tutorial; 
    [SerializeField] private UnityEngine.Video.VideoPlayer videoPlayer;

    public event Action<TutorialMananger, Vector3> OnTutorialComplete;

    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] List<Tutorial> tutorials = new List<Tutorial>();

    private void Start()
    {
        upgradeStatusController = UpgradeStatusController.instance;
    }

    public void UpdateTutorial(int index)
    {
        title.text = tutorials[index].Title;
        description.text = tutorials[index].Description;
        videoPlayer.clip = tutorials[index].VideoClip;
    }

    public void EnableTutorial(int updateIndex)
    {
        tutorial.gameObject.SetActive(true);
        UpdateTutorial(updateIndex);
        PlayVideo();
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            CloseTutorialManager();
    }

    public void CloseTutorialManager()
    {
        if (tutorial.activeInHierarchy)
        {
            if (OnTutorialComplete != null)
                OnTutorialComplete(this, transform.position);

            title.text = "";
            description.text = "";

            videoPlayer.Stop();
            videoPlayer.clip = null;
            tutorial.SetActive(false);
        }
    }

    [System.Serializable]
    public class Tutorial 
    {
        public string Title;
        [TextArea(4, 10)] public string Description;
        public UnityEngine.Video.VideoClip VideoClip;
    }

}
