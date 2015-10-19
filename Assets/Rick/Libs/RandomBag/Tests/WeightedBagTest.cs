using UnityEngine;
using System.Collections.Generic;
using Rick.RandomBags;

public class WeightedBagTest : MonoBehaviour {

	public bool test;
		
	void OnDrawGizmos () {
		if(test){
			test = false;
			testBase();
		}
	}

	void testBase() {
		WeightedBag<int> bag = new WeightedBag<int>(new System.Random());
		bag.add(25,1);
		bag.add(25,2);
		bag.add(50,3);
		Dictionary<int,int> valueRecieve = new Dictionary<int, int>();
		valueRecieve.Add(1,0);
		valueRecieve.Add(2,0);
		valueRecieve.Add(3,0);
		for (int i = 0; i < 1000; i++) {
			int value = bag.next();
			int nbTime = valueRecieve[value] + 1 ;
			valueRecieve.Remove(value);
			valueRecieve.Add(value, nbTime);
		}
		
		Debug.Log("Test base");
		foreach (var keyvalue in valueRecieve) {
			Debug.Log(keyvalue.Key + " : " + keyvalue.Value);
		}
		
	}
}