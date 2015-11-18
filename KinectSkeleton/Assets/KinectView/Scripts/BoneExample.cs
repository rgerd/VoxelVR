using UnityEngine;
using System.Collections;

public class BoneExample : MonoBehaviour {

	public GameObject sphere_prefab;
	public GameObject bone_prefab;

	private GameObject[] bones;
	private GameObject[] spheres;

	private Vector3[] pos;
	private Vector3[] targets;

	private float t;


	private Vector3 midpoint;

	void Start () {
		int numSpheres = 2 * 15;//(int)(Random.Range(1, 5));

		spheres = new GameObject[numSpheres];
		for (int i = 0; i < numSpheres; i++) {
			spheres[i] = (GameObject) Instantiate(sphere_prefab, new Vector3(Random.value * 10, Random.value * 10, Random.value * 10), Quaternion.identity);
		}

		bones = new GameObject[numSpheres - 1];
		for (int i = 0; i < bones.Length; i++) {
			// Store the clone of the bone prefab in an array
			bones[i] = (GameObject) Instantiate (bone_prefab, Vector3.zero, Quaternion.identity);
			
			// Add the joints by getting the script as a component
			BoneScript script = bones[i].GetComponent("BoneScript") as BoneScript;
			script.joint1 = spheres[i];
			script.joint2 = spheres[i + 1];
			script.radius = 0.5f;
		}

		pos = new Vector3[numSpheres];
		targets = new Vector3[numSpheres];

		t = 1;
	}
	
	// Moves the joints around a sphere. 
	// All updating for the bone happens automatically.
	void Update () {
		if (Mathf.Min (t, 1) == 1) {
			for(int i = 0; i < spheres.Length; i++) {
				pos[i] = spheres[i].transform.position;
				targets[i] = Random.onUnitSphere * Random.Range(6, 10);
			}
			t = 0;
		}

		for (int i = 0; i < spheres.Length; i++) {
			spheres[i].transform.position = Vector3.Lerp(pos[i], targets[i], t);
		}

		t += Time.deltaTime;
	}
}
