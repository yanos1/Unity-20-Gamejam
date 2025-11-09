using System;
using Managers;
using TMPro;
using UnityEngine;

public class endMenu : MonoBehaviour
{
    // Update is called once per frame
    public void Restart()
    {
        System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe"));
        Application.Quit();
    }

    private void OnEnable()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = $"after {VersionManager.Instance.currentVersion} inhouse versions, Punity was released to the public. It was trash and people sticked to unity, the end." ;
    }
}
