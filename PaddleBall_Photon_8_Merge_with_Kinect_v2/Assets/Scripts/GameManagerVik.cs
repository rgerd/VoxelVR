using UnityEngine;
using System.Collections;

public class GameManagerVik : Photon.MonoBehaviour {

    // this is a object name (must be in any Resources folder) of the prefab to spawn as player avatar.
    // read the documentation for info how to spawn dynamically loaded game objects at runtime (not using Resources folders)
    public string playerPrefabName = "Sphere";

    void OnJoinedRoom()
    {
        Debug.Log("HELLO WORLD");
        Camera.main.farClipPlane = 1000; //Main menu set this to 0.4 for a nicer BG    

        if (PhotonNetwork.countOfPlayers == 2)
        {
            OSCReceiver.playerID = "P2";
			OSCReceiver.anchor = new Vector3(0, 2, -30);
        }
        else
        {
            OSCReceiver.playerID = "P1";
			OSCReceiver.anchor = new Vector3(0, 2, 0);
			Camera.main.transform.Rotate(0, 180, 0);
        }

        Debug.Log("PLAYER ID " + OSCReceiver.playerID);
        GameObject head = PhotonNetwork.Instantiate("Head", new Vector3(0, 0, 0), Quaternion.identity, 0);
        head.gameObject.tag = OSCReceiver.playerID + "Head";
        OSCReceiver.head = head;

        GameObject body = PhotonNetwork.Instantiate("Body", new Vector3(0, 0, 0), Quaternion.identity, 0);
        body.gameObject.tag = OSCReceiver.playerID + "Body";
        OSCReceiver.body = body;


        GameObject leftHand = PhotonNetwork.Instantiate("LeftHand", new Vector3(0, 0, 0), Quaternion.identity, 0);
        leftHand.gameObject.tag = OSCReceiver.playerID + "LeftHand";
        OSCReceiver.leftHand = leftHand;

        GameObject rightHand = PhotonNetwork.Instantiate("RightHand", new Vector3(0, 0, 0), Quaternion.identity, 0);
        rightHand.gameObject.tag = OSCReceiver.playerID + "RightHand";
        OSCReceiver.rightHand = rightHand;

        GameObject leftElbow = PhotonNetwork.Instantiate("LeftElbow", new Vector3(0, 0, 0), Quaternion.identity, 0);
        leftElbow.gameObject.tag = OSCReceiver.playerID + "LeftElbow";
        OSCReceiver.leftElbow = leftElbow;

        GameObject rightElbow = PhotonNetwork.Instantiate("RightElbow", new Vector3(0, 0, 0), Quaternion.identity, 0);
        rightElbow.gameObject.tag = OSCReceiver.playerID + "RightElbow";
        OSCReceiver.rightElbow = rightElbow;
    }
    
    IEnumerator OnLeftRoom()
    {
        //Easy way to reset the level: Otherwise we'd manually reset the camera

        //Wait untill Photon is properly disconnected (empty room, and connected back to main server)
        while(PhotonNetwork.room!=null || PhotonNetwork.connected==false)
            yield return 0;

        Application.LoadLevel(Application.loadedLevel);

    }




    void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton");
    }    
}
