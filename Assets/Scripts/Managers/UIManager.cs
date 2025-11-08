using System;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {

        public static UIManager Instance;
        
        [SerializeField] private TextMeshProUGUI startRecording;
        [SerializeField] private TextMeshProUGUI releaseNewUnityVersion;


        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            EventManager.Instance.AddListener(EventNames.StartRecording, OnStartRecording);
            EventManager.Instance.AddListener(EventNames.StopRecording, OnStopRecording);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener(EventNames.StartRecording, OnStartRecording);
            EventManager.Instance.RemoveListener(EventNames.StopRecording, OnStopRecording);

        }
        
        private void OnStartRecording(object obj)
        {
            startRecording.gameObject.SetActive(false);
            releaseNewUnityVersion.gameObject.SetActive(true);
        }
        
        private void OnStopRecording(object obj)
        {
            startRecording.gameObject.SetActive(true);
            releaseNewUnityVersion.gameObject.SetActive(false);
        }

        public void HideLoadingScreen()
        {
            return;
        }

        public void ShowLoadingScreen()
        {
            return;
        }

        public bool IsFadeInFinished()
        {
            return true;
        }

        public bool InstanceIsFadeInFinished()
        {
            return true;
        }
    }
}