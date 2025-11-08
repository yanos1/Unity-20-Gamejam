using Obstacles.Buttons;
using Player;
using UnityEngine;

namespace Obstacles.Laser
{
    public class Laser : MonoBehaviour, IActivate
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

        public void Activate()
        {
            gameObject.SetActive(false);
        }

        public void Deactivate()
        {
            gameObject.SetActive(true);
        }
    }
}