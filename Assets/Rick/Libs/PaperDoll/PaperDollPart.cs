using UnityEngine;
using System.Collections;
using System;

public abstract class PaperDollPart : MonoBehaviour {

    Animator animator;

    
	public void Init () {
        animator = GetComponent<Animator>();	
	}

    abstract public void Update();

    internal void PlayAnimation(string animationKey)
    {        
        animator.SetTrigger(animationKey);
    }
}
