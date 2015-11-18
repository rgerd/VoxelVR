using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodySourceView : MonoBehaviour {
	public GameObject BodySourceManager;
	public GameObject bone_prefab;
	// TODO: Switch to Camera.main to avoid needing public GameObject
	public GameObject camera;

	public string message;

	private GameObject[] bones;
	private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
	private BodySourceManager _BodyManager;

	private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
	{
	  { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
	  { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
	  { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
	  { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

	  { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
	  { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
	  { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
	  { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

	  { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
	  { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
	  { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
	  { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
	  { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
	  { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

	  { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
	  { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
	  { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
	  { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
	  { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
	  { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

	  { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
	  { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
	  { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
	  { Kinect.JointType.Neck, Kinect.JointType.Head },
	};

	ulong trackID = 0;

	void Update () {
		if (BodySourceManager == null) return;
		_BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
		if (_BodyManager == null) return;

		Kinect.Body[] data = _BodyManager.GetData();
		if (data == null) return;

		List<ulong> trackedIds = new List<ulong>();
		foreach(var body in data) {
		  if (body == null) continue;

		  if(body.IsTracked)
		    trackedIds.Add (body.TrackingId);
		}

		List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

		// First delete untracked bodies
		foreach(ulong trackingId in knownIds) {
		  if(!trackedIds.Contains(trackingId)) {
		    Destroy(_Bodies[trackingId]);
		    _Bodies.Remove(trackingId);
		    if (trackID == trackingId)
		      trackID = 0;
		  }
		}

		foreach (var body in data) {
		  if (trackID == 0) {
		    trackID = body.TrackingId;
		  }
		  if (trackID == body.TrackingId) {
		    if (body == null) continue;

		    if (body.IsTracked) {
		      if (!_Bodies.ContainsKey (body.TrackingId)) {
		        _Bodies [body.TrackingId] = CreateBodyObject (body.TrackingId);
		      }
		      RefreshBodyObject (body, _Bodies [body.TrackingId]);
		    }
		  }
		}
	}

	private GameObject CreateBodyObject(ulong id) {
		GameObject body = new GameObject("Body:" + id);

		for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++) {
		    GameObject jointObj = GameObject.Instantiate(new GameObject());
		    jointObj.name = jt.ToString();
		    jointObj.transform.parent = body.transform;
		}

		Transform bodyTransform = body.transform;

		bones = new GameObject[5];
		bones[0] = addBone("LeftArmTop", 0.5f, bone_prefab, bodyTransform,
		bodyTransform.FindChild(Kinect.JointType.ShoulderLeft.ToString()).gameObject,
		bodyTransform.FindChild(Kinect.JointType.ElbowLeft.ToString()).gameObject);

		bones[1] = addBone("LeftArmBottom", 0.5f, bone_prefab, bodyTransform,
		bodyTransform.FindChild(Kinect.JointType.ElbowLeft.ToString()).gameObject,
		bodyTransform.FindChild(Kinect.JointType.HandLeft.ToString()).gameObject);

		bones[2] = addBone("RightArmTop", 0.5f, bone_prefab, bodyTransform,
		bodyTransform.FindChild(Kinect.JointType.ShoulderRight.ToString()).gameObject,
		bodyTransform.FindChild(Kinect.JointType.ElbowRight.ToString()).gameObject);

		bones[3] = addBone("RightArmBottom", 0.5f, bone_prefab, bodyTransform,
		bodyTransform.FindChild(Kinect.JointType.ElbowRight.ToString()).gameObject,
		bodyTransform.FindChild(Kinect.JointType.HandRight.ToString()).gameObject);

		bones[4] = addBone("Body", 1.5f, bone_prefab, bodyTransform,
		bodyTransform.FindChild(Kinect.JointType.SpineBase.ToString()).gameObject,
		bodyTransform.FindChild(Kinect.JointType.SpineShoulder.ToString()).gameObject);

		return body;
	}

	private GameObject addBone(String name, float radius, GameObject prefab, Transform body, GameObject joint1, GameObject joint2) {
		GameObject bone = (GameObject) Instantiate(prefab, Vector3.zero, Quaternion.identity);
		bone.name = name;
		bone.transform.parent = body;
		BoneScript script = bone.getComponent("BoneScript") as BoneScript;
		script.joint1 = joint1;
		script.joint2 = joint2;
		script.radius = radius;
		return bone;
	}

	private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject) {
		message = "";
		for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++) {
		  Kinect.Joint sourceJoint = body.Joints[jt];
		  Kinect.Joint? targetJoint = null;

		  if(_BoneMap.ContainsKey(jt))
		    targetJoint = body.Joints[_BoneMap[jt]];

		  Transform jointObj = bodyObject.transform.FindChild(jt.ToString());
		  jointObj.localPosition = GetVector3FromJoint(sourceJoint);

		  float xval = (int) ((jointObj.position.x)* 10000) / 10000f;
		  float yval = (int) ((jointObj.position.y) * 10000) / 10000f;
		  float zval = (int) ((jointObj.position.z) * 10000) / 10000f;
		  message += xval.ToString() + " " + yval.ToString() + " " + zval.ToString() + " ";

		  if (jt == Kinect.JointType.Head)
		    camera.transform.position = (Vector3) jointObj.transform.position;
		}
	}

	private static Color GetColorForState(Kinect.TrackingState state) {
		switch (state) {
		  case Kinect.TrackingState.Tracked:
		  return Color.green;

		  case Kinect.TrackingState.Inferred:
		  return Color.red;

		  default:
		  return Color.black;
		}
	}

	private static Vector3 GetVector3FromJoint(Kinect.Joint joint) {
		return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
	}

	public string getString() {
		return message;
	}
}
