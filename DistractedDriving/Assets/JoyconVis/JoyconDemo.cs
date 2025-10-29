using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class JoyconDemo : MonoBehaviour {
	
	[Tooltip("Zero-based index of the Joycon in the list of connected controllers.")]
    public int index = 0;

    [Tooltip("Analog stick deflection - very poor range. Apply deadzone!")]
    public Vector2 stick;

	[Tooltip("Rotation rate around each local axis.")]
    public Vector3 gyro;

	[Tooltip("Direction of acceleration in local coordinates (points up when supported against gravity).")]
    public Vector3 accel;    

	[Tooltip("Ready-to-use rotation combining accelerometer and gyroscope sensors.")]
	public Quaternion orientation;

	Transform model;
	private List<Joycon> joycons;

	[ContextMenu("Recenter")]
	void Recenter() {
		if (joycons != null && joycons.Count > index) {
			joycons[index].Recenter();
		}
	}

    void Start ()
    {
        gyro = new Vector3(0, 0, 0);
        accel = new Vector3(0, 0, 0);
        // get the public Joycon array attached to the JoyconManager in scene
        joycons = JoyconManager.Instance.j;
		if (joycons.Count <= index) {
			Destroy(gameObject);
		}
	}

    // Update is called once per frame
    void Update () {
		// make sure the Joycon only gets checked if attached
		if (joycons.Count > index)
        {
			Joycon j = joycons [index];

			if (JoyconManager.Instance?.prefabLeft != null && model == null) {
				var prefab = j.isLeft ? JoyconManager.Instance.prefabLeft : JoyconManager.Instance.prefabRight;
				model = Instantiate(prefab, transform);
			}
			
			// GetButtonDown checks if a button has been pressed (not held)
            if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
            {
				Debug.Log ("Shoulder button 2 pressed");
				// GetStick returns a 2-element vector with x/y joystick components
				Debug.Log($"Stick x: {j.GetStick()}");
            
				// Joycon has no magnetometer, so it cannot accurately determine its yaw value. Joycon.Recenter allows the user to reset the yaw value.
				j.Recenter ();
			}
			// GetButtonDown checks if a button has been released
			if (j.GetButtonUp (Joycon.Button.SHOULDER_2))
			{
				Debug.Log ("Shoulder button 2 released");
			}
			// GetButtonDown checks if a button is currently down (pressed or held)
			if (j.GetButton (Joycon.Button.SHOULDER_2))
			{
				Debug.Log ("Shoulder button 2 held");
			}

			if (j.GetButtonDown (Joycon.Button.DPAD_DOWN)) {
				Debug.Log ("Rumble");

				// Rumble for 200 milliseconds, with low frequency rumble at 160 Hz and high frequency rumble at 320 Hz. For more information check:
				// https://github.com/dekuNukem/Nintendo_Switch_Reverse_Engineering/blob/master/rumble_data_table.md

				j.SetRumble (160, 320, 0.6f, 200);

				// The last argument (time) in SetRumble is optional. Call it with three arguments to turn it on without telling it when to turn off.
                // (Useful for dynamically changing rumble values.)
				// Then call SetRumble(0,0,0) when you want to turn it off.
			}

            stick = j.GetStick();

            // Gyro values: x, y, z axis values (in radians per second)
            gyro = j.GetGyro();

            // Accel values:  x, y, z axis values (in Gs)
            accel = j.GetAccel();

            orientation = j.GetOrientation();

            gameObject.transform.rotation = orientation;
        }
    }
}