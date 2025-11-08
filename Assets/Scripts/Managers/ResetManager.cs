using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Managers
{
    public class ResetManager : MonoBehaviour
    {

        private void OnEnable()
        {
            EventManager.Instance.AddListener(EventNames.StartNewScene, FindAllResetables);
            EventManager.Instance.AddListener(EventNames.StopRecording, FindAllResetables);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener(EventNames.StartNewScene, FindAllResetables);
            EventManager.Instance.RemoveListener(EventNames.StopRecording, FindAllResetables);
        }

        private void FindAllResetables(object obj)
        {
            // Find all active objects that implement IReset
            print("called reset");
            var resetables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (var mb in resetables)
            {
                if (mb is IReset resetable)
                {
                    try
                    {
                        resetable.OnReset(); // or whatever method your interface defines
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"[ResetManager] Failed to reset {mb.name}: {e.Message}");
                    }
                }
            }
            Debug.Log($"[ResetManager] Reset {resetables.Length} objects.");
        }
    }
}