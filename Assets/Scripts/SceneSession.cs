using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSession : MonoBehaviour
{
    // key of new scene
    int sceneKey;

    // Awake is called once before the Start
    void Awake()
    {
        // set scene key of new scene
        sceneKey = SceneManager.GetActiveScene().buildIndex;
        // get the active scenes (the first one is the first inserted)
        SceneSession[] activeSceneSessions = FindObjectsByType<SceneSession>(FindObjectsSortMode.None);
        foreach (var session in activeSceneSessions)
        {
            if (session == this) continue;

            if (session.sceneKey == sceneKey)
            {
                // If an existing session has the same scene key, destroy this new one
                //Debug.Log("Duplicate session, destroying new instance");
                Destroy(gameObject);
                return;
            }
            else
            {
                // Otherwise, destroy the old one
                //Debug.Log("Scene changed, destroying old session");
                Destroy(session.gameObject);
            }
        }

        // No duplicates, mark this as persistent
        //Debug.Log("SceneSession initialized and kept alive");
        DontDestroyOnLoad(gameObject);
    }

    // destroy scene persist
    public void DestroySceneSession()
    {
        Destroy(gameObject);
    }
}
