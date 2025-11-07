using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        private void Start()
        {
            CoreManager.Instance.player = this;
        }

        private void OnEnable()
        {
            EventManager.Instance.AddListener(EventNames.StartRecording, OnStartRecording);
            EventManager.Instance.AddListener(EventNames.StopRecording, OnStopRecording);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener(EventNames.StartRecording, OnStartRecording);
            EventManager.Instance.RemoveListener(EventNames.StopRecording, OnStopRecording);

        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener(EventNames.StartRecording, OnStartRecording);
            EventManager.Instance.RemoveListener(EventNames.StopRecording, OnStartRecording);
        }
        private void OnStartRecording(object obj)
        {
            transform.position = CheckPointManager.Instance.GetCheckPointPosition();
        }
        
        private void OnStopRecording(object obj)
        {
            StartCoroutine(WaitBeforeResetPos());
        }

        private IEnumerator WaitBeforeResetPos()
        {
            // Optional: disable player input and physics
            DisablePlayer();

            // Optional: hide sprite
            SetVisible(false);

            // Wait
            yield return new WaitForSeconds(1f);

            // Reset position
            transform.position = CheckPointManager.Instance.GetCheckPointPosition();

            // Re-enable player
            EnablePlayer();
            SetVisible(true);
        }

        private void DisablePlayer()
        {
            // Example: stop movement
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = false;

            // Stop input scripts
            var movement = GetComponent<PlayerMovement2D>();
            if (movement != null) movement.enabled = false;
        }

        private void EnablePlayer()
        {
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = true;

            var movement = GetComponent<PlayerMovement2D>();
            if (movement != null) movement.enabled = true;
        }

        private void SetVisible(bool visible)
        {
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = visible;
        }



        private bool _isClone;
        public bool IsClone => _isClone;
    
        public void Init(bool isClone)
        {
            _isClone = isClone;
            if (isClone)
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                Destroy(gameObject.GetComponent<PlayerMovement2D>());
                Destroy(gameObject.GetComponent<PlayerRecorder>());
                Destroy(gameObject.GetComponent<PlayerManager>());
                GetComponent<SpriteRenderer>().color *= Color.darkGray;
                gameObject.layer = LayerMask.NameToLayer("Ground");
            }
        }
    }
}