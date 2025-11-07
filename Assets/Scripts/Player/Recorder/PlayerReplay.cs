using System.Collections.Generic;
using UnityEngine;

public class PlayerReplay : MonoBehaviour
{
    private List<PlayerRecorder.PlayerFrameData> frames;
    private int frameIndex = 0;
    private float playbackInterval = 0.02f;
    private float timer = 0f;
    private bool isPlaying = false;

    public void LoadFrames(List<PlayerRecorder.PlayerFrameData> data)
    {
        frames = data;
        frameIndex = 0;
        isPlaying = true;
    }

    private void Update()
    {
        if (!isPlaying || frames == null || frameIndex >= frames.Count)
        {
            // Freeze after playback
            isPlaying = false;
            return;
        }

        timer += Time.deltaTime;
        if (timer >= playbackInterval)
        {
            timer = 0f;

            transform.position = frames[frameIndex].position;
            // Optionally trigger animations, jumps, etc. here
            frameIndex++;
        }
    }
}