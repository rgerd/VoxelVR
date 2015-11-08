using UnityEngine;
using System.Collections;

public class MainMenuVik : MonoBehaviour
{

    void Start()
    {
        //PhotonNetwork.logLevel = NetworkLogLevel.Full;

        //Connect to the main photon server. This is the only IP and port we ever need to set(!)
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("v1.0"); // version of the game/demo. used to separate older clients from newer ones (e.g. if incompatible)

        //Load name from PlayerPrefs
        PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999));

        //Set camera clipping for nicer "main menu" background
        Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;

    }

    private string roomName = "myRoom";
    private Vector2 scrollPos = Vector2.zero;

    void OnJoinedLobby()
    {
        if (PhotonNetwork.room != null)
            return; //Only when we're not in a Room

        //if (PhotonNetwork.GetRoomList().Length == 0)
        //{
        //    PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = 2 }, TypedLobby.Default);
        //}
        //else
        //{
		PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { maxPlayers = 2 }, TypedLobby.Default);
        //}
    }
}
