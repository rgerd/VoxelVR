using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public float speedOffset = 1f;
	public GameObject burst;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	   transform.Translate(Vector3.forward * (Time.deltaTime * OpponentSpeed.speed + speedOffset));
	}

	void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.CompareTag ("East_West Wall") || 
			other.gameObject.CompareTag ("Ceiling_Floor Wall") ||
			other.gameObject.CompareTag ("North_South Wall")) {
			Instantiate(burst, transform.position, Quaternion.identity);
			PhotonNetwork.Destroy (gameObject);
		}
		if (other.gameObject.CompareTag ("Player")) {
			Instantiate(burst, transform.position, Quaternion.identity);
			PhotonNetwork.Destroy (gameObject);
		}
	}
}
