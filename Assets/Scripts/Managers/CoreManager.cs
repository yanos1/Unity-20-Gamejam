using System;
using System.Collections;
using Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class CoreManager : MonoBehaviour
    {
        public static CoreManager Instance;
        [DoNotSerialize] public PlayerManager player;

        private void Awake()
        {
            Instance = this;
            StartCoroutine(InitPlayerAfterDelay());
        }

        private IEnumerator InitPlayerAfterDelay()
        {
            yield return new WaitUntil(() => player is not null);
            player.Init(isClone: false);
        }
    }
}