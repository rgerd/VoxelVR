using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OSCReceiver : MonoBehaviour {

	public string RemoteIP = "127.0.0.1"; //127.0.0.1 signifies a local host (if testing locally
	public int SendToPort = 8000; //the port you will be sending from
	public int ListenerPort = 9000; //the port you will be listening on
	public Transform controller;
	private Osc handler;
	
	public TextMesh debug_mesh;
	private string debug_message;

	public GameObject camera;
	private Vector3 cam_pos;

	private Vector3 left_hand_pos;
	private Vector3 right_hand_pos;
	private Vector3 left_elbow_pos;
	private Vector3 right_elbow_pos;
	private Vector3 body_pos;

	void Start () {
		UDPPacketIO udp = (UDPPacketIO) GetComponent ("UDPPacketIO");
		udp.init (RemoteIP, SendToPort, ListenerPort);
		handler = (Osc) GetComponent ("Osc");
		handler.init(udp);
		handler.SetAllMessageHandler(AllMessageHandler);
		debug_message = "" + RemoteIP;

		cam_pos = new Vector3 (0, 0, 0);
		left_hand_pos = new Vector3 (0, 0, 0);
		right_hand_pos = new Vector3 (0, 0, 0);
		left_elbow_pos = new Vector3 (0, 0, 0);
		right_elbow_pos = new Vector3 (0, 0, 0);
		body_pos = new Vector3 (0, 0, 0);
	}
    public static string playerID = null;
    public static GameObject body = null;
    public static GameObject leftHand = null;
    public static GameObject rightHand = null;
    public static GameObject leftElbow = null;
    public static GameObject rightElbow = null;
    public static GameObject head = null;
	void Update () {
        if (playerID == null)
            return;
		camera.transform.position = cam_pos;
        body.transform.position = body_pos;
        leftHand.transform.position = left_hand_pos;
        rightHand.transform.position = right_hand_pos;
        leftElbow.transform.position = left_elbow_pos;
        rightElbow.transform.position = right_elbow_pos;
        head.transform.position = cam_pos;
    }

    public void AllMessageHandler(OscMessage oscMessage) {
		string msg = Osc.OscMessageToString (oscMessage).Substring (1);
		string[] _vals = msg.Split (' ');

		float[] vals = new float[_vals.Length];
		for (int i = 0; i < vals.Length; i++) {
			vals[i] = float.Parse(_vals[i]);
		}

		cam_pos = new Vector3 (vals[9], vals[10], -vals[11]);

		left_hand_pos = new Vector3 (vals [33], vals [34], -vals [35]);
		right_hand_pos = new Vector3(vals[21], vals[22], -vals[23]);

		left_elbow_pos = new Vector3(vals[15], vals[16], -vals[17]);
		right_elbow_pos = new Vector3 (vals [27], vals [28], -vals [29]);

		body_pos = new Vector3 (vals[3], vals[4], -vals[5]);
	}
}
