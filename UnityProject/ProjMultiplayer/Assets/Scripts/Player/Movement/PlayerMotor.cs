using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using Photon.Bolt;

public class PlayerMotor : EntityBehaviour<IPhysicState>
{
    [SerializeField]
    private Camera _cam = null;
    [SerializeField]
    private GameObject _HUD = null;
    
    //private List<MeshRenderer> _playerMeshesToHide = new List<MeshRenderer>();

    private NetworkRigidbody _networkRigidbody = null;

    private float _speed = 7f;

    private Vector3 _lastServerPos = Vector3.zero;
    private bool _firstState = true;

    private bool _jumpPressed = false;
    private float _jumpForce = 9f;

    private bool _isGrounded = false;
    private float _maxAngle = 45f;

    [SerializeField]
    private Transform _holdItem;
    private bool _isHolding = false;
    private PickableItem _lastItemHolded = null;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Transform _head;


    private float _pickupDistance = 4.0f; //2.5f;

    private void Awake()
    {
        _networkRigidbody = GetComponent<NetworkRigidbody>();
    }

    public override void Attached()
    {
        state.SetAnimator(_animator);
        state.SetTransforms(state.HeadTransform, _cam.transform);
    }

    public void Init(bool isMine)
    {
        if (isMine)
        {
            _cam.gameObject.SetActive(true);
            _HUD.SetActive(true);

            foreach(Renderer mesh in GetComponentsInChildren<Renderer>())
            {
                mesh.enabled = false;
            }


        }
    }

    public State ExecuteCommand(bool forward, bool backward, bool left, bool right, bool jump, float yaw, float pitch, bool holding)
    {
        Vector3 movingDir = Vector3.zero;
        if (forward ^ backward)
        {
            movingDir += forward ? transform.forward : -transform.forward;
        }
        if (left ^ right)
        {
            movingDir += right ? transform.right : -transform.right;
        }

        if (jump)
        {
            if (_jumpPressed == false && _isGrounded)
            {
                _isGrounded = false;
                _jumpPressed = true;
                _networkRigidbody.MoveVelocity += Vector3.up * _jumpForce;
            }
        }
        else
        {
            if (_jumpPressed)
                _jumpPressed = false;
        }

#region Holding objects
        if (holding && !_isHolding)
        {
            Ray RayOrigin;
            RaycastHit hit;
            Transform cameraTransform = _cam.transform;

            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * _pickupDistance, Color.yellow);
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, _pickupDistance))
            {
                _HUD.GetComponentInChildren<Text>().text = hit.collider.name;

                if(hit.collider.GetComponent<PickableItem>() != null)
                {
                    hit.collider.GetComponent<PickableItem>().Hold(_holdItem, true);
                    _isHolding = true;
                    _lastItemHolded = hit.collider.GetComponent<PickableItem>();
                }
            }
        }

        if(!holding && _isHolding)
        {
            _lastItemHolded.Hold(_holdItem, false);
            _isHolding = false;
        }
#endregion

        movingDir.Normalize();
        movingDir *= _speed;
        _networkRigidbody.MoveVelocity = new Vector3(movingDir.x, _networkRigidbody.MoveVelocity.y, movingDir.z);

        _cam.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
        transform.rotation = Quaternion.Euler(0, yaw, 0);

        State stateMotor = new State();
        stateMotor.position = transform.position;
        stateMotor.rotation = yaw;

        #region move animation

        state.isMoving = movingDir != Vector3.zero;

        #endregion



        return stateMotor;
    }

    private void FixedUpdate()
    {

        if (state.isMoving)
        {
            state.Animator.Play("DrunkRun");
        }
        else
        {
            state.Animator.Play("DrunkIdle");
        }

        if(!entity.HasControl)
        {
            
        }

        _head.rotation = state.HeadTransform.Rotation;

        


        if (entity.IsAttached)
        {
            if (entity.IsControllerOrOwner)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.3f))
                {
                    float slopeNormal = Mathf.Abs(Vector3.Angle(hit.normal, new Vector3(hit.normal.x, 0, hit.normal.z)) - 90) % 90;

                    if (_networkRigidbody.MoveVelocity.y < 0)
                        _networkRigidbody.MoveVelocity = Vector3.Scale(_networkRigidbody.MoveVelocity, new Vector3(1, 0, 1));

                    if (!_isGrounded && slopeNormal <= _maxAngle)
                    {
                        _isGrounded = true;
                    }
                }
                else
                {
                    if (_isGrounded)
                    {
                        _isGrounded = false;
                    }
                }
            }
        }
    }

    public void SetState(Vector3 position, float rotation)
    {
        if (Mathf.Abs(rotation - transform.rotation.y) > 5f)
            transform.rotation = Quaternion.Euler(0, rotation, 0);

        if (_firstState)
        {
            if (position != Vector3.zero)
            {
                transform.position = position;
                _firstState = false;
                _lastServerPos = Vector3.zero;
            }
        }
        else
        {
            if (position != Vector3.zero)
            {
                _lastServerPos = position;
            }

            transform.position += (_lastServerPos - transform.position) * 0.5f;
        }
    }

    public struct State
    {
        public Vector3 position;
        public float rotation;
    }
}