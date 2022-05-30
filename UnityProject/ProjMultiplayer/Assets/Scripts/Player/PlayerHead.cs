using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class PlayerHead : EntityBehaviour<IPlayerState>
{
    //Start()
    public override void Attached()
    {
        state.SetTransforms(state.HeadTransform, transform);
    }
}
