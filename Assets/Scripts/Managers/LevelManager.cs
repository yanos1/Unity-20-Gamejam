using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        int currentLevel = 0;
        int[] maxVersionsPerLevel = { 3, 4, 5, 6, 7 };  
        int currentMaxVersions;


        private void Awake()
        {
            Instance = this;
            currentMaxVersions = maxVersionsPerLevel[0];
        }

        private void OnEnable()
        {
            EventManager.Instance.AddListener(EventNames.StartNewScene, OnStartNewScene);
            EventManager.Instance.AddListener(EventNames.StartRecording, OnStartRecording);
        }

        private void OnStartNewScene(object obj)
        {
            print("Starting new scene, updating level and max versions");   
            currentLevel++;   
            currentMaxVersions = currentLevel < maxVersionsPerLevel.Length ? 
                maxVersionsPerLevel[currentLevel]: maxVersionsPerLevel[maxVersionsPerLevel.Length - 1];
        }

        private void OnStartRecording(object obj)
        {
            print("Started recording, reducing allowed versions");
            currentMaxVersions--;
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener(EventNames.StartNewScene, OnStartNewScene);
            EventManager.Instance.RemoveListener(EventNames.StartRecording, OnStartRecording);
        }
        public int GetAllowedVersionsForCurrentLevel()
        {
            return currentMaxVersions;
        }

        public bool canReleaseNewVersion()
        {
            return currentMaxVersions > 0;
        }

    }
}
