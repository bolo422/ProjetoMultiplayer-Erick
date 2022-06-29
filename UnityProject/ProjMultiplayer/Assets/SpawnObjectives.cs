using Photon.Bolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectives : GlobalEventListener
{
    List<TagColors> colors;

    void Start()
    {
        colors = new List<TagColors>();
        foreach (Banner b in FindObjectsOfType<Banner>())
        {
            colors.Add(b.CurrentColor);
        }

        ServerGameManager.Instance.Objective = 0;


        foreach (var item in FindObjectsOfType<BoxSpawner>())
        {
            BoltEntity box = BoltNetwork.Instantiate(BoltPrefabs.Box, item.GetSpawnPosition(), Quaternion.identity);
            box.GetComponent<Box>().TagColor = colors[Random.Range(0, colors.Count)];
            ServerGameManager.Instance.Objective++;
        }
    }

}
