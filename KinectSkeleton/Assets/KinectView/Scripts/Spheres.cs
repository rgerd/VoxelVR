using UnityEngine;
using System.Collections;

public class Spheres : MonoBehaviour {
	public GameObject sphere;

	private int count;
	// Use this for initialization
	void Start () {
		count = 0;
	}
	
	// Update is called once per frame
	void Update () {
		count++;
		if (count > 100) {
			Vector3 initialPosition = new Vector3(0, 0, 0);
			initialPosition.x = gameObject.transform.position.x + Random.Range(-10, 10);
			initialPosition.y = gameObject.transform.position.y + Random.Range(-10, 10);
			initialPosition.z = gameObject.transform.position.z + Random.Range(-10, 10);
			GameObject newSphere = Instantiate<GameObject>(sphere);
			sphere.transform.position = initialPosition;
			count = 0;
		}
	}
}
