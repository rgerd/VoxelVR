using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			transform.RotateAround(new Vector3(0,0,0), Vector3.left, 10*Time.deltaTime);
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			transform.RotateAround(new Vector3(0,0,0), Vector3.left, -10*Time.deltaTime);
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			transform.RotateAround(new Vector3(0,0,0), Vector3.up, 10*Time.deltaTime);
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			transform.RotateAround(new Vector3(0,0,0), Vector3.up, -10*Time.deltaTime);
		}
	}
}
