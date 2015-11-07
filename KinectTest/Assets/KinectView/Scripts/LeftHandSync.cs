using UnityEngine;
using System.Collections;
using Kinect = Windows.Kinect;

public class LeftHandSync : MonoBehaviour {

	public GameObject leftSphere;
	public GameObject rightSphere;
	public GameObject head;
	public GameObject bv;
	public GameObject bm;

	LineRenderer leftLR;
	LineRenderer rightLR;

	// Use this for initialization
	void Start () {
		leftLR = leftSphere.AddComponent<LineRenderer> ();
		rightLR = rightSphere.AddComponent < LineRenderer> ();
		leftLR.SetVertexCount (2);
		rightLR.SetVertexCount (2);
	}
	
	// Update is called once per frame
	void Update () {
		Transform leftObj = bv.GetComponent<BodySourceView> ().jointObjs [7];
		Transform rightObj = bv.GetComponent<BodySourceView> ().jointObjs [11];
		Transform headObj = bv.GetComponent<BodySourceView> ().jointObjs [(int)Kinect.JointType.Head];
		if (headObj != null) {
			Vector3 top = headObj.position;
			head.transform.position = top;
		}
		if (rightObj != null) { 
			Vector3 right = rightObj.position;
			rightSphere.transform.position = right;
			rightLR.SetPosition (0, rightObj.position);
			rightLR.SetPosition (1, headObj.position);
		}
		if (leftObj != null) {
			Vector3 left = leftObj.position;
			leftSphere.transform.position = left;
			leftLR.SetPosition (0, leftObj.position);
			leftLR.SetPosition (1, headObj.position);
		}
	} 
}
