using System;
using UdpKit;
using UnityEngine;
using Photon.Bolt;
using Photon.Bolt.Matchmaking;

public class NetworkManager : GlobalEventListener
{
    [SerializeField]
    private UnityEngine.UI.Text feedback;

    [SerializeField]
    private UnityEngine.UI.Text roomNameInputText;

    [SerializeField]
    private GameObject RoomNameInput;

    public void FeedbackUser(string text)
    {
        feedback.text = text;
    }

    public void Connect()
    {
        //RoomNameInput.SetActive(false);
        //feedback.GetComponent<GameObject>().SetActive(true);

        FeedbackUser("Connnecting ...");
        BoltLauncher.StartClient();
    }

    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
    {
        FeedbackUser("Searching ...");
        if(roomNameInputText != null)
            BoltMatchmaking.JoinSession(roomNameInputText.text);
        else
            BoltMatchmaking.JoinSession(HeadlessServerManager.RoomID());
    }

    public override void Connected(BoltConnection connection)
    {
        FeedbackUser("Connected !");
    }
}