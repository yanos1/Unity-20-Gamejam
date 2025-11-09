using System;
using UnityEditor;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
        
        public int currentLevel = 0;
        int[] maxVersionsPerLevel = { 1,3,1,1,1,2,1,1,1,1,1,1 };  
        int currentMaxVersions;


        private void Start()
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
            if (obj is ValueTuple<bool, int> isNewSceneToNewSceneIndex)
            {
                if (isNewSceneToNewSceneIndex.Item1)
                {
                    currentLevel++;   
                    currentMaxVersions = currentLevel < maxVersionsPerLevel.Length ? 
                        maxVersionsPerLevel[currentLevel]: maxVersionsPerLevel[maxVersionsPerLevel.Length - 1];
                }
                else
                {
                    currentMaxVersions = maxVersionsPerLevel[currentLevel];
                }
            }
         
        }

        private void OnStartRecording(object obj)
        {
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
