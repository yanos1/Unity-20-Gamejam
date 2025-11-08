using System;
using UnityEngine;
using Obstacles.Buttons;
using Player;

namespace Obstacles.Elevator
{
    [RequireComponent(typeof(Collider2D))] // Or Collider if 3D
    public class Elevator : MonoBehaviour, IActivate
    {
        [Header("References")]
        [SerializeField] private Transform endingPoint; // Target top position

        [Header("Settings")] [SerializeField] private float moveSpeed;

        private Vector3 _startPoint;
        private Vector3 _targetPoint;
        private bool _isMoving = false;
        private bool _isActivated = false;
      

        private void Awake()
        {
            _startPoint = transform.position;
            _targetPoint = _startPoint;
        }

        private void FixedUpdate()
        {
            if (!_isMoving)
                return;

            transform.position = Vector3.MoveTowards(transform.position, _targetPoint, moveSpeed * Time.fixedDeltaTime);

            if (Vector3.Distance(transform.position, _targetPoint) < 0.01f)
            {
                transform.position = _targetPoint;
                _isMoving = false;
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

            _targetPoint = _startPoint;
            _isMoving = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var playerManager = other.gameObject.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.transform.SetParent(transform);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            var playerManager = other.gameObject.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.transform.SetParent(null);
                DontDestroyOnLoad(playerManager);
            }
        }
    }
}
