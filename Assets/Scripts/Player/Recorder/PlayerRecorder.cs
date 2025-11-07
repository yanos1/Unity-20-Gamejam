using System.Collections.Generic;
using UnityEngine;

public class PlayerRecorder : MonoBehaviour
{
    private bool isRecording;
    [SerializeField] private float recordInterval = 0.02f; // every 20ms (50 fps equivalent)

    private float recordTimer;
    private List<PlayerFrameData> recordedFrames = new List<PlayerFrameData>();

    private void Update()
    {
        if (!isRecording) return;

        recordTimer += Time.deltaTime;
        if (recordTimer >= recordInterval)
        {
            recordTimer = 0f;

            // You can add more inputs/actions here as needed
            bool jump = Input.GetButtonDown("Jump");
            bool dash = Input.GetKeyDown(KeyCode.LeftShift);

            recordedFrames.Add(new PlayerFrameData(transform.position, jump, dash));
        }
    }

    public void StartRecording()
    {
        recordedFrames.Clear();
        isRecording = true;
    }

    public List<PlayerFrameData> StopRecording()
    {
        isRecording = false;
        return new List<PlayerFrameData>(recordedFrames);
    }
    
    [System.Serializable]
    public class PlayerFrameData
    {
        public Vector2 position;
        public bool jumped;
        public bool dashed;

        public PlayerFrameData(Vector2 pos, bool jump, bool dash)
        {
            position = pos;
            jumped = jump;
            dashed = dash;
        }
    }

}