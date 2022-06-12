using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using UnityEngine.SceneManagement;

public class ServerGameManager : MonoBehaviour
{
    public static ServerGameManager Instance { get; private set; }

    private int objective = 3;

    private HashSet<PickableItem> objectives = new HashSet<PickableItem>();

    bool yaWinningSon = false;
    bool subindo = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if(yaWinningSon)
        {
            if (subindo)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                if (transform.position.y > 4.0f)
                {
                    subindo = false;
                }
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
                if (transform.position.y < 1.0f)
                {
                    subindo = true;
                }
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }

    private void CheckForObjective()
    {
        if(objectives.Count >= objective)
            yaWinningSon=true;
        else
            yaWinningSon=false;
    }

    public void AddToObjectiveHashSet(PickableItem objective)
    {
        objectives.Add(objective);
        CheckForObjective();
    }

    public void RemoveFromObjectiveHashSet(PickableItem objective)
    {
        objectives.Remove(objective);
        CheckForObjective();
    }

}
