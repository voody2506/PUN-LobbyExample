using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
public class ButtonClick : MonoBehaviourPunCallbacks
{
    PhotonView photonView;
    public Text ClientCount;
    public Text HostCount;
    int HostCNT = 0;
    int ClientCNT = 0;

    bool winnerDetected = false;

    //ToString()
    // Start is called before the first frame update
    void Start()
    {
      photonView = PhotonView.Get(this);

    }

    // Update is called once per frame
    void Update()
    {
      if (PhotonNetwork.IsMasterClient && winnerDetected == false)
      {
        if (HostCNT == 5){
          PlayerPrefs.SetString("Result", "Winner");
          PhotonNetwork.LoadLevel("FinishScene");
          winnerDetected = true;
        }

        if (ClientCNT == 5){
          PlayerPrefs.SetString("Result", "Lose");
          PhotonNetwork.LoadLevel("FinishScene");
          winnerDetected = true;
        }

      }

    }



    public void Click(){
      if (PhotonNetwork.IsMasterClient)
      {
        HostCNT = HostCNT + 1;
          photonView.RPC("HostAdd", RpcTarget.All,HostCNT);
      }else{
        ClientCNT = ClientCNT + 1;
        photonView.RPC("ClientAdd", RpcTarget.All,ClientCNT);
      }
    }

    [PunRPC]
    void HostAdd(int a){
      Debug.Log(a);
      HostCount.text = "Host: " + a.ToString();
    }

    [PunRPC]
    void ClientAdd(int a){
      Debug.Log(a);
        ClientCount.text = "Client: " + a.ToString();


    }
}
