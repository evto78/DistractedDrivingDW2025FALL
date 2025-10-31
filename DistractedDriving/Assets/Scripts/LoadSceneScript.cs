using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneScript : MonoBehaviour
{
    public string sceneToLoad;
    public void SetPreference(bool useJoycons)
    {
        if (useJoycons) { PlayerPrefs.SetInt("JOYMODE", 1); }
        else { PlayerPrefs.SetInt("JOYMODE", 0); }
    }
    public void LoadTheScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
