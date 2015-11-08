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

	public static Vector3 anchor = new Vector3(0, 0, 0);

	public static string playerID = null;
	public static GameObject body = null;
	public static GameObject leftHand = null;
	public static GameObject rightHand = null;
	public static GameObject leftElbow = null;
	public static GameObject rightElbow = null;
	public static GameObject head = null;

	public float firing_threshold = 5f;
	public float fire_cooldown = 3f;

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
	float lastFire = -1;
	void Update () {
        if (playerID == null)
            return;
		camera.transform.position = cam_pos;
        body.transform.position = body_pos;
        rightHand.transform.position = left_hand_pos;
        leftHand.transform.position = right_hand_pos;
		leftElbow.transform.position = left_elbow_pos;
		rightElbow.transform.position = right_elbow_pos;
        head.transform.position = cam_pos;

		Vector3 distance = new Vector3 (0,0,0);
		GameObject bullet = null;

		distance = leftHand.transform.position - rightElbow.transform.position;
		//Debug.Log ("distance mag:" + distance.magnitude);
		if (Time.time - lastFire > fire_cooldown && distance.magnitude < firing_threshold) {
			lastFire = Time.time;
			Debug.Log("firing bullet from right hand");
			bullet = PhotonNetwork.Instantiate("Bullet", rightHand.transform.position, Quaternion.identity, 0, null);
			distance = rightHand.transform.position - rightElbow.transform.position;
			((BulletController) bullet.GetComponent("BulletController")).direction = distance != Vector3.zero ? distance : Vector3.zero;
		}

		distance = rightHand.transform.position - leftElbow.transform.position;
		if (Time.time - lastFire > fire_cooldown && distance.magnitude < firing_threshold) {
			lastFire = Time.time;
			Debug.Log("firing bullet from left hand");
			bullet = PhotonNetwork.Instantiate("Bullet", leftHand.transform.position, Quaternion.identity, 0, null);
			distance = leftHand.transform.position - leftElbow.transform.position;
			((BulletController) bullet.GetComponent("BulletController")).direction = distance != Vector3.zero ? distance : Vector3.zero;
		}

    }

    public void AllMessageHandler(OscMessage oscMessage) {
		if (playerID == null)
			return;
		string msg = Osc.OscMessageToString (oscMessage).Substring (1);
		string[] _vals = msg.Split (' ');

		float[] vals = new float[_vals.Length];
		for (int i = 0; i < vals.Length; i++) {
			vals[i] = float.Parse(_vals[i]);
		}

		cam_pos = new Vector3 (vals[9] * (playerID == "P1" ? -1 : 1), vals[10], -vals[11] * (playerID == "P1" ? -1 : 1) + 19) + anchor;

		left_hand_pos = new Vector3 (vals [33] * (playerID == "P1" ? -1 : 1) + anchor.x, vals [34], -vals [35] * (playerID == "P1" ? -1 : 1) + 19) + anchor;
		right_hand_pos = new Vector3(vals[21] * (playerID == "P1" ? -1 : 1), vals[22], -vals[23] * (playerID == "P1" ? -1 : 1) + 19) + anchor;

		left_elbow_pos = new Vector3(vals[15] * (playerID == "P1" ? -1 : 1), vals[16], -vals[17] * (playerID == "P1" ? -1 : 1) + 19) + anchor;
		right_elbow_pos = new Vector3 (vals [27] * (playerID == "P1" ? -1 : 1), vals [28], -vals [29] * (playerID == "P1" ? -1 : 1) + 19) + anchor;

		body_pos = new Vector3 (vals[3] * (playerID == "P1" ? -1 : 1), vals[4], -vals[5] * (playerID == "P1" ? -1 : 1) + 19) + anchor;
	}
}
