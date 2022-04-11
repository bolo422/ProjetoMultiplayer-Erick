using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class PlayerHealth : EntityBehaviour<IPlayerState>
{
    int localHealth = 3;

    //Start()
    public override void Attached()
    {
        state.Health = localHealth;

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
        localHealth = state.Health;

        if (localHealth <= 0)
        {
            BoltNetwork.Destroy(gameObject);
        }
    }
}
