using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class MovableCube : EntityBehaviour<IMovableCube>
{
    private Transform reference;
    private bool holding = false;

    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);
    }

    public override void SimulateOwner()
    {
        if(holding && reference != null)
            transform.position = reference.position;
    }

    public void Hold (Transform _reference, bool _holding)
    {
        reference = _reference;
        holding = _holding;
    }


}
