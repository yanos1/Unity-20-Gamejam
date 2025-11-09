using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {

        public static UIManager Instance;
        
        [SerializeField] private TextMeshProUGUI startRecording;
        [SerializeField] private TextMeshProUGUI releaseNewUnityVersion;
        [SerializeField] private TextMeshProUGUI AllowedVersionsPerLevel;
        [SerializeField] private TextMeshProUGUI recordCountDown;
        [SerializeField] private Button restart;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            EventManager.Instance.AddListener(EventNames.StartRecording, OnStartRecording);
            EventManager.Instance.AddListener(EventNames.StopRecording, OnStopRecording);
            EventManager.Instance.AddListener(EventNames.StartNewScene, OnStartNewScene);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener(EventNames.StartRecording, OnStartRecording);
            EventManager.Instance.RemoveListener(EventNames.StopRecording, OnStopRecording);
            EventManager.Instance.RemoveListener(EventNames.StartNewScene, OnStartNewScene);
        }
        
        private int recordingDisplayCount = 0;
        private const int MaxDisplayCount = 2;

        private void OnStartRecording(object obj)
        {
            // Increment display count each time recording starts
            recordingDisplayCount++;
            UpdateAllowedVersionsText();

            if (recordingDisplayCount <= MaxDisplayCount)
            {
                if (startRecording != null)
                    startRecording.gameObject.SetActive(false);

                if (releaseNewUnityVersion != null)
                    releaseNewUnityVersion.gameObject.SetActive(true);

                if (recordCountDown != null)
                    recordCountDown.gameObject.SetActive(true);

            }
            else
            {
                // After limit reached, hide all permanently
                HideAllRecordingUI();
            }
        }

        private void OnStopRecording(object obj)
        {
            if (recordingDisplayCount <= MaxDisplayCount)
            {
                if (releaseNewUnityVersion != null)
                    releaseNewUnityVersion.gameObject.SetActive(false);

                if (recordCountDown != null)
                    recordCountDown.gameObject.SetActive(false);

                if (startRecording != null)
                    startRecording.gameObject.SetActive(true);
            }
            else
            {
                HideAllRecordingUI();
            }
        }

        private void HideAllRecordingUI()
        {
            if (startRecording != null)
                startRecording.gameObject.SetActive(false);
            if (releaseNewUnityVersion != null)
                releaseNewUnityVersion.gameObject.SetActive(false);
            if (recordCountDown != null)
                recordCountDown.gameObject.SetActive(false);
        }

        public void Restart()
        {
            ScenesManager.Instance.ReloadCurrentScene();
        }

        private void OnStartNewScene(object obj)
        {
            if (obj is ValueTuple<bool, int> scenedata)
            {
                if (scenedata.Item2 >= 1)
                {
                    restart.gameObject.SetActive(true);
                }
                else
                {
                     restart.gameObject.SetActive(false);
                }
            }
            UpdateAllowedVersionsText();
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

        private void UpdateAllowedVersionsText()
        {
            int allowedVersions = LevelManager.Instance.GetAllowedVersionsForCurrentLevel();

            string displayText = allowedVersions > 50 ? "Unlimited" : allowedVersions.ToString();

            AllowedVersionsPerLevel.text = "Releases Left: " + displayText;
        }

        private void Start()
        {
            UpdateAllowedVersionsText();
        }
        
        public void UpdateRecordCountDown(float timeLeft)
        {
            recordCountDown.text = timeLeft.ToString("F1") + "s";
        }
    }
}