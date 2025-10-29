using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
[System.Serializable]
public class CarManager : MonoBehaviour
{
    //can drive bool to stop for obstcles
    public bool canDrive = true;

    [Header("Stats")]
    public float drivingSpeed;
    public float turningSpeed;
    public float turnAngle;
    [Header("Steering")]
    public bool joyConSteering;
    public Transform wheel;
    public float steeringIntensity;
    public float resetResistence;
    public AnimationCurve turnCurve;
    float currentTurn;
    float shakeAffectAngle;
    public float maxZTurn;
    [Header("Camera")]
    public Transform camTransform; Vector3 camNormalPos;
    public Camera cam;
    public float camShakeIntensityModifier;
    float currentCamShakeTinensity;
    float camShakeTimer;
    [Header("Driving")]
    public float currentSpeed;
    public Vector2 minMaxSpeed;
    [Header("User Interface")]
    public TextMeshProUGUI kph;
    ControllerManager controllerManager;
    bool paused;
    public GameObject setupUI;
    public GameObject setupText1;
    public GameObject setupText2; public Image radialFillCircle; float stayStillTimer; public TextMeshProUGUI stayStillText;
    public GameObject setupText3;
    [Header("Effects")]
    public Transform tireTreads;
    [Header("Pizza Boxes")]
    public List<PizzaBoxScript> pizzas;
    [Header("Audio")]
    public AudioSource horn;

    public bool horned = false;

    void Start()
    {
        camNormalPos = camTransform.localPosition;
        currentCamShakeTinensity = 0f;
        currentTurn = transform.localEulerAngles.y;
        controllerManager = GetComponent<ControllerManager>();
        //paused = true;
        //StartCoroutine(IntroSetUp());
    }
    void Update()
    {
        if (paused) { return; }

        InputManager();

        wheel.transform.localEulerAngles = steeringIntensity * -480 * Vector3.forward;
        currentSpeed = Mathf.Lerp(currentSpeed, minMaxSpeed.x, Time.deltaTime / 2f);

        ManageCameraShake();
        ManageUI();
        ManageTurning();
        Move();
        ManageCarShake();
    }
    //Honking feature
    void Honk()
    {
        if(horn == null) { return; }
        horn.Play();
        Debug.Log("Honk!");
    }
    void Move()
    {
        transform.position += currentSpeed * Time.deltaTime * transform.forward;
    }
    void InputManager()
    {
        if (!joyConSteering)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                steeringIntensity -= Time.deltaTime * turningSpeed;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
               steeringIntensity += Time.deltaTime * turningSpeed;
            }
            else
            {
                if(steeringIntensity > 0) { steeringIntensity -= Time.deltaTime * resetResistence; if (steeringIntensity < 0) { steeringIntensity = 0; } }
                if(steeringIntensity < 0) { steeringIntensity += Time.deltaTime * resetResistence; if (steeringIntensity > 0) { steeringIntensity = 0; } }
            }
        }
        else
        {
            steeringIntensity = Mathf.Lerp(steeringIntensity, controllerManager.gyro.y / 5f, Time.deltaTime * 10);
        }
        steeringIntensity = Mathf.Clamp(steeringIntensity, -1f, 1f);

        if (Input.GetKeyDown(KeyCode.Space)) { CameraShake(1f); }
        if (controllerManager.buttonsPressed || Input.GetKey(KeyCode.Space)) { Honk(); } horned = controllerManager.buttonsPressed || Input.GetKey(KeyCode.Space);

        if (canDrive == true && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow ))
        {
            currentSpeed += Time.deltaTime * drivingSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            currentSpeed -= Time.deltaTime * drivingSpeed;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, minMaxSpeed.x, minMaxSpeed.y);
        //Tire Tracks
        bool emitTracks = currentSpeed > minMaxSpeed.y / 6f && (steeringIntensity > 0.2f || steeringIntensity < 0.2f);
        foreach (TrailRenderer tr in tireTreads.GetComponentsInChildren<TrailRenderer>()) { tr.emitting = emitTracks; }
    }
    void ManageTurning()
    {
        float yAngle;
        if(steeringIntensity < 0) { yAngle = turnCurve.Evaluate(-steeringIntensity) * -turnAngle; }
        else { yAngle = turnCurve.Evaluate(steeringIntensity) * turnAngle; }
        currentTurn += (yAngle/2f)*((currentSpeed / minMaxSpeed.y) * 2f);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, currentTurn+yAngle, ((Random.Range(yAngle/8f,yAngle)/turnAngle)*currentSpeed/(minMaxSpeed.y/2f))*maxZTurn);
        foreach(PizzaBoxScript p in pizzas)
        {
            p.currentStability += (yAngle / turnAngle) * Time.deltaTime * 1.6f;
            if(p.currentStability > 0) { p.currentStability -= Time.deltaTime; }
            if(p.currentStability < 0) { p.currentStability += Time.deltaTime; }
        }
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
    void ManageCarShake()
    {
        shakeAffectAngle += Time.deltaTime * 2; if(shakeAffectAngle > 1) { shakeAffectAngle = -1; }
        float shakeIntensity = (currentSpeed / minMaxSpeed.y)*170*Time.deltaTime;
        transform.position = new Vector3(transform.position.x, Random.Range(0f,shakeIntensity), transform.position.z);
        steeringIntensity += shakeAffectAngle * Random.Range(0f, shakeIntensity) / 40f;
    }
    void ManageUI()
    {
        kph.text = Mathf.RoundToInt(currentSpeed*2f) + " / KPH";
        kph.transform.parent.localScale = Vector3.one * (((currentSpeed*2f)/minMaxSpeed.y)+0.8f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ob") { Destroy(gameObject); }
    }

    IEnumerator IntroSetUp()
    {
        setupUI.SetActive(true);
        setupText1.SetActive(true);
        setupText2.SetActive(false);
        setupText3.SetActive(false);
        while (!controllerManager.buttonsPressed && !Input.GetKey(KeyCode.Space))
        {
            yield return new WaitForEndOfFrame();
        }
        setupText1.SetActive(false);
        setupText2.SetActive(true);
        stayStillTimer = 0f;
        while (stayStillTimer < 5.3f)
        {
            if (Mathf.Abs(controllerManager.accelMagnitude-1) < 0.5f) { stayStillTimer += Time.deltaTime; } else { stayStillTimer = 0f; }
            radialFillCircle.fillAmount = stayStillTimer / 5f; stayStillText.text = Mathf.RoundToInt(5-stayStillTimer).ToString();
            yield return new WaitForEndOfFrame();
        }
        controllerManager.Recenter();
        setupText2.SetActive(false);
        setupText3.SetActive(true);
        while (!controllerManager.buttonsPressed && !Input.GetKey(KeyCode.Space))
        {
            yield return new WaitForEndOfFrame();
        }

        //Done setup
        setupUI.SetActive(false);
        setupText1.SetActive(false);
        setupText2.SetActive(false);
        setupText3.SetActive(false);
        paused = false; yield return null;
    }
}
