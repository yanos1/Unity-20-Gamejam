using System;
using Managers;
using Player;
using UnityEngine;

namespace Checkpoint
{
    public class Checkpoint : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerManager>())
            {
                CheckPointManager.Instance.RecordPosition(transform.position);
            }
        }
    }
}