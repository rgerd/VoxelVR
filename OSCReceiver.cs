using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OSCReceiver : MonoBehaviour {
	
	public string RemoteIP = "127.0.0.1"; //127.0.0.1 signifies a local host (if testing locally
	public int SendToPort = 8000; //the port you will be sending from
	public int ListenerPort = 9000; //the port you will be listening on
	private Osc handler;
	
	public static float[] vars;

	void Start () {
		UDPPacketIO udp = (UDPPacketIO) GetComponent ("UDPPacketIO");
		udp.init (RemoteIP, SendToPort, ListenerPort);
		handler = (Osc) GetComponent ("Osc");
		handler.init(udp);
		handler.SetAllMessageHandler(AllMessageHandler);
	}

	void Update () {

	}
	
	public void AllMessageHandler(OscMessage oscMessage) {
		string msg = Osc.OscMessageToString (oscMessage).Substring (1);
		string[] _vals = msg.Split (' ');
		
		float[] vals = new float[_vals.Length];
		for (int i = 0; i < vals.Length; i++) {
			vals[i] = float.Parse(_vals[i]);
		}
	}
}
