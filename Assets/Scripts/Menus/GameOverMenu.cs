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

	public void ResetScene()
	{
		_gameOverUI.SetActive(false);

		// Set scene entrance so AreaEntrance can place the player
		SceneManagement.Instance.SetTransitionName("Start");  // must match a real entrance name

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}