using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    SymbolManager symbolController;
    LocationController locationController;
    UIController uiController;

    public RawImage cameraRender;

    public LatLonH cameraLocation = new LatLonH();

    public GameObject cameraMain;

    public void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        symbolController = SymbolManager.instance;
        locationController = LocationController.instance;
        uiController = UIController.instance;

        /*
        WebCamTexture webcamTexture = new WebCamTexture();
        cameraRender.texture = webcamTexture;
        cameraRender.material.mainTexture = webcamTexture;
        webcamTexture.Play();
        */
        /*
        locationController.currentLocation.latitude = 39.913399f;
        locationController.currentLocation.longitude = 32.834782f;
        locationController.currentLocation.altitude = 0.0f;
        */

        InvokeRepeating("LocateUserLocation", 0, 5);
    }
    
    void LocateUserLocation()
    {
        //Failed to get Location Information!
        if (!locationController.isLocationServiceActive)
        {
            //In editor mode, set default long, lat and alt values
            //Savunma Sanayi Müsteşarlığı
         
            cameraLocation.setLongitude(SymbolController.Instance.userStaticLon);
            cameraLocation.setLatitude(SymbolController.Instance.userStaticLat);
            cameraLocation.setAltitude(0.0f);
            
            locationController.Locate(cameraMain, cameraLocation);

            uiController.SetUserLocationInfo("" + SymbolController.Instance.userStaticLat, "" + SymbolController.Instance.userStaticLon, "" + 0.0f, false);
        }
        //Location Info is Getting..
        else
        {
            cameraLocation.setLatitude(locationController.currentLocation.latitude);
            cameraLocation.setLongitude(locationController.currentLocation.longitude);
            cameraLocation.setAltitude(locationController.currentLocation.altitude);

            locationController.Locate(cameraMain, cameraLocation);

            //Set user location info to UI
            uiController.SetUserLocationInfo(locationController.latitudeValue, locationController.longitudeValue, locationController.altitudeValue);
        }
    }
}
