using UnityEngine;
using System.Collections;

public class Vector  {

    public Vector(float tx, float ty, float tz)
    {
        x = tx;
        y = ty;
        z = tz;
    }

    public Vector()
    {
        x = 0;
        y = 0;
        z = 0;
    }

    float x;
    float y;
    float z;

    public float getX() { return x; }
    public float getY() { return y; }
    public float getZ() { return z; }

    public void setX(float tx) { x = tx; }
    public void setY(float ty) { y = ty; }
    public void setZ(float tz) { z = tz; }

    public Vector3 toVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector diffFrom(Vector vec)
    {
        return new Vector(vec.getX() - x, vec.getY() - y, vec.getZ() - z);
    }
}
