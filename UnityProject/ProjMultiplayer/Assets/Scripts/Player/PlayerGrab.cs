using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using UnityEngine.Animations.Rigging;

public class PlayerGrab : EntityBehaviour<IPhysicState>
{
    [SerializeField]
    private Transform leftHandTarget;
    [SerializeField]
    private Transform rightHandTarget;
    [SerializeField]
    private Rig armsRig;

    private bool isHolding = false;

    public override void Attached()
    {
        state.SetTransforms(state.LeftHand, leftHandTarget);
        state.SetTransforms(state.RightHand, rightHandTarget);
    }

    public override void SimulateOwner()
    {
        base.SimulateOwner();
    }

}
