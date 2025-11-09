using System;
using System.Collections;
using Managers;
using ScriptableObjects;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovement2D playerMovement2D;
        private Rigidbody2D _rb;

        private bool _isClone;
        public bool IsClone => _isClone;

        private void Awake()
        {
            CoreManager.Instance.player = this;
            _rb = GetComponent<Rigidbody2D>();
        }
        
        private void OnEnable()
        {
            EventManager.Instance.AddListener(EventNames.StartRecording, OnStartRecording);
            EventManager.Instance.AddListener(EventNames.StopRecording, OnStopRecording);
            EventManager.Instance.AddListener(EventNames.StartNewScene, OnStartNewScene);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener(EventNames.StartRecording, OnStartRecording);
            EventManager.Instance.RemoveListener(EventNames.StopRecording, OnStopRecording);
            EventManager.Instance.RemoveListener(EventNames.StartNewScene, OnStartNewScene);
        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener(EventNames.StartRecording, OnStartRecording);
            EventManager.Instance.RemoveListener(EventNames.StopRecording, OnStartRecording);
            EventManager.Instance.RemoveListener(EventNames.StartNewScene, OnStartNewScene);
        }

        void Update()
        {
            // Check if "CrashingClones" bug is active
            if (_isClone && !GetComponent<PlayerReplay>().isPlaying && BugManager.Instance.currentBugs.Contains(BugManager.Bugs.CrashingClones))
            {
                // Move this clone downward slowly
                transform.position += Vector3.down * (Time.deltaTime * 0.4f); // 1f = speed, adjust as needed
            }
        }

        private void OnStartNewScene(object obj)
        {
            transform.position = FindAnyObjectByType<Checkpoint.Checkpoint>().gameObject.transform.position;
        }

        private void OnStartRecording(object obj)
        {
            if(_isClone) return;

            transform.position = CheckPointManager.Instance.GetCheckPointPosition();
        }

        private void OnStopRecording(object obj)
        {
            if(_isClone) return;

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


        public void Init(bool isClone)
        {
            _isClone = isClone;
            if (!isClone)
            {
                DontDestroyOnLoad(gameObject);
            }

            if (isClone)
            {
                _rb.bodyType = RigidbodyType2D.Kinematic;
                Destroy(gameObject.GetComponent<PlayerMovement2D>());
                Destroy(gameObject.GetComponent<PlayerRecorder>());
                GetComponent<SpriteRenderer>().color *= Color.darkGray;
                gameObject.layer = LayerMask.NameToLayer("Ground");
            }
        }

        public void UpdatePlayerVersion(UnityVersionData unityVersion)
        {
            playerMovement2D.UpdatePlayerVersion(unityVersion);
        }

        public void Die()
        {
            if(ScenesManager.Instance.IsSwitchingScene) return;
            EventManager.Instance.InvokeEvent(EventNames.Die, null);
            ScenesManager.Instance.ReloadCurrentScene();
            GetComponent<PlayerRecorder>().DeleteRecording();
        }
    }
}