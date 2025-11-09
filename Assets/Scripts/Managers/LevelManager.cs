using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
        public TextMeshProUGUI t;
        public GameObject panel;
        
        public int currentLevel = 0;
        int[] maxVersionsPerLevel = { 1000,3,1,1,1,2,2,1,1,2,1,1 };  
        int currentMaxVersions;


        private void Start()
        {
            Instance = this;
            currentMaxVersions = maxVersionsPerLevel[0];
            StartCoroutine(ShowTutorial());
        }

        private IEnumerator ShowTutorial()
        {
            print("cotoutine started 123");

            InputManager.Instance.DisableInput();
            yield return new WaitForSeconds(5f);
            print("5 seconds passed 123");
            t.gameObject.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            panel.SetActive(false);
            InputManager.Instance.EnableInput();
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
