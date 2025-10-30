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

	public Joycon myJoycon;

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
	}

    // Update is called once per frame
    void Update () {
		// make sure the Joycon only gets checked if attached
		if (joycons.Count > index)
        {
			Joycon j = joycons [index];
			myJoycon = j;

			if (JoyconManager.Instance?.prefabLeft != null && model == null) {
				var prefab = j.isLeft ? JoyconManager.Instance.prefabLeft : JoyconManager.Instance.prefabRight;
				model = Instantiate(prefab, transform);

			}
			orientation = j.GetOrientation();
			gameObject.transform.localRotation = orientation;
        }
    }
}