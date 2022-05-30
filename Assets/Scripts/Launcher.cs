using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

//                              ┌─────────────────┐
//                              │ Launcher.cs     │
//             ┌────────────────┼─────────────────┤
//             │TMP_InputField  │_roomInputField  ├───────────────────────────────────────────┐
//             ├────────────────┼─────────────────┤                                           │
//         ┌───┤      TMP_Text  │_errorText       ├───────────────────────────────────────────┼─┐
//         │П  ├────────────────┼─────────────────┤                                           │ │
//         │о  │      TMP_Text  │_roomNameText    ├─────────────────────────────────────────┐ │ │
//         │л  ├────────────────┼─────────────────┤                                         │ │ │
//         │я  │      Transform │_roomList        ├───────────────────────────────────────┐ │ │ │
//         │   ├────────────────┼─────────────────┤                                       │ │ │ │
//         │К  │      GameObject│_roomButtonPrefab├─────────────────────────────────────┐ │ │ │ │
//         │л  ├────────────────┼─────────────────┤                                     │ │ │ │ │
//         │а  │      Transform │_playerList      ├─────────────────────────────────┬─┐ │ │ │ │ │
//         │с  ├────────────────┼─────────────────┤                                 │ │ │ │ │ │ │
//         │с  │     GameObject │_playerNamePrefab├─────────────────────────────────┼─┼─┼─┼─┼─┼─┼──┐
//         │а  ├────────────────┼─────────────────┤                                 │ │ │ │ │ │ │  │
//         │   │      GameObject│_startGameButton ├──────────────────────────────┬──┼─┼─┼─┼─┼─┼─┼──┼────────┐
//     ┌───┴───┴────────────┬───┴─────────────────┴──────────────────────────────┤  │ │ │ │ │ │ │  │        │
//     │        private void│Start()                                             │  │ │ │ │ │ │ │  │        │
//     ├────────────────────┼────────────────────────────────────────────────────┤  │ │ │ │ │ │ │  │        │
// ┌───┤public override void│OnConnectedToMaster()                               │  │ │ │ │ │ │ │  │        │
// │М  ├────────────────────┼────────────────────────────────────────────────────┤  │ │ │ │ │ │ │  │        │
// │е  │public override void│OnJoinedLobby()                                     │  │ │ │ │ │ │ │  │        │
// │т  ├────────────────────┼────────────────────────────────────────────────────┤  │ │ │ │ │ │ │  │        │
// │о  │         public void│StartGame()                                         │  │ │ │ │ │ │ │  │        │
// │д  ├────────────────────┼────────────────────────────────────────────────────┤  │ │ │ │ │ │ │  │        │
// │ы  │         public void│CreateRoom()                                        │◄─┼─┼─┼─┼─┼─┘ │  │        │
// │   ├────────────────────┼────────────────────────────────────────────────────┤  │ │ │ │ │   │  │        │
// │К  │public override void│OnJoinedRoom()                                      │◄─▼─┼─┼─┼─┘◄──┼──┤◄───────┤
// │л  ├────────────────────┼────────────────────────────────────────────────────┤    │ │ │     │  │        │
// │а  │public override void│OnMasterClientSwitched(Player newMasterClient)      │◄───┼─┼─┼─────┼──┼────────┘
// │с  ├────────────────────┼────────────────────────────────────────────────────┤    │ │ │     │  │
// │с  │public override void│OnCreateRoomFailed(short returnCode, string message)│◄───┼─┼─┼─────┘  │
// │а  ├────────────────────┼────────────────────────────────────────────────────┤    │ │ │        │
// │   │         public void│LeaveRoom()                                         │    │ │ │        │
// └───┼────────────────────┼────────────────────────────────────────────────────┤    │ │ │        │
//     │public override void│OnLeftRoom()                                        │    │ │ │        │
//     ├────────────────────┼────────────────────────────────────────────────────┤    │ │ │        │
//     │         public void│JoinRoom(RoomInfo info)                             │    │ │ │        │
//     ├────────────────────┼────────────────────────────────────────────────────┤    │ │ │        │
//     │public override void│OnRoomListUpdate(List<RoomInfo> roomList)           │◄───┼─┴─┘        │
//     ├────────────────────┼────────────────────────────────────────────────────┤    │            │
//     │public override void│OnPlayerEnteredRoom(Player player)                  │◄───┘◄───────────┘
//     └────────────────────┴────────────────────────────────────────────────────┘

public class Launcher : MonoBehaviourPunCallbacks
{

    public static Launcher instance;

    [SerializeField] private TMP_InputField _roomInputField;      //Поля для ввода имени комнаты 
                                                                    // которое мы используем в CreateRoom()
    [SerializeField] private TMP_Text _errorText;                 //Текст ошибки
                                                                    // которое мы используем в OnCreateRoomFailed()
    [SerializeField] private TMP_Text _roomNameText;              //Имя комнаты
                                                                    // которое мы используем в OnJoinedRoom()
    [SerializeField] private Transform _roomList;                 //Для обновления списка комнат
                                                                    //
    [SerializeField] private GameObject _roomButtonPrefab;        //Для добавления комнат в Мастер сервер и Лобби
                                                                    //Влаживается сам объект "кнопка"
    [SerializeField] private Transform _playerList;               //Для обновления списка игроков
                                                                    //
    [SerializeField] private GameObject _playerNamePrefab;        //Для добавления игроков в Мастер сервер и Лобби
                                                                    //
    [SerializeField] private GameObject _startGameButton;         //Кнопка для начала игры (только для хоста)
                                                                  
                        /*                  Start is called before the first frame update                  */
    private void Start()                            //Подключит к Мастер серверу
    {                                                   //
        instance = this;
        Debug.Log("Will join in master server");        //Выводит "Will join in master server" когда функция заработала
        PhotonNetwork.ConnectUsingSettings();           //
        MenuManager.instance.OpenMenu("loading");       //Открывает меню загрузки пока выполняется функция
    }                                                   //

    public override void OnConnectedToMaster()      //Подключились к Мастер серверу
    {                                                   //
        Debug.Log("Join in master server");             //Выводит "Join in master server" когда функция заработала
        PhotonNetwork.JoinLobby();                      //
        PhotonNetwork.AutomaticallySyncScene = true;    //Если начинает игру один, то начинают игру все. 
    }                                                   //Подобная привилегия должна оставатся только хосту.
                                                        
    public override void OnJoinedLobby()                                                        //Подключились к Лобби
    {                                                                                               //
                                                                                                    //
        Debug.Log("Join in lobby");                                                                 //Выводит "Join in lobby" когда функция заработала
        MenuManager.instance.OpenMenu("title");                                                     //Открывает главное меню
        PhotonNetwork.NickName = "Player" + UnityEngine.Random.Range(0, 1000).ToString("0000");     //Генерация имён для игроков начинается с Player
                                                                                                    // и заканчивается 4-мя случайными числами
    }

    public void StartGame()                                //Функция запускающая сцену с игрой
    {                                                               //
        PhotonNetwork.LoadLevel(1);                                 //Функция в Photon для загрузки сцены с определённым id
    }                                                               //Но это будет для одного игрока. AutomaticallySyncScene это поправляет.

    public void CreateRoom()                               //Функция создания комнаты
    {                                                               //
        if(string.IsNullOrEmpty(_roomInputField.text))              //Проверяем не пустое ли название
        {                                                           //
            return;                                                 // Если пустое то return
        }                                                           //
        PhotonNetwork.CreateRoom(_roomInputField.text);             //Встроенная функция в Photon для создания комнаты
        MenuManager.instance.OpenMenu("loading");                   //Открывает меню загрузки пока выполняется функция
    }                                                               //
    
    public override void OnJoinedRoom()                         //Функция срабатывает при успешном создании комнаты
    {                                                               //
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;        //Изменение названия комнаты
        MenuManager.instance.OpenMenu("room");                      //Перейти в меню комнаты
                                                                    //
        Player[] players = PhotonNetwork.PlayerList;                //Извлекаем массив игроков из PlayerList в переменную
                                                                    //Производим очистку перед добавлением ник неймов//
        for(int i = 0; i<_playerList.childCount; i++)               //Проходимся по всем игрокам что есть
        {                                                           //
            Destroy(_playerList.GetChild(i).gameObject);            //Удаляем объекты с ник неймом из списка
        }                                                           //
                                                                    //
        for(int i = 0; i < players.Length; i++)                     //Проходимся по всем игрокам что есть
        {                                                           //
            Instantiate(_playerNamePrefab, _playerList).GetComponent<PlayerListItem>().SetUp(players[i]); 
                                                                    // Создаем объект с ник неймом игрока (самих игроков)
        }                                                           
                                                                    //
        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);   //Если мы MasterClient(хост) то ты можешь начать игру 
    }

    public override void OnMasterClientSwitched(Player newMasterClient)  //Если хост выходит, то и кнопка старта должна обновиться для игроковю.
    {                                                                        //Кнопка становится доступной для игрока что первее остальных подключился
        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);            //start game FOR NEW HOST
    }                                                                        //

    public override void OnCreateRoomFailed(short returnCode, string message)   //Функция срабатывает если создание комнаты провалено
    {                                                                               //
        _errorText.text = "Error" + message;                                        //Форма текста самой ошибки
        MenuManager.instance.OpenMenu("error");                                     //Перейти в меню ошибки
                                                                                    //if error open - error menu
    }

    public void LeaveRoom()                             //Функция для выхода из комнаты
    {                                                       //
        PhotonNetwork.LeaveRoom();                          //Говорит Photon серверу что игрок покинул игру
        MenuManager.instance.OpenMenu("loading");           //Открывает меню загрузки пока выполняется функция
    }                                                       // (поставили функцию на кнопку Leave Room)

    public override void OnLeftRoom()                   //Срабатывает после LeaveRoom()
    {                                                       //
        MenuManager.instance.OpenMenu("title");             //Открывает главное меню
    }                                                       //

    public void JoinRoom(RoomInfo info)                 //Функция подключения к комнате
    {                                                       //
        PhotonNetwork.JoinRoom(info.Name);                  //Photon функция для подключения
        MenuManager.instance.OpenMenu("loading");           //Открывает меню загрузки пока выполняется функция
    }                                                       //(используется в RoomListItem.cs OnClick)

    public override void OnRoomListUpdate(List<RoomInfo> roomList)  //Вызывается каждый раз когда создается комната
    {                                                                   // или кто-то присоединяется
                                                                        //
        for (int i = 0; i < _roomList.childCount; i++)                  //Проходим по всем объектам списка
        {                                                               //
            Destroy(_roomList.GetChild(i).gameObject);                  //Удаляем кнопки из списка
        }                                                               //
                                                                        //
        for (int i = 0; i < roomList.Count; i++)                        //Проходимся по всем комнатам что есть
        {                                                               //
            if(roomList[i].RemovedFromList)                             //Если комната полная/скрытая то не будет показываться в комнате
                continue;                                               //
            Instantiate(_roomButtonPrefab, _roomList).GetComponent<RoomListItem>().SetUp(roomList[i]);
                        /*                  Создаёт кнопку                  */
                        /*         и получает компанент room List Item      */
        }
         
    }
    public override void OnPlayerEnteredRoom(Player player)                                         //Спавним префаб имени в список всех игроков
    {                                                                                               //
        Instantiate(_playerNamePrefab, _playerList).GetComponent<PlayerListItem>().SetUp(player);   //Используем функцию SetUp (PlayerListItem.cs)
    }                                                                                               //
}                                                                                                   //
