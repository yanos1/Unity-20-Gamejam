using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Managers
{
    public class CloneManager : MonoBehaviour
    {
        public static CloneManager Instance;
        private List<PlayerManager> players = new();


        private void OnEnable()
        {
            EventManager.Instance.AddListener(EventNames.Die, ClearClones);
            EventManager.Instance.AddListener(EventNames.StartNewScene, ClearClones);
        }
        
        private void OnDisable()
        {
            EventManager.Instance.RemoveListener(EventNames.Die, ClearClones);
            EventManager.Instance.RemoveListener(EventNames.StartNewScene, ClearClones);
        }

        private void Awake()
        {
            Instance = this;
        }


        public void RegisterPlayer(PlayerManager player)
        {
            if (player.IsClone) players.Add(player);
        }

        public void RemovePlayer(PlayerManager player)
        {
            players.Remove(player);
        }

        public void ClearClones(object obj)
        {
            // foreach (var player in players)
            // {
            //     if (player is not null)
            //     {
            //         Destroy(player.gameObject);
            //     }
            // }
        }
    }
}