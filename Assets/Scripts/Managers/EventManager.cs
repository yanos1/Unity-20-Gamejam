using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance;
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private Dictionary<EventNames, List<Action<object>>> _activeListeners = new();

        public void AddListener(EventNames eventName, Action<object> listener)
        {
            if (_activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                listOfEvents.Add(listener);

                return;
            }

            _activeListeners.Add(eventName, new List<Action<object>> { listener });
        }

        public void RemoveListener(EventNames eventName, Action<object> listener)
        {
            if (_activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                listOfEvents.Remove(listener);

                if (listOfEvents.Count <= 0)
                {
                    _activeListeners.Remove(eventName);
                }
            }
        }

        public void InvokeEvent(EventNames eventName, object obj)
        {
            if (_activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                for (int i = 0; i < listOfEvents.Count; i++)
                {
                    listOfEvents[i].Invoke(obj);
                }
            }
        }
    }

    public enum EventNames
    {
        None = 0,
        StartRecording = 1,
        StopRecording = 2,
        StartNewScene = 3,
    }
}