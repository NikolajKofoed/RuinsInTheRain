using System.Collections;
using UnityEngine;


/// <summary>
/// Used to specify a location on the scene that can be loaded by the AreaExit script
/// </summary>
public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;

    private void Start()
    {
		string activeTransition = SceneManagement.Instance.SceneTransitionName;

		if (string.IsNullOrEmpty(activeTransition))
		{
			activeTransition = "Start";
		}

		Debug.Log($"Transition name: {transitionName}");
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            Debug.Log("transition name is correct");
            Player2D.Instance.transform.position = this.transform.position; //Set player position to be where entrance is.
            CameraController.Instance.SetPlayerCameraFollow();

            UIFade.Instance.FadeToClear();
        }
        Debug.Log($"Error: name:{SceneManagement.Instance.SceneTransitionName}");
    }
}
