using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour {

    public static UIController instance;

    public GameObject lonTxt;
    public GameObject latTxt;
    public GameObject altTxt;

    public Text accTxt;
    public Text timeStampTxt;


    public Text locServiceTxt;
    public Text serverStatusTxt;
    public Text gpsReceiveTxt;

    public Text receivedData;

    public Text symbolSize;

    public void Awake()
    {
        instance = this;
    }

    public void SetUserLocationInfo(string lat, string lon, string alt, bool isDynamicLocation = true, string accuracy = "", string timeStamp = "")
    {
        if(isDynamicLocation)
        {
            latTxt.transform.GetComponent<TextMeshProUGUI>().text = "Enlem: " + lat;
            lonTxt.transform.GetComponent<TextMeshProUGUI>().text = "Boylam: " + lon;
            altTxt.transform.GetComponent<TextMeshProUGUI>().text = "Yükseklik: " + alt;

            //accTxt.text = "Konum Doğruluğu: " + accuracy;
            //timeStampTxt.text = "Zaman: " + timeStamp;
        }
        else
        {
            latTxt.transform.GetComponent<TextMeshProUGUI>().text = "(Statik)Enlem: " + lat;
            lonTxt.transform.GetComponent<TextMeshProUGUI>().text = "(Statik)Boylam: " + lon;
            altTxt.transform.GetComponent<TextMeshProUGUI>().text = "(Statik)Yükseklik: " + alt;

            //accTxt.text = "Konum Doğruluğu: " + "(Statik Mod)";
            //timeStampTxt.text = "Zaman: " + "(Statik Mod)";
        }
    }
    public void SetSymbolLocationInfo(string locationName, string lat, string lon, string alt, bool isDynamicLocation = true, string accuracy = "", string timeStamp = "")
    {
        receivedData.text = "Yer: " + locationName + "\n" +
                               "Enlem: " + lat + "\n" +
                               "Boylam: " + lon + "\n" +
                               "Yükseklik: " + alt;
    }
    public void WarningMessage(string _warning)
    {
        switch(_warning)
        {
            case "package duplication":
                serverStatusTxt.text = "Package Duplication is Detected!";
                break;
            case "connected":
                serverStatusTxt.text = "Connected.";
                break;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
