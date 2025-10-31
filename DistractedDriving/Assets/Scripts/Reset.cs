using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    Joycon j; string sceneName;
    private void Start()
    {
        if(JoyconManager.Instance.j.Count > 0) { j = JoyconManager.Instance.j[0]; }
        sceneName = SceneManager.GetActiveScene().name;
    }
    void Update()
    {
        if (j == null)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
        else
        {
            if (j.GetButton(Joycon.Button.DPAD_DOWN)
            || j.GetButton(Joycon.Button.DPAD_UP)
            || j.GetButton(Joycon.Button.DPAD_LEFT)
            || j.GetButton(Joycon.Button.DPAD_RIGHT)
            || Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
