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
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            if (EventManager.Instance == null) return;
            EventManager.Instance.AddListener(EventNames.StartRecording, OnStartRecording);
            EventManager.Instance.AddListener(EventNames.StopRecording, OnStopRecording);
        }

        private void OnDisable()
        {
            if (EventManager.Instance == null) return;
            EventManager.Instance.RemoveListener(EventNames.StartRecording, OnStartRecording);
            EventManager.Instance.RemoveListener(EventNames.StopRecording, OnStopRecording);
        }

        // ----------------- Public API -----------------
        public void PlayMusic(AudioClip clip, bool loop = true, float fadeSeconds = 0.5f)
        {
            if (clip == null) return;
            if (musicSource.clip == clip && musicSource.isPlaying) return;
            StartCoroutine(SwapMusic(clip, loop, fadeSeconds));
        }

        public void StopMusic(float fadeSeconds = 0.5f) =>
            StartCoroutine(FadeOut(musicSource, fadeSeconds));

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
