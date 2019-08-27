using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CompassController : MonoBehaviour {

	// Use this for initialization

    public Image compass;
    public Text compassAcc;

	void Start () {
        Input.compass.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.compass.headingAccuracy != 0)
        {
            compassAcc.text = "" + Input.compass.headingAccuracy;
        }
        else
        {
            compassAcc.text = "Not Avaliable";
        }
        if (Input.compass.headingAccuracy < 0)
        {
            compassAcc.text = "Unreliable";
        }
        compass.gameObject.transform.rotation = Quaternion.Euler(0, 0, Input.compass.trueHeading);
        
	
	}
}
