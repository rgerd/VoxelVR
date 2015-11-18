using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodySourceView : MonoBehaviour 
{
    public Material BoneMaterial;
    public GameObject BodySourceManager;
	public GameObject jointPrefab;
	public GameObject bodyPrefab;
    public GameObject bone_prefab;
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
    
    void Update () 
    {
        if (BodySourceManager == null)
        {
            return;
        }
        
        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }
        
        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }
        
        List<ulong> trackedIds = new List<ulong>();
        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
              }
                
            if(body.IsTracked)
            {
                trackedIds.Add (body.TrackingId);
            }
        }
        
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);
        
        // First delete untracked bodies
        foreach(ulong trackingId in knownIds)
        {
            if(!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
				if (trackID == trackingId) {
					trackID = 0;
				}
            }
        }

        foreach (var body in data) {
			if (trackID == 0) {
				trackID = body.TrackingId;
			} 
			if (trackID == body.TrackingId) {
				if (true) {
					if (body == null) {
						continue;

					}
            
					if (body.IsTracked) {
						if (!_Bodies.ContainsKey (body.TrackingId)) {
							_Bodies [body.TrackingId] = CreateBodyObject (body.TrackingId);
						}
                
						RefreshBodyObject (body, _Bodies [body.TrackingId]);
					}
				}
			}
		}
    }
    
    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);

        
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
			GameObject jointObj = GameObject.Instantiate(new GameObject());
			

            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.05f, 0.05f);

            
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;

        }
        Transform bodyTransform = body.transform;
        
        bones = new GameObject[5];
        bones[0] = (GameObject)Instantiate(bone_prefab, Vector3.zero, Quaternion.identity);
        bones[0].name = "LeftArmTop";
        bones[0].transform.parent = body.transform;
        BoneScript script0 = bones[0].GetComponent("BoneScript") as BoneScript;
        script0.joint1 = bodyTransform.FindChild(Kinect.JointType.ShoulderLeft.ToString()).gameObject;
        script0.joint2 = bodyTransform.FindChild(Kinect.JointType.ElbowLeft.ToString()).gameObject;
        script0.radius = 0.5f;

        bones[1] = (GameObject)Instantiate(bone_prefab, Vector3.zero, Quaternion.identity);
        bones[1].name = "LeftArmBottom";
        bones[1].transform.parent = body.transform;
        BoneScript script1 = bones[1].GetComponent("BoneScript") as BoneScript;
        script1.joint1 = bodyTransform.FindChild(Kinect.JointType.ElbowLeft.ToString()).gameObject;
        script1.joint2 = bodyTransform.FindChild(Kinect.JointType.HandLeft.ToString()).gameObject;
        script1.radius = 0.5f;

        bones[2] = (GameObject)Instantiate(bone_prefab, Vector3.zero, Quaternion.identity);
        bones[2].name = "Body";
        bones[2].transform.parent = body.transform;
        BoneScript script2 = bones[2].GetComponent("BoneScript") as BoneScript;
        script2.joint1 = bodyTransform.FindChild(Kinect.JointType.SpineBase.ToString()).gameObject;
        script2.joint2 = bodyTransform.FindChild(Kinect.JointType.SpineShoulder.ToString()).gameObject;
        script2.radius = 1.5f;


        return body;
    }
    
    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
		message = "";
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;
            
            if(_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }
            
            Transform jointObj = bodyObject.transform.FindChild(jt.ToString());
			jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            if(targetJoint.HasValue)
            {
                lr.SetPosition(0, jointObj.localPosition);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
                lr.SetColors(GetColorForState (sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));

            }
            else
            {
                lr.enabled = false;
            }
			float xval = (int) ((jointObj.position.x)* 10000)/10000f;
			float yval = (int) ((jointObj.position.y) * 10000)/10000f;
			float zval = (int) ((jointObj.position.z) * 10000)/10000f;
			message += xval.ToString() + " " + yval.ToString() + " " + zval.ToString() + " ";
	
			if (jt == Kinect.JointType.Head) {
				camera.transform.position = jointObj.transform.position + new Vector3(0, 0, 0);
			}
        }
    }
    
    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
        case Kinect.TrackingState.Tracked:
            return Color.green;

        case Kinect.TrackingState.Inferred:
            return Color.red;

        default:
            return Color.black;
        }
    }
    
    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

	public string getString() {
		return message;
	}
	
}
