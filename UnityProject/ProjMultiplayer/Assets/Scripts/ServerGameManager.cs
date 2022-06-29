using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using UnityEngine.SceneManagement;

public class ServerGameManager : GlobalEventListener
{
    public static ServerGameManager Instance { get; private set; }
    public bool CanFinishLevel { get => canFinishLevel; set => canFinishLevel = value; }
    public int Objective { get => objective; set => objective = value; }

    private int objective;

    private HashSet<PickableItem> redObjectives = new HashSet<PickableItem>();
    private HashSet<PickableItem> greenObjectives = new HashSet<PickableItem>();
    private HashSet<PickableItem> blueObjectives = new HashSet<PickableItem>();
    private HashSet<PickableItem> magentaObjectives = new HashSet<PickableItem>();
    private HashSet<PickableItem> yellowObjectives = new HashSet<PickableItem>();

    bool canFinishLevel = false;
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

    private void Start()
    {
        //colors = new List<TagColors>();
        //foreach (Banner b in FindObjectsOfType<Banner>())
        //{
        //    colors.Add(b.CurrentColor);
        //}

        //objective = 0;


        //foreach (var item in FindObjectsOfType<BoxSpawner>())
        //{
        //    BoltEntity box = BoltNetwork.Instantiate(BoltPrefabs.Box, item.GetSpawnPosition(), Quaternion.identity);
        //    box.GetComponent<Box>().TagColor = colors[Random.Range(0, colors.Count)];
        //    objective++;
        //}

        //if(SceneManager.GetActiveScene().name == "wh_1")
        //{

       // }

        //BoltEntity box = BoltNetwork.Instantiate(BoltPrefabs.Box, boxSpawnPosition, Quaternion.identity);
        //box.GetComponent<Box>().TagColor = (TagColors)boxSpawnTag;
        //_interactCooldown = false;
        //StartCoroutine(IntereactCooldown());
    }


    void Update()
    {
        //if(yaWinningSon)
        //{
        //    if (subindo)
        //    {
        //        transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        //        if (transform.position.y > 4.0f)
        //        {
        //            subindo = false;
        //        }
        //    }
        //    else
        //    {
        //        transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
        //        if (transform.position.y < 1.0f)
        //        {
        //            subindo = true;
        //        }
        //    }
        //}
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        redObjectives = new HashSet<PickableItem>();
        greenObjectives = new HashSet<PickableItem>();
        blueObjectives = new HashSet<PickableItem>();
        magentaObjectives = new HashSet<PickableItem>();
        yellowObjectives = new HashSet<PickableItem>();

    }

    private void CheckForObjective()
    {
        int totalCount = redObjectives.Count + greenObjectives.Count + blueObjectives.Count + magentaObjectives.Count + yellowObjectives.Count;
        if (totalCount >= objective)
        {
            canFinishLevel = true;
        }
        else
        {
            canFinishLevel = false;
        }
        Debug.Log("objective: " + objective);
        Debug.Log("Result: " + CalculateResult());

        var evnt = canFinishLevelEvent.Create();
        evnt.canFinish = canFinishLevel;
        evnt.total = objective;
        evnt.achived = totalCount;
        evnt.result = CalculateResult();
        evnt.Send();
    }

    private float CalculateResult()
    {
        //objective 100
        //correct x
        int correct = 0;

        foreach(PickableItem p in redObjectives)
        {if (p.GetComponent<Box>().TagColor == TagColors.red) correct++;}

        foreach (PickableItem p in greenObjectives)
        { if (p.GetComponent<Box>().TagColor == TagColors.green) correct++; }

        foreach (PickableItem p in blueObjectives)
        { if (p.GetComponent<Box>().TagColor == TagColors.blue) correct++; }

        foreach (PickableItem p in magentaObjectives)
        { if (p.GetComponent<Box>().TagColor == TagColors.magenta) correct++;}

        foreach (PickableItem p in yellowObjectives)
        { if (p.GetComponent<Box>().TagColor == TagColors.yellow) correct++; }

        Debug.Log("Correct: " + correct);
        return correct * 100 / objective;
    }

    public void AddToObjectiveHashSet(PickableItem objective, TagColors color)
    {

        //var evnt = canFinishLevelEvent.Create();
        //evnt.canFinish = true;
        //evnt.Send();

        //switch(objective.GetComponent<Box>().TagColor)
        switch (color)
        {
            case TagColors.red: redObjectives.Add(objective); break;
            case TagColors.green: greenObjectives.Add(objective); break;
            case TagColors.blue: blueObjectives.Add(objective); break;
            case TagColors.magenta: magentaObjectives.Add(objective); break;
            case TagColors.yellow: yellowObjectives.Add(objective); break;
        }

        //objectives.Add(objective);

        CheckForObjective();
    }

    public void RemoveFromObjectiveHashSet(PickableItem objective, TagColors color)
    {
        //switch (objective.GetComponent<Box>().TagColor)
        switch (color)
        {
            case TagColors.red: redObjectives.          Remove(objective); break;
            case TagColors.green: greenObjectives.      Remove(objective); break;
            case TagColors.blue: blueObjectives.        Remove(objective); break;
            case TagColors.magenta: magentaObjectives.  Remove(objective); break;
            case TagColors.yellow: yellowObjectives.    Remove(objective); break;

        }
        CheckForObjective();
    }

    //public override void OnEvent(SpawnBox evnt)
    //{
    //    Box box = BoltNetwork.Instantiate(BoltPrefabs.Box, evnt.Position, Quaternion.identity).GetComponent<Box>();
    //    box.CurrentTagColor = (TagColors)evnt.TagColor;
    //}

}
