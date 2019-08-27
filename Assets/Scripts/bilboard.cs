using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class bilboard : MonoBehaviour {

    public Transform target;

    public Text lookPosition;

    void Update () 
    {
        //Symbols always look towards the User
        Vector3 relativePos = target.position - transform.position;
        relativePos = relativePos * -1;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = rotation;
    }
}
