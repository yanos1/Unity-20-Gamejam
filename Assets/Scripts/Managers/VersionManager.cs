using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class VersionManager : MonoBehaviour
    {
        public static VersionManager Instance;
        [SerializeField] private List<UnityVersionData> unityVersions;
        private int curIndex;
        public int currentVersion = 0;

        [SerializeField] private AudioSource src;
        private bool isPlaying = false;
        

        private void Awake()
        {
            Instance = this;
        }
        public void UpdateVersion()
        {
            CoreManager.Instance.player.UpdatePlayerVersion(unityVersions[curIndex++]);
        }

        private void Update()
        {
            if (!isPlaying)
            {
                if (LevelManager.Instance.currentLevel >= 2)
                {
                    src.Play();
                }
            }
        }
    }
}