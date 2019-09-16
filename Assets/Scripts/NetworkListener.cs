using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Collections.Generic;
using UnityEngine.UI;


public class NetworkListener : MonoBehaviour {

    UdpClient udpClient;
    IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
    //IPAddress ipaddress = IPAddress.Parse("192.168.20.195");
    //IPAddress ipAddress = IPAddress.Parse("192.168.137.140");
    
    public SymbolManager symbolManager;

    public Text receivedData;
    public Text transmittingData;
    public string transmitString;
    [Serializable]
    public struct UserInfo
    {
        public string senderId;
        public string senderName;
    }

    [Serializable]
    public struct TrackInfo
    {
        public string trackId;
        public string category;
        public string subcategory;
        public float trackLat;
        public float trackLon;
        public float trackAlt;
        public int detectionTime;
    }

    [Serializable]
    public struct TrackData
    {
        public TrackInfo trackInfo;
        public UserInfo userInfo;
    }

    //public struct UnitInfo
    //{
    //    public LocationInfo location;
    //    public int time;
    //    public UserInfo sender;
    //}
	// Use this for initialization

    public string receivedString = "Pending..";
	void Start () {
        Debug.Log("NETWORK LISTENER ");

	    udpClient = new UdpClient(1333);
        Debug.Log("IPAddress: "+ ipAddress);
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(ipAddress, 1333);
        Debug.Log("Start Listening..");
        udpClient.BeginReceive(new System.AsyncCallback(recvCb), null);
      
        
	}
	
	// Update is called once per frame
	void Update () {
       //receivedData.text = receivedString;
	
	}

    //List<string> knownTracks;

    public void recvCb(System.IAsyncResult res)
    {
        Debug.Log("veri alınıyor");
      
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1333);

        byte[] received = udpClient.EndReceive(res, ref RemoteIpEndPoint);
   

        receivedString = System.Text.Encoding.UTF8.GetString(received);
        //Debug.Log("Data Received : " + receivedString); 
        TrackData trackData = JsonUtility.FromJson<TrackData>(receivedString);
        Debug.Log("Alınan Veri:  "+receivedString);

        //Process codes
        if (trackData.trackInfo.trackLat != 0 && trackData.trackInfo.trackLon != 0)
        {
            symbolManager.AddSymbol(trackData.trackInfo.trackId, trackData.trackInfo.trackLon, trackData.trackInfo.trackLat, trackData.trackInfo.trackAlt, trackData.trackInfo.category);
            //Debug.Log("id" + trackData.trackInfo.trackId);
            //Debug.Log("lon" + trackData.trackInfo.trackLon);
            //Debug.Log("lat" + trackData.trackInfo.trackLat);
            //Debug.Log("caty" + trackData.trackInfo.category);
        }
        else
        {
            Debug.Log("Lokasyon degişmedi"+trackData.trackInfo.trackLat+"-"+trackData.trackInfo.trackLon);
        }
        udpClient.BeginReceive(new System.AsyncCallback(recvCb), null);
    }
    public void transmitData(System.IAsyncResult res)
    {
        //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        
        //IPEndPoint IpTransEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);

        //transmitString =;
        //byte[] transmitByte = System.Text.Encoding.UTF8.GetBytes();
    }
}
