using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

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
    Transform playerListContent;

    [SerializeField]
    GameObject roomListItemPrefab;

    [SerializeField]
    GameObject PlayerListItemPrefab;

    [SerializeField]
    GameObject startGameButton;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log("Connecting to master");
        PhotonNetwork.ConnectUsingSettings(); //Photon Server Settingste belirtti�imiz kurallara g�re Photon un master server�na ba�lan�rs
    }

    public override void OnConnectedToMaster() //Master servera ba�ar�l� �ekilde ba�lan�ld���nda �al���r.
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();//Oda bulma ya da oda olu�turmay� lobide yapar�z.
        PhotonNetwork.AutomaticallySyncScene = true; // Host sahne de�i�tirdi�inde (oyunu a�t���nda) clientlar (di�er oyuncular) i�in de sahneyi de�i�tiriyor.
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        MenuManager.Instance.OpenMenu("title");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
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

    public override void OnJoinedRoom() // Oda olu�turdu�umuzda ya da odaya kat�ld���m�zda �al���r.
    {
        MenuManager.Instance.OpenMenu("room menu");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name; //��inde bulundu�umuz odan�n ismi

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient); //Odan�n kurucusuysa oyunu ba�latma butonu aktif de�ilse aktif de�il.
    }

    public override void OnMasterClientSwitched(Player newMasterClient) //E�er master client ayr�l�rsa Master client rol� ba�kas�na ge�erken �al���r.
    { //Metod t�m clientlar i�in �al��acak ve hangi client master olduysa onun i�in Start game butonu aktifle�tirilecek. 
        startGameButton.SetActive(PhotonNetwork.IsMasterClient); 
    }

    public override void OnCreateRoomFailed(short returnCode,string message)
    {
        errorText.text = "Room Creation Failed " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1); //Build penceresinde game sahnesinin indexi 1 idi o y�zde onu parametre ge�tik.
        // Bu metod Hem host hem de di�er oyuncular i�in sahneyi ba�lat�yor. Bundan dolay� Unity'nin kendi scene management�n� kullanm�yoruz.
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
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

}
