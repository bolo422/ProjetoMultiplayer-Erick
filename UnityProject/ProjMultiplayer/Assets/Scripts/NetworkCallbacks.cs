using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class NetworkCallbacks : GlobalEventListener
{
    [SerializeField]
    GameObject playerPrefab;

    public override void SceneLoadLocalDone(string scene, IProtocolToken token)
    {
        var spawnPos = new Vector3(Random.Range(-3, 3), 2, Random.Range(-3, 3));
        BoltNetwork.Instantiate(playerPrefab, spawnPos, Quaternion.identity);
    }

    public override void OnEvent(PlayerJoinedEvent evnt)
    {
        Debug.LogWarning(evnt.Message);
    }
}
