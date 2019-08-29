using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Collections.Generic;
using UnityEngine.UI;


public class NetworkListener : MonoBehaviour {

    UdpClient udpClient;
    IPAddress ipaddress = IPAddress.Parse("127.0.0.1");
    //IPAddress ipaddress = IPAddress.Parse("192.168.20.195");
    
    public SymbolManager symbolManager;

    public Text receivedData;

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
	    udpClient = new UdpClient(1234);
        Debug.Log("IPAddress: "+ ipaddress);
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(ipaddress, 1234);
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
      
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);

        byte[] received = udpClient.EndReceive(res, ref RemoteIpEndPoint);
    

        receivedString = System.Text.Encoding.UTF8.GetString(received);
        Debug.Log("Data Received : " + receivedString); 
        TrackData trackData = JsonUtility.FromJson<TrackData>(receivedString);
        //Process codes


        symbolManager.AddSymbol(trackData.trackInfo.trackId, trackData.trackInfo.trackLon, trackData.trackInfo.trackLat, trackData.trackInfo.trackAlt, trackData.trackInfo.category);


        udpClient.BeginReceive(new System.AsyncCallback(recvCb), null);
    }
}
