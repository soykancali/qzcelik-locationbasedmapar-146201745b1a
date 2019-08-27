using UnityEngine;
using System.Collections;

public class LatLonH
{
    const float EQUATORIAL_R = 6378137.0f;
    const float POLAR_R = 6356752.3f;
    const float E_KARE = 6.69437999f / (1000.0f);
    const float E_PRIME_KARE = 6.73949674228f / (1000.0f);


    public LatLonH()
    {
        longitude = 0.0f;
        latitude = 0.0f;
        altitude = 0.0f;
    }
    public LatLonH(float lon, float lat, float alt)
    {
        longitude = lon;
        latitude = lat;
        altitude = alt;
    }

    float longitude;
    float latitude;
    float altitude;

    public float getLongitude() { return longitude; }
    public float getLatitude() { return latitude; }
    public float getAltitude() { return altitude; }

    public void setLongitude(float t) { longitude = t; }
    public void setLatitude(float t) { latitude = t; }
    public void setAltitude(float t) { altitude = t; }



    public Vector toWorldCoord()
    {
        Vector t = new Vector();

        float sinLat = Mathf.Sin(Mathf.Deg2Rad * latitude);
        float cosLat = Mathf.Cos(Mathf.Deg2Rad * latitude);
        float sinLon = Mathf.Sin(Mathf.Deg2Rad * longitude);
        float cosLon = Mathf.Cos(Mathf.Deg2Rad * longitude);

        float chi = Mathf.Sqrt(1.0f - E_KARE * sinLat * sinLat);
        float r = (EQUATORIAL_R / chi) + altitude;

        t.setX(r * cosLat * cosLon);
        t.setY(r * cosLat * sinLon);
        t.setZ((r - EQUATORIAL_R * E_KARE / chi) * sinLat);

        return t;
    }

    public void fromWorldCoord(Vector wc)
    {
        float ratio = wc.getY() / wc.getX();
        longitude = Mathf.Rad2Deg * Mathf.Atan(ratio);

        float p = Mathf.Sqrt(1.0f + ratio * ratio) * wc.getX();
        float theta = Mathf.Atan(wc.getZ() * EQUATORIAL_R / (p * POLAR_R));
        float sinT = Mathf.Sin(theta);
        float cosT = Mathf.Cos(theta);

        float latitudeRad = Mathf.Atan( (wc.getZ() + E_PRIME_KARE*POLAR_R*sinT*sinT*sinT) / (p - E_KARE*EQUATORIAL_R*cosT*cosT*cosT) );
        altitude = ( p / Mathf.Cos(latitudeRad) - EQUATORIAL_R / Mathf.Sqrt(1.0f - E_KARE*Mathf.Sin(latitudeRad) * Mathf.Sin(latitudeRad)) );
        latitude = Mathf.Rad2Deg * latitudeRad;
    }

    public Vector getLocalNorth()
    {
        Vector t = new Vector();
        return t;
    }

    public Vector getLocalEast()
    {
        Vector t = new Vector();
        return t;
    }

    public Vector getLocalUp()
    {
        Vector t = new Vector();
        return t;
    }
}
