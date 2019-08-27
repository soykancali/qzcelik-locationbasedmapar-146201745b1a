using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gyro : MonoBehaviour
{
    public Text gyroValue;

    public float distance = 5f;
    public Vector3 offset = Vector3.zero;
    public bool useAngleX = true;
    public bool useAngleY = true;
    public bool useAngleZ = true;
    public bool useRealHorizontalAngle = true;
    public bool resetViewOnTouch = true;

    private Quaternion initialRotation = Quaternion.identity;
    private Quaternion currentRotation;
    private static Vector3 gyroAngles; // original angles from gyro
    private static Vector3 usedAngles; // converted into unity world coordinates

    private int userSleepTimeOut; // original device SleepTimeOut setting
    private bool gyroAvail = false;

    void Awake()
    {
        Input.compensateSensors = true;
        Input.gyro.enabled = true;

    }

    void Start()
    {
       
    }
    void asdasd()
    {   
        if (gyroAvail == false)
        {
            if (Input.gyro.attitude.eulerAngles != Vector3.zero && Time.frameCount > 30)
            {
                gyroAvail = true;
                initialRotation = Quaternion.Euler(new Vector3(0, 0, 0)); //Input.gyro.attitude;
            }
            return; // early out
        }

        // reset origin on touch or not yet set origin
        if (resetViewOnTouch && (Input.touchCount > 0))
            initialRotation = Quaternion.Euler(new Vector3(0, 0, 0)); //Input.gyro.attitude;
        
        // new rotation
        currentRotation = Quaternion.Inverse(initialRotation) * Input.gyro.attitude;

        gyroAngles = currentRotation.eulerAngles;
        
        //usedAngles = Quaternion.Inverse (currentRotation).eulerAngles;
        usedAngles = gyroAngles;

        // reset single angle values
        if (useAngleX == false)
            usedAngles.x = 0f;
        if (useAngleY == false)
            usedAngles.y = 0f;
        if (useAngleZ == false)
            usedAngles.z = 0f;

        if (useRealHorizontalAngle)
            usedAngles.y *= -1;

        

        Camera.main.transform.localRotation = Quaternion.Lerp(Camera.main.transform.localRotation, Quaternion.Euler(new Vector3(-usedAngles.x, usedAngles.y, usedAngles.z)), 0.1f);
        


        //transform.localRotation = Quaternion.Euler(new Vector3(-usedAngles.x, usedAngles.y, usedAngles.z));
        //gyroValue.text = "Gyro Values: " + Input.gyro.attitude.eulerAngles + " ARCamera Values: " + transform.rotation.eulerAngles;
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

    }

    public static Vector3 GetUsedAngles()
    {
        return usedAngles;
    }

    public static Vector3 GetGyroAngles()
    {
        return gyroAngles;
    }

    public void ResetView()
    {
        initialRotation = Input.gyro.attitude;
    }

    void OnEnable()
    {
        // sensor on
        Input.gyro.enabled = true;
        initialRotation = Quaternion.identity;
        gyroAvail = false;

        // store device sleep timeout setting
        userSleepTimeOut = Screen.sleepTimeout;
        // disable sleep timeout when app is running
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void OnDisable()
    {
        // restore original sleep timeout
        Screen.sleepTimeout = userSleepTimeOut;
        //sensor off
        Input.gyro.enabled = false;
    }
}