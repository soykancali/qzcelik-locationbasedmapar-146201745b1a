using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.IO;
using System.Net.Sockets;

public class ClientControllerTCP : MonoBehaviour
{
    bool socketReady = false;

    TcpClient mySocket;
    Stream theStream;

    StreamWriter theWriter;
    StreamReader theReader;
    //String Host = "127.0.0.1";
    //String Host = "192.168.20.59"; //PC
    //String Host = "192.168.20.51"; //LG G1
    public String Host = "192.168.20.59";
    public int Port = 19070;


    public GameObject sourceObj;
    Texture2D sourceTexture;
    Texture2D destTexture;
    Renderer sourceRenderer;

 
    //*************************************************************
    // Use this for initialization
    void Start()
    {
        //Get the source object renderer (camera renderer)
        sourceRenderer = sourceObj.GetComponent<Renderer>();
        //----------
        //Set the camera renderer as texture2D
        sourceTexture = sourceRenderer.material.mainTexture as Texture2D;

        //Get the pixels od camera renderer
        Color32[] pix = sourceTexture.GetPixels32();
        System.Array.Reverse(pix);

        //Create a new texture2D with the sizes of source texture2D
        destTexture = new Texture2D(sourceTexture.width, sourceTexture.height);
        //Set the pixels of camera renderer (texture2D) to the destTexture
        destTexture.SetPixels32(pix);
        //Apply these pixel
        destTexture.Apply();

        Array.Clear(pix, 0, pix.Length);
        
        
        GetComponent<Renderer>().material.mainTexture = destTexture;


        //----------------
        setupSocket();

    }

    // Update is called once per frame
    void Update()
    {
        
        sourceRenderer = sourceObj.GetComponent<Renderer>();
        //----------
        sourceTexture = sourceRenderer.material.mainTexture as Texture2D;

        Color32[] pix = sourceTexture.GetPixels32();
        //Color32[] testPix = Enumerable.Repeat(sourceTexture, sourceTexture.width * sourceTexture.height).ToArray();
        System.Array.Reverse(pix);

        destTexture = new Texture2D(sourceTexture.width, sourceTexture.height);
        destTexture.SetPixels32(pix);
        destTexture.Apply();

        GetComponent<Renderer>().material.mainTexture = destTexture;

        //destTexture.Apply(makeNoLongerReadable);
        Array.Clear(pix, 0, pix.Length);

        Resources.UnloadUnusedAssets();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        
         
        //--------------
        setupSocket();
    }

    public void setupSocket()
    {
        try
        {
            mySocket = new TcpClient();
            
            mySocket.Connect(Host, Port);

            theStream = mySocket.GetStream();
        

            socketReady = true;

            Debug.Log("Client is Ready.");


            if (!socketReady)
                return;
            else
            {
                byte[] data = destTexture.EncodeToJPG();
                
                if (data != null)
                {
                    theStream.Write(data, 0, data.Length);
                    
                    Debug.Log("Data has been sent!");
                }
                else
                {
                    Debug.Log("Data is NULL!");
                }
            }

        }
        catch (Exception e)
        {
            Debug.Log("Socket error:" + e);
        }
    }
} 