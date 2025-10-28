using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarManager : MonoBehaviour
{
    [Header("Steering")]
    public Transform wheel;
    public float steeringIntensity;
    public float wheelResistence;
    public float resetResistence;
    [Header("Camera")]
    public Transform camTransform; Vector3 camNormalPos;
    public Camera cam;
    public float camShakeIntensityModifier;
    float currentCamShakeTinensity;
    float camShakeTimer;
    [Header("Driving")]
    public Transform terrain;
    public float drivingSpeed;
    public float currentSpeed;
    public Vector2 minMaxSpeed;
    [Header("User Interface")]
    public TextMeshProUGUI kph;

    void Start()
    {
        camNormalPos = camTransform.localPosition;
        currentCamShakeTinensity = 0f;
    }
    void Update()
    {
        InputManager();
        wheel.transform.localEulerAngles = steeringIntensity * -480 * Vector3.forward;

        ManageCameraShake();
        ManageUI();
        terrain.transform.position -=  currentSpeed * Time.deltaTime * Vector3.forward;
        if(terrain.transform.position.z < -250f) { terrain.transform.position += 250f * Vector3.forward; }
        currentSpeed = Mathf.Lerp(currentSpeed, minMaxSpeed.x, Time.deltaTime/2f);
    }
    void InputManager()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            steeringIntensity -= Time.deltaTime * 2f * wheelResistence;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            steeringIntensity += Time.deltaTime * 2f * wheelResistence;
        }
        else
        {
            if(steeringIntensity > 0) { steeringIntensity -= Time.deltaTime * resetResistence; if (steeringIntensity < 0) { steeringIntensity = 0; } }
            if(steeringIntensity < 0) { steeringIntensity += Time.deltaTime * resetResistence; if (steeringIntensity > 0) { steeringIntensity = 0; } }
        }
        steeringIntensity = Mathf.Clamp(steeringIntensity, -1f, 1f);

        if (Input.GetKeyDown(KeyCode.Space)) { CameraShake(1f); }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            currentSpeed += Time.deltaTime * drivingSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            currentSpeed -= Time.deltaTime * drivingSpeed;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, minMaxSpeed.x, minMaxSpeed.y);
    }
    void CameraShake(float intensity)
    {
        camShakeTimer = 0.5f;
        currentCamShakeTinensity += intensity * camShakeIntensityModifier;
    }
    void ManageCameraShake()
    {
        if (camShakeTimer <= 0) { camShakeTimer = 0f; camTransform.localPosition = camNormalPos; currentCamShakeTinensity = 0f; return; }
        camTransform.localPosition = camNormalPos + new Vector3(Random.Range(-currentCamShakeTinensity, currentCamShakeTinensity), Random.Range(-currentCamShakeTinensity/2f, currentCamShakeTinensity/2f), Random.Range(-currentCamShakeTinensity/3f, currentCamShakeTinensity/3f));
        camShakeTimer -= Time.deltaTime * currentCamShakeTinensity * 1.5f; currentCamShakeTinensity -= Time.deltaTime * currentCamShakeTinensity*1.5f;
    }
    void ManageUI()
    {
        kph.text = Mathf.RoundToInt(currentSpeed*2f) + " / KPH";
    }
}
