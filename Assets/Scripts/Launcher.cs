using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] 
    TMP_InputField roomNameInputField;

    [SerializeField]
    TMP_Text errorText;

    [SerializeField]
    TMP_Text roomNameText;

    [SerializeField]
    Transform roomListContent;

    [SerializeField]
    GameObject roomListItemPrefab;

    void Awake()
    {
        Instance = this;
    }

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
        MenuManager.Instance.OpenMenu("title");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom() // Oda oluþturduðumuzda ya da odaya katýldýðýmýzda çalýþýr.
    {
        MenuManager.Instance.OpenMenu("room menu");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name; //Ýçinde bulunduðumuz odanýn ismi
    }

    public override void OnCreateRoomFailed(short returnCode,string message)
    {
        errorText.text = "Room Creation Failed " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }
}
