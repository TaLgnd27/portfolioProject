using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsMenuManager : MonoBehaviour
{
    private static SettingsMenuManager instance;
    public static GameManager gm;
    public static PauseMenuManager pm;

    private void Awake()
    {
        if (gm == null)
        {
            gm = FindAnyObjectByType<GameManager>();
            //return;
        }

        if (pm == null)
        {
            pm = FindAnyObjectByType<PauseMenuManager>();
            //return;
        }


        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            //return;
        }

        // Set the instance and mark as don't destroy on load
        instance = this;
    }

    public void OnReturnAction(InputAction.CallbackContext context)
    {
        Debug.Log("Return");
        // Check if the action was Performed to ensure it's only triggered once
        if (context.performed)
        {
            CloseSettings();
        }
    }

    public void CloseSettings()
    {
        if (pm != null)
        {
            pm.OnSettings();
            return;
        }
        else if (gm != null)
        {
            gm.ToggleMenu(SceneIndex.SettingsMenu);
            return;
        }
    }

    
}
