using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TemporaryServerController : MonoBehaviour
{
    public static TemporaryServerController Instance { get; private set; }

    //private string[] symbols = {"Arac", "Kisi", "Keskin Nisanci", "EYP", "Mayin", "Yangin", "Polis", "Ambulans", "IlkYardim", "user" };

    //Lat and Long
    //39.919640, 32.822550 Bahçelievler Anadolu Lisesi (kisi)
    //39.920315, 32.819503 Emek Cami (keskin nisanci)
    //39.916480, 32.847849 Genelkurmay (polis)
    //39.918270, 32.810506 AŞTİ (bina)
    //39.912987, 32.805506 TOBB Hastane (ambulans)
    //-----------------------------------------------------
    //37.259353, 42.455606 Silopi Devlet Hastanesi (TC)
    //37.151096, 42.567467 Habur Sınır Kapısı (TC)
    //37.518635, 42.457888 Şırnak Merkez (TC)
    //37.084547, 42.427976 Dayrabun Merkez (Irak)
    //37.146479, 42.687593 Zakho Merkez (Irak)
    //37.175935, 42.138727 Al-Malikiyah Merkez (Suriye)
    //37.570414, 44.284387 Hakkari Yüksekova (TC)
    //36.352700, 43.165541 Musul Merkez (Irak)

    public void Awake()
    {
        Instance = this;
    }
    SymbolManager symbolController;
    

    /*public Dictionary<string, float> generatedLatitude = new Dictionary<string, float>();
    public Dictionary<string, float> generatedLongitude = new Dictionary<string, float>();
    public Dictionary<string, string> generatedCategory = new Dictionary<string, string>();
    public Dictionary<int, string> indexies = new Dictionary<int, string>();*/


    public float userStaticLat;
    public float userStaticLon;

    public GameObject symbolManagerObj;
    private SymbolManager symbolManager;

    public NetworkListener netListener;

    private bool completed = false;


    public string[] _locations;
    public string[] _category;

    public float[] _latitudes;
    public float[] _longitudes;
    public float[] _altitudes;

    // Use this for initialization
    void Start()
    {
        symbolController = SymbolManager.instance;
        Invoke("symbolGenerator", 1);
    }

    IEnumerator PackageSender(string name, float longi, float lati, float alti, string category)
    {
        yield return new WaitForSeconds(5.0f);  //Wait 1 seconds
        //netListener.symbolManager.AddSymbol(name, longi, lati, alti, category);
        symbolController.AddSymbol(name, longi, lati, alti, category);
    }

    void symbolGenerator()
    {
        for (int i = 0; i < _locations.Length; i++)
        {
            StartCoroutine(PackageSender(_locations[i], _longitudes[i], _latitudes[i], _altitudes[0], _category[i]));
        }
    }
}

/*
    generatedLatitude.Add("hospital", 37.259353f);
    generatedLatitude.Add("observatory", 37.151096f);
    generatedLatitude.Add("millitary", 37.518635f);
    generatedLatitude.Add("mall", 37.084547f);
    generatedLatitude.Add("trafficLight1", 37.146479f);
    generatedLatitude.Add("trafficLight2", 37.175935f);
    generatedLatitude.Add("trafficLight3", 37.570414f);
    generatedLatitude.Add("trafficLight4", 36.352700f);

    //**************
    generatedLongitude.Add("court", 42.455606f);
    generatedLongitude.Add("observatory", 42.567467f);
    generatedLongitude.Add("millitary", 42.457888f);
    generatedLongitude.Add("mall", 42.427976f);
    generatedLongitude.Add("trafficLight1", 42.687593f);
    generatedLongitude.Add("trafficLight2", 42.138727f);
    generatedLongitude.Add("trafficLight3", 44.284387f);
    generatedLongitude.Add("trafficLight4", 43.165541f);
         
    //**************
    generatedCategory.Add("firstaid", "Silopi Devlet Hastanesi (TC)");
    generatedCategory.Add("police", "Habur Sınır Kapısı (TC)");
    generatedCategory.Add("office_building", "Şırnak Merkez (TC)");
    generatedCategory.Add("office_building", "Dayrabun Merkez (Irak)");
    generatedCategory.Add("office_building", "Zakho Merkez (Irak)");
    generatedCategory.Add("office_building", "Al-Malikiyah Merkez (Suriye)");
    generatedCategory.Add("office_building", "Hakkari Yüksekova Merkez (TC)");
    generatedCategory.Add("office_building", "Musul Merkez (Irak)");

    //**************
    indexies.Add(0, "Silopi Devlet Hastanesi (TC)");
    indexies.Add(1, "Habur Sınır Kapısı (TC)");
    indexies.Add(2, "Dayrabun Merkez (Irak)");
    indexies.Add(3, "Zakho Merkez (Irak)");
    indexies.Add(4, "Al-Malikiyah Merkez (Suriye)");
    indexies.Add(5, "Hakkari Yüksekova Merkez (TC)");
    indexies.Add(6, "Musul Merkez (Irak)");

    /*------------------------------------------------------------
    generatedLatitude.Add("court", 39.845011f);
    generatedLatitude.Add("observatory", 39.842542f);
    generatedLatitude.Add("millitary", 39.836256f);
    generatedLatitude.Add("mall", 39.8289f);
    generatedLatitude.Add("trafficLight1", 39.844339f);
    generatedLatitude.Add("trafficLight2", 39.829681f);
    generatedLatitude.Add("trafficLight3", 39.838889f);
    generatedLatitude.Add("trafficLight4", 39.836178f);
    generatedLatitude.Add("trafficLight5", 39.836117f);
    generatedLatitude.Add("trafficLight6", 39.83085f);
    generatedLatitude.Add("trafficLight7", 39.8447f);


    //**************
    generatedLongitude.Add("court", 32.790564f);
    generatedLongitude.Add("observatory", 32.778872f);
    generatedLongitude.Add("millitary", 32.760358f);
    generatedLongitude.Add("mall", 32.731536f);
    generatedLongitude.Add("trafficLight1", 32.782425f);
    generatedLongitude.Add("trafficLight2", 32.734128f);
    generatedLongitude.Add("trafficLight3", 32.7741f);
    generatedLongitude.Add("trafficLight4", 32.765139f);
    generatedLongitude.Add("trafficLight5", 32.761392f);
    generatedLongitude.Add("trafficLight6", 32.7479f);
    generatedLongitude.Add("trafficLight7", 32.7913f);
    //**************
    generatedCategory.Add("court", "Anayasa");
    generatedCategory.Add("observatory", "Rasathane");
    generatedCategory.Add("millitary", "Askeriye");
    generatedCategory.Add("mall", "Migros Market");
    generatedCategory.Add("trafficLight1", "Trafik Lambasi");
    generatedCategory.Add("trafficLight2", "Trafik Lambasi");
    generatedCategory.Add("trafficLight3", "Trafik Lambasi");
    generatedCategory.Add("trafficLight4", "Trafik Lambasi");
    generatedCategory.Add("trafficLight5", "Trafik Lambasi");
    generatedCategory.Add("trafficLight6", "Trafik Lambasi");
    generatedCategory.Add("trafficLight7", "Trafik Lambasi");
    //**************
    indexies.Add(0, "court");
    indexies.Add(1, "observatory");
    indexies.Add(2, "millitary");
    indexies.Add(3, "mall");
    indexies.Add(4, "trafficLight1");
    indexies.Add(5, "trafficLight2");
    indexies.Add(6, "trafficLight3");
    indexies.Add(7, "trafficLight4");
    indexies.Add(8, "trafficLight5");
    indexies.Add(9, "trafficLight6");
    indexies.Add(10, "trafficLight7");
    */

//* 
