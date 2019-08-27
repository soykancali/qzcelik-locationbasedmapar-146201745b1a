using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceController : MonoBehaviour {

    public static DistanceController instance;

    SymbolManager symbolController;
    LocationController locationController;

  
    //Calculate distance from LatLong in Meter
    public float GetDistanceFromLatLonInMeter(float lat1, float lon1, float lat2, float lon2)
    {
        int R = 6371; // Radius of the earth in km
        float dLat = Deg2rad(lat2 - lat1);  // deg2rad below
        float dLon = Deg2rad(lon2 - lon1);
        float a =
            Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
            Mathf.Cos(Deg2rad(lat1)) * Mathf.Cos(Deg2rad(lat2)) *
            Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);

        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        float d = R * c; // Distance in km
        float distInMeter = d * 1000; //Distance in meter
        return distInMeter;
    }
    float Deg2rad(float deg)
    {
        return deg * (Mathf.PI / 180);
    }



    public void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start () {
        symbolController = SymbolManager.instance;
        locationController = LocationController.instance;
    }
}
