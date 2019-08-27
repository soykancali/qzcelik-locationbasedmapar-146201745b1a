using UnityEngine;
using System.Collections;

public class FixedRotation : MonoBehaviour {

	// Use this for initialization

    Quaternion init;
	void Start () {
        init = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void LateUpdate()
    {
        transform.rotation = init;

    }
}
