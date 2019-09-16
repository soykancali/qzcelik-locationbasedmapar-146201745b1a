using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class SymbolManager : MonoBehaviour
{

    public static SymbolManager instance;

    LocationController locationController;
    CameraController cameraController;
    DistanceController distanceController;
    UIController uiController;

    bool isSymbolCreated=false;
    public void Awake()
    {
        instance = this;
    }
    //*******************************

    public GameObject contentObject;
    public GameObject baseSymbolObject;
    public GameObject baseModelObject;


    //*******************************

    public Texture barrier90;
    public Texture barrier270;

    public Texture ditch90;
    public Texture ditch270;

    public Texture office_building90;
    public Texture office_building270;

    public Texture car90;
    public Texture car270;

    public Texture male90;
    public Texture male270;

    public Texture hunting90;
    public Texture hunting270;

    public Texture bomb90;
    public Texture bomb270;

    public Texture blast90;
    public Texture blast270;

    public Texture fire90;
    public Texture fire270;

    public Texture police90;
    public Texture police270;

    public Texture ambulance90;
    public Texture ambulance270;

    public Texture firstaid90;
    public Texture firstaid270;

    public Texture user90;
    public Texture user270;

    public Texture traffic_light90;
    public Texture traffic_light270;
    //*******************************

    public GameObject distanceTextObj;

    public GameObject selectedLocationBase;
    private GameObject objTemp;

    //-----------------------
    public bool trafficLightDetectionStatus = false;
    private bool gpsStaticActive = false;



    private int symbolCounter = 0;
    //Dictionary<key,value>
    public Dictionary<string, GameObject> symbols = new Dictionary<string, GameObject>();
    public Dictionary<GameObject, LatLonH> locations = new Dictionary<GameObject, LatLonH>();
    public Dictionary<int, string> indexies = new Dictionary<int, string>();

    private int refreshCounter = 0;
    [System.Serializable]
    public struct LoadInfo
    {
        public string name;
        public LatLonH location;
        public string category;
        public LoadInfo(string n, LatLonH l, string cat)
        {
            name = n;
            location = l;
            category = cat;
        }
    }

    public LoadInfo loadInfoLocal;

    public List<LoadInfo> loadInfoList = new List<LoadInfo>();
    //public List<LoadInfo> loadInfoListLocated = new List<LoadInfo>();


    float lonMult = 111000.0f;
    float latMult = 85000.0f;
    float altMult = 1.0f;


    public void AddSymbol(string name, float lon, float lat, float alt, string category)
    {
        loadInfoList.Add(new LoadInfo(name, new LatLonH(lon, lat, alt), category));
    }

    void Start()
    {
        locationController = LocationController.instance;
        cameraController = CameraController.instance;
        distanceController = DistanceController.instance;
        uiController = UIController.instance;


        selectedLocationBase.SetActive(false);
        objTemp = new GameObject();


        InvokeRepeating("SymbolCreator", 2, 5);
        InvokeRepeating("AttachDistanceInfo", 3, 5);

    }


    //************************************************************************************************************
    //SYMBOL SELECTION WITH RAYCAST
    void Update()
    {
        //If specific symbol is touched
        //if (Input.touchCount > 0)
        if (Input.GetMouseButton(0))
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitPoint;

            if (Physics.Raycast(ray, out hitPoint) && symbols.ContainsValue(hitPoint.transform.gameObject))
            {
                string lat = locations[hitPoint.transform.gameObject].getLatitude().ToString();
                string lon = locations[hitPoint.transform.gameObject].getLongitude().ToString();
                string alt = locations[hitPoint.transform.gameObject].getAltitude().ToString();

                selectedLocationBase.SetActive(true);

                //Display selected symbol information
                uiController.SetSymbolLocationInfo(hitPoint.transform.gameObject.name, lat, lon, alt);

                //Bu kısımı yeniden düzenle ve uiController a taşı.
                /*
                if (hitPoint.transform.gameObject.tag.Contains("TOP"))
                {
                    receivedData.text = receivedData.text + "\n" + "Atış Mesafesi: 21.7 Km";
                }
                */
                 
                //Activate selected symbol object border
                for (int i = 0; i < indexies.Count; i++)
                {
                    string tempID = indexies[i];
                    if (symbols[tempID] != hitPoint.transform.gameObject)
                    {
                        symbols[tempID].transform.Find("border").GetComponent<Renderer>().enabled = false;
                    }
                    else
                    {
                        symbols[tempID].transform.Find("border").GetComponent<Renderer>().enabled = true;
                    }
                }
            }
            else
            {
                selectedLocationBase.SetActive(false);

                for (int i = 0; i < indexies.Count; i++)
                {
                    string tempID = indexies[i];
                    symbols[tempID].transform.Find("border").GetComponent<Renderer>().enabled = false;
                }
            }

            isSymbolCreated = true;
        }
    }
    //************************************************************************************************************
    //SYMBOL CREATION IN THEIR SPECIFIC LOCATIONS
    private void SymbolCreator()
    {
        while (isSymbolCreated)
        {
            //Get the symbol package and assign to 'loadInfo'
            LoadInfo loadInfo = loadInfoList[0];

            //If a symbol with same unique name gets more then once from server (package duplicate problem)
            if (symbols.ContainsKey(loadInfo.name))
            {
                uiController.WarningMessage("package duplication");
            }
            //If a symbol with unique name gets first time (Package is obtaining) 
            else
            {       //Create new object, enable its renderer

                GameObject cubeNew = null;

                if (!loadInfo.category.Contains("TOP"))
                {
                    cubeNew = Instantiate(baseSymbolObject) as GameObject;
                    cubeNew.GetComponent<Renderer>().enabled = true;
                    cubeNew.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                }
                else
                {
                    cubeNew = Instantiate(baseModelObject) as GameObject;
                }

                cubeNew.transform.parent = contentObject.transform;

                //Store the symbol information into the seperate dictionaries
                indexies.Add(symbolCounter, loadInfo.name);
                symbolCounter++;

                symbols.Add(loadInfo.name, cubeNew);
                locations.Add(cubeNew, loadInfo.location);

                //Locate the new Symbol to its specific location and then set its tag
                locationController.Locate(cubeNew, loadInfo.location);

                cubeNew.SetActive(true);
                cubeNew.tag = loadInfo.category;
                cubeNew.name = loadInfo.name;
                cubeNew.transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = loadInfo.name;
                cubeNew.transform.localEulerAngles = new Vector3(-90, 0, 0);

                uiController.WarningMessage("connected");


                //Set Symbol Texture
                SymbolTextureSetter(loadInfo.category, cubeNew);

                //Adjust symbol scale in terms of its distance
                float symbolDistance = Vector3.Distance(Camera.main.transform.position, cubeNew.transform.position);
                int symbolScaleDistanceRatio = 9;

                if (symbolDistance >= 0 && symbolDistance <= 100)
                    symbolScaleDistanceRatio = 30;
                else if (symbolDistance > 100 && symbolDistance <= 150)
                    symbolScaleDistanceRatio = 33;
                else if (symbolDistance > 150 && symbolDistance <= 200)
                    symbolScaleDistanceRatio = 36;
                else if (symbolDistance > 200 && symbolDistance <= 300)
                    symbolScaleDistanceRatio = 39;
                else if (symbolDistance > 300 && symbolDistance <= 400)
                    symbolScaleDistanceRatio = 42;
                else if (symbolDistance > 400 && symbolDistance <= 500)
                    symbolScaleDistanceRatio = 45;
                else if (symbolDistance > 500)
                    symbolScaleDistanceRatio = 48;

                float properScaleValue = symbolDistance / symbolScaleDistanceRatio;

                cubeNew.transform.localScale = new Vector3(properScaleValue, properScaleValue, 1);

                /*
                if(!cubeNew.tag.Contains("TOP"))
                    cubeNew.transform.localScale = new Vector3(properScaleValue, properScaleValue, 1);
                else
                    cubeNew.transform.localScale = new Vector3(properScaleValue, properScaleValue, properScaleValue);
                    */
            }
            loadInfoList.RemoveAt(0);
        }
    }


    //************************************************************************************************************
    //Attach the distance information to the symbols in meter(user to symbols distance)
    public void AttachDistanceInfo()
    {
        for (int i = 0; i < indexies.Count; i++)
        {
            float distanceToSymbol = 0;
            string symbolID = indexies[i];
            GameObject obj = symbols[symbolID];

            float latSymbol = locations[obj].getLatitude();
            float lonSymbol = locations[obj].getLongitude();


            //Get the distance (User to Symbol)
            //Debug.Log("cam lat: " + cameraController.cameraLocation.getLatitude() + "lat symbol: " + latSymbol);
            distanceToSymbol =
                distanceController.GetDistanceFromLatLonInMeter(cameraController.cameraLocation.getLatitude(), cameraController.cameraLocation.getLongitude(), latSymbol, lonSymbol);


            //In terms of Symbol Object Rotation active proper Distance holder
            //if (obj.transform.localEulerAngles.y > 180.0f)
            //{
            symbols[symbolID].transform.GetChild(0).GetComponent<Renderer>().enabled = true;
            symbols[symbolID].transform.GetChild(1).GetComponent<Renderer>().enabled = false;

            if (symbols[symbolID].transform.GetChild(0) != null)
            {
                if (distanceToSymbol < 1000.0f)
                {
                    float inMeter = distanceToSymbol;

                    symbols[symbolID].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = inMeter.ToString("F1") + "metre";
                }
                else
                {
                    float inKm = distanceToSymbol / 1000;
                    
                    symbols[symbolID].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = inKm.ToString("F2") +"km";
                    //symbols[symbolID].transform.GetChild(0).GetComponent<TextMeshPro>().text = inKm.ToString("F2") + "km";
                }
            }
            //}
            /*if (obj.transform.localEulerAngles.y < 180.0f)
            //{
                symbols[symbolID].transform.GetChild(1).GetComponent<Renderer>().enabled = true;
                symbols[symbolID].transform.GetChild(0).GetComponent<Renderer>().enabled = false;

                if(symbols[symbolID].transform.GetChild(1) != null)
                {
                    if (distanceToSymbol < 1000.0f)
                    {
                        float inMeter = distanceToSymbol;
                        symbols[symbolID].transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().text = inMeter.ToString("F1") + "\n" + "metre";
                    }
                    else
                    {
                        float inKm = distanceToSymbol / 1000;
                        symbols[symbolID].transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().text = inKm.ToString("F2") + "\n" + "km";
                    }
                }
            }*/

            //***Refresh the symbol texture rotations for correct orientation***
            SymbolTextureSetter(obj.transform.tag, obj);
        }
    }
    
    
    //************************************************************************************************************
    //Set Symbol Textures in terms of their Category
    void SymbolTextureSetter(string category, GameObject cubeNew)
    {
        if (category == "Ilkyardim")
        {
            if (cubeNew.transform.localEulerAngles.y < 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = firstaid90;
            if (cubeNew.transform.localEulerAngles.y > 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = firstaid270;
        }
        else if (category == "Engel")
        {
            if (cubeNew.transform.localEulerAngles.y < 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = barrier90;
            if (cubeNew.transform.localEulerAngles.y > 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = barrier270;
        }
        else if (category == "Bina")
        {
            if (cubeNew.transform.localEulerAngles.y < 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = office_building90;
            if (cubeNew.transform.localEulerAngles.y > 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = office_building270;
        }
        //------------------------------------
        else if (category == "Rasathane")
        {
            if (cubeNew.transform.localEulerAngles.y < 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = male90;
            if (cubeNew.transform.localEulerAngles.y > 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = male270;
        }

        else if (category == "Askeriye")
        {
            if (cubeNew.transform.localEulerAngles.y < 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = hunting90;
            if (cubeNew.transform.localEulerAngles.y > 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = hunting270;
        }

        else if (category == "Trafik Lambasi")
        {
            if (cubeNew.transform.localEulerAngles.y < 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = traffic_light90;
            if (cubeNew.transform.localEulerAngles.y > 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = traffic_light270;
        }
        else if (category == "Ada")
        {
            if (cubeNew.transform.localEulerAngles.y < 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = ditch90;
            if (cubeNew.transform.localEulerAngles.y > 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = ditch270;
        }
        /*
        else if (category == "Firtina Obus TOP")
        {
            if (cubeNew.transform.localEulerAngles.y < 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = blast90;
            if (cubeNew.transform.localEulerAngles.y > 180.0f)
                cubeNew.GetComponent<Renderer>().material.mainTexture = blast270;
        }
        */
    }
}
