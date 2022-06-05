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
        }
    }

    
    public override void OnEvent(SpawnPlayerEvent evnt)
    {
        BoltEntity entity = BoltNetwork.Instantiate(BoltPrefabs.Player, new Vector3(0, 1, 0), Quaternion.identity);
        entity.AssignControl(evnt.RaisedBy);
    }

    public void SpawnPlayer()
    {
        SpawnPlayerEvent evnt = SpawnPlayerEvent.Create(GlobalTargets.OnlyServer);
        evnt.Send();
    }

}
