using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class ScenesManager : MonoBehaviour
    {
        [Tooltip("The name of the persistent scene.")]
        public static ScenesManager Instance { get; private set; }
        
        private int numScenes;

        private int currentSceneIndex = 0; // Tracks the index of the currently active scene

        private bool isSwitchingScene = false;

        public bool IsSwitchingScene => isSwitchingScene;

        public int CurrentScene => currentSceneIndex;
        private void RestartScene(object obj)
        {
            StartCoroutine(SwitchScene(currentSceneIndex, doFade: true));
        }

        private void Start()
        {
            Instance ??= this;
            DontDestroyOnLoad(gameObject);
            numScenes = SceneManager.sceneCountInBuildSettings;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                LoadNextScene();
            }
            if (Input.GetKeyDown(KeyCode.Q) && LevelManager.Instance.currentLevel > 0)
            {
                ReloadCurrentScene();
            }
        }
        
        
        private IEnumerator SwitchToMainMenu()
        {
            const int mainMenuIndex = 2;
            
            UIManager.Instance.ShowLoadingScreen();
            while (!UIManager.Instance.InstanceIsFadeInFinished())
            {
                yield return null;
            }
            
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(mainMenuIndex, LoadSceneMode.Additive);
            while (!loadOp.isDone)
            {
                yield return null;
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(mainMenuIndex));

            if (currentSceneIndex >= 0 && currentSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentSceneIndex);
                while (!unloadOp.isDone)
                {
                    yield return null;
                }
            }

            
            UIManager.Instance.HideLoadingScreen();
            
            currentSceneIndex = mainMenuIndex;
            EventManager.Instance.InvokeEvent(EventNames.StartNewScene, mainMenuIndex);
        }

        public int LoadNextScene(float delayBetweenFades = 0)
        {
            if (isSwitchingScene) return -1;
            if (currentSceneIndex + 1 < numScenes)
            {
                StartCoroutine(SwitchScene(currentSceneIndex + 1, doFade: true, delayBetweenFades));
            }
            else
            {
                Debug.LogWarning("No more scenes to load. You are at the last scene.");
            }
        
            return currentSceneIndex + 1;
        }
        
        public int ReloadCurrentScene()
        {
            StartCoroutine(SwitchScene(currentSceneIndex, doFade: true));
            return currentSceneIndex;
        }

        private IEnumerator SwitchScene(int newSceneIndex, bool doFade, float delayBetweenFades = 0)
        {
            if (newSceneIndex >= 0 && newSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                isSwitchingScene = true;
                print($"old scene is {currentSceneIndex} {SceneManager.GetActiveScene().name}");
                if (doFade)
                {
                    UIManager.Instance.ShowLoadingScreen();
                    while (!UIManager.Instance.IsFadeInFinished())
                    {
                        yield return null;
                    }
                }
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(newSceneIndex, LoadSceneMode.Additive);
                while (!loadOperation.isDone)
                {
                    yield return null;
                }
                print("loading new scene");

                // CoreManager.Instance.AudioManager.ResumeAllAudio();
                // Set the new scene as active
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(newSceneIndex));
            }
            else
            {
                Debug.LogWarning("Invalid scene index: " + newSceneIndex);
                yield break;
            }

            if (currentSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                print("Unloading last scene");
                AsyncOperation unloadOperation;
                unloadOperation = SceneManager.UnloadSceneAsync(currentSceneIndex);
                
                while (!unloadOperation.isDone)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(delayBetweenFades);
                if (doFade)
                {
                    UIManager.Instance.HideLoadingScreen();

                }

                isSwitchingScene = false;
                EventManager.Instance.InvokeEvent(EventNames.StartNewScene,
                    (currentSceneIndex != newSceneIndex, newSceneIndex));
            }

            currentSceneIndex = newSceneIndex;
            print($"current scene is {currentSceneIndex} {SceneManager.GetActiveScene()}");
        }
    }

    public enum SceneType
    {
        None = 0,
        Cutscene = 1,
        Gameplay = 2,
        Menu = 3,
    }
}