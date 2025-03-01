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
        PhotonNetwork.ConnectUsingSettings(); //Photon Server Settingste belirttiðimiz kurallara göre Photon un master serverýna baðlanýrs
    }

    public override void OnConnectedToMaster() //Master servera baþarýlý þekilde baðlanýldýðýnda çalýþýr.
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();//Oda bulma ya da oda oluþturmayý lobide yaparýz.
        PhotonNetwork.AutomaticallySyncScene = true; // Host sahne deðiþtirdiðinde (oyunu açtýðýnda) clientlar (diðer oyuncular) için de sahneyi deðiþtiriyor.
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

    public override void OnJoinedRoom() // Oda oluþturduðumuzda ya da odaya katýldýðýmýzda çalýþýr.
    {
        MenuManager.Instance.OpenMenu("room menu");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name; //Ýçinde bulunduðumuz odanýn ismi

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient); //Odanýn kurucusuysa oyunu baþlatma butonu aktif deðilse aktif deðil.
    }

    public override void OnMasterClientSwitched(Player newMasterClient) //Eðer master client ayrýlýrsa Master client rolü baþkasýna geçerken çalýþýr.
    { //Metod tüm clientlar için çalýþacak ve hangi client master olduysa onun için Start game butonu aktifleþtirilecek. 
        startGameButton.SetActive(PhotonNetwork.IsMasterClient); 
    }

    public override void OnCreateRoomFailed(short returnCode,string message)
    {
        errorText.text = "Room Creation Failed " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1); //Build penceresinde game sahnesinin indexi 1 idi o yüzde onu parametre geçtik.
        // Bu metod Hem host hem de diðer oyuncular için sahneyi baþlatýyor. Bundan dolayý Unity'nin kendi scene managementýný kullanmýyoruz.
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
