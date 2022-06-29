using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using Photon.Bolt;
using UnityEngine.Animations.Rigging;

public class PlayerMotor : EntityBehaviour<IPhysicState>
{
    [SerializeField]
    private GameObject head = null;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject _HUD = null;
    
    //private List<MeshRenderer> _playerMeshesToHide = new List<MeshRenderer>();

    private NetworkRigidbody _networkRigidbody = null;

    private float _speed = 6f;

    private Vector3 _lastServerPos = Vector3.zero;
    private bool _firstState = true;

    private bool _jumpPressed = false;
    private float _jumpForce = 5f;

    private bool _isGrounded = false;
    private float _maxAngle = 45f;

    [SerializeField]
    private Transform _holdItem;
    private bool _isHolding = false;
    private PickableItem _lastItemHolded = null;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Transform _headAim;

    private float _pickupDistance = 4.0f; //2.5f;

    [SerializeField]
    private Rig armsRig;

    bool _interactCooldown = true;

    private void Awake()
    {
        _networkRigidbody = GetComponent<NetworkRigidbody>();
    }

    public override void Attached()
    {
        state.SetAnimator(_animator);
        state.isHolding = false;
    }

    public void Init(bool isMine)
    {
        if (isMine)
        {
            head.SetActive(true);
            _HUD.SetActive(true);

            foreach(Renderer mesh in GetComponentsInChildren<Renderer>())
            {
                mesh.enabled = false;
            }


        }
    }

    public State ExecuteCommand(bool forward, bool backward, bool left, bool right, bool jump, float yaw, float pitch, bool holding, bool interact, Vector3 boxSpawnPosition, int boxSpawnTag)
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

        Jump(jump);

        HoldObejct(holding);

        movingDir.Normalize();
        movingDir *= _speed;
        _networkRigidbody.MoveVelocity = new Vector3(movingDir.x, _networkRigidbody.MoveVelocity.y, movingDir.z);

        head.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
        transform.rotation = Quaternion.Euler(0, yaw, 0);

        State stateMotor = new State();
        stateMotor.position = transform.position;
        stateMotor.rotation = yaw;

        #region move animation

        state.isMoving = movingDir != Vector3.zero;

        #endregion

        //if(interact)
        //{
        //    BoxSpawner[] interactables = FindObjectsOfType<BoxSpawner>();
        //    List<GameObject> interactablesList = new List<GameObject>();
        //    foreach(BoxSpawner interactable in interactables)
        //    {
        //        interactablesList.Add(interactable.gameObject);
        //    }
        //    BUtils.Closest(transform.position, interactablesList.ToArray(), out int index);
        //    if(interactables[index].HasPlayerInside())
        //    {
        //        Box box = BoltNetwork.Instantiate(BoltPrefabs.Box, interactables[index].GetSpawnPosition(), Quaternion.identity).GetComponent<Box>();
        //        box.CurrentTagColor = interactables[index].GetTagColor();
        //    }

        //}

        if (entity.IsOwner && interact && _interactCooldown)
        {
            BoltEntity box = BoltNetwork.Instantiate(BoltPrefabs.Box, boxSpawnPosition, Quaternion.identity);
            box.GetComponent<Box>().TagColor = (TagColors)boxSpawnTag;
            _interactCooldown = false;
            StartCoroutine(IntereactCooldown());
        }

        //if (interact && _interactCooldown)
        //{
        //    var spawnBoxEvent = SpawnBox.Create();
        //    spawnBoxEvent.Position = boxSpawnPosition;
        //    spawnBoxEvent.TagColor = boxSpawnTag;
        //    spawnBoxEvent.Send();

        //    _interactCooldown = false;
        //    StartCoroutine(IntereactCooldown());

        //}

        return stateMotor;
    }

    IEnumerator IntereactCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        _interactCooldown = true;
    }

    private void HoldObejct(bool holding)
    {
        if (holding && !_isHolding)
        {
            Ray RayOrigin;
            RaycastHit hit;
            Transform cameraTransform = cam.transform;

            int pickableItemLayerIndex = LayerMask.NameToLayer("PickableItemLayer");
            int dontCollideWithPlayersLayerIndex = LayerMask.NameToLayer("DontCollideWithPlayers");
            int layerMask = (1 << pickableItemLayerIndex) | (1 << dontCollideWithPlayersLayerIndex);

            //Debug.DrawRay(cameraTransform.position, cameraTransform.forward * _pickupDistance, Color.yellow);
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, _pickupDistance, layerMask))
            {
               // _HUD.GetComponentInChildren<Text>().text = hit.collider.name;

                if (hit.collider.GetComponent<PickableItem>() != null)
                {
                    hit.collider.GetComponent<PickableItem>().Hold(_holdItem, true);
                    _isHolding = true;
                    _lastItemHolded = hit.collider.GetComponent<PickableItem>();
                    state.isHolding = true;
                }
            }
        }

        if (!holding && _isHolding)
        {
            _lastItemHolded.Hold(_holdItem, false);
            _isHolding = false;
            state.isHolding = false;
        }
    }

    private void Jump(bool jump)
    {
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
    }

    private void FixedUpdate()
    {
        Animate();

        if(state.isHolding)
            armsRig.weight = 100f;
        else
            armsRig.weight = 0f;


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

    private void Animate()
    {
        if (state.isMoving)
        {
            state.Animator.Play("DrunkRun");
        }
        else
        {
            state.Animator.Play("DrunkIdle");
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