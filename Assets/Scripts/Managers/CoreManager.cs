using System;
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
        }
    }
}