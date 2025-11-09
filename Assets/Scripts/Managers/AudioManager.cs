using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("SFX Clips")]
        [SerializeField] private AudioClip jumpClip;
        [SerializeField] private AudioClip patchNotesClip;
        [SerializeField] private AudioClip recordingClip;

        [Header("Mixer (optional)")]
        [SerializeField] private AudioMixer mixer;

        public enum SFXType { Jump, PatchNotes, Recording }

        // ----------------- Lifecycle -----------------
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            EventManager.Instance.AddListener(EventNames.StartRecording, OnStartRecording);
            EventManager.Instance.AddListener(EventNames.Die, OnStopRecording);
            EventManager.Instance.AddListener(EventNames.StartNewScene, (object o) => PlayMusic());
            EventManager.Instance.AddListener(EventNames.StopRecording, OnStopRecording);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener(EventNames.StartRecording, OnStartRecording);
            EventManager.Instance.RemoveListener(EventNames.StopRecording, OnStopRecording);
            EventManager.Instance.RemoveListener(EventNames.Die, OnStopRecording);

        }

        // ----------------- Public API -----------------
        public void PlayMusic()
        {
            print("play music 123");
            if (!musicSource.isPlaying)
            {
                print("laying music 123");
                musicSource.Play();
            }
            OnStopRecording(null);
        }
        

        public void PlaySFX(SFXType sound, float volume = 1f)
        {
            AudioClip clip = sound switch
            {
                SFXType.Jump => jumpClip,
                SFXType.PatchNotes => patchNotesClip,
                SFXType.Recording => recordingClip,
                _ => null
            };

            if (clip != null)
                sfxSource.PlayOneShot(clip, volume);
        }

        public void SetMusicVolume(float dB) =>
            mixer?.SetFloat("MusicVolume", dB);

        public void SetSFXVolume(float dB) =>
            mixer?.SetFloat("SFXVolume", dB);

        // ----------------- Private Helpers -----------------
        private IEnumerator SwapMusic(AudioClip newClip, bool loop, float fadeSeconds)
        {
            yield return FadeOut(musicSource, fadeSeconds);
            musicSource.clip = newClip;
            musicSource.loop = loop;
            musicSource.Play();
            yield return FadeIn(musicSource, fadeSeconds);
        }

        private static IEnumerator FadeOut(AudioSource src, float seconds)
        {
            if (!src) yield break;
            float start = src.volume;
            float t = 0f;
            while (t < seconds)
            {
                t += Time.unscaledDeltaTime;
                src.volume = Mathf.Lerp(start, 0f, t / seconds);
                yield return null;
            }
            src.volume = 0f;
            src.Stop();
        }

        private static IEnumerator FadeIn(AudioSource src, float seconds)
        {
            if (!src) yield break;
            float t = 0f;
            float target = 1f;
            src.volume = 0f;
            while (t < seconds)
            {
                t += Time.unscaledDeltaTime;
                src.volume = Mathf.Lerp(0f, target, t / seconds);
                yield return null;
            }
            src.volume = target;
        }

        // ----------------- Event Callbacks -----------------
        private void OnStartRecording(object _) => PlaySFX(SFXType.Recording);

        private void OnStopRecording(object _)
        {
            if (sfxSource.isPlaying)
                sfxSource.Stop();
        }
    }
}
