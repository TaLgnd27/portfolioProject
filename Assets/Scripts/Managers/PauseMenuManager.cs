using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    private static PauseMenuManager instance;
    public static GameManager gm;

    public bool settingsOpen = false;

    private void Awake()
    {
        if (gm == null)
        {
            gm = FindAnyObjectByType<GameManager>();
            //return;
        }


        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set the instance and mark as don't destroy on load
        instance = this;
    }

    public void OnResume()
    {
        if (gm != null && !settingsOpen)
        {
            gm.TogglePauseGame();
        }
    }

    public void OnSettings()
    {
        settingsOpen = !settingsOpen;
        Debug.Log("Toggle Settings");
        gm.ToggleMenu(SceneIndex.SettingsMenu);
    }

    public void OnQuit()
    {
        gm.EndGame();
    }
}
