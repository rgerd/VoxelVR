using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class SkeletonRender : MonoBehaviour {
	public GameObject BodySourceManager;
	public GameObject bone_prefab;
	
	private GameObject body;
	private ulong body_id;
	public static GameObject head;
	public static GameObject[] bones;
	private SkeletonManager skeletonManager;
	
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
	
	void Update () {
		if (SkeletonManager == null) return;
		skeletonManager = SkeletonManager.GetComponent<SkeletonManager>();
		if (skeletonManager == null) return;
		
		Kinect.Body[] data = skeletonManager.GetData();
		if (data == null) return;
		
		List<ulong> trackingIds = new List<ulong> ();
		foreach (var body in data) {
			if(body == null) continue;
			if(body.IsTracked) trackingIds.Add(body.TrackingId);
		}

		if (!trackingIds.Contains (body_id)) {
			Destroy(body); body = null;
			body_id = 0;
		}
		
		foreach (var _body in data) {
			if (body_id == 0) body_id = _body.TrackingId;
			
			if (body_id == _body.TrackingId) {
				if (_body == null) continue;
				
				if (_body.IsTracked) {
					if (body == null)
						body = CreateBodyObject (body_id);
					RefreshBodyObject (_body, body);
				}
			}
		}
	}
	
	private GameObject CreateBodyObject(ulong body_id) {
		GameObject body = new GameObject("Body:" + body_id);
		
		for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++) {
			GameObject jointObj = null;
			if(jt == Kinect.JointType.Head)
				jointObj = Instantiate (CreatePrimitive(PrimitiveType.Sphere));
			else
				GameObject.Instantiate(new GameObject());
			jointObj.name = jt.ToString();
			jointObj.transform.parent = body.transform;
		}
		
		Transform bodyTransform = body.transform;
		bones = new GameObject[9];
		bones[0] = addBone("Body", 1.5f, bone_prefab, bodyTransform, 
		                   Kinect.JointType.SpineBase, 
		                   Kinect.JointType.SpineShoulder);
		bones[1] = addBone("LeftArmTop", 0.8f, bone_prefab, bodyTransform, Kinect.JointType.ElbowLeft);
		bones[2] = addBone("LeftArmBottom", 0.5f, bone_prefab, bodyTransform, Kinect.JointType.WristLeft);
		bones[3] = addBone("RightArmTop", 0.8f, bone_prefab, bodyTransform, Kinect.JointType.ElbowRight);
		bones[4] = addBone("RightArmBottom", 0.5f, bone_prefab, bodyTransform, Kinect.JointType.WristRight);
		bones[5] = addBone("LeftLegTop", 0.8f, bone_prefab, bodyTransform, Kinect.JointType.KneeLeft);
		bones[6] = addBone("LeftLegBottom", 0.5f, bone_prefab, bodyTransform, Kinect.JointType.AnkleLeft);
		bones[7] = addBone("RightLegTop", 0.8f, bone_prefab, bodyTransform, Kinect.JointType.KneeRight);
		bones[8] = addBone("RightLegBottom", 0.5f, bone_prefab, bodyTransform, Kinect.JointType.AnkleRight);
		
		return body;
	}
	
	private GameObject addBone(String name, float radius, GameObject prefab, Transform body, Kinect.JointType joint1, Kinect.JointType joint2 = null) {
		GameObject bone = (GameObject) Instantiate(prefab, Vector3.zero, Quaternion.identity);
		bone.name = name; bone.transform.parent = body;
		BoneScript script = bone.getComponent("BoneScript") as BoneScript;
		script.radius = radius;
		script.joint1 = body.FindChild (joint1.ToString ()).gameObject;
		if (joint2 != null)
			script.joint2 = body.FindChild (joint2.ToString ()).gameObject;
		else if (_BoneMap.ContainsKey (joint1))
			script.joint2 = body.FindChild (_BoneMap [joint1].ToString ()).gameObject;
		else
			Debug.LogError ("BAD JOINT: " + joint1);
		return bone;
	}
	
	private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject) {
		for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++) {
			Kinect.Joint sourceJoint = body.Joints[jt];
			Transform jointObj = bodyObject.transform.FindChild(jt.ToString());
			jointObj.localPosition = GetVector3FromJoint(sourceJoint);
			if (jt == Kinect.JointType.Head)
				Camera.main.transform.position = (Vector3) jointObj.transform.position;
		}
	}
	
	private static Vector3 GetVector3FromJoint(Kinect.Joint joint) {
		return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
	}
}
