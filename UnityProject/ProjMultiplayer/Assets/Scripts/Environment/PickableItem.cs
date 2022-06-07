using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class PickableItem : EntityBehaviour<IPickableItem>
{
    private Transform reference;
    private bool beignHeld = false;
    private Rigidbody rb;

    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);
        rb = GetComponent<Rigidbody>();

        if (entity.IsOwner)
            state.UseGravity = rb.useGravity;

        state.AddCallback("UseGravity", UseGravityChanged);
    }

    public override void SimulateOwner()
    {
        if (beignHeld && reference != null)
        {
            transform.position = reference.position;
            gameObject.layer = LayerMask.NameToLayer("DontCollideWithPlayers");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    void UseGravityChanged()
    {
        rb.useGravity = state.UseGravity;
    }

    public void Hold (Transform _reference, bool wantToHoldMe)
    {
        bool carryingMe = this.beignHeld && wantToHoldMe && this.reference == _reference;
        if (carryingMe)
            return;
        
        bool wantToReleaseMe = this.beignHeld && !wantToHoldMe && this.reference == _reference;
        if (wantToReleaseMe)
        {
            this.beignHeld = false;
            this.reference = null;
            rb.useGravity = true;
            state.UseGravity = rb.useGravity;
            return;
        }

        bool beignHeldBySomeoneElse = beignHeld && this.reference != null && this.reference != _reference;
        if (beignHeldBySomeoneElse)
            return;

        bool pickedUp = !beignHeld && wantToHoldMe;
        if (pickedUp)
        {
            this.reference = _reference;
            beignHeld = wantToHoldMe;
            rb.useGravity = false;
            state.UseGravity = rb.useGravity;
            return;
            
        }

    }


}
