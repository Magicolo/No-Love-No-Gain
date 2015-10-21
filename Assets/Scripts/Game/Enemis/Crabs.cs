using UnityEngine;
using System.Collections;
using Magicolo;

public class Crabs : DamageableBase {

    [Disable]
    public CrabPince[] pinces;

    public enum CrabState {IDLE,CHARGING_ATTACK,ATTACKING};
    [Disable] public CrabState crabState;

    public override void Die()
    {
        this.Destroy();
    }

    internal override void TakeDamage()
    {
        //throw new NotImplementedException();
    }

    void Start ()
    {
        pinces = GetComponentsInChildren<CrabPince>();
        
    }

    
}