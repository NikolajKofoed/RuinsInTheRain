using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
	[SerializeField] private string respawnScenePos;
	[SerializeField] public GameObject _gameOverUI;

	private void Start()
	{
		_gameOverUI.SetActive(false);
		string activeTransition = SceneManagement.Instance.SceneTransitionName;

		if (string.IsNullOrEmpty(activeTransition))
		{
			activeTransition = "Start";
		}
		Debug.Log($"Transition name: {respawnScenePos}");
		if (respawnScenePos == SceneManagement.Instance.SceneTransitionName)
		{
			Debug.Log("transition name is correct");
			// Do NOT move the player here
		}
		Debug.Log($"Error: name:{SceneManagement.Instance.SceneTransitionName}");
	}

	public void GameOverUI()
	{
		_gameOverUI.SetActive(true);
	}

	public void MainMenu()
	{
		_gameOverUI.SetActive(false);
		Health.PlayerIsDead = false;
		SceneManager.LoadSceneAsync(0);
	}

	public void ResetScene()
	{
		_gameOverUI.SetActive(false);
		// Just in case, reset player static state
		Health.PlayerIsDead = false;

		// Set scene entrance so AreaEntrance can place the player
		string lastRespawn = SceneManagement.Instance.RespawnTransitionName;
		Debug.Log($"[GameOverMenu] Setting respawn transition to: " + respawnScenePos);

		SceneManagement.Instance.SetTransitionName(lastRespawn);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}