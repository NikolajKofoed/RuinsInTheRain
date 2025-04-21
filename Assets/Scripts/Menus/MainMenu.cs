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
        Cursor.visible = true;
    }

    public void PlayGame()
    {
		// Load the first gameplay scene (Scene 1 in the build order)
		SceneManager.LoadSceneAsync(1);
        Cursor.visible = false;
    }

	public void Settings()
    {
        MenuScreen.SetActive(false);
        OptionMenu.SetActive(true);
        Cursor.visible = true;

    }

    public void Quitting()
    {
        MenuScreen.SetActive(false);
        QuitMenu.SetActive(true);
        Cursor.visible = true;

    }

    public void Return()
    {
        MenuScreen.SetActive(true);
        OptionMenu.SetActive(false);
        QuitMenu.SetActive(false);
        Cursor.visible = true;

    }

    public void QuitGame()
    {
        Debug.Log("Quit game!");
		Application.Quit();
	}
}