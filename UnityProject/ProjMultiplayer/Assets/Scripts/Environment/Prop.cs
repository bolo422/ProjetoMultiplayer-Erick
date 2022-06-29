using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class Prop : EntityBehaviour<IProp>
{
    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);
    }
}
