using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    //                      ┌─────────┐
    //                      │ Menu.cs │
    // ┌───┬────────────────┼─────────┤
    // │П  │public RoomInfo │info     │─────────────────┐
    // │о  ├────────────────┼─────────┤                 │
    // │л  │private TMP_Text│roomName │                 │
    // │я  ├────────────────┴─────────┘                 │
    // │   │                                            │
    // │К  │                                            │
    // │л  │                                            │
    // │а  │                                            │
    // │с  │                                            │
    // │с  │                                            │
    // │а  │                                            │
    // │   │                                            │
    // ├───┼────────────────┬────────────────────────┐  │
    // │М  │public void     │SetUp(RoomInfo roomInfo)│◄─┘
    // │е  ├────────────────┼────────────────────────┤
    // │т  │public void     │Close()                 │
    // │о  ├────────────────┴────────────────────────┘
    // │д  │
    // │ы  │
    // │   │
    // │К  │
    // │л  │
    // │а  │
    // │с  │
    // │с  │
    // │а  │
    // │   │
    // └───┘
    [SerializeField] private TMP_Text roomName; //Текст кнопки 
                                                    // что меняем в функции SetUp
    public RoomInfo info;                       //Класс с информацией о комнате в которой находится игрок
                                                    //Берём название комнаты
    public void SetUp(RoomInfo roomInfo)        //Меняет название комнаты
    {                                               //
        info = roomInfo;                            //информация по комнате
        roomName.text = info.Name;                  //переопределяем новое имя комнаты
    }                                               //
                                                    //
    public void OnClick()                       //Функция для ивентного клика нашей функции
    {                                               //
        Launcher.instance.JoinRoom(info);           //Когда нажимаем на комнату
    }                                               //переходим в комнату
                                                    //
}
