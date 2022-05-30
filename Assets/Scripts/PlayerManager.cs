using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using System.IO;

public class PlayerManager : MonoBehaviour
{

    private PhotonView _photonView;

    void Start()
    {
        _photonView = GetComponent<PhotonView>();   //Получаем переменную _photonView
        if (_photonView.IsMine)                         //Если игрока
        {                                               //
            CreateController();                         //то спавним Player Controller(Игрока)
        }
    }

    private void CreateController()                                                                    //Спавн Player Controller
    {                                                                                                                       //
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);    //
    }                                                                                                                       //
    
}
