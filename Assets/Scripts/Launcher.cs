using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    
    void Start()
    {
        Debug.Log("Connecting to master");
        PhotonNetwork.ConnectUsingSettings(); //Photon Server Settingste belirtti�imiz kurallara g�re Photon un master server�na ba�lan�rs
    }

    public override void OnConnectedToMaster() //Master servera ba�ar�l� �ekilde ba�lan�ld���nda �al���r.
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();//Oda bulma ya da oda olu�turmay� lobide yapar�z.
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
