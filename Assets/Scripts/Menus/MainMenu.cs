using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MenuScreen;
    public GameObject OptionMenu;
	public GameObject QuitMenu;

    private void Start()
    {
        GameObject goUI = GameObject.Find("GameOverMenu");
        if (goUI != null && goUI.activeSelf)
        {
            goUI.SetActive(false);
            Debug.Log("GameOverMenu hidden on returning to main menu.");
        }
    }

	public void PlayGame()
    {
		// Load the first gameplay scene (Scene 1 in the build order)
		SceneManager.LoadSceneAsync(1);
    }

	public void Settings()
    {
        MenuScreen.SetActive(false);
        OptionMenu.SetActive(true);
    }

    public void Quitting()
    {
        MenuScreen.SetActive(false);
        QuitMenu.SetActive(true);
    }

    public void Return()
    {
        MenuScreen.SetActive(true);
        OptionMenu.SetActive(false);
        QuitMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit game!");
		Application.Quit();
	}
}