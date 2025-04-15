using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //SceneManager.LoadSceneAsync(1);
    }

    public void Settings()
    {

    }

    public void QuitGame()
    {
        Console.WriteLine("Quit game");
        Application.Quit();
    }
}
