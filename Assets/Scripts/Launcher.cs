using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    
    void Start()
    {
        Debug.Log("Connecting to master");
        PhotonNetwork.ConnectUsingSettings(); //Photon Server Settingste belirttiðimiz kurallara göre Photon un master serverýna baðlanýrs
    }

    public override void OnConnectedToMaster() //Master servera baþarýlý þekilde baðlanýldýðýnda çalýþýr.
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();//Oda bulma ya da oda oluþturmayý lobide yaparýz.
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
