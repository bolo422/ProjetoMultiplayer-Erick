using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    List<Vector3> spawnPositions;

    public static PlayerSpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }


        spawnPositions = new List<Vector3>();
        foreach(var p in GameObject.FindGameObjectsWithTag("playerSpawnPosition"))
        { 
            spawnPositions.Add(p.transform.position);
        }

    }

    public Vector3 GetRandomSpawnPoint()
    {
        return spawnPositions[Random.Range(0, spawnPositions.Count)];
    }
}
