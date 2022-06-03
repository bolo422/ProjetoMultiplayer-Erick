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
        var spawnPos = new Vector3(Random.Range(-3, 3), 2, Random.Range(-3, 3));
        GameObject playerPrefab = Resources.Load<GameObject>("Player");
        BoltEntity ent = BoltNetwork.Instantiate(playerPrefab, spawnPos, Quaternion.identity);        

        if(BoltNetwork.IsServer)
        {
            GameObject cube = Resources.Load<GameObject>("MovableCube2");
            props.Add(BoltNetwork.Instantiate(cube, new Vector3(0,5,0), Quaternion.identity));
        }

        

    }

    public override void OnEvent(PlayerJoinedEvent evnt)
    {
        Debug.LogWarning(evnt.Message);
    }
}
