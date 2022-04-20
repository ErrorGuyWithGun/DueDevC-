using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text roomName;

    private RoomInfo _info;

    public void SetUp(RoomInfo roomInfo)
    {
        _info = roomInfo;
        roomName.text = _info.Name;
    }

    public void OnClick()
    {
        Launcher.instance.JoinRoom(_info);
    }
}
