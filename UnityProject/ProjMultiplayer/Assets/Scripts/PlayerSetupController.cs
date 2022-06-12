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
        }
        else
        {
            BoltNetwork.Instantiate(BoltPrefabs.MovableCube, new Vector3(1, 5, 0), Quaternion.identity);
            BoltNetwork.Instantiate(BoltPrefabs.MovableCube, new Vector3(2, 5, 1), Quaternion.identity);
            BoltNetwork.Instantiate(BoltPrefabs.MovableCube, new Vector3(1, 5, 4), Quaternion.identity);
        }
    }

    
    public override void OnEvent(SpawnPlayerEvent evnt)
    {
        BoltEntity entity = BoltNetwork.Instantiate(BoltPrefabs.Player2, new Vector3(0, 1, 0), Quaternion.identity);
        entity.AssignControl(evnt.RaisedBy);
        Debug.LogError("Spawning player");
    }

    public void SpawnPlayer()
    {
        Debug.LogError(" ****-*--*-*-*-* TESTING *-********-*-");
        SpawnPlayerEvent evnt = SpawnPlayerEvent.Create(GlobalTargets.OnlyServer);
        evnt.Send();
        Debug.LogError("Sending SpawnPlayerEvent event");
    }

}
