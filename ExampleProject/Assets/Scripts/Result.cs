using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
public class Result : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    PhotonView photonView;
    string ResultValue;
    public Text ResultText;


    void Start()
    {
      photonView = PhotonView.Get(this);

      if (PhotonNetwork.IsMasterClient)
      {
      ResultValue = PlayerPrefs.GetString("Result", "No Result");

      if (ResultValue == "Winner"){
        ResultText.text = ResultValue;
        photonView.RPC("HostResult", RpcTarget.Others,"Lose");

      }

      if (ResultValue == "Lose"){
        ResultText.text = ResultValue;
        photonView.RPC("HostResult", RpcTarget.Others,"Winner");
      }
    }

    }

    [PunRPC]
    void HostResult(string a){
      ResultText.text = a;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
