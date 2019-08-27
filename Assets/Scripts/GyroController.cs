using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroController : MonoBehaviour
{
    private bool gyroEnabled;
    private Gyroscope gyro;
    public GameObject cameraContainer;
    private Quaternion rot;
    Vector3 gyroscope;
    
    private void Start()
    {
         
        cameraContainer = new GameObject("CameraContainer");
        cameraContainer.transform.position = Vector3.zero;

        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        CameraController.instance.cameraMain = cameraContainer;


        //cameraCursor = GameObject.Find("targetCursor").GetComponent<FollowCameraCursor>();
        //cameraCursor.setCamera(cameraContainer.transform.GetChild(0).GetChild(0).gameObject.transform);
        gyroEnabled = EnableGyro();

    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;


            cameraContainer.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            rot = new Quaternion(-1, 0, 0, 0);

            return true;
        }
        return false;
    }
    private void Update()
    {
        if (gyroEnabled)
        {
            transform.localRotation = gyro.attitude * rot;
        }
    }
}