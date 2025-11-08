using Player;
using UnityEngine;

namespace Obstacles.Laser
{
    public class Laser : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerManager>() is { } player)
            {
                if (!player.IsClone)
                {
                    player.Die();
                }
            }
        }
    }
}