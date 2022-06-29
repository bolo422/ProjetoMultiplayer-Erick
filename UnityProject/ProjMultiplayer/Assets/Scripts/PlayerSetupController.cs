using UnityEngine;
using Photon.Bolt;

public class PlayerSetupController : GlobalEventListener
{

    [SerializeField]
    private Camera _sceneCamera;

    [SerializeField]
    private GameObject _setupPanel;

    public Camera SceneCamera { get => _sceneCamera; }

    public override void SceneLoadLocalDone(string scene, IProtocolToken token)
    {
        if (!BoltNetwork.IsServer)
        {
            _setupPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            //BoltNetwork.Instantiate(BoltPrefabs.MovableCube, new Vector3(1, 5, 0), Quaternion.identity);
            //BoltNetwork.Instantiate(BoltPrefabs.MovableCube, new Vector3(2, 5, 1), Quaternion.identity);
            //BoltNetwork.Instantiate(BoltPrefabs.MovableCube, new Vector3(1, 5, 4), Quaternion.identity);
        }
    }

    
    public override void OnEvent(SpawnPlayerEvent evnt)
    {
        Vector3 spawnPos = new Vector3(0, 1, 0);

        if(PlayerSpawner.Instance != null)
        {
            spawnPos = PlayerSpawner.Instance.GetRandomSpawnPoint();
        }

        BoltEntity entity = BoltNetwork.Instantiate(BoltPrefabs.Player2, spawnPos, Quaternion.identity);
        entity.AssignControl(evnt.RaisedBy);
        Debug.Log("Spawning player");
    }

    public void SpawnPlayer()
    {
        Debug.Log(" ****-*--*-*-*-* TESTING *-********-*-");
        SpawnPlayerEvent evnt = SpawnPlayerEvent.Create(GlobalTargets.OnlyServer);
        evnt.Send();
        Debug.Log("Sending SpawnPlayerEvent event");
    }

}
