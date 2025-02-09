using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public int floor;

    public static GameManager instance;

    public Image fadePanel; // Reference to the Panel's Image
    public float fadeDuration = 1f;

    public bool isTransitioning;

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
        DontDestroyOnLoad(fadePanel.GetComponentInParent<Canvas>().gameObject);
        currentState = GameState.MainMenu;
    }

    private void Start()
    {
        // Optional: Start with a fade-in effect
        if (fadePanel != null)
        {
            StartCoroutine(FadeIn());
        }
    }

    public void ChangeState(GameState state)
    {
        currentState = state;
    }

    public void ChangeScene(SceneIndex scene)
    {

        if (fadePanel != null && !isTransitioning)
        {
            StartCoroutine(FadeOutAndChangeScene(scene));
        }
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
        floor = 0;
        ChangeScene(SceneIndex.Game);
        ChangeState(GameState.Game);
    }

    public void LoadNewFloor()
    {
        ChangeScene(SceneIndex.Game);
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

    public void OnQuit()
    {
        Application.Quit();
    }

    public void EndGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.MoveGameObjectToScene(gameObject, currentScene);
        DestroyAll();

        SceneManager.LoadScene((int) SceneIndex.MainMenu);
    }

    public void DestroyAll()
    {
        GameObject tempParent = new GameObject("TempParent");

        try
        {
            // Temporarily move all DontDestroyOnLoad objects under a new parent
            foreach (GameObject obj in FindObjectsOfType<GameObject>())
            {
                if (obj.scene.name == null || obj.scene.name == "" || obj.scene.name == "DontDestroyOnLoad") // Indicates DontDestroyOnLoad
                {
                    obj.transform.SetParent(tempParent.transform);
                }
            }

            // Destroy the temporary parent, which destroys all children
            //Destroy(tempParent);
        }
        finally
        {
            // Ensure we clean up the temporary parent if something goes wrong
            if (tempParent != null)
            {
                //Destroy(tempParent);
            }
        }

        Debug.Log("All DontDestroyOnLoad objects destroyed.");
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color panelColor = fadePanel.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration); // Fade from 1 (opaque) to 0 (transparent)
            fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, alpha);
            yield return null;
        }

        fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, 0); // Ensure full transparency
    }

    private System.Collections.IEnumerator FadeOutAndChangeScene(SceneIndex scene)
    {
        isTransitioning = true;
        float elapsedTime = 0f;
        Color panelColor = fadePanel.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration); // Fade from 0 (transparent) to 1 (opaque)
            fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, alpha);
            yield return null;
        }

        fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, 1); // Ensure full opacity

        // Load the new scene
        SceneManager.LoadScene((int) scene);

        StartCoroutine(FadeIn());
        isTransitioning = false;
    }
}
