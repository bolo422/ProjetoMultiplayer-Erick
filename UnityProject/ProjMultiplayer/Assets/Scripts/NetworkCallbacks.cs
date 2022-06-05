using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class NetworkCallbacks : GlobalEventListener
{
    public override void SceneLoadLocalDone(string scene, IProtocolToken token)
    {
        if (BoltNetwork.IsClient)
        {
            var evnt = PlayerJoinedEvent.Create();
            evnt.Message = "Hello there!";
            evnt.Send();
        }


        //BoltEntity ent = BoltNetwork.Instantiate(BoltPrefabs.Player, spawnPos, Quaternion.identity);
        if (BoltNetwork.IsServer)
        {
            BoltNetwork.Instantiate(BoltPrefabs.MovableCube, new Vector3(0, 3, 0), Quaternion.identity);
            Vector3 spawnPos = new Vector3(Random.Range(-3, 3), 2, Random.Range(-3, 3));
            while (spawnPos.x == 0 || spawnPos.y == 0)
            {
                spawnPos = new Vector3(Random.Range(-3, 3), 2, Random.Range(-3, 3));
            }

            BoltNetwork.Instantiate(BoltPrefabs.Player, spawnPos, Quaternion.identity);
        }
    }

    public override void OnEvent(PlayerJoinedEvent evnt)
    {
        Debug.LogWarning(evnt.Message);
        Vector3 spawnPos = new Vector3(Random.Range(-3, 3), 2, Random.Range(-3, 3));
        while (spawnPos.x == 0 || spawnPos.y == 0)
        {
            spawnPos = new Vector3(Random.Range(-3, 3), 2, Random.Range(-3, 3));
        }

        BoltEntity ent = BoltNetwork.Instantiate(BoltPrefabs.Player, spawnPos, Quaternion.identity);
        ent.AssignControl(evnt.RaisedBy);
    }


}
