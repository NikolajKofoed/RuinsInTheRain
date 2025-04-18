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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
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
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void SettingsMenu()
    {
        PauseMenuUI.SetActive(false);
        OptionMenuUI.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        PauseMenuUI.SetActive(false);
        MainMenuReturnUI.SetActive(true);
    }

    public void QuttingMenu()
    {
        PauseMenuUI.SetActive(false);
        QuitGameUI.SetActive(true);
    }

    public void ReturnToPauseMenu()
    {
        OptionMenuUI.SetActive(false);
        MainMenuReturnUI.SetActive(false);
        QuitGameUI.SetActive(false);
        PauseMenuUI.SetActive(true);
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
	}

    public void QuitGame()
    {
        Debug.Log("Quit game!");
        Application.Quit();
    }
}
