using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton pattern ensuring persistance through scenes and the existence of only one gameobject of that type,
/// is useful for ensuring only 1 player exists.
/// </summary>
/// <typeparam name="T">The script that implements the singleton class</typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;
    public static T Instance { get { return _instance; } }

    protected virtual void Awake()
    {
        if (_instance != null && this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = (T)this;
        }

        if (!gameObject.transform.parent)
        {
            // don't destroy on sceneload
            DontDestroyOnLoad(gameObject);
        }
    }
}
