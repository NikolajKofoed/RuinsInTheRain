using UnityEngine;

/// <summary>
/// Sets the name of the scene that is to be loaded, will not work if a scene of that name doesn't exist
/// </summary>
public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneTransitionName { get; private set; }
    public string RespawnTransitionName { get; private set; }
    public void SetTransitionName(string sceneTransitionName)
    {
        this.SceneTransitionName = sceneTransitionName;
    }

    public void SetRespawnTransitionName(string transitionName)
    {
        RespawnTransitionName = transitionName;
    }
}