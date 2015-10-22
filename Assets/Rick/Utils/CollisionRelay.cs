using UnityEngine;
using System.Collections;

public class CollisionRelay : MonoBehaviour {

	public GameObject gameObjectToRelayTo;
	public bool sendOnCollisionEnter2D;
	public bool sendOnCollisionExit2D;
	public bool sendOnCollisionStay2D;
	
	
	void OnCollisionEnter2D(Collision2D collision) {
		if(sendOnCollisionEnter2D){	
			gameObjectToRelayTo.SendMessage("OnCollisionEnter2D",collision);
		}
		
	}
	
	void OnCollisionStay2D(Collision2D collision) {
		if(sendOnCollisionStay2D){
			gameObjectToRelayTo.SendMessage("OnCollisionStay2D",collision);
		}
	}
	
	void OnCollisionExit2D(Collision2D collision) {
		if(sendOnCollisionExit2D){
			gameObjectToRelayTo.SendMessage("OnCollisionExit2D",collision);
		}
	}
}
