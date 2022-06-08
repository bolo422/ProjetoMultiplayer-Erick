using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveHead : MonoBehaviour
{
    [SerializeField]
    private Transform head;

    private void FixedUpdate()
    {
        head.rotation = transform.rotation;

    }
}
