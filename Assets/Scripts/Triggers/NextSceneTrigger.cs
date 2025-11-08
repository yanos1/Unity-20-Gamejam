using System;
using Managers;
using Player;
using UnityEngine;

namespace Checkpoint
{
    public class NextSceneTrigger : MonoBehaviour
    {
        private bool triggered = false;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if ( !triggered && other.GetComponent<PlayerManager>() is { } player)
            {
                print("go to next scene!");
                triggered = true;
                ScenesManager.Instance.LoadNextScene();
            }
        }
    }
}