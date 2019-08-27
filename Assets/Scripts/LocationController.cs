using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationController : MonoBehaviour {

    public static LocationController instance;

    public string locationStatus;

    public string longitudeValue = "No Info.";
    public string latitudeValue = "No Info.";
    public string altitudeValue = "No Info.";
    public string accuracyValue = "No Info.";
    public string timeStampValue = "No Info.";


    public bool isLocationServiceActive = false;
    public int maxWait = 30;


    float lonMult = 1110.0f;
    float latMult = 850.0f;
    float altMult = 1.0f;

    [System.Serializable]
    public class Location
    {
        public float longitude { get; set; }
        public float latitude { get; set; }
        public float altitude { get; set; }

        public float horizontalAcc { get; set; }
        public double timeStamp { get; set; }
    }
    [SerializeField]
    public Location currentLocation;


    public void Locate(GameObject obj, LatLonH loc)
    {
        // decrease the numbers to prevent floating precision limit error

        float latdif = (loc.getLatitude() - currentLocation.latitude) * 100;
        float londif = (loc.getLongitude() - currentLocation.longitude) * 100;
        float altdif = (loc.getAltitude() - currentLocation.altitude) * 1;

        Vector diff = new Vector(londif, altdif, latdif);
        obj.transform.position = diff.toVector3();
        //Debug.Log("Object Name: " + obj.name + "Pos: " + obj.transform.position);
    }

    //*********************************************************************************************
    //*********************************************************************************************
    IEnumerator Start()
    {
        //Check whether is in editor
        if(Application.isEditor)
        {
            isLocationServiceActive = false;
            locationStatus = "Editor Mode.";
            yield break;
        }
        // Check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            isLocationServiceActive = false;
            locationStatus = "Device Location Service is Inactive.";
            CancelInvoke("LocationUpdater");

            yield break;
        }

        // Start service before querying location
        locationStatus = "Location Service is Started.";
        Input.location.Start(5, 5);

        // Wait until service initializes
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 30 seconds
        if (maxWait < 1)
        {
            isLocationServiceActive = false;
            locationStatus = "Location Service TimeOut!";

            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            isLocationServiceActive = false;
            locationStatus = "Location Service Failed!";
            CancelInvoke("LocationUpdater");

            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            InvokeRepeating("LocationUpdater", 0, 10);
        }

        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
    }
    //*********************************************************************************************
    //*********************************************************************************************
    void LocationUpdater()
    {
        isLocationServiceActive = true;
        locationStatus = "Location Service Active..";

        currentLocation.latitude = Input.location.lastData.latitude;
        currentLocation.longitude = Input.location.lastData.longitude;
        currentLocation.altitude = Input.location.lastData.altitude;

        currentLocation.horizontalAcc = Input.location.lastData.horizontalAccuracy;
        currentLocation.timeStamp = Input.location.lastData.timestamp;

        latitudeValue = "" + Input.location.lastData.latitude;
        longitudeValue = "" + Input.location.lastData.longitude;
        altitudeValue = "" + Input.location.lastData.altitude;

        accuracyValue = "" + Input.location.lastData.latitude;
        timeStampValue = "" + Input.location.lastData.latitude;
    }


    public void Awake()
    {
        instance = this;
    }
}
