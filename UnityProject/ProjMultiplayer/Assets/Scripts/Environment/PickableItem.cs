using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class PickableItem : EntityBehaviour<IPickableItem>
{
    private Transform reference;
    private bool beignHeld = false;
    private Rigidbody rb;

    //private GameObject objectiveTrigger;

    //private bool insideObjective = false;

    public override void Attached()
    {
        //GameObject[] children = GetComponentsInChildren<GameObject>();

        //foreach(GameObject child in children)
        //{
        //    if (child.tag == "Objective")
        //        objectiveTrigger = child;
        //}

        state.SetTransforms(state.Transform, transform);
        rb = GetComponent<Rigidbody>();

        if (entity.IsOwner)
        {
            state.UseGravity = rb.useGravity;
            //state.InsideTriggerObjective = insideObjective;
        }

        state.AddCallback("UseGravity", UseGravityChanged);
        //state.AddCallback("InsideTriggerObjective", InsideTriggerObjectiveChanged);
    }

    public override void SimulateOwner()
    {
        if (beignHeld && reference != null)
        {
            transform.position = reference.position;
            transform.rotation = reference.rotation;
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

            if (entity.IsOwner)
            { 
                rb.useGravity = true;
                state.UseGravity = rb.useGravity;
            }

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

            if (entity.IsOwner)
            {
                rb.useGravity = false;
                state.UseGravity = rb.useGravity;
            }

            return;
            
        }

    }

    public void InsideTriggerObjectiveChanged()
    {
        //insideObjective = state.InsideTriggerObjective;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!entity.IsOwner)
            return;

        ServerGameManager.Instance.AddToObjectiveHashSet(this);

        //insideObjective = true;
        //state.InsideTriggerObjective = insideObjective;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!entity.IsOwner)
            return;

        ServerGameManager.Instance.RemoveFromObjectiveHashSet(this);
        //insideObjective = false;
        //state.InsideTriggerObjective = insideObjective;
    }


}
