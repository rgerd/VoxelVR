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
		if (rightObj != null) { //test comment 1
			Vector3 right = rightObj.position;
			rightSphere.transform.position = right;
		}
		if (leftObj != null) {
			Vector3 left = leftObj.position;
			leftSphere.transform.position = left;
		}
	} //test comment 2
}
