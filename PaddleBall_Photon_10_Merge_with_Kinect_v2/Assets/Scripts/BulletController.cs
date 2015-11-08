using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public float speedOffset = 1f;
	public GameObject burst;
	public Vector3 direction = Vector3.forward;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("direction: " + direction);
	    transform.Translate(direction * (Time.deltaTime * OpponentSpeed.speed + speedOffset));
	}

	void OnTriggerEnter(Collider other) {

		string playerName = OSCReceiver.playerID;
		
		if (other.gameObject.CompareTag ("East_West Wall") || 
			other.gameObject.CompareTag ("Ceiling_Floor Wall") ||
			other.gameObject.CompareTag ("North_South Wall")) {
			Instantiate(burst, transform.position, Quaternion.identity);
			PhotonNetwork.Destroy (gameObject);
		}

		if (other.gameObject.CompareTag (playerName + "Head") ||
		    other.gameObject.CompareTag (playerName + "Body") ||
		    other.gameObject.CompareTag (playerName + "LeftHand") ||
		    other.gameObject.CompareTag (playerName + "RightHand") ||
		    other.gameObject.CompareTag (playerName + "LeftElbow") ||
		    other.gameObject.CompareTag (playerName + "RightElbow")) {
			OpponentSpeed.score += 1;
			Instantiate(burst, transform.position, Quaternion.identity);
			PhotonNetwork.Destroy (gameObject);
		}
	}
}
