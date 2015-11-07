using UnityEngine;
using System.Collections;

public class OneSphere : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.z < -200) {
			GameObject.Destroy (gameObject);
		}
	}
}
