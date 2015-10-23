using UnityEngine;
using System.Collections.Generic;
using System;
using Magicolo;
using Rick;

public class PaperDoll : MonoBehaviour {

    [Disable]
    public List<PaperDollPart> parts;
	

	void Start () {
        parts = new List<PaperDollPart>();
        parts.AddArray(GetComponentsInChildren<PaperDollPart>());
	}
	
	
	void Update () {
        foreach (var part in parts)
        {
            part.Update();
        }
    }

    public void PlayAnimation(string animationKey)
    {
        foreach (var part in parts)
        {
            part.PlayAnimation(animationKey);
        }
    }
}
