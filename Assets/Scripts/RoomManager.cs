using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;         //Singleton

    void Start()                            //Удаляет персонажа если он уходит из комнаты
    {                                           //
        if(instance)                            //
        {                                       //
            Destroy(gameObject);                //Уничтоженик gameObject и возвращение значения
            return;                             //
        }                                       //
        DontDestroyOnLoad(gameObject);          //После смены игровой сцены room manager должен остаться на Game сцене
        instance = this;                        //Привязываем instance к gameObject
    }                                           //

    public override void OnEnable()                 //
    {                                                   //
        base.OnEnable();                                //
        SceneManager.sceneLoaded += OnSceneLoaded;      //
    }                                                   //

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)                //Для загрузки сцены
    {                                                                                       //
        if(scene.buildIndex == 1)                                                           //Проверяем, перешли ли мы на сцену id 1
        {                                                                                   //
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }                                                                                   //Если так, то спавним Player Manager
    }                                                                                           

    public override void OnDisable()        //
    {                                           //
        base.OnDisable();                       //
    }                                           //
}
