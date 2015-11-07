using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class LeftHandSync : MonoBehaviour {

	public GameObject leftSphere;
	public GameObject rightSphere;
	public GameObject bv;
	public GameObject bm;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Transform leftObj = bv.GetComponent<BodySourceView> ().jointObjs [7];
		Transform rightObj = bv.GetComponent<BodySourceView> ().jointObjs [11];
		if (rightObj != null) { 
			Vector3 right = rightObj.position;
			rightSphere.transform.position = right;
		}
		if (leftObj != null) {
			Vector3 left = leftObj.position;
			leftSphere.transform.position = left;
		}
	} 
}
