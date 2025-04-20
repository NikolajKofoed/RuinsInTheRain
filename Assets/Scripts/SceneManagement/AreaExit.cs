using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// Used to load another scene
public class AreaExit : MonoBehaviour
{
    /// <summary>
    /// Name of another scene to load
    /// </summary>
    [SerializeField] private string sceneName;
    /// <summary>
    /// Name of the location where the new scene will be loaded, must match a value of the AreaEntrance script
    /// </summary>
    [SerializeField] private string sceneTransitionName;


    /// <summary>
    /// Starts the scene transition when hitting a collider on the object this script is on
    /// </summary>
    /// <param name="collision">The collider that triggers the scene transition</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player2D>())
        {
            StartCoroutine(LoadSceneRoutine());
        }
    }

    /// <summary>
    /// Loads another scene using a smooth transition darkening when loading the scene
    /// </summary>
    /// <returns>Wait time between scenes</returns>
    private IEnumerator LoadSceneRoutine()
    {
        UIFade.Instance.FadeToBlack();
        SceneManagement.Instance.SetTransitionName(sceneTransitionName);
        SceneManagement.Instance.SetRespawnTransitionName(sceneTransitionName);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}