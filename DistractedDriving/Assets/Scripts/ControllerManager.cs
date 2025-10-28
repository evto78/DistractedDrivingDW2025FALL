using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
public class ControllerManager : MonoBehaviour
{
    public XInputController controllerX;
    public UnityEngine.InputSystem.Gamepad controllerG;
    void Start()
    {
        controllerG = XInputController.current;
        Debug.Log(controllerG.allControls);
        foreach(UnityEngine.InputSystem.InputControl ic in controllerG.allControls)
        {
            Debug.Log(ic.displayName + " | " + ic.name + " | " + ic.magnitude);
        }
        
    }
    void Update()
    {
    }
}
