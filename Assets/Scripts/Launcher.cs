using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{

    public static Launcher instance;

    [SerializeField] private TMP_InputField _roomInputField;//room name

    [SerializeField] private TMP_Text _errorText;

    [SerializeField] private TMP_Text _roomNameText;

    [SerializeField] private Transform _roomList;

    [SerializeField] private GameObject _roomButtonPrefab;

    [SerializeField] private Transform _playerList;

    [SerializeField] private GameObject _playerNamePrefab;

    [SerializeField] private GameObject _startGameButton;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
        Debug.Log("Will join in master server");
        PhotonNetwork.ConnectUsingSettings();
        MenuManager.instance.OpenMenu("loading");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Join in master server");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true; // if use one player start game - all player start game
    }

    public override void OnJoinedLobby()
    {
        
        Debug.Log("Join in lobby");
        MenuManager.instance.OpenMenu("title");
        PhotonNetwork.NickName = "Player" + UnityEngine.Random.Range(0, 1000).ToString("0000");
        
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1); //loading level with 1 id
    }

    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(_roomInputField.text))
        {
            return; // if not name - return
        }
        PhotonNetwork.CreateRoom(_roomInputField.text);
        MenuManager.instance.OpenMenu("loading");
    }
    
    public override void OnJoinedRoom()
    {
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        MenuManager.instance.OpenMenu("room");

        Player[] players = PhotonNetwork.PlayerList;

        for(int i = 0; i<_playerList.childCount; i++)
        {
            Destroy(_playerList.GetChild(i).gameObject);
        }

        for(int i = 0; i < players.Length; i++)
        {
            Instantiate(_playerNamePrefab, _playerList).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        _startGameButton.SetActive(PhotonNetwork.IsMasterClient); //start game ONLY FOR HOST
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _startGameButton.SetActive(PhotonNetwork.IsMasterClient); //start game FOR NEW HOST
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _errorText.text = "Error" + message;
        MenuManager.instance.OpenMenu("error");
        //if error open - error menu
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("title");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("loading");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        for (int i = 0; i < _roomList.childCount; i++)
        {
            Destroy(_roomList.GetChild(i).gameObject);
        }

        for (int i = 0; i < roomList.Count; i++) 
        {
            if(roomList[i].RemovedFromList)
                continue;
            Instantiate(_roomButtonPrefab, _roomList).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
         
    }
    public override void OnPlayerEnteredRoom(Player player)
    {
        Instantiate(_playerNamePrefab, _playerList).GetComponent<PlayerListItem>().SetUp(player);
    }
}
