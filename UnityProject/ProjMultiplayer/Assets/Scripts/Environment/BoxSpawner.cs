using System.Collections;
using UnityEngine;
using Photon.Bolt;


public class BoxSpawner : GlobalEventListener
{
    [Header("If empty, will use the transform of this object")]
    [Tooltip("If empty, will use the transform of this object")]
    [SerializeField]
    Transform spawnPosition;

    [SerializeField]
    private bool playerCanInteract = false;

    bool playerInside = false;

    TagColors tagColor;// = TagColors.magenta;

    public bool PlayerCanInteract { get => playerCanInteract; }

    //public GameObject GetGameObject { get => gameObject; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInside = false;
        }
    }

    //private void Update()
    //{
    //    if (playerInside && Input.GetKeyDown(KeyCode.E))
    //    {
    //        if (spawnPosition == null)
    //            spawnPosition = transform;

    //        var evnt = SpawnBox.Create();
    //        evnt.Position = spawnPosition.position;
    //        evnt.TagColor = (int)TagColors.magenta;
    //        evnt.Send();
    //    }
    //}

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    public bool HasPlayerInside()
    {
        return playerInside;
    }

    public Vector3 GetSpawnPosition()
    {
        if (spawnPosition == null)
            spawnPosition = transform;

        return spawnPosition.position;
    }

    //public TagColors GetTagColor()
    //{
    //    //if(ServerGameManager.Instance.HasNextColor)
    //    //{
    //    //    return ServerGameManager.Instance.NextColor();
    //    //}
    //    //else
    //    return (TagColors)Random.Range(TagRange.firstIndex, TagRange.lastIndex+1);
    //}
}
