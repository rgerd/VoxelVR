using UnityEngine;
using System.Collections;

public class BurstController : MonoBehaviour {
	private ParticleSystem burst;

	// Use this for initialization
	void Start () {
		burst = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (burst && !burst.IsAlive()) {
			Destroy(gameObject);
		}
	}
}
