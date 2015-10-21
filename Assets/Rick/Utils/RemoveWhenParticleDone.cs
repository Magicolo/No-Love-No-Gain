using UnityEngine;
using System.Collections;
using Magicolo;

public class RemoveWhenParticleDone : MonoBehaviour {

	ParticleSystem emitter;
	// Use this for initialization
	void Awake () {
		emitter = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if(emitter.isStopped){
			gameObject.Remove();
		}
	}
}
