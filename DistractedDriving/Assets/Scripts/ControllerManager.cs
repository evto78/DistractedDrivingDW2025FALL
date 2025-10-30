using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ControllerManager : MonoBehaviour
{
    public JoyconDemo connectedDemo;
    Joycon connectedJoycon;
    // Controls
    public Vector3 gyro;
    public Vector3 accel; public float accelMagnitude;
    public Quaternion orientation;
    public Vector3 demoOrientation;
    public bool buttonsPressed;
    void Update()
    {
        if (connectedJoycon == null)
        {
            if(connectedDemo.myJoycon != null) { connectedJoycon = connectedDemo.myJoycon; } else { return; }
        }

        gyro = connectedJoycon.GetGyro();
        accel = connectedJoycon.GetAccel(); accelMagnitude = accel.magnitude;
        orientation = connectedJoycon.GetOrientation();
        demoOrientation = connectedDemo.transform.localEulerAngles;

        if (connectedJoycon.GetButton(Joycon.Button.DPAD_DOWN)
            || connectedJoycon.GetButton(Joycon.Button.DPAD_UP)
            || connectedJoycon.GetButton(Joycon.Button.DPAD_LEFT)
            || connectedJoycon.GetButton(Joycon.Button.DPAD_RIGHT))
        {
            buttonsPressed = true;
            connectedJoycon.SetRumble(160, 320, 0.6f, 200);
        }
        else { buttonsPressed = false; }
        //Debug.Log(connectedJoycon.)
    }
    public void Recenter()
    {
        if(connectedJoycon == null) { return; }
        connectedJoycon.Recenter();
    }
}
