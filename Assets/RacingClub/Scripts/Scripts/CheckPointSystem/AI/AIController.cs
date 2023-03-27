using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    public Material brakeLights;

    //other classes ->
    //private carModifier modifier;
    //private inputManager IM;
    [HideInInspector] public bool test; //engine sound boolean

    [Header("Variables")]
    public float finalDrive = 3.4f;
    public float brakes = 1;
    public float maxRPM, minRPM;
    public float[] gears;
    public AnimationCurve enginePower;

    [HideInInspector] public int gearNum = 1;
    [HideInInspector] public bool playPauseSmoke = false, hasFinished;
    [HideInInspector] public float KPH;
    [HideInInspector] public float engineRPM;
    [HideInInspector] public bool reverse = false;
    [HideInInspector] public float nitrusValue, totalPower;
    [HideInInspector] public bool nitrusFlag = false, drifting = false;
    [HideInInspector] public float[] wheelSlip;


    [HideInInspector] public WheelCollider[] wheels = new WheelCollider[4];
    private GameObject centerOfMass;
    private Rigidbody rigidbody;
    //public finishTrigger triggerScript;

    [HideInInspector]
    public float engineLerpValue, startStifness, radius = 4, steerSmooth, gearChangeRate, brakPower = 0,
     wheelsRPM, driftFactor, lastValue, acceleration, velocity = 0, downforce;

    [HideInInspector] public bool flag = false, lightsFlag, grounded, engineLerp = false;
    private float startForwardFriction, startSidewaysFriction;

    [HideInInspector] public string carName;
    [HideInInspector] public int carPrice;

    private WheelFrictionCurve forwardFriction, sidewaysFriction;

    public float horizontal, vertical;

    private void Start()
    {
        //getObjects();
    }

    private void FixedUpdate()
    {
        steerVehicle();
        calculateEnginePower();
        activateLights();
        animateWheels();
        runAudio();
        //checkDistance();
        VerticalHandler();
        sensorSystem();
    }

    private void activateLights()
    {
        if (vertical < 0 || KPH <= 1) turnLightsOn();
        else turnLightsOff();
    }

    private void turnLightsOn()
    {
        if (lightsFlag) return;
        brakeLights.SetColor("_EmissionColor", new Color(255f, 35f, 35f) * 0.015f);
        lightsFlag = true;
    }

    private void turnLightsOff()
    {
        if (!lightsFlag) return;
        brakeLights.SetColor("_EmissionColor", Color.black);
        lightsFlag = false;
    }

    private void calculateEnginePower()
    {
        lerpEngine();
        wheelRPM();

        acceleration = vertical > 0 ? vertical : wheelsRPM <= 1 ? vertical : 0;

        if (engineRPM >= maxRPM)
        {
            setEngineLerp(maxRPM - 1000);
        }
        if (!engineLerp)
        {
            engineRPM = Mathf.Lerp(engineRPM, 1000 + (Mathf.Abs(wheelsRPM) * finalDrive * (gears[gearNum])), 5 * Time.deltaTime);
            totalPower = (enginePower.Evaluate(engineRPM) * finalDrive) * acceleration;
        }


        moveVehicle();
        shifter();
    }

    private void VerticalHandler()
    {
        WheelHit hit;
        for (int i = 0; i < wheels.Length; i++)
        {

            if (wheels[i].GetGroundHit(out hit))
                wheelSlip[i] = Mathf.Abs(hit.forwardSlip / 2) + Mathf.Abs(hit.sidewaysSlip);

            vertical = Mathf.Abs(hitDistance - wheelSlip[i] * 2);

        }
        steerHelper();
    }

    private float localSteerHelper, _steerHelper = 1, oldRotation;

    void steerHelper()
    {
        localSteerHelper = Mathf.SmoothStep(localSteerHelper, _steerHelper * Mathf.Abs(horizontal), 0.1f);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _steerHelper *= -1;
        }

        foreach (WheelCollider wc in wheels)
        {
            WheelHit wheelHit;
            wc.GetGroundHit(out wheelHit);
            if (wheelHit.normal == Vector3.zero)
                return;
        }

        if (Mathf.Abs(oldRotation - transform.eulerAngles.y) < 10)
        {
            float turnAdjust = (transform.eulerAngles.y - oldRotation) * _steerHelper;
            Quaternion velRotation = Quaternion.AngleAxis(turnAdjust, Vector3.up);
            rigidbody.velocity = velRotation * rigidbody.velocity;
        }

        oldRotation = transform.eulerAngles.y;
    }

    private void wheelRPM()
    {
        float sum = 0;
        int R = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += wheels[i].rpm;
            R++;
        }
        wheelsRPM = (R != 0) ? sum / R : 0;

        if (wheelsRPM < 0 && !reverse)
        {
            reverse = true;
        }
        else if (wheelsRPM > 0 && reverse)
        {
            reverse = false;
        }
    }

    private void shifter()
    {

        if (!isGrounded()) return;
        if (engineRPM > maxRPM && gearNum < gears.Length - 1 && !reverse && Time.time >= gearChangeRate && KPH > 55)
        {
            gearNum++;
            gearChangeRate = Time.time + 1f / 2f;
        }
        if (engineRPM < minRPM && gearNum > 0)
        {
            gearNum--;
        }

    }

    public bool isGrounded()
    {
        if (wheels[0].isGrounded && wheels[1].isGrounded && wheels[2].isGrounded && wheels[3].isGrounded)
            return true;
        else
            return false;
    }

    private void moveVehicle()
    {

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].motorTorque = totalPower / wheels.Length;
        }

        brakPower = (vertical == -1 && wheelsRPM > 1) ? brakes : 0;

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].brakeTorque = brakPower;
        }

        if (hasFinished) return;

        rigidbody.angularDrag = KPH / 70;

        KPH = rigidbody.velocity.magnitude * 3.6f;

    }

    private void steerVehicle()
    {

        Vector3 relativeVector = transform.InverseTransformPoint(currentNode.nextWaypoint.transform.position);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude) * 2;
        horizontal = Mathf.SmoothDamp(horizontal, newSteer, ref velocity, Time.deltaTime * 2);


        if (horizontal > 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
        }
        else if (horizontal < 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
        }
        else
        {
            wheels[0].steerAngle = 0;
            wheels[1].steerAngle = 0;
        }

    }

    /*private void getObjects()
    {

        IM = GetComponent<inputManager>();
        modifier = GetComponent<carModifier>();
        rigidbody = GetComponent<Rigidbody>();
        wheelSlip = new float[wheels.Length];
        centerOfMass = gameObject.transform.Find("mass").gameObject;
        rigidbody.centerOfMass = centerOfMass.transform.localPosition;
        triggerScript = transform.Find("finishTriggerObject").GetComponent<finishTrigger>();

    }*/

    private void setEngineLerp(float num)
    {
        engineLerp = true;
        engineLerpValue = num;
    }

    public void lerpEngine()
    {
        if (engineLerp)
        {
            totalPower = 0;
            engineRPM = Mathf.Lerp(engineRPM, engineLerpValue, 20 * Time.deltaTime);
            engineLerp = engineRPM <= engineLerpValue + 100 ? false : true;
        }
    }

    [Header("wheels")]
    private float wheelRadius = 0.36f;
    private float suspensionDistance = 0.1f;
    private float suspensionoffset = 0.03f;

    private GameObject wheelsFolder;
    private GameObject[] wheelz;

    private Vector3 wheelPosition;
    private Quaternion wheelRotation;

    void Awake()
    {
        wheelsFolder = gameObject.transform.Find("wheels").gameObject;

        wheelz = new GameObject[wheelsFolder.transform.childCount - 1];
        wheels = new WheelCollider[wheelz.Length];

        for (int i = 0; i < wheelz.Length; i++)
        {
            wheelz[i] = wheelsFolder.transform.GetChild(i + 1).gameObject;
        }

        GameObject wheelObject = wheelsFolder.transform.GetChild(0).gameObject;
        spawnWheelColliders();
        spawnWheels(wheelObject);

    }

    public void spawnWheels(GameObject wheel)
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            GameObject newWheel = Instantiate(wheel);
            newWheel.transform.parent = wheelz[i].transform;
            newWheel.transform.position = wheelz[i].transform.position;
            newWheel.transform.localScale = new Vector3(1, 1, 1);
            newWheel.transform.localRotation = new Quaternion(0, 0, 0, 0);
            if (i % 2 != 0)
                newWheel.transform.Rotate(new Vector3(0, 180, 0));
        }
        wheel.SetActive(false);
    }

    public void spawnWheelColliders()
    {

        GameObject wheelColliderFolder = new GameObject();
        wheelColliderFolder.transform.parent = gameObject.transform;
        wheelColliderFolder.name = "wheelCollidersFolder";

        for (int i = 0; i < wheels.Length; i++)
        {
            GameObject wheel = new GameObject();
            wheel.name = "wheel_" + i;
            wheel.transform.parent = wheelColliderFolder.transform;
            wheel.transform.position = wheelz[i].transform.position;
            setupWheelColldier(wheel, i);
        }

    }

    private void animateWheels()
    {

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelz[i].transform.rotation = wheelRotation;
            wheelz[i].transform.position = wheelPosition;

        }
    }

    public void setupWheelColldier(GameObject element, int i)
    {
        WheelCollider collider = element.AddComponent<WheelCollider>();
        wheels[i] = collider;

        collider.radius = wheelRadius;
        collider.suspensionDistance = suspensionDistance;
        collider.forceAppPointDistance = 0.05f;
        collider.center = new Vector3(0, suspensionoffset, 0);
    }

    [Header("Audio")]
    public AudioClip lowAccelClip;
    public AudioClip lowDecelClip;
    public AudioClip highAccelClip;
    public AudioClip highDecelClip;

    private float accFade = 0;

    [Range(0, 1)] public float pitchMultiplier = 1f;
    private float accelerationz;
    public float lowPitchMin = 1f;
    public float lowPitchMax = 6f;
    [Range(0, 1)] public float highPitchMultiplier = 0.25f;
    private float maxRolloffDistance = 500;
    private AudioSource m_LowAccel;
    private AudioSource m_LowDecel;
    private AudioSource m_HighAccel;
    private AudioSource m_HighDecel;
    private bool m_StartedSound;

    private void StartSound()
    {

        m_HighAccel = SetUpEngineAudioSource(highAccelClip);
        m_LowAccel = SetUpEngineAudioSource(lowAccelClip);
        m_LowDecel = SetUpEngineAudioSource(lowDecelClip);
        m_HighDecel = SetUpEngineAudioSource(highDecelClip);

        m_StartedSound = true;
    }

    private void StopSound()
    {
        foreach (var source in GetComponents<AudioSource>())
        {
            Destroy(source);
        }
        m_StartedSound = false;
    }

    private void runAudio()
    {

        float camDist = (Camera.main.transform.position - transform.position).sqrMagnitude;

        accFade = Mathf.Lerp(accFade, Mathf.Abs(accelerationz), 15 * Time.deltaTime);
        if (m_StartedSound && camDist > maxRolloffDistance * maxRolloffDistance)
        {
            StopSound();
        }
        if (!m_StartedSound && camDist < maxRolloffDistance * maxRolloffDistance)
        {
            StartSound();
        }
        if (m_StartedSound)
        {
            if (totalPower > 0 && !engineLerp)
                accelerationz = 1;
            else accelerationz = 0;

            float pitch = ULerp(lowPitchMin, lowPitchMax, engineRPM / maxRPM);
            pitch = Mathf.Min(lowPitchMax, pitch);
            m_LowAccel.pitch = pitch * pitchMultiplier;
            m_LowDecel.pitch = pitch * pitchMultiplier;
            m_HighAccel.pitch = pitch * highPitchMultiplier * pitchMultiplier;
            m_HighDecel.pitch = pitch * highPitchMultiplier * pitchMultiplier;

            float decFade = 1 - accFade;
            float highFade = Mathf.InverseLerp(0.2f, 0.8f, engineRPM / 10000);
            float lowFade = 1 - highFade;

            highFade = 1 - ((1 - highFade) * (1 - highFade));
            lowFade = 1 - ((1 - lowFade) * (1 - lowFade));
            decFade = 1 - ((1 - decFade) * (1 - decFade));
            m_LowAccel.volume = lowFade * accFade;
            m_LowDecel.volume = lowFade * decFade;
            m_HighAccel.volume = highFade * accFade;
            m_HighDecel.volume = highFade * decFade;

        }

    }

    private AudioSource SetUpEngineAudioSource(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0;
        source.spatialBlend = 1;
        source.loop = true;
        source.dopplerLevel = 0;
        source.time = Random.Range(0f, clip.length);
        source.Play();
        source.minDistance = 5;
        source.maxDistance = maxRolloffDistance;
        return source;
    }

    private float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }

    [Header("AI")]
    public float distance;
    public Node currentNode;

    public GameObject FrontSensor;

    public float distanceThreshold, hitDistance = 1;

    private Vector3 Destination, lastPosition;

    /*void checkDistance()
    {
        try
        {
            if (Vector3.Distance(transform.position, currentNode.transform.position) <= distance)
                reachedDestination();
        }
        catch { }
    }

   /* private void reachedDestination()
    {
        if (currentNode.nextWaypoint == null)
        {
            currentNode = currentNode.previousWaypont;
            return;
        }
        if (currentNode.previousWaypont == null)
        {
            currentNode = currentNode.nextWaypoint;
            return;
        }
        else
            currentNode = currentNode.nextWaypoint;
    }*/

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (currentNode != null)
            Gizmos.DrawSphere(currentNode.transform.position, 0.5f);
    }

    public void sensorSystem()
    {
        RaycastHit hit;
        if (Physics.Raycast(FrontSensor.transform.position, FrontSensor.transform.forward, out hit, distanceThreshold))
        {
            if (hit.collider.tag == "AiDriver")
            {
                Debug.DrawLine(FrontSensor.transform.position, hit.point);
                hitDistance = hit.distance / distanceThreshold;
            }
            else
            {
                hitDistance = 1;
            }

        }

    }

}