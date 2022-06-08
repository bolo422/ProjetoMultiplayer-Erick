using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class CameraMoveHead : EntityBehaviour<IPhysicState>
{
    [SerializeField]
    private Transform headAim;
    [SerializeField]
    private Transform headAimSlot;

    public override void Attached()
    {
        state.SetTransforms(state.HeadAim, headAim);
    }

    public override void SimulateOwner()
    {
        headAim.position = headAimSlot.position;
        headAim.rotation = headAimSlot.rotation;
    }
}
