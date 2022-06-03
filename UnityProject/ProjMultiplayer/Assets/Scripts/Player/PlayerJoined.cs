using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class PlayerJoined : EntityBehaviour<IPlayerState>
{
    [SerializeField]
    private Camera _camera;

    public override void Attached()
    {
        var evnt = PlayerJoinedEvent.Create();
        evnt.Message = "Hello there!";
        evnt.Send();

        if (entity.IsOwner)
        {
            _camera.gameObject.SetActive(true);
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
