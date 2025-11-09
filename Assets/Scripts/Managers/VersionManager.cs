using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class VersionManager : MonoBehaviour
    {
        [SerializeField] private List<UnityVersionData> unityVersions;
        private int curIndex;
        
        
        public void UpdateVersion()
        {
            CoreManager.Instance.player.UpdatePlayerVersion(unityVersions[curIndex++]);
        }
    }
}