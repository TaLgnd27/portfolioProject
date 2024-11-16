using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum SceneIndex
{
    MainMenu,
    Game,
    PauseMenu,
    SettingsMenu
}

public enum GameState
{
    MainMenu,
    Game,
    Won,
    Pause
}

public class GameManager : MonoBehaviour
{
    GameState currentState;

    bool settingsOpen = false;

    private static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Time.timeScale = 1.0f;

        // Set the instance and mark as don't destroy on load
        instance = this;
        DontDestroyOnLoad(gameObject);
        currentState = GameState.MainMenu;
    }

    public void ChangeState(GameState state)
    {
        currentState = state;
    }

    public void ChangeScene(SceneIndex scene)
    {
        
        SceneManager.LoadScene((int) scene);
    }

    public void LoadMenu(SceneIndex scene)
    {
        SceneManager.LoadSceneAsync((int) scene, LoadSceneMode.Additive);
    }

    public void UnloadMenu(SceneIndex scene)
    {
        SceneManager.UnloadSceneAsync((int) scene);
    }

    public bool IsSceneLoaded(SceneIndex index)
    {
        Scene scene = SceneManager.GetSceneByBuildIndex((int) index);
        return scene.isLoaded;
    }

    public void ToggleMenu(SceneIndex scene)
    {
        if (!IsSceneLoaded(scene))
        {
            Debug.Log("Load");
            LoadMenu(scene);
        }
        else
        {
            Debug.Log("Unload");
            UnloadMenu(scene);
        }
    }

    public void StartGame()
    {
        ChangeScene(SceneIndex.Game);
        ChangeState(GameState.Game);
    }

    public void OnPauseAction(InputAction.CallbackContext context)
    {
        // Check if the action was Performed to ensure it's only triggered once
        if (context.performed)
        {
            TogglePauseGame();
        }
    }

    public void TogglePauseGame()
    {
        if (!IsSceneLoaded(SceneIndex.SettingsMenu))
        {
            //Debug.Log("Loading Scene");
            if (currentState == GameState.Game)
            {
                currentState = GameState.Pause;
                Time.timeScale = 0f;
                LoadMenu(SceneIndex.PauseMenu);
            }
            else if (currentState == GameState.Pause)
            {
                UnloadMenu(SceneIndex.PauseMenu);
                currentState = GameState.Game;
                Time.timeScale = 1.0f;
            }
        }
    }

    public void OnSettings()
    {
        settingsOpen = !settingsOpen;
        Debug.Log("Toggle Settings");
        ToggleMenu(SceneIndex.SettingsMenu);
    }

    public void EndGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.MoveGameObjectToScene(gameObject, currentScene);

        SceneManager.LoadScene((int) SceneIndex.MainMenu);
    }
}
