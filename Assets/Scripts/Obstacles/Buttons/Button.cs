using System;
using Player;
using UnityEngine;

namespace Obstacles.Buttons
{
    [RequireComponent(typeof(Collider2D))]
    public class Button : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MonoBehaviour activatorObject; // Must implement IActivate

        [Header("Settings")]
        [SerializeField] private float pressDepth = 0.2f; // How far the button moves down
        [SerializeField] private float moveSpeed = 6f;    // Lerp speed

        private IActivate _activator;
        private Vector3 _startPosition;
        private Vector3 _pressedPosition;
        private Vector3 _targetPosition;
        private bool _isPressed;

        private void Awake()
        {
            // Cast the serialized reference to IActivate safely
            if (activatorObject is IActivate activator)
                _activator = activator;
            else
                Debug.LogError($"{name} → Activator object does not implement IActivate!");

            _startPosition = transform.localPosition;
            _pressedPosition = _startPosition + Vector3.down * pressDepth;
            _targetPosition = _startPosition;

            // Ensure trigger collider
            var col = GetComponent<Collider2D>();
            if (col != null) col.isTrigger = true;
        }

        private void Update()
        {
            // Smoothly lerp to the target position
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                _targetPosition,
                Time.deltaTime * moveSpeed
            );
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerManager>() == null)
                return;

            _isPressed = true;
            _targetPosition = _pressedPosition;
            _activator?.Activate();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<PlayerManager>() == null)
                return;

            _isPressed = false;
            _targetPosition = _startPosition;
            _activator?.Deactivate();
        }
    }
}
