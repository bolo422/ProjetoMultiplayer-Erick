using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class PlayerJoined : EntityBehaviour<IPlayerState>
{
    [SerializeField]
    Camera camera;

    public override void Attached()
    {

        if (entity.HasControl)
        {
            camera.gameObject.SetActive(true);
        }
    }

    //private void Update()
    //{
    //    if (entity.IsOwner && camera.gameObject.activeInHierarchy == false)
    //    {
    //        camera.gameObject.SetActive(true);
    //    }
    //}

}
