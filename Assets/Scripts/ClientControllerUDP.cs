using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using System.IO;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;
public class ClientControllerUDP : MonoBehaviour
{
    bool clientReady = false;

    public UdpClient udpClient;

    //TcpClient mySocket;
    Stream theStream;
    StreamWriter theWriter;
    StreamReader theReader;
    public String Host = "192.168.3.200";
    //public String Host = "192.168.137.140"; // Sony Z2
    //String Host = "192.168.20.51"; //LG G1
    //public String Host = "192.168.20.59";  //Samsung S6
    public int Port = 80;

    public static string loc;
    

    //public GameObject sourceObj;
    //Texture2D sourceTexture;
    //Texture2D destTexture;
    //Renderer sourceRenderer;

    public Texture2D failureTex;
    public Text packageInfo;
    //*************************************************************
    // Use this for initialization
    void Start()
    {
        //Get the source object renderer(camera renderer)
        //sourceRenderer = sourceObj.GetComponent<Renderer>();
        //----------
        //Set the camera renderer as texture2D
        //sourceTexture = sourceRenderer.material.mainTexture as Texture2D;

        //Get the pixels od camera renderer
        //Color32[] pix = sourceTexture.GetPixels32();
        //System.Array.Reverse(pix);

        //Create a new texture2D with the sizes of source texture2D
        //destTexture = new Texture2D(sourceTexture.width, sourceTexture.height);
        //Set the pixels of camera renderer(texture2D) to the destTexture
        //destTexture.SetPixels32(pix);
        //Apply these pixel
        //destTexture.Apply();

        //Call setupSocket to send destTexture
        //setupSocket();

        //Clear memory
        //Array.Clear(pix, 0, pix.Length);

        Resources.UnloadUnusedAssets();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        
        
        
        //GetComponent<Renderer>().material.mainTexture = destTexture;


    }
    public class MapInfo
    {
        public float lat;
        public float lot;
        public float alt;
        public string name;
        public int symbolIndex;

        public MapInfo(float lat, float lot, float alt, string name,int symbolIndex)
        {
            this.lat = lat;
            this.lot = lot;
            this.alt = alt;
            this.name = name;
            this.symbolIndex = symbolIndex;
        }
        public MapInfo() { }


        public float getLat()
        {
            return lat;
        }

        public float getLot()
        {
            return lot;
        }
        public float getAlt()
        {
            return alt;
        }
        public string getName()
        {
            return name;
        }
        public int getSymbolIndex()
        {
            return symbolIndex;
        }
    }
   

    // Update is called once per frame
    void Update()
    {
       // JsonConvert.SerializeObject("nesne");
       //mesageınfo =  JsonConvert.DeserializeObject<"sınıf tipi">("strinbg");

        
        //sourceRenderer = sourceObj.GetComponent<Renderer>();
        ////----------
        //sourceTexture = sourceRenderer.material.mainTexture as Texture2D;

        //Color32[] pix = sourceTexture.GetPixels32();
        ////Color32[] testPix = Enumerable.Repeat(sourceTexture, sourceTexture.width * sourceTexture.height).ToArray();
        //System.Array.Reverse(pix);
       

        //destTexture = new Texture2D(sourceTexture.width, sourceTexture.height);
        //destTexture.SetPixels32(pix);
        //Array.Clear(pix, 0, pix.Length);

        //destTexture.Apply();

        //Call setupSocket to send destTexture
        setupSocket();

        //GetComponent<Renderer>().material.mainTexture = destTexture;

       // Invoke("garbageCollector", 1);
    }

    public void setupSocket()
    {
        
            udpClient  = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(Host), Port);
           
            MapInfo mapInfo = new MapInfo(39.90795f, 32.75042f, 2000.0f, "Bytes",1);

        // Debug.Log(JsonUtility.ToJson(mapInfo));
        string a = JsonConvert.SerializeObject(mapInfo);

            MapInfo map = JsonConvert.DeserializeObject<MapInfo>(a);
        if (a.Length != 0)
        {
            clientReady = true;
        }
            //Debug.Log(map.getAlt());
            //Debug.Log(map.getLat());
            //Debug.Log(map.getLot());
            //Debug.Log(map.getName());
            Debug.Log("Client is Ready." + a);
        try
        {

            udpClient.Connect(ip);
            if (!clientReady)
                return;         
            else
            {
                Debug.Log("baglantı saglandı"+ip);
                //byte[] data = destTexture.EncodeToJPG();
                //a = map.lat.ToString()+map.lot.ToString();
                byte[] locVeri = System.Text.Encoding.ASCII.GetBytes(a);

                if (locVeri != null)
                {
                    Debug.Log("symbolIndex :" + map.getSymbolIndex().ToString());
                    //udpClient.Send(data, data.Length);
                    udpClient.Send(locVeri, locVeri.Length);
                    //destTexture.Apply(true, true);
                    Debug.Log("Local Veri" + locVeri.Length+locVeri[1]);
                    // Debug.Log("Data has been sent!");
                }
                else
                {
                    Debug.Log("Data is NULL!");
                }
            }
        }

        catch (Exception e)
        {
            //packageInfo.text = "Exception: " + e;
            e.ToString();

            //byte[] infoData = failureTex.EncodeToJPG();
            //udpClient.Send(infoData, infoData.Length);

            Debug.Log("Socket error:" + e);
        }
        clientReady = false;
        
    }
    private void OnApplicationQuit()
    {
        udpClient.Dispose();
    }

    private void garbageCollector()
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
        
        GC.WaitForPendingFinalizers();

        Invoke("garbageCollector", 1);
    }
} 