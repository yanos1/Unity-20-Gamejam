using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class BugManager : MonoBehaviour
    {

        public static BugManager Instance;
        public List<Bugs> currentBugs = new();

        public Dictionary<int, List<Bugs>> levelToBugsMap = new();


        private void Start()
        {
            levelToBugsMap[2] = new List<Bugs>() { Bugs.CrashingClones };
            levelToBugsMap[4] = new List<Bugs>() { Bugs.ReverseInput };
            levelToBugsMap[8] = new List<Bugs>() { Bugs.JumpMightFail, Bugs.CrashingClones };
        }

        private void OnEnable()
        {
            EventManager.Instance.AddListener(EventNames.StartNewScene, OnStartNewScene);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener(EventNames.StartNewScene, OnStartNewScene);
        }

        private void OnStartNewScene(object obj)
        {
            if (obj is ValueTuple<bool, int> isNewSceneToNewSceneIndex)
            {
                int newSceneIndex = isNewSceneToNewSceneIndex.Item2;

                if (!levelToBugsMap.TryGetValue(newSceneIndex, out currentBugs))
                    currentBugs = new List<BugManager.Bugs>();
            }
        }


        private void Awake()
        {
            Instance ??= this;
        }

        public void AddBug(Bugs bug)
        {
            currentBugs.Add(bug);
        }

        public void RemoveBug(Bugs bug)
        {
            currentBugs.Remove(bug);
        }

        public void ClearBug()
        {
            currentBugs.Clear();
        }
        public enum Bugs
        {
            None = 0,
            ReverseInput =1,
            JumpMightFail =2,
            CrashingClones = 3,
        }
    }
}
