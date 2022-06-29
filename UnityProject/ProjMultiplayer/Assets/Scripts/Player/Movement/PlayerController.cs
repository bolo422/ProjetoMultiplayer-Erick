using UnityEngine;
using Photon.Bolt;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : EntityBehaviour<IPhysicState>
{
    private PlayerMotor _playerMotor;
    private bool _forward;
    private bool _backward;
    private bool _left;
    private bool _right;
    private float _yaw;
    private float _pitch;
    private bool _jump;

    private bool _hold = false;
    private bool _interact = false;
    private bool _doInteract = false;
    private bool _canInteract = false;

    private bool _interactHasCooldown = false;

    BoxSpawner _lastBoxSpawner;
    

    private bool _hasControl = false;

    private float _mouseSensitivity = 5f;

    //private DrunkEffect drunkEffect;

    //private Camera camera;

    public void Awake()
    {
        _playerMotor = GetComponent<PlayerMotor>();
        _lastBoxSpawner = FindObjectOfType<BoxSpawner>();
        //drunkEffect = GetComponentInChildren<DrunkEffect>();
        //camera = GetComponentInChildren<Camera>();
    }

    public override void Attached()
    {
        if (entity.HasControl)
        {
            _hasControl = true;
        }

        Init(entity.HasControl);
        _playerMotor.Init(entity.HasControl);
    }

    public void Init(bool isMine)
    {
        if (isMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
            FindObjectOfType<PlayerSetupController>().SceneCamera.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_hasControl)
            PollKeys();

        if (Input.GetKeyDown(KeyCode.E) && !_interactHasCooldown && _canInteract)
        {
            _interactHasCooldown = true;
            StartCoroutine(IntereactCooldown());
            _doInteract = true;
        }

        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (Cursor.lockState == CursorLockMode.Locked)
        //        Cursor.lockState = CursorLockMode.None;
        //    else
        //        Cursor.lockState = CursorLockMode.Locked;
        //}
    }

    IEnumerator IntereactCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        _interactHasCooldown = false;
    }

    private void PollKeys()
    {
        _forward = Input.GetKey(KeyCode.W);
        _backward = Input.GetKey(KeyCode.S);
        _left = Input.GetKey(KeyCode.A);
        _right = Input.GetKey(KeyCode.D);
        _jump = Input.GetKey(KeyCode.Space);
        _hold = Input.GetKey(KeyCode.Mouse0);
        _interact = _doInteract;

        if(_doInteract)
            _doInteract = false;

        _yaw += Input.GetAxisRaw("Mouse X") * _mouseSensitivity;
        _yaw %= 360f;
        _pitch += -Input.GetAxisRaw("Mouse Y") * _mouseSensitivity;
        _pitch = Mathf.Clamp(_pitch, -85, 85);

        //drunkEffect.pitch = _pitch;
        //camera.transform.localEulerAngles = new Vector3(_pitch, 0f, 0f);
    }

    public override void SimulateController()
    {
        IPlayerCommandInput input = PlayerCommand.Create();
        input.Forward = _forward;
        input.Backward = _backward;
        input.Right = _right;
        input.Left = _left;
        input.Yaw = _yaw;
        input.Pitch = _pitch;
        input.Jump = _jump;
        input.Hold = _hold;
        input.Interact = _interact;
        input.BoxSpawnPosition = _lastBoxSpawner.GetSpawnPosition();
        input.BoxSpawnTag = 1;//(int)_lastBoxSpawner.GetTagColor();

        entity.QueueInput(input);

        _playerMotor.ExecuteCommand(_forward, _backward, _left, _right, _jump, _yaw, _pitch, _hold, _interact, _lastBoxSpawner.GetSpawnPosition(), 1);// (int)_lastBoxSpawner.GetTagColor());
        
    }


    public override void ExecuteCommand(Command command, bool resetState)
    {
        PlayerCommand cmd = (PlayerCommand)command;

        if (resetState)
        {
            _playerMotor.SetState(cmd.Result.Position, cmd.Result.Rotation);
        }
        else
        {
            PlayerMotor.State motorState = new PlayerMotor.State();

            if (!entity.HasControl)
            {
                motorState = _playerMotor.ExecuteCommand(
                cmd.Input.Forward,
                cmd.Input.Backward,
                cmd.Input.Left,
                cmd.Input.Right,
                cmd.Input.Jump,
                cmd.Input.Yaw,
                cmd.Input.Pitch,
                cmd.Input.Hold,
                cmd.Input.Interact,
                cmd.Input.BoxSpawnPosition,
                cmd.Input.BoxSpawnTag);
            }

            cmd.Result.Position = motorState.position;
            cmd.Result.Rotation = motorState.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_hasControl)
        {
            if(other.CompareTag("boxSpawner"))
            {
                if (!other.GetComponent<BoxSpawner>().PlayerCanInteract) return;

                _canInteract = true;
                _lastBoxSpawner = other.GetComponent<BoxSpawner>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_hasControl)
        {
            if (other.CompareTag("boxSpawner"))
            {
                if (!other.GetComponent<BoxSpawner>().PlayerCanInteract) return;

                _canInteract = false;
            }
        }
    }

}