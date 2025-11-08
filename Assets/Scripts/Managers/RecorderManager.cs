using System;
using Managers;
using Player;
using UnityEngine;

public class ReplayManager : MonoBehaviour
{
    public GameObject playerClonePrefab;
    private PlayerRecorder _recorder;
    private float recordMaxTime =  6f; 
    private float recordCurrentTime = 0f;

    private bool isRecording = false;

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventNames.Die, OnDie);
    }
    
    private void OnDisable()
    {
        EventManager.Instance.AddListener(EventNames.Die, OnDie);
    }

    private void OnDie(object obj)
    {
        isRecording = false;
    }

    private void Update()
    {
        if (isRecording)
        {
            print("recording... delta time is"+ Time.deltaTime);
            recordCurrentTime += Time.deltaTime;
            print("current time: "+ recordCurrentTime);
        }


        // Start recording
        if (!isRecording && Input.GetKeyDown(KeyCode.R) && LevelManager.Instance.canReleaseNewVersion())
        {
            EventManager.Instance.InvokeEvent(EventNames.StartRecording, null);
            isRecording = true;
            SetRecorder();
            _recorder.StartRecording();
        }

        // Stop recording & spawn clone
        else if (isRecording && (Input.GetKeyDown(KeyCode.R) || recordCurrentTime >= recordMaxTime))
        {
            EventManager.Instance.InvokeEvent(EventNames.StopRecording, null);
            isRecording = false;
            SetRecorder();
            recordCurrentTime = 0;

            var recordedData = _recorder.StopRecording();
            GameObject clone = Instantiate(playerClonePrefab, _recorder.transform.position, Quaternion.identity);
            var player = clone.GetComponent<PlayerManager>();
            player.Init(isClone: true);
            clone.GetComponent<PlayerReplay>().LoadFrames(recordedData);
            
            CloneManager.Instance.RegisterPlayer(player);
        }
    }

    private void SetRecorder()
    {
        if (_recorder is null)
        {
            _recorder = CoreManager.Instance.player.GetComponent<PlayerRecorder>();
        }
    }
}