using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class NetworkCallbacks : GlobalEventListener
{
    List<BoltEntity> props = new List<BoltEntity>();
    int mtimes = 0;

    public override void SceneLoadLocalDone(string scene, IProtocolToken token)
    {


        

    }

    public override void OnEvent(PlayerJoinedEvent evnt)
    {
        Debug.LogWarning(evnt.Message);
    }
}
