using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    public float mouseSensitivity {get; set;}

    [SerializeField]
    private Transform _playerHead;

    [SerializeField]
    private Transform _playerBody;

    private float _xRotation = 0f;

    void Awake()
    {
        if(mouseSensitivity == 0)
            mouseSensitivity = 200f;    // TODO: transformar estes 200f em uma variavel global?
                                        //TODO: Associate with "Scripts/Player/Movement/PlayerControlle:_mouseSensistivty"
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _playerHead.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseX);
    }

    void SetMouseSensitivity(float value)
    {
        mouseSensitivity = value;
    }
}
