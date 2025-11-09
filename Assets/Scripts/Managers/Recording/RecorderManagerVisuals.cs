using System;
using System.Collections;
using UnityEngine;

namespace Managers.Recording
{
    public class RecorderManagerVisuals : MonoBehaviour
    {
        [SerializeField] private GameObject recordingDot;
        [SerializeField] private GameObject recordingScreen;

        private Coroutine _blinkingCoroutine;
        private bool _isRecording;
        
        public void TurnOnRecording()
        {
            if (_isRecording) return; // Already active

            _isRecording = true;

            if (recordingScreen != null)
                recordingScreen.SetActive(true);

            if (recordingDot != null)
            {
                recordingDot.SetActive(true);
                _blinkingCoroutine = StartCoroutine(BlinkDot());
            }
        }

        public void TurnOffRecording()
        {
            if (!_isRecording) return; // Not recording, nothing to stop

            _isRecording = false;

            if (_blinkingCoroutine != null)
            {
                StopCoroutine(_blinkingCoroutine);
                _blinkingCoroutine = null;
            }

            if (recordingDot != null)
                recordingDot.SetActive(false);

            if (recordingScreen != null)
                recordingScreen.SetActive(false);
        }

        private IEnumerator BlinkDot()
        {
            while (_isRecording)
            {
                recordingDot.SetActive(!recordingDot.activeSelf);
                yield return new WaitForSeconds(0.7f);
            }
        }
    }
}