using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro; // for nice text rendering


namespace Managers
{
    [Serializable]
    public class PatchNotes
    {
        public List<string> updates = new();
        public List<string> bugs = new();

        string ColorToHex(Color c)
        {
            int r = Mathf.RoundToInt(c.r * 255f);
            int g = Mathf.RoundToInt(c.g * 255f);
            int b = Mathf.RoundToInt(c.b * 255f);
            return $"#{r:X2}{g:X2}{b:X2}";
        }

        public PatchNotes(List<string> updates, List<string> bugs)
        {
            this.updates = updates;
            this.bugs = bugs;
        }
    }

    public class PatchNotesDisplayer : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject patchPanel;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text contentText;

        [Header("Display Settings")]
        [SerializeField] private float typingSpeed = 0.03f;

        private bool isDisplaying;
        private Coroutine currentRoutine;

        // ---------------- LEVEL → PATCH NOTES MAP ----------------
        private readonly Dictionary<int, PatchNotes> levelToPatchNotes = new()
        {
            { 1, new PatchNotes(
                new List<string> { "No updates" },
                new List<string> { "No bugs" }
            ) },

            { 2, new PatchNotes(
                new List<string> { " Added TrailRenderer ! Hurray !." },
                new List<string> { " System bug, old versions to crash unexpectedly." }
            ) },

            { 3, new PatchNotes(
                new List<string> { "Improved performance speed (faster)" },
                new List<string> { "No bugs" }
            ) },

            { 4, new PatchNotes(
                new List<string> { " No updates" },
                new List<string> { " Input System bug - Reversed input ." }
            ) },

            { 5, new PatchNotes(
                new List<string> { " Added new sound system (music)." },
                new List<string> { " None known." }
            ) },

            { 6, new PatchNotes(
                new List<string> { " No updates" },
                new List<string> { " No bugs." }
            ) },

            { 7, new PatchNotes(
                new List<string> { " Added subtle lighting improvements. (yeah right)" },
                new List<string> { " No bugs" }
            ) },

            { 8, new PatchNotes(
                new List<string> { " Added dash ability." },
                new List<string>
                {
                    "- Jump might fail randomly (30% chance).",
                    "- System bug, old versions to crash unexpectedly."
                }
            ) },

            { 9, new PatchNotes(
                new List<string> { "No updates" },
                new List<string> { " No bugs" }
            ) },

            { 10, new PatchNotes(
                new List<string> { " No updates" },
                new List<string> { "No bugs" }
            ) },

            { 11, new PatchNotes(
                new List<string> { " No updates" },
                new List<string> { " No bugs!" }
            ) },
        };
        // ----------------------------------------------------------

        private void OnEnable()
        {
            EventManager.Instance.AddListener(EventNames.StartNewScene, OpenPatches);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener(EventNames.StartNewScene, OpenPatches);
        }

        private void OpenPatches(object obj)
        {
            
            if (isDisplaying) return;
            if (obj is not ValueTuple<bool, int> sceneInfo) return;
            if(!sceneInfo.Item1) return;
            int level = sceneInfo.Item2;
            InputManager.Instance.DisableInput();

            // Try to get patch notes for this level; if none, use empty
            if (!levelToPatchNotes.TryGetValue(level, out var notes))
                notes = new PatchNotes(new List<string>(), new List<string>());

            string formatted = FormatPatchNotes(notes.updates, notes.bugs);

            patchPanel.SetActive(true);
            isDisplaying = true;
            contentText.text = "";

            if (currentRoutine != null)
                StopCoroutine(currentRoutine);

            currentRoutine = StartCoroutine(TypeText(formatted));
        }

        private IEnumerator TypeText(string text)
        {
            titleText.text = $"whats new in version {VersionManager.Instance.currentVersion} ";
            contentText.text = "";

            foreach (char c in text)
            {
                contentText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            // Wait for any key press or tap to close
            yield return new WaitUntil(() => Input.anyKeyDown);
            
            patchPanel.SetActive(false);
            isDisplaying = false;
            InputManager.Instance.EnableInput();
        }

        private string FormatPatchNotes(List<string> updates, List<string> bugs)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<b>Updates</b>");
            if (updates.Count == 0) sb.AppendLine("• None");
            else foreach (var u in updates) sb.AppendLine("• " + u);

            sb.AppendLine();
            sb.AppendLine("<b>Known Bugs:</b>");
            if (bugs.Count == 0) sb.AppendLine("• None");
            else foreach (var b in bugs) sb.AppendLine("• " + b);

            return sb.ToString();
        }
    }
}