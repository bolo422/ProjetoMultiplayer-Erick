using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class MovableCube : EntityBehaviour<IMovableCube>
{
    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);
    }
}
