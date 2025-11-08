using System.Collections;
using Interfaces;
using Player;
using UnityEngine;

namespace Obstacles.RotatingPlatform
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class RotatingPlatform : MonoBehaviour, IReset
    {
        [Header("Swing Settings")]
        [SerializeField] private float angularVelocityThreshold = 10f; // how fast before it counts as rotating
        [SerializeField] private float swingForce = 10f;               // how strong the push is

        private Quaternion startingRot;
        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            startingRot = transform.rotation;
        }

        public void OnReset()
        {
            print("reset rotating platform");
            StartCoroutine(ResetRotation());
        }

        private IEnumerator ResetRotation()
        {
            yield return new WaitForSeconds(0.1f);
            rb.angularVelocity = 0f;
            transform.rotation = startingRot;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            // Only apply force if platform is rotating counterclockwise
            var playerManager = other.gameObject.GetComponent<PlayerManager>();
            if (playerManager?.IsClone != false) return;

            // Skip if not rotating fast enough or rotating clockwise
            if (rb.angularVelocity <= 0f || Mathf.Abs(rb.angularVelocity) < angularVelocityThreshold)
            {
                print($"no force added bbecause angular is : {rb.angularVelocity} ");
                return;
            }

        var playerRb = playerManager.GetComponent<Rigidbody2D>();
            if (playerRb == null)
                return;

            // Direction from platform to player
            Vector2 toPlayer = playerRb.transform.position - transform.position;
            print("add force to player");
            // Apply upward (outward) impulse relative to platform position
            playerRb.AddForce(toPlayer.normalized * swingForce, ForceMode2D.Impulse);
        }

    }
}