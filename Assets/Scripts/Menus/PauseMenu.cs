using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject OptionMenuUI;
    public GameObject MainMenuReturnUI;
    public GameObject QuitGameUI;

    private void Awake()
    {
        // This is to make the UI canvas deleted when the player return to the main menu
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "MainMenu")
		{
			Destroy(gameObject);
		}
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	// Update is called once per frame
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
			if (Health.PlayerIsDead) return; // Prevent pause if dead

			if (GameIsPaused)
            {
                Resume();
                Cursor.visible = false;
            } else
            {
                Pause();
                Cursor.visible = true;
            }
        }
        
    }

    public void Resume ()
    {
        PauseMenuUI.SetActive(false);
        OptionMenuUI.SetActive(false);
        MainMenuReturnUI.SetActive(false);
        QuitGameUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
        Cursor.visible = false;

    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.visible = true;

    }

    public void SettingsMenu()
    {
        PauseMenuUI.SetActive(false);
        OptionMenuUI.SetActive(true);
        Cursor.visible = true;

    }

    public void ReturnToMainMenu()
    {
        PauseMenuUI.SetActive(false);
        MainMenuReturnUI.SetActive(true);
        Cursor.visible = true;

    }

    public void QuttingMenu()
    {
        PauseMenuUI.SetActive(false);
        QuitGameUI.SetActive(true);
        Cursor.visible = true;

    }

    public void ReturnToPauseMenu()
    {
        OptionMenuUI.SetActive(false);
        MainMenuReturnUI.SetActive(false);
        QuitGameUI.SetActive(false);
        PauseMenuUI.SetActive(true);
        Cursor.visible = true;

    }

    public void MainMenu()
    {
		PauseMenuUI.SetActive(false);
		OptionMenuUI.SetActive(false);
		MainMenuReturnUI.SetActive(false);
		QuitGameUI.SetActive(false);
		Time.timeScale = 1.0f;
		GameIsPaused = false;
		SceneManager.LoadSceneAsync(0);
        Cursor.visible = true;

    }

    public void QuitGame()
    {
        Debug.Log("Quit game!");
        Application.Quit();
    }
}