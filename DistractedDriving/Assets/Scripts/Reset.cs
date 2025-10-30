using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    Joycon j;
    private void Start()
    {
        j = JoyconManager.Instance.j[0];
    }
    void Update()
    {
        if (j.GetButton(Joycon.Button.DPAD_DOWN)
            || j.GetButton(Joycon.Button.DPAD_UP)
            || j.GetButton(Joycon.Button.DPAD_LEFT)
            || j.GetButton(Joycon.Button.DPAD_RIGHT)
            || Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Map1");
        }
    }
}
