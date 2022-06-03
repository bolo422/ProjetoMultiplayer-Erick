using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class PlayerHealth : EntityBehaviour<IPlayerState>
{
    private int _localHealth = 3;

    //Start()
    public override void Attached()
    {
        state.Health = _localHealth;

        state.AddCallback("Health", HealthCallback);
    }

    public void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    state.Health -= 1;
        //}
    }

    private void HealthCallback()
    {
        _localHealth = state.Health;

        if (_localHealth <= 0)
        {
            BoltNetwork.Destroy(gameObject);
        }
    }
}
