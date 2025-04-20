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
			Player2D.Instance.transform.position = this.transform.position;
			CameraController.Instance.SetPlayerCameraFollow();

			UIFade.Instance.FadeToClear();
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
		SceneManager.LoadSceneAsync(0);
	}

	public void ResetScene()
	{
		_gameOverUI.SetActive(false);

		// Set scene entrance so AreaEntrance can place the player
		SceneManagement.Instance.SetTransitionName(respawnScenePos);  // must match a real entrance name

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}