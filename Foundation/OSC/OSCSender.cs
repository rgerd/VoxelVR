using UnityEngine;
using System.Collections;

public class OSCSender : MonoBehaviour {
	public string remoteIp = "127.0.0.1";
	public int sendToPort = 9000;
	public int listenerPort = 8000;
	private Osc handler = null;
	
	void Start() {
		UDPPacketIO udp = (UDPPacketIO) GetComponent("UDPPacketIO");
		udp.init(remoteIp, sendToPort, listenerPort);
		handler = (Osc) GetComponent("Osc");
		handler.init(udp);
		//oscHandler.SetAddressHandler("/1/push1", Example);
	}
	
	void Update() {
		string message = "";
		OscMessage oscM = null;
		oscM = Osc.StringToOscMessage("/" + message);
		handler.Send(oscM);
	}

	void OnDisable() {
		Debug.Log("Closing OSC UDP socket in OnDisable");
		handler.Cancel();
		handler = null;
	}
}
