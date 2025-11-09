using System;
using System.Collections;
using Interfaces;
using UnityEngine;
using Obstacles.Buttons;
using Player;

namespace Obstacles.Elevator
{
    [RequireComponent(typeof(Collider2D))] // Or Collider if 3D
    public class Elevator : MonoBehaviour, IActivate, IReset
    {
        [Header("References")]
        [SerializeField] private Transform endingPoint; // Target top position

        [Header("Settings")]
        [SerializeField] private float moveSpeed = 2f;

        private Vector3 _startPoint;
        private Vector3 _targetPoint;
        private bool _isMoving = false;
        private bool _isActivated = false;

        private Vector3 _lastPosition;
        private Vector2 _currentVelocity;

        private PlayerManager _currentPlayer; // The player currently on the elevator

        private void Awake()
        {
            _startPoint = transform.position;
            _targetPoint = _startPoint;
            _lastPosition = _startPoint;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void FixedUpdate()
        {
            // Move the elevator
            if (_isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPoint, moveSpeed * Time.fixedDeltaTime);

                if (Vector3.Distance(transform.position, _targetPoint) < 0.01f)
                {
                    transform.position = _targetPoint;
                    _isMoving = false;
                }
            }

            // Calculate elevator velocity
            _currentVelocity = ((Vector2)(transform.position - _lastPosition)) / Time.fixedDeltaTime;
            _lastPosition = transform.position;

            // Apply elevator velocity to the player
            if (_currentPlayer != null)
            {
                Rigidbody2D playerRb = _currentPlayer.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    // Only move player if grounded or on elevator
                    playerRb.position += _currentVelocity * Time.fixedDeltaTime;
                }
            }
        }

        public void Activate()
        {
            if (_isActivated) return;
            _isActivated = true;

            _targetPoint = endingPoint.position;
            _isMoving = true;
        }

        public void Deactivate()
        {
            if (!_isActivated) return;
            _isActivated = false;

            // Stop elevator immediately
            _isMoving = false;

            // Start coroutine to return after 1 second
            StartCoroutine(ReturnAfterDelay(1f));
        }

        private IEnumerator ReturnAfterDelay(float delay)
        {
            // Wait for the delay
            yield return new WaitForSeconds(delay);

            // Start moving back
            _targetPoint = _startPoint;
            _isMoving = true;
        }



        private void OnCollisionEnter2D(Collision2D other)
        {
            var playerManager = other.gameObject.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                _currentPlayer = playerManager;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            var playerManager = other.gameObject.GetComponent<PlayerManager>();
            if (playerManager != null && _currentPlayer == playerManager)
            {
                _currentPlayer = null;
            }
        }

        public void OnReset()
        {
            transform.position = _startPoint;
        }
    }
}
