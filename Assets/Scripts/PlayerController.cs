using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private PhotonView _photonView;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _cameraSensitivity = 2f;
    [SerializeField] private float _movementSpeed = 4f;
    [SerializeField] private float _checkJumpRadius = 0.2f;
    [SerializeField] private float _jumpForce = 3f;
    
    
    private float _rotationX;
    

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();
        Cursor.visible = false;                                     //Откл. курсор мыши
        Cursor.lockState = CursorLockMode.Locked;                   // Пока курсор откл. он находится по центру

        if (!_photonView.IsMine)    //Если не наш игрок
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);   //То уничтожает камеру
            Destroy(_rigidbody);                                    //И уничтожает риджитбоди чужого игрока
        }
    }

    private void FixedUpdate() //Проверка:принадлежит ли игрок нам
    {
        if(_photonView.IsMine)
            PlayerMovement();
    }
    
    private void Update()
    {
        if(!_photonView.IsMine) //Если не наш игрок то ничего не делаем
            return;
        
        RotatePlayerRightLeft();
        RotateCameraUpDown();
        if(Input.GetButtonDown("Jump"))
            TryJump();
    }
    
    private void TryJump()                  //Функция прыжка
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position - Vector3.down * 0.5f, _checkJumpRadius);
        
        foreach(Collider collider in colliders)
        {
            if (collider.gameObject != gameObject)
                return;
        }
        
        _rigidbody.AddForce(Vector3.up * _jumpForce,ForceMode.Impulse);
    }
    

    private void PlayerMovement()       //Хотьба
    {
        float h = Input.GetAxis("Horizontal");  //Отвечает за нажатие клавиш A и D
        float v = Input.GetAxis("Vertical");    //Отвечает за нажатие клавиш W и S

        Vector3 movementDir = transform.forward * v + transform.right * h;
        movementDir = Vector3.ClampMagnitude(movementDir, 1f);
        
        _rigidbody.velocity = new Vector3(movementDir.x * _movementSpeed, _rigidbody.velocity.y,
            movementDir.z * _movementSpeed);
    }

    private void RotatePlayerRightLeft()        //Поворачивает модельку и камеру в сторону направления движения мыши влево вправо
    {
        transform.Rotate(Vector3.up, Input.GetAxisRaw("Mouse X") * _cameraSensitivity);     
    }

    private void RotateCameraUpDown()           //Поворачивает модельку и камеру в сторону направления движения мыши вверх вниз
    {
        _rotationX -= _cameraSensitivity * Input.GetAxisRaw("Mouse Y");
        _rotationX = Mathf.Clamp(_rotationX, -75, 75);                                                  //не даёт задать значение меньше -75 и больше 75
        _camera.eulerAngles = new Vector3(_rotationX, _camera.eulerAngles.y, _camera.eulerAngles.z);
    }
}