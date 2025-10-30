using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;
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
    public float joyConTurnProgress;
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
    public PhoneScript phone;
    SoundManager sm;
    [Header("Effects")]
    public Transform tireTreads;
    public GameObject explosionEffect;
    [Header("Pizza Boxes")]
    public List<PizzaBoxScript> pizzas;
    public GameObject pizzaSetPrefab;
    public Transform pizzaSpawnPos;
    public Transform deliverPoints;
    public DeliveryPoint targetPoint;
    public NavMeshAgent agent; float agentTimer = 0;
    public float deliveryTimer = 100; public Image phoneFill; public TextMeshProUGUI phoneTimer;
    public int moneyEarned = 0; public TextMeshProUGUI moneyEarnedText; public TextMeshProUGUI deadMoneyEarnedText; public GameObject deathUI;
    int deliverysMade = 1;
    public GameObject pizzaPlayerIcon;
    public GameObject truckPlayerIcon;
    [Header("Audio")]
    public AudioSource horn;

    public bool horned = false;

    void Start()
    {
        deliveryTimer = 99999999f; phoneTimer.text = "...";
        joyConTurnProgress = 0f;
        camNormalPos = camTransform.localPosition;
        currentCamShakeTinensity = 0f;
        currentTurn = transform.localEulerAngles.y;
        controllerManager = GetComponent<ControllerManager>();
        if (joyConSteering)
        {
            paused = true;
            StartCoroutine(IntroSetUp());
        }
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        truckPlayerIcon.SetActive(true);
    }
    void Update()
    {
        if (paused) { return; }

        InputManager();

        if (joyConSteering) { JoyconTurningWheel(); }
        else
        {
            wheel.transform.localEulerAngles = steeringIntensity * -480 * Vector3.forward;
        }
        currentSpeed = Mathf.Lerp(currentSpeed, minMaxSpeed.x, Time.deltaTime / 2f);

        ManageCameraShake();
        ManageUI();
        ManagePointer();
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
    void JoyconTurningWheel()
    {
        //Quaternion curRotation = wheel.localRotation;
        //wheel.transform.localRotation = controllerManager.orientation;
        //wheel.transform.localEulerAngles = (Vector3.forward * wheel.transform.localEulerAngles.x) + Vector3.forward * 90f;
        //Quaternion tarRotation = wheel.localRotation;
        //wheel.localRotation = Quaternion.Lerp(curRotation, tarRotation, Time.deltaTime * 4f);
        wheel.localEulerAngles += -Vector3.forward * controllerManager.gyro.y/2f;
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
            joyConTurnProgress += controllerManager.gyro.y/25f;
            steeringIntensity = Mathf.Lerp(steeringIntensity, joyConTurnProgress/5f, Time.deltaTime * 10);
        }
        steeringIntensity = Mathf.Clamp(steeringIntensity, -1f, 1f);

        if (Input.GetKeyDown(KeyCode.Space)) { CameraShake(1f); }
        if (controllerManager.buttonsPressed || Input.GetKey(KeyCode.Space)) { Honk(); } horned = controllerManager.buttonsPressed || Input.GetKey(KeyCode.Space);

        if (canDrive == true && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow )))
        {
            currentSpeed += Time.deltaTime * drivingSpeed;
        }
        if (canDrive == true && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            currentSpeed -= Time.deltaTime * drivingSpeed;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, minMaxSpeed.x-20, minMaxSpeed.y);
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
            p.currentStability += (yAngle / turnAngle) * Time.deltaTime * (currentSpeed/(minMaxSpeed.y/10f));
            if(p.currentStability > 0) { p.currentStability -= Time.deltaTime; }
            if(p.currentStability < 0) { p.currentStability += Time.deltaTime; }
        }
    }
    public void CameraShake(float intensity)
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
        deliveryTimer -= Time.deltaTime; phoneFill.fillAmount = deliveryTimer / 30f; phoneTimer.text = Mathf.RoundToInt(deliveryTimer).ToString();
        if(deliveryTimer < 0) { Crash(); }
        if(deliveryTimer > 100) { phoneTimer.text = "..."; }
        kph.text = Mathf.RoundToInt(currentSpeed*1f) + " / KPH";
        kph.transform.parent.localScale = Vector3.one * (((currentSpeed*2f)/minMaxSpeed.y)+0.8f);
        moneyEarnedText.text = moneyEarned + ".00$";

        if(targetPoint != null)
        {
            targetPoint.IconUpdate(deliveryTimer / (32f / (deliverysMade / 2f)));
        }

        sm.SetMotorVolAndPitch(currentSpeed, minMaxSpeed.y / 2f);
    }
    public void Crash()
    {
        deathUI.SetActive(true);
        deadMoneyEarnedText.text += moneyEarnedText.text;

        GameObject explosion = Instantiate(explosionEffect);
        explosion.transform.position = transform.position;
        explosion.transform.rotation = transform.rotation;
        explosion.GetComponent<CarExplosion>().crashVel = transform.forward * currentSpeed;
        Destroy(gameObject);
    }
    public void PickUpPizza()
    {
        pizzaPlayerIcon.SetActive(true); truckPlayerIcon.SetActive(false);
        if(pizzas.Count < 1) { deliveryTimer = 32f / (deliverysMade / 2f); }
        pizzas.Clear();
        if(pizzaSpawnPos.childCount > 0) { Destroy(pizzaSpawnPos.GetChild(0).gameObject); }
        Instantiate(pizzaSetPrefab, pizzaSpawnPos);
        pizzas.AddRange(pizzaSpawnPos.GetComponentsInChildren<PizzaBoxScript>());
        phone.ChangeState(PhoneScript.state.dropoff);

        targetPoint = deliverPoints.GetChild(Random.Range(0, deliverPoints.childCount)).GetComponent<DeliveryPoint>();
        targetPoint.gameObject.SetActive(true);
    }
    public void Deliver()
    {
        moneyEarned += Mathf.RoundToInt(pizzas.Count * (deliveryTimer / (16f/(deliverysMade/2f))));
        pizzaPlayerIcon.SetActive(false); truckPlayerIcon.SetActive(true);

        pizzas.Clear();
        if (pizzaSpawnPos.childCount > 0) { Destroy(pizzaSpawnPos.GetChild(0).gameObject); }

        targetPoint = null;
        phone.ChangeState(PhoneScript.state.pickup);
        deliveryTimer = 32f / (deliverysMade / 2f);
        deliverysMade++;
    }
    public void ManagePointer()
    {
        if (agent.isActiveAndEnabled) { agent.isStopped = true; }
        agentTimer -= Time.deltaTime; if(agentTimer < 0) { agentTimer = 0.05f; } else { return; }
        if(targetPoint == null) { return; }
        //Debug.Log("Spawn");
        NavMeshAgent agentClone = Instantiate(agent.gameObject, null).GetComponent<NavMeshAgent>();
        agentClone.gameObject.SetActive(true);
        agentClone.isStopped = false;
        agentClone.transform.position = transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPoint.transform.position, out hit, 10f, NavMesh.AllAreas))
        {
            agentClone.destination = hit.position;
        }
        Destroy(agentClone.gameObject, 1f);

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
