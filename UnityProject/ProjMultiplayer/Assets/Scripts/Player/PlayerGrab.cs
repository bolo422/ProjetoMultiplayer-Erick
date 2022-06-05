using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class PlayerGrab : EntityBehaviour<IPhysicState>
{
    [SerializeField]
    Transform holdItem;

    private GameObject cube;
    private bool isHolding = false;

    public override void Attached()
    {
        cube = GameObject.FindGameObjectWithTag("movableCube");
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.K))
        {
            isHolding = !isHolding;
            cube.GetComponent<MovableCube>().Hold(holdItem, isHolding);
        }
    }

}
