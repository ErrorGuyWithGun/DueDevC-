using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks         //для OnPlayerLeftRoom и OnLeftRoom
{
    private Player _player;                                     //Объект обозначающий игрока

    [SerializeField] TMP_Text playerName;                       //Для получения имени игрока

    public void SetUp(Player player)                            //Для установки имени игрока
    {                                                               //
        _player = player;                                           //
        playerName.text = player.NickName;                          //
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)   //Когда игрок покидает комнату
    {                                                               //
        if(_player == otherPlayer)                                  //Если игрок покидает комнату
            Destroy(gameObject);                                    //Уничтожаем текст обозначающий игрока
    }                                                               //

    public override void OnLeftRoom()   //Если кто-то другой выходит из игры
    {                                       //
        Destroy(gameObject);                //То удаляем имя что совпадает с именем того кто вышел
    }                                       //
}
