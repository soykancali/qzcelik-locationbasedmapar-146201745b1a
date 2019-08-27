using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraCursor : MonoBehaviour {

   

    private void Update()
    {
        transform.position = Camera.main.transform.position;
        transform.eulerAngles = Camera.main.transform.eulerAngles;
    }

   

}
