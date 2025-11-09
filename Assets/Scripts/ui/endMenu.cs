using UnityEngine;

public class endMenu : MonoBehaviour
{


    // Update is called once per frame
    public void Restart()
    {
        System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe"));
        Application.Quit();
    }
}
