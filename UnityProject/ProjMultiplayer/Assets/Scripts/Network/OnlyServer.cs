using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class OnlyServer : MonoBehaviour
{
    void Awake()
    {
        if (!BoltNetwork.IsServer)
        {
            Destroy(gameObject);
            return;
        }
    }
}
