using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
public class PhotonMaster : MonoBehaviourPunCallbacks
{

  public Text connectStatus;
  RoomOptions roomOptions = new RoomOptions();
  private string roomName;
  public InputField serverName;
  private bool connected = false;

  // Start is called before the first frame update
  void Start()
  {
    PhotonNetwork.ConnectUsingSettings();
  }

  // Update is called once per frame


  public override void OnConnectedToMaster()
  {
    Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
    PhotonNetwork.AutomaticallySyncScene = true;

    roomOptions.MaxPlayers = 2;

    connectStatus.text = "Connected To Photon Master";
    connectStatus.color = new Color(58.0f/255.0f, 255.0f/255.0f, 4.0f/255.0f);
    connected = true;

  }


  public void ConnectByName(){
    if (connected == true ){
      if (serverName.text != "" && serverName.text != null){
        PhotonNetwork.JoinOrCreateRoom(serverName.text, roomOptions, TypedLobby.Default);
        roomName = serverName.text;
      }else{
        connectStatus.text = "Null TextField";
      }
    }else{
      connectStatus.text = "Wait For connection";
    }
  }



  public void ConnectRandom(){
    if (connected == true){
      PhotonNetwork.JoinRandomRoom();
    }else{
      connectStatus.text = "Wait For connection";
    }
  }

  public override void OnJoinRandomFailed(short returnCode, string message)
  {
    Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
    // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
    roomName = System.Guid.NewGuid().ToString();
    PhotonNetwork.CreateRoom(roomName, roomOptions);
  }



  public override void OnJoinedRoom()
  {
    Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
    connectStatus.text = "Joined to room";

  }



  public override void OnPlayerEnteredRoom(Player other)
  {
    Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


    if (PhotonNetwork.IsMasterClient)
    {
      Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


      LoadArena();
    }
  }

  public override void OnPlayerLeftRoom(Player other)
  {
    Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


    if (PhotonNetwork.IsMasterClient)
    {
      Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


      LoadArena();
    }
  }


  void LoadArena()
  {
    if (!PhotonNetwork.IsMasterClient)
    {
      Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
    }
    Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
    PhotonNetwork.LoadLevel("GamePlayScene");
  }



}
