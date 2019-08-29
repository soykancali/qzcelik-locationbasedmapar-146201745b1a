
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class RenderController : MonoBehaviour
{
    private static int localPort;

    // prefs
    private string IP;  // define in init
    public int port;  // define in init

    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;

    // gui
    string strMessage = ""; 


    //----------------------------
    Texture2D camTexture;
    //public Texture2D sourceTex;
    public GameObject sourceObj;

    // call it from shell (as program)
    private static void Main()
    {
        RenderController sendObj = new RenderController();
        sendObj.init();

        // testing via console
        // sendObj.inputFromConsole();

        // as server sending endless
        sendObj.sendEndless(" endless infos \n");

    }
    // start from unity3d
    public void Start()
    {
        init();
    }


    void Update()
    {

        

    }


      private class TransmissionData{
         public int curDataIndex; //current position in the array of data already received.
         public byte[] data;
 
         public TransmissionData(byte[] _data){
             curDataIndex = 0;
             data = _data;
         }
     }
    private int defaultBufferSize = 1024;
    // OnGUI
    void OnGUI()
    {
        Rect rectObj = new Rect(40, 380, 200, 400);
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        GUI.Box(rectObj, "# UDPSend-Data\n127.0.0.1 " + port + " #\n"
                    + "shell> nc -lu 127.0.0.1  " + port + " \n"
                , style);

        // ------------------------
        // send it
        // ------------------------
        strMessage = GUI.TextField(new Rect(40, 420, 140, 20), strMessage);
        if (GUI.Button(new Rect(190, 420, 40, 20), "send"))
        {
            sendString(strMessage + "\n");
            
        }
        //-------------------------------------------------------
        
        Texture2D sourceTex = sourceObj.GetComponent<Renderer>().material.mainTexture as Texture2D;
        Texture2D optimizedSourceTex = new Texture2D(sourceTex.width / 100, sourceTex.height / 100);

        //Texture2D destTex = new Texture2D(sourceTex.width / 100, sourceTex.height / 100);

        Color32[] pix = optimizedSourceTex.GetPixels32();
       
        System.Array.Reverse(pix);

        optimizedSourceTex.SetPixels32(pix);
        optimizedSourceTex.Apply();
        
        //GetComponent<Renderer>().material.mainTexture = destTex;

        byte[] textureBytes = optimizedSourceTex.EncodeToPNG();

        
        client.Send(textureBytes, textureBytes.Length, remoteEndPoint);



        for (int i = 0; i < textureBytes.Length; i++)
        {
            

        }
        
       

        Debug.Log("Size of Byte Array: " + textureBytes.Length);
        //client.Close();
        
        
    }

    //***************
    /*
    public IEnumerator SendBytesToClientsRoutine(int transmissionId, byte[] data)
    {
        //Debug.Assert(!serverTransmissionIds.Contains(transmissionId));
        //Debug.Log(LOG_PREFIX + "SendBytesToClients processId=" + transmissionId + " | datasize=" + data.Length);

        //tell client that he is going to receive some data and tell him how much it will be.
        RpcPrepareToReceiveBytes(transmissionId, data.Length);
        yield return null;

        //begin transmission of data. send chunks of 'bufferSize' until completely transmitted.
        serverTransmissionIds.Add(transmissionId);
        TransmissionData dataToTransmit = new TransmissionData(data);
        int bufferSize = defaultBufferSize;
        while (dataToTransmit.curDataIndex < dataToTransmit.data.Length - 1)
        {
            //determine the remaining amount of bytes, still need to be sent.
            int remaining = dataToTransmit.data.Length - dataToTransmit.curDataIndex;
            if (remaining < bufferSize)
                bufferSize = remaining;

            //prepare the chunk of data which will be sent in this iteration
            byte[] buffer = new byte[bufferSize];
            System.Array.Copy(dataToTransmit.data, dataToTransmit.curDataIndex, buffer, 0, bufferSize);

            //send the chunk
            RpcReceiveBytes(transmissionId, buffer);
            dataToTransmit.curDataIndex += bufferSize;

            yield return null;

            if (null != OnDataFragmentSent)
                OnDataFragmentSent.Invoke(transmissionId, buffer);
        }

        //transmission complete.
        serverTransmissionIds.Remove(transmissionId);

        if (null != OnDataComepletelySent)
            OnDataComepletelySent.Invoke(transmissionId, dataToTransmit.data);
    }
     * */
    //***************


    // init
    public void init()
    {
        // Endpunkt definieren, von dem die Nachrichten gesendet werden.
        print("UDPSend.init()");

        // define
        IP = "127.0.0.1";
        port = 19070;

        // ----------------------------
        // Senden
        // ----------------------------
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();

        // status
        print("Sending to " + IP + " : " + port);
        print("Testing: nc -lu " + IP + " : " + port);

    }

    // inputFromConsole
    private void inputFromConsole()
    {
        try
        {
            string text;
            do
            {
                text = Console.ReadLine();

                // Den Text zum Remote-Client senden.
                if (text != "")
                {
                    

                    // Daten mit der UTF8-Kodierung in das Binärformat kodieren.
                    byte[] data = Encoding.UTF8.GetBytes(text);

                    // Den Text zum Remote-Client senden.
                    client.Send(data, data.Length, remoteEndPoint);
                }
            } while (text != "");
        }
        catch (Exception err)
        {
            print(err.ToString());
        }

    }

    // sendData
    private void sendString(string message)
    {
        try
        {
            //if (message != "")
            //{

            // Daten mit der UTF8-Kodierung in das Binärformat kodieren.
            byte[] data = Encoding.UTF8.GetBytes(message);

            // Den message zum Remote-Client senden.
            client.Send(data, data.Length, remoteEndPoint);
            //}
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }


    // endless test
    private void sendEndless(string testStr)
    {
        do
        {
            sendString(testStr);


        }
        while (true);

    }

}


/*
using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;

public class RenderController : MonoBehaviour {

    UdpClient udpClient;
    IPAddress ipaddress = IPAddress.Parse("127.0.0.1");

	// Use this for initialization
    //public GameObject renderPlane;

    Texture2D camTexture;
    //public Texture2D sourceTex;
    public GameObject sourceObj;



    public string receivedString = "Pending..";

	void Start () {
        
        udpClient = new UdpClient(19070);
        Debug.Log("IPAddress: " + ipaddress);
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(ipaddress, 19070);
        Debug.Log("Start Listening..");
        udpClient.BeginReceive(new System.AsyncCallback(recvCb), null);
        
	}
	

	// Update is called once per frame
	void Update () {

        Texture2D sourceTex = sourceObj.GetComponent<Renderer>().material.mainTexture as Texture2D;
        Color32[] pix = sourceTex.GetPixels32();
        System.Array.Reverse(pix);
            
        Texture2D destTex = new Texture2D(sourceTex.width, sourceTex.height);
        destTex.SetPixels32(pix);
        destTex.Apply();
        GetComponent<Renderer>().material.mainTexture = destTex;

            
	}



    
    public void recvCb(System.IAsyncResult res)
    {

        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 19070);

        byte[] received = udpClient.EndReceive(res, ref RemoteIpEndPoint);


        receivedString = System.Text.Encoding.UTF8.GetString(received);
        Debug.Log("Data Received : " + receivedString);
        TrackData trackData = JsonUtility.FromJson<TrackData>(receivedString);
        //Process codes


        symbolManager.AddSymbol(trackData.trackInfo.trackId, trackData.trackInfo.trackLon, trackData.trackInfo.trackLat, trackData.trackInfo.trackAlt, trackData.trackInfo.category);


        udpClient.BeginReceive(new System.AsyncCallback(recvCb), null);
     * 

    }
    
}
*/