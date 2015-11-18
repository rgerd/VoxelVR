using UnityEngine;
using System.Collections;

private Osc oscHandler = null;
public String remoteIp;
public int sendToPort;
public int listenerPort;
public GameObject BodyView;


public class OSCSender : MonoBehaviour {
	// Start is called just before any of the Update methods is called the first time.
	void Start() {
		UDPPacketIO udp = (UDPPacketIO) GetComponent("UDPPacketIO");
		udp.init(remoteIp, sendToPort, listenerPort);
		
		oscHandler = (Osc) GetComponent("Osc");
		oscHandler.init(udp);
		//oscHandler.SetAddressHandler("/1/push1", Example);
	}
	
	void Update() {
		String message = BodyView.GetComponent("BodySourceView").getString();
		OscMessage oscM = null;
		oscM = Osc.StringToOscMessage("/" + message);
		oscHandler.Send(oscM);
	}

	void OnDisable() {
		// close OSC UDP socket
		Debug.Log("closing OSC UDP socket in OnDisable");
		oscHandler.Cancel();
		oscHandler = null;
	}
}
