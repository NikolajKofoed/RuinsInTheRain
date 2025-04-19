using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MenuScreen;
    public GameObject OptionMenu;
	public GameObject QuitMenu;


	public void PlayGame()
    {
		// Reset any necessary game data before loading the scene
		// ResetGameData();

		// Load the first gameplay scene (Scene 1 in the build order)
		SceneManager.LoadSceneAsync(1);
    }

	//private void ResetGameData()
	//{
		// Reset any static variables, game state, or settings you need to reset
	//}

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