using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
	public GameObject _gameOverUI;

	public void GameOverUI()
	{
		_gameOverUI.SetActive(true);
	}

	public void MainMenu()
	{
		_gameOverUI.SetActive(false);
		SceneManager.LoadSceneAsync(0);
	}

	public void ContinueGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
